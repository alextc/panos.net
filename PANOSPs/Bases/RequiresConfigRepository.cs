namespace PANOS
{
    public abstract class RequiresConfigRepository : RequiresConnection
    {
        protected IConfigRepository ConfigRepository { get; private set; }

        protected override void BeginProcessing()
        {
            ConfigRepository = new ConfigRepository(
                new ConfigCommandFactory(new ApiUriFactory(
                    this.Connection.Host),
                new ConfigApiPostKeyValuePairFactory(
                        this.Connection.AccessToken,
                        this.Connection.Vsys)));
        }
    }
}
