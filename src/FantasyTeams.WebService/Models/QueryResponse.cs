using System.Collections.Generic;
using System.Linq;

namespace FantasyTeams.Models
{
    public class QueryResponse
    {
        internal QueryResponse(bool succeeded, object results, IEnumerable<string> errors)
        {
            Succeeded = succeeded;
            Results = results;
            Errors = errors.ToArray();
        }
        public bool Succeeded { get; set; }

        public string[] Errors { get; set; }
        public object Results { get; set; }

        public static QueryResponse Success(object results)
        {
            return new QueryResponse(true, results, new string[] { });
        }
        public static QueryResponse Success()
        {
            return new QueryResponse(true, new string[] { }, new string[] { });
        }

        public static QueryResponse Failure(IEnumerable<string> errors)
        {
            return new QueryResponse(false, new string[] { }, errors);
        }
    }
}
