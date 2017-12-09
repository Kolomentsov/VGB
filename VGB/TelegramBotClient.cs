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

namespace VGB
{
    class TelegramBotClient : ITelegramBot
    {

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
