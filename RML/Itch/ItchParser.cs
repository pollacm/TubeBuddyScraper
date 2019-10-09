using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TubeBuddyScraper.Itch
{
    public class ItchParser
    {
        private readonly ChromeDriver _driver;
        private readonly int _maxGameSize;
        private string ItchPopularUrl = "https://itch.io/games/tag-horror";
        private string ItchNewAndPopularUrl = "https://itch.io/games/new-and-popular/tag-horror";
        private string ItchRecentUrl = "https://itch.io/games/newest/tag-horror";

        public ItchParser(ChromeDriver driver, int maxGameSize)
        {
            _driver = driver;
            _maxGameSize = maxGameSize;
        }

        public List<Game> GetGames()
        {
            var games = new List<Game>();
            games.AddRange(BuildGamesByUrl(ItchPopularUrl, Game.GameType.Popular));
            games.AddRange(BuildGamesByUrl(ItchNewAndPopularUrl, Game.GameType.NewAndPopular));
            games.AddRange(BuildGamesByUrl(ItchRecentUrl, Game.GameType.Recent));

            return games;
        }

        private List<Game> BuildGamesByUrl(string url, Game.GameType type)
        {
            var games = new List<Game>();
            int pageNumber = 1;
            while (games.Count < _maxGameSize)
            {
                _driver.NavigateToUrl(url + "?page=" + pageNumber);
                var gameCells = _driver.FindElements(By.XPath("//div[@class='game_grid_widget browse_game_grid']/div[contains(@class,'game_cell')]"));
                foreach (var gameCell in gameCells)
                {
                    var game = new Game();
                    
                    var title = gameCell.FindElement(By.XPath("./div[@class='game_cell_data']/div[@class='game_title']/a"));
                    game.Title = title.Text;
                    game.GameUrl = title.GetAttribute("href");
                    
                    var description = gameCell.FindElements(By.XPath("./div[@class='game_cell_data']/div[@class='game_text']"));
                    if (description.Any())
                        game.Description = description.First().Text;

                    game.Genre = "Horror";
                    var genre = gameCell.FindElements(By.XPath("./div[@class='game_cell_data']/div[@class='game_genre']"));
                    if (genre.Any())
                        game.Genre += $"; {genre.First().Text}";
                    game.DateChecked = DateTime.Now;

                    game.Site = Game.GameSite.Itch;
                    game.Platform = Game.GameSystem.Online;
                    game.Type = type;

                    var price = gameCell.FindElements(By.XPath("./div[@class='game_cell_data']/div[@class='game_title']/a/div[@class='price_value']"));
                    if (price.Any())
                        game.Price = price.First().Text;

                    var thumbnail = gameCell.FindElements(By.XPath("./a[contains(@class,'thumb_link')]/div[@class='game_thumb']"));
                    if (thumbnail.Any())
                        game.ThumbnailUrl = thumbnail.First().GetAttribute("data-background_image");
                    games.Add(game);
                }

                pageNumber++;
            }

            return games;
        }
    }
}