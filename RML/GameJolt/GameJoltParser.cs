using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TubeBuddyScraper.GameJolt
{
    public class GameJoltParser
    {
        private readonly ChromeDriver _driver;
        private readonly int _maxGameSize;
        private readonly List<Game> _existingGames;
        private string GameJoltPopularUrl = "https://gamejolt.com/games/tag-horror";
        private string GameJoltNewAndPopularUrl = "https://gamejolt.com/games/featured/tag-horror";
        private string GameJoltRecentUrl = "https://gamejolt.com/games/new/tag-horror";

        public GameJoltParser(ChromeDriver driver, int maxGameSize, List<Game> existingGames)
        {
            _driver = driver;
            _maxGameSize = maxGameSize;
            _existingGames = existingGames;
        }

        public List<Game> GetGames()
        {
            var games = new List<Game>();
            games.AddRange(BuildGamesByUrl(GameJoltPopularUrl, Game.GameType.Popular));
            games.AddRange(BuildGamesByUrl(GameJoltNewAndPopularUrl, Game.GameType.NewAndPopular));
            games.AddRange(BuildGamesByUrl(GameJoltRecentUrl, Game.GameType.Recent));

            return games;
        }

        private List<Game> BuildGamesByUrl(string url, Game.GameType type)
        {
            var games = new List<Game>();
            int pageNumber = 1;
            while (games.Count < _maxGameSize)
            {
                _driver.NavigateToUrl(url + "?page=" + pageNumber);
                var gameCells = _driver.FindElements(By.XPath("//div[@class='game-grid-item']"));
                foreach (var gameCell in gameCells)
                {
                    var game = new Game();
                    
                    var title = gameCell.FindElement(By.XPath("./a/div/div[@class='-meta']/div[@class='-title']"));
                    game.Title = title.Text;

                    var gameUrl = gameCell.FindElement(By.XPath("./a")).GetAttribute("href");
                    game.GameUrl = gameUrl;
                    
                    game.Genre = "Horror";
                    game.DateChecked = DateTime.Now.Date;

                    game.Site = Game.GameSite.GameJolt;
                    game.Platform = Game.GameSystem.Online;
                    game.Type = type;

                    var price = gameCell.FindElements(By.XPath("./a/div/div[@class='-meta']/div[contains(@class, '-pricing')]/span/span"));
                    if (price.Any())
                        game.Price = price.First().Text == "FREE" || price.First().Text == "NAME YOUR PRICE" ? "" : price.First().Text;

                    var thumbnail = gameCell.FindElements(By.XPath("./a/div/div[contains(@class, 'game-thumbnail-img')]/div[@class='-inner']/div[@class='-media']/img"));
                    if (thumbnail.Any())
                        game.ThumbnailUrl = thumbnail.First().GetAttribute("src");

                    if (!_existingGames.Any(g => g.Title.ToLower() == game.Title.ToLower()))
                        games.Add(game);

                    if (games.Count >= _maxGameSize)
                    {
                        break;
                    }
                }

                pageNumber++;
            }

            return games;
        }
    }
}