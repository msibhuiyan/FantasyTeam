﻿using FantasyTeams.Models;
using MediatR;

namespace FantasyTeams.Commands
{
    public class SetPlayerForSaleCommand : IRequest<CommandResponse>
    {
        public string PlayerId { get; set; }
        public double AskingPrice { get; set; }
    }
}
