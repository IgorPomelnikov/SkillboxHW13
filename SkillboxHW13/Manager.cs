using Microsoft.Data.SqlClient;
using System;



namespace SkillboxHW13
{

    public class Manager
    {
        public string Name { get; private set; }
        SqlConnectionStringBuilder sqlConnectionString;
        Func<int> getMoneyValueFromUser;
        Func<int> getClientTypeFromUser;
        Func<string> getNameValueFromUser;
        public event Action<string> sendMessageFromManager;

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

        /// <summary>
        /// Создаёт запись о новом клиенте в базе данных
        /// </summary>
        private void CreateClientInSqlDB()
        {
            string sqlScypt = $"INSERT INTO [dbo].[Clients] ([firstName], [clientTypeId]) VALUES (N'{getNameValueFromUser()}', {getClientTypeFromUser()})";
            ExecuteSQLCommand(sqlScypt);
        }

        /// <summary>
        /// Открывает клиенту новый банковский счёт
        /// </summary>
        /// <param name="client">Клиент, которому открывается счёт</param>
        /// <param name="accountType">Тип открываемого счёта</param>
        public void CreateBankAccount(int clientId, int bankAccountTypes)
        {
            string sqlScypt = $"INSERT INTO [dbo].[BankAccounts] ([ownerId], [balance],[dateOpen], [bankAccountType]) " +
                              $"VALUES ({clientId}, {getMoneyValueFromUser()}, '{DateTime.Now.ToString("yyyy - MM - dd")}', {bankAccountTypes})";
            ExecuteSQLCommand(sqlScypt);
        }

        /// <summary>
        /// Закрывает банковский счёт клиента
        /// </summary>
        /// <param name="client">Клиет, чей это счёт</param>
        /// <param name="bankAccount">Счёт для закрытия</param>
        public void RemoveBankAccount(int clientID, int bankAccountID)
        {
            string sqlScrypt = $"DELETE FROM BankAccounts " +
                               $"WHERE BankAccounts.id = {bankAccountID} AND BankAccounts.ownerId = {clientID}";
            ExecuteSQLCommand(sqlScrypt);
        }

        /// <summary>
        /// Выполняет SQL  скрипт 
        /// </summary>
        /// <param name="sqlScypt">Скрипт для выполнения</param>
        private void ExecuteSQLCommand(string sqlScypt)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString.ConnectionString))
                {
                    var sqlCommand = new SqlCommand(sqlScypt, sqlConnection);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception e) { sendMessageFromManager(e.Message); }
        }

        #region Методы присвоения ссылок делегатам

        /// <summary>
        /// Сохраняет ссылку на внешний метод получения денежной суммы
        /// </summary>
        /// <param name="intPage">Внешний метод</param>
        public void SetMoneyGetter(Func<int> intPage) { getMoneyValueFromUser = intPage; }

        /// <summary>
        /// Сохраняет ссылку на внешний метод получения типа клиента в виде целочисленного значения для использования в качестве swich переключателя
        /// </summary>
        /// <param name="intPage">Внешний метод</param>
        public void SetClientTypeGetter(Func<int> intPage) { getClientTypeFromUser = intPage; }

        /// <summary>
        /// Сохраняет ссылку на внешний метод получения имени клиента
        /// </summary>
        /// <param name="intPage">Внешний метод</param>
        public void SetNameGetter(Func<string> stringPage) { getNameValueFromUser = stringPage; }

        #endregion

    }
}
