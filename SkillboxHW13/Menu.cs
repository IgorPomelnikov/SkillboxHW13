using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillboxHW13
{
    public class Menu
    {
        public Menu(Bank bank, Manager manager)
        {
            Welcome();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("What would you like to do?\n" +
                              "1) Register a new client\n" +
                              "2) Choose a client ");
                ConsoleKeyInfo number = Console.ReadKey(true);
                switch (number.Key)
                {
                    case ConsoleKey.D1: manager.RegisterClient(); break;
                    case ConsoleKey.D2:
                        Console.Clear();
                        PrintAllClients(bank);
                        Console.Write("Write client's id and press Enter: ");
                        int id = GetIdFromConsole();
                        manager.ChooseCliehtById(id);
                        break;
                    default: Console.Write("\b"); break;
                }

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
            bool condition = true;
            List<char> id = new List<char>();
            do
            {
                ConsoleKeyInfo kay = Console.ReadKey();

                char number = kay.KeyChar;
                if (char.IsNumber(number))
                {
                    id.Add(number);
                }
                else if (kay.Key == ConsoleKey.Enter)
                {
                    condition = false;
                }
                else Console.Write("\b \b");

            } while (condition);
            
            string s  = new(id.ToArray());
            return int.Parse(s);
        }
    }
}
