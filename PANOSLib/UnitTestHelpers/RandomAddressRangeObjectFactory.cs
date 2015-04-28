namespace PANOS
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.Threading;

    public class RandomAddressRangeObjectFactory : IRandomFirewallObjectGenerator<AddressRangeObject>
    {

        private string GenerateRandomName()
        {
            Thread.Sleep(1000); // Sleeping to create a new seed https://msdn.microsoft.com/en-us/library/ctssatww(v=vs.110).aspx
            var rnd = new Random();
            var randomAddressName = "API-TEST-RANGE" + rnd.Next(1000, 150000).ToString(CultureInfo.InvariantCulture);
            return randomAddressName;
        }

        private AddressRangeObject GenerateRandomAddressRange()
        {
            Thread.Sleep(1000); // Sleeping to create a new seed https://msdn.microsoft.com/en-us/library/ctssatww(v=vs.110).aspx
            var rnd = new Random();
            var start = IPAddress.Parse("10.10.10." + rnd.Next(1, 10));
            var end = IPAddress.Parse("10.10.10." + rnd.Next(11, 254));
            return new AddressRangeObject(GenerateRandomName(), start, end);
        }

        public AddressRangeObject Generate()
        {
            return GenerateRandomAddressRange();
        }
    }
}
