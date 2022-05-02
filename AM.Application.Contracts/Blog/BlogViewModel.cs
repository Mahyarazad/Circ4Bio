using System;
using System.ComponentModel.DataAnnotations;
using _0_Framework;
using _0_Framework.Application;
using Microsoft.AspNetCore.Http;

namespace AM.Application.Contracts.Blog
{
    public class CreateBlogModel
    {
        [Required(ErrorMessage = ValidationMessages.FieldRequired)]
        public string? Title { get; set; }

        [Required(ErrorMessage = ValidationMessages.FieldRequired)]
        public string? Category { get; set; }

        [Required(ErrorMessage = ValidationMessages.FieldRequired)]
        public string? Auther { get; set; }

        [Required(ErrorMessage = ValidationMessages.FieldRequired)]
        public int ReadDuration { get; set; }

        [MaxLength(500, ErrorMessage = ValidationMessages.MaxChar500)]
        [Required(ErrorMessage = ValidationMessages.FieldRequired)]
        public string? ShortDescription { get; set; }

        [Required(ErrorMessage = ValidationMessages.FieldRequired)]
        public string? Body { get; set; }

        [MaxFileSize(1 * 1024 * 1024, ErrorMessage = ValidationMessages.SizeError1M)]
        public IFormFile? Image { get; set; }
        public long UserId { get; set; }
        public string? Slug { get; set; }
        public string? AvatarImage { get; set; }
    }

    public class EditBlogViewModel : CreateBlogModel
    {
        public long Id { get; set; }

    }

    public class BlogViewModel : EditBlogViewModel
    {
        public string? ImageString { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime CreationTime { get; set; }
    }
}