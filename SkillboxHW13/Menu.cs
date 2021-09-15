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
        Bank bank;
        Manager manager;
        Client currientClient;
        Random random = new Random();
        public event Action<object, string> menuEvent;

        public Menu(Bank bank, Manager manager)
        {
            this.bank = bank;
            this.manager = manager;
            manager.SetMoneyGetter(OpenPageMoney);
            manager.SetMounthsGetter(OpenPageMounths);
            manager.SetClientTypeGetter(OpenPageGetClientType);
            manager.SetCapitalizationGetter(OpenPageCapitalization);
            manager.SetNameGetter(GetRandomName);
            manager.sendMessageFromManager += OpenPageWarning;
        }

        #region Страницы меню
        /// <summary>
        /// Открывает страницу баланса счёта клиента
        /// </summary>
        void OpenPageBalance()
        {
            BankAccount bankAccount = OpenPageBankAccounts();
            Console.Clear();
            if (bankAccount is not null)
            {
                Console.WriteLine($"Balance of account id{bankAccount.Id} is {bankAccount.Balance}\nPress any kay to continue...");

            }
            else Console.WriteLine("Press any kay to continue...");
            Console.ReadKey();
        }
        /// <summary>
        /// Открывает траницу выбора действия с клиентом
        /// </summary>
        void OpenPageClientAction()
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
                    case ConsoleKey.D3:
                        {
                            BankAccount temp = OpenPageBankAccounts();
                            if (temp is not null)
                            {
                                manager.RemoveBankAccount(currientClient, temp); break;
                            }
                            else 
                            {
                                OpenPageWarning("Chosen account is null");
                                menuEvent(this, "Chosen account is null");
                                break;
                            }

                        } 
                    case ConsoleKey.D4: OpenPageBalance(); break;
                    case ConsoleKey.D0: condition = false; break;
                    default: break;
                }
            } while (condition);
        }
        /// <summary>
        /// Открывает страницу содания банковского счёта
        /// </summary>
        void OpenPageCreateAccount()
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
        /// <summary>
        /// Открывает страницу для выбора банковского счёта, печатает список счетов клиента
        /// </summary>
        /// <returns>Выбранный банковсий счёт</returns>
        BankAccount OpenPageBankAccounts()
        {
            Console.Clear();
            if (currientClient.BankAccounts is not null)
            {
                PrintClientAccounts();
            }
            else
            {
                Console.WriteLine("This client has no bank accounts");
                return null;
            }
            Console.Write("\n\nType id of account what you need: ");
            try
            {
                return currientClient.BankAccounts.Find(account => account.Id == GetIntFromConsole());
            }
            catch (Exception)
            {
                OpenPageWarning("There is no accounts with this ID!\nTry again...");
                Thread.Sleep(1000);
                return OpenPageBankAccounts();
            }
        }
        /// <summary>
        /// Открывает начальную страницу меню
        /// </summary>
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
        /// <summary>
        /// Открывает страницу для ввода денежной суммы
        /// </summary>
        /// <returns>Целое число</returns>
        public static int OpenPageMoney()
        {
            Console.Clear();
            Console.Write("Type count of money and press Enter: ");
            return GetIntFromConsole();
        }
        /// <summary>
        /// Открывает страницу для ввода количества месяцев
        /// </summary>
        /// <returns>Целое число</returns>
        public static int OpenPageMounths()
        {
            Console.Clear();
            Console.Write("Type count of mounths and press Enter: ");
            return GetIntFromConsole();
        }
        /// <summary>
        /// Открывает страницу выбора типа клиента
        /// </summary>
        /// <returns>Возврат числа от 1 до 3 для дальшейшего использования в качестве переключателя swich</returns>
        public static int OpenPageGetClientType()
        {
            Console.Clear();
            Console.WriteLine("Chose a kind of a client:\n" +
                              "1) Regular client\n" +
                              "2) VIP client\n" +
                              "3) Entity client");
            while (true)
            {
                ConsoleKeyInfo number = Console.ReadKey(true);
                switch (number.Key)
                {
                    case ConsoleKey.D1: return 1;
                    case ConsoleKey.D2: return 2;
                    case ConsoleKey.D3: return 3;
                    default: Console.Write("\b"); break;
                }
            }
        }
        /// <summary>
        /// Открывает страницу выбора типа депозита
        /// </summary>
        /// <returns>Возврат лочгического значения для дальшейшего использования</returns>
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
        /// <summary>
        /// Открывает страницу для вывода ошибки
        /// </summary>
        /// <param name="message">Сообщение ошибки</param>
        public static void OpenPageWarning(string message)
        {
            Console.Clear();
            Console.WriteLine(message);
            Thread.Sleep(3000);
        }
        /// <summary>
        /// Дописывает в конце строки сообщение об успесшной работе
        /// </summary>
        public static void Success()
        {
            Console.WriteLine("Success!");
            Thread.Sleep(2500);
        }
        #endregion

        
        /// <summary>
        /// Выдаёт одно из имен в списке
        /// </summary>
        /// <returns>Случайно выбранное имя</returns>
        string GetRandomName()
        {
            int numerOfRandomName = random.Next(1, 21);
            switch (numerOfRandomName)
            {
                case 1: return "Виктория";
                case 2: return "Пётр";
                case 3: return "Анна";
                case 4: return "Алексей";
                case 5: return "Али";
                case 6: return "Мария";
                case 7: return "Глеб";
                case 8: return "Тамара";
                case 9: return "Виктор";
                case 10: return "Евгения";
                case 11: return "Евгений";
                case 12: return "Мартина";
                case 13: return "Антон";
                case 14: return "Ксения";
                case 15: return "Георг";
                case 16: return "Ольга";
                case 17: return "Максим";
                case 18: return "Юлия";
                case 19: return "Сергей";
                case 20: return "Майя";
               default: return "";
            }
        }
        /// <summary>
        /// Выводит в консоль список счетов клиента
        /// </summary>
        void PrintClientAccounts()
        {
            try
            {

                if (currientClient.BankAccounts is null)
                {
                    throw new BankAccountException();
                }
                    Console.Clear();
                    foreach (var account in currientClient.BankAccounts)
                    {
                        Console.WriteLine($"{account.ToString()} - id{account.Id}");
                    }
                    Console.WriteLine("Press any kay to continue...");
                    Console.ReadKey();
            }
            catch (BankAccountException e)
            {
                OpenPageWarning(e.Message);

            }
            catch (Exception e)
            {
                OpenPageWarning("Error! "+e.Message);
            }            
        }
        /// <summary>
        /// Выводит в консоль всех клиентов данного банка
        /// </summary>
        /// <param name="bank">Банк с которым производится работа</param>
        void PrintAllClients(Bank bank)
        {
            foreach (var client in bank.Clients)
            {
                Console.WriteLine($"{client.Id}");
            }
        }
        /// <summary>
        /// Выбирает клиента из всех клиентов банка
        /// </summary>
        /// <param name="bank">Банк с которым производится работа</param>
        /// <param name="manager">Проводящий операцию менеджер</param>
        /// <returns></returns>
        Client ChooseClient(Bank bank, Manager manager)
        {
            Client currientClient;
            Console.Clear();
            PrintAllClients(bank);
            Console.Write("\n\nWrite client's id and press Enter: ");
            int id = GetIntFromConsole();
            currientClient = manager.ChooseCliehtById(id);

            try
            {
                if (currientClient is null)
                {
                    throw new ClientException();
                }
                    Console.Clear();
                    return currientClient;
            }
            catch (ClientException e)
            {
                OpenPageWarning(e.Message);
                Console.Clear();
                OpenPageWarning("There is no clients with this id!\nTry again...");
                return ChooseClient(bank, manager);
            }
            catch(Exception e)
            {
                OpenPageWarning(e.Message);
                return null;
            }
            
            
        }
        /// <summary>
        /// Защищает ввод от нерелевантных символов
        /// </summary>
        /// <returns>Целое число</returns>
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
