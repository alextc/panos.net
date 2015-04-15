namespace PANOS
{
    using System.Collections.Generic;

    public interface IConfigCommandFactory
    {
        ICommand<TApiResponse> CreateGetAll<TApiResponse>(string schemaName, ConfigTypes configType = ConfigTypes.Running) where TApiResponse : ApiResponse;

        ICommand<TApiResponse> CreateGetSingle<TApiResponse>(string schemaName, string name, ConfigTypes configType = ConfigTypes.Running) where TApiResponse : ApiResponse; 

        ICommand<ApiResponseWithMessage> CreateSet(FirewallObject firewallObject);

        ICommand<ApiResponseWithMessage> CreateSetMembership(GroupFirewallObject groupFirewallObject);

        ICommand<ApiResponseWithMessage> CreateDelete(string schemaName, string name);

        ICommand<ApiResponseWithMessage> CreateRename(string schemaName, string oldName, string newName);
    }
}