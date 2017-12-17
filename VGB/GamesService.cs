using IGamesData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using VGB.Interfaces;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using IGamesData.GamesData;
using VGB.IGdbDTO;
using static IGamesData.GamesData.Repository;

namespace VGB
{
    class GamesService : IGamesService
    {
        const string _token = "cfaa31dfb2add3e949ea02fd4ed5581f";
        public List<Repository.Game> SingleGameSearch(string query)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var t = (HttpWebRequest)WebRequest.Create(@"https://api-2445582011268.apicast.io/games/?search=" + query + "&fields=*");
                    t.Headers.Add("user-key: cfaa31dfb2add3e949ea02fd4ed5581f");
                    t.Accept = "application/json";
                    var s = (HttpWebResponse)t.GetResponse();
                    var responseString = new StreamReader(s.GetResponseStream()).ReadToEnd();
                    return JsonConvert.DeserializeObject<Repository.Game[]>(responseString).ToList();
                }
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
        public List<Character> SingleCharacterSearch(string query)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var t = (HttpWebRequest)WebRequest.Create(@"https://api-2445582011268.apicast.io/characters/?search=" + query + "&fields=*");
                    t.Headers.Add("user-key: cfaa31dfb2add3e949ea02fd4ed5581f");
                    t.Accept = "application/json";
                    var s = (HttpWebResponse)t.GetResponse();
                    var responseString = new StreamReader(s.GetResponseStream()).ReadToEnd();
                    return JsonConvert.DeserializeObject<Character[]>(responseString).ToList();
                }
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public List<Repository.Game> SearchGames(string query)
        {
            var t = (HttpWebRequest)WebRequest.Create(@"https://api-2445582011268.apicast.io/games/?search=" + query + "&fields=*");
            t.Headers.Add("user-key: cfaa31dfb2add3e949ea02fd4ed5581f");
            t.Accept = "application/json";
            var s = (HttpWebResponse)t.GetResponse();
            var responseString = new StreamReader(s.GetResponseStream()).ReadToEnd();
            return JsonConvert.DeserializeObject<Repository.Game[]>(responseString).ToList();
        }
        public List<Character> SearchCharacters(string query)
        {
            var t = (HttpWebRequest)WebRequest.Create(@"https://api-2445582011268.apicast.io/characters/?search=" + query + "&fields=*");
            t.Headers.Add("user-key: cfaa31dfb2add3e949ea02fd4ed5581f");
            t.Accept = "application/json";
            var s = (HttpWebResponse)t.GetResponse();
            var responseString = new StreamReader(s.GetResponseStream()).ReadToEnd();
            return JsonConvert.DeserializeObject<Character[]>(responseString).ToList();
        }
        public List<GameModes> SearchGamesMod(string query)
        {
            var t = (HttpWebRequest)WebRequest.Create(@"https://api-2445582011268.apicast.io/game_modes/?search=" + query + "&fields=*");
            t.Headers.Add("user-key: cfaa31dfb2add3e949ea02fd4ed5581f");
            t.Accept = "application/json";
            var s = (HttpWebResponse)t.GetResponse();
            var responseString = new StreamReader(s.GetResponseStream()).ReadToEnd();
            return JsonConvert.DeserializeObject<GameModes[]>(responseString).ToList();
        }

        public string GetRandom100()
        {
            List<string> top100 = new List<string>();
            using (StreamReader streamReader = new StreamReader
                ("../../Top100.txt", Encoding.UTF8))
            {
                while (!streamReader.EndOfStream)

                {
                    string game = streamReader.ReadLine();
                    top100.Add(game.Trim());
                }
            }
            Random rnd = new Random();
            return top100[rnd.Next(0, top100.Count)];
        }
        public List<Genre> GetGenres()
        {
            var t = (HttpWebRequest)WebRequest.Create(@"https://api-2445582011268.apicast.io/genres/?fields=*");
            t.Headers.Add("user-key: cfaa31dfb2add3e949ea02fd4ed5581f");
            t.Accept = "application/json";
            var s = (HttpWebResponse)t.GetResponse();
            var responseString = new StreamReader(s.GetResponseStream()).ReadToEnd();
            return JsonConvert.DeserializeObject<Genre[]>(responseString).ToList();
        }
        public static Game ConvertToGame(IGdbGames game)
        {
            return new Game
            {

                name = game.name,
                created_at = game.created_at,
                slug = game.slug,
                url = game.url,
                id = game.id
            };
        }
    }
}
