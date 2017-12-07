using Newtonsoft.Json;

namespace IGamesData.BotData
{
    public class DataUpdate
    {
        [JsonProperty(PropertyName = "update_id", Required = Required.Always)]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "message", Required = Required.Default)]
        public MessageData Message { get; set; }
    }
}
