using IGamesData;
using IGamesData.BotData;
using IGamesData.BotData.ActionWithBot;
using IGamesData.GamesData;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VGB.Interfaces;

namespace VGB
{
    class BotManager
    {
        List<long> _searchingSimilarUsers;
        Dictionary<long, QueryAction> _botWaitsForQuery;
        Dictionary<long, List<string>> _multipleDataForUser;
        Dictionary<long, string> _singleDataForUser;

        IGamesService _service;
        ITelegramBot _client;

        public BotManager(ITelegramBot client, IGamesService service)
        {
            _botWaitsForQuery = new Dictionary<long, QueryAction>();
            _multipleDataForUser = new Dictionary<long, List<string>>();
            _singleDataForUser = new Dictionary<long, string>();
            _searchingSimilarUsers = new List<long>();
            _service = service;
            _client = client;

            client.OnMessageReceived += ProcessMessage;
        }

        public async void ProcessMessage(MessageData messagedata)
        {
            var chatId = messagedata.Chat.Id;

            //Checking what bot is doing : waiting for user's answer or choicing
            if (_botWaitsForQuery.ContainsKey(chatId))
            {
                switch (_botWaitsForQuery[chatId])
                {
                    case QueryAction.GameSearching:
                        {
                            var games = _service.SearchGames(messagedata.Text);
                            if (games != null && games.Count != 0)
                            {
                                if (games.Count == 1)
                                {
                                    await SendInfoAboutGame(chatId, games[0]);

                                    _singleDataForUser[chatId] = games[0].name;
                                }
                                else
                                {
                                    await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.SimpleInjectionAnswer());
                                    await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.GamesSearchAnswer(games));
                                    await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.GamesChooseMessage(games.Count()));

                                    _multipleDataForUser[chatId] = games.Select(m => m.name).ToList();
                                    _botWaitsForQuery[chatId] = QueryAction.GameSelecting;
                                }
                            }
                            else
                            {
                                await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.NotFoundMessage());

                                _botWaitsForQuery.Remove(chatId);
                            }
                            break;
                        }
                    case QueryAction.GenreSearching:
                        {
                            switch (messagedata.Text.Trim().ToLower())
                            {
                                case "show":
                                    var title = _service.GetGenres();
                                    _singleDataForUser[chatId] = title[0].name;
                                    await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.GameModAnswer(title[0]));
                                    await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.SearchAdvice());
                                    break;
                                case "no":
                                case "cancel":
                                    _singleDataForUser.Remove(chatId);
                                    _botWaitsForQuery.Remove(chatId);
                                    await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.SimpleCancelAnswer());
                                    break;
                                default:
                                    await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.WrongChoiceMessage());
                                    break;
                            }
                            break;

                        }
                    case QueryAction.GameSelecting:
                        {
                            //Checking if user entered the correct number of selecting game or cancelled the operation
                            int chosenIndex = 1;

                            if (int.TryParse(messagedata.Text, out chosenIndex) && chosenIndex >= 1 &&
                                chosenIndex <= _multipleDataForUser[chatId].Count)
                            {
                                var title = _multipleDataForUser[chatId][chosenIndex - 1];
                                var game = _service.SingleGameSearch(title);
                                _multipleDataForUser.Remove(chatId);


                                //If this is done, the bot will show simiar games instead.
                                if (!_searchingSimilarUsers.Contains(chatId))
                                {
                                    await SendInfoAboutGame(chatId, game[0]);
                                    _singleDataForUser[chatId] = title;
                                }
                                else
                                {
                                    {
                                        await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.NotFoundSimilar());
                                    }
                                    _searchingSimilarUsers.Remove(chatId);
                                    _botWaitsForQuery.Remove(chatId);

                                }
                            }
                            else if (messagedata.Text.Trim().ToLower() == "cancel")
                            {
                                await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.SimpleCancelAnswer());
                                _multipleDataForUser.Remove(chatId);
                                _botWaitsForQuery.Remove(chatId);
                                _searchingSimilarUsers.Remove(chatId);
                            }
                            else
                            {
                                await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.WrongChoiceMessage());
                            }
                        }
                        break;
                    case QueryAction.RandomGameSelecting:
                        {
                            string title;
                            switch (messagedata.Text.Trim().ToLower())
                            {
                                case "next":
                                    title = _service.GetRandom100();
                                    _singleDataForUser[chatId] = title;
                                    await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.RandomGamesAnswer(title));
                                    await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.RandomGamesChoose());
                                    break;
                                case "no":
                                case "cancel":
                                    _singleDataForUser.Remove(chatId);
                                    _botWaitsForQuery.Remove(chatId);

                                    await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.SimpleCancelAnswer());
                                    break;
                                default:
                                    await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.WrongChoiceMessage());
                                    break;
                            }
                            break;
                        }

                    case QueryAction.CharacterSearching:
                        {
                            var characters = _service.SearchCharacters(messagedata.Text);

                            if (characters != null && characters.Count != 0)
                            {
                                if (characters.Count == 1)
                                {
                                    _multipleDataForUser.Remove(chatId);
                                    await SendInfoAboutCharacter(chatId, characters[0]);
                                    _botWaitsForQuery.Remove(chatId);
                                }
                                else
                                {
                                    await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.SimpleInjectionAnswer());
                                    await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.CharacterSearchAnswer(characters));
                                    await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.CharacterSearchChoose(characters.Count()));

                                    _multipleDataForUser[chatId] = characters.Select(a => a.name).ToList();
                                    _botWaitsForQuery[chatId] = QueryAction.CharacterSelecting;
                                }
                            }
                            else
                            {
                                await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.NotFoundMessage());
                                _botWaitsForQuery.Remove(chatId);
                            }
                            break;
                        }
                    case QueryAction.CharacterSelecting:
                        {
                            int chosenIndex = 1;

                            if (int.TryParse(messagedata.Text, out chosenIndex) && chosenIndex >= 1 &&
                                chosenIndex <= _multipleDataForUser[chatId].Count)
                            {
                                var title = _multipleDataForUser[chatId][chosenIndex - 1];
                                var character = _service.SingleCharacterSearch(title);
                                _multipleDataForUser.Remove(chatId);

                                //If this is done, the bot will show characters instead
                                if (!_searchingSimilarUsers.Contains(chatId))
                                {
                                    await SendInfoAboutCharacter(chatId, character[0]);
                                    _singleDataForUser[chatId] = title;
                                }
                                else
                                {
                                    await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.NotFoundSimilar());
                                }
                                _searchingSimilarUsers.Remove(chatId);
                                _botWaitsForQuery.Remove(chatId);
                            }
                            else if (messagedata.Text.Trim().ToLower() == "cancel")
                            {
                                await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.SimpleCancelAnswer());
                                _multipleDataForUser.Remove(chatId);
                                _botWaitsForQuery.Remove(chatId);
                            }
                            else
                            {
                                await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.WrongChoiceMessage());
                            }
                        }
                        break;
                }
            }
            else
            {
                switch (messagedata.Text.ToLower().Trim())
                {
                    case "/start":
                    case "/getinfo":
                    case "hi":
                    case "hello":
                        {
                            await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.GetInfo());
                            break;
                        }
                    case "/searchgames":
                        {
                            await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.GamesSearchInjection());
                            _botWaitsForQuery[chatId] = QueryAction.GameSearching;
                            break;
                        }
                    case "/gettop100":
                        {
                            string title = _service.GetRandom100();
                            _singleDataForUser[chatId] = title;
                            await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.RandomGamesAnswer(title));
                            await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.RandomGamesChoose());
                            _botWaitsForQuery.Add(chatId, QueryAction.RandomGameSelecting);
                            break;
                        }
                    case "/searchcharacters":
                        {
                            await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.CharacterSearchInjection());
                            _botWaitsForQuery[chatId] = QueryAction.CharacterSearching;
                            break;
                        }
                    case "/getgenre":
                        {
                            await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.GenresInjection());
                            _botWaitsForQuery[chatId] = QueryAction.GenreSearching;
                            break;
                        }
                    default:
                        {
                            await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.WrongQueryMessage());
                            break;
                        }
                }
            }
        }
        public async Task SendInfoAboutGame(long chatId, IGamesData.GamesData.Repository.Game game)
        {
            await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.GetGameInformMessage(game));
            await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.SearchAdvice());

        }
        public async Task SendInfoAboutGenre(long chatId, Genre genre)
        {
            await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.GameModAnswer(genre));
            await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.SearchAdvice());

        }
        public async Task SendInfoAboutCharacter(long chatId, Character character)
        {
            if (character != null)
            {
                await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.GetCharacterInformMessage(character));
                await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.GamesSearchAdvice());
            }
            else await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.NotFoundMessage());
        }
    }
}
