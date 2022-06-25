﻿using _0_Framework.Infrastructure;
using AM.Application.Contracts.Blog;
using AM.Infrastructure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Dashboard.Pages.Blog
{
    public class EditModel : PageModel
    {
        public BlogViewModel Command;
        private readonly IBlogApplication _blogApplication;

        public EditModel(IBlogApplication blogApplication)
        {
            _blogApplication = blogApplication;
        }

        [NeedsPermission(UserPermission.EditBlogPost)]
        public void OnGet(long Id)
        {
            Command = _blogApplication.GetSingleBlog(Id).Result;
        }

        [NeedsPermission(UserPermission.EditBlogPost)]
        public JsonResult OnPost(BlogViewModel command)
        {
            return new JsonResult(_blogApplication.EditBlog(command));
        }

    }
}
