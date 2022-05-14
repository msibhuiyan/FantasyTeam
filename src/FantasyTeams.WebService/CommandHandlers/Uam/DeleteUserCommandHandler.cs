using FantasyTeams.Commands.Uam;
using FantasyTeams.Contracts;
using FantasyTeams.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace FantasyTeams.CommandHandlers
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, CommandResponse>
    {

        private readonly ILogger<DeleteUserCommandHandler> _logger;
        private readonly IUamService _uamService;
        public DeleteUserCommandHandler(ILogger<DeleteUserCommandHandler> logger,
            IUamService uamService)
        {
            _logger = logger;
            _uamService = uamService;
        }

        public async Task<CommandResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            return await _uamService.DeleteUser(request);
        }
    }
}
