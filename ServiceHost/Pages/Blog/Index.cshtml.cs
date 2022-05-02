using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _0_Framework.Application;
using AM.Application.Contracts.Blog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages.Blog
{
    public class IndexModel : PageModel
    {
        private readonly IBlogApplication _blogApplication;
        public BlogViewModel Blog { get; set; }
        public IndexModel(IBlogApplication blogApplication)
        {
            _blogApplication = blogApplication;
        }

        public void OnGet(string id)
        {
            Blog = _blogApplication.GetSingleBlog(id).Result;
        }
    }
}
