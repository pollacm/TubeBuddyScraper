using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TubeBuddyScraper.Teams;
using TubeBuddyScraper.Weeks;

namespace TubeBuddyScraper.Trophies
{
    public class NfcDivisionChampionshipTrophy : ITrophy
    {
        public NfcDivisionChampionshipTrophy(Team team, string additionalInfo)
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
            return TrophyConstants.NfcChamp;
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
            var op = JsonConvert.DeserializeObject<PlayerOfTheWeek>(AdditionalInfo);
            return $"For winning the NFC Division!!!!!";
        }

        public string GetReason(Team team)
        {
            return string.Empty;
        }
    }
}
