using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Loger;
using System.Threading;

namespace SkillboxHW13
{
    class Program
    {
        static void Main(string[] args)
        {
            var bank = new Bank();
            var manager = new Manager(bank);
            var menu = new Menu(bank, manager);
            var logWriter = new MenuLogWriter("C:/");
            menu.SubscribeOnMenuEvents(logWriter.WriteLog);
            manager.sendAutomaticMessageFromManager += logWriter.WriteLog;
            menu.OpenPageStart();
            
        }
    }

}
