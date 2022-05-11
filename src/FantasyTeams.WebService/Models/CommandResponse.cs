using Microsoft.AspNetCore.Mvc;

namespace FantasyTeams.Models
{
    public class CommandResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }
}
