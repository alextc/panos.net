namespace PANOS
{
    using System.Collections.Generic;

    public abstract class ApiResponseForGetAll : ApiResponse
    {
        public abstract Dictionary<string, FirewallObject> GetPayload();
    }
}
