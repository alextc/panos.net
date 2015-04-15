namespace PANOS
{
    public interface ICommitCommandFactory
    {
        ICommand<ApiEnqueuedResponse> CreateCommit(bool force);
    }
}
