using FantasyTeams.Enums;

namespace FantasyTeams.Entities
{
    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string TeamId { get; set; }
        public string TeamName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string Role { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
    }
}
