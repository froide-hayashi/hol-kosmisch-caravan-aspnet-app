using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyWebApp.Filters
{
    public class LogFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var req = filterContext.HttpContext.Request;
            Trace.WriteLine($"Action Executing.|Path={req.Path}");
        }
    }
}