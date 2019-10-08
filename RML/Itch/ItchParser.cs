using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TubeBuddyScraper.Itch
{
    public class ItchParser
    {
        private string ItchPopularUrl = "https://itch.io/games/tag-horror";
        private string ItchNewAndPopularUrl = "https://itch.io/games/new-and-popular/tag-horror";
        private string ItchRecentUrl = "https://itch.io/games/newest/tag-horror";

        public List<Game> GetGames(ChromeDriver driver)
        {
            var games = new List<Game>();
            games.AddRange(BuildGamesByUrl(driver, "https://itch.io/games/tag-horror", Game.GameType.Popular));
            games.AddRange(BuildGamesByUrl(driver, "https://itch.io/games/new-and-popular/tag-horror", Game.GameType.NewAndPopular));
            games.AddRange(BuildGamesByUrl(driver, "https://itch.io/games/newest/tag-horror", Game.GameType.Recent));

            return games;
        }

        private List<Game> BuildGamesByUrl(ChromeDriver driver, string url, Game.GameType type)
        {
            var games = new List<Game>();
            driver.NavigateToUrl(url);
            var gameCells = driver.FindElements(By.XPath("//div[@class='game_grid_widget browse_game_grid']/div[contains(@class,'game_cell')]"));
            foreach (var gameCell in gameCells)
            {
                var game = new Game();
                
                //var title = driver.FindElement()
                //title
                //description
                game.DateChecked = DateTime.Now;
                game.Genre = Game.GameGenre.Horror;
                game.Site = Game.GameSite.Itch;
                game.Type = type;

                //price
                //gameurl
                //thumb url

            }


            return games;
        }
    }
}

///
/// //div[@class='game_grid_widget browse_game_grid']/div[contains=(@class, 'game_cell')]