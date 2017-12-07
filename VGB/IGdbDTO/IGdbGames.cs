using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VGB.IGdbDTO
{
    public class IGdbGames
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public int Year { get; set; }

        [JsonProperty(PropertyName = "cover")]
        public object Cover { get; set; }

        [JsonProperty(PropertyName = "id")]
        public uint IGdbID { get; set; }

        [JsonProperty(PropertyName = "category")]
        public int Category { get; set; }

        [JsonProperty(PropertyName = "game_modes")]
        public List<int> GameMod { get; set; }

        [JsonProperty(PropertyName = "summary")]
        public string Discription { get; set; }

        [JsonProperty(PropertyName = "rating")]
        public double IGdbRating { get; set; }

        [JsonProperty(PropertyName = "publishers")]
        public List<int> Publisher { get; set; }

        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }

    }
}
