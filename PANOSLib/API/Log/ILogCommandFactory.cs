namespace PANOS
{
    public interface ILogCommandFactory
    {
        ICommand<ApiEnqueuedResponse> CreateRequestForTrafficLog(string query, uint nlogs, uint skip);

        ICommand<GetTrafficLogApiResponse> CreateConsumeTrafficLog(uint jobId);
    }
}
