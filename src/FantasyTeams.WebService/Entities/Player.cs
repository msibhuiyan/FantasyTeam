namespace FantasyTeams.Entities
{
    public class Player
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Country { get; set; }
        public string TeamId { get; set; }
        public string TeamName { get; set; }
        public int Age { get; set; }
        public string PlayerType { get; set; }
        public double Value { get; set; }
        public bool ForSale { get; set; }
        public double AskingPrice{ get; set; }
    }
}
