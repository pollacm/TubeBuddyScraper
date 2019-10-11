using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TubeBuddyScraper.Games
{
    public class GameRepository
    {
        private readonly string jsonFile = "../../games.json";

        public void RefreshGames(List<Game> games)
        {
            var json = JsonConvert.SerializeObject(games);

            using (StreamWriter file = new StreamWriter(jsonFile))
            {
                file.Write(json);
            }
        }

        public List<Game> GetGames()
        {
            using (StreamReader file = new StreamReader(jsonFile))
            {
                var json = file.ReadToEnd();
                return JsonConvert.DeserializeObject<List<Game>>(json);
            }
        }

        public void AddGame(Game gameToAdd)
        {
            var games = GetGames();
            games.Add(gameToAdd);
            RefreshGames(games);
        }

        public void AddGames(List<Game> gamesToAdd)
        {
            var games = GetGames();
            games.AddRange(gamesToAdd);
            RefreshGames(games);
        }

        public List<Game> CleanStaleGamesFromDay(DateTime date)
        {
            var games = GetGames();
            games.RemoveAll(g => g.DateChecked == date && string.IsNullOrEmpty(g.TubebuddyScore));
            RefreshGames(games);

            return GetGames();
        }

        public List<Game> CleanStaleGamesFromDayAndAppend(DateTime date, List<Game> newGames)
        {
            var games = CleanStaleGamesFromDay(date);
            games.AddRange(newGames);
            RefreshGames(games);

            return GetGames();
        }

        public List<Game> GetGamesForDay(DateTime date)
        {
            var games = GetGames().Where(g => g.DateChecked == date).ToList();
            return games;
        }

        public List<Game> GetGamesNotCompleteForDay(DateTime date, List<Game> newGames)
        {
            var gamesForDay = GetGamesForDay(date);
            var completedGamesForDay = gamesForDay.Where(g => !string.IsNullOrEmpty(g.TubebuddyScore)).ToList();
            var nonCompleteGamesForDay = newGames.Where(g => !completedGamesForDay.Any(g2 => g2.Title == g.Title));

            return nonCompleteGamesForDay.ToList();
        }
    }
}
