using FantasyTeams.Commands.Uam;
using FantasyTeams.Commands.Team;
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
using System.Collections.Generic;

namespace FantasyTeams.Services
{
    public class UamService : IUamService
    {
        private readonly ILogger<UamService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IPlayerRepository _playerRepository;

        public UamService(ILogger<UamService> logger,
            IConfiguration configuration,
            IUserRepository userRepository,
            ITeamRepository teamRepository,
            IPlayerRepository playerRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _userRepository = userRepository;
            _teamRepository = teamRepository;
            _playerRepository = playerRepository;
        }

        public async Task<CommandResponse> DeleteUser(DeleteUserCommand deleteUserCommand)
        {
            var user = await _userRepository.GetByEmailAsync(deleteUserCommand.Email);
            if(user == null)
            {
                return CommandResponse.Failure(new string[] { "User doesn't exists for deletion" });
            }
            await _teamRepository.DeleteAsync(user.TeamId);
            await _playerRepository.DeleteManyAsync(user.TeamId);
            await _userRepository.DeleteAsync(deleteUserCommand.Email);
            return CommandResponse.Success();
        }

        public async Task<User> GetUserInfo(string userEmail)
        {
            return await _userRepository.GetByEmailAsync(userEmail);
        }

        public async Task<CommandResponse> RegisterUser(UserRegistrationCommand userRegistrationCommand)
        {
            var user = await _userRepository.GetByEmailAsync(userRegistrationCommand.Email);
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
            var teamCreationResponse = await CreateNewTeam(user.TeamName, user.Country,
             user.TeamId);
            if(teamCreationResponse.Errors.Length > 0)
            {
                var userCreationResponseErrors = teamCreationResponse.Errors.ToList();
                userCreationResponseErrors.Add("User creation failed");
                return CommandResponse.Failure(userCreationResponseErrors.ToArray());
            }
            await _userRepository.CreateAsync(user);

            return CommandResponse.Success();
        }
        public async Task<CommandResponse> CreateNewTeam(string teamName, string country, string teamId)
        {
            var team = await _teamRepository.GetByNameAsync(teamName);
            if (team != null)
            {
                return CommandResponse.Failure(new string[] { "Team already exists." });
            }
            team = new Team();
            team.Id = teamId;
            team.Name = teamName;
            team.Country = country;

            var getTeamMembers = await CreateNewTeamPlayers(team);

            team.Attackers = getTeamMembers.Where(x => x.PlayerType == PlayerType.Attacker.ToString()).Select(x => x.Id).ToArray();
            team.Defenders = getTeamMembers.Where(x => x.PlayerType == PlayerType.Defender.ToString()).Select(x => x.Id).ToArray();
            team.MidFielders = getTeamMembers.Where(x => x.PlayerType == PlayerType.MidFielder.ToString()).Select(x => x.Id).ToArray();
            team.GoalKeepers = getTeamMembers.Where(x => x.PlayerType == PlayerType.GoalKeeper.ToString()).Select(x => x.Id).ToArray();
            team.Budget = 5000000;
            team.Value = 20000000;
            await _teamRepository.CreateAsync(team);
            return CommandResponse.Success();
        }

        public async Task<List<Player>> CreateNewTeamPlayers(Team team)
        {
            List<Player> players = new List<Player>();
            players.AddRange(AddGoalKeepers(team));
            players.AddRange(AddDefenders(team));
            players.AddRange(AddAttackers(team));
            players.AddRange(AddMidFielders(team));

            await _playerRepository.CreateManyAsync(players);
            return players;
        }
        private IEnumerable<Player> AddMidFielders(Team team)
        {
            Random rnd = new Random();
            List<Player> players = new List<Player>();
            for (int i = 0; i < 6; i++)
            {
                var player = new Player();
                player.Id = Guid.NewGuid().ToString();
                player.FirstName = "MidFielder";
                player.LastName = Guid.NewGuid().ToString();
                player.FullName = player.FirstName + " " + player.LastName;
                player.Country = team.Country;
                player.Value = 1000000;
                player.Age = rnd.Next(18, 40);
                player.ForSale = false;
                player.AskingPrice = 0;
                player.PlayerType = PlayerType.MidFielder.ToString();
                player.TeamId = team.Id;
                player.TeamName = team.Name;
                players.Add(player);
            }
            return players;
        }

        private IEnumerable<Player> AddAttackers(Team team)
        {
            Random rnd = new Random();
            List<Player> players = new List<Player>();
            for (int i = 0; i < 5; i++)
            {
                var player = new Player();
                player.Id = Guid.NewGuid().ToString();
                player.FirstName = "Attacker";
                player.LastName = Guid.NewGuid().ToString();
                player.FullName = player.FirstName + " " + player.LastName;
                player.Country = team.Country;
                player.Value = 1000000;
                player.Age = rnd.Next(18, 40);
                player.ForSale = false;
                player.AskingPrice = 0;
                player.PlayerType = PlayerType.Attacker.ToString();
                player.TeamId = team.Id;
                player.TeamName = team.Name;
                players.Add(player);
            }
            return players;
        }

        private IEnumerable<Player> AddDefenders(Team team)
        {
            Random rnd = new Random();
            List<Player> players = new List<Player>();
            for (int i = 0; i < 6; i++)
            {
                var player = new Player();
                player.Id = Guid.NewGuid().ToString();
                player.FirstName = "Defender";
                player.LastName = Guid.NewGuid().ToString();
                player.FullName = player.FirstName + " " + player.LastName;
                player.Country = team.Country;
                player.Value = 1000000;
                player.Age = rnd.Next(18, 40);
                player.ForSale = false;
                player.AskingPrice = 0;
                player.PlayerType = PlayerType.Defender.ToString();
                player.TeamId = team.Id;
                player.TeamName = team.Name;
                players.Add(player);
            }
            return players;
        }

        private IEnumerable<Player> AddGoalKeepers(Team team)
        {
            Random rnd = new Random();
            List<Player> players = new List<Player>();
            for (int i = 0; i < 3; i++)
            {
                var player = new Player();
                player.Id = Guid.NewGuid().ToString();
                player.FirstName = "Keeper";
                player.LastName = Guid.NewGuid().ToString();
                player.FullName = player.FirstName + " " + player.LastName;
                player.Country = team.Country;
                player.Value = 1000000;
                player.Age = rnd.Next(18, 40);
                player.ForSale = false;
                player.AskingPrice = 0;
                player.PlayerType = PlayerType.GoalKeeper.ToString();
                player.TeamId = team.Id;
                player.TeamName = team.Name;
                players.Add(player);
            }
            return players;
        }

        public async Task<CommandResponse> UserLogin(UserLoginCommand userLoginCommand)
        {
            
            var user = await _userRepository.GetByEmailAsync(userLoginCommand.Email);
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

        public async Task<CommandResponse> OnboardUser(OnboardUserCommand onboardUserCommand)
        {
            var user = await _userRepository.GetByEmailAsync(onboardUserCommand.Email);
            if (user != null)
            {
                return CommandResponse.Failure(new string[] { "User Already Exists" });
            }
            user = new User();

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(onboardUserCommand.Password, out passwordHash, out passwordSalt);

            user.Id = Guid.NewGuid().ToString();
            user.Email = onboardUserCommand.Email;
            user.Password = passwordHash;
            user.Salt = passwordSalt;
            user.TeamId = "";
            user.TeamName = "";
            user.FirstName = onboardUserCommand.FirstName;
            user.LastName = onboardUserCommand.LastName;
            user.Country = onboardUserCommand.Country;
            user.Role = Role.Member.ToString();
            await _userRepository.CreateAsync(user);

            return CommandResponse.Success();
        }
    }
}
