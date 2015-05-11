namespace PANOS
{
    using System.Net.Http;

    public interface IConfigPostKeyValuePairFactory
    {
        FormUrlEncodedContent CreateGetSingle(string schemaName, string name, ConfigTypes configType);

        FormUrlEncodedContent CreateGetAll(string schemaName, ConfigTypes configType);

        FormUrlEncodedContent CreateDelete(string schemName, string name);

        FormUrlEncodedContent CreateRename(string schemaName, string oldName, string newName);
        
        FormUrlEncodedContent CreateSet(FirewallObject firewallObject);
    }
}