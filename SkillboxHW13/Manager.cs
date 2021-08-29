using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillboxHW13
{
    public class Manager
    {
        private Bank _bank;
        public Manager(Bank bank)
        {
            _bank = bank;
        }

        public void RegisterClient()
        {
            _bank.Clients.Add(CreateClient());
            Console.Clear();
            Console.WriteLine("You've created client {0}", _bank.Clients[^1].Id);
        }

        public void OpenNewAccount(Client client, int accountType)
        {
            switch (accountType)
            {
                case 1:
                    {
                        client.OpenCreditAccount(client, Menu.OpenPageMoney(), Menu.OpenPageMounths()); 
                        break;
                    }
                case 2: client.OpenDepositAccount(client, Menu.OpenPageMoney(), Menu.OpenPageMounths(), Menu.OpenPageCapitalization()); break;
                default:
                    break;
            }
        }
        public void RemoveBankAccount(Client client, BankAccount bankAccount)
        {
            if (bankAccount.Balance == 0 || bankAccount is null)
            {
                client.BankAccounts.Remove(bankAccount);
            }
            else Menu.OpenPageWarning("Balance must be equal 0");
            
        }

        int GetClientType()
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

        public Client ChooseCliehtById(int id)
        {
            return _bank.Clients.Find(x => x.Id == id);
        }
        Client CreateClient()
        {
            Console.Clear();
            switch (GetClientType())
            {
                case 1: return new RegularClient();
                case 2: return new VIPClient();
                case 3: return new EntityClient();
                default: return null;
            }
        }
        

        /* добавить метод выбора действия с клиентом
         * Какие действия могут быть?
         * создать клиента - done
         * удалить клиента 
         * открыть счёт
         * закрыть счёт
         * посмотреть остаток кредита
         * посмотреть счёт депозита
         * 
         * внести деньги на счёт\
         *                       |>----выполнить перевод со счёта на счёт                       
         * снять деньги со счёта/
         * 
         *
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * добавить метод вывода в консоль данных по счетам клиента
         * добавить метод просмотра состояния кредита по дате, где будет выбор посмотреть по текущей дате или установить дату вручную для просмотра.
         * добавить метод внесения платежей
         * добавить метод снятия денег
         
         */

    }
}
