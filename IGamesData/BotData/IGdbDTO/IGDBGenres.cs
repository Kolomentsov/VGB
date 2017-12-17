using Newtonsoft.Json;

namespace IGamesData.BotData.IGdbDTO
{
    public class IGDBGenres
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
