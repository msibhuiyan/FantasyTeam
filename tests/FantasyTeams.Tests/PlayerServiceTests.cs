using AutoFixture;
using FantasyTeams.Commands.Player;
using FantasyTeams.Contracts;
using FantasyTeams.Entities;
using FantasyTeams.Enums;
using FantasyTeams.Queries.Player;
using FantasyTeams.Repository;
using FantasyTeams.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

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
        [Fact]
        public async Task CreateNewPlayerShouldReturnError_WhenTeamDoesnotExists()
        {
            //Arrange
            var createTeamCommand = _fixture.Build<CreateNewPlayerCommand>()
                .Create();
            //Act
            var result = await _sut.CreateNewPlayer(createTeamCommand);
            //Assert

            Assert.True(result.Errors.Length == 1);
            Assert.Matches("Team doesn't exist to create player", result.Errors.FirstOrDefault());
        }
        [Fact]
        public async Task CreateNewPlayerShouldReturnError_WhenPlayerExists()
        {
            //Arrange
            var createTeamCommand = _fixture.Build<CreateNewPlayerCommand>()
                .With(x=> x.FirstName, "FName")
                .With(x=> x.LastName, "LName")
                .Create();

            var teamMock = _fixture.Build<Team>()
                .Create();

            var team = _teamRepository.Setup(x => x.GetByIdAsync(createTeamCommand.TeamId))
                .ReturnsAsync(teamMock);

            var fullName = createTeamCommand.FirstName + " " + createTeamCommand.LastName;

            var playerMock = _fixture.Build<Player>()
                .With(x=> x.FullName, fullName)
                .Create();


            var player = _playerRepository.Setup(x => x.GetByNameAsync(fullName))
                .ReturnsAsync(playerMock);
            //Act
            var result = await _sut.CreateNewPlayer(createTeamCommand);
            //Assert

            Assert.True(result.Errors.Length == 1);
            Assert.Matches("Player name already exists", result.Errors.FirstOrDefault());
        }
        [Theory]
        [InlineData(PlayerType.Defender)]
        [InlineData(PlayerType.Attacker)]
        [InlineData(PlayerType.MidFielder)]
        [InlineData(PlayerType.GoalKeeper)]
        public async Task CreateNewPlayerShouldSuccess_WhenTeamExistsPlayerDoesNotExists(PlayerType playerType)
        {
            //Arrange
            var createTeamCommand = _fixture.Build<CreateNewPlayerCommand>()
                .With(x => x.FirstName, "FName")
                .With(x => x.LastName, "LName")
                .With(x => x.PlayerType, playerType.ToString())
                .Create();

            var teamMock = _fixture.Build<Team>()
                .Create();

            var team = _teamRepository.Setup(x => x.GetByIdAsync(createTeamCommand.TeamId))
                .ReturnsAsync(teamMock);
            //Act
            var result = await _sut.CreateNewPlayer(createTeamCommand);
            //Assert

            Assert.True(result.Errors.Length == 0);
            Assert.True(result.Succeeded);
        }
        [Theory]
        [InlineData("")]
        [InlineData("396049c2-31a6-49c6-bd8b-06afc9e61363")]
        public async Task GetAllPlayerShouldReturnAllPlayer_WhenQueried(string teamId)
        {
            //Arrange
            var getAllPlayerQuery = _fixture.Build<GetAllPlayerQuery>()
                .With(x=> x.TeamId, teamId)
                .Create();

            var playerMock = _fixture.Build<Player>()
                .CreateMany()
                .ToList();

            if (string.IsNullOrEmpty(teamId))
            {
                var player = _playerRepository.Setup(x => x.GetAllAsync())
                .ReturnsAsync(playerMock);
            }
            else
            {
                var player = _playerRepository.Setup(x => x.GetAllAsync(getAllPlayerQuery.TeamId))
                .ReturnsAsync(playerMock);
            }
            //Act
            var result = await _sut.GetAllPlayer(getAllPlayerQuery);
            //Assert
            Assert.True(result.Errors.Length == 0);
            Assert.True(result.Succeeded);
        }
        [Fact]
        public async Task SetPlayerForSaleShouldReturnError_WhenOtherTeamPlayerUpdateRequestReceives()
        {
            //Arrange
            var setPlayerForSaleCommand = _fixture.Build<SetPlayerForSaleCommand>()
                .With(x => x.TeamId, Guid.NewGuid().ToString())
                .Create();

            var playerMock = _fixture.Build<Player>()
                .With(x=> x.TeamId , Guid.NewGuid().ToString())
                .Create();

            var player = _playerRepository.Setup(x => x.GetByIdAsync(setPlayerForSaleCommand.PlayerId))
                .ReturnsAsync(playerMock);
            //Act
            var result = await _sut.SetPlayerForSale(setPlayerForSaleCommand);
            //Assert
            Assert.True(result.Errors.Length == 1);
            Assert.True(result.StatusCode == System.Net.HttpStatusCode.Forbidden);
            Assert.Matches("Can not update other team player price", result.Errors.FirstOrDefault());
        }
        [Fact]
        public async Task SetPlayerForSaleShouldReturnSuccess_WhenCorrectTeamPlayerUpdateRequestReceives()
        {
            //Arrange
            var teamId = Guid.NewGuid().ToString();
            var setPlayerForSaleCommand = _fixture.Build<SetPlayerForSaleCommand>()
                .With(x => x.TeamId, teamId)
                .Create();

            var playerMock = _fixture.Build<Player>()
                .With(x => x.TeamId, teamId)
                .With(x => x.AskingPrice, setPlayerForSaleCommand.AskingPrice)
                .Create();

            var player = _playerRepository.Setup(x => x.GetByIdAsync(setPlayerForSaleCommand.PlayerId))
                .ReturnsAsync(playerMock);
            _playerRepository.Setup(x => x.UpdateAsync(playerMock.Id, playerMock));
            //Act
            var result = await _sut.SetPlayerForSale(setPlayerForSaleCommand);
            //Assert
            Assert.True(result.Errors.Length == 0);
            Assert.True(result.Succeeded);
        }
        [Fact]
        public async Task UpdatePlayerInfoShouldRetunError_WhenPlayerNotFound()
        {
            //Arrange
            var updatePlayerCommand = _fixture.Build<UpdatePlayerCommand>()
                .Create();
            //Act
            var result = await _sut.UpdatePlayerInfo(updatePlayerCommand);
            //Assert
            Assert.True(result.Errors.Length == 1);
            Assert.Matches("Player not found for update", result.Errors.FirstOrDefault());
        }
        [Fact]
        public async Task UpdatePlayerInfoShouldRetunError_WhenWrongTeamUpdateRequestReceives()
        {
            //Arrange
            var updatePlayerCommand = _fixture.Build<UpdatePlayerCommand>()
                .With(x=> x.TeamId, Guid.NewGuid().ToString())
                .Create();

            var playerMock = _fixture.Build<Player>()
                .With(x => x.TeamId, Guid.NewGuid().ToString())
                .Create();

            var player = _playerRepository.Setup(x => x.GetByIdAsync(updatePlayerCommand.PlayerId))
                .ReturnsAsync(playerMock);
            //Act
            var result = await _sut.UpdatePlayerInfo(updatePlayerCommand);
            //Assert
            Assert.True(result.Errors.Length == 1);
            Assert.True(result.StatusCode == System.Net.HttpStatusCode.Forbidden);
            Assert.Matches("You can not update other team", result.Errors.FirstOrDefault());
        }
        //[Fact]
        //public async Task UpdatePlayerInfoShouldRetunError_WhenPlayerNameAlreadyExists()
        //{
        //    //Arrange
        //    var teamId = Guid.NewGuid().ToString();
        //    var updatePlayerCommand = _fixture.Build<UpdatePlayerCommand>()
        //        .With(x => x.TeamId, teamId)
        //        .With(x => x.FirstName, "FName")
        //        .With(x => x.LastName, "LName")
        //        .Create();

        //    var playerMock = _fixture.Build<Player>()
        //        .With(x => x.TeamId, teamId)
        //        .Create();
        //    var player = _playerRepository.Setup(x => x.GetByIdAsync(updatePlayerCommand.PlayerId))
        //        .ReturnsAsync(playerMock);

        //    var fullName = updatePlayerCommand.FirstName + updatePlayerCommand.LastName;

        //    var existingPlayerMock = _fixture.Build<Player>()
        //        .With(x => x.FullName, fullName)
        //        .Create();
        //    var existingPlayer = _playerRepository.Setup(x => x.GetByNameAsync(fullName))
        //        .ReturnsAsync(existingPlayerMock);

        //    //Act
        //    var result = await _sut.UpdatePlayerInfo(updatePlayerCommand);
        //    //Assert
        //    Assert.True(result.Errors.Length == 1);
        //    Assert.Matches("Player name already exists", result.Errors.FirstOrDefault());
        //}
        [Theory]
        [InlineData("", "")]
        [InlineData("", "LName")]
        [InlineData("FName", "")]
        public async Task UpdatePlayerInfoShouldUpdate_WhenProperDataProvided(string fName, string lName)
        {
            //Arrange
            var teamId = Guid.NewGuid().ToString();
            var updatePlayerCommand = _fixture.Build<UpdatePlayerCommand>()
                .With(x => x.TeamId, teamId)
                .With(x => x.FirstName, fName)
                .With(x => x.LastName, lName)
                .Create();

            var playerMock = _fixture.Build<Player>()
                .With(x => x.TeamId, teamId)
                .Create();
            var player = _playerRepository.Setup(x => x.GetByIdAsync(updatePlayerCommand.PlayerId))
                .ReturnsAsync(playerMock);

            var fullName = updatePlayerCommand.FirstName + updatePlayerCommand.LastName;

            var existingPlayerMock = _fixture.Build<Player>()
                .With(x => x.FullName, fullName)
                .Create();
            var existingPlayer = _playerRepository.Setup(x => x.GetByNameAsync(fullName))
                .ReturnsAsync(existingPlayerMock);

            _playerRepository.Setup(x => x.UpdateAsync(updatePlayerCommand.PlayerId, playerMock));
            //Act
            var result = await _sut.UpdatePlayerInfo(updatePlayerCommand);
            //Assert
            Assert.True(result.Errors.Length == 0);
            Assert.True(result.Succeeded);
        }
        [Fact]
        public async Task GetPlayerShouldReturnSuccess_WhenPlayerAreQueried()
        {
            //Arrange
            var getPlayerQuery = _fixture.Build<GetPlayerQuery>()
                .Without(x => x.PlayerId)
                .Create();
            //Act
            var result = await _sut.GetPlayer(getPlayerQuery);
            //Assert

            Assert.True(result.Errors.Count() == 1);
            Assert.Matches("Player not found", result.Errors.FirstOrDefault());
        }
        [Fact]
        public async Task GetPlayerShouldReturnError_WhenOtherTeamPlayerAreQueried()
        {
            //Arrange
            var getPlayerQuery = _fixture.Build<GetPlayerQuery>()
                .With(x=> x.TeamId, Guid.NewGuid().ToString())
                .Create();

            var playerMock = _fixture.Build<Player>()
                .With(x => x.TeamId, Guid.NewGuid().ToString())
                .Create();

            var team = _playerRepository.Setup(x => x.GetByIdAsync(getPlayerQuery.PlayerId))
                .ReturnsAsync(playerMock);
            //Act
            var result = await _sut.GetPlayer(getPlayerQuery);
            //Assert

            Assert.True(result.Errors.Count() == 1);
            Assert.True(result.StatusCode == System.Net.HttpStatusCode.Forbidden);
            Assert.Matches("Can not fetch other team player", result.Errors.FirstOrDefault());
        }
        [Fact]
        public async Task GetPlayerShouldReturnSuccessWithPlayer_WhenPlayerAreQueried()
        {
            //Arrange
            var teamId = Guid.NewGuid().ToString();
            var getPlayerQuery = _fixture.Build<GetPlayerQuery>()
                .With(x => x.TeamId, teamId)
                .Create();

            var playerMock = _fixture.Build<Player>()
                .With(x => x.TeamId, teamId)
                .Create();

            var player = _playerRepository.Setup(x => x.GetByIdAsync(getPlayerQuery.PlayerId))
                .ReturnsAsync(playerMock);
            //Act
            var result = await _sut.GetPlayer(getPlayerQuery);
            //Assert

            Assert.True(result.Succeeded);
            Assert.Equal(playerMock, result.Results as Player);
        }
        [Fact]
        public async Task DeletePlayerShouldReturnError_WhenPlayerNotFound()
        {
            var deletePlayerCommand = _fixture.Build<DeletePlayerCommand>()
                .Create();
            //Act
            var result = await _sut.DeletePlayer(deletePlayerCommand);
            //Assert
            Assert.True(result.Errors.Length == 1);
            Assert.Matches("Player not found to delete", result.Errors.FirstOrDefault());
        }
        [Theory]
        [InlineData(PlayerType.Attacker)]
        [InlineData(PlayerType.Defender)]
        [InlineData(PlayerType.MidFielder)]
        [InlineData(PlayerType.GoalKeeper)]
        public async Task DeletePlayerShouldDeletePlayer_WhenPlayerFound(PlayerType playertype)
        {
            var deletePlayerCommand = _fixture.Build<DeletePlayerCommand>()
                .Create();
            var playerMock = _fixture.Build<Player>()
                .With(x=> x.PlayerType, playertype.ToString())
                .Create();

            var player = _playerRepository.Setup(x => x.GetByIdAsync(deletePlayerCommand.PlayerId))
                .ReturnsAsync(playerMock);

            var teamMock = _fixture.Build<Team>()
                .Create();

            var team = _teamRepository.Setup(x => x.GetByIdAsync(playerMock.TeamId))
                .ReturnsAsync(teamMock);

            _teamRepository.Setup(x => x.UpdateAsync(teamMock.Id, teamMock));
            //Act
            var result = await _sut.DeletePlayer(deletePlayerCommand);
            //Assert
            Assert.True(result.Errors.Length == 0);
            Assert.True(result.Succeeded);
        }
        [Fact]
        public async Task UpdatePlayerValueShouldReturnError_WhenPlayerNotFound()
        {
            var updatePlayerValueCommand = _fixture.Build<UpdatePlayerValueCommand>()
                .Create();
            //Act
            var result = await _sut.UpdatePlayerValue(updatePlayerValueCommand);
            //Assert
            Assert.True(result.Errors.Length == 1);
            Assert.Matches("Player not found for update", result.Errors.FirstOrDefault());
        }
        [Fact]
        public async Task UpdatePlayerValueShouldUpdatePlayer_WhenPlayerWithoutTeam()
        {
            var updatePlayerValueCommand = _fixture.Build<UpdatePlayerValueCommand>()
                .Create();
            var playerMock = _fixture.Build<Player>()
                .Create();

            var player = _playerRepository.Setup(x => x.GetByIdAsync(updatePlayerValueCommand.PlayerId))
                .ReturnsAsync(playerMock);

            _playerRepository.Setup(x => x.UpdateAsync(updatePlayerValueCommand.PlayerId, playerMock));
            //Act
            var result = await _sut.UpdatePlayerValue(updatePlayerValueCommand);
            //Assert
            Assert.True(result.Errors.Length == 0);
            Assert.True(result.Succeeded);
        }
        [Fact]
        public async Task UpdatePlayerValueShouldDeltePlayer_WhenPlayerWithTeam()
        {
            var updatePlayerValueCommand = _fixture.Build<UpdatePlayerValueCommand>()
                .Create();
            var playerMock = _fixture.Build<Player>()
                .Create();

            var player = _playerRepository.Setup(x => x.GetByIdAsync(updatePlayerValueCommand.PlayerId))
                .ReturnsAsync(playerMock);

            var teamMock = _fixture.Build<Team>()
                .Create();

            var team = _teamRepository.Setup(x => x.GetByIdAsync(playerMock.TeamId))
                .ReturnsAsync(teamMock);

            _teamRepository.Setup(x => x.UpdateAsync(playerMock.TeamId, teamMock));

            _playerRepository.Setup(x => x.UpdateAsync(updatePlayerValueCommand.PlayerId, playerMock));
            //Act
            var result = await _sut.UpdatePlayerValue(updatePlayerValueCommand);
            //Assert
            Assert.True(result.Errors.Length == 0);
            Assert.True(result.Succeeded);
        }
    }
}
