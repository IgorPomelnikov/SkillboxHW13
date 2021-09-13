using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillboxHW13
{
    public abstract class Client
    {
        public event AccountEvent BankAccountStatus;
        public int Id { get; protected set; }
        protected static int CommonId { get; set; } = 0;
        public List<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
        public string Name { get; protected set; }
        public double DepositPercent { get; protected set; }
        public double CreditPercent { get; protected set; }
        /// <summary>
        /// Открывает депозитный счёт и заносит новый бансковский счёт в  в коллекцию счетов клиента.
        /// </summary>
        /// <param name="sum"></param>
        /// <param name="percent"></param>
        /// <param name="mounth"></param>
        /// <param name="capitalized"></param>
        void AddDeposit(double sum, double percent, int mounth, bool capitalized)
        {
            Deposit deposit = new(sum, percent, mounth, capitalized);
            BankAccounts.Add(deposit);
            BankAccountStatus(this, $"New Credit (id {deposit.Id}) was created for client {Name} (id {Id})");
        }
        /// <summary>
        /// Открывает кредитный счёт и заносит новый бансковский счёт в коллекцию счетов клиента.
        /// </summary>
        /// <param name="sum"></param>
        /// <param name="percent"></param>
        /// <param name="mounth"></param>
        void AddCredit(double sum, double percent, int mounth)
        {
            Credit credit = new(sum, percent, mounth);
            BankAccounts.Add(credit);

            BankAccountStatus(this, $"New Credit (id {credit.Id}) was created for client {Name} (id {Id})");
        }
        /// <summary>
        /// Открывает кредитный счёт
        /// </summary>
        /// <param name="client">Конкретный клиент</param>
        /// <param name="sum">Сумма, запрашиваемая клиентом</param>
        /// <param name="mounths">Срок кредитования</param>
        public void OpenCreditAccount(Client client, double sum, int mounths)
        {
            this.AddCredit(sum, client.CreditPercent, mounths);

        }
        /// <summary>
        /// Открывает депозитный счёт
        /// </summary>
        /// <param name="client">конкретный клиент</param>
        /// <param name="sum">Сумма, помещающаяся на счёт</param>
        /// <param name="mounth">Срок на который открывается счёт</param>
        /// <param name="capitalized">Определение того, является ли счёт капитализированным</param>
        public void OpenDepositAccount(Client client, double sum, int mounth, bool capitalized)
        {
            this.AddDeposit(sum, client.DepositPercent, mounth, capitalized);
           
        }
        /// <summary>
        /// Закрывает банковский счёт
        /// </summary>
        /// <param name="id">ID банковского счёта</param>
        public void CloseBankAccount(int id)
        {
            int index = BankAccounts.IndexOf(BankAccounts.Find(BankAccounts => BankAccounts.Id == id));
            if (BankAccounts[index].Balance == 0 && BankAccounts[index] is not null)
            {
                BankAccounts.RemoveAt(index);
            }
            BankAccountStatus(this, $"Bank account Id_{id} of client {Name} (id {Id}) was closed");
        }
        /// <summary>
        /// Переводит деньги с одного счёта на другой
        /// </summary>
        /// <param name="fromId">Счёт с которого снимаются деньги, им может быть только депозит</param>
        /// <param name="toId">Счёт на который записываются деньги</param>
        /// <param name="count">Сума для перевода</param>
        public void TransferMoney(int fromId, int toId, double count)
        {
            int index = BankAccounts.IndexOf(BankAccounts.Find(BankAccounts => BankAccounts.Id == fromId));
            int index2 = BankAccounts.IndexOf(BankAccounts.Find(BankAccounts => BankAccounts.Id == toId));
            if (BankAccounts[index].Balance >= count && BankAccounts[index] is Deposit)
            {
                    if (BankAccounts[index] is not null)
                    {
                        BankAccounts[index].TakeMoney(count);
                        BankAccounts[index2].MakePayment(count);
                    }
                    else BankAccountStatus(this, $"A broblem with bank account id {toId}");
            }
            else BankAccountStatus(this, $"A broblem with bank account id {fromId}");
        }

       
    }
}
