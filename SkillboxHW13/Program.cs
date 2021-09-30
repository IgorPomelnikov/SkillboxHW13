using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;
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
            var sqlDB = new SqlConnectionStringBuilder()
            {
                DataSource=@"(LocalDB)\MSSQLLocalDB",
                InitialCatalog= @"\\rttv.ru\profile\UserData2\iapomelnikov\Desktop\Skillbox\Домашние работы\SkillboxHW13\SkillboxHW17DB\SkillboxHW17DB.mdf",
                IntegratedSecurity = true
            };
            menu.SubscribeOnMenuEvents(logWriter.WriteLog);
            manager.sendAutomaticMessageFromManager += logWriter.WriteLog;
            menu.OpenPageStart();
            
        }
    }

}
