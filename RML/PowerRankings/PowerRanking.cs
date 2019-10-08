using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TubeBuddyScraper.PowerRankings
{
    public class PowerRanking
    {
        public string TeamName { get; set; }
        public string TeamAbbreviation { get; set; }
        public int PreviousPowerRanking { get; set; }
        public int CurrentPowerRanking { get; set; }
        public decimal CurrentTotal { get; set; }
        public decimal PreviousTotal { get; set; }
    }
}
