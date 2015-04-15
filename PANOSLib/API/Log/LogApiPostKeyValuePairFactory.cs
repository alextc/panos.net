namespace PANOS
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Security;

    // https://palab.redmond.corp.microsoft.com/api/?type=log&log-type=traffic&query=( receive_time geq '2015/01/22 08:00:00')&REST_API_TOKEN=1888195679
    // https://palab.redmond.corp.microsoft.com/api/?type=log&action=get&job-id=20&REST_API_TOKEN=1888195679

    public class LogApiPostKeyValuePairFactory
    {
        private readonly KeyValuePair<string, string> accessTokenPair;
        private readonly KeyValuePair<string, string> typeLogPair = new KeyValuePair<string, string>("type", "log");
        private readonly KeyValuePair<string, string> trafficLogType = new KeyValuePair<string, string>("log-type", "traffic");
        
        public LogApiPostKeyValuePairFactory(SecureString accessToken)
        {
            accessTokenPair = new KeyValuePair<string, string>("key", SecureStringUtils.ConvertToUnSecureString(accessToken));
        }

        public FormUrlEncodedContent CreateRequestForTrafficLog(string query, uint nlogs, uint skip)
        {
            if (string.IsNullOrEmpty(query))
            {
                return new FormUrlEncodedContent(new[]
                    {
                        accessTokenPair,
                        typeLogPair, 
                        trafficLogType,
                        new KeyValuePair<string, string>("nlogs", nlogs.ToString()), 
                        new KeyValuePair<string, string>("skip", skip.ToString())
                    });
            }

            return new FormUrlEncodedContent(new[]
            {
                accessTokenPair,
                typeLogPair, 
                trafficLogType,
                new KeyValuePair<string, string>("nlogs", nlogs.ToString()), 
                new KeyValuePair<string, string>("skip", skip.ToString()),
                new KeyValuePair<string, string>("query", query)
            });
        }

        public FormUrlEncodedContent CreateConsumeTrafficLog(uint jobId)
        {
            return new FormUrlEncodedContent(new[]
            {
                accessTokenPair,
                typeLogPair,
                new KeyValuePair<string, string>("action", "get"), 
                new KeyValuePair<string, string>("job-id", jobId.ToString())
            });
        }
    }
}
