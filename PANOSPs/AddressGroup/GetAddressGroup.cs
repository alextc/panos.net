namespace PANOS
{
    using System.Collections.Generic;
    using System.Management.Automation;

    [Cmdlet(VerbsCommon.Get, "PANOSAddressGroup", DefaultParameterSetName = "GetAll")]
    [OutputType(typeof(AddressObject))]
    public class GetAddressGroup : SearchesObjects
    {
        private Dictionary<string, AddressGroupObject> result;

        // I assume that it is as expensive to get all addresses as one address so getting them all - later in ProcessRecord filtering out based on what the user requested
        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            ISearchableRepository<AddressGroupObject> searchableRepository = new SearchableRepository<AddressGroupObject>(
               new ConfigCommandFactory(
                   new ApiUriFactory(Connection.Host),
                   new ConfigPostKeyValuePairFactory(Connection.AccessToken, Connection.Vsys)),
               Schema.AddressGroupSchemaName);

            try
            {
                result = searchableRepository.GetAll<GetAllAddressGroupApiResponse>(ConfigType);
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
