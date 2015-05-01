namespace PANOS
{
    public interface IMembershipRepository
    {
        void SetGroupMembership(GroupFirewallObject firewallGroupObject);

        void InflateMembers<T, TDeserializer>(
            ISearchableRepository<T> searchableRepository,
            GroupFirewallObject groupFirewallObject,
            ConfigTypes configType) where T : FirewallObject where TDeserializer : ApiResponseForGetAll;
    }
}
