namespace PANOS
{
    using System;

    public  class MembershipRepository : IMembershipRepository
    {
        private readonly IConfigCommandFactory commandFactory;

        public MembershipRepository(IConfigCommandFactory commandFactory)
        {
            this.commandFactory = commandFactory;
        }

        public void SetGroupMembership(GroupFirewallObject firewallGroupObject)
        {
            var response = commandFactory.CreateSetMembership(firewallGroupObject).Execute();
            if (!response.Status.Equals("success"))
            {
                throw new Exception(string.Format("Set Membership Method failed. PANOS error code {0}", response.Status));
            }
        }
    }
}
