namespace PANOSLibTest
{
    using System.Linq;
    using NUnit.Framework;
    using PANOS;
    
    [TestFixture(typeof(AddressObject), typeof(GetSingleAddressApiResponse), Schema.AddressSchemaName)]
    [TestFixture(typeof(SubnetObject), typeof(GetSingleAddressApiResponse), Schema.AddressSchemaName)]
    [TestFixture(typeof(AddressRangeObject), typeof(GetSingleAddressApiResponse), Schema.AddressSchemaName)]
    [TestFixture(typeof(AddressGroupObject), typeof(GetSingleAddressGroupApiResponse), Schema.AddressGroupSchemaName)]
    public class DeleteTests<T, TDeserializer> : BaseConfigTest
        where T : FirewallObject
        where TDeserializer : ApiResponseForGetSingle
    {
        private readonly ISearchableRepository<T> searchableRepository;

        public DeleteTests(string schemaName)
        {
            searchableRepository = new SearchableRepository<T>(ConfigCommandFactory, schemaName );
        }

        [Test]
        public void ShouldDeleteObject()
        {
            // Precondition
            var sut = RandomObjectFactory.GenerateRandomObject<T>();
            AddableRepository.Add(sut);
            Assert.IsTrue(searchableRepository.GetSingle<TDeserializer>(sut.Name, ConfigTypes.Candidate).Any());

            // Test
            DeletableRepository.Delete(sut.SchemaName, sut.Name);

            // Postcondition
            Assert.IsFalse(searchableRepository.GetSingle<TDeserializer>(sut.Name, ConfigTypes.Candidate).Any());
        }

        [Test]
        [ExpectedException(typeof(ObjectNotFound))]
        public void ShouldNOtDeleteNonExistingObject() 
        {
            // Precondition
            var sut = RandomObjectFactory.GenerateRandomObject<T>();

            // Test
            ConfigRepository.Delete(sut.SchemaName, sut.Name);
        }
    }
}
