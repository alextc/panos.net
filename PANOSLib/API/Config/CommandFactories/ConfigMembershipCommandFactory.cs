namespace PANOS
{
    public class ConfigMembershipCommandFactory : IConfigMembershipCommandFactory
    {
        private readonly ApiUriFactory apiUriFactory;
        private readonly IConfigMembershipPostKeyValuePairFactory apiPostKeyValuePairFactory;

        public ConfigMembershipCommandFactory(
            ApiUriFactory apiUriFactory,
            IConfigMembershipPostKeyValuePairFactory apiPostKeyValuePairFactory)
        {
            this.apiUriFactory = apiUriFactory;
            this.apiPostKeyValuePairFactory = apiPostKeyValuePairFactory;
        }

        public ICommand<ApiResponseWithMessage> CreateSetMembership(GroupFirewallObject groupFirewallObject) => 
            new Command<ApiResponseWithMessage>(
                apiUriFactory.Create(),
                apiPostKeyValuePairFactory.CreateSetMembership(groupFirewallObject));
        
        public ICommand<ApiResponseWithMessage> CreateAddMember(string groupName, string schemaName, string memberName) => 
            new Command<ApiResponseWithMessage>(
               apiUriFactory.Create(),
               apiPostKeyValuePairFactory.CreateAddMember(groupName, schemaName, memberName));
    }
}
