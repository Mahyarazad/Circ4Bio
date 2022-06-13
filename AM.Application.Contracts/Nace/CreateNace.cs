using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using _0_Framework;

namespace AM.Application.Contracts.Nace
{
    public class CreateNace
    {
        [Required(ErrorMessage = ApplicationMessage.SubmitRequiredInfo)]
        public string? Title { get; set; }
        public List<CreateDetail>? Items { get; set; }
    }
}