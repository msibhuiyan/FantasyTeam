using AutoFixture;
using FantasyTeams.Contracts;
using FantasyTeams.Repository;
using FantasyTeams.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace FantasyTeams.Tests
{

    public class PlayerServiceTests
    {
        private readonly Mock<ILogger<PlayerService>> _logger;
        private readonly Mock<ITeamRepository> _teamRepository;
        private readonly Mock<IPlayerRepository> _playerRepository;
        private readonly IPlayerService _sut;
        private readonly IFixture _fixture = new Fixture();
        public PlayerServiceTests()
        {
            _logger = new Mock<ILogger<PlayerService>>();
            _teamRepository = new Mock<ITeamRepository>();
            _playerRepository = new Mock<IPlayerRepository>();
            _sut = new PlayerService(
                _logger.Object,
                _playerRepository.Object,
                _teamRepository.Object);
        }
    }
}
