using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TubeBuddyScraper
{
    public class PlayerOfTheWeek
    {
        public string Name { get; set; }
        public string Team { get; set; }
        public string TeamAbbreviation { get; set; }
        public int PlayerId { get; set; }
        public decimal Points { get; set; }
    }
}
