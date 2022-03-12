using System.Threading.Tasks;
using _0_Framework.Application.PayPal;
using AM.Application.Contracts.Deal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using PayPalPaymentExecutedResponse = _0_Framework.Application.PayPal.PayPalPaymentExecutedResponse;

namespace ServiceHost.Areas.Dashboard.Pages
{
    public class Canceled : PageModel
    {
        private readonly IPayPalService _payPalService;
        private readonly IDealApplication _dealApplication;
        public PayPalPaymentExecutedResponse Command;
        public JToken RelatedResource;


        public Canceled(IPayPalService payPalService, IDealApplication dealApplication)
        {
            _payPalService = payPalService;
            _dealApplication = dealApplication;
        }

        public async Task OnGet([FromQuery(Name = "paymentId")] string paymentId,
            [FromQuery(Name = "payerId")] string payerId)
        {

            Command = _payPalService.ExecutedPayment(paymentId, payerId).Result;
            if (Command != null)
            {
                RelatedResource = JObject.FromObject(Command.transactions[0].related_resources[0]).First.First;
            }
            else
            {
                Command = new PayPalPaymentExecutedResponse();
                RelatedResource = new JObject();
            }


        }

    }
}