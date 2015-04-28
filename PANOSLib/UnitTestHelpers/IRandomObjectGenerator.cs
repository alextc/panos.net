namespace PANOS
{
    public interface IRandomFirewallObjectGenerator<T> where T: FirewallObject
    {
        T Generate();
    }
}
