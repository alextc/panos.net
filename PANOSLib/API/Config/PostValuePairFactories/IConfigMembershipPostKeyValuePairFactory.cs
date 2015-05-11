namespace PANOS
{
    using System.Net.Http;

    public interface IConfigMembershipPostKeyValuePairFactory
    {
        FormUrlEncodedContent CreateSetMembership(GroupFirewallObject groupFirewallObject);

        FormUrlEncodedContent CreateAddMember(string groupName, string schemaName, string memberName);

        // TODO: Consider adding an overload
        // FormUrlEncodedContent CreateSetMembership(string name, string schemaName, List<string> members); 
    }
}
