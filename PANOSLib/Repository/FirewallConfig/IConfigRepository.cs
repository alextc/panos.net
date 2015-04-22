namespace PANOS
{
    using System.Collections.Generic;

    public interface IConfigRepository
    {
        FirewallObject GetSingle<TDeserializer, TObject>(
            string schemaName,
            string objectName,
            ConfigTypes configType)
            where TDeserializer : ApiResponse, IPayload
            where TObject : FirewallObject;

        Dictionary<string, TObject> GetAll<TDeserializer, TObject>(
            string schemaName,
            ConfigTypes configType) 
            where TDeserializer : ApiResponse, IDictionaryPayload 
            where TObject : FirewallObject;

        string Rename(string schemaName, string oldName, string newName);

        string Delete(string schemaName, string name);

        void Set(FirewallObject firewallObject);

        ApiResponseWithMessage SetGroupMembership(GroupFirewallObject groupName);

        // By inflation I mean the following: the caller supplies a list of members (as strings) and this method returns a list of corresponding objects
        // Example, input Members = "address1", "address2", output List<AddressObject> Addresses
        // The input is in the List<string> Members of the groupFirewallObject
        // The ouput is in the List<FirewallObject> MemberObjects of the groupFirewallObject
        void InflateMembers<TDeserializer, TObject>(
            GroupFirewallObject groupFirewallObject,
            string memberSchemaName,
            ConfigTypes configType) 
            where TDeserializer : ApiResponse, IDictionaryPayload 
            where TObject : FirewallObject;
    }
}