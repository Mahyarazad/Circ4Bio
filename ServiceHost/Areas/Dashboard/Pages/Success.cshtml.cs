using System.Threading.Tasks;
using _0_Framework.Application.PayPal;
using AM.Application.Contracts.Deal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using PayPalPaymentExecutedResponse = _0_Framework.Application.PayPal.PayPalPaymentExecutedResponse;

namespace ServiceHost.Areas.Dashboard.Pages
{
    public class Success : PageModel
    {
        private readonly IPayPalService _payPalService;
        private readonly IDealApplication _dealApplication;
        public PayPalPaymentExecutedResponse Command;
        public JToken RelatedResource;


        public Success(IPayPalService payPalService, IDealApplication dealApplication)
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
                var dealViewModeltobeUpdated =
                    _dealApplication.ReturnDealIdWithTrackingRef(Command.transactions[0].custom);
                dealViewModeltobeUpdated.PaymentId = Command.id;
                dealViewModeltobeUpdated.PaymentTime = Command.create_time;
                dealViewModeltobeUpdated.PayerEmail = Command.payer.payer_info.email;
                dealViewModeltobeUpdated.PayerFirstName = Command.payer.payer_info.first_name;
                dealViewModeltobeUpdated.PayerLastName = Command.payer.payer_info.last_name;
                var result = _dealApplication.FinishDeal(dealViewModeltobeUpdated);


            }
            else
            {
                Command = new PayPalPaymentExecutedResponse();
                RelatedResource = new JObject();
            }


        }

    }
}