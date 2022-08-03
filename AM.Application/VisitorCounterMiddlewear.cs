using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace AM.Application
{
    public class VisitorCounterMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        public VisitorCounterMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }
        public int count;
        public async Task Invoke(HttpContext httpContext)
        {

            string visitorId = httpContext.Request.Cookies["VisitorId"];
            if (visitorId == null)
            {
                httpContext.Response.Cookies.Append(
                    "VisitorId",
                    Guid.NewGuid().ToString()
                    , new CookieOptions
                    {
                        Path = "/",
                        HttpOnly = true,
                        Secure = false
                    });
                count++;
                Console.WriteLine(count);
            }

            await _requestDelegate(httpContext);
        }
    }
}
