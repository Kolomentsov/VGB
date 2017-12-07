using System;
using System.Collections.Generic;

namespace IGamesData.BotData.ActionWithBot
{
    public enum MessageAction
    {

        Text,
        Photo,
        Video,
        Sticker,
    }

    public static class MessageAddition
    {
        public static KeyValuePair<string, string> ToKeyValue(this MessageAction message)
        {
            switch (message)
            {
                case MessageAction.Text:
                    return new KeyValuePair<string, string>("sendMessage", "text");

                case MessageAction.Photo:
                    return new KeyValuePair<string, string>("sendPhoto", "photo");

                case MessageAction.Video:
                    return new KeyValuePair<string, string>("sendVideo", "video");

                case MessageAction.Sticker:
                    return new KeyValuePair<string, string>("sendSticker", "sticker");

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
