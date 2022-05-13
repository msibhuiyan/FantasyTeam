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
using System.Linq;
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
                return CommandResponse.Failure(new string[] { "User doesn't exists for deletion" });
            }
            await _teamService.DeleteTeam(user.TeamId);
            await _repository.DeleteAsync(deleteUserCommand.Email);
            return CommandResponse.Success();
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
                return CommandResponse.Failure(new string[] { "User Already Exists" });
            }
            user = new User();

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(userRegistrationCommand.Password, out passwordHash, out passwordSalt);

            user.Id = Guid.NewGuid().ToString();
            user.Email = userRegistrationCommand.Email;
            user.Password = passwordHash;
            user.Salt = passwordSalt;
            user.TeamId = Guid.NewGuid().ToString();
            user.TeamName = userRegistrationCommand.TeamName;
            user.FirstName = userRegistrationCommand.FirstName;
            user.LastName = userRegistrationCommand.LastName;
            user.Country = userRegistrationCommand.Country;
            user.Role = Role.Member.ToString();
            var teamCreationResponse = await _teamService.CreateNewTeam(new CreateTeamCommand{
                Name = user.TeamName,
                Country = user.Country
            }, user.TeamId);
            if(teamCreationResponse.Errors.Length > 0)
            {
                var userCreationResponseErrors = teamCreationResponse.Errors.ToList();
                userCreationResponseErrors.Add("User creation failed");
                return CommandResponse.Failure(userCreationResponseErrors.ToArray());
            }
            //await _playerService.CreateNewTeamPlayers(new Team { 
            //    Id = user.TeamId,
            //    Name = user.TeamName,
            //    Country = user.Country
            //});
            await _repository.CreateAsync(user);

            return CommandResponse.Success();
        }

        public async Task<CommandResponse> UserLogin(UserLoginCommand userLoginCommand)
        {
            
            var user = await _repository.GetByEmailAsync(userLoginCommand.Email);
            if (user == null)
                return CommandResponse.Failure(new string[] { "User Doesn't Exists" });
            if (!VerifyPasswordHash(userLoginCommand.Password, user.Password, user.Salt))
                return CommandResponse.Failure(new string[] { "Provide correct password" });

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
            return CommandResponse.Success(new
            {
                AccessToken = accesstoken,
                ExpiresAt = tokenExpiryDate,
            });
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
