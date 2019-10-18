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
        public Status GameStatus { get; set; }
        public DateTime?  DateAdded { get; set; }
        public DateTime? DateExpired { get; set; }

        //TODO: Need position for each site??
        public int Position { get; set; }
        public DateTime? PositionChangeDate { get; set; }

        public override string ToString()
        {
            return Title + "\t" +  Keyword + "\t" + Description + "\t" + DateReleased + "\t" + Site + "\t" + Platform + "\t" + Price + "\t" + Type + "\t" + GameUrl + "\t" + Genre + "\t" + ThumbnailUrl + "\t" + Score + "\t" + TubebuddyScore
                   + "\t" + TubebuddyGrade + "\t" + TubebuddySearchVolume + "\t" + TubebuddySearchVolumeExact + "\t" + TubebuddyCompetitionScore + "\t" + TubebuddyCompetitionScoreExact
                   + "\t" + TubebuddyOptimizationScore + "\t" + TubebuddyOptimizationScoreExact + "\t" + TubebuddyAverageViews + "\t" + TubebuddyTargetViews + "\t" + TubebuddyMyAverageViews + "\t" + TubebuddyNumberOfVideos
                   + "\t" + TubebuddySearchesPerMonth + "\t" + string.Join("; ", TubebuddyRelatedSearches) +  "\t" + GameStatus + "\t" + DateAdded + "\t" + DateExpired;
        }

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
            NewAndPopular,
            AndroidFree,
            AndroidPaid
        }

        public enum Status
        {
            Current,
            New,
            Expired
        }

        //public enum GameGenre
        //{
        //    Horror,
        //    Other
        //}
    }

    public class GameComparer : IEqualityComparer<Game>
    {
        bool IEqualityComparer<Game>.Equals(Game x, Game y)
        {
            return String.Equals(x.Title, y.Title, StringComparison.CurrentCultureIgnoreCase) && x.Type == y.Type;
        }

        int IEqualityComparer<Game>.GetHashCode(Game obj)
        {
            if (Object.ReferenceEquals(obj, null))
                return 0;

            return obj.Title.ToLower().GetHashCode() + obj.Type.GetHashCode();
        }
    }
}
