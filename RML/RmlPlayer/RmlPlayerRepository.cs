using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TubeBuddyScraper.RmlPlayer
{
    public class RmlPlayerRepository
    {
        private readonly string jsonFile = "../../rmlPlayers.json";

        public void RefreshRmlPlayers(List<RmlPlayer> rmlPlayers)
        {
            var json = JsonConvert.SerializeObject(rmlPlayers);

            using (StreamWriter file = new StreamWriter(jsonFile))
            {
                file.Write(json);
            }
        }

        public List<RmlPlayer> GetRmlPlayers()
        {
            using (StreamReader file = new StreamReader(jsonFile))
            {
                var json = file.ReadToEnd();
                return JsonConvert.DeserializeObject<List<RmlPlayer>>(json);
            }
        }
    }
}
