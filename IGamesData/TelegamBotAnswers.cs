using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGamesData
{
    public class TelegamBotAnswers
    {
        public static string GetInfo()
        {
            return String.Format(@"Hello! I'm bot that can provides you some information about computer games:
Here is my list of commands:
/getinfo - Shows information about this bot. 
/searchcharacters - Search characters by the name.
/searchgames - Provides search by games title.
/getbygamemod - Returns random games by a particular genre.
/getsimilars - Returns games that are similar to some game.
/some command???
/gettop100 - Returns random games from IGDB top-100 best games.");
        }
        public static string SimpleCancelAnswer()
        => "Ok! Operation is cancelled, let's try /getinfo to see what else I can do for you!";

        public static string SimpleInjectionAnswer()
        => "This is all what I can offer you:";

        public static string GamesSearchInjection()
        => $"Please! Enter a name of the game you are looking for";

        public static string GamesSearchAdvice()
        => "You can always use /searchgames command to see more information about one of these games";

        public static string RandomGamesAnswer(string t)
        => $"There is a random games from IGDB top 100 gamess for you:\n ''{t}''";

        public static string RandomGamesChoose()
        => @"- enter ''Ok'' to view info about this game
             - enter ''Next'' to get another games
             - enter ''Cancel to cancel this operation";

        public static string GetSimilar()
        => "Ok! First I should find a game to which you want to see similars";

        public static string NotFoundSimilar()
        => "Unfortunately, I can't find similars to these game :( Please, try again!";

        public static string NotFoundMessage()
        => "Unfortunately, I couldn't find anything for you. Please, make sure your request is correct and try again!";

        public static string WrongQueryMessage()
        => "I can't understand your query. Write down /getinfo to get all possible commands";

        public static string WrongChoiceMessage()
        => "You are asked to make a choice to get more information or put ''Cancel''! Please, try again!";

        public static string GamesChooseMessage(int end, int from = 1)
        => $"Please, choose the exact game to get more detailed information (enter its number from {from} to {end}) " +
           "or send ''Cancel''  if you just don't want to see detailed information or if there is no suitable game in the list above";

        public static string GenresInjection()
        => "Here are all the genres I've already known:";

        public static string GenresChoose()
        => $"Please enter the name of genre to get random game of this or ''Cancel'' to cancel the operation";

        public static string SomeGameOfGenre(string genre)
        => $"I find random game of {genre} for you";

        public static string GameIGDBApp(string linktoweb)
        => $"You can see more information about this game on IGDB official website\n{linktoweb}";

        public static string NowPlayingInjection()
        => "Here are the games that are now playing people:";

        public static string CharacterSearchChoose(int end, int from = 1)
        => $"Please, choose the exact character to get more detailed information (enter his number from {from} to {end}) " +
           "or send ''Cancel'' if there is no suitable character in the list above or you just don't want to see detailed info";
        public static string CharacterSearchInjection()
        => $"Please, enter a name of the character you are looking for";
        public static string GetGameInformMessage(Game game)
        {
            return String.Format(@"{0}  ({1}) 
Genre: {2}
IGdbID: {3}
Category: {4}
GameMod: {5}
Discription: {6}
IGdbRating:{7}
Publisher: {8}
Status: {9}"
 , game.Name, game.Year, game.Cover, game.IGdbID, game.Category, game.GameMod, game.Discription, game.IGdbRating, game.Publisher, game.Status);
        }
        public static string NowPlayAnswer(List<Game> games)
        {
            StringBuilder sb = new StringBuilder();
            int i = 1;
            foreach (var game in games)
            {
                sb.AppendLine($"{i++}. {game.Name}");
                sb.AppendLine($"Plot: {game.Discription} \n");
            }
            return sb.ToString();
        }
        public static string GameModAnswer(List<string> gamemods)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var gm in gamemods)
            {
                sb.AppendLine($"- {gm}");
            }
            return sb.ToString();
        }

        public static string GamesSearchAnswer(List<Game> games)
        {
            StringBuilder sb = new StringBuilder();
            int i = 1;

            foreach (var game in games)
                sb.AppendLine($"{i++}. {game.Name} ({game.Year})");

            return sb.ToString();
        }

        public static string CharacterSearchAnswer(List<Character> characters)
        {
            StringBuilder sb = new StringBuilder();
            int n = 1;
            foreach (var ch in characters)
            {
                sb.AppendLine($"{n++}. {ch.Name}");
            }
            return sb.ToString();
        }
        public static string SingleCharactersAnswer(Character ch)
        {
            string CharacterName = $"Sending most popular games of {ch.Name}: \n";
            StringBuilder sb = new StringBuilder();
            int i = 1;
            foreach (var game in ch.Games)
            {
                sb.AppendLine($"\n{i++}. {game.Name}");
                sb.AppendLine($"Plot: {game.Discription}");
            }
            return CharacterName + sb.ToString();
        }
    }
}
