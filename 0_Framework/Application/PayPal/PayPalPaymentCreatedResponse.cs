using System;

namespace _0_Framework.Application.PayPal
{
    public class PayPalPaymentCreatedResponse
    {
        public string Id { get; set; }
        public string Intent { get; set; }
        public string State { get; set; }
        public string tracking_code { get; set; }
        public PaypalPayment.PayPalPaymentCreatedResponse.Payer Payer { get; set; }
        public PaypalPayment.PayPalPaymentCreatedResponse.Transaction[] Transactions { get; set; }
        public PaypalPayment.PayPalPaymentCreatedResponse.Link[] Links { get; set; }
        public DateTime CreationTime { get; set; }
    }
}