using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillboxHW13
{

    public class Manager
    {
        public string Name { get; private set; }

        Bank _bank;
        Func<int> getMoneyValueFromUser;
        Func<int> getMouthsValueFromUser;
        Func<int> getClientTypeFromUser;
        Func<bool> getCapitalizationValueFromUser;
        Func<string> getNameValueFromUser;
        public event Action<string> sendMessageFromManager;

        
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
        public Manager(Bank bank)
        {
            _bank = bank;
        }
       
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


        /// <summary>
        /// Регистрирует нового клиента
        /// </summary>
        public void RegisterClient()
        {
            _bank.Clients.Add(CreateClient());

            sendMessageFromManager($"Manager {Name} created client {_bank.Clients[^1].Id}");
        }
        /// <summary>
        /// Открывает клиенту новый банковский счёт
        /// </summary>
        /// <param name="client">Клиент, которому открывается счёт</param>
        /// <param name="accountType">Тип открываемого счёта</param>
        public void OpenNewAccount(Client client, int accountType)
        {
            switch (accountType)
            {
                case 1:
                    {
                        client.OpenCreditAccount(client, getMoneyValueFromUser(), getMouthsValueFromUser()); 
                        break;
                    }
                case 2: client.OpenDepositAccount(client, getMoneyValueFromUser(), getMouthsValueFromUser(), getCapitalizationValueFromUser()); break;
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
            if (bankAccount.Balance == 0 || bankAccount is null)
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
        public Client ChooseCliehtById(int id)
        {
            return _bank.Clients.Find(x => x.Id == id);
        }
        
    }
}
