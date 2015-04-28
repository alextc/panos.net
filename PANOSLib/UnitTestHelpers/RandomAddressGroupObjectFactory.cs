namespace PANOS
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;

    public class RandomAddressGroupObjectFactory : IRandomFirewallObjectGenerator<AddressGroupObject>
    {
        private readonly IConfigRepository configRepository;

        public RandomAddressGroupObjectFactory(IConfigRepository configRepository)
        {
            this.configRepository = configRepository;
        }

         private string GenerateRandomName()
         {
            Thread.Sleep(1000); // Sleeping to create a new seed https://msdn.microsoft.com/en-us/library/ctssatww(v=vs.110).aspx
            var rnd = new Random();
            var randomAddressName = "API-TEST-ADDRESSGROUP" +  rnd.Next(1000, 150000).ToString(CultureInfo.InvariantCulture);
            return randomAddressName;
        }

        public AddressGroupObject Generate()
        {
            var address = new RandomAddressObjectFactory().Generate();
            var subnet = new RandomSubnetObjectFactory().Generate();
            var range = new RandomAddressRangeObjectFactory().Generate();
            this.configRepository.Set(address);
            this.configRepository.Set(subnet);
            this.configRepository.Set(range);

            var members = new List<string> { address.Name, subnet.Name, range.Name };
            return new AddressGroupObject(GenerateRandomName(), members);
        }
    }
}
