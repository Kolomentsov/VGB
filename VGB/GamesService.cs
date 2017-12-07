using IGamesData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VGB.Interfaces;
using Newtonsoft.Json;
using System.IO;
using VGB.IGdbDTO;

namespace VGB
{
    class GamesService : IGamesService
    {
        const string _token = "cfaa31dfb2add3e949ea02fd4ed5581f";
        public async Task<List<Game>> SearchGames(string query)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string[] words = query.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    string source = String.Join("+", words);
                    string result = await client.GetStringAsync(String.Format("https://api-2445582011268.apicast.io/?s={0}&y=&plot=short&r=json", source));
                    var data = JsonConvert.DeserializeObject<IGdbGamesResponce>(result);

                    if (data.Games != null && data.Games.Count != 0)
                    {
                        return data.Games.Select(x => ConvertToGame(x)).ToList();
                    }
                    else return null;
                }
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

 
                return null;
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }


        public static Game ConvertToGame(IGdbGames game)
        {
            return new Game
            {

                Name = game.Name,
                Year = game.Year,
                Cover = game.Cover,
                IGdbID = game.IGdbID,
                Category = game.Category,
                GameMod = game.GameMod,
                Discription = game.Discription,
                IGdbRating = game.IGdbRating,
                Publisher = game.Publisher,
                Status = game.Status
            };
        }
    }
}
