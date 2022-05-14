using FantasyTeams.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FantasyTeams.Commands.Uam
{
    public class UserLoginCommand : IRequest<CommandResponse>
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,10}$", 
            ErrorMessage = "At least one upper case English letter, one lower case English letter, one digit, one special character and length of 8 to 10")]
        public string Password { get; set; }
    }
}
