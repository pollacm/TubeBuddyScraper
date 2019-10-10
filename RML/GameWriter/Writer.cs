using System.Collections.Generic;
using System.IO;

namespace TubeBuddyScraper.GameWriter
{
    public class Writer
    {
        private readonly List<Game> _games;
        //private string gameFile = @"E:\Dropbox\Private\GCG\GameDoc.txt";
        private string gameFile = @"C:\Users\cxp6696\Dropbox\Private\GCG\GameDoc.txt";

        public Writer(List<Game> games)
        {
            _games = games;
        }

        public void WriteGameFile()
        {
            using (StreamWriter file = new StreamWriter(gameFile))
            {
                PrintHeader(file);

                foreach (var game in _games)
                {
                    file.WriteLine(game.ToString());
                }
            }
        }

        private void PrintHeader(StreamWriter file)
        {
            file.WriteLine("Title\tKeyword\tDescription\tDateReleased\tSite\tPlatform\tPrice\tType\tGameUrl\tGenre\tThumbnailUrl\tScore\tTubebuddyScore\tTubebuddyGrade\tTubebuddySearchVolume\t" +
                           "TubebuddySearchVolumeExact\tTubebuddyCompetitionScore\tTubebuddyCompetitionScoreExact\tTubebuddyOptimizationScore\tTubebuddyOptimizationScoreExact\tTubebuddyAverageViews\t" +
                           "TubebuddyTargetViews\tTubebuddyMyAverageViews\tTubebuddyNumberOfVideos\tTubebuddySearchesPerMonth\tTubebuddyRelatedSearches");
            
        }
    }
}