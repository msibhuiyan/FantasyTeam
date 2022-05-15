using FantasyTeams.Commands.Uam;
using FantasyTeams.Contracts;
using FantasyTeams.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
namespace FantasyTeams.CommandHandlers.Uam
{
    public class OnboardUserCommandHandler : IRequestHandler<OnboardUserCommand, CommandResponse>
    {
        private readonly ILogger<UserLoginCommandHandler> _logger;
        private readonly IUamService _uamService;
        public OnboardUserCommandHandler(ILogger<UserLoginCommandHandler> logger,
            IUamService uamService)
        {
            _logger = logger;
            _uamService = uamService;
        }

        public async Task<CommandResponse> Handle(OnboardUserCommand request, CancellationToken cancellationToken)
        {
            return await _uamService.OnboardUser(request);
        }
    }
}
