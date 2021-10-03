using System;
using System.Text;
using System.Threading;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;


namespace SkillboxHW13
{

    public class Menu
    {
        readonly SqlConnectionStringBuilder sqlConnectionString;
        readonly Manager manager;
        public static event Action<string> menuEvent;

        public Menu(SqlConnectionStringBuilder sqlConnectionStringBuilder, Manager manager)
        {
            this.manager = manager;
            sqlConnectionString = sqlConnectionStringBuilder;
            manager.SetMoneyGetter(OpenPageMoney);
            //manager.SetMounthsGetter(OpenPageMounths);
            manager.SetClientTypeGetter(OpenPageGetClientType);
            //manager.SetCapitalizationGetter(OpenPageCapitalization);
            manager.SetNameGetter(GetRandomName);
            manager.sendMessageFromManager += OpenPageWarning;
        }

        #region Страницы меню
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
                                  "0) Back to previous page");

                ConsoleKeyInfo number = Console.ReadKey(true);
                switch (number.Key)
                {
                    case ConsoleKey.D1:
                        PrintClientAccounts(GetClietnId());
                        Pause();
                        Success();
                        break;

                    case ConsoleKey.D2:
                        OpenPageCreateAccount();
                        Pause();
                        Success();
                        break;

                    case ConsoleKey.D3:
                        int clientId = GetClietnId();
                        int bankAccountId = GetBankAccountId(clientId);
                        manager.RemoveBankAccount(clientId, bankAccountId);
                        break;

                    case ConsoleKey.D0:
                        condition = false;
                        break;

                    default:
                        break;
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
                        manager.CreateBankAccount(GetClietnId(), 2);
                        Success(); break;
                    case ConsoleKey.D2:
                        menuEvent?.Invoke("Создание кредитного счёта");
                        manager.CreateBankAccount(GetClietnId(), 1);
                        Success(); break;
                    case ConsoleKey.D0:
                        condition = false;
                        break;
                    default:; break;
                }
            } while (condition);
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
                                  "2) Choose an action with existing clients\n");

                ConsoleKeyInfo number = Console.ReadKey(true);
                switch (number.Key)
                {
                    case ConsoleKey.D1: manager.RegisterClient(); Success(); break;
                    case ConsoleKey.D2: OpenPageClientAction(); break;
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
            Thread.Sleep(5000);
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

        /// <summary>
        /// Выбирает клиента из всех клиентов банка
        /// </summary>
        /// <param name="bank">Банк с которым производится работа</param>
        /// <param name="manager">Проводящий операцию менеджер</param>
        /// <returns></returns>
        int GetClietnId()
        {
            menuEvent?.Invoke("Начат процесс выбора клиента");
            Console.Clear();
            PrintAllClients();
            Console.Write("\n\nWrite client's id and press Enter: ");
            return GetIntFromConsole();

        }

        /// <summary>
        /// Выводит в консоль список счетов клиента
        /// </summary>
        int GetBankAccountId(int clietnId)
        {
            PrintClientAccounts(clietnId);
            return GetIntFromConsole();
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
                menuEvent?.Invoke("Выбрано число " + s);
                return int.Parse(s);
            }
            catch
            {
                menuEvent?.Invoke("Ошибка, результат -1");
                return -1;
            }
        }

        /// <summary>
        /// Выдаёт одно из имен в списке
        /// </summary>
        /// <returns>Случайно выбранное имя</returns>
        public string GetRandomName()
        {
            Random random = new Random();
            int numerOfRandomName = random.Next(1, 21);
            string randomName = "";
            switch (numerOfRandomName)
            {
                case 1: randomName = "Виктория"; break;
                case 2: randomName = "Пётр"; break;
                case 3: randomName = "Анна"; break;
                case 4: randomName = "Алексей"; break;
                case 5: randomName = "Али"; break;
                case 6: randomName = "Мария"; break;
                case 7: randomName = "Глеб"; break;
                case 8: randomName = "Тамара"; break;
                case 9: randomName = "Виктор"; break;
                case 10: randomName = "Евгения"; break;
                case 11: randomName = "Евгений"; break;
                case 12: randomName = "Мартина"; break;
                case 13: randomName = "Антон"; break;
                case 14: randomName = "Ксения"; break;
                case 15: randomName = "Георг"; break;
                case 16: randomName = "Ольга"; break;
                case 17: randomName = "Максим"; break;
                case 18: randomName = "Юлия"; break;
                case 19: randomName = "Сергей"; break;
                case 20: randomName = "Майя"; break;
                default: randomName = ""; break;
            }
            menuEvent?.Invoke($"Выдано случайное имя ({randomName})пользователю");
            return randomName;
        }

        /// <summary>
        /// Выводит в консоль список счетов клиента
        /// </summary>
        void PrintClientAccounts(int clientId)
        {
            menuEvent?.Invoke("Вывод в консоль список аккаунтов клиентов");
            Console.Clear();
            string sqlScypt = $"SELECT " +
                              $"BankAccounts.id AS ID, " +
                              $"Clients.firstName AS Owner_Name, " +
                              $"BankAccounts.balance AS Balance, " +
                              $"BankAccountTypes.accountType AS AccountType " +
                              $"FROM BankAccounts, BankAccountTypes, Clients " +
                              $"WHERE BankAccounts.ownerId = {clientId} AND Clients.id = {clientId} AND BankAccountTypes.id = BankAccounts.bankAccountType";
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString.ConnectionString))
                {
                    var sqlCommand = new SqlCommand(sqlScypt, sqlConnection);
                    sqlConnection.Open();
                    SqlDataReader dr = sqlCommand.ExecuteReader();
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append($"{"ID",6} | " +
                                         $"{"Owner",20} | " +
                                         $"{"Balance",13} | " +
                                         $"{"Type",8}\n\n");
                    while (dr.Read())
                    {
                        stringBuilder.Append($"{dr[0],6} | " +
                                             $"{dr[1],20} | " +
                                             $"{dr[2],13} | " +
                                             $"{dr[3],8}\n");
                    }
                    Console.WriteLine(stringBuilder);
                }

            }
            catch (Exception e)
            {
                OpenPageWarning(e.Message);
            }

        }

        /// <summary>
        /// Выводит в консоль всех клиентов данного банка
        /// </summary>
        /// <param name="bank">Банк с которым производится работа</param>
        void PrintAllClients()
        {
            string sqlScypt = $"SELECT " +
                              $"Clients.id AS ID," +
                              $"Clients.firstName AS [Name], " +
                              $"ClientTypes.typeOfClient AS [Type] " +
                              $"FROM Clients, ClientTypes " +
                              $"WHERE Clients.clientTypeId = ClientTypes.id ";
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString.ConnectionString))
                {
                    var sqlCommand = new SqlCommand(sqlScypt, sqlConnection);
                    sqlConnection.Open();
                    SqlDataReader dr = sqlCommand.ExecuteReader();
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append($"{"ID",6} | " +
                                         $"{"Name",20} | " +
                                         $"{"Type",16} | \n\n");

                    while (dr.Read())
                    {
                        stringBuilder.Append($"{dr[0],6} | " +
                                             $"{dr[1],20} | " +
                                             $"{dr[2],16} | \n");
                    }
                    Console.WriteLine(stringBuilder);
                }

            }
            catch (Exception e)
            {
                OpenPageWarning(e.Message);
            }
        }

        /// <summary>
        /// Подписывает наблюдателя на события меню
        /// </summary>
        /// <param name="logWriter"></param>
        public void SubscribeOnMenuEvents(Action<string> logWriter)
        {
            menuEvent += logWriter;
        }

        /// <summary>
        /// Делает задержку, для продолжения нажать Enter
        /// </summary>
        private static void Pause()
        {
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
    }
}

