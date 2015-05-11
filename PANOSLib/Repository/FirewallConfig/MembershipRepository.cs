namespace PANOS
{
    using System;
    using System.Linq;

    public  class MembershipRepository : IMembershipRepository
    {
        private readonly IConfigMembershipCommandFactory commandFactory;

        public MembershipRepository(IConfigMembershipCommandFactory commandFactory)
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

        public void InflateMembers<T, TDeserializer>(
            ISearchableRepository<T> searchableRepository,
            GroupFirewallObject groupFirewallObject,
            ConfigTypes configType) where T : FirewallObject where TDeserializer : ApiResponseForGetAll
        {
            var allTObjects = searchableRepository.GetAll<TDeserializer>(configType);
            groupFirewallObject.MemberObjects.AddRange(
                (from tObject in allTObjects
                 where groupFirewallObject.Members.Contains(tObject.Key)
                 select tObject.Value)
                .ToList());
        }
    }
}
