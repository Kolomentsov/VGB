using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VGB
{
    public class BotSession
    { 
        public BotState State { get; set; }


        public DateTime TimeStamp { get; set; }

        public long ChatID { get; set; }

        public string PhoneNumber { get; set; }

        public BotSession(long chatId, BotState bsb = BotState.BaseStateBot)
        {
            ChatID = chatId;
            State = bsb;
            TimeStamp = DateTime.Now;
        }
    }
}
