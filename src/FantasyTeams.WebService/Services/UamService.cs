using FantasyTeams.Commands;
using FantasyTeams.Contracts;
using FantasyTeams.Entities;
using FantasyTeams.Enums;
using FantasyTeams.Models;
using FantasyTeams.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FantasyTeams.Services
{
    public class UamService : IUamService
    {
        private readonly ILogger<UamService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _repository;
        private readonly IPlayerService _playerService;
        private readonly ITeamService _teamService;

        public UamService(ILogger<UamService> logger,
            IConfiguration configuration,
            IUserRepository repository,
            IPlayerService playerService,
            ITeamService teamService)
        {
            _logger = logger;
            _configuration = configuration;
            _repository = repository;
            _playerService = playerService;
            _teamService = teamService;
        }

        public async Task<CommandResponse> DeleteUser(DeleteUserCommand deleteUserCommand)
        {
            var user = await _repository.GetByEmailAsync(deleteUserCommand.Email);
            if(user == null)
            {
                return new CommandResponse
                {
                    Message = "User doesn't exists for deletion",
                    IsSuccess = false
                };
            }
            await _teamService.DeleteTeam(user.TeamId);
            await _repository.DeleteAsync(deleteUserCommand.Email);
            return new CommandResponse
            {
                Message = "User deleted",
                IsSuccess = true
            };
        }

        public async Task<User> GetUserInfo(string userEmail)
        {
            return await _repository.GetByEmailAsync(userEmail);
        }

        public async Task<CommandResponse> RegisterUser(UserRegistrationCommand userRegistrationCommand)
        {
            var user = await _repository.GetByEmailAsync(userRegistrationCommand.Email);
            if (user != null)
            {
                return new CommandResponse
                {
                    Message = "User Already Exists",
                    IsSuccess = false
                };
            }
            user = new User();

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(userRegistrationCommand.Password, out passwordHash, out passwordSalt);

            user.Id = Guid.NewGuid().ToString();
            user.Email = userRegistrationCommand.Email;
            user.Password = passwordHash;
            user.Salt = passwordSalt;
            user.TeamId = Guid.NewGuid().ToString();
            user.Role = Role.Member.ToString();
            await _repository.CreateAsync(user);
            await _playerService.CreateNewTeamPlayers(user.TeamId);

            return new CommandResponse
            {
                Message = "User Created",
                IsSuccess = true
            };
        }

        public async Task<AuthCommandResponse> UserLogin(UserLoginCommand userLoginCommand)
        {
            
            var user = await _repository.GetByEmailAsync(userLoginCommand.Email);
            if (user == null)
                return new AuthCommandResponse
                {
                    Message = "User Doesn't Exists"
                };
            if (!VerifyPasswordHash(userLoginCommand.Password, user.Password, user.Salt))
                return new AuthCommandResponse
                {
                    Message = "Wrong Password"
                };

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email.ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, user.TeamId),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_configuration.GetSection("AppSettings:TokenKey").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenExpiryDate = DateTime.Now.AddDays(1);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = tokenExpiryDate,
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string accesstoken = tokenHandler.WriteToken(token);
            return new AuthCommandResponse
            {
                Message = "User Exists",
                AccessToken = accesstoken,
                ExpiresAt = tokenExpiryDate,
            };
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }
    }
}
