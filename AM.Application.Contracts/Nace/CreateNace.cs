using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using _0_Framework;

namespace AM.Application.Contracts.Nace
{
    public class CreateNace
    {
        [Required(ErrorMessage = ValidationMessages.FieldRequired)]
        public string? Title { get; set; }
        public List<CreateDetail>? Items { get; set; }
    }
}