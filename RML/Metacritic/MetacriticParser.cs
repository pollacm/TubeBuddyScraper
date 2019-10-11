using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TubeBuddyScraper.Metacritic
{
    public class MetacriticParser
    {
        private readonly ChromeDriver _driver;
        private readonly int _maxGameSize;
        private readonly List<Game> _existingGames;
        private string MetacriticPS4Url = "https://www.metacritic.com/browse/games/release-date/new-releases/ps4/date";
        private string MetacriticPCUrl = "https://www.metacritic.com/browse/games/release-date/new-releases/pc/date";
        private string MetacriticIOSUrl = "https://www.metacritic.com/browse/games/release-date/new-releases/ios/date";

        public MetacriticParser(ChromeDriver driver, int maxGameSize, List<Game> existingGames)
        {
            _driver = driver;
            _maxGameSize = maxGameSize;
            _existingGames = existingGames;
        }

        public List<Game> GetGames()
        {
            var games = new List<Game>();
            games.AddRange(BuildGamesByUrl(MetacriticPS4Url, Game.GameSystem.PS4));
            games.AddRange(BuildGamesByUrl(MetacriticPCUrl, Game.GameSystem.PC));
            games.AddRange(BuildGamesByUrl(MetacriticIOSUrl, Game.GameSystem.IOS));

            return games;
        }

        private List<Game> BuildGamesByUrl(string url, Game.GameSystem platform)
        {
            var games = new List<Game>();

            _driver.NavigateToUrl(url);
            var gameCells = _driver.FindElements(By.XPath("//div[@class='product_condensed']/ol/li"));
            foreach (var gameCell in gameCells)
            {
                var game = new Game();
                
                var title = gameCell.FindElement(By.XPath("./div[@class='product_wrap']/div[contains(@class,'product_title')]/a"));
                game.Title = title.Text;
                game.GameUrl = title.GetAttribute("href");

                game.DateChecked = DateTime.Now.Date;

                var releaseDate = gameCell.FindElement(By.XPath("./div[@class='product_wrap']/div[contains(@class, 'condensed_stats')]/ul/li[contains(@class, 'release_date')]/span[@class='data']"));
                game.DateReleased = releaseDate.Text;

                var score = gameCell.FindElement(By.XPath("./div[contains(@class, 'product_wrap')]/div[contains(@class, 'product_score')]/div"));
                game.Score = score.Text == "tbd" ? "" : score.Text;

                game.Site = Game.GameSite.Metacritic;
                game.Platform = platform;

                if (!_existingGames.Any(g => g.Title.ToLower() == game.Title.ToLower()))
                    games.Add(game);

                if (games.Count >= _maxGameSize)
                    break;

                if (DateTime.Compare(DateTime.Now.AddDays(-30), DateTime.Parse(game.DateReleased)) == 1)
                {
                    break;
                }
            }

            return games;
        }
    }
}