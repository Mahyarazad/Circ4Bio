using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _0_Framework.Application;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ServiceHost
{
    [HtmlTargetElement(Attributes = "Permission")]
    public class PermissionTagHelper : TagHelper
    {
        public int Permission { get; set; }
        private readonly IAutenticateHelper _autenticateHelper;

        public PermissionTagHelper(IAutenticateHelper autenticateHelper)
        {
            _autenticateHelper = autenticateHelper;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var permissions = _autenticateHelper.GetPermission();
            if (!_autenticateHelper.IsAuthenticated())
            {
                output.SuppressOutput();
                return;
            }

            if (!permissions.Contains(Permission))
            {
                output.SuppressOutput();
                return;
            }
            base.Process(context, output);
        }
    }
}
