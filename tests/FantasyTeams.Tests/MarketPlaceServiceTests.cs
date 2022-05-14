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
    public class MarketPlaceServiceTests
    {
        private readonly Mock<ILogger<MarketPlaceService>> _logger;
        private readonly Mock<ITeamRepository> _teamRepository;
        private readonly Mock<IMarketPlaceRepository> _marketRepository;
        private readonly IMarketPlaceService _sut;
        private readonly IFixture _fixture = new Fixture();
        public MarketPlaceServiceTests()
        {
            _logger = new Mock<ILogger<MarketPlaceService>>();
            _teamRepository = new Mock<ITeamRepository>();
            _marketRepository = new Mock<IMarketPlaceRepository>();
            _sut = new MarketPlaceService(
                _logger.Object,
                _marketRepository.Object,
                _teamRepository.Object);
        }
    }
}
