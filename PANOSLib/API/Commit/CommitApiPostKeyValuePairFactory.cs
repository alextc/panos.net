namespace PANOS
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Security;

    public class CommitApiPostKeyValuePairFactory
    {
        private readonly KeyValuePair<string, string> accessTokenPair;

        public CommitApiPostKeyValuePairFactory(SecureString accessToken)
        {
            accessTokenPair = new KeyValuePair<string, string>("key", SecureStringUtils.ConvertToUnSecureString(accessToken));
        }

        public FormUrlEncodedContent CreateCommit(bool force)
        {
            var commitDirective = 
                force ? 
                    new KeyValuePair<string, string>("cmd", "<commit><force>body</force></commit>") : 
                    new KeyValuePair<string, string>("cmd", "<commit></commit>");

            return new FormUrlEncodedContent(new[]
            {
                accessTokenPair,
                new KeyValuePair<string, string>("type", "commit"),
                commitDirective
            });
        }
    }
}
