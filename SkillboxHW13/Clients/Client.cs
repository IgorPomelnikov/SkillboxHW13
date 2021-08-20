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

        public void OpenDebet()
        {

        }
        public void OpenCredit()
        {

        }
        public void OpenDeposit()
        {

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
