namespace PANOS
{
    using System;
    using System.Management.Automation;

    public abstract class RequiresConnection : PSCmdlet
    {
        private const string ConnectionSessionVariable = "PanosConnectionSessionVariable";

        [Parameter]
        [ValidateNotNullOrEmpty]
        public Connection Connection { get; set; }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            this.ValidateParameters();
        }

        private void ValidateParameters()
        {
            if (Connection != null)
            {
                SessionState.PSVariable.Set(ConnectionSessionVariable, Connection);
            }
            else
            {
                Connection = SessionState.PSVariable.GetValue(ConnectionSessionVariable) as Connection;
                if (Connection == null)
                {
                    ThrowParameterError("Connection");
                }
            }
        }

        private void ThrowParameterError(string parameterName)
        {
            ThrowTerminatingError(
                new ErrorRecord(
                    new ArgumentException(String.Format(
                        "Must specify '{0}'", parameterName)),
                    Guid.NewGuid().ToString(),
                    ErrorCategory.InvalidArgument,
                    null));
        }
    }
}
