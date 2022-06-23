using _0_Framework.Application;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ServiceHost
{
    [HtmlTargetElement(Attributes = "Permission")]
    public class PermissionTagHelper : TagHelper
    {
        public int Permission { get; set; }
        private readonly IAuthenticateHelper _authenticateHelper;

        public PermissionTagHelper(IAuthenticateHelper authenticateHelper)
        {
            _authenticateHelper = authenticateHelper;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            if (!_authenticateHelper.IsAuthenticated())
            {
                output.SuppressOutput();
                return;
            }
            var permissions = _authenticateHelper.GetPermission();
            if (!permissions.Contains(Permission))
            {
                output.SuppressOutput();
                return;
            }
            base.Process(context, output);
        }
    }
}
