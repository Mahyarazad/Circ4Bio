using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using _0_Framework;

namespace AM.Application.Contracts.Nace
{
    public class GetDetailList
    {
        [Required(ErrorMessage = ValidationMessages.FieldRequired)]
        public List<string>? DetailBody { get; set; }
        public List<int> GroupSize { get; set; }
        [Required(ErrorMessage = ValidationMessages.FieldRequired)]
        public List<string>? ItemDetailList { get; set; }
    }

    public class CreateDetail
    {
        [Required(ErrorMessage = ValidationMessages.FieldRequired)]
        public string? DetailBody { get; set; }
        [Required(ErrorMessage = ValidationMessages.FieldRequired)]
        public List<string>? ItemDetailList { get; set; }
    }

    public class EditIndexDetail
    {
        public int IndexId { get; set; }
        [Required(ErrorMessage = ValidationMessages.FieldRequired)]
        public string? DetailBody { get; set; }
        public bool IsDeleted { get; set; }
        public List<EditIndexDetailItems>? ItemDetailList { get; set; }
    }
    public class EditIndexDetailItems
    {
        public int IndexDetailId { get; set; }
        public int RefId { get; set; }
        public bool IsDeleted { get; set; }
        [Required(ErrorMessage = ValidationMessages.FieldRequired)]
        public string? DetailString { get; set; }

    }

    public class EditIndexDetailTDO
    {
        public List<int> IndexId { get; set; }
        [Required(ErrorMessage = ValidationMessages.FieldRequired)]
        public List<string>? DetailBody { get; set; }
        public List<bool> IsDeleted { get; set; }

    }

    public class EditIndexDetailItemsTDO
    {
        public List<int> IndexDetailId { get; set; }
        public List<int> RefId { get; set; }
        public List<bool> IsDeleted { get; set; }
        [Required(ErrorMessage = ValidationMessages.FieldRequired)]
        public List<string>? DetailString { get; set; }

    }
}