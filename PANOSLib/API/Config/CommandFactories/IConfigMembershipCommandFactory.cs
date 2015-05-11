namespace PANOS
{
    public interface  IConfigMembershipCommandFactory
    {
        ICommand<ApiResponseWithMessage> CreateSetMembership(GroupFirewallObject groupFirewallObject);

        ICommand<ApiResponseWithMessage> CreateAddMember(string groupName, string schemaName, string memberName);
    }
}
