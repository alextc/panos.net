namespace PANOS
{
    using System.Net.Http;

    public interface IConfigMembershipPostKeyValuePairFactory
    {
        FormUrlEncodedContent CreateSetMembership(GroupFirewallObject groupFirewallObject);

        // TODO: Consider adding an overload
        // FormUrlEncodedContent CreateSetMembership(string name, string schemaName, List<string> members); 
    }
}
