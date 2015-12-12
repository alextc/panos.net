namespace PANOSLibTest
{
    using System.Linq;
    using NUnit.Framework;
    using PANOS;
    
    [TestFixture(typeof(AddressObject), typeof(GetSingleAddressApiResponse), Schema.AddressSchemaName)]
    [TestFixture(typeof(SubnetObject), typeof(GetSingleAddressApiResponse), Schema.AddressSchemaName)]
    [TestFixture(typeof(AddressRangeObject), typeof(GetSingleAddressApiResponse), Schema.AddressSchemaName)]
    [TestFixture(typeof(AddressGroupObject), typeof(GetSingleAddressGroupApiResponse), Schema.AddressGroupSchemaName)]
    public class RenameTests<T, TDeserializer> : BaseConfigTest
        where T : FirewallObject
        where TDeserializer : ApiResponseForGetSingle
    {
        private readonly ISearchableRepository<T> searchableRepository;
        private readonly IRenamableRepository renamableRepository;

        public RenameTests(string schemaName)
        {
            searchableRepository = new SearchableRepository<T>(ConfigCommandFactory, schemaName);
            renamableRepository = new RenamableRepository(ConfigCommandFactory);
        }

        [Test]
        public void ShouldRenameObject()  
        {
            // Setup
            var sut = RandomObjectFactory.GenerateRandomObject<T>();
            AddableRepository.Add(sut);

            // Test
           var newName = RandomObjectFactory.GenerateRandomName();
           renamableRepository.Rename(sut.SchemaName, sut.Name, newName);

            // Postcondition
            Assert.IsTrue(searchableRepository.GetSingle<TDeserializer>(newName, ConfigTypes.Candidate).Any());
            Assert.IsFalse(searchableRepository.GetSingle<TDeserializer>(sut.Name, ConfigTypes.Candidate).Any());

            // Clean-up
            DeletableRepository.Delete(sut.SchemaName, newName);
        }

        [Test]
        public void ShouldNotRenameNonExistingTest()
        {
            // Precondition
            var obj = RandomObjectFactory.GenerateRandomObject<T>();

            // Test
            Assert.That( 
                () => 
                    renamableRepository.Rename(obj.SchemaName, obj.Name, "foo"),
                    Throws.TypeOf<AttemptToRenameNonExistingObject>());
        }
    }
}
