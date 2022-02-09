using System.ComponentModel.DataAnnotations;
using _0_Framework;

namespace AM.Application.Contracts.User
{
    public class CreateDeliveryLocation
    {
        public long UserId { get; set; }
        [Required(ErrorMessage = ValidationMessages.DeliveryLocation)]
        [MaxLength(500, ErrorMessage = ValidationMessages.MaxCharAddress)]
        public string? Location { get; set; }
        public int LocationId { get; set; }
    }
}