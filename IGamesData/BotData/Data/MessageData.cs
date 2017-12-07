using Newtonsoft.Json;

namespace IGamesData.BotData
{
    public class MessageData
    {
        [JsonProperty(PropertyName = "chat", Required = Required.Always)]
        public ChatData Chat { get; set; }

        [JsonProperty(PropertyName = "message_id", Required = Required.Always)]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "from", Required = Required.Default)]
        public UserData User { get; set; }

        [JsonProperty(PropertyName = "text", Required = Required.Default)]
        public string Text { get; set; }
    }
}
