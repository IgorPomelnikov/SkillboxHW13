using System;

namespace SkillboxHW13
{
    class Program
    {
        static void Main(string[] args)
        {
            var bank = new Bank();
            var manager = new Manager(bank);
            var menu = new Menu(bank, manager);
            menu.OpenPageStart();
        }
    }
}
