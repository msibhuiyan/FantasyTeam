using System;

namespace FantasyTeams.Models
{
    public class AuthCommandResponse
    {
        public string Message { get; set; }
        public string AccessToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
