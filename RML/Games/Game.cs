using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TubeBuddyScraper
{
    public class Game
    {
        public Game()
        {
            TubebuddyRelatedSearches = new List<string>();
        }
        public string Title { get; set; }
        public string Keyword { get; set; }//(base, base + game, base + gameplay, base + horror, base + base + lets play,...)
        public string Description { get; set; }
        public string DateReleased { get; set; }
        public DateTime? DateChecked { get; set; }
        public GameSite Site { get; set; }
        public GameSystem Platform { get; set; }
        public string Price { get; set; }
        public GameType Type { get; set; }
        public string GameUrl { get; set; }
        public string Genre { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Score { get; set; }
        public string TubebuddyScore { get; set; }
        public string TubebuddyGrade { get; set; }
        public string TubebuddySearchVolume { get; set; }
        public string TubebuddySearchVolumeExact { get; set; }
        public string TubebuddyCompetitionScore { get; set; }
        public string TubebuddyCompetitionScoreExact { get; set; }
        public string TubebuddyOptimizationScore { get; set; }
        public string TubebuddyOptimizationScoreExact { get; set; }
        public string TubebuddyAverageViews { get; set; }
        public string TubebuddyTargetViews { get; set; }
        public string TubebuddyMyAverageViews { get; set; }
        public string TubebuddyNumberOfVideos { get; set; }
        public string TubebuddySearchesPerMonth { get; set; }
        public List<string> TubebuddyRelatedSearches { get; set; }

        public enum GameSystem
        {
            PS4,
            PC,
            Online,
            IOS,
            Android
        }

        public enum GameSite
        {
            Itch,
            GameJolt,
            Metacritic,
            GooglePlay
        }

        public enum GameType
        {
            Recent,
            Popular,
            NewAndPopular
        }

        //public enum GameGenre
        //{
        //    Horror,
        //    Other
        //}
    }
}
