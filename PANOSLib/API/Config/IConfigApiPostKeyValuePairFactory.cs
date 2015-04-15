namespace PANOS
{
    using System.Collections.Generic;
    using System.Net.Http;

    public interface IConfigApiPostKeyValuePairFactory
    {
        FormUrlEncodedContent CreateGetSingle(string schemaName, string name, ConfigTypes configType);

        FormUrlEncodedContent CreateGetAll(string schemaName, ConfigTypes configType);

        FormUrlEncodedContent CreateDelete(string schemName, string name);

        FormUrlEncodedContent CreateRename(string schemaName, string oldName, string newName);
        
        FormUrlEncodedContent CreateSet(FirewallObject firewallObject);

        FormUrlEncodedContent CreateSetMembership(GroupFirewallObject groupFirewallObject); 

        // TODO: Consider adding an overload
        // FormUrlEncodedContent CreateSetMembership(string name, string schemaName, List<string> members); 
    }
}