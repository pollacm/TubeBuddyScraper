using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using TubeBuddyScraper.GameJolt;
using TubeBuddyScraper.Itch;
using TubeBuddyScraper.Metacritic;

namespace TubeBuddyScraper
{
    internal class Program
    {
        private static readonly int maxGameSize = 30;

        private static void Main(string[] args)
        {
            String pathToProfile = @"C:\Users\cxp6696\ChromeProfiles\User Data";
            //String pathToProfile = @"C:\Users\Owner\ChromeProfiles\User Data";
            string pathToChromedriver = @"C:\Users\cxp6696\source\repos\TubeBuddyScraper\packages\Selenium.WebDriver.ChromeDriver.77.0.3865.4000\driver\win32\chromedriver.exe";
            //string pathToChromedriver = @"C:\Users\Owner\source\repos\TubeBuddyScraper\packages\Selenium.WebDriver.ChromeDriver.77.0.3865.4000\driver\win32\chromedriver.exe";

            ChromeOptions options = new ChromeOptions();
            //options.AddArguments("--load-extension=" +pathToExtension);
            options.AddArguments("user-data-dir=" + pathToProfile);
            //options.AddArguments("profile-directory=Profile 1");
            //options.AddArgument("-no-sandbox");
            Environment.SetEnvironmentVariable("webdriver.chrome.driver", pathToChromedriver);
            var games = new List<Game>();
            ChromeDriver driver = new ChromeDriver(options);

            //finished
            //var itchParser = new ItchParser(driver, maxGameSize);
            //games = itchParser.GetGames();

            //itch popular
            //https://itch.io/games/tag-horror
            //itch new and popular
            //https://itch.io/games/new-and-popular/tag-horror
            //itch most recent
            //https://itch.io/games/newest/tag-horror

            //gamejolt hot (popular)
            //https://gamejolt.com/games/tag-horror
            //gamejolt new (recent)
            //https://gamejolt.com/games/new/tag-horror
            //gamejolt featured (new and popular)
            //https://gamejolt.com/games/featured/tag-horror

            //var gameJoltParser = new GameJoltParser(driver, maxGameSize);
            //games.AddRange(gameJoltParser.GetGames());

            //var metacriticParser = new MetacriticParser(driver, maxGameSize);
            //games.AddRange(metacriticParser.GetGames());

            //metacritic ps4
            //https://www.metacritic.com/browse/games/release-date/new-releases/ps4/date
            //metacritic pc
            //https://www.metacritic.com/browse/games/release-date/new-releases/pc/date
            //metacritic ios
            //https://www.metacritic.com/browse/games/release-date/new-releases/ios/date

            //android new free
            //https://play.google.com/store/apps/new/category/GAME?hl=en_US
            //android new paid
            //https://play.google.com/store/apps/new/category/GAME?hl=en_US


            //tubebuddy analysis
            driver.Navigate().GoToUrl($"https://www.youtube.com");
            driver.FindElement(By.XPath("//button[@id='tb-main-menu']")).Click();
            driver.FindElement(By.XPath("//li[contains(text(),'Keyword Explorer')]")).Click();

            //foreach (var game in games)
            //{

            //}


            var searchbox = driver.FindElement(By.XPath("//input[@id='tb-tag-explorer-input']"));
            searchbox.SendKeys("test");

            driver.FindElement(By.XPath("//button[@id='tb-tag-explorer-explore']")).Click();
        }
    }
}