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
        public double Persent { get; private set; }
        public Dictionary<DateTime, double> Schedule { get; private set; }
        public Deposit(double count, double percent, int mounth, bool isCapitalized)
        {
            Balance = Math.Round(count,2);
            Persent = percent;
            Capitalization = isCapitalized;
            Schedule = PaymentSсhedule(isCapitalized, mounth);
            Opened = DateTime.Now;
            LastUpdate = Opened;
            Id = CommonId++;
        }
        /// <summary>
        /// Создаёт расписание начислений процентов по вкладам. У капитализированного надбавка происходит каждый месяц, у простого, только на последнем месяце.
        /// </summary>
        /// <param name="capitalized">Является ли депозит капитализированным вкладом False если нет, True если да</param>
        /// <param name="mounth">Количество месяцев на который открывается депозит</param>
        /// <returns>Коллекция словарного типа, содержащая в качестве ключа дату и в качестве значения то, какой процент будет на вкладе к этой дате</returns>
        Dictionary<DateTime, double> PaymentSсhedule(bool capitalized, int mounth)
        {
            Dictionary<DateTime, double> schedule = new Dictionary<DateTime, double>();
            if (!capitalized)
            {
                for (int i = 1; i < mounth; i++)
                {
                    schedule.Add(Opened.AddMonths(i), Balance);
                }
                    schedule.Add(Opened.AddMonths(mounth), Balance / 100 * Persent);
            }
            else
            {
                double tempCount = Balance;
                for (int i = 1; i <= mounth; i++)
                {
                    tempCount += tempCount / 100 * Persent;
                    schedule.Add(Opened.AddMonths(i), tempCount);
                }
                
            }
            return schedule;
        }

        public override void MakePayment(double payment)
        {
            Balance += payment;
        }

        public override void TakeMoney(double payment)
        {
            Balance -= payment;
        }


    }
}
