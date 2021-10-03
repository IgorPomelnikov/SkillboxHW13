using Loger;
using Microsoft.Data.SqlClient;

namespace SkillboxHW13
{
    class Program
    {
        static void Main(string[] args)
        {
            var sqlDB = new SqlConnectionStringBuilder()
            {
                //Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Igor\Desktop\SkillBox\HomeWorks\HW13\SkillboxHW13\SkillboxHW17DB\SkillboxHW17DB.mdf;Integrated Security=True;Connect Timeout=30
                DataSource = @"(LocalDB)\MSSQLLocalDB",
                InitialCatalog = @"C:\Users\Igor\Desktop\SkillBox\HomeWorks\HW13\SkillboxHW13\SkillboxHW17DB\SkillboxHW17DB.mdf",
                IntegratedSecurity = true
            };
            var manager = new Manager(sqlDB, "TestName");
            var menu = new Menu(sqlDB, manager);
            var logWriter = new MenuLogWriter("C:/");
            menu.SubscribeOnMenuEvents(logWriter.WriteLog);
            menu.OpenPageStart();

        }
    }

}
