using IGamesData;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VGB.Interfaces
{
    interface IGamesService
    {
        Task<List<Game>> SearchGames(string query);

        Task<Game> SingleGameSearch(string query);

        string GetRandom100();

        Task<List<Character>> SearchCharacters(string query);

        Task<List<Game>> SomeCommand();

        Task<Dictionary<string, int>> GetGameMod();

        Task<Game> GetRandomGameMod(int genreId);

        Task<List<Game>> GetSimilarGames(Game game);
    }
}
