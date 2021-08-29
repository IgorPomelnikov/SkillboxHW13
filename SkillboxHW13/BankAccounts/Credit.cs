using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillboxHW13
{
    public class Credit : BankAccount
    {
        public double Payment { get; private set; }
        public Credit(double credit, double percents, int mounths)
        {
            Payment = credit * (percents * 100 / (12 * 10000)) / (1 - Math.Pow((1 + (percents * 100 / (12 * 10000))), -Convert.ToDouble(mounths)));
            //формула аннуитетного платежа https://ru.wikipedia.org/wiki/%D0%90%D0%BD%D0%BD%D1%83%D0%B8%D1%82%D0%B5%D1%82
            //аннуитетный платёж умножаю на количество месяцев
            Balance = Math.Round(Payment * mounths * (-1), 2);
            Opened = DateTime.Now;
            LastUpdate = Opened;
            Id = CommonId++;
        }

        public void MakePayment(double payment)
        {
            Balance+= payment;
        }

       
    }
}
