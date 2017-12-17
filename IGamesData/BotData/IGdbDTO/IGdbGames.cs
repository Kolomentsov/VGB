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
        [JsonProperty(PropertyName ="id")]
        public int id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string name { get; set; }
        [JsonProperty(PropertyName = "slug")]
        public string slug { get; set; }
        [JsonProperty(PropertyName = "url")]
        public string url { get; set; }
        [JsonProperty(PropertyName = "created_at")]
        public object created_at { get; set; }
        

    }
}
