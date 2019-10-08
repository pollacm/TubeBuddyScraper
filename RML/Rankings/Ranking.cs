using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TubeBuddyScraper.Rankings
{
    public class Ranking
    {
        public string Team { get; set; }
        public int Rank { get; set; }
        public int Wins { get; set; }
        public int Loses { get; set; }
        public int Draws { get; set; }
        public string Division { get; set; }
        public int Id { get; set; }
    }
}
