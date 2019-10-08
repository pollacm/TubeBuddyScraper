using System;
using TubeBuddyScraper.Teams;

namespace TubeBuddyScraper.Weeks
{
    public class Score
    {
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }

        public decimal MarginOfVictory => Math.Abs(HomeTeam.TeamPoints - AwayTeam.TeamPoints);
    }
}
