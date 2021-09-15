using System;

namespace BankAccountLibrary
{
    public abstract class BankAccount
    {
        public double Balance { get; protected set; } = 0;
        public DateTime Opened { get; protected set; }
        public DateTime LastUpdate { get; protected set; }
        protected static int CommonId { get; set; } = 0;
        public int Id { get; protected set; }
        public abstract void MakePayment(double payment);
        public abstract void TakeMoney(double payment);
        public abstract override string ToString();
    }
}
