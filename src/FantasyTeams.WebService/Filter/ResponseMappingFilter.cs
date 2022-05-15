using FantasyTeams.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace FantasyTeams.Filter
{
    public class ResponseMappingFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult && objectResult.Value is CommandResponse commandResponse && commandResponse.StatusCode != HttpStatusCode.OK)
                context.Result = new ObjectResult(commandResponse) { StatusCode = (int)commandResponse.StatusCode };
            if (context.Result is ObjectResult objectQueryResult && objectQueryResult.Value is QueryResponse queryResponse && queryResponse.StatusCode != HttpStatusCode.OK)
                context.Result = new ObjectResult( queryResponse ) { StatusCode = (int)queryResponse.StatusCode };
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}
