namespace PANOS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    public class SearchableRepository<T> : ISearchableRepository<T> where T : FirewallObject
    {
        private readonly IConfigCommandFactory commandFactory;

        private readonly string schemaName;

        public SearchableRepository(IConfigCommandFactory commandFactory, string schemaName)
        {
            this.commandFactory = commandFactory;
            this.schemaName = schemaName;
        }
        
        public Maybe<T> GetSingle<TDeserializer>(string objectName, ConfigTypes configType) where TDeserializer : ApiResponseForGetSingle
        {
            TDeserializer deserializedResult;
            try
            {
                deserializedResult = commandFactory.CreateGetSingle<TDeserializer>(schemaName, objectName, configType).Execute();
            }
            catch (ResponseFailure ex)
            {
                if (configType == ConfigTypes.Running && ex.Data[ResponseFailure.MessageFiled].ToString().Contains("No such node"))
                {
                    return new Maybe<T>();
                }

                throw;
            }

            if (!deserializedResult.GetPayload().Any())
            {
                return new Maybe<T>();
            }

            var deserializedFirewallObject = deserializedResult.GetPayload().Single() as T;
            if (deserializedFirewallObject == null)
            {
                throw new SerializationException("Unable to Deserealize Payload to the requested type");
            }

            return new Maybe<T>(deserializedFirewallObject);
        }

        public Dictionary<string, T> GetAll<TDeserializer>(ConfigTypes configType) where TDeserializer : ApiResponseForGetAll
        {
            var deserializedResult = commandFactory.CreateGetAll<TDeserializer>(schemaName, configType).Execute();
            if (deserializedResult.Status.Equals("success"))
            {
                return deserializedResult.
                    GetPayload().
                    Where(entry => entry.Value.GetType() == typeof(T)).
                    ToDictionary(entry => entry.Key, entry => (T)entry.Value);
            }

            throw new Exception(string.Format("GetAll Method failed. PANOS error code {0}", deserializedResult.Status));
        }
    }
}
