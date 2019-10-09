using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TubeBuddyScraper.TubeBuddyAnalyzer
{
    public class Analyzer
    {
        private readonly ChromeDriver _driver;
        private List<Game> _games;
        private string ItchPopularUrl = "https://itch.io/games/tag-horror";

        private readonly List<string> _additionalSearches = new List<string>
        {
            " game",
            " gameplay",
            " horror game",
            " horror gameplay",
            " game ending"
        };

        public Analyzer(ChromeDriver driver, List<Game> games)
        {
            _driver = driver;
            _games = games;
        }

        public List<Game> Analyze()
        {
            var games = AnalyzeGames();
            return games;
        }

        private List<Game> AnalyzeGames()
        {
            _driver.Navigate().GoToUrl($"https://www.youtube.com");
            _driver.FindElement(By.XPath("//button[@id='tb-main-menu']")).Click();
            _driver.FindElement(By.XPath("//li[contains(text(),'Keyword Explorer')]")).Click();
            var newGames = new List<Game>();

            foreach (var game in _games)
            {
                var searchbox = _driver.FindElement(By.XPath("//input[@id='tb-tag-explorer-input']"));
                searchbox.SendKeys(game.Title);
                _driver.FindElement(By.XPath("//button[@id='tb-tag-explorer-explore']")).Click();

                //game.TubebuddyGrade
                //game.TubebuddyScore
                //game.TubebuddySearchVolume
                //game.TubebuddyCompetitionScore
                //game.TubebuddyOptimizationScore
                //game.TubebuddyNumberOfVideos
                //game.TubebuddySearchesPerMonth
                //game.TubebuddyAverageViews
                //game.TubebuddyTargetViews
                //game.TubebuddyMyAverageViews
                //game.TubebuddyRelatedSearches
                game.Keyword = game.Title;

                foreach (var relatedSearch in game.TubebuddyRelatedSearches)
                {
                    var relatedGame = new Game();
                    relatedGame.Title = game.Title;
                    relatedGame.Keyword = game.Title + relatedSearch;
                    relatedGame.DateChecked = game.DateChecked;
                    relatedGame.DateReleased = game.DateReleased;
                    relatedGame.Description = game.Description;
                    relatedGame.GameUrl = game.GameUrl;
                    relatedGame.Genre = game.Genre;
                    relatedGame.Platform = game.Platform;
                    relatedGame.Price = game.Price;
                    relatedGame.Score = game.Score;
                    relatedGame.Site = game.Site;
                    relatedGame.ThumbnailUrl = game.ThumbnailUrl;
                    relatedGame.Type = game.Type;

                    searchbox.SendKeys(relatedGame.Keyword);
                    _driver.FindElement(By.XPath("//button[@id='tb-tag-explorer-explore']")).Click();

                    //relatedGame.TubebuddyGrade
                    //relatedGame.TubebuddyScore
                    //relatedGame.TubebuddySearchVolume
                    //relatedGame.TubebuddyCompetitionScore
                    //relatedGame.TubebuddyOptimizationScore
                    //relatedGame.TubebuddyNumberOfVideos
                    //relatedGame.TubebuddySearchesPerMonth
                    //relatedGame.TubebuddyAverageViews
                    //relatedGame.TubebuddyTargetViews
                    //relatedGame.TubebuddyMyAverageViews
                    //relatedGame.TubebuddyRelatedSearches

                    newGames.Add(relatedGame);
                }
            }

            _games.AddRange(newGames);

            return _games;
        }
    }
}