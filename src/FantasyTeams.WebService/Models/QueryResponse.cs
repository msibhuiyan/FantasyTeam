using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace FantasyTeams.Models
{
    public class QueryResponse
    {
        internal QueryResponse(bool succeeded, object results, IEnumerable<string> errors, HttpStatusCode statuscode)
        {
            Succeeded = succeeded;
            StatusCode = statuscode;
            Results = results;
            Errors = errors.ToArray();
        }
        public bool Succeeded { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public string[] Errors { get; set; }
        public object Results { get; set; }

        public static QueryResponse Success(object results)
        {
            return new QueryResponse(true, results, new string[] { }, HttpStatusCode.OK);
        }
        public static QueryResponse Success()
        {
            return new QueryResponse(true, new string[] { }, new string[] { }, HttpStatusCode.OK);
        }

        public static QueryResponse Failure(IEnumerable<string> errors)
        {
            return new QueryResponse(false, new string[] { }, errors, HttpStatusCode.OK);
        }
        public static QueryResponse FailureForBidden(IEnumerable<string> errors)
        {
            return new QueryResponse(false, new string[] { }, errors, HttpStatusCode.Forbidden);
        }
    }
}
