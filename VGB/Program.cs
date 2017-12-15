using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VGB
{
    public class Program
    {
        static void Main(string[] args)
        {
            TelegramBotClient telegramBotClient = new TelegramBotClient("506656231:AAFEDn50G7eeyA4d2r7h5dVe8zVAlqCtAts");
            telegramBotClient.LogMessage += m => Console.WriteLine(m);

            GamesService gamesService = new GamesService();

            BotManager manager = new BotManager(telegramBotClient, gamesService);

            telegramBotClient.StartBot();
            Console.ReadLine();

        }
    }
}
