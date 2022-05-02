using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AM.Application.Contracts.Blog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Dashboard.Pages.Blog
{
    public class IndexModel : PageModel
    {
        private readonly IBlogApplication _blogApplication;
        public List<BlogViewModel> BlogList;
        public bool IsDeleted { get; set; }

        public IndexModel(IBlogApplication blogApplication)
        {
            _blogApplication = blogApplication;
        }

        public void OnGet()
        {
            BlogList = _blogApplication.GetBlogList().Result;
        }

        public JsonResult OnPostMarkDelete(long Id)
        {
            return new JsonResult(_blogApplication.DeleteBlog(Id));
        }

        public JsonResult OnPostMarkUnDelete(long Id)
        {
            return new JsonResult(_blogApplication.UnDeleteBlog(Id));
        }

        public void OnPostDeletedFilter(bool isDeleted)
        {
            if (isDeleted)
            {
                IsDeleted = true;
                BlogList = _blogApplication.GetBlogList().Result;
                BlogList = BlogList.Where(x => !x.IsDeleted).ToList();
            }
            else
            {
                IsDeleted = false;
                BlogList = _blogApplication.GetBlogList().Result;
            }
        }
    }
}
