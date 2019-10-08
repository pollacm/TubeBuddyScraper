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
    public class DefensivePlayerOfTheWeekTrophy : ITrophy
    {
        public DefensivePlayerOfTheWeekTrophy(Team team, string additionalInfo)
        {
            Team = team;
            AdditionalInfo = additionalInfo;
            BuildTogether = false;
        }
        public Team Team { get; set; }
        public string AdditionalInfo { get; }
        public bool BuildTogether { get; }

        public string GetTrophyName()
        {
            return TrophyConstants.DefensivePlayerOfTheWeek;
        }

        public string LeaguePageText()
        {
            return GetTrophyName().ToUpper();
        }

        public string GetTrophyBody()
        {
            var dp = JsonConvert.DeserializeObject<PlayerOfTheWeek>(AdditionalInfo);
            return $"[player#{dp.PlayerId}]{dp.Name.ToUpper()}[/player] ({dp.Team.ToUpper()})" + @"

                [image]<update>[/image]";
        }

        public string GetHeadline(Team team)
        {
            var op = JsonConvert.DeserializeObject<PlayerOfTheWeek>(AdditionalInfo);
            return $"{op.Name.ToUpper()} - {op.Points} POINTS!!!!!";
        }

        public string GetReason(Team team)
        {
            return string.Empty;
        }
    }
}
