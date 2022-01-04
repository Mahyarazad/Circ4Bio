using System.Reflection;
using _0_Framework.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ServiceHost
{
    public class PageSecurityFilter : IPageFilter
    {
        private readonly IAutenticateHelper _autenticateHelper;

        public PageSecurityFilter(IAutenticateHelper autenticateHelper)
        {
            _autenticateHelper = autenticateHelper;
        }

        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            var permissions =
               (RequirePermission)context.HandlerMethod.MethodInfo.GetCustomAttributes(typeof(RequirePermission));
            var accountPermission = _autenticateHelper.GetPermission();
            if (!accountPermission.Contains(permissions.Permission))
                context.HttpContext.Response.Redirect("./Login");
        }

        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}