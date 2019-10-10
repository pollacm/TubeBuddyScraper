using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TubeBuddyScraper.Android
{
    public class AndroidParser
    {
        private readonly ChromeDriver _driver;
        private readonly int _maxGameCount;
        private readonly List<Game> _existingGames;
        private string AndroidURL = "https://play.google.com/store/apps/new/category/GAME?hl=en_US";

        public AndroidParser(ChromeDriver driver, int maxGameCount, List<Game> existingGames)
        {
            _driver = driver;
            _maxGameCount = maxGameCount;
            _existingGames = existingGames;
        }

        public List<Game> GetGames()
        {
            var games = new List<Game>();
            games.AddRange(BuildGamesByUrl(AndroidURL, true));
            games.AddRange(BuildGamesByUrl(AndroidURL, false));

            return games;
        }

        private List<Game> BuildGamesByUrl(string url, bool free)
        {
            var games = new List<Game>();
            _driver.NavigateToUrl(url);
            if (free)
            {
                _driver.FindElement(By.XPath("//h2[contains(text(),'Top New Free Games')]")).Click();
            }
            else
            {
                _driver.FindElement(By.XPath("//h2[contains(text(),'Top New Paid Games')]")).Click();
            }
            Thread.Sleep(new TimeSpan(0,0,0,10));
            var gameCells = _driver.FindElements(By.XPath("//html[1]/body[1]/div[1]/div[4]/c-wiz[2]/div[1]/c-wiz[1]/div[1]/c-wiz[1]/c-wiz[1]/c-wiz[1]/div[1]/div[2]/div"));
            foreach (var gameCell in gameCells)
            {
                var game = new Game();
                
                var title = gameCell.FindElement(By.XPath("./c-wiz[1]/div[1]/div[1]/div[2]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/a[1]/div[1]"));
                game.Title = title.Text;

                var gameUrl = gameCell.FindElement(By.XPath("./c-wiz[1]/div[1]/div[1]/div[1]/div[1]/div[1]/a[1]"));
                game.GameUrl = gameUrl.GetAttribute("href");

                game.DateChecked = DateTime.Now;

                game.Site = Game.GameSite.GooglePlay;
                game.Platform = Game.GameSystem.Android;
                game.Type = Game.GameType.Recent;

                var price = gameCell.FindElements(By.XPath("./c-wiz[1]/div[1]/div[1]/div[2]/div[1]/div[1]/div[2]/div[1]/div[2]/div[1]/button[1]/div[1]/span[1]/span[1]"));
                if (price.Any())
                {
                    game.Price = price.First().Text;
                }

                var score = gameCell.FindElements(By.XPath("./c-wiz[1]/div[1]/div[1]/div[2]/div[1]/div[1]/div[2]/div[1]/div[1]/div[1]/div[1]"));
                if (score.Any())
                {
                    var rating = score.First().GetAttribute("aria-label").Split(new string[] { "Rated " }, StringSplitOptions.None)[1].Split(new string[] { " stars out of five stars" }, StringSplitOptions.None)[0];
                
                    game.Score = rating;
                }

                var thumbnail = gameCell.FindElements(By.XPath("./c-wiz/div/div/div[1]/div[1]/span[1]/span/img"));
                if (thumbnail.Any())
                    game.ThumbnailUrl = thumbnail.First().GetAttribute("data-src");

                if(!_existingGames.Any(g => g.Title == game.Title))
                    games.Add(game);

                if (games.Count >= _maxGameCount)
                {
                    break;
                }
            }

            return games;
        }
    }
}