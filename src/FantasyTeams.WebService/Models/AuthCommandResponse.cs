using System;

namespace FantasyTeams.Models
{
    public class AuthCommandResponse
    {
        public string Message { get; set; }
        public string AccessToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        //internal AuthCommandResponse(bool succeeded, Dictionary<string, string> results, IEnumerable<string> errors)
        //{
        //    Succeeded = succeeded;
        //    Results = results;
        //    Errors = errors.ToArray();
        //}
        //public bool Succeeded { get; set; }

        //public string[] Errors { get; set; }
        //public Dictionary<string, string> Results { get; set; }

        //public static AuthCommandResponse Success(Dictionary<string, string> results)
        //{
        //    return new AuthCommandResponse(true, results, new string[] { });
        //}

        //public static AuthCommandResponse Failure(IEnumerable<string> errors)
        //{
        //    return new AuthCommandResponse(false, new Dictionary<string, string>(), errors);
        //}
    }
}
