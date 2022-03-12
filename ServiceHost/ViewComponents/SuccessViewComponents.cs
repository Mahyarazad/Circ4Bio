using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _0_Framework.Application.PayPal;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.ViewComponents
{
    public class SuccessViewComponents : ViewComponent
    {
        private readonly IPayPalService _payPalService;

        public SuccessViewComponents(IPayPalService payPalService)
        {
            _payPalService = payPalService;
        }

        public async Task<IViewComponentResult> InvokeAsync([FromQuery(Name = "paymentId")] string paymentId,
            [FromQuery(Name = "payerId")] string payerId)
        {
            var result = await _payPalService.ExecutedPayment(paymentId, payerId);
            Console.WriteLine(result);
            return View("Success");
        }
    }
}
