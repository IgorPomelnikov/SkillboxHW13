using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillboxHW13
{
    public class RegularClient : Client
    {
        public RegularClient()
        {
            Percent = 10;
        }
        public void OpenCredit(double sum, int mounths)
        {
            base.OpenCredit(this, sum, mounths);
        }
    }
}
