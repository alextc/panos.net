namespace PANOS
{
    using System;
    using System.Net;
    using System.Security;
    using System.Text;
    using System.Text.RegularExpressions;

    public class ConnectionProperties 
    {
        private readonly Regex accessTokenRegex = new Regex("^[A-Za-z0-9+/=]+$");
        private readonly Regex vsysRegex = new Regex("^[A-Za-z0-9-_]+$");

        public ConnectionProperties(string hostName, SecureString accessToken, string vsys)
        {
            // Add try catch?
            Host = Dns.GetHostEntry(hostName); ;

            if (vsysRegex.Match(vsys).Success)
            {
                Vsys = vsys;
            }
            else
            {
                throw new ArgumentException("Invalid VSYS name format");
            }

            // Removed Uri.EscapeDataString since HttpClient UrlEncodes posted data, thus satisfying the requirement of
            // Url encoding access tokens
            if (accessTokenRegex.Match(SecureStringUtils.ConvertToUnSecureString(accessToken)).Success)
            {
                AccessToken = accessToken;
            }
            else
            {
                throw new ArgumentException("Invalid AccessToken format");
            }
        }

        public IPHostEntry Host { get; private set; }

        public string Vsys { get; private set; }

        public SecureString AccessToken { get; private set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Host: {0},", Host.HostName);
            sb.AppendFormat("Vys: {0}", Vsys);
            return sb.ToString();
        }
    }
}
