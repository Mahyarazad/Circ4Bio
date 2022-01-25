using System.ComponentModel.DataAnnotations;
using _0_Framework;

namespace AM.Application.Contracts.Listing
{
    public class InputAmount
    {
        public int Id { get; set; }
        public bool OperationType { get; set; }
        public long ListingId { get; set; }
        public double CurrentAmount { get; set; }
        [Range(1, 1000000000, ErrorMessage = "The input number should be a postive number!")]
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public double Count { get; set; }
        public string? Description { get; set; }
        public long DealId { get; set; }
        public long UserId { get; set; }
    }
}