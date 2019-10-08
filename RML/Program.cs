using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using TubeBuddyScraper.PlayerComparer;
using TubeBuddyScraper.PowerRankings;
using TubeBuddyScraper.Rankings;
using TubeBuddyScraper.Returners;
using TubeBuddyScraper.RmlPlayer;
using TubeBuddyScraper.SitePlayer;
using TubeBuddyScraper.Standings;
using TubeBuddyScraper.Teams;
using TubeBuddyScraper.Trophies;
using TubeBuddyScraper.Weeks;

namespace TubeBuddyScraper
{
    internal class Program
    {
        private static readonly int year = 2018;
        private static readonly int week = 11;

        private static void Main(string[] args)
        {
            var options = new ChromeOptions();
            //options.AddArgument("--headless");
            var driver = new ChromeDriver(options);

            //new ReturnerBuilder(driver).GenerateReturners();

            //login

            //itch popular
            //https://itch.io/games/tag-horror
            //itch new and popular
            //https://itch.io/games/new-and-popular/tag-horror
            //itch most recent
            //https://itch.io/games/newest/tag-horror

            //gamejolt hot
            //https://gamejolt.com/games/tag-horror
            //gamejolt new
            //https://gamejolt.com/games/new/tag-horror
            //gamejolt featured
            //https://gamejolt.com/games/featured/tag-horror

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




            driver.Navigate().GoToUrl($"http://games.espn.com/ffl/standings?leagueId=127291&seasonId={year}");
            driver.SwitchTo().Frame("disneyid-iframe");
            var userNameBox = driver.FindElement(By.CssSelector("div.field-username-email input"));
            userNameBox.SendKeys(Keys.ArrowDown);
            userNameBox.SendKeys("pollacm@gmail.com");

            var passwordBox = driver.FindElement(By.CssSelector("div.field-password input"));
            passwordBox.SendKeys(Keys.ArrowDown);
            passwordBox.SendKeys("grip1334");
            passwordBox.SendKeys(Keys.Enter);
            Thread.Sleep(2000);

            driver.Manage().Timeouts().PageLoad = new TimeSpan(0, 0, 0, 5);

            //var rmlPlayerBuilder = new RmlPlayerBuilder(driver, 2018);
            //var rmlPlayers = rmlPlayerBuilder.BuildRmlPlayers();
            //var rmlPlayerRepository = new RmlPlayerRepository();

            //rmlPlayerRepository.RefreshRmlPlayers(rmlPlayers);

            //var rmlPlayers = rmlPlayerRepository.GetRmlPlayers();

            //var sitePlayerBuilder = new SitePlayerBuilder(driver);
            //var sitePlayers = sitePlayerBuilder.BuildSitePlayers();
            //sitePlayerRepository.RefreshSitePlayers(sitePlayers);

            //var sitePlayerRepository = new SitePlayerRepository();
            //var sitePlayers = sitePlayerRepository.GetSitePlayers();

            //Console.WriteLine("Writing returner File:");
            //new PrintPlayerComparerService(sitePlayers.OrderBy(p => p.Name).ToList(), rmlPlayers).WritePlayerComparerFile();
            //Console.WriteLine("Writing returner File COMPLETE");

            //postDraftRankings
            //var postDraftRankingGenerator = new PostDraftRankingGenerator(driver);
            //postDraftRankingGenerator.GenerateRankings();

            //driver.Manage().Timeouts().PageLoad = new TimeSpan(0, 0, 0, 5);

            //get teams
            //var teamBuilder = new TeamBuilder(driver, year);
            //var teamRepository = new TeamRepository();

            //var teams = teamBuilder.BuildTeams();
            //teamRepository.RefreshTeams(teams);
            //var teams = teamRepository.GetTeams();

            driver.WaitUntilElementExists(By.CssSelector("table.tableBody"));

            //OPS
            driver.Navigate().GoToUrl($"http://games.espn.com/ffl/freeagency?leagueId=127291&seasonId={year}");
            driver.WaitUntilElementExists(By.Id("playerTableContainerDiv"));

            var opLink = driver.FindElement(By.XPath("//ul[@class='filterToolsOptionSet']/li/a[contains(.,'OP')]"));
            opLink.Click();

            Thread.Sleep(2000);

            var onRosterLink = driver.FindElement(By.XPath("//ul[@class='filterToolsOptionSet']/li/a[contains(.,'On Rosters')]"));
            onRosterLink.Click();

            Thread.Sleep(2000);

            var lastLink = driver.FindElement(By.XPath("//tr[contains(@class, 'playerTableBgRowSubhead')]/td/a[contains(.,'LAST')]"));
            lastLink.Click();

            Thread.Sleep(5000);

            var opRows = driver.FindElements(By.XPath("//tr[contains(@class, 'pncPlayerRow')]/td[3][not(contains(.,'FA'))]/parent::tr"));
            var opsOfTheWeek = new List<PlayerOfTheWeek>();
            var topPoints = 0m;
            foreach (var opRow in opRows)
            {
                var rowPoints = decimal.Parse(opRow.FindElement(By.XPath("./td[contains(@class, 'sortedCell')]")).Text);
                if (rowPoints >= topPoints)
                {
                    topPoints = rowPoints;
                }
                else
                {
                    break;
                }

                var opOfTheWeek = new PlayerOfTheWeek();
                opOfTheWeek.Name = opRow.FindElement(By.XPath("./td[@class='playertablePlayerName']/a")).Text;
                opOfTheWeek.Team = opRow.FindElement(By.XPath("./td[3]/a")).GetAttribute("title");
                opOfTheWeek.TeamAbbreviation = opRow.FindElement(By.XPath("./td[3]/a")).Text;
                opOfTheWeek.PlayerId = int.Parse(opRow.FindElement(By.XPath("./td[@class='playertablePlayerName']/a")).GetAttribute("playerid"));
                opOfTheWeek.Points = rowPoints;

                opsOfTheWeek.Add(opOfTheWeek);
            }

            //DPS
            var dpLink = driver.FindElement(By.XPath("//ul[@class='filterToolsOptionSet']/li/a[contains(.,'DP')]"));
            dpLink.Click();

            Thread.Sleep(2000);

            var dpRows = driver.FindElements(By.XPath("//tr[contains(@class, 'pncPlayerRow')]/td[3][not(contains(.,'FA'))]/parent::tr"));
            var dpsOfTheWeek = new List<PlayerOfTheWeek>();
            topPoints = 0m;
            foreach (var dpRow in dpRows)
            {
                var rowPoints = decimal.Parse(dpRow.FindElement(By.XPath("./td[contains(@class, 'sortedCell')]")).Text);
                if (rowPoints >= topPoints)
                {
                    topPoints = rowPoints;
                }
                else
                {
                    break;
                }

                var dpOfTheWeek = new PlayerOfTheWeek();
                dpOfTheWeek.Name = dpRow.FindElement(By.XPath("./td[@class='playertablePlayerName']/a")).Text;
                dpOfTheWeek.Team = dpRow.FindElement(By.XPath("./td[3]/a")).GetAttribute("title");
                dpOfTheWeek.TeamAbbreviation = dpRow.FindElement(By.XPath("./td[3]/a")).Text;
                dpOfTheWeek.PlayerId = int.Parse(dpRow.FindElement(By.XPath("./td[@class='playertablePlayerName']/a")).GetAttribute("playerid"));
                dpOfTheWeek.Points = rowPoints;

                dpsOfTheWeek.Add(dpOfTheWeek);
            }

            var weekRepository = new WeekRepository();
            var powerRankings = GetPowerRankings(driver, weekRepository);
            var currentWeek = weekRepository.GetWeek(driver, week, year);

            var weeklyPayoutTeams = string.Empty;
            for (var i = 1; i <= week; i++)
            {
                var weekForWeeklyPayouts = weekRepository.GetWeek(driver, i, year);
                var teamsForWeeklyPayouts = weekForWeeklyPayouts.Scores.Select(s => s.AwayTeam).ToList();
                teamsForWeeklyPayouts.AddRange(weekForWeeklyPayouts.Scores.Select(s => s.HomeTeam));
                weeklyPayoutTeams += i + ". " + teamsForWeeklyPayouts.OrderByDescending(t => t.TeamPoints).First().TeamName.ToUpper() + @"
                ";
            }
            
            List<ITrophy> trophies = new List<ITrophy>();
            //Console.WriteLine("Do you want to assign trophies? (Y/N[default])");
            //var assignTrophiesInput = Console.ReadLine();

            //if (assignTrophiesInput == "Y")
            //{
                trophies = AssignTrophies(currentWeek, opsOfTheWeek, dpsOfTheWeek, driver);
            //}

            CreateLeaguePage(powerRankings, weeklyPayoutTeams, trophies, currentWeek, week);
            var x = 1;
        }

        //TODO: assign old league message to dropbox folder under the previous week, new week under the new week
        private static void CreateLeaguePage(List<PowerRanking> powerRankings, string weeklyPayoutTeams, List<ITrophy> trophies, Week currentWeek, int i)
        {
            var leagueMessage = @"[b]<update> IN WEEK " + week + @"[/b]!!!!!

[image]<update>[/image]

[b]R.M.L. WEEK " + week + @" - <update>[/b]

<update>

[b]WEEK " + week + @" RECAP[/b]

" + GetRecapInfo(currentWeek) + @"

  [b]" + BuildTrophies(trophies) + @"[/b]
  [i][b]DISCLAIMER:[/b] For the [b]POWER RANKINGS[/b], I use an algorithm to calculate how likely you are to win against another team at any given time.It's not my personal opinion of the teams. The algorithm is basically a total of your points scored over the last 3 weeks, plus 50 points for each win over that same time period. [/i]
  
  [b]WEEK " + week + @" POWER RANKINGS[/b]

  " + GeneratePowerRankings(powerRankings) + @"


  [b]WEEKLY PAYOUTS[/b]
  [b]
  " + weeklyPayoutTeams + @"
  [/b]
";
        }

        private static string BuildTrophies(List<ITrophy> trophies)
        {
            var trophyWriter = new TrophyWriter(trophies);
            return trophyWriter.GetTrophyTextForLeaguePage();
        }

        private static string GeneratePowerRankings(List<PowerRanking> powerRankings)
        {
            var powerRankingString = string.Empty;
            //1.  [b]Double Trouble(+0)[/b]

            foreach (var ranking in powerRankings.OrderBy(p => p.CurrentPowerRanking))
            {
                powerRankingString += ranking.CurrentPowerRanking + ".  [b]" + ranking.TeamName + "[/b] [i][b](" + (ranking.CurrentPowerRanking >= ranking.PreviousPowerRanking
                                          ? "+" + (ranking.CurrentPowerRanking - ranking.PreviousPowerRanking)
                                          : "-" + (ranking.PreviousPowerRanking - ranking.CurrentPowerRanking)) + ")[/b][/i]";
                powerRankingString += @"
                ";
            }

            return powerRankingString;
        }

        private static string BuildPlayersOfTheWeek(List<PlayerOfTheWeek> playersOfTheWeek)
        {
            var playersOfTheWeekString = string.Empty;
            foreach (var player in playersOfTheWeek)
            {
                playersOfTheWeekString += @"
                [b] [player#" + player.PlayerId + "]" + player.Name.ToUpper() + "[/player] (" + player.Team.ToUpper() + ") - " + player.Points + @" POINTS [/b]
            
            [image]<update>[/image]
            
            ";
            }

            return playersOfTheWeekString;
        }

        private static string GetRecapInfo(Week currentWeek)
        {
            var recapString = string.Empty;

            foreach (var score in currentWeek.Scores)
            {
                if (score.HomeTeam.Win)
                    recapString += $"[b]{score.HomeTeam.TeamName.ToUpper()}[/b] <update> [b]{score.AwayTeam.TeamName.ToUpper()}[/b]!!!!!";
                else
                    recapString += $"[b]{score.AwayTeam.TeamName.ToUpper()}[/b] <update> [b]{score.HomeTeam.TeamName.ToUpper()}[/b]!!!!!";

                recapString += @"

                ";
            }

            return recapString;
        }

        private static List<ITrophy> AssignTrophies(Week currentWeek, List<PlayerOfTheWeek> opsOfTheWeek, List<PlayerOfTheWeek> dpsOfTheWeek, ChromeDriver driver)
        {
            var trophies = new List<ITrophy>();

            //500 club
            var winners = currentWeek.Scores.Where(s => s.AwayTeam.TeamPoints >= 500 && s.AwayTeam.TeamPoints < 600).Select(s => s.AwayTeam).ToList();
            winners.AddRange(currentWeek.Scores.Where(s => s.HomeTeam.TeamPoints >= 500 && s.HomeTeam.TeamPoints < 600).Select(s => s.HomeTeam));

            var trophyAssigner = new TrophyAssigner(driver, year);
            foreach (var team in winners.OrderByDescending(t => t.TeamPoints))
            {
                trophies.Add(trophyAssigner.AssignTrophy(currentWeek, team, new FiveHundredClubTrophy(team, string.Empty)));
            }

            //600 club
            winners = currentWeek.Scores.Where(s => s.AwayTeam.TeamPoints >= 600 && s.AwayTeam.TeamPoints < 700).Select(s => s.AwayTeam).ToList();
            winners.AddRange(currentWeek.Scores.Where(s => s.HomeTeam.TeamPoints >= 600 && s.HomeTeam.TeamPoints < 700).Select(s => s.HomeTeam));
            foreach (var team in winners.OrderByDescending(t => t.TeamPoints))
            {
                trophies.Add(trophyAssigner.AssignTrophy(currentWeek, team, new SixHundredClubTrophy(team, string.Empty)));
            }

            //700 club
            winners = currentWeek.Scores.Where(s => s.AwayTeam.TeamPoints >= 700).Select(s => s.AwayTeam).ToList();
            winners.AddRange(currentWeek.Scores.Where(s => s.HomeTeam.TeamPoints >= 700).Select(s => s.HomeTeam));
            foreach (var team in winners.OrderByDescending(t => t.TeamPoints))
            {
                trophies.Add(trophyAssigner.AssignTrophy(currentWeek, team, new SevenHundredClubTrophy(team, string.Empty)));
            }

            //ballers/losers
            var largestMargin = currentWeek.Scores.OrderByDescending(s => s.MarginOfVictory).First().MarginOfVictory;
            var ballersAndMeeksOfTheWeek = currentWeek.Scores.Where(s => s.MarginOfVictory == largestMargin);

            foreach (var score in ballersAndMeeksOfTheWeek)
            {
                if (score.HomeTeam.TeamPoints > score.AwayTeam.TeamPoints)
                {
                    trophies.Add(trophyAssigner.AssignTrophy(currentWeek, score.HomeTeam, new BallerOfTheWeekTrophy(score.HomeTeam, score.MarginOfVictory.ToString(CultureInfo.InvariantCulture))));
                    trophies.Add(trophyAssigner.AssignTrophy(currentWeek, score.AwayTeam, new LoserOfTheWeekTrophy(score.AwayTeam, score.MarginOfVictory.ToString(CultureInfo.InvariantCulture))));
                }
                else
                {
                    trophies.Add(trophyAssigner.AssignTrophy(currentWeek, score.AwayTeam, new BallerOfTheWeekTrophy(score.AwayTeam, score.MarginOfVictory.ToString(CultureInfo.InvariantCulture))));
                    trophies.Add(trophyAssigner.AssignTrophy(currentWeek, score.HomeTeam, new LoserOfTheWeekTrophy(score.HomeTeam, score.MarginOfVictory.ToString(CultureInfo.InvariantCulture))));
                }
            }

            Thread.Sleep(2000);
            //ops
            var teams = currentWeek.Scores.Select(s => s.HomeTeam).ToList();
            teams.AddRange(currentWeek.Scores.Select(s => s.AwayTeam));

            foreach (var op in opsOfTheWeek)
            {
                string additionalInfo = JsonConvert.SerializeObject(op);
                var team = teams.Single(t => t.TeamAbbreviation == op.TeamAbbreviation);
                trophies.Add(trophyAssigner.AssignTrophy(currentWeek, team, new OffensivePlayerOfTheWeekTrophy(team, additionalInfo)));
            }

            //dps
            foreach (var dp in dpsOfTheWeek)
            {
                string additionalInfo = JsonConvert.SerializeObject(dp);
                var team = teams.Single(t => t.TeamAbbreviation == dp.TeamAbbreviation);
                trophies.Add(trophyAssigner.AssignTrophy(currentWeek, team, new DefensivePlayerOfTheWeekTrophy(team, additionalInfo)));
            }

            if (currentWeek.WeekNumber == 13)
            {
                var rankingGenerator = new RankingGenerator(driver, year);
                var rankings = rankingGenerator.GenerateRankings();

                //#1 ranked
                var topRanked = rankings.OrderBy(r => r.Rank).First();
                var topRankedTeam = teams.Single(t => t.TeamName == topRanked.Team);
                trophies.Add(trophyAssigner.AssignTrophy(currentWeek, topRankedTeam, new TopRankedSeasonTrophy(topRankedTeam, topRanked.Team)));

                //top scoring
                var standings = new StandingsGenerator(driver, year).GenerateStandings();
                var highestScoringStanding = standings.OrderByDescending(s => s.PointsFor).First();
                var standingInfo = JsonConvert.SerializeObject(highestScoringStanding);
                var highestScoringTeam = teams.Single(t => t.TeamName == highestScoringStanding.Team);
                trophies.Add(trophyAssigner.AssignTrophy(currentWeek, highestScoringTeam, new HighestScoringSeasonTrophy(highestScoringTeam, standingInfo)));

                //OPOY
                driver.Navigate().GoToUrl($"http://games.espn.com/ffl/freeagency?leagueId=127291&seasonId={year}");
                driver.WaitUntilElementExists(By.Id("playerTableContainerDiv"));

                var opLink = driver.FindElement(By.XPath("//ul[@class='filterToolsOptionSet']/li/a[contains(.,'OP')]"));
                opLink.Click();

                Thread.Sleep(2000);
                
                var opRows = driver.FindElements(By.XPath("//tr[contains(@class, 'pncPlayerRow')]/td[3][not(contains(.,'FA'))]/parent::tr"));
                var opsOfTheYear = new List<PlayerOfTheWeek>();
                var topPoints = 0m;
                foreach (var opRow in opRows)
                {
                    var rowPoints = decimal.Parse(opRow.FindElement(By.XPath("./td[contains(@class, 'sortedCell')]")).Text);
                    if (rowPoints >= topPoints)
                    {
                        topPoints = rowPoints;
                    }
                    else
                    {
                        break;
                    }

                    var opOfTheYear = new PlayerOfTheWeek();
                    opOfTheYear.Name = opRow.FindElement(By.XPath("./td[@class='playertablePlayerName']/a")).Text;
                    opOfTheYear.Team = opRow.FindElement(By.XPath("./td[3]/a")).GetAttribute("title");
                    opOfTheYear.TeamAbbreviation = opRow.FindElement(By.XPath("./td[3]/a")).Text;
                    opOfTheYear.PlayerId = int.Parse(opRow.FindElement(By.XPath("./td[@class='playertablePlayerName']/a")).GetAttribute("playerid"));
                    opOfTheYear.Points = rowPoints;

                    opsOfTheYear.Add(opOfTheYear);
                }

                //DPOY
                var dpLink = driver.FindElement(By.XPath("//ul[@class='filterToolsOptionSet']/li/a[contains(.,'DP')]"));
                dpLink.Click();
                var dpsOfTheYear = new List<PlayerOfTheWeek>();
                Thread.Sleep(2000);

                var dpRows = driver.FindElements(By.XPath("//tr[contains(@class, 'pncPlayerRow')]/td[3][not(contains(.,'FA'))]/parent::tr"));
                topPoints = 0m;
                foreach (var dpRow in dpRows)
                {
                    var rowPoints = decimal.Parse(dpRow.FindElement(By.XPath("./td[contains(@class, 'sortedCell')]")).Text);
                    if (rowPoints >= topPoints)
                    {
                        topPoints = rowPoints;
                    }
                    else
                    {
                        break;
                    }

                    var dpOfTheYear = new PlayerOfTheWeek();
                    dpOfTheYear.Name = dpRow.FindElement(By.XPath("./td[@class='playertablePlayerName']/a")).Text;
                    dpOfTheYear.Team = dpRow.FindElement(By.XPath("./td[3]/a")).GetAttribute("title");
                    dpOfTheYear.TeamAbbreviation = dpRow.FindElement(By.XPath("./td[3]/a")).Text;
                    dpOfTheYear.PlayerId = int.Parse(dpRow.FindElement(By.XPath("./td[@class='playertablePlayerName']/a")).GetAttribute("playerid"));
                    dpOfTheYear.Points = rowPoints;
                    dpsOfTheYear.Add(dpOfTheYear);
                }

                foreach (var opOfTheYear in opsOfTheYear)
                {
                    var additionalInfo = JsonConvert.SerializeObject(opOfTheYear);
                    var opoyTeam = teams.Single(t => t.TeamName == opOfTheYear.Team);
                    trophies.Add(trophyAssigner.AssignTrophy(currentWeek, opoyTeam, new OffensivePlayerOfTheYearTrophy(opoyTeam, additionalInfo)));
                }

                foreach (var dpOfTheYear in dpsOfTheYear)
                {
                    var additionalInfo = JsonConvert.SerializeObject(dpOfTheYear);
                    var dpoyTeam = teams.Single(t => t.TeamName == dpOfTheYear.Team);
                    trophies.Add(trophyAssigner.AssignTrophy(currentWeek, dpoyTeam, new DefensivePlayerOfTheYearTrophy(dpoyTeam, additionalInfo)));
                }

                //NFC champ
                var nfcChamp = rankings.Where(s => s.Division == "NFC").OrderBy(s => s.Rank).First();
                var nfcChampionshipTeam = teams.Single(t => t.TeamName == nfcChamp.Team);
                trophies.Add(trophyAssigner.AssignTrophy(currentWeek, nfcChampionshipTeam, new NfcDivisionChampionshipTrophy(nfcChampionshipTeam, string.Empty)));
                //AFC champ
                var afcChamp = rankings.Where(s => s.Division == "AFC").OrderBy(s => s.Rank).First();
                var afcChampionshipTeam = teams.Single(t => t.TeamName == afcChamp.Team);
                trophies.Add(trophyAssigner.AssignTrophy(currentWeek, afcChampionshipTeam, new AfcDivisionChampionshipTrophy(afcChampionshipTeam, string.Empty)));

                //#12 ranked
                var bottomRanked = rankings.OrderByDescending(r => r.Rank).First();
                var bottomRankedTeam = teams.Single(t => t.TeamName == bottomRanked.Team);
                trophies.Add(trophyAssigner.AssignTrophy(currentWeek, bottomRankedTeam, new BottomRankedSeasonTrophy(bottomRankedTeam, bottomRanked.Team)));
            }

            return trophies;
        }

        private static List<PowerRanking> GetPowerRankings(ChromeDriver driver, WeekRepository weekRepository)
        {
            var currentWeek = week;
            var weeksForPowerRankings = new List<Week>();
            
            for (var i = 3; i >= 0; i--)
                if (currentWeek - i > 0)
                    weeksForPowerRankings.Add(weekRepository.GetWeek(driver, currentWeek - i, year));

            var powerRankingGenerator = new PowerRankingGenerator(weeksForPowerRankings, currentWeek);
            return powerRankingGenerator.GeneratePowerRankings();
        }
    }
}