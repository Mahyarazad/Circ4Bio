using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using _0_Framework;

namespace AM.Application.Contracts.Nace
{
    public class EditNace
    {
        public long NaceId { get; set; }
        public bool IsDeleted { get; set; }
        [Required(ErrorMessage = ValidationMessages.FieldRequired)]
        public string Title { get; set; }
        [Required(ErrorMessage = ValidationMessages.FieldRequired)]
        public List<EditIndexDetail> EditIndices { get; set; }
    }
}