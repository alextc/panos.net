namespace PANOS
{
    using System.Collections.Generic;

    public interface ISearchableRepository<T> where T : FirewallObject
    {
        Maybe<T> GetSingle<TDeserializer>(string objectName, ConfigTypes configType)
            where TDeserializer : ApiResponseForGetSingle;
        
        Dictionary<string, T> GetAll<TDeserializer>(ConfigTypes configType)
            where TDeserializer : ApiResponseForGetAll;
    }
}
