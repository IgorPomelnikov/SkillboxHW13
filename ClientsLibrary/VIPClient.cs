namespace ClientsLibrary
{
    public class VIPClient : Client
    {
        public VIPClient(string name)
        {
            Name = name;
            CreditPercent = 5;
            DepositPercent = 15;
            Id = CommonId++;
        }

    }
}
