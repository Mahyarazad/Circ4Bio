using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PayPal.Api;

namespace ServiceHost.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;
        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public void OnGet()
        {
            // var config = _configuration.GetSection("PayPal")["Account"];
            // var configKey = _configuration.GetSection("PayPal")["AccessToken"];
            // var accessToken = new OAuthTokenCredential(config, configKey).GetAccessToken();
            // var apiContext = new APIContext(accessToken);
            // var payment = Payment.Create(apiContext, new Payment
            // {
            //     intent = "sale",
            //     payer = new Payer
            //     {
            //         payment_method = "paypal"
            //     },
            //     transactions = new List<Transaction>
            //     {
            //         new Transaction
            //         {
            //             description = "Transaction description.",
            //             invoice_number = "001",
            //             amount = new Amount
            //             {
            //                 currency = "USD",
            //                 total = "100.00",
            //                 details = new Details
            //                 {
            //                     tax = "15",
            //                     shipping = "10",
            //                     subtotal = "75"
            //                 }
            //             },
            //             item_list = new ItemList
            //             {
            //                 items = new List<Item>
            //                 {
            //                     new Item
            //                     {
            //                         name = "Item Name",
            //                         currency = "USD",
            //                         price = "15",
            //                         quantity = "5",
            //                         sku = "sku"
            //                     }
            //                 }
            //             }
            //         }
            //     },
            //     redirect_urls = new RedirectUrls
            //     {
            //         return_url = "http://mysite.com/return",
            //         cancel_url = "http://mysite.com/cancel"
            //     }
            // });
        }
    }
}
