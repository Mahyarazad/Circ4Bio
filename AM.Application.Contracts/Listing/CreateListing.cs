namespace AM.Application.Contracts.Listing
{
    public class CreateListing
    {
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? DeliveryMethod { get; set; }
        // It can be kg, lit, ton, peice and etc.
        public string? Unit { get; set; }
        public double UnitPrice { get; set; }
        public double Amount { get; set; }
        // 0 for public and 1 for private
        public bool Status { get; set; }
    }
    public class EditListing : CreateListing
    {
        public long Id { get; set; }
    }
}