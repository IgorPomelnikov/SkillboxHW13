using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillboxHW13
{
    public abstract class Client
    {
        public List<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();

        public double Percent { get; protected set; }
        protected void OpenCredit(Client client, double sum, int mounths)
        {
            switch (client)
            {
                case RegularClient: this.AddCredit(sum, client.Percent, mounths); break;
                case VIPClient: this.AddCredit(sum, client.Percent, mounths); break;
                case EntityClient: this.AddCredit(sum, client.Percent, mounths); break;
                default:
                    break;
            }
        }
        protected void OpenDeposit(Client client)
        {
            switch (client)
            {
                case RegularClient: break;
                case VIPClient: break;
                case EntityClient: break;
                default:
                    break;
            }
        }

        private void AddCredit(double sum, double percent, int mounth)
        {
            Credit credit = new(sum, percent, mounth);
            BankAccounts.Add(credit);
        }
        public void CloseAccouint(int id)
        {
            int index = BankAccounts.IndexOf(BankAccounts.Find(BankAccounts => BankAccounts.Id == id));
            if (BankAccounts[index].Count != 0 && BankAccounts[index] is not null)
            {
                BankAccounts.RemoveAt(index);
            }


        }
        public void CloseDeposit()
        {

        }
    }
}
