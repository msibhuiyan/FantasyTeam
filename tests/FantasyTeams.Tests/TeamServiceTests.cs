using AutoFixture;
using FantasyTeams.Commands.Team;
using FantasyTeams.Contracts;
using FantasyTeams.Entities;
using FantasyTeams.Queries;
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
    public class TeamServiceTests
    {
        private readonly Mock<ILogger<TeamService>> _logger;
        private readonly Mock<ITeamRepository> _teamRepository;
        private readonly Mock<IPlayerService> _playerService;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly ITeamService _sut;
        private readonly IFixture _fixture = new Fixture();
        public TeamServiceTests()
        {
            _logger = new Mock<ILogger<TeamService>>();
            _teamRepository = new Mock<ITeamRepository>();
            _playerService = new Mock<IPlayerService>();
            _userRepository = new Mock<IUserRepository>();
            _sut = new TeamService(
                _logger.Object,
                _teamRepository.Object,
                _playerService.Object,
                _userRepository.Object);
        }
        [Fact]
        public async Task CreateTeamShouldNotCreateTeamIfATeamAlaredyExists()
        {
            //Arrange
            CreateTeamCommand createTeamCommand = _fixture.Build<CreateTeamCommand>()
                .Create();

            var teamMock = _fixture.Build<Team>()
                .Create();

            var team = _teamRepository.Setup(x => x.GetByNameAsync(createTeamCommand.Name))
                .ReturnsAsync(teamMock);
            //Act
            var result = await _sut.CreateNewTeam(createTeamCommand);
            //Assert

            Assert.True(result.Errors.Length == 1);
            Assert.Matches("Team already exists.", result.Errors.FirstOrDefault());
        }
        [Fact]
        public async Task GetTeamInfoByTeamNameShouldReturnSuccess_WhenTeamsAreQueried()
        {
            //Arrange
            GetTeamQuery getTeamQuery = _fixture.Build<GetTeamQuery>()
                .Without(x => x.TeamId)
                .Create();
            //Act
            var result = await _sut.GetTeamInfo(getTeamQuery);
            //Assert

            Assert.True( result.Succeeded);
            var resultMessage = result.Results as string[];
            Assert.Matches("No team Found", resultMessage.FirstOrDefault());
        }
        [Fact]
        public async Task GetTeamInfoByTeamNameShouldReturnSuccessWithTeam_WhenTeamsAreQueried()
        {
            //Arrange
            GetTeamQuery getTeamQuery = _fixture.Build<GetTeamQuery>()
                .Without(x => x.TeamId)
                .Create();

            var teamMock = _fixture.Build<Team>()
                .With(x=> x.Name, getTeamQuery.TeamName)
                .Create();

            var team = _teamRepository.Setup(x => x.GetByNameAsync(getTeamQuery.TeamName))
                .ReturnsAsync(teamMock);
            //Act
            var result = await _sut.GetTeamInfo(getTeamQuery);
            //Assert

            Assert.True(result.Succeeded);
            Assert.Equal(teamMock, result.Results as Team);
        }
        [Fact]
        public async Task GetTeamInfoByTeamIdShouldReturnSuccess_WhenTeamsAreQueried()
        {
            //Arrange
            GetTeamQuery getTeamQuery = _fixture.Build<GetTeamQuery>()
                .Without(x => x.TeamName)
                .Create();

            var teamMock = _fixture.Build<Team>()
                .With(x => x.Id, getTeamQuery.TeamId)
                .Create();

            var team = _teamRepository.Setup(x => x.GetByNameAsync(getTeamQuery.TeamId))
                .ReturnsAsync(teamMock);
            //Act
            var result = await _sut.GetTeamInfo(getTeamQuery);
            //Assert
            Assert.True(result.Succeeded);
            var resultMessage = result.Results as string[];
            Assert.Matches("No team Found", resultMessage.FirstOrDefault());
        }
        [Fact]
        public async Task GetTeamInfoByTeamIdShouldReturnSuccessWithTeam_WhenTeamsAreQueried()
        {
            //Arrange
            GetTeamQuery getTeamQuery = _fixture.Build<GetTeamQuery>()
                .Without(x => x.TeamName)
                .Create();

            var teamMock = _fixture.Build<Team>()
                .With(x => x.Id, getTeamQuery.TeamId)
                .Create();

            var team = _teamRepository.Setup(x => x.GetByIdAsync(getTeamQuery.TeamId))
                .ReturnsAsync(teamMock);
            //Act
            var result = await _sut.GetTeamInfo(getTeamQuery);
            //Assert

            Assert.True(result.Succeeded);
            Assert.Equal(teamMock, result.Results as Team);
        }
        [Fact]
        public async Task GetAllTeamsShouldReturnAllTeams_WhenQueried()
        {
            //Arrange
            var teamMock = _fixture.Build<Team>()
                .CreateMany()
                .ToList();

            var teams = _teamRepository.Setup(x => x.GetAllAsync())
                .ReturnsAsync(teamMock);
            //Act
            var result = await _sut.GetAllTeams();
            //Assert

            Assert.True(result.Succeeded);
            Assert.Equal(teamMock, result.Results as List<Team>);
        }
        [Fact]
        public async Task UpdateTeamInfoShouldReturnError_WhenTeamNotFound()
        {
            //Arrange
            UpdateTeamCommand updateTeamCommand = _fixture.Build<UpdateTeamCommand>()
                .Create();
            //Act
            var result = await _sut.UpdateTeamInfo(updateTeamCommand);
            //Assert

            Assert.True(result.Errors.Length == 1);
            Assert.Matches("No team found for update", result.Errors.FirstOrDefault());
        }
        [Fact]
        public async Task UpdateTeamInfoShouldReturnError_WhenTeamNameExists()
        {
            //Arrange
            UpdateTeamCommand updateTeamCommand = _fixture.Build<UpdateTeamCommand>()
                .With(x=> x.Name, "TestTeamName")
                .Create();

            var teamMock = _fixture.Build<Team>()
                .With(x=>x.Name, "TestTeamName")
                .Create();

            var teamById = _teamRepository.Setup(x => x.GetByIdAsync(updateTeamCommand.TeamId))
                .ReturnsAsync(teamMock);

            var teamByName = _teamRepository.Setup(x => x.GetByNameAsync(updateTeamCommand.Name))
                .ReturnsAsync(teamMock);
            //Act
            var result = await _sut.UpdateTeamInfo(updateTeamCommand);
            //Assert

            Assert.True(result.Errors.Length == 1);
            Assert.Matches("Already a team exists on this name", result.Errors.FirstOrDefault());
        }
        [Fact]
        public async Task UpdateTeamInfoShouldReturnSuccess_WhenTeamUpdated()
        {
            //Arrange
            UpdateTeamCommand updateTeamCommand = _fixture.Build<UpdateTeamCommand>()
                .With(x => x.Name, "TestTeamName")
                .Create();

            var teamMock = _fixture.Build<Team>()
                .With(x => x.Name, "ExistingName")
                .Create();

            var teamById = _teamRepository.Setup(x => x.GetByIdAsync(updateTeamCommand.TeamId))
                .ReturnsAsync(teamMock);

            _teamRepository.Setup(x => x.UpdateAsync(updateTeamCommand.TeamId, teamMock));
            //Act
            var result = await _sut.UpdateTeamInfo(updateTeamCommand);
            //Assert

            Assert.True(result.Errors.Length == 0);
            Assert.True(result.Succeeded);
        }
        [Fact]
        public async void DeleteTeamShoudReturnErrorWhenTeamNotFound()
        {
            DeleteTeamCommand deleteTeamCommand = _fixture.Build<DeleteTeamCommand>()
                .Create();
            //Act
            var result = await _sut.DeleteTeam(deleteTeamCommand);
            //Assert
            Assert.True(result.Errors.Length == 1);
            Assert.Matches("No team found for delete", result.Errors.FirstOrDefault());
        }
        [Fact]
        public async void DeleteTeamShoudSuccessWhenTeamFound()
        {
            DeleteTeamCommand deleteTeamCommand = _fixture.Build<DeleteTeamCommand>()
                .Create();

            var teamMock = _fixture.Build<Team>()
                .Create();

            var teamById = _teamRepository.Setup(x => x.GetByIdAsync(deleteTeamCommand.TeamId))
                .ReturnsAsync(teamMock);

            _teamRepository.Setup(x => x.DeleteAsync(deleteTeamCommand.TeamId));
            //Act
            var result = await _sut.DeleteTeam(deleteTeamCommand);
            //Assert

            Assert.True(result.Errors.Length == 0);
            Assert.True(result.Succeeded);
        }
    }
}
