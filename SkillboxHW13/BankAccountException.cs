using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillboxHW13
{
    public class BankAccountException : Exception
    {
        public override string Message { get => "Ошибка банковского счёта!"; }

    }
}
