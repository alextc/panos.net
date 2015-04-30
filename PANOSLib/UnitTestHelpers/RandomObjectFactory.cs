namespace PANOS
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;

    public class RandomObjectFactory
    {
        private readonly IAddableRepository addableRepository;

        public RandomObjectFactory(IAddableRepository addableRepository)
        {
            this.addableRepository = addableRepository;
        }

        public static string GenerateRandomName(string schemaName = "")
        {
            Thread.Sleep(1000);
                // Sleeping to create a new seed https://msdn.microsoft.com/en-us/library/ctssatww(v=vs.110).aspx
            var rnd = new Random();
            var randomAddressName = "API-TEST-" + schemaName
                                    + rnd.Next(1000, 150000).ToString(CultureInfo.InvariantCulture);
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
            
            // Looks to complicated: why down and then up casting?
            switch (typeToGenerate)
            {
                case "AddressObject":
                    FirewallObject addressObject = new RandomAddressObjectFactory().Generate();
                    return (T)addressObject;
                case "SubnetObject":
                    FirewallObject newSubnet = new RandomSubnetObjectFactory().Generate();
                    return (T)newSubnet;

                case "AddressRangeObject":
                    FirewallObject addressRange = new RandomAddressRangeObjectFactory().Generate();
                    return (T)addressRange;

                case "AddressGroupObject":
                    FirewallObject addressGroup = new RandomAddressGroupObjectFactory(addableRepository).Generate();
                    return (T)addressGroup;

                default:
                    throw new ArgumentException(string.Format("Unexpected type {0}", typeToGenerate));
            }
        }
    }
}
