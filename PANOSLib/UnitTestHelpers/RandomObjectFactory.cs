namespace PANOS
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net;
    using System.Threading;

    public class RandomObjectFactory
    {
        private readonly IConfigRepository configRepository;

        public RandomObjectFactory(IConfigRepository configRepository)
        {
            this.configRepository = configRepository;
        }

        public static string GenerateRandomName(string schemaName = "")
        {
            Thread.Sleep(1000); // Sleeping to create a new seed https://msdn.microsoft.com/en-us/library/ctssatww(v=vs.110).aspx
            var rnd = new Random();
            var randomAddressName = "API-TEST-" + schemaName + rnd.Next(1000, 150000).ToString(CultureInfo.InvariantCulture);
            return randomAddressName;
        }

        public List<T> GenerateRandomObjects<T>(int count = 2) where T : FirewallObject
        {
            var result = new List<T>();
            for (var i = 0; i < count; i++)
            {
                result.Add(GenerateRandomObject<T>());
            }

            return result;
        }

        public T GenerateRandomObject<T>() where T : FirewallObject
        {
            var typeToGenerate = typeof(T).Name;
            var randomName = GenerateRandomName();

            switch (typeToGenerate)
            {
                case "AddressObject":
                    FirewallObject newAddress = new AddressObject(randomName, GenerateRandomIpAddress(), string.Empty);
                    return (T)newAddress;

                case "SubnetObject":
                    FirewallObject newSubnet = new SubnetObject(
                        randomName,
                        GenerateRandomIpSubnetAddress(),
                        24,
                        string.Empty);
                    return (T)newSubnet;

                case "AddressRangeObject":
                    FirewallObject addressRange = GenerateRandomAddressRange();
                    return (T)addressRange;

                case "AddressGroupObject":
                    FirewallObject addressGroup = GenerateRandomAddressGroup();
                    return (T)addressGroup;

                default:
                    throw new ArgumentException(string.Format("Unexpected type {0}", typeToGenerate));
            }
        }

        
        public static IPAddress GenerateRandomIpAddress()
        {
            Thread.Sleep(1000); // Sleeping to create a new seed https://msdn.microsoft.com/en-us/library/ctssatww(v=vs.110).aspx
            var rnd = new Random();
            return IPAddress.Parse("10." + rnd.Next(1, 254) + "." + rnd.Next(1, 254) + "." + rnd.Next(1, 254));
        }

        public static AddressRangeObject GenerateRandomAddressRange()
        {
            Thread.Sleep(1000); // Sleeping to create a new seed https://msdn.microsoft.com/en-us/library/ctssatww(v=vs.110).aspx
            var rnd = new Random();
            var start = IPAddress.Parse("10.10.10." + rnd.Next(1, 10));
            var end = IPAddress.Parse("10.10.10." + rnd.Next(11, 254));
            return new AddressRangeObject(GenerateRandomName(), start, end);
        }

        public static IPAddress GenerateRandomIpSubnetAddress()
        {
            Thread.Sleep(1000); // Sleeping to create a new seed https://msdn.microsoft.com/en-us/library/ctssatww(v=vs.110).aspx
            var rnd = new Random();
            var subnet = IPAddress.Parse("10." + rnd.Next(1, 254) + "." + rnd.Next(1, 254) + ".0");
            return subnet;
        }

        // TODO: Add Nested AddressGroup
        private AddressGroupObject GenerateRandomAddressGroup()
        {
            var address = GenerateRandomObject<AddressObject>();
            var subnet = GenerateRandomObject<SubnetObject>();
            var range = GenerateRandomObject<AddressRangeObject>();
            this.configRepository.Set(address);
            this.configRepository.Set(subnet);
            this.configRepository.Set(range);

            var members = new List<string> { address.Name, subnet.Name, range.Name };
            return new AddressGroupObject(GenerateRandomName(), members);
        }
    }
}
