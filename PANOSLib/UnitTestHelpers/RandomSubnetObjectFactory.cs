namespace PANOS
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.Threading;

    public class RandomSubnetObjectFactory : IRandomFirewallObjectGenerator<SubnetObject>
    {
        private static string GenerateRandomName()
        {
            Thread.Sleep(1000); // Sleeping to create a new seed https://msdn.microsoft.com/en-us/library/ctssatww(v=vs.110).aspx
            var rnd = new Random();
            var randomSubnetName = "API-TEST-SUBNET"  + rnd.Next(1000, 150000).ToString(CultureInfo.InvariantCulture);
            return randomSubnetName;
        }

        private IPAddress GenerateRandomIpSubnetAddress()
        {
            Thread.Sleep(1000); // Sleeping to create a new seed https://msdn.microsoft.com/en-us/library/ctssatww(v=vs.110).aspx
            var rnd = new Random();
            var subnet = IPAddress.Parse("10." + rnd.Next(1, 254) + "." + rnd.Next(1, 254) + ".0");
            return subnet;
        }

        public SubnetObject Generate()
        {
            return new SubnetObject(GenerateRandomName(), GenerateRandomIpSubnetAddress(), 24, string.Empty);
        }
    }
}
