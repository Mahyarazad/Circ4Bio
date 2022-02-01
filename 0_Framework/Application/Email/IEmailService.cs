namespace _0_Framework.Application.Email
{
    public interface IEmailService<T> where T : class
    {
        OperationResult SendEmail(T entity);
    }

    public class EmailModel
    {
        public string? Subject { get; set; }
        public string? Fullname { get; set; }
        public string? Email { get; set; }
        public string? Body { get; set; }
        public string? Body1 { get; set; }
        public string? Body2 { get; set; }
        public string? Body3 { get; set; }
        public string? Body4 { get; set; }
        public string? Phone { get; set; }
        public string? Title { get; set; }
        public string? Recipient { get; set; }
        public int EmailTemplate { get; set; }
        public string? ResetPasswordLink { get; set; }
        public string? AccountVerificationLink { get; set; }
        public string? StatusChangeMessage { get; set; }
        public string? ListingName { get; set; }
        public string? ImageString { get; set; }
        public string? BuyyerImageString { get; set; }
        public string? SellerName { get; set; }
        public string? BuyyerName { get; set; }
        public string? SellerEmail { get; set; }
        public string? BuyyerEmail { get; set; }
        public string? ItemType { get; set; }
        public string? SellerImageString { get; set; }
        public string? DeliveryMethod { get; set; }
        public string? Unit { get; set; }
        public double UnitPrice { get; set; }
        public double Amount { get; set; }
        public string? Currency { get; set; }
    }
}