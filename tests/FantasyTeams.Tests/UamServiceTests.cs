using Microsoft.Extensions.Configuration;
using FantasyTeams.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FantasyTeams.Repository;
using FantasyTeams.Contracts;
using AutoFixture;
using FantasyTeams.Commands.Uam;
using System.Threading.Tasks;
using FantasyTeams.Commands.Team;
using FantasyTeams.Models;
using FantasyTeams.Entities;
using System.Linq;

namespace FantasyTeams.Tests
{
    public class UamServiceTests
    {
        private readonly Mock<ILogger<UamService>> _logger;
        private readonly Mock<IConfiguration> _configuration;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<ITeamRepository> _teamRepository;
        private readonly Mock<IPlayerRepository> _playerRepository;
        private readonly IUamService _sut;
        private readonly IFixture _fixture = new Fixture();

        public UamServiceTests()
        {
            _logger = new Mock<ILogger<UamService>>();
            _configuration = new Mock<IConfiguration>();
            _userRepository = new Mock<IUserRepository>();
            _teamRepository = new Mock<ITeamRepository>();
            _playerRepository = new Mock<IPlayerRepository>();
            _sut = new UamService(
                _logger.Object,
                _configuration.Object,
                _userRepository.Object,
                _teamRepository.Object,
                _playerRepository.Object);
        }

        [Fact]
        public async Task RegisterUserShouldNotRegisterNewUser_WhenThisUserExists()
        {
            //Arrange
            UserRegistrationCommand userRegistrationCommand = _fixture.Build<UserRegistrationCommand>()
                .With(x => x.Email , "saif_lesnar@outlok.com")
                .With(x => x.Password , "1qazZAQ!")
                .Create();

            var userMock = _fixture.Build<User>()
                .With(x => x.Email, userRegistrationCommand.Email)
                .Create();

            var user = _userRepository.Setup(x => x.GetByEmailAsync(userRegistrationCommand.Email))
                .ReturnsAsync(userMock);
            //Act
            var result = await _sut.RegisterUser(userRegistrationCommand);
            //Assert

            Assert.True(result.Errors.Length == 1);
            Assert.Matches("User Already Exists", result.Errors.FirstOrDefault());
        }
        [Fact]
        public async Task RegisterUserShouldNotRegisterNewUser_WhenTeamCannotGenerated()
        {
            //Arrange
            var userRegistrationCommand = _fixture.Build<UserRegistrationCommand>()
                .With(x => x.Email, "saif_lesnar@outlok.com")
                .With(x => x.Password, "1qazZAQ!")
                .Create();

            var team = _fixture.Build<Team>()
                .With(x=> x.Name, userRegistrationCommand.TeamName)
                .Create();

            var teamByName = _teamRepository.Setup(x => x.GetByNameAsync(userRegistrationCommand.TeamName)).ReturnsAsync(team);

            //Act
            var result = await _sut.RegisterUser(userRegistrationCommand);
            //Assert

            Assert.True(result.Errors.Length == 2);
            //Assert.Matches("User creation failed", result.Errors.FirstOrDefault());
        }
        [Fact]
        public async Task RegisterUserShouldRegisterNewUser_WhenTeamGenerated()
        {
            //Arrange
            var userRegistrationCommand = _fixture.Build<UserRegistrationCommand>()
                .With(x => x.Email, "saif_lesnar@outlok.com")
                .With(x => x.Password, "1qazZAQ!")
                .Create();

            var user = _fixture.Build<User>()
                .Create();

            _userRepository.Setup(x => x.CreateAsync(user));
            //Act
            var result = await _sut.RegisterUser(userRegistrationCommand);
            //Assert
            Assert.True(result.Errors.Length == 0);
            Assert.True(result.Succeeded);
        }
        [Fact]
        public async Task UserLoginShouldReturnError_WhenUserDoesnotExists()
        {
            //Arrange
            UserLoginCommand userLoginCommand = _fixture.Build<UserLoginCommand>()
                .With(x => x.Email, "saif_lesnar@outlok.com")
                .With(x => x.Password, "1qazZAQ!")
                .Create();

            //Act
            var result = await _sut.UserLogin(userLoginCommand);
            //Assert

            Assert.True(result.Errors.Length == 1);
            Assert.Matches("User Doesn't Exists", result.Errors.FirstOrDefault());
        }
        [Fact]
        public async Task UserLoginShouldReturnError_WhenUserProvidesWrongPassword()
        {
            //Arrange
            UserLoginCommand userLoginCommand = _fixture.Build<UserLoginCommand>()
                .With(x => x.Email, "saif_lesnar@outlok.com")
                .With(x => x.Password, "1qazZAQ!")
                .Create();
            var userMock = _fixture.Build<User>()
                .With(x => x.Email, "saif_lesnar@outlok.com")
                .Create();

            var user = _userRepository.Setup(
                x => x.GetByEmailAsync(userLoginCommand.Email))
                .ReturnsAsync(userMock);

            //Act
            var result = await _sut.UserLogin(userLoginCommand);
            //Assert

            Assert.True(result.Errors.Length == 1);
            Assert.Matches("Provide correct password", result.Errors.FirstOrDefault());
        }
        [Fact]
        public async Task DeleteUserShouldNotDeleteUser_WhenUserDoesnotExists()
        {
            //Arrange
            DeleteUserCommand deleteUserCommand = _fixture.Build<DeleteUserCommand>()
                .With(x => x.Email, "saif_lesnar@outlok.com")
                .Create();
            //Act
            var result = await _sut.DeleteUser(deleteUserCommand);
            //Assert

            Assert.True(result.Errors.Length == 1);
            Assert.Matches("User doesn't exists for deletion", result.Errors.FirstOrDefault());
        }
        [Fact]
        public async Task DeleteUserShouldDeleteUser_WhenUserExists()
        {
            //Arrange
            DeleteUserCommand deleteUserCommand = _fixture.Build<DeleteUserCommand>()
                .With(x => x.Email, "saif_lesnar@outlok.com")
                .Create();
            var userMock = _fixture.Build<User>()
                .With(x => x.Email, "saif_lesnar@outlok.com")
                .Create();

            var user = _userRepository.Setup(
                x => x.GetByEmailAsync(deleteUserCommand.Email))
                .ReturnsAsync(userMock);

            //Act
            var result = await _sut.DeleteUser(deleteUserCommand);
            //Assert

            Assert.True(result.Errors.Length == 0);
        }
        [Fact]
        public async Task OnboardUserShouldReturnError_WhenUserExists()
        {
            //Arrange
            var onboardUserCommand = _fixture.Build<OnboardUserCommand>()
                .With(x=> x.Password, "1qazZAQ!")
                .Create();
            var userMock = _fixture.Build<User>()
                .Create();

            var user = _userRepository.Setup(
                x => x.GetByEmailAsync(onboardUserCommand.Email))
                .ReturnsAsync(userMock);

            //Act
            var result = await _sut.OnboardUser(onboardUserCommand);
            //Assert

            Assert.True(result.Errors.Length == 1);
            Assert.Matches("User Already Exists", result.Errors.FirstOrDefault());
        }

        [Fact]
        public async Task OnboardUserShouldReturnSuccess_WhenUserDoesnotExists()
        {
            //Arrange
            var onboardUserCommand = _fixture.Build<OnboardUserCommand>()
                .With(x => x.Password, "1qazZAQ!")
                .Create();

            //Act
            var result = await _sut.OnboardUser(onboardUserCommand);
            //Assert

            Assert.True(result.Errors.Length == 0);
            Assert.True(result.Succeeded);
        }

    }
}
