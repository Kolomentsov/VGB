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
using System.Net;
using IGamesData.GamesData;
using VGB.IGdbDTO;

namespace VGB
{
    class GamesService : IGamesService
    { 
        const string _token = "cfaa31dfb2add3e949ea02fd4ed5581f";
        //public async Task<List<IGamesData.GamesData.Repository.Game>> SearchGames(string query)
        //{   

        //    try 
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            string[] words = query.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        //            string source = String.Join("+", words);
        //            string result = await client.GetStringAsync(String.Format("https://api-2445582011268.apicast.io/?s={0}&Accept=json", source));
        //            var data = JsonConvert.DeserializeObject<IGdbGamesResponce<IGdbGames>>(result);

        //            if (data.Games != null && data.Games.Count != 0)
        //            {
        //                return data.Games.Select(x => ConvertToGame(x)).ToList();
        //            }
        //            else return null;
        //        }
        //    }
        //    catch (HttpRequestException)
        //    {
        //        return null;
        //    }
        //}

        //public async Task<Repository.Game> SingleMovieSearch(string query)
        //{
        //   
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            string[] words = query.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        //            string source = String.Join("+", words);
        //            string result = await client.GetStringAsync(String.Format("GET https://api-2445582011268.apicast.io/games/?t={0}&plot=full&Accept=json", source));
        //            var game = JsonConvert.DeserializeObject<IGdbGames>(result);
        //            return ConvertToGame(game);
        //        }
        //    }
        //    catch (HttpRequestException)
        //    {
        //        return null;
        //    }
        //}

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
            var t = (HttpWebRequest)WebRequest.Create(@"https://api-2445582011268.apicast.io//characters/?search=" + query + "&fields=*");
            t.Headers.Add("user-key: cfaa31dfb2add3e949ea02fd4ed5581f");
            t.Accept = "application/json";
            var s = (HttpWebResponse)t.GetResponse();
            var responseString = new StreamReader(s.GetResponseStream()).ReadToEnd();
            return JsonConvert.DeserializeObject<Character[]>(responseString).ToList();
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
        public Task<Repository.Game> SingleGameSearch(string query)
        {
            throw new NotImplementedException();
        }

        public Repository.Game SearchGame(string query)
        {
            throw new NotImplementedException();
        }


        public Task<List<Repository.Game>> SomeCommand()
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, int>> GetGameMod()
        {
            throw new NotImplementedException();
        }

        public Task<Repository.Game> GetRandomGameMod(int genreId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Repository.Game>> GetSimilarGames(Repository.Game game)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Character>> SearchActors(string query)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string result = await client.GetStringAsync($"GET https://api-2445582011268.apicast.io/games/person?user_key={_token}&include_adult=False&query={query}");
                    var response = JsonConvert.DeserializeObject<IGdbGamesResponce<IGdbGames>>(result);
                    if (response.TotalResults!= null && response.TotalResults.Count != 0)
                    {
                        return response.TotalResults.Select(actor => new Character
                        {
                            Descriprtion = actor.Discription,
                            //Games = actor.Name.Select(game => new Game
                            //{
                            //     Publisher = game,
                            //    Description = game.Plot
                            //}).ToList(),
                            Poster = actor.Cover
                        }).ToList();
                    }
                    else return null;
                }
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }


        //public async Task<List<Game>> GetNowPlaying()
        //{
        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            string result = await client.GetStringAsync($"");
        //            var response = JsonConvert.DeserializeObject<themoviedbResponse<themoviedbMovie>>(result);
        //            if (response.Results != null && response.Results.Count != 0)
        //            {
        //                return response.Results.Select(movie =>
        //                          new Movie
        //                          {
        //                              Title = movie.Title,
        //                              Description = movie.Plot,
        //                              ImdbRating = movie.VoteAverage
        //                          }).Take(10).ToList();
        //            }
        //            else return null;
        //        }
        //    }
        //    catch (HttpRequestException)
        //    {
        //        return null;
        //    }
        //}


        //public async Task<Dictionary<string, int>> GetGenres()
        //{
        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            string result = await client.GetStringAsync($");
        //            var response = JsonConvert.DeserializeObject<themoviedbGenreResponse>(result);
        //            var genres = response.Genres.ToDictionary(genre => genre.Name.ToLower().Trim(), genre => genre.Id);
        //            genres.Remove("tv movie");
        //            genres.Remove("documentary");
        //            return genres;
        //        }
        //    }
        //    catch (HttpRequestException)
        //    {
        //        return null;
        //    }
        //}


        //public async Task<Game> GetRandomByGenre(int genreId)
        //{
        //    string url =
        //        $"";
        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            string result1 = await client.GetStringAsync(url);
        //            var response1 = JsonConvert.DeserializeObject<themoviedbResponse<themoviedbMovie>>(result1);

        //            Random rnd = new Random();
        //            var page = rnd.Next(1, response1.TotalPages + 1);
        //            string result2 = await client.GetStringAsync(url + $"&page={page}");

        //            var response2 = JsonConvert.DeserializeObject<themoviedbResponse<themoviedbMovie>>(result2);
        //            var movies = response2.Results.Select(movie => movie.Title).ToList();
        //            return await SingleMovieSearch(movies[rnd.Next(0, movies.Count)]);
        //        }
        //    }
        //    catch (HttpRequestException)
        //    {
        //        return null;
        //    }
        //}


    


       


        //public static Game ConvertToGame(IGdbGames game)
        //{
        //    return new Game
        //    {

        //        Name = game.Name,
        //        Year = game.Year,
        //        Cover = game.Cover,
        //        IGdbID = game.IGdbID,
        //        Category = game.Category,
        //        GameMod = game.GameMod,
        //        Discription = game.Discription,
        //        IGdbRating = game.IGdbRating,
        //        Publisher = game.Publisher,
        //        Status = game.Status
        //    };
        //}

    }
}
