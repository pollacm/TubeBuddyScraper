using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TubeBuddyScraper.Teams;
using TubeBuddyScraper.Weeks;

namespace TubeBuddyScraper.Trophies
{
    public class TopRankedSeasonTrophy : ITrophy
    {
        public TopRankedSeasonTrophy(Team team, string additionalInfo)
        {
            Team = team;
            AdditionalInfo = additionalInfo;
            BuildTogether = true;
        }
        public Team Team { get; set; }
        public string AdditionalInfo { get; }
        public bool BuildTogether { get; }

        public string GetTrophyName()
        {
            return TrophyConstants.Number1Seed;
        }

        public string LeaguePageText()
        {
            return GetTrophyName().ToUpper() + @" AWARD GOES TO...";
        }

        public string GetTrophyBody()
        {
            return Team.TeamName.ToUpper();
        }

        public string GetHeadline(Team team)
        {
            return $"For finishing the season ranked #1!!!!!";
        }

        public string GetReason(Team team)
        {
            return string.Empty;
        }
    }
}
