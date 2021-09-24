using BankAccountLibrary;
using ClientsLibrary;
using System;
using System.Collections.Generic;
using System.Threading;


namespace SkillboxHW13
{

    public class Menu
    {
        Bank bank;
        Manager manager;
        Client currientClient;
        Random random = new Random();
        public static event Action<string> menuEvent;

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
            menuEvent?.Invoke("Открыта страница просмотра баланса");
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
                menuEvent?.Invoke("Открыта страница выбора действия с клиентом");
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
                    case ConsoleKey.D1: 
                        PrintClientAccounts(); 
                        break;
                    case ConsoleKey.D2: 
                        OpenPageCreateAccount(); 
                        break;
                    case ConsoleKey.D3:
                        BankAccount temp = OpenPageBankAccounts();
                        try
                        {

                            if (temp is null)
                            {
                                throw new BankAccountException();
                            }

                            manager.RemoveBankAccount(currientClient, temp);
                            menuEvent?.Invoke("");
                            break;
                        }
                        catch (BankAccountException)
                        {
                            OpenPageWarning("Chosen account is null");
                            menuEvent?.Invoke("Ошибка. Выбранный аккаунт равен null");
                            break;
                        }

                    case ConsoleKey.D4: 
                        OpenPageBalance(); 
                        menuEvent?.Invoke("Открыта страница просмотра баланса"); 
                        break;

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
                menuEvent?.Invoke("Открыта страница создания аккаунта");

                Console.Clear();
                Console.WriteLine("What account would you like to open?\n" +
                                  "1) Deposit\n" +
                                  "2) Credit\n" +
                                  "0) Back to previous page");
                ConsoleKeyInfo number = Console.ReadKey(true);
                switch (number.Key)
                {
                    case ConsoleKey.D1: 
                        menuEvent?.Invoke("Создание депозитного счёта");
                        manager.OpenNewAccount(currientClient, BankAccountTypes.Deposit);
                        Success(); break;
                    case ConsoleKey.D2:
                        menuEvent?.Invoke("Создание кредитного счёта");
                        manager.OpenNewAccount(currientClient, BankAccountTypes.Credit); 
                        Success(); break;
                    case ConsoleKey.D0: 
                        condition = false;                       
                        break;
                    default:; break;
                }
            } while (condition);
        }
        /// <summary>
        /// Открывает страницу для выбора банковского счёта, печатает список счетов клиента
        /// </summary>
        /// <returns>Выбранный банковсий счёт</returns>
        BankAccount OpenPageBankAccounts()
        {
            menuEvent?.Invoke("Открыта страница выбора банковского счёта");
            Console.Clear();
            if (currientClient.BankAccounts is not null)
            {
                PrintClientAccounts();
            }
            else
            {
                OpenPageWarning("This client has no bank accounts");
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
                menuEvent?.Invoke("Открыта начальная страница");
                Console.Clear();
                Console.WriteLine("What would you like to do?\n" +
                                  "1) Register a new client\n" +
                                  "2) Choose a client\n" +
                                  "3) Auto mode of creation 1 000 000 clients with deposits\n"); 
                ConsoleKeyInfo number = Console.ReadKey(true);
                switch (number.Key)
                {
                    case ConsoleKey.D1: manager.RegisterClient(1); Success(); break;
                    case ConsoleKey.D2:
                        {
                            currientClient = ChooseClient(bank, manager);
                            try
                            {
                                if (currientClient is null) throw new ClientException();
                                Console.WriteLine($"Client with id {currientClient.Id} has been chosen!\n\n");
                                Thread.Sleep(2000);
                                OpenPageClientAction();
                                Console.Clear();
                                break;

                            }
                            catch (ClientException e)
                            {
                                OpenPageWarning(e.Message);
                                OpenPageStart();
                                break;
                            }
                        }
                    case ConsoleKey.D3: manager.RegisterClient(2); OpenPageWarning("Производится автоматическое создание клинетов в фоновом режиме"); break;
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
            menuEvent?.Invoke("Открыта страница ввода суммы");
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
            menuEvent?.Invoke("Открыта страница выбора количества месяцев");
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
            menuEvent?.Invoke("Открыта страница выбора типа клиента");
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
                    case ConsoleKey.D1: menuEvent?.Invoke("Выбран клиент Regular"); return 1;
                    case ConsoleKey.D2: menuEvent?.Invoke("Выбран клиент VIP"); return 2;
                    case ConsoleKey.D3: menuEvent?.Invoke("Выбран клиент Entity"); return 3;
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
            menuEvent?.Invoke("Открыта страница выбора капитализации");
            Console.Clear();
            Console.Write("Is deposit capitalized? y/n: ");
            ConsoleKeyInfo number = Console.ReadKey(true);
            switch (number.Key)
            {
                case ConsoleKey.Y: menuEvent?.Invoke("Установлен параметр капитализации в true"); return true;
                case ConsoleKey.N: menuEvent?.Invoke("Установлен параметр капитализации в false"); return false;
                default: Console.Write("\b"); return OpenPageCapitalization();
            }
        }
        /// <summary>
        /// Открывает страницу для вывода ошибки
        /// </summary>
        /// <param name="message">Сообщение ошибки</param>
        public static void OpenPageWarning(string message)
        {
            menuEvent?.Invoke(message);
            Console.Clear();
            Console.WriteLine(message);
            Thread.Sleep(1000);
        }
        /// <summary>
        /// Дописывает в конце строки сообщение об успесшной работе
        /// </summary>
        public static void Success()
        {
            Console.Clear();
            menuEvent?.Invoke("Успех!");
            Console.WriteLine("Success!");
        }
        #endregion

        public void SubscribeOnMenuEvents(Action<string> logWriter)
        {
            menuEvent += logWriter;
        }

        /// <summary>
        /// Выдаёт одно из имен в списке
        /// </summary>
        /// <returns>Случайно выбранное имя</returns>
        string GetRandomName()
        {
            int numerOfRandomName = random.Next(1, 21);
            string randomName = "";
            switch (numerOfRandomName)
            {
                case 1: randomName = "Виктория"; break;
                case 2: randomName =  "Пётр"; break;
                case 3: randomName =  "Анна"; break;
                case 4: randomName =  "Алексей"; break;
                case 5: randomName =  "Али"; break;
                case 6: randomName =  "Мария"; break;
                case 7: randomName =  "Глеб"; break;
                case 8: randomName =  "Тамара"; break;
                case 9: randomName =  "Виктор"; break;
                case 10: randomName =  "Евгения"; break;
                case 11: randomName =  "Евгений"; break;
                case 12: randomName =  "Мартина"; break;
                case 13: randomName =  "Антон"; break;
                case 14: randomName =  "Ксения"; break;
                case 15: randomName =  "Георг"; break;
                case 16: randomName =  "Ольга"; break;
                case 17: randomName =  "Максим"; break;
                case 18: randomName =  "Юлия"; break;
                case 19: randomName =  "Сергей"; break;
                case 20: randomName =  "Майя"; break;
                default: randomName =  ""; break;
            }
            menuEvent?.Invoke($"Выдано случайное имя ({randomName})пользователю");
            return randomName;
        }
        /// <summary>
        /// Выводит в консоль список счетов клиента
        /// </summary>
        void PrintClientAccounts()
        {
            menuEvent?.Invoke("Вывод в консоль список аккаунтов клиентов");
            try
            {
                if (currientClient.BankAccounts is null)
                {
                    throw new BankAccountException();
                }
                Console.Clear();
                foreach (var account in currientClient.BankAccounts)
                {
                    Console.WriteLine($"{account.ToString(),8} - id {account.Id}");
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
                OpenPageWarning("Error! " + e.Message);
            }
        }
        /// <summary>
        /// Выводит в консоль всех клиентов данного банка
        /// </summary>
        /// <param name="bank">Банк с которым производится работа</param>
        void PrintAllClients(Bank bank)
        {
            //    foreach (var client in bank.Clients)
            //    {
            //        Console.WriteLine($"{client.Id} client.Name");
            //    }
            for (int i = 0; i < bank.Clients.Count; i++)
            {
                Console.WriteLine($"{bank.Clients[i].Id} {bank.Clients[i].Name} ");
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
            menuEvent?.Invoke("Начат процесс выбора клиента");
            Client currientClient;
            Console.Clear();
            PrintAllClients(bank);
            Console.Write("\n\nWrite client's id and press Enter: ");
            int id = GetIntFromConsole();
            currientClient = manager.ChooseCliehtById(id);
            try
            {
                if (id < 0) throw new ClientException();
                if (currientClient is null) throw new NullReferenceException("There is no clients with this id!"); 
                Console.Clear();
                return currientClient;
            }
            catch (ClientException)
            {
                Console.Clear();
                OpenPageWarning("There is no clients!");
                return null;
            }
            catch (NullReferenceException e)
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
            menuEvent?.Invoke("Начат процесс ввода числа из консоли");
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

            string s = new(charCollection.ToArray());
            try
            {
                if (s == "")
                {
                    throw new IndexOutOfRangeException();
                }
                menuEvent?.Invoke("Выбрано число "+s);
                return int.Parse(s);
            }
            catch
            {
                menuEvent?.Invoke("Ошибка, результат -1");
                return -1;
            }
        }
    }
}
