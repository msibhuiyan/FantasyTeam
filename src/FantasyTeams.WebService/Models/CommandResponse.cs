using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace FantasyTeams.Models
{
    public class CommandResponse
    {
        internal CommandResponse(bool succeeded, object results, IEnumerable<string> errors, HttpStatusCode statuscode)
        {
            Succeeded = succeeded;
            StatusCode = statuscode;
            Results = results;
            Errors = errors.ToArray();
        }
        public bool Succeeded { get; set; }

        public string[] Errors { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public object Results { get; set; }

        public static CommandResponse Success(object results)
        {
            return new CommandResponse(true, results, new string[] { }, HttpStatusCode.OK);
        }
        public static CommandResponse Success()
        {
            return new CommandResponse(true, new string[] { }, new string[] { }, HttpStatusCode.OK);
        }

        public static CommandResponse Failure(IEnumerable<string> errors)
        {
            return new CommandResponse(false, new string[] { }, errors, HttpStatusCode.OK);
        }
        public static CommandResponse FailureForBidden(IEnumerable<string> errors)
        {
            return new CommandResponse(false, new string[] { }, errors, HttpStatusCode.Forbidden);
        }
    }
}
