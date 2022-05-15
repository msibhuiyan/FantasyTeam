using AutoFixture;
using FantasyTeams.Contracts;
using FantasyTeams.Entities;
using FantasyTeams.Repository;
using FantasyTeams.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FantasyTeams.Commands.MarketPlace;
using FantasyTeams.Enums;
using FantasyTeams.Queries;

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
        [Fact]
        public async Task GetAllMarketPlacePlayerShouldReturnAllMarketPlacePlayer_WhenQueried()
        {
            //Arrange
            var playersMock = _fixture.Build<Player>()
                .With(x=> x.ForSale, true)
                .CreateMany()
                .ToList();

            var players = _marketRepository.Setup(x => x.GetAllAsync())
                .ReturnsAsync(playersMock);

            //Act
            var result = await _sut.GetAllMarketPlacePlayer();
            //Assert

            Assert.True(result.Succeeded);
            Assert.True(result.Errors.Length == 0);
            Assert.Equal(playersMock, result.Results as List<Player>);
        }
        [Fact]
        public async Task GetMarketPlacePlayerShouldReturnMarketPlacePlayer_WhenQueried()
        {
            //Arrange
            string playerId = _fixture.Build<string>().Create();

            var playerMock = _fixture.Build<Player>()
                .With(x => x.ForSale, true)
                .Create();

            var players = _marketRepository.Setup(x => x.GetByIdAsync(playerId))
                .ReturnsAsync(playerMock);

            //Act
            var result = await _sut.GetMarketPlacePlayer(playerId);
            //Assert

            Assert.True(result.Succeeded);
            Assert.True(result.Errors.Length == 0);
            Assert.Equal(playerMock, result.Results as Player);
        }
        [Fact]
        public async Task PurchasePlayerPlayerShouldReturnError_WhenBuyerDoesnotExist()
        {
            //Arrange
            var purchasePlayerCommand = _fixture.Build<PurchasePlayerCommand>().Create();

            //Act
            var result = await _sut.PurchasePlayer(purchasePlayerCommand);
            //Assert

            Assert.False(result.Succeeded);
            Assert.True(result.Errors.Length == 1);
            Assert.Equal("Buyer team doesn't exists", result.Errors.FirstOrDefault());
        }
        [Fact]
        public async Task PurchasePlayerPlayerShouldReturnError_WhenPlayerDoesnotExist()
        {
            //Arrange
            var purchasePlayerCommand = _fixture.Build<PurchasePlayerCommand>().Create();

            var buyerTeamInfoMock = _fixture.Build<Team>()
                .Create();

            var buyerTeamInfo = _teamRepository.Setup(x => x.GetByIdAsync(purchasePlayerCommand.TeamId))
                .ReturnsAsync(buyerTeamInfoMock);

            //Act
            var result = await _sut.PurchasePlayer(purchasePlayerCommand);
            //Assert

            Assert.True(result.Errors.Length == 1);
            Assert.Equal("The player is no longer in market place", result.Errors.FirstOrDefault());
        }
        [Fact]
        public async Task PurchasePlayerPlayerShouldReturnError_WhenTeamBuysOwnPlayer()
        {
            //Arrange
            var teamId = Guid.NewGuid().ToString();
            
            var purchasePlayerCommand = _fixture.Build<PurchasePlayerCommand>()
                .With(x=> x.TeamId, teamId)
                .Create();

            var playerMock = _fixture.Build<Player>()
                .With(x => x.ForSale, true)
                .With(x => x.TeamId, teamId)
                .Create();

            var players = _marketRepository.Setup(x => x.GetByIdAsync(purchasePlayerCommand.PlayerId))
                .ReturnsAsync(playerMock);

            var buyerTeamInfoMock = _fixture.Build<Team>()
                .With(x=> x.Id, teamId)
                .Create();

            var buyerTeamInfo = _teamRepository.Setup(x => x.GetByIdAsync(purchasePlayerCommand.TeamId))
                .ReturnsAsync(buyerTeamInfoMock);

            //Act
            var result = await _sut.PurchasePlayer(purchasePlayerCommand);
            //Assert

            Assert.False(result.Succeeded);
            Assert.True(result.Errors.Length == 1);
            Assert.Equal("Can not purchase your own player", result.Errors.FirstOrDefault());
        }
        [Fact]
        public async Task PurchasePlayerPlayerShouldReturnError_WhenTeamahasLessBudget()
        {
            //Arrange

            var purchasePlayerCommand = _fixture.Build<PurchasePlayerCommand>()
                .Create();

            var playerMock = _fixture.Build<Player>()
                .With(x => x.ForSale, true)
                .With(x => x.AskingPrice, 1000)
                .Create();

            var players = _marketRepository.Setup(x => x.GetByIdAsync(purchasePlayerCommand.PlayerId))
                .ReturnsAsync(playerMock);

            var buyerTeamInfoMock = _fixture.Build<Team>()
                .With(x => x.Budget, 100)
                .Create();

            var buyerTeamInfo = _teamRepository.Setup(x => x.GetByIdAsync(purchasePlayerCommand.TeamId))
                .ReturnsAsync(buyerTeamInfoMock);

            //Act
            var result = await _sut.PurchasePlayer(purchasePlayerCommand);
            //Assert

            Assert.False(result.Succeeded);
            Assert.True(result.Errors.Length == 1);
            Assert.Equal("Dont have enough budget to purchase", result.Errors.FirstOrDefault());
        }
        [Theory]
        [InlineData(PlayerType.Attacker)]
        [InlineData(PlayerType.Defender)]
        [InlineData(PlayerType.MidFielder)]
        [InlineData(PlayerType.GoalKeeper)]
        public async Task PurchasePlayerPlayerShouldPurchasePlayer_WhenTeamWantToPurchasePlayer(PlayerType playerType)
        {
            //Arrange

            var purchasePlayerCommand = _fixture.Build<PurchasePlayerCommand>()
                .Create();

            var playerMock = _fixture.Build<Player>()
                .With(x => x.ForSale, true)
                .With(x => x.TeamId, Guid.NewGuid().ToString())
                .With(x => x.PlayerType, playerType.ToString())
                .With(x => x.AskingPrice, 1000)
                .Create();

            var players = _marketRepository.Setup(x => x.GetByIdAsync(purchasePlayerCommand.PlayerId))
                .ReturnsAsync(playerMock);

            var buyerTeamInfoMock = _fixture.Build<Team>()
                .With(x => x.Budget, 10000)
                .Create();

            var buyerTeamInfo = _teamRepository.Setup(x => x.GetByIdAsync(purchasePlayerCommand.TeamId))
                .ReturnsAsync(buyerTeamInfoMock);

            var sellerTeamInfoMock = _fixture.Build<Team>()
                .Create();

            var sellerTeamInfo = _teamRepository.Setup(x => x.GetByIdAsync(playerMock.TeamId))
                .ReturnsAsync(sellerTeamInfoMock);


            _marketRepository.Setup(x=> x.UpdateAsync(playerMock.Id, playerMock));
            _teamRepository.Setup(x=>x.UpdateAsync(buyerTeamInfoMock.Id, buyerTeamInfoMock));
            _teamRepository.Setup(x=>x.UpdateAsync(sellerTeamInfoMock.Id, sellerTeamInfoMock));
            //Act
            var result = await _sut.PurchasePlayer(purchasePlayerCommand);
            //Assert

            Assert.True(result.Succeeded);
            Assert.True(result.Errors.Length == 0);
        }
        [Theory]
        [InlineData(PlayerType.Attacker)]
        [InlineData(PlayerType.Defender)]
        [InlineData(PlayerType.MidFielder)]
        [InlineData(PlayerType.GoalKeeper)]
        public async Task PurchasePlayerPlayerShouldPurchasePlayer_WhenTeamWantToPurchasePlayerCreatedByAdmins(PlayerType playerType)
        {
            //Arrange

            var purchasePlayerCommand = _fixture.Build<PurchasePlayerCommand>()
                .Create();

            var playerMock = _fixture.Build<Player>()
                .With(x => x.ForSale, true)
                .With(x => x.PlayerType, playerType.ToString())
                .With(x => x.AskingPrice, 1000)
                .With(x => x.Value, 1000)
                .Without(x => x.TeamId)
                .Create();

            var players = _marketRepository.Setup(x => x.GetByIdAsync(purchasePlayerCommand.PlayerId))
                .ReturnsAsync(playerMock);

            var buyerTeamInfoMock = _fixture.Build<Team>()
                .With(x => x.Budget, 10000)
                .Create();

            var buyerTeamInfo = _teamRepository.Setup(x => x.GetByIdAsync(purchasePlayerCommand.TeamId))
                .ReturnsAsync(buyerTeamInfoMock);

            _marketRepository.Setup(x => x.UpdateAsync(playerMock.Id, playerMock));
            _teamRepository.Setup(x => x.UpdateAsync(buyerTeamInfoMock.Id, buyerTeamInfoMock));
            //Act
            var result = await _sut.PurchasePlayer(purchasePlayerCommand);
            //Assert

            Assert.True(result.Succeeded);
            Assert.True(result.Errors.Length == 0);
        }
        [Fact]
        public async Task DeletePlayerShouldReturnError_WhenWhenPlayerNotFound()
        {
            //Arrange
            var deletePlayerCommand = _fixture.Build<DeleteMarketPlacePlayerCommand>()
                .Create();
            //Act
            var result = await _sut.DeletePlayer(deletePlayerCommand);
            //Assert

            Assert.False(result.Succeeded);
            Assert.True(result.Errors.Length == 1);
            Assert.Matches("Player not found in marketplace for deletion", result.Errors.FirstOrDefault());
        }
        [Fact]
        public async Task DeletePlayerShouldReturnFailure_WhenWhenPlayerHasATeam()
        {
            //Arrange
            var deletePlayerCommand = _fixture.Build<DeleteMarketPlacePlayerCommand>()
                .Create();

            var playerMock = _fixture.Build<Player>()
                .With(x => x.Id , deletePlayerCommand.PlayerId)
                .With(x => x.ForSale, true)
                .Create();

            var players = _marketRepository.Setup(x => x.GetByIdAsync(deletePlayerCommand.PlayerId))
                .ReturnsAsync(playerMock);

            //Act
            var result = await _sut.DeletePlayer(deletePlayerCommand);
            //Assert

            Assert.False(result.Succeeded);
            Assert.True(result.Errors.Length == 1);
            Assert.Matches("Can not delete market place player associated with a team", result.Errors.FirstOrDefault());
        }

        [Fact]
        public async Task DeletePlayerShouldReturnFailure_WhenWhenPlayerHasNoTeam()
        {
            //Arrange
            var deletePlayerCommand = _fixture.Build<DeleteMarketPlacePlayerCommand>()
                .Create();

            var playerMock = _fixture.Build<Player>()
                .With(x => x.Id, deletePlayerCommand.PlayerId)
                .Without(x => x.TeamId)
                .With(x => x.ForSale, true)
                .Create();

            var players = _marketRepository.Setup(x => x.GetByIdAsync(deletePlayerCommand.PlayerId))
                .ReturnsAsync(playerMock);

            _marketRepository.Setup(x => x.DeleteAsync(deletePlayerCommand.PlayerId));
            //Act
            var result = await _sut.DeletePlayer(deletePlayerCommand);
            //Assert

            Assert.True(result.Succeeded);
            Assert.True(result.Errors.Length == 0);
        }
        [Fact]
        public async Task CreateNewMarketPlacePlayerShouldReturnError_WhenPlayerAlreadyExists()
        {
            //Arrange
            var createMarketPlacePlayerCommand = _fixture.Build<CreateMarketPlacePlayerCommand>()
                .Create();

            string fullname = createMarketPlacePlayerCommand.FirstName + " " + createMarketPlacePlayerCommand.LastName;
            var playerMock = _fixture.Build<Player>()
                .With(x => x.FullName, fullname)
                .With(x => x.ForSale, true)
                .Create();
            var players = _marketRepository.Setup(x => x.GetByNameAsync(fullname))
                .ReturnsAsync(playerMock);

            //Act
            var result = await _sut.CreateNewMarketPlacePlayer(createMarketPlacePlayerCommand);
            //Assert

            Assert.False(result.Succeeded);
            Assert.True(result.Errors.Length == 1);
            Assert.Matches("This player already exists", result.Errors.FirstOrDefault());
        }
        [Fact]
        public async Task CreateNewMarketPlacePlayerShouldReturnSuccess_WhenPlayerDoesnotExists()
        {
            //Arrange
            var createMarketPlacePlayerCommand = _fixture.Build<CreateMarketPlacePlayerCommand>()
                .Create();
            var playerMock = _fixture.Build<Player>()
               .With(x => x.ForSale, true)
               .Create();
            
            _marketRepository.Setup(x => x.CreateAsync(playerMock));
            //Act
            var result = await _sut.CreateNewMarketPlacePlayer(createMarketPlacePlayerCommand);
            //Assert
            Assert.True(result.Succeeded);
            Assert.True(result.Errors.Length == 0);
        }
    }
}
