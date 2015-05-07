namespace PANOS
{
    using System;
    
    public class AddableRepository : IAddableRepository
    {
        private readonly IConfigCommandFactory commandFactory;

        public AddableRepository(IConfigCommandFactory commandFactory)
        {
            this.commandFactory = commandFactory;
        }

        public void Add(FirewallObject firewallObject)
        {
            var response = commandFactory.CreateSet(firewallObject).Execute();
            if (!response.Status.Equals("success"))
            {
                throw new Exception(string.Format("Add Method failed. PANOS error code {0}", response.Status));
            }
        }
    }
}
