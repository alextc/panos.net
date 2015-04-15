namespace PANOS.Integration
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;

    using PANOS.Logging;

    public class DnsRepository
    {
        public AddressObject IpV4AddressObjectFromFqdn(string fqnd)
        {
            var address = Dns.GetHostAddresses(fqnd).FirstOrDefault(h => h.AddressFamily == AddressFamily.InterNetwork);
            // During tests I have seen situations where DNS was not able to resolve IP or only returned IPV6 - skipping such case
            // TODO: need to warn caller that some entreis were skipped
            if (address != null)
            {
                Logger.LogDnsResolutionResult(fqnd, address);
                return new AddressObject(fqnd.ToUpper(), address);
            }
            
            Logger.LogDnsResolutionFailure(fqnd);
            return null;
        }

        public string HostNameFromIp(IPAddress ipAddress)
        {
            try
            {
                return Dns.GetHostEntry(ipAddress.ToString()).HostName;
            }
            catch (Exception)
            {

                return ipAddress.ToString();
            }
        }
    }
}
