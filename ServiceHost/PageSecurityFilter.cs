using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using _0_Framework;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace ServiceHost
{
    public class PageSecurityFilter : IAsyncPageFilter
    {
        private readonly IAuthenticateHelper _authenticateHelper;

        public PageSecurityFilter(IAuthenticateHelper authenticateHelper)
        {
            _authenticateHelper = authenticateHelper;
        }

        public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            return Task.CompletedTask;
        }

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context
            , PageHandlerExecutionDelegate next)
        {

            NeedsPermissionAttribute handlerPermission = null;
            if (context.HandlerMethod != null)
                handlerPermission =
                    (NeedsPermissionAttribute)context.HandlerMethod.MethodInfo.GetCustomAttribute(
                        typeof(NeedsPermissionAttribute));

            if (handlerPermission != null)
            {

                List<int> accountPermissions;
                accountPermissions = _authenticateHelper.GetPermission();

                if (accountPermissions.All(x => x != handlerPermission.Permission))
                {
                    context.Result = new RedirectResult("/Dashboard/AccessDenied");
                    return;
                }
            }

            await next.Invoke();
        }
    }
}