namespace PANOS
{
    public interface  IConfigMembershipCommandFactory
    {
        ICommand<ApiResponseWithMessage> CreateSetMembership(GroupFirewallObject groupFirewallObject);
    }
}
