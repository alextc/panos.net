namespace PANOS
{
    using System.Management.Automation;
    using System.Security;

    [Cmdlet(VerbsCommon.New, "PANOSConnection")]
    [OutputType(typeof(Connection))]
    public class NewConnection : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        public string HostName { get; set; }

        [Parameter(Mandatory = true)]
        public SecureString AccessToken { get; set; }

        [Parameter(Mandatory = true)]
        public string Vsys { get; set; }

        protected override void ProcessRecord() 
        {
            WriteObject(new Connection(HostName, AccessToken, Vsys));
        }
    }
}
