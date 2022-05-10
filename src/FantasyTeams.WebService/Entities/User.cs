using FantasyTeams.Enums;

namespace FantasyTeams.Entities
{
    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string TeamId { get; set; }
        public string Role { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
    }
}
