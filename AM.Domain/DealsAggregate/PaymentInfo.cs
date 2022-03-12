using System;

namespace AM.Domain
{
    public class PaymentInfo
    {
        protected PaymentInfo() { }

        public PaymentInfo(string? paymentId, DateTime paymentTime
            , string? payerEmail, string? payerFirstName, string? payerLastName)
        {
            PaymentId = paymentId;
            PaymentTime = paymentTime;
            PayerEmail = payerEmail;
            PayerFirstName = payerFirstName;
            PayerLastName = payerLastName;
        }


        public string? PaymentId { get; private set; }
        public DateTime PaymentTime { get; private set; }
        public string? PayerEmail { get; private set; }
        public string? PayerFirstName { get; private set; }
        public string? PayerLastName { get; private set; }
    }
}