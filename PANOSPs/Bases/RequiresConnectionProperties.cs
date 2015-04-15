namespace PANOS
{
    using System.Management.Automation;

    public abstract class RequiresConnectionProperties : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public ConnectionProperties ConnectionProperties { get; set; }
    }
}
