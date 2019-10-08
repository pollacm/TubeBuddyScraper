using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TubeBuddyScraper.Rankings
{
    public class PostDraftRanking
    {
        public string PlayerName { get; set; }
        public string PlayerPosition { get; set; }
        public string TeamName { get; set; }
        public decimal Projection { get; set; }

        public decimal WeeklyProjection => Projection / 16;
    }
}
