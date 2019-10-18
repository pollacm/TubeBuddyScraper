using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using TubeBuddyScraper.Android;
using TubeBuddyScraper.GameJolt;
using TubeBuddyScraper.Games;
using TubeBuddyScraper.GameWriter;
using TubeBuddyScraper.Itch;
using TubeBuddyScraper.Metacritic;
using TubeBuddyScraper.Slack;
using TubeBuddyScraper.TubeBuddyAnalyzer;

namespace TubeBuddyScraper
{
    internal class Program
    {
        private static readonly int maxGameSize = 40;

        private static void Main(string[] args)
        {
            //string[] dirs = Directory.GetFiles(@"E:\Owner\Videos\Resources\Sounds\aaa Common Sounds");

            //foreach (string dir in dirs)
            //{
            //    var file = Path.GetFileName(dir);

            //    if (file.Contains(' '))
            //    {
            //        file = file.Replace(' ', '-');
            //        var path = Path.GetPathRoot(dir);
            //        System.IO.File.Move(dir, path + file);
            //    }
            //}

            var appStartTime = DateTime.Now.Date;

            String pathToProfile = @"C:\Users\cxp6696\ChromeProfiles\User Data";
            //String pathToProfile = @"C:\Users\Owner\ChromeProfiles\User Data";
            string pathToChromedriver = @"C:\Users\cxp6696\source\repos\TubeBuddyScraper\packages\Selenium.WebDriver.ChromeDriver.77.0.3865.4000\driver\win32\chromedriver.exe";
            //string pathToChromedriver = @"C:\Users\Owner\source\repos\TubeBuddyScraper\packages\Selenium.WebDriver.ChromeDriver.77.0.3865.4000\driver\win32\chromedriver.exe";
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("user-data-dir=" + pathToProfile);
            Environment.SetEnvironmentVariable("webdriver.chrome.driver", pathToChromedriver);

            var games = new List<Game>();

            //TODO: Mark All New as Current where not today

            ChromeDriver driver = new ChromeDriver(options);

            var itchParser = new ItchParser(driver, maxGameSize, games);
            games.AddRange(itchParser.GetGames());

            var gameJoltParser = new GameJoltParser(driver, maxGameSize, games);
            games.AddRange(gameJoltParser.GetGames());

            //var metacriticParser = new MetacriticParser(driver, maxGameSize, games);
            //games.AddRange(metacriticParser.GetGames());

            var androidParser = new AndroidParser(driver, maxGameSize, games);
            games.AddRange(androidParser.GetGames());

            //if (args.Any())
            //{
                var analyzer = new Analyzer(driver, games, appStartTime, false);
                games = analyzer.Analyze();
            //}

            var gameRepository = new GameRepository();
            gameRepository.CleanStaleGamesFromDayAndAppendMarkAsExpiredAndNew(appStartTime, games);

            var writer = new Writer(games);
            writer.WriteGameFile();

            //var expiredGames = gameRepository.GetExpiredGamesForDay(DateTime.Now.Date);
            //if (expiredGames.Any())
            //{
            //    var expiredText = $"**********\nEXPIRED GAMES\n**********\n\n{string.Join("\n",expiredGames)}\n\n\n";
            //    //new SlackClient().PostMessage(expiredText);
            //}
            //var addedGames = gameRepository.GetAddedGamesForDay(DateTime.Now.Date);
            //if (addedGames.Any())
            //{
            //    var addedText = $"**********\nADDED GAMES\n**********\n\n{string.Join("\n", addedGames)}\n\n\n";
            //    //new SlackClient().PostMessage(addedText);
            //}
        }
    }
}