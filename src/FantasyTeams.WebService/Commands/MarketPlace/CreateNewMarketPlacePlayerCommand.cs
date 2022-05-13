using FantasyTeams.Enums;

namespace FantasyTeams.Commands
{
    public class CreateNewMarketPlacePlayerCommand
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public double AskingPrice { get; set; }
        public string PlayerType { get; set; }
    }
}
