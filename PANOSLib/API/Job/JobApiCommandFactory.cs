namespace PANOS
{
    public class JobApiCommandFactory : IJobCommandFactory
    {
        private readonly ApiUriFactory apiUriFactory;
        private readonly JobApiPostKeyValuePairFactory jobApiPostKeyValuePairFactory;

        public JobApiCommandFactory(
            ApiUriFactory apiUriFactory,
            JobApiPostKeyValuePairFactory jobApiPostKeyValuePairFactory)
        {
            this.apiUriFactory = apiUriFactory;
            this.jobApiPostKeyValuePairFactory = jobApiPostKeyValuePairFactory;  
        }

        public ICommand<ApiEnqueuedJobRequestStatusResponse> CreateGetJobStatus(uint jobId)
        {
            return new Command<ApiEnqueuedJobRequestStatusResponse>(apiUriFactory.Create(), jobApiPostKeyValuePairFactory.CreateGetJobStatus(jobId));
        }
    }
}
