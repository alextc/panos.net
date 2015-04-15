namespace PANOS
{
    using System.Management.Automation;
    using System.Security;

    [Cmdlet(VerbsCommon.New, "PANOSConnectionProperties")]
    [OutputType(typeof(ConnectionProperties))]
    public class NewConnectionProperties : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        public string HostName { get; set; }

        [Parameter(Mandatory = true)]
        public SecureString AccessToken { get; set; }

        [Parameter(Mandatory = true)]
        public string Vsys { get; set; }

        protected override void ProcessRecord() 
        {
            WriteObject(new ConnectionProperties(HostName, AccessToken, Vsys));
        }
    }
}
