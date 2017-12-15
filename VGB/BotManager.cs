using IGamesData;
using IGamesData.BotData;
using IGamesData.BotData.ActionWithBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IGamesData.GamesData;
using VGB.Interfaces;

namespace VGB
{
    class BotManager
    {
        Dictionary<string, int> _modes;
        List<long> _searchingSimilarUsers;


        Dictionary<long, QueryAction> _botWaitsForQuery;
        Dictionary<long, List<string>> _multipleDataForUser;
        Dictionary<long, string> _singleDataForUser;

        bool genresWasDownloaded;


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

            //Checking if bot is waiting for user's answer or choice.
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
                    case QueryAction.GameSelecting:
                        {
                            //Checking if user entered the correct number of movie or cancelled the operation.
                            int chosenIndex = 1;

                            if (int.TryParse(messagedata.Text, out chosenIndex) && chosenIndex >= 1 &&
                                chosenIndex <= _multipleDataForUser[chatId].Count)
                            {
                                var title = _multipleDataForUser[chatId][chosenIndex - 1];
                                var game = await _service.SingleGameSearch(title);
                                _multipleDataForUser.Remove(chatId);

                               
                                //If it is, we won't show full info about movies, we will show simiar movies instead.
                                if (!_searchingSimilarUsers.Contains(chatId))
                                {
                                    await SendInfoAboutGame(chatId, game);
                                    _singleDataForUser[chatId] = title;
                                }
                                else
                                {
                                    var games = await _service.GetSimilarGames(game);
                                    if (games != null && games.Count != 0)
                                    {
                                        await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.GamesSearchAnswer(games));
                                        await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.GamesSearchAdvice());
                                    }
                                    else
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
                                case "Ok":
                                    _singleDataForUser.TryGetValue(chatId, out title);
                                    var game = await _service.SingleGameSearch(title);
                                    _singleDataForUser[chatId] = game.name;
                                    await SendInfoAboutGame(chatId, game);
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
                                    await SendInfoAboutCharacter(chatId, characters[0].Name);
                                    _botWaitsForQuery.Remove(chatId);
                                }
                                else
                                {
                                    await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.SimpleInjectionAnswer());
                                    await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.CharacterSearchAnswer(characters));
                                    await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.CharacterSearchChoose(characters.Count()));

                                    _multipleDataForUser[chatId] = characters.Select(a => a.Name).ToList();
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
                                await SendInfoAboutCharacter(chatId, _multipleDataForUser[chatId][chosenIndex - 1]);

                                _multipleDataForUser.Remove(chatId);
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
                    case QueryAction.GameModSelecting:
                        {
                            if (genresWasDownloaded && _modes.ContainsKey(messagedata.Text.Trim().ToLower()))
                            {
                                await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.SomeGameOfGenre(messagedata.Text.Trim().ToLower()));
                                var game = await _service.GetRandomGameMod(_modes[messagedata.Text.ToLower().Trim()]);
                                await SendInfoAboutGame(chatId, game);
                                
                                _singleDataForUser[chatId] = game.name;
                            }
                            else if (messagedata.Text.Trim().ToLower() == "cancel")
                            {
                                await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.SimpleCancelAnswer());
                                _botWaitsForQuery.Remove(chatId);
                            }
                            else
                            {
                                await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.WrongChoiceMessage());
                            }
                            break;
                        }
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
                      //  Repository.Game title = _service.SearchGame("Battlefield-1");
                      //  _singleDataForUser[chatId] = title.name;
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
                    case "/getinforamation":
                    {
                        await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.ShowInforamation());
                        _botWaitsForQuery[chatId] = QueryAction.GameSearching;
                        break;

                    }
                    //case "/getnowplaying":
                    //    {
                    //        var movies = await _service.GetNowPlaying();
                    //        await _client.SendMessageAsync(MessageType.TextMessage, chatId, BotAnswers.NowPlayingIntroduction());
                    //        await _client.SendMessageAsync(MessageType.TextMessage, chatId, BotAnswers.NowPlayingAnswer(movies));
                    //        await _client.SendMessageAsync(MessageType.TextMessage, chatId, BotAnswers.MovieSearchAdvice());
                    //        break;
                    //    }
                    case "getbygamemod":
                        {
                            _modes = await _service.GetGameMod();
                            genresWasDownloaded = _modes != null && _modes.Count != 0;
                            await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.GenresInjection());
                            await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.GameModAnswer(_modes.Keys.ToList()));
                            await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.GenresChoose());
                            _botWaitsForQuery[chatId] = QueryAction.GameModSelecting;
                            break;
                        }
                    case "/getsimilars":
                        {
                            await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.GetSimilar());
                            _searchingSimilarUsers.Add(chatId);
                            goto case "/searchgames";
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

            await _client.SendChatAction(chatId, ChatAction.Uploading_Photo);
            await _client.SendPhotoAsync(chatId,$"Poster to ''{game.name}''");
            await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.GetGameInformMessage(game));
            //if (game._IGDBlink != null)
            //    await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.GameIGDBApp(game._IGDBlink));
        }

        public async Task SendInfoAboutCharacter(long chatId, string name)
        {
            var characters = _service.SearchCharacters(name);
            if (characters != null && characters.Count != 0)
            {
                await _client.SendChatAction(chatId, ChatAction.Uploading_Photo);
                await _client.SendPhotoAsync(chatId, characters[0].Poster);
                await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.SingleCharactersAnswer(characters[0]));
                await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.GamesSearchAdvice());
            }
            else await _client.SendMessageAsync(MessageAction.MesText, chatId, TelegamBotAnswers.NotFoundMessage());

        }

    }
}
