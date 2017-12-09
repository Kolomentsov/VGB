using IGamesData.BotData;
using IGamesData.BotData.ActionWithBot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VGB.Interfaces
{
    interface ITelegramBot
    {
        event Action<MessageData> OnMessageReceived;

        void StartBot();
        Task<bool> TestBot();

        Task<UserData> GetMeAsync();

        Task<DataUpdate[]> GetUpdatesAsync();

        Task<MessageData> SendMessageAsync(MessageAction action, long chatId, string content,
            Dictionary<string, object> parameters = null);

        Task<MessageData> SendPhotoAsync(long chatId, string path, string caption = "");

        Task<MessageData> SendStickerAsync(long chatId, string stickerId);

        Task<bool> SendChatAction(long chatId, ChatAction action);
    }
}
