namespace PANOS
{
    public interface IJobCommandFactory
    {
        ICommand<ApiEnqueuedJobRequestStatusResponse> CreateGetJobStatus(uint jobId);
    }
}
