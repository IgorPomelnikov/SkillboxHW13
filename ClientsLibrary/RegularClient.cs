namespace ClientsLibrary
{
    public class RegularClient : Client
    {
        public RegularClient(string name)
        {
            Name = name;
            CreditPercent = 10;
            DepositPercent = 10;
            Id = CommonId++;
        }

    }
}
