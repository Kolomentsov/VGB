using Newtonsoft.Json;

namespace IGamesData.BotData
{
    public class DataResponse<T>
    {
        [JsonProperty(PropertyName = "result", Required = Required.Default)]
        public T Result { get; set; }

        [JsonProperty(PropertyName = "OK", Required = Required.Always)]
        public bool Ok { get; set; }
        

    }
}
