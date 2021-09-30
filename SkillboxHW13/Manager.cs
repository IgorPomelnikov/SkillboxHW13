using BankAccountLibrary;
using ClientsLibrary;
using System;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections.Generic;
using System.Threading.Tasks;



namespace SkillboxHW13
{

    public class Manager
    {
        SqlConnectionStringBuilder sqlConnectionString;
        public string Name { get; private set; }
        Random random = new Random();
        Bank _bank;
        Func<int> getMoneyValueFromUser;
        Func<int> getMouthsValueFromUser;
        Func<int> getClientTypeFromUser;
        Func<bool> getCapitalizationValueFromUser;
        Func<string> getNameValueFromUser;
        public event Action<string> sendMessageFromManager;
        public event Action<string> sendAutomaticMessageFromManager;


        public Manager(Bank bank) => _bank = bank;
        public Manager(SqlConnectionStringBuilder sqlConnection, string managerName)
        {
            Name = managerName;
            sqlConnectionString = sqlConnection;
        } 




        /// <summary>
        /// Регистрирует нового клиента
        /// </summary>
        public void RegisterClient()
        {
            CreateClientInSqlDB();
            sendMessageFromManager($"Manager {Name} created client");
        }

        private void CreateClientInSqlDB()
        {
            try
            {
                string sqlScypt = $"INSERT INTO [dbo].[Clients] ([firstName], [clientTypeId]) VALUES ('{getNameValueFromUser}', {getClientTypeFromUser})";
                using (var sqlConnection = new SqlConnection(sqlConnectionString.ConnectionString))
                {
                    var sqlCommand = new SqlCommand(sqlScypt, sqlConnection);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();

                }

            }
            catch (Exception e)
            {
                sendMessageFromManager(e.Message);
            }
        }

        /// <summary>
        /// Открывает клиенту новый банковский счёт
        /// </summary>
        /// <param name="client">Клиент, которому открывается счёт</param>
        /// <param name="accountType">Тип открываемого счёта</param>
        public void OpenNewAccount(Client client, BankAccountTypes accountType)
        {
            switch (accountType)
            {
                case BankAccountTypes.Credit: client.OpenCreditAccount(client, getMoneyValueFromUser(), getMouthsValueFromUser()); break;
                case BankAccountTypes.Deposit: client.OpenDepositAccount(client, getMoneyValueFromUser(), getMouthsValueFromUser(), getCapitalizationValueFromUser()); break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Закрывает банковский счёт клиента
        /// </summary>
        /// <param name="client">Клиет, чей это счёт</param>
        /// <param name="bankAccount">Счёт для закрытия</param>
        public void RemoveBankAccount(Client client, BankAccount bankAccount)
        {
            if (bankAccount is not null && bankAccount.Balance == 0)
            {
                client.BankAccounts.Remove(bankAccount);
            }
            else sendMessageFromManager("Balance must be equal 0");
        }
        /// <summary>
        /// Выбирает клиента из коллекци клиентов банка
        /// </summary>
        /// <param name="id">Id клиента</param>
        /// <returns>Клиент, выбранный по id</returns>
        public Client ChooseCliehtById(int id) => _bank.Clients.Find(x => x.Id == id);
      
        #region Методы присвоения ссылок делегатам

        /// <summary>
        /// Сохраняет ссылку на внешний метод получения денежной суммы
        /// </summary>
        /// <param name="intPage">Внешний метод</param>
        public void SetMoneyGetter(Func<int> intPage) { getMoneyValueFromUser = intPage; }
        /// <summary>
        /// Сохраняет ссылку на внешний метод получения количества месяцев
        /// </summary>
        /// <param name="intPage">Внешний метод</param>
        public void SetMounthsGetter(Func<int> intPage) { getMouthsValueFromUser = intPage; }
        /// <summary>
        /// Сохраняет ссылку на внешний метод получения типа клиента в виде целочисленного значения для использования в качестве swich переключателя
        /// </summary>
        /// <param name="intPage">Внешний метод</param>
        public void SetClientTypeGetter(Func<int> intPage) { getClientTypeFromUser = intPage; }
        /// <summary>
        /// Сохраняет ссылку на внешний метод получения информации о типе депозита
        /// </summary>
        /// <param name="intPage">Внешний метод</param>
        public void SetCapitalizationGetter(Func<bool> boolPage) { getCapitalizationValueFromUser = boolPage; }
        /// <summary>
        /// Сохраняет ссылку на внешний метод получения имени клиента
        /// </summary>
        /// <param name="intPage">Внешний метод</param>
        public void SetNameGetter(Func<string> stringPage) { getNameValueFromUser = stringPage; }
        #endregion

        #region Unusing methods
        Client CreateClient()
        {

            switch (getClientTypeFromUser())
            {
                case 1: return new RegularClient(getNameValueFromUser());
                case 2: return new VIPClient(getNameValueFromUser());
                case 3: return new EntityClient(getNameValueFromUser());
                default: return null;
            }
        }   
        void AutomaticlyCreateClients()
        {
            var task = Task.CurrentId;
            sendAutomaticMessageFromManager($"!!!Начато автоматическое создание клиентов--------------");
            for (int i = 0; i < 1000000; i++)
            {
                switch (random.Next(1, 4))
                {
                    case 1: _bank.Clients.Add(new RegularClient("TestClientRegular")); break;
                    case 2: _bank.Clients.Add(new VIPClient("TestClientVIP")); break;
                    case 3: _bank.Clients.Add(new EntityClient("TestClientEntity")); break;
                    default: break;
                }
                sendAutomaticMessageFromManager($"Task (id {Task.CurrentId}) created client {_bank.Clients[^1].Id} {_bank.Clients[^1].Name}");
            }

            sendAutomaticMessageFromManager($"!!!Завершено автоматическое создание клиентов--------------");
        }

        #endregion

    }
}
