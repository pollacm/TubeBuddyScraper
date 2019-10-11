using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TubeBuddyScraper.Games;

namespace TubeBuddyScraper.TubeBuddyAnalyzer
{
    public class Analyzer
    {
        private readonly ChromeDriver _driver;
        private List<Game> _games;
        private readonly DateTime _dateStarted;
        private string ItchPopularUrl = "https://itch.io/games/tag-horror";
        private readonly int SecondsToSleep = 5;
        private readonly List<string> _additionalSearches = new List<string>
        {
            " gameplay",
            " horror game",
            " horror gameplay",
            " game ending"
        };

        public Analyzer(ChromeDriver driver, List<Game> games, DateTime dateStarted)
        {
            _driver = driver;
            _games = games;
            _dateStarted = dateStarted;
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
            var gameRepository = new GameRepository();
            _games = gameRepository.GetGamesNotCompleteForDay(_dateStarted, _games);

            foreach (var game in _games)
            {
                var searchbox = _driver.FindElement(By.XPath("//input[@id='tb-tag-explorer-input']"));
                //include game in first search
                searchbox.Clear();
                searchbox.SendKeys(game.Title + " game");
                _driver.FindElement(By.XPath("//button[@id='tb-tag-explorer-explore']")).Click();

                Thread.Sleep(new TimeSpan(0,0,0, SecondsToSleep));

                game.TubebuddyGrade = _driver.FindElement(By.XPath("//span[@id='tb-tag-explorer-total-score-label']")).Text;
                game.TubebuddyScore = _driver.FindElement(By.XPath("//span[@id='tb-tag-explorer-total-score']")).Text;

                //115-0 - Closer to 0 the better 115-92 - red; 92-69 - orange; 69-46 - yellow; 46-23 light-green; 23-0 - green
                var arrowSelectors = _driver.FindElements(By.CssSelector("svg.highcharts-root g.highcharts-series-group g.highcharts-markers image"));

                game.TubebuddySearchVolume = _driver.FindElement(By.XPath("//span[@id='tb-tag-explorer-search-volume']")).Text;
                game.TubebuddySearchVolumeExact = arrowSelectors[0].GetAttribute("y");

                game.TubebuddyCompetitionScore = _driver.FindElement(By.XPath("//span[@id='tb-tag-weighted-competition']")).Text;
                game.TubebuddyCompetitionScoreExact = arrowSelectors[1].GetAttribute("y");

                game.TubebuddyOptimizationScore = _driver.FindElement(By.XPath("//span[@class='tb-tag-explorer-chart-label tb-tag-explorer-keyword-count']")).Text;
                game.TubebuddyOptimizationScoreExact = arrowSelectors[3].GetAttribute("y");

                game.TubebuddyNumberOfVideos = _driver.FindElement(By.XPath("//span[@class='tb-tag-explorer-chart-label tb-tag-explorer-search-result-count']")).Text;
                game.TubebuddySearchesPerMonth = _driver.FindElement(By.XPath("//span[@id='tb-tag-explorer-search-volume-amount']")).Text;

                var viewDataSelectors = _driver.FindElements(By.CssSelector("svg.highcharts-root g.highcharts-data-labels g text tspan:nth-of-type(2n)"));
                game.TubebuddyAverageViews = viewDataSelectors[0].Text;
                game.TubebuddyTargetViews = viewDataSelectors[1].Text;
                game.TubebuddyMyAverageViews = viewDataSelectors[2].Text;

                var relatedSearches = _driver.FindElements(By.XPath("//div[@class='tb-tag-explorer-related-keyword-tab-youtube-list']/div"));
                foreach (var relatedSearch in relatedSearches)
                {
                    if(!relatedSearch.Text.Contains("No related video"))
                        game.TubebuddyRelatedSearches.Add(relatedSearch.FindElement(By.XPath("./a")).Text.ToLower());
                }

                game.Keyword = game.Title;

                gameRepository.AddGame(game);

                foreach (var relatedSearch in game.TubebuddyRelatedSearches)
                {
                    var relatedGame = new Game();
                    relatedGame.Title = game.Title;
                    relatedGame.Keyword = relatedSearch;
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

                    searchbox.Clear();
                    searchbox.SendKeys(relatedGame.Keyword);
                    _driver.FindElement(By.XPath("//button[@id='tb-tag-explorer-explore']")).Click();

                    Thread.Sleep(new TimeSpan(0, 0, 0, SecondsToSleep));

                    relatedGame.TubebuddyGrade = _driver.FindElement(By.XPath("//span[@id='tb-tag-explorer-total-score-label']")).Text;
                    relatedGame.TubebuddyScore = _driver.FindElement(By.XPath("//span[@id='tb-tag-explorer-total-score']")).Text;
                    //115-0 - Closer to 0 the better 115-92 - red; 92-69 - orange; 69-46 - yellow; 46-23 light-green; 23-0 - green

                    arrowSelectors = _driver.FindElements(By.CssSelector("svg.highcharts-root g.highcharts-series-group g.highcharts-markers image"));

                    relatedGame.TubebuddySearchVolume = _driver.FindElement(By.XPath("//span[@id='tb-tag-explorer-search-volume']")).Text;
                    relatedGame.TubebuddySearchVolumeExact = arrowSelectors[0].GetAttribute("y");

                    relatedGame.TubebuddyCompetitionScore = _driver.FindElement(By.XPath("//span[@id='tb-tag-weighted-competition']")).Text;
                    relatedGame.TubebuddyCompetitionScoreExact = arrowSelectors[2].GetAttribute("y");

                    relatedGame.TubebuddyOptimizationScore = _driver.FindElement(By.XPath("//span[@class='tb-tag-explorer-chart-label tb-tag-explorer-keyword-count']")).Text;
                    relatedGame.TubebuddyOptimizationScoreExact = arrowSelectors[3].GetAttribute("y");

                    relatedGame.TubebuddyNumberOfVideos = _driver.FindElement(By.XPath("//span[@class='tb-tag-explorer-chart-label tb-tag-explorer-search-result-count']")).Text;
                    relatedGame.TubebuddySearchesPerMonth = _driver.FindElement(By.XPath("//span[@id='tb-tag-explorer-search-volume-amount']")).Text;

                    viewDataSelectors = _driver.FindElements(By.CssSelector("svg.highcharts-root g.highcharts-data-labels g text tspan:nth-of-type(2n)"));
                    relatedGame.TubebuddyAverageViews = viewDataSelectors[0].Text;
                    relatedGame.TubebuddyTargetViews = viewDataSelectors[1].Text;
                    relatedGame.TubebuddyMyAverageViews = viewDataSelectors[2].Text;

                    newGames.Add(relatedGame);
                    gameRepository.AddGame(relatedGame);
                }

                foreach (var additionalSearch in _additionalSearches)
                {
                    if (game.TubebuddyRelatedSearches.Contains(additionalSearch.ToLower()))
                        continue;

                    var additionalSearchGame = new Game();
                    additionalSearchGame.Title = game.Title;
                    additionalSearchGame.Keyword = game.Title + additionalSearch;
                    additionalSearchGame.DateChecked = game.DateChecked;
                    additionalSearchGame.DateReleased = game.DateReleased;
                    additionalSearchGame.Description = game.Description;
                    additionalSearchGame.GameUrl = game.GameUrl;
                    additionalSearchGame.Genre = game.Genre;
                    additionalSearchGame.Platform = game.Platform;
                    additionalSearchGame.Price = game.Price;
                    additionalSearchGame.Score = game.Score;
                    additionalSearchGame.Site = game.Site;
                    additionalSearchGame.ThumbnailUrl = game.ThumbnailUrl;
                    additionalSearchGame.Type = game.Type;

                    searchbox.Clear();
                    searchbox.SendKeys(additionalSearchGame.Keyword);
                    _driver.FindElement(By.XPath("//button[@id='tb-tag-explorer-explore']")).Click();

                    Thread.Sleep(new TimeSpan(0, 0, 0, SecondsToSleep));

                    additionalSearchGame.TubebuddyGrade = _driver.FindElement(By.XPath("//span[@id='tb-tag-explorer-total-score-label']")).Text;
                    additionalSearchGame.TubebuddyScore = _driver.FindElement(By.XPath("//span[@id='tb-tag-explorer-total-score']")).Text;
                    //115-0 - Closer to 0 the better 115-92 - red; 92-69 - orange; 69-46 - yellow; 46-23 light-green; 23-0 - green

                    arrowSelectors = _driver.FindElements(By.CssSelector("svg.highcharts-root g.highcharts-series-group g.highcharts-markers image"));

                    additionalSearchGame.TubebuddySearchVolume = _driver.FindElement(By.XPath("//span[@id='tb-tag-explorer-search-volume']")).Text;
                    additionalSearchGame.TubebuddySearchVolumeExact = arrowSelectors[0].GetAttribute("y");

                    additionalSearchGame.TubebuddyCompetitionScore = _driver.FindElement(By.XPath("//span[@id='tb-tag-weighted-competition']")).Text;
                    additionalSearchGame.TubebuddyCompetitionScoreExact = arrowSelectors[2].GetAttribute("y");

                    additionalSearchGame.TubebuddyOptimizationScore = _driver.FindElement(By.XPath("//span[@class='tb-tag-explorer-chart-label tb-tag-explorer-keyword-count']")).Text;
                    additionalSearchGame.TubebuddyOptimizationScoreExact = arrowSelectors[3].GetAttribute("y");

                    additionalSearchGame.TubebuddyNumberOfVideos = _driver.FindElement(By.XPath("//span[@class='tb-tag-explorer-chart-label tb-tag-explorer-search-result-count']")).Text;
                    additionalSearchGame.TubebuddySearchesPerMonth = _driver.FindElement(By.XPath("//span[@id='tb-tag-explorer-search-volume-amount']")).Text;

                    viewDataSelectors = _driver.FindElements(By.CssSelector("svg.highcharts-root g.highcharts-data-labels g text tspan:nth-of-type(2n)"));
                    additionalSearchGame.TubebuddyAverageViews = viewDataSelectors[0].Text;
                    additionalSearchGame.TubebuddyTargetViews = viewDataSelectors[1].Text;
                    additionalSearchGame.TubebuddyMyAverageViews = viewDataSelectors[2].Text;

                    newGames.Add(additionalSearchGame);
                    gameRepository.AddGame(additionalSearchGame);
                }
            }

            _games.AddRange(newGames);

            return _games;
        }
    }
}