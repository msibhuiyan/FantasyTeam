using FantasyTeams.Commands.Uam;
using FantasyTeams.Contracts;
using FantasyTeams.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace FantasyTeams.CommandHandlers
{
    public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, CommandResponse>
    {
        private readonly ILogger<UserLoginCommandHandler> _logger;
        private readonly IUamService _uamService;
        public UserLoginCommandHandler(ILogger<UserLoginCommandHandler> logger,
            IUamService uamService)
        {
            _logger = logger;
            _uamService = uamService;
        }

        public async Task<CommandResponse> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            return await _uamService.UserLogin(request);
        }
    }
}
