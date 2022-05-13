using FantasyTeams.Commands;
using FantasyTeams.Contracts;
using FantasyTeams.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace FantasyTeams.CommandHandlers
{
    public class UserRegistrationCommandHandler : IRequestHandler<UserRegistrationCommand, CommandResponse>
    {
        private readonly ILogger<UserRegistrationCommandHandler> _logger;
        private readonly IUamService _uamService;
        public UserRegistrationCommandHandler(ILogger<UserRegistrationCommandHandler> logger,
            IUamService uamService)
        {
            _logger = logger;
            _uamService = uamService;
        }
        public async Task<CommandResponse> Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
        {
            return await _uamService.RegisterUser(request);
        }
    }
}
