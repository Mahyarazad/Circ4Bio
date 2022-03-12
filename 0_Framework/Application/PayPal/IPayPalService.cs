using System.Threading.Tasks;
using PaypalPayment;

namespace _0_Framework.Application.PayPal
{
    public interface IPayPalService
    {
        public Task<string> GetRedirectUrltoPayPal(double totalAmount, string currency, string trackingCode);
        public Task<PayPalPaymentExecutedResponse> ExecutedPayment(string PaymentId, string PayerId);
    }
}