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

        public void OpenDebet( Client client)
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
        public void OpenCredit(Client client)
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
        public void OpenDeposit(Client client)
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
        public void CloseDebet()
        {

        }
        public void CloseCredit()
        {

        }
        public void CloseDeposit()
        {

        }
    }
}
