namespace PANOS
{
    public class LogCommandFactory : ILogCommandFactory
    {
        private readonly ApiUriFactory apiUriFactory;
        private readonly LogApiPostKeyValuePairFactory logApiPostKeyValuePairFactory;

        public LogCommandFactory(
            ApiUriFactory apiUriFactory,
            LogApiPostKeyValuePairFactory apiPostKeyValuePairFactory)
        {
            this.apiUriFactory = apiUriFactory;
            this.logApiPostKeyValuePairFactory = apiPostKeyValuePairFactory;  
        }

        public ICommand<ApiEnqueuedResponse> CreateRequestForTrafficLog(string query, uint nlogs, uint skip)
        {
            return new Command<ApiEnqueuedResponse>(
                apiUriFactory.Create(),
                logApiPostKeyValuePairFactory.CreateRequestForTrafficLog(query, nlogs, skip));
        }

        public ICommand<GetTrafficLogApiResponse> CreateConsumeTrafficLog(uint jobId)
        {
            return new Command<GetTrafficLogApiResponse>(
                apiUriFactory.Create(),
                logApiPostKeyValuePairFactory.CreateConsumeTrafficLog(jobId));
        }
    }
}
