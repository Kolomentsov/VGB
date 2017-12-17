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

        List<Game> SingleGameSearch(string query);

        string GetRandom100();
        List<Character> SearchCharacters(string query);
        List<Character> SingleCharacterSearch(string query);

        Task<List<Game>> SomeCommand();

        Task<Game> GetRandomGameMod(int genreId);

        Task<List<Game>> GetSimilarGames(Game game);
    }
}
