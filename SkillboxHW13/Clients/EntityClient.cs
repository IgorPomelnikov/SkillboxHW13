using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillboxHW13
{
    public class EntityClient : Client
    {
        public EntityClient()
        {
            CreditPercent = 20;
            DepositPercent = 5;
            Id = CommonId++;
        }
        
        public void OpenCredit(double sum, int mounths)
        {
            base.OpenCredit(this, sum, mounths);
        }
    }
}
