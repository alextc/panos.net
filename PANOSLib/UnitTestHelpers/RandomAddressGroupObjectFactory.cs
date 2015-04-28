namespace PANOS
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;

    public class RandomAddressGroupObjectFactory : IRandomFirewallObjectGenerator<AddressGroupObject>
    {
        private readonly IAddableRepository addableRepository;

        // TODO: Make this depend only on Set Method of the IConfigRepository
        public RandomAddressGroupObjectFactory(IAddableRepository addableRepository)
        {
            this.addableRepository = addableRepository;
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
            this.addableRepository.Add(address);
            this.addableRepository.Add(subnet);
            this.addableRepository.Add(range);

            var members = new List<string> { address.Name, subnet.Name, range.Name };
            return new AddressGroupObject(GenerateRandomName(), members);
        }
    }
}
