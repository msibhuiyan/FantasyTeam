using System.Collections.Generic;
using System.Linq;

namespace FantasyTeams.Models
{
    public class CommandResponse
    {
        internal CommandResponse(bool succeeded, object results, IEnumerable<string> errors)
        {
            Succeeded = succeeded;
            Results = results;
            Errors = errors.ToArray();
        }
        public bool Succeeded { get; set; }

        public string[] Errors { get; set; }
        public object Results { get; set; }

        public static CommandResponse Success(object results)
        {
            return new CommandResponse(true, results, new string[] { });
        }
        public static CommandResponse Success()
        {
            return new CommandResponse(true, new string[] { }, new string[] { });
        }

        public static CommandResponse Failure(IEnumerable<string> errors)
        {
            return new CommandResponse(false, new string[] { }, errors);
        }
    }
}
