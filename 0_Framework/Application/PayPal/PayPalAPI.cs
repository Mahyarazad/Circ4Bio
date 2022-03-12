using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PaypalPayment;

namespace _0_Framework.Application.PayPal
{
    public class PayPalAPI : IPayPalService
    {
        private readonly IConfiguration _configuration;
        public PayPalAPI(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GetRedirectUrltoPayPal(double totalAmount, string currency, string trackingCode)
        {
            try
            {
                return Task.Run(async () =>
                {
                    HttpClient http = GetPayPalHttpClient();
                    PayPalAccessToken accessToken = await GetPayPalAccessTokenAsync(http);
                    PayPalPaymentCreatedResponse createdPayment =
                        await CreatePayPalPaymentAsync(http, accessToken, totalAmount, currency, trackingCode);
                    return createdPayment.Links.FirstOrDefault(x => x.rel == "approval_url").href;
                }).Result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, "Failed to login to PayPal");
                return null;
            }
        }

        public async Task<PayPalPaymentExecutedResponse> ExecutedPayment(string PaymentId, string PayerId)
        {
            try
            {
                var http = GetPayPalHttpClient();
                var accessToken = await GetPayPalAccessTokenAsync(http);
                return await ExecutePayPalPaymentAsync(http, accessToken, PaymentId, PayerId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, "Failed to login to PayPal");
                return null;
            }
        }

        private HttpClient GetPayPalHttpClient()
        {
            return new HttpClient
            {
                BaseAddress = new Uri(_configuration.GetSection("PayPal")["urlAPI"]),
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        private async Task<PayPalAccessToken> GetPayPalAccessTokenAsync(HttpClient Http)
        {
            var bytes = Encoding
                 .GetEncoding("iso-8859-1")
                 .GetBytes(
                     $"{_configuration.GetSection("PayPal")["clientId"]}:{_configuration.GetSection("PayPal")["secret"]}");
            var request = new HttpRequestMessage(HttpMethod.Post, "v1/oauth2/token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(bytes));

            var form = new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials"
            };
            request.Content = new FormUrlEncodedContent(form);
            var response = await Http.SendAsync(request);

            string content = await response.Content.ReadAsStringAsync();
            PayPalAccessToken? accessToken = JsonConvert.DeserializeObject<PayPalAccessToken>(content);

            return accessToken;

        }

        private async Task<PayPalPaymentCreatedResponse> CreatePayPalPaymentAsync(HttpClient Http,
            PayPalAccessToken accessToken, double totalAmount, string currency, string TrackingCode)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "v1/payments/payment");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.access_token);

            var payment = JObject.FromObject(new
            {
                intent = "sale",
                redirect_urls = new
                {
                    return_url = _configuration.GetSection("PayPal")["returnUrl"],
                    cancel_url = _configuration.GetSection("PayPal")["cancelUrl"]
                },
                payer = new { payment_method = "paypal" },
                transactions = JArray.FromObject(new[]
                {
                    new
                    {
                        amount = new
                        {
                            total = totalAmount,
                            currency = currency,

                        },
                        custom = TrackingCode
                    }
                })

            });

            request.Content = new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, "application/json");
            var response = await Http.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();
            PayPalPaymentCreatedResponse? payPalPaymentCreatedResponse
                = JsonConvert.DeserializeObject<PayPalPaymentCreatedResponse>(content);

            return payPalPaymentCreatedResponse;
        }

        private async Task<PayPalPaymentExecutedResponse> ExecutePayPalPaymentAsync(HttpClient Http,
            PayPalAccessToken accessToken, string paymentId, string payerId)
        {

            var request = new HttpRequestMessage(HttpMethod.Post, $"v1/payments/payment/{paymentId}/execute");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.access_token);

            var payment = JObject.FromObject(new
            {
                payer_id = payerId
            });

            request.Content = new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, "application/json");
            var response = await Http.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);
            PayPalPaymentExecutedResponse? executedResponse
                = JsonConvert.DeserializeObject<PayPalPaymentExecutedResponse>(content);

            return executedResponse;
        }
    }
}