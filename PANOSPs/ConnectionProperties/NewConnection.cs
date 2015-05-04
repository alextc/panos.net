namespace PANOS
{
    using System.Management.Automation;
    using System.Security;

    [Cmdlet(VerbsCommon.New, "PANOSConnection")]
    [OutputType(typeof(Connection))]
    public class NewConnection : PSCmdlet
    {
        private const string ConnectionSessionVariable = "PanosConnectionSessionVariable";

        [Parameter(Mandatory = true)]
        public string HostName { get; set; }

        [Parameter(Mandatory = true)]
        public SecureString AccessToken { get; set; }

        [Parameter(Mandatory = true)]
        public string Vsys { get; set; }

        [Parameter]
        public SwitchParameter StoreInSession { get; set; }

        protected override void ProcessRecord()
        {
            var connection = new Connection(HostName, AccessToken, Vsys);
            if (StoreInSession)
            {
                SessionState.PSVariable.Set(ConnectionSessionVariable, connection);
            }
            WriteObject(new Connection(HostName, AccessToken, Vsys));
        }
    }
}
