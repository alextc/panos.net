namespace PANOS
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Security;

    public class JobApiPostKeyValuePairFactory
    {
        private readonly KeyValuePair<string, string> accessTokenPair;

        public JobApiPostKeyValuePairFactory(SecureString accessToken)
        {
            accessTokenPair = new KeyValuePair<string, string>("key", SecureStringUtils.ConvertToUnSecureString(accessToken));
        }

        public FormUrlEncodedContent CreateGetJobStatus(uint jobId)
        {
            return new FormUrlEncodedContent(new[]
            {
                accessTokenPair,
                new KeyValuePair<string, string>("type", "op"),
                new KeyValuePair<string, string>("cmd", string.Format("<show><jobs><id>{0}</id></jobs></show>", jobId))
            });
        }
    }
}
