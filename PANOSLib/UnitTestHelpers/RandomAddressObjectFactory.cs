namespace PANOS
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.Threading;

    public class RandomAddressObjectFactory : IRandomFirewallObjectGenerator<AddressObject>
    {
        private static IPAddress GenerateRandomIpAddress() 
        {
            Thread.Sleep(1000); // Sleeping to create a new seed https://msdn.microsoft.com/en-us/library/ctssatww(v=vs.110).aspx
            var rnd = new Random();
            return IPAddress.Parse("10." + rnd.Next(1, 254) + "." + rnd.Next(1, 254) + "." + rnd.Next(1, 254));
        }

        private static string GenerateRandomAddressName()
        {
            Thread.Sleep(1000); // Sleeping to create a new seed https://msdn.microsoft.com/en-us/library/ctssatww(v=vs.110).aspx
            var rnd = new Random();
            var randomAddressName = "API-TEST-ADDRESS" + rnd.Next(1000, 150000).ToString(CultureInfo.InvariantCulture);
            return randomAddressName;
        }

        public AddressObject Generate()
        {
            return new AddressObject(GenerateRandomAddressName(), GenerateRandomIpAddress(), string.Empty);
        }
    }
}
