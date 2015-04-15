namespace PANOSLibTest
{
    using PANOS;

    public class BaseLogTest : BaseTest
    {
        private ILogCommandFactory LogCommandFactory { get; set; }

        private IJobCommandFactory JobCommandFactory { get; set; }

        protected ILogRepository LogRepository { get; set; }

        protected BaseLogTest()
        {
            LogCommandFactory = 
                new LogCommandFactory(
                    new ApiUriFactory(
                        ConnectionProperties.Host), 
                    new LogApiPostKeyValuePairFactory(
                        ConnectionProperties.AccessToken));

            JobCommandFactory = new JobApiCommandFactory(
                new ApiUriFactory(ConnectionProperties.Host),
                new JobApiPostKeyValuePairFactory(ConnectionProperties.AccessToken));
            
            LogRepository = new LogRepository(LogCommandFactory);
        }
    }
}
