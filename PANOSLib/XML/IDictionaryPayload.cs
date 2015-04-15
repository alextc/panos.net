namespace PANOS
{
    using System.Collections.Generic;

    public interface IDictionaryPayload
    {
        Dictionary<string, FirewallObject> GetPayload();
    }
}
