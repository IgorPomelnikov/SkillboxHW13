namespace ClientsLibrary
{
    public class EntityClient : Client
    {
        public EntityClient(string name)
        {
            Name = name;
            CreditPercent = 20;
            DepositPercent = 5;
            Id = CommonId++;
        }


    }
}
