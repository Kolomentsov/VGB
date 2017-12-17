using Newtonsoft.Json;

namespace IGamesData.BotData
{
    public class ChatData
    {
        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public long Id { get; set; }
    }
}
