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
    }
}
