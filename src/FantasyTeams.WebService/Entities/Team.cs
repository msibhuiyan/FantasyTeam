namespace FantasyTeams.Entities
{
    public class Team
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public double Value { get; set; }
        public double Budget { get; set; }
        public string[] GoalKeepers { get; set; }
        public string[] Defenders { get; set; }
        public string[] MidFielders { get; set; }
        public string[] Attackers { get; set; }

    }
}
