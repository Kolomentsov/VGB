using IGamesData;
using System.Collections.Generic;
using System.Threading.Tasks;
using IGamesData.GamesData;
using static IGamesData.GamesData.Repository;

namespace VGB.Interfaces
{
    interface IGamesService
    {
        List<Game> SearchGames(string query);

        Task<Game> SingleGameSearch(string query);

        string GetRandom100();
        Game SearchGame(string query);
       
        List<Character> SearchCharacters(string query);

        Task<List<Game>> SomeCommand();

        Task<Dictionary<string, int>> GetGameMod();

        Task<Game> GetRandomGameMod(int genreId);

        Task<List<Game>> GetSimilarGames(Game game);
    }
}
