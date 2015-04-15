namespace PANOS
{
    public interface ICommand<out TApiResponse>
    {
        TApiResponse Execute();
    }
}
