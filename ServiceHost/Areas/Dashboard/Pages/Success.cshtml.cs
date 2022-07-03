using System.Threading.Tasks;
using _0_Framework;
using _0_Framework.Application;
using _0_Framework.Application.Email;
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
        private readonly IAuthenticateHelper _authenticateHelper;
        private readonly IEmailService<EmailModel> _emailService;
        public PayPalPaymentExecutedResponse Command;
        public JToken RelatedResource;


        public Success(IPayPalService payPalService, IDealApplication dealApplication, IAuthenticateHelper authenticateHelper, IEmailService<EmailModel> emailService)
        {
            _payPalService = payPalService;
            _dealApplication = dealApplication;
            _authenticateHelper = authenticateHelper;
            _emailService = emailService;
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
                dealViewModeltobeUpdated.PaidAmount = (double)RelatedResource["amount"]["total"];
                dealViewModeltobeUpdated.TransactionFee = (double)RelatedResource["transaction_fee"]["value"];
                var result = _dealApplication.FinishDeal(dealViewModeltobeUpdated);

                _emailService.SendEmail(new EmailModel
                {
                    EmailTemplate = EmailType.QuotationCreated,
                    Title = ApplicationMessage.PaymentDone,
                    Recipient = _authenticateHelper.CurrentAccountRole().Email,
                    Body = ApplicationMessage.PaymentDone,
                    Body1 = $"Your payment ID: {dealViewModeltobeUpdated.PaymentId} @ {dealViewModeltobeUpdated.PaymentTime}",
                    Body2 = $"The transaction fee is {dealViewModeltobeUpdated.TransactionFee}",
                    Body3 = $"The total payment amount is {dealViewModeltobeUpdated.PaidAmount}",
                });
            }
            else
            {
                Command = new PayPalPaymentExecutedResponse();
                RelatedResource = new JObject();
            }
        }
    }
}