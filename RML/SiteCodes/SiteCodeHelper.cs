using System.Linq;
using TubeBuddyScraper.PlayerComparer;

namespace TubeBuddyScraper.SiteCodes
{
    public static class SiteCodeHelper
    {
        public static string GetEspnCodeFromTeam(string fullSite)
        {
            return PlayerConstants.SiteCodes.Single(c => c.TeamCode == fullSite).EspnCode;
        }
    }
}
