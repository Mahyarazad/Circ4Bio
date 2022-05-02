using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _0_Framework.Application.PayPal;
using AM.Application.Contracts.Blog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PayPal.Api;

namespace ServiceHost.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IBlogApplication _blogApplication;
        public List<BlogViewModel> BlogList { get; set; } = new List<BlogViewModel>();
        public IndexModel(ILogger<IndexModel> logger, IBlogApplication blogApplication)
        {
            _logger = logger;
            _blogApplication = blogApplication;
        }

        public void OnGet()
        {
            BlogList = _blogApplication
                .GetBlogList()
                .Result.Where(x => !x.IsDeleted).ToList();
        }

    }
}
