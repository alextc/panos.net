namespace PANOS
{
    using System.Management.Automation;

    public abstract class RequiresConnection : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public Connection Connection { get; set; }
    }
}
