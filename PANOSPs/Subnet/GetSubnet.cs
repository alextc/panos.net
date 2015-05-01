namespace PANOS
{
    using System.Collections.Generic;
    using System.Management.Automation;

    [Cmdlet(VerbsCommon.Get, "PANOSSubnet", DefaultParameterSetName = "GetAll")]
    [OutputType(typeof(AddressObject))]
    public class GetSubnet : SearchesObjects
    {
        private Dictionary<string, SubnetObject> result;
        
        
        // I assume that it is as expensive to get all addresses as one address so getting them all - later in ProcessRecord filtering out based on what the user requested
        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            ISearchableRepository<SubnetObject> searchableRepository = new SearchableRepository<SubnetObject>(
               new ConfigCommandFactory(
                   new ApiUriFactory(Connection.Host),
                   new ConfigApiPostKeyValuePairFactory(Connection.AccessToken, Connection.Vsys)),
               Schema.AddressSchemaName);
            
            try
            {
                result = searchableRepository.GetAll<GetAllAddressesApiResponse>(ConfigType);
            }
            catch (ResponseFailure ex)
            {
                ThrowTerminatingError(new ErrorRecord(ex, ex.Data[ResponseFailure.MessageFiled].ToString(), ErrorCategory.ConnectionError, null));  
            }
        }

        protected override void ProcessRecord()
        {
            FilterResponse(result);
        } 
    }
}

