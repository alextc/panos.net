namespace PANOS
{
    using System;
    using System.Net;

    public class ApiUriFactory
    {
        private readonly IPHostEntry hostEntry;
        public ApiUriFactory(IPHostEntry hostEntry)
        {
            // Should not I validate the host here by doing DNS lookup or poing?
            this.hostEntry = hostEntry;
        }

        public Uri Create()
        {
            Uri hostUri;
            if (!Uri.TryCreate(string.Format("https://{0}/api/?", hostEntry.HostName), UriKind.Absolute, out hostUri))
            {
                throw new Exception(
                    string.Format("Unable to create Uri from {0}", hostEntry.HostName));
            }

            return hostUri;
        }
    }
}
