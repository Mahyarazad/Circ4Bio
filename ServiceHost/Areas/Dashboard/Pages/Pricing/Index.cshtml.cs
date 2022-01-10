using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ServiceHost.Areas.Dashboard.Pages.Pricing
{
    public class IndexModel : PageModel
    {
        public Guid token = Guid.NewGuid();

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet(string slug)
        {
        }
    }
}
