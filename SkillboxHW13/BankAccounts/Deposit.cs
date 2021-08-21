using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillboxHW13
{
    public class Deposit : BankAccount
    {
        public bool Capitalization { get; private set; }
        public Deposit(bool isCapitalized)
        {
            Capitalization = isCapitalized;
        }
    }
}
