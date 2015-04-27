namespace PANOS
{
    public abstract class ApiResponseForGetSingle : ApiResponse
    {
        public abstract Maybe<FirewallObject> GetPayload();
    }
}
