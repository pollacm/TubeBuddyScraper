using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TubeBuddyScraper.Standings;
using TubeBuddyScraper.Teams;
using TubeBuddyScraper.Weeks;

namespace TubeBuddyScraper.Trophies
{
    public class HighestScoringSeasonTrophy : ITrophy
    {
        public HighestScoringSeasonTrophy(Team team, string additionalInfo)
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
            return TrophyConstants.HighestScoringTeamOfTheYear;
        }

        public string LeaguePageText()
        {
            return "THE " + GetTrophyName().ToUpper() + @" AWARD GOES TO...";
        }

        public string GetTrophyBody()
        {
            return Team.TeamName.ToUpper();
        }

        public string GetHeadline(Team team)
        {
            var standing = JsonConvert.DeserializeObject<Standing>(AdditionalInfo);
            return $"For putting up a total of {standing.PointsFor} throughout the season!!!!!";
        }

        public string GetReason(Team team)
        {
            return string.Empty;
        }
    }
}
