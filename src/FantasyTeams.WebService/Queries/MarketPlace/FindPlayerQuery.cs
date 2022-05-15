using FantasyTeams.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FantasyTeams.Queries
{
    public class FindPlayerQuery : IRequest<QueryResponse>
    {
        [RegularExpression("[a-zA-Z0-9 ]+",
               ErrorMessage = "Please provide alpha numeric value")]
        public string PlayerName { get; set; }
        [RegularExpression("[a-zA-Z0-9]+",
               ErrorMessage = "Please provide alpha numeric value")]
        public string TeamName { get; set; }
        [RegularExpression("[a-zA-Z0-9 ]+",
               ErrorMessage = "Please provide alpha numeric value")]
        public string Country { get; set; }
        [Range(0, 100000000000,
               ErrorMessage = "Please provide between 0 and 100000000000")]
        public double? Value { get; set; }
    }
}
