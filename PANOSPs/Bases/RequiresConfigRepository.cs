namespace PANOS
{
    public abstract class RequiresConfigRepository : RequiresConnectionProperties
    {
        protected IConfigRepository ConfigRepository { get; private set; }

        protected override void BeginProcessing()
        {
            ConfigRepository = new ConfigRepository(
                new ConfigCommandFactory(new ApiUriFactory(
                    ConnectionProperties.Host),
                new ConfigApiPostKeyValuePairFactory(
                        ConnectionProperties.AccessToken,
                        ConnectionProperties.Vsys)));
        }
    }
}
