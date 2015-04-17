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
                        Connection.Host), 
                    new LogApiPostKeyValuePairFactory(
                        Connection.AccessToken));

            JobCommandFactory = new JobApiCommandFactory(
                new ApiUriFactory(Connection.Host),
                new JobApiPostKeyValuePairFactory(Connection.AccessToken));
            
            LogRepository = new LogRepository(LogCommandFactory);
        }
    }
}
