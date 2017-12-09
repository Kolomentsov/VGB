using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VGB.IGdbDTO
{
    public class IGdbGamesResponce<T>
    {
        [JsonProperty(PropertyName = "Search")]
        public List<IGdbGames> Games { get; set; }

        [JsonProperty(PropertyName = "totalResults")]
        public List<T> TotalResults { get; set; }
    }
}
