
namespace PANOS
{
    using System.Management.Automation;

    [Cmdlet(VerbsData.Save, "PANOSChanges")]
    [OutputType(typeof(ApiEnqueuedResponse))]
    public class Commit : RequiresConnection
    {
        protected override void ProcessRecord()
        {
            ICommitCommandFactory commitCommandFactory = 
                new CommitApiCommandFactory(new ApiUriFactory(this.Connection.Host), new CommitApiPostKeyValuePairFactory(this.Connection.AccessToken) );
            var command = commitCommandFactory.CreateCommit(true);

            try
            {
                var commitResponse = command.Execute();
                WriteObject(commitResponse);
            }
            catch (ResponseFailure ex)
            {
                ThrowTerminatingError(new ErrorRecord(ex, ex.Data[ResponseFailure.MessageFiled].ToString(), ErrorCategory.InvalidArgument, null));
            }
        }
    }
}
