﻿namespace PANOS
{
    using System.Collections.Generic;
    using System.Management.Automation;

    [Cmdlet(VerbsCommon.Get, "PANOSAddress", DefaultParameterSetName = "GetAll")]
    [OutputType(typeof(AddressObject))]
    [OutputType(typeof(ObjectNotFoundError))]
    public class GetAddress : SearchesObjects
    {
        private Dictionary<string, AddressObject> result;
        
        // I assume that it is as expensive to get all addresses as one address so getting them all - later in ProcessRecord filtering out based on what the user requested
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            
            try
            {
                result = this.ConfigRepository.GetAll<GetAllAddressesApiResponse, AddressObject>(Schema.AddressSchemaName, ConfigType);
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
