using FantasyTeams.Commands;
using FantasyTeams.Contracts;
using FantasyTeams.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace FantasyTeams.CommandHandlers
{
    public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, AuthCommandResponse>
    {
        private readonly ILogger<UserLoginCommandHandler> _logger;
        private readonly IUamService _uamService;
        public UserLoginCommandHandler(ILogger<UserLoginCommandHandler> logger,
            IUamService uamService)
        {
            _logger = logger;
            _uamService = uamService;
        }

        public async Task<AuthCommandResponse> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            return await _uamService.UserLogin(request);
        }
    }
}
