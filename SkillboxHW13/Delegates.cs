using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillboxHW13
{
    //Делегаты, используемые в программе
    public delegate void AccountEvent(object sender, string msg);
    public delegate void Notify(string Msg);
    public delegate int IntGetter();
    public delegate bool BoolGetter();
    public delegate string StringGetter();
    public delegate void MenuEvent(object sender, string msg);
}
