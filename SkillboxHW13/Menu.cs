using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SkillboxHW13
{
    public class Menu
    {
        Client currientClient;
        Manager manager;
        Bank bank;
        public Menu(Bank bank, Manager manager)
        {
            this.bank = bank;
            this.manager = manager;
        }

        public void OpenPageStart()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("What would you like to do?\n" +
                                  "1) Register a new client\n" +
                                  "2) Choose a client ");
                ConsoleKeyInfo number = Console.ReadKey(true);
                switch (number.Key)
                {
                    case ConsoleKey.D1: manager.RegisterClient(); Success(); break;
                    case ConsoleKey.D2:
                        {
                            currientClient = ChooseClient(bank, manager);
                            Console.WriteLine($"Client with id {currientClient.Id} has been chosen!\n\n");
                            Thread.Sleep(2000);
                            OpenPageClientAction();
                            Console.Clear();
                            break;
                        }
                    default: break;
                }

            }
        }

        private void OpenPageClientAction()
        {
            bool condition = true;
            do
            {

                Console.Clear();
                Console.WriteLine("What would you like to do?\n" +
                                  "1) Look all bank accounts\n" +
                                  "2) Add new bank account\n" +
                                  "3) Remove an account\n" +
                                  "4) Look balance for today\n" +
                                  "0) Back to previous page");
                ConsoleKeyInfo number = Console.ReadKey(true);
                switch (number.Key)
                {
                    case ConsoleKey.D1: PrintClientAccounts(); break;
                    case ConsoleKey.D2: OpenPageCreateAccount(); break;
                    case ConsoleKey.D3: manager.RemoveBankAccount(currientClient, OpenPageBankAccounts()); break;
                    case ConsoleKey.D4: OpenPageBalance(); break;
                    case ConsoleKey.D0: condition = false; break;
                    default: break;
                }
            } while (condition);
        }
        BankAccount OpenPageBankAccounts()
        {
            Console.Clear();
            PrintClientAccounts();
            Console.Write("\n\nType id of account what you need: ");
            try
            {
                return currientClient.BankAccounts.Find(account => account.Id == GetIntFromConsole());
            }
            catch (Exception)
            {
                OpenPageWarning("There is no accounts with this ID!\nTry again...");
                return OpenPageBankAccounts();
            }
        }
        void OpenPageBalance()
        {
            BankAccount bankAccount = OpenPageBankAccounts();
            Console.Clear();
            Console.WriteLine($"Balance of account id{bankAccount.Id} is {bankAccount.Balance}\nPress any kay to continue...");
            Console.ReadKey();
        }
        public static bool OpenPageCapitalization()
        {
            Console.Write("Is deposit capitalized? y/n: ");
            ConsoleKeyInfo number = Console.ReadKey(true);
            switch (number.Key)
            {
                case ConsoleKey.Y: return true;
                case ConsoleKey.N: return false;
                default: Console.Write("\b"); return OpenPageCapitalization();
            }
        }
        public static void OpenPageWarning(string message)
        {
            Console.Clear();
            Console.WriteLine(message);
            Thread.Sleep(3000);
        }
        private void OpenPageCreateAccount()
        {
            bool condition = true;
            do
            {

                Console.Clear();
                Console.WriteLine("What account would you like to open?\n" +
                                  "1) Deposit\n" +
                                  "2) Credit\n" +
                                  "0) Back to previous page");
                ConsoleKeyInfo number = Console.ReadKey(true);
                switch (number.Key)
                {
                    case ConsoleKey.D1: manager.OpenNewAccount(currientClient, 1); Success(); break;
                    case ConsoleKey.D2: manager.OpenNewAccount(currientClient, 2); Success(); break;
                    case ConsoleKey.D0: condition = false; break;
                    default: ; break;
                }
            } while (condition);
        }
        public static void Success()
        {
            Console.Clear();
            Console.WriteLine("Success!");
            Thread.Sleep(2500);
        }
        public static int OpenPageMoney()
        {
            Console.Clear();
            Console.Write("Type count of money and press Enter: ");
            return GetIntFromConsole();
        }
        public static int OpenPageMounths()
        {
            Console.Clear();
            Console.Write("Type count of mounths and press Enter: ");
            return GetIntFromConsole();
        }
        private void PrintClientAccounts()
        {
            Console.Clear();
            foreach (var account in currientClient.BankAccounts)
            {
                Console.WriteLine($"{account.ToString()} - id{account.Id}");
            }
            Console.WriteLine("Press any kay to continue...");
            Console.ReadKey();
        }
        private Client ChooseClient(Bank bank, Manager manager)
        {
            Client currientClient;
            Console.Clear();
            PrintAllClients(bank);
            Console.Write("\n\nWrite client's id and press Enter: ");
            int id = GetIntFromConsole();
            currientClient = manager.ChooseCliehtById(id);
            if (currientClient is not null)
            {
                Console.Clear();
                return currientClient;
            }
            else
            {
                Console.Clear();
                OpenPageWarning("There is no clients with this id!\nTry again...");
                return ChooseClient(bank, manager);
            }
        }

        void PrintAllClients(Bank bank)
        {
            foreach (var client in bank.Clients)
            {
                Console.WriteLine($"{client.Id}");
            }
        }
        static int GetIntFromConsole()
        {
            bool condition = true;
            List<char> charCollection = new List<char>();
            do
            {
                ConsoleKeyInfo kay = Console.ReadKey();

                char number = kay.KeyChar;
                if (char.IsNumber(number))
                {
                    charCollection.Add(number);
                }
                else if (kay.Key == ConsoleKey.Enter)
                {
                    condition = false;
                }
                else Console.Write("\b \b");

            } while (condition);
            
            string s  = new(charCollection.ToArray());
            return int.Parse(s);
        }
    }
}
