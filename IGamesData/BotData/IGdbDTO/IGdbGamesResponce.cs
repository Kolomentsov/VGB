using System.Collections.Generic;
using IGamesData.BotData.IGdbDTO;
using Newtonsoft.Json;

namespace VGB.IGdbDTO
{
    public class IGdbGamesResponce
    {
        [JsonProperty(PropertyName = "Search")]
        public List<IGDBGenres> Genres { get; set; }
    }
}
