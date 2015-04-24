namespace PANOS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    using PANOS.Logging;

    // TODO: Add input validation:
    // 1. check for nulls
    // 2. do regex to check for valid PANOS names
    public class ConfigRepository : IConfigRepository
    {
        private readonly IConfigCommandFactory commandFactory;

        public ConfigRepository(IConfigCommandFactory commandFactory)
        {
            this.commandFactory = commandFactory;
        }

        public Maybe<TObject> GetSingle<TDeserializer, TObject>(
            string schemaName,
            string objectName,
            ConfigTypes configType)
            where TDeserializer : ApiResponse, IPayload
            where TObject : FirewallObject
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
                    return new Maybe<TObject>();
                }

                throw;
            }

            if (!deserializedResult.GetPayload().Any())
            {
                 return new Maybe<TObject>();
            }

            var deserializedFirewallObject =  deserializedResult.GetPayload().Single() as TObject;
            if (deserializedFirewallObject == null)
            {
                throw new SerializationException("Unable to Deserealize Payload to the requested type");
            }
            
            return new Maybe<TObject>(deserializedFirewallObject);
        }

        public Dictionary<string, TObject> GetAll<TDeserializer, TObject>(
            string schemaName,
            ConfigTypes configType) 
            where TDeserializer : ApiResponse, IDictionaryPayload 
            where TObject : FirewallObject
        {
            Logger.LogFunctionEntered("GetAll");
            var deserializedResult = commandFactory.CreateGetAll<TDeserializer>(schemaName, configType).Execute();
            if (deserializedResult.Status.Equals("success"))
            {
                return deserializedResult.
                    GetPayload().
                    Where(entry => entry.Value.GetType() == typeof(TObject)).
                    ToDictionary(entry => entry.Key, entry => (TObject)entry.Value);
            }

            Logger.LogFunctionExited("GetAll");
            throw new Exception(string.Format("GetAll Method failed. PANOS error code {0}", deserializedResult.Status));
        }

        // TODO: Violates CQS, refactor to return void
        // Add a method that would allow a caller to check for the status of the last command
        public string Rename(string schemaName, string oldName, string newName)
        {
            var renameCommand = commandFactory.CreateRename(schemaName, oldName, newName);
            var response = renameCommand.Execute();
            // What is the status of an attempt to rename an non-existing object
            if (response.Status.Equals("success"))
            {
                return response.NameActedUpon;
            }
            
            throw new Exception(string.Format("Rename Method failed. PANOS error code {0}", response.Status));
        }

        // TODO: Fix CQS violation
        public void Delete(string schemaName, string name)
        {
            var response = commandFactory.CreateDelete(schemaName, name).Execute();
            // What is the status of an attempt to delete an non-existing object
            if (response.Status.Equals("success") && !response.Message.Equals("Object doesn't exist"))
            {
                return;
            }

            if (response.Message.Equals("Object doesn't exist"))
            {
                throw new ObjectNotFound(string.Format("Attempt to Delete a non-existing object {0}", name));
            }

            throw new Exception(string.Format("Delete Method failed. PANOS error code {0}", response.Status));
        }

        // TODO: Fix CQS
        public void Set(FirewallObject firewallObject)
        {
            var response = commandFactory.CreateSet(firewallObject).Execute();
            if (response.Status.Equals("success"))
            {
                Logger.LogPanosAddEditResponse(response);
            }
            else
            {
                Logger.LogPanosFailedAddEditResponse(response, firewallObject);
                throw new Exception(string.Format("Add Method failed. PANOS error code {0}", response.Status));
            }
        }

        // TODO: Fix CQS
        public ApiResponseWithMessage SetGroupMembership(GroupFirewallObject groupFirewallObject)
        {
            var response = commandFactory.CreateSetMembership(groupFirewallObject).Execute();
            if (response.Status.Equals("success"))
            {
                return response;
            }

            throw new Exception(string.Format("Set Membership Method failed. PANOS error code {0}", response.Status));
        }

        public void InflateMembers<TDeserializer, TObject>(GroupFirewallObject groupFirewallObject, string memberSchemaName, ConfigTypes configType) 
            where TDeserializer : ApiResponse, IDictionaryPayload
            where TObject : FirewallObject
        {
            var allTObjects = GetAll<TDeserializer, TObject>(memberSchemaName, configType);
            groupFirewallObject.MemberObjects.AddRange(
                (from tObject in allTObjects 
                 where groupFirewallObject.Members.Contains(tObject.Key) 
                 select tObject.Value)
                .ToList());
        }
    }
}
