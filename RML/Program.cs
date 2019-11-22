using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using TubeBuddyScraper;

namespace YoutubeSubscriberManager
{
    internal class Program
    {
        //less than a minute watches
        static List<string> blacklist = new List<string>
        {
            // = 30 second watch
            //* = need to watch a vid and see if they respond
            "TOMXGAMERS".ToLower(), //
            "KHADIJA PRODUCTIONS Tutorials".ToLower(),//
            "I Am Lif3ofdreads".ToLower(),//
            "Xander Zone".ToLower(),//
            "Criminal 2020".ToLower(),
            "GameHunter".ToLower(),
            "Aryan Satya".ToLower(),
            "Dude Gamer".ToLower(),
            "Teele Loves Jobu".ToLower(),
            "PS5 Gamer".ToLower(),
            "CHILLSCISSORS".ToLower(),//
            "Timothy B. Salinas".ToLower(),
            "DeGRA".ToLower(),
            "Zio bugio".ToLower(),
            "Mister Omega".ToLower(),
            "JeremiahGR".ToLower(),
            "The Nintendo Network".ToLower(),
            "SurgeTV".ToLower(),//*
            "CoryJT".ToLower(),//
            "Dan Hundred Bankss Entertainment".ToLower(),
            "Lady Judged".ToLower(),
            "Noizey Plays".ToLower(),
            "Zerasino Reboot".ToLower(),//*
            "Optic Ninja".ToLower(),
            "CSN_CityGirl".ToLower(),
            "Tat Test Dummies".ToLower(),
            "I'm Norman!".ToLower(),
            "Predator Zone Crazy".ToLower(),
            "Orjane Cristobal".ToLower(),
        };


        static List<string> whitelist = new List<string>
        {
            "milada มิลา".ToLower(), //16/32
            "Food Idea by Tuhin".ToLower(), //4.5/32
            "MT GAMING5".ToLower(), //15/32,
            "Game Boys".ToLower(), //8/32
            "SK Gowrob".ToLower(),
            "BROWEN".ToLower(), //10/32
            "Ranscan KNRT".ToLower(),//10/10

        };
        private static void Main(string[] args)
        {

            var appStartTime = DateTime.Now.Date;
            
            var acceptableWatchTimes = new List<string>
            {
                "minutes",
                "hours",
                "days",
                "minute",
                "hour",
                "day",
                "1 week",
                "2 weeks",
                "3 weeks"
            };
            var rowsToIncrementOnSubPage = 4;

            String pathToProfile = @"C:\Users\cxp6696\ChromeProfiles\User Data";
            //String pathToProfile = @"C:\Users\Owner\ChromeProfiles\User Data";
            string pathToChromedriver = @"C:\Users\cxp6696\source\repos\TubeBuddyScraper\packages\Selenium.WebDriver.ChromeDriver.77.0.3865.4000\driver\win32\chromedriver.exe";
            //string pathToChromedriver = @"C:\Users\Owner\source\repos\TubeBuddyScraper\packages\Selenium.WebDriver.ChromeDriver.77.0.3865.4000\driver\win32\chromedriver.exe";
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("user-data-dir=" + pathToProfile);
            Environment.SetEnvironmentVariable("webdriver.chrome.driver", pathToChromedriver);

            var subscribers = new List<Subscriber.Subscriber>();
            ChromeDriver driver = new ChromeDriver(options);
            driver.NavigateToUrl("https:/www.youtube.com/feed/subscriptions");
            Thread.Sleep(3000);

            for (int i = 0; i < rowsToIncrementOnSubPage; i++)
            {
                ScrollToBottom(driver);
                Thread.Sleep(3000);
            }

            var videos = driver.FindElementsByXPath("//ytd-grid-video-renderer");
            foreach (var video in videos)
            {
                var subscriberName = video.FindElement(By.XPath("./div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/ytd-channel-name")).Text;
                var subscriber = subscribers.SingleOrDefault(s => s.Name == subscriberName);
                if (subscriber == null)
                {
                    subscriber = new Subscriber.Subscriber();
                    subscriber.Name = subscriberName;
                    var watched = video.FindElements(By.XPath("./div[1]/ytd-thumbnail[1]/a[1]/div[1]/ytd-thumbnail-overlay-resume-playback-renderer")).Any();
                    if (watched)
                        subscriber.Watches++;
                    var currentViewCount = video.FindElement(By.XPath("./div[1]//div[1]//div[1]//div[1]//div[1]//div[2]//span[1]")).Text.Split(new string[] {" view"}, StringSplitOptions.None)[0].Split(new string[] { " watching" }, StringSplitOptions.None)[0];
                    subscriber.ViewCounts.Add(GetIntegerViews(currentViewCount));
                    subscriber.Videos = 1;
                    subscriber.AverageViewCount = subscriber.ViewCounts.Sum(Convert.ToInt32) == 0 ? 0 : subscriber.ViewCounts.Sum(Convert.ToInt32) / subscriber.Videos;
                    
                    subscribers.Add(subscriber);
                }
                else
                {
                    var watched = video.FindElements(By.XPath("./div[1]/ytd-thumbnail[1]/a[1]/div[1]/ytd-thumbnail-overlay-resume-playback-renderer")).Any();
                    if (watched)
                        subscriber.Watches++;
                    var currentViewCount = video.FindElement(By.XPath("./div[1]//div[1]//div[1]//div[1]//div[1]//div[2]//span[1]")).Text.Split(new string[] { " view" }, StringSplitOptions.None)[0].Split(new string[] { " watching" }, StringSplitOptions.None)[0];
                    subscriber.ViewCounts.Add(GetIntegerViews(currentViewCount));
                    subscriber.Videos++;
                    subscriber.AverageViewCount = subscriber.ViewCounts.Sum(Convert.ToInt32) == 0 ? 0 : subscriber.ViewCounts.Sum(Convert.ToInt32) / subscriber.Videos;
                }
            }

            driver.NavigateToUrl("https:/studio.youtube.com/channel/UCUDTfpBksfE4KqLYjG9u00g/comments/inbox?utm_campaign=upgrade&utm_medium=redirect&utm_source=%2Fcomments&filter=%5B%5D");
            SelectElement selectBox = new SelectElement(driver.FindElementByXPath("//ytcp-comments-filter[@id='filter-bar']//select[@class='tb-comment-filter-studio-select-auto-load tb-comment-filter-studio-select']"));
            selectBox.SelectByText("100 results");
            var button = driver.FindElementByXPath("//ytcp-comments-filter[@id='filter-bar']//button[@class='tb-btn tb-btn-grey tb-comment-filter-studio-go'][contains(text(),'Go')]");
            button.Click();
            Thread.Sleep(10000);

            var comments = driver.FindElementsByXPath("//body//ytcp-comment-thread");
            foreach (var comment in comments)
            {
                if (comment.FindElements(By.XPath("./ytcp-comment[@id='comment']//yt-formatted-string[@class='author-text style-scope ytcp-comment']")).Count == 1)
                {
                    var commenterName = comment.FindElement(By.XPath("./ytcp-comment[@id='comment']//yt-formatted-string[@class='author-text style-scope ytcp-comment']")).Text;
                    var commenter = subscribers.SingleOrDefault(s => s.Name == commenterName);
                    if (commenter != null)
                    {
                        var watchTime = comment.FindElement(By.XPath("./ytcp-comment[1]/div[1]/div[1]/div[2]/div[1]/yt-formatted-string[1]")).Text;
                        foreach (var acceptableWatchTime in acceptableWatchTimes)
                        {
                            if (watchTime.Contains(acceptableWatchTime))
                            {
                                commenter.CommentedLately = true;
                                break;
                            }
                        }
                    }
                }
            }

            driver.NavigateToUrl("https:/www.youtube.com/feed/subscriptions");
            for (int i = 0; i < rowsToIncrementOnSubPage; i++)
            {
                ScrollToBottom(driver);
                Thread.Sleep(3000);
            }

            //Current supporters
            videos = driver.FindElementsByXPath("//ytd-grid-video-renderer");
            var currentElement = 0;
            foreach (var video in videos)
            {
                var subscriberName = video.FindElement(By.XPath("./div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/ytd-channel-name")).Text;
                var subscriber = subscribers.SingleOrDefault(s => s.Name == subscriberName);
                if (subscriber != null)
                {
                    if (blacklist.Contains(subscriberName.ToLower()) || (!subscriber.CommentedLately || subscriber.Watches > 2))
                    {
                        RemoveElement(driver, currentElement);
                    }
                    else
                    {
                        StampElement(driver, subscriberName, currentElement);
                        currentElement++;
                    }
                }
            }

            //blacklist or haven't returned watch
            driver.NavigateToUrl("https:/www.youtube.com/feed/subscriptions");
            for (int i = 0; i < rowsToIncrementOnSubPage; i++)
            {
                ScrollToBottom(driver);
                Thread.Sleep(3000);
            }

            videos = driver.FindElementsByXPath("//ytd-grid-video-renderer");
            currentElement = 0;
            foreach (var video in videos)
            {
                var subscriberName = video.FindElement(By.XPath("./div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/ytd-channel-name")).Text;
                var subscriber = subscribers.SingleOrDefault(s => s.Name == subscriberName);
                if (subscriber != null)
                {
                    if (blacklist.Contains(subscriberName.ToLower()) || subscriber.Watches > 0)
                    {
                        RemoveElement(driver, currentElement);
                    }
                    else
                    {
                        StampElement(driver, subscriberName, currentElement);
                        currentElement++;
                    }
                }
            }

            //non-supporters
            driver.NavigateToUrl("https:/www.youtube.com/feed/subscriptions");
            for (int i = 0; i < rowsToIncrementOnSubPage; i++)
            {
                ScrollToBottom(driver);
                Thread.Sleep(3000);
            }

            videos = driver.FindElementsByXPath("//ytd-grid-video-renderer");
            currentElement = 0;
            foreach (var video in videos)
            {
                var subscriberName = video.FindElement(By.XPath("./div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/ytd-channel-name")).Text;
                var subscriber = subscribers.SingleOrDefault(s => s.Name == subscriberName);
                if (subscriber != null)
                {
                    if (subscriber.CommentedLately)
                    {
                        RemoveElement(driver, currentElement);
                    }
                    else
                    {
                        StampElement(driver, subscriberName, currentElement);
                        currentElement++;
                    }
                }
            }

            //blacklist
            driver.NavigateToUrl("https:/www.youtube.com/feed/subscriptions");
            for (int i = 0; i < rowsToIncrementOnSubPage; i++)
            {
                ScrollToBottom(driver);
                Thread.Sleep(3000);
            }

            videos = driver.FindElementsByXPath("//ytd-grid-video-renderer");
            currentElement = 0;
            foreach (var video in videos)
            {
                var subscriberName = video.FindElement(By.XPath("./div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/ytd-channel-name")).Text;
                var subscriber = subscribers.SingleOrDefault(s => s.Name == subscriberName);
                if (subscriber != null)
                {
                    if (!blacklist.Contains(subscriberName.ToLower()))
                    {
                        RemoveElement(driver, currentElement);
                    }
                    else
                    {
                        StampElement(driver, subscriberName, currentElement);
                        currentElement++;
                    }
                }
            }

            //white list
            driver.NavigateToUrl("https:/www.youtube.com/feed/subscriptions");
            for (int i = 0; i < rowsToIncrementOnSubPage; i++)
            {
                ScrollToBottom(driver);
                Thread.Sleep(3000);
            }

            videos = driver.FindElementsByXPath("//ytd-grid-video-renderer");
            currentElement = 0;
            foreach (var video in videos)
            {
                var subscriberName = video.FindElement(By.XPath("./div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/ytd-channel-name")).Text;
                var subscriber = subscribers.SingleOrDefault(s => s.Name == subscriberName);
                if (subscriber != null)
                {
                    if (!whitelist.Contains(subscriberName.ToLower()))
                    {
                        RemoveElement(driver, currentElement);
                    }
                    else
                    {
                        StampElement(driver, subscriberName, currentElement);
                        currentElement++;
                    }
                }
            }
            
            //higher views
            driver.NavigateToUrl("https:/www.youtube.com/feed/subscriptions");
            for (int i = 0; i < rowsToIncrementOnSubPage; i++)
            {
                ScrollToBottom(driver);
                Thread.Sleep(3000);
            }

            videos = driver.FindElementsByXPath("//ytd-grid-video-renderer");
            currentElement = 0;
            foreach (var video in videos)
            {
                var subscriberName = video.FindElement(By.XPath("./div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/ytd-channel-name")).Text;
                var subscriber = subscribers.SingleOrDefault(s => s.Name == subscriberName);
                if (subscriber != null)
                {
                    if (subscriber.AverageViewCount < 50)
                    {
                        RemoveElement(driver, currentElement);
                    }
                    else
                    {
                        StampElement(driver, subscriberName, currentElement);
                        currentElement++;
                    }
                }
            }

            var x = 1;
        }

        private static void RemoveElement(ChromeDriver driver, int index)
        {
            var jse = (IJavaScriptExecutor)driver;
            jse.ExecuteScript($"return document.getElementsByTagName('ytd-grid-video-renderer')[{index}].remove();");
        }

        private static void StampElement(ChromeDriver driver, string subscriberName, int index)
        {
            var jse = (IJavaScriptExecutor)driver;

            if(blacklist.Contains(subscriberName.ToLower()))
                jse.ExecuteScript($"return document.getElementsByTagName('ytd-grid-video-renderer')[{index}].style.border = \"5px solid red\";");
            if (whitelist.Contains(subscriberName.ToLower()))
                jse.ExecuteScript($"return document.getElementsByTagName('ytd-grid-video-renderer')[{index}].style.border = \"5px solid green\";");
        }

        private static void ScrollToBottom(ChromeDriver driver)
        {
            var jse = (IJavaScriptExecutor)driver;
            jse.ExecuteScript("scroll(0, 100000);");
        }

        private static double GetIntegerViews(string views)
        {
            if (views == "No"|| views.Contains("Premiere"))
                return 0;
            var integerViews = double.Parse(views.Split(new string[] {"K"}, StringSplitOptions.None)[0].Split(new string[] { "M" }, StringSplitOptions.None)[0]);
            if (views.Contains("K"))
            {
                integerViews *= 1000;
            }
            if (views.Contains("M"))
            {
                integerViews *= 1000000;
            }

            return integerViews;
        }
        
    }
}