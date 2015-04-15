namespace PANOS
{
    public class ConfigCommandFactory : IConfigCommandFactory
    {
        private readonly ApiUriFactory apiUriFactory;
        private readonly IConfigApiPostKeyValuePairFactory apiPostKeyValuePairFactory;
         
        /*
        private const string CommitTemplate = 
            "https://{0}/api/?key={1}&type=commit&cmd=<commit></commit>";

        private const string SetTemplate =
            "https://{0}/api/?key={1}&type=config&action=set&xpath=/config/devices/entry/vsys/entry[@name='{2}']/{3}/entry[@name='{4}']&element={5}";

        private const string GetTemplate =
            "https://{0}/api/?key={1}&type=config&action={2}&xpath=/config/devices/entry/vsys/entry[@name='{3}']/{4}/entry[@name='{5}']";

        private const string GetAllTemplate = 
            "https://{0}/api/?key={1}&type=config&action={2}&xpath=/config/devices/entry/vsys/entry[@name='{3}']/{4}";

        private const string DeleteTemplate =
            "https://{0}/api/?key={1}&type=config&action=delete&xpath=/config/devices/entry/vsys/entry[@name='{2}']/{3}/entry[@name='{4}']";
         */

        public ConfigCommandFactory(
            ApiUriFactory apiUriFactory,
            IConfigApiPostKeyValuePairFactory apiPostKeyValuePairFactory)
        {
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
            // I am interested in the scenario where a newly created object could be immediately passed via Pipeline to the next command
            // For this reason I am attaching the input to the output in the Command constructor; this way the ApiResponse object will contain the output object.
            // Potentially this may be better handled with PassThru, but then I will miss the ouput of the API response 
            return new Command<ApiResponseWithMessage>(
                apiUriFactory.Create(),
                apiPostKeyValuePairFactory.CreateSet(firewallObject),
                firewallObject);
        }

        public ICommand<ApiResponseWithMessage> CreateSetMembership(GroupFirewallObject groupFirewallObject)
        {
            return new Command<ApiResponseWithMessage>(
                apiUriFactory.Create(),
                apiPostKeyValuePairFactory.CreateSetMembership(groupFirewallObject),
                groupFirewallObject);
        }

        public ICommand<ApiResponseWithMessage> CreateDelete(string schemaName, string name)
        {
            return new Command<ApiResponseWithMessage>(
                apiUriFactory.Create(),
                apiPostKeyValuePairFactory.CreateDelete(schemaName, name),
                name);
        }

        public ICommand<ApiResponseWithMessage> CreateRename(string schemaName, string oldName, string newName)
        {
            return new Command<ApiResponseWithMessage>(
                apiUriFactory.Create(),
                apiPostKeyValuePairFactory.CreateRename(schemaName, oldName, newName),
                newName);
        }
    }
}
