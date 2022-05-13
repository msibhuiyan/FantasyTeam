using FantasyTeams.Enums;
using FantasyTeams.Models;
using MediatR;

namespace FantasyTeams.Commands
{
    public class CreateMarketPlacePlayerCommand : IRequest<CommandResponse>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public double AskingPrice { get; set; }
        public string PlayerType { get; set; }
    }
}
