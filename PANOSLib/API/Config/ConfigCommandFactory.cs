namespace PANOS
{
    using log4net;

    public class ConfigCommandFactory : IConfigCommandFactory
    {
        private readonly ApiUriFactory apiUriFactory;
        private readonly IConfigApiPostKeyValuePairFactory apiPostKeyValuePairFactory;
        private readonly ILog log = LogManager.GetLogger(typeof(ConfigCommandFactory));
       
         
        public ConfigCommandFactory(
            ApiUriFactory apiUriFactory,
            IConfigApiPostKeyValuePairFactory apiPostKeyValuePairFactory)
        {
            Logger.Configure();
            log.Info("Entered ConfigCommandFactory");

            this.apiUriFactory = apiUriFactory;
            this.apiPostKeyValuePairFactory = apiPostKeyValuePairFactory;
        }

        public ICommand<TApiResponse> CreateGetAll<TApiResponse>(string schemaName, ConfigTypes configType = ConfigTypes.Running) where TApiResponse : ApiResponse
        {
            return new Command<TApiResponse>(apiUriFactory.Create(), apiPostKeyValuePairFactory.CreateGetAll(schemaName, configType));
        }

        public ICommand<TApiResponse> CreateGetSingle<TApiResponse>(string schemaName, string name, ConfigTypes configType = ConfigTypes.Running) where TApiResponse : ApiResponse
        {
            return new Command<TApiResponse>(apiUriFactory.Create(), apiPostKeyValuePairFactory.CreateGetSingle(schemaName, name, configType));
        }

        public ICommand<ApiResponseWithMessage> CreateSet(FirewallObject firewallObject)
        {
            return new Command<ApiResponseWithMessage>(
                apiUriFactory.Create(),
                apiPostKeyValuePairFactory.CreateSet(firewallObject));
        }

        public ICommand<ApiResponseWithMessage> CreateSetMembership(GroupFirewallObject groupFirewallObject)
        {
            return new Command<ApiResponseWithMessage>(
                apiUriFactory.Create(),
                apiPostKeyValuePairFactory.CreateSetMembership(groupFirewallObject));
        }

        public ICommand<ApiResponseWithMessage> CreateDelete(string schemaName, string name)
        {
            return new Command<ApiResponseWithMessage>(
                apiUriFactory.Create(),
                apiPostKeyValuePairFactory.CreateDelete(schemaName, name));
        }

        public ICommand<ApiResponseWithMessage> CreateRename(string schemaName, string oldName, string newName)
        {
            return new Command<ApiResponseWithMessage>(
                apiUriFactory.Create(),
                apiPostKeyValuePairFactory.CreateRename(schemaName, oldName, newName));
        }
    }
}
