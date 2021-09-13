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

        public event Notify NewMassage;
        private Bank _bank;
        private IntGetter money;
        private IntGetter mouths;
        private IntGetter clientType;
        private BoolGetter capitalization;
        private StringGetter name;

        
        Client CreateClient()
        {
            
            switch (clientType())
            {
                case 1: return new RegularClient(name());
                case 2: return new VIPClient(name());
                case 3: return new EntityClient(name());
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
        public void SetMoneyGetter(IntGetter intPage) { money = intPage; }
        /// <summary>
        /// Сохраняет ссылку на внешний метод получения количества месяцев
        /// </summary>
        /// <param name="intPage">Внешний метод</param>
        public void SetMounthsGetter(IntGetter intPage) { mouths = intPage; }
        /// <summary>
        /// Сохраняет ссылку на внешний метод получения типа клиента в виде целочисленного значения для использования в качестве swich переключателя
        /// </summary>
        /// <param name="intPage">Внешний метод</param>
        public void SetClientTypeGetter(IntGetter intPage) { clientType = intPage; }
        /// <summary>
        /// Сохраняет ссылку на внешний метод получения информации о типе депозита
        /// </summary>
        /// <param name="intPage">Внешний метод</param>
        public void SetCapitalizationGetter(BoolGetter boolPage) { capitalization = boolPage; }
        /// <summary>
        /// Сохраняет ссылку на внешний метод получения имени клиента
        /// </summary>
        /// <param name="intPage">Внешний метод</param>
        public void SetNameGetter(StringGetter stringPage) { name = stringPage; }
        #endregion


        /// <summary>
        /// Регистрирует нового клиента
        /// </summary>
        public void RegisterClient()
        {
            _bank.Clients.Add(CreateClient());
            
            NewMassage($"Manager {Name} created client {_bank.Clients[^1].Id}");
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
                        client.OpenCreditAccount(client, money(), mouths()); 
                        break;
                    }
                case 2: client.OpenDepositAccount(client, money(), mouths(), capitalization()); break;
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
            else NewMassage("Balance must be equal 0");
            
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
