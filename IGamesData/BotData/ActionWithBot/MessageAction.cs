using System;
using System.Collections.Generic;

namespace IGamesData.BotData.ActionWithBot
{
    public enum MessageAction
    {

        MesText,
        MesPhoto,
        MesVideo,
        MesSticker,
    }

    public static class MessageAddition
    {
        public static KeyValuePair<string, string> ToKeyValue(this MessageAction message)
        {
            switch (message)
            {
                case MessageAction.MesText:
                    return new KeyValuePair<string, string>("sendMessage", "text");

                case MessageAction.MesPhoto:
                    return new KeyValuePair<string, string>("sendPhoto", "photo");

                case MessageAction.MesVideo:
                    return new KeyValuePair<string, string>("sendVideo", "video");

                case MessageAction.MesSticker:
                    return new KeyValuePair<string, string>("sendSticker", "sticker");

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
