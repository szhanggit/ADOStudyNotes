using Microsoft.AspNetCore.Mvc.Filters;

namespace Services.Utility.Telemetry
{
    public class RequestTelemetryFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }
}
