using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillboxHW13.BankAccounts
{
    public class Menu
    {
        public Menu(Bank bank, Manager manager)
        {
            Welcome();
            Console.WriteLine("What would you like to do?\n"+
                              "1) Register a new client\n" +
                              "2) Choose a client ");
            ConsoleKeyInfo number = Console.ReadKey(true);
            switch (number.Key)
            {
                case ConsoleKey.D1: manager.RegisterClient(); break;
                case ConsoleKey.D2:  2;
                default: Console.Write("\b"); break;
            }
        }

        private static void Welcome()
        {
            Console.WriteLine("Welcome to \"My Homework Bank\"");
            Wait(3);
            Console.Clear();
        }

        private static void Wait(int seconds)
        {
            Task.Delay(seconds * 1000);
        }
        void PrintAllClients(Bank bank)
        {
            foreach (var client in bank.Clients)
            {
                Console.WriteLine($"{client.Id}");
            }
        }
        int GetIdFromConsole()
        {
            List<char> id = new List<char>();
            ConsoleKeyInfo number = Console.ReadKey();
            switch (number)
            {
                case '1': break;
            }
        }
    }
}
