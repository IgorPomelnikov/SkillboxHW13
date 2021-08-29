using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillboxHW13
{
    public abstract class Client
    {
        public int Id { get; protected set; }
        protected static int CommonId { get; set; } = 0;
        public List<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
        public string Name { get; protected set; }

        public double DepositPercent { get; protected set; }
        public double CreditPercent { get; protected set; }
        public void OpenCreditAccount(Client client, double sum, int mounths)
        {
             this.AddCredit(sum, client.CreditPercent, mounths);
        }
        public void OpenDepositAccount(Client client, double sum, int mounth, bool capitalized)
        {
            this.AddDeposit(sum, client.DepositPercent, mounth, capitalized);
           
        }
        private void AddDeposit(double sum, double percent, int mounth, bool capitalized)
        {
            Deposit deposit = new(sum, percent, mounth, capitalized);
            BankAccounts.Add(deposit);
        }
        private void AddCredit(double sum, double percent, int mounth)
        {
            Credit credit = new(sum, percent, mounth);
            BankAccounts.Add(credit);
        }
        public void CloseBankAccount(int id)
        {
            int index = BankAccounts.IndexOf(BankAccounts.Find(BankAccounts => BankAccounts.Id == id));
            if (BankAccounts[index].Balance != 0 && BankAccounts[index] is not null)
            {
                BankAccounts.RemoveAt(index);
            }
        }
        public void TransferMoney(int senderId, int targetId)
        {

        }
       
    }
}
