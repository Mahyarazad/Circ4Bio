using _0_Framework.Application;
using _0_Framework.Infrastructure;
using AM.Application.Contracts.Blog;
using AM.Infrastructure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Dashboard.Pages.Blog
{
    public class CreateModel : PageModel
    {
        public BlogViewModel Command;
        private readonly IBlogApplication _blogApplication;

        public CreateModel(IBlogApplication blogApplication)
        {
            _blogApplication = blogApplication;
        }

        public void OnGet()
        {
            Command = new BlogViewModel();

        }
        [NeedsPermission(UserPermission.CreateBlogPost)]
        public JsonResult OnPost(CreateBlogModel command)
        {
            return new JsonResult(_blogApplication.CreateBlog(command));
        }

    }
}
