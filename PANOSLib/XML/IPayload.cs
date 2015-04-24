namespace PANOS
{
    public interface IPayload
    {
        Maybe<FirewallObject> GetPayload();
    }
}
