using IGamesData.BotData;
using IGamesData.BotData.ActionWithBot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VGB.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Linq;

namespace VGB
{
    class TelegramBotClient : ITelegramBot
    {
       /* private TelegramBotClient client;

        private IGamesData.StorageDB.Database db;

        private Dictionary<int, BotSession> state;

        private System.Timers.Timer timer;

        /// <summary>
        /// Конструктор
        /// </summary>
        public TelegramBotClient()
        {
            //client = new TelegramBotClient("506656231:AAFEDn50G7eeyA4d2r7h5dVe8zVAlqCtAts");
           // client.OnMessage += MessageProcessor;
            //db = IGamesData.StorageDB.Database.Open()
            state = new Dictionary<int, BotSession>();
            timer = new System.Timers.Timer();

            timer.Interval = 1000;
            // Многократный запуск  
            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;
        }
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            DateTime now = DateTime.Now;

            foreach (KeyValuePair<int, BotSession> pair in state)
            {
                // Проверка истечения таймаута состояния 
                // Таймаут задан в конфигурации в секундах
                if (((now - pair.Value.TimeStamp).TotalSeconds > Properties.Settings.Default.StateTimeout && (pair.Value.State != BotState.BaseStateBot)))
                {
                   // client.SendMessageAsync1(pair.Value.ChatID, "Похоже, вы про меня забыли, попробуйте позже");
                    pair.Value.State = BotState.BaseStateBot;
                    pair.Value.TimeStamp = now;
                }
            }
        }

        /// <summary>
        /// Текущее состояние пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns></returns>
        private BotState GetState(int userId)
        {
            return state.ContainsKey(userId) ? state[userId].State : BotState.BaseStateBot;
        }

        /// <summary>
        /// Установить состояние пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="newState">Новое состояние</param>
        private void SetState(int userId, BotState newState = BotState.BaseStateBot)
        {
            // Проверка на существование состояния
            if (!state.ContainsKey(userId))
            {
                state[userId] = new BotSession(userId, newState);
            }
            else
            {
                // Установка нового состояния и обновление метки времени
                state[userId].State = newState;
                state[userId].TimeStamp = DateTime.Now;
            }
        }

        /// <summary>
        /// Обработка получаемых сообщений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageProcessor(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            try
            {
                // Протоколирование (если разрешено в конфигурации)
                if (Properties.Settings.Default.Verbose)
                {
                    Console.WriteLine("{0} {1}: {2}", e.Message.From.FirstName, e.Message.From.LastName, e.Message.Text);
                }
                // Проверка типа сообщения
                // Если в классе присутствует публичный метод с именем <тип сообщения>Processor
                // он будет вызван как обработчик сообщения соответствующего типа, иначе
                // будет выведено информационное сообщение                   
                string processor = e.Message.Type.ToString() + "Processor";
                System.Reflection.MethodInfo info = GetType().GetMethod(processor);
                if (info != null)
                {
                    // Вызов метода по имени с заданным параметром
                    info.Invoke(this, new object[] { e.Message });
                }
                else
                {
                   // client.SendMessageAsync(e.Message.Chat.Id, "Я пока не понимаю это: " + e.Message.Type);
                }
            }
            catch (Exception ex)
            {
               // client.SendMessageAsync(e.Message.Chat.Id, "К сожалению, у меня проблема: " + ex.Message);
                // Вывод всех вложенных исключений
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                   // client.SendMessageAsync(e.Message.Chat.Id, "Дополнительная информация: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Обработка текстовых сообщений
        /// </summary>
        /// <param name="msg">Сообщение</param>
        public void TextMessageProcessor(Message msg)
        {
            string message;

            // Проверка на команду
            if (msg.Text.Substring(0, 1) == "/")
            {
                CommandProcessor(msg, msg.Text.Substring(1));
            }
            else
            {
                // Селектор состояний
                switch (GetState(msg.From.Id))
                {
                    case BotState.BaseStateBot:
                        message = string.Format("Привет, {0}!", msg.From.FirstName);
                      //  client.SendMessageAsync(msg.Chat.Id, message);
                        break;

                    case BotState.Registration:
                        if (string.IsNullOrEmpty(state[msg.From.Id].PhoneNumber))
                        {
                           // client.SendMessageAsync(msg.Chat.Id, "Отправьте, пожалуйста, Ваш номер телефона");
                            break;
                        }
                        bool emailFound = false;
                        foreach (var entity in msg.Entities)
                        {
                            if (entity.Type == Telegram.Bot.Types.Enums.MessageEntityType.Email)
                            {
                                // Регистрация пользователя
                                IGamesData.StorageDB.User student = new IGamesData.StorageDB.User()
                                {
                                    ID = Guid.NewGuid(),
                                    UserID = msg.From.Id,
                                    Name = msg.From.FirstName,
                                    FamilyName = msg.From.LastName,
                                    UserName = msg.From.Username,
                                    EMail = msg.Text.Substring(entity.Offset, entity.Length),
                                    PhoneNumber = state[msg.From.Id].PhoneNumber
                                };
                                db.Users.Add(student);
                                db.SaveChanges();
                                // E-Mail нашелся
                                message = string.Format("Спасибо, {0}, регистрация выполнена!", msg.From.FirstName);
                             //   client.SendMessageAsync(msg.Chat.Id, message);
                                emailFound = true;
                                // Возвращение к базовому состоянию
                                SetState(msg.From.Id, BotState.BaseStateBot);
                                break;
                            }
                        }
                        if (!emailFound)
                        {
                            message = string.Format("{0}, для завершения регистрации отправьте, пожалуйста, E-mail", msg.From.FirstName);
                           // client.SendMessageAsync(msg.Chat.Id, message);
                        }
                        break;
                }
            }
        }
        /// <param name="msg">Сообщение</param>
        /// <param name="cmd">Команда</param>
        public void CommandProcessor(Message msg, string cmd)
        {
            // Имя метода для обработки команды
            string name = string.Format("{0}Command", cmd.ToLower());
            System.Reflection.MethodInfo info = GetType().GetMethod(name);
            if (info != null)
            {
                // Вызов метода по имени с заданным параметром
                info.Invoke(this, new object[] { msg });
            }
            else
            {
              //  client.SendMessageAsync(msg.Chat.Id, "Неизвестная команда: " + cmd);
            }
        }

        /// <summary>
        /// Команда начала работы с ботом
        /// </summary>
        /// <param name="msg">Сообщение</param>
        public void startCommand(Message msg)
        {
          //  client.SendTextMessageAsync(msg.Chat.Id, string.Format("Привет, {0}, я учебный бот по языку C#", msg.From.FirstName));
        }

        /// <summary>
        /// Отметить присутствие студента на занятии
        /// </summary>
        /// <param name="msg">Сообщение</param>
        public void checkinCommand(Message msg)
        {
            // Кнопка для регистрации
            KeyboardButton b = new KeyboardButton("Отметиться")
            {
                RequestLocation = true
            };
            // Панель кнопок
            KeyboardButton[] keys = new KeyboardButton[1] { b };
            // Разметка ответа
            var markup = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup(keys, true, true);
            // Ответ
          //  client.SendMessageAsync(msg.Chat.Id, content:"Предоставьте регистрационные данные", ReplyMarkups: markup);
        }

        /// <summary>
        /// Зарегистрировать студента в списке
        /// </summary>
        /// <param name="msg">Сообщение</param>
        public void registerCommand(Message msg)
        {
            IGamesData.StorageDB.User student = db.Users.Where(a => a.UserID == msg.From.Id).FirstOrDefault();
            if (student != null)
            {
               // client.SendMessageAsync(msg.Chat.Id, string.Format("{0}, вы уже зарегистрированы, ваш E-mail: {1}. Для редактирования профиля используется команда /profile", msg.From.FirstName, student.EMail));
            }
            else
            {
                // Кнопка для регистрации с запросом номера телефона
                KeyboardButton b = new KeyboardButton("Отправить телефон")
                {
                    RequestContact = true
                };
                // Панель кнопок
                KeyboardButton[] keys = new KeyboardButton[1] { b };
                // Разметка ответа
                var markup = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup(keys, true, true);
                //client.SendMessageAsync(msg.Chat.Id, "Для регистрации отправьте мне телефон и после него E-mail", replyMarkup: markup);
                // Переход в состояние регистрации пользователя
                SetState(msg.From.Id, BotState.Registration);
                state[msg.From.Id].PhoneNumber = null;
            }
        }
       /// <summary>
        /// Обработка полученного контакта
        /// </summary>
        /// <param name="msg"></param>
        public void ContactMessageProcessor(Message msg)
        {
            switch (GetState(msg.From.Id))
            {
                case BotState.Registration:
                    if (!string.IsNullOrEmpty(state[msg.From.Id].PhoneNumber))
                    {
                      //  client.SendMessageAsync(msg.Chat.Id, "Вы уже отправляли номер телефона, теперь отправьте Ваш E-mail");
                    }
                    else
                    {
                        state[msg.From.Id].PhoneNumber = msg.Contact.PhoneNumber;
                       // client.SendMessageAsync(msg.Chat.Id, "Спасибо, теперь отправьте Ваш E-mail");
                    }
                    break;

                default:
                  //  client.SendMessageAsync(msg.Chat.Id, "Извините, не понимаю, что надо сделать с этим контактом");
                    break;
            }
        }

        /// <summary>
        /// Обработка метки геолокации
        /// </summary>
        /// <param name="msg">Сообщение с меткой геолокации</param>
        public void LocationMessageProcessor(Message msg)
        {
        }
        public void DocumentMessageProcessor(Message msg)
        {
            string s = string.Format("Я получил файл {0}", msg.Document.FileName);
            //client.SendMessageAsync(msg.Chat.Id, chatId: 1);
        } */







        public event Action<MessageData> OnMessageReceived;
        public event Action<string> LogMessage;

        private readonly string _token;
        private const string _baseUrl = "https://api.telegram.org/bot";
        private long _offset = 0;
        public TelegramBotClient(string token)
        {
            _token = token;

            OnMessageReceived += m =>
            {
                LogMessage($"Message from {m.User.FirstName} {m.User.LastName} with chat_id {m.Chat.Id} was recieved: {m.Text}");
            };
        }

        public async void StartBot()
        {
            LogMessage?.Invoke("The bot is running");
            while (true)
            {
                try
                {
                    var updates = await GetUpdatesAsync();
                    foreach (var update in updates)
                    {
                        if (update.Message.Text != null && update.Message.Text != String.Empty)
                            OnMessageReceived?.Invoke(update.Message);

                        _offset = update.Id + 1;
                    }
                }
                catch (Exception)
                {
                    Thread.Sleep(10000);
                }
            }
        }


        public async Task<bool> TestBot()
        {
            try
            {
                var response = await GetMeAsync();
                return response != null;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<UserData> GetMeAsync() => await SendWebRequest<UserData>("getMe");

        public async Task<DataUpdate[]> GetUpdatesAsync()
        {
            var parameters = new Dictionary<string, object>
            {
                {"offset", _offset},
            };
            return await SendWebRequest<DataUpdate[]>("getUpdates", parameters);
        }



        public Task<MessageData> SendMessageAsync(MessageAction type, long chatId, string content,
     Dictionary<string, object> parameters = null)
        {
            if (parameters == null)
                parameters = new Dictionary<string, object>();

            var typeInfo = type.ToKeyValue();

            parameters.Add("chat_id", chatId);
            if (!string.IsNullOrEmpty(typeInfo.Value))
                parameters.Add(typeInfo.Value, content);
            LogMessage($"Message to {chatId} was sent");

            return SendWebRequest<MessageData>(typeInfo.Key, parameters);
        }


        public async Task<MessageData> SendPhotoAsync(long chatId, string path, string caption = "")
        {
            var parameters = new Dictionary<string, object>
            {
                {"caption", caption }
            };
            LogMessage($"Photo to {chatId} was sent");
            return await SendMessageAsync(MessageAction.MesPhoto, chatId, path, parameters);
        }


        public async Task<MessageData> SendStickerAsync(long chatId, string stickerId)
            => await SendMessageAsync(MessageAction.MesSticker, chatId, stickerId);

        public async Task<bool> SendChatAction(long chatId, ChatAction action)
        {
            var parameters = new Dictionary<string, object>
            {
                {"chat_id", chatId},
                {"action",action.ToString() }
            };
            return await SendWebRequest<bool>("sendChatAction", parameters);
        }


        public async Task<T> SendWebRequest<T>(string methodName, Dictionary<string, object> parameters = null)
        {
            var uri = new Uri($"{_baseUrl}{_token}/{methodName}");
            DataResponse<T> responseObject = null;

            using (var client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response;

                    if (parameters == null || parameters.Count == 0)
                    {
                        response = await client.GetAsync(uri);
                    }
                    else
                    {
                        var data = JsonConvert.SerializeObject(parameters);
                        var httpContent = new StringContent(data, Encoding.UTF8, "application/json");
                        response = await client.PostAsync(uri, httpContent);
                    }
                    var resultStr = await response.Content.ReadAsStringAsync();
                    responseObject = JsonConvert.DeserializeObject<DataResponse<T>>(resultStr);
                }
                catch (HttpRequestException e)
                {
                    LogMessage?.Invoke($"Unable to provide operation {methodName}. Exception occured: {e.Message}");
                    throw new HttpRequestException(e.Message);
                }
            }
            return responseObject.Result;
        }
    }
}