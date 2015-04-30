namespace PANOSLibTest
{
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using PANOS;

    [TestFixture(typeof(AddressObject), typeof(GetSingleAddressApiResponse), typeof(GetAllAddressesApiResponse), Schema.AddressSchemaName, ConfigTypes.Candidate)]
    [TestFixture(typeof(SubnetObject), typeof(GetSingleAddressApiResponse), typeof(GetAllAddressesApiResponse), Schema.AddressSchemaName, ConfigTypes.Candidate)]
    [TestFixture(typeof(AddressRangeObject), typeof(GetSingleAddressApiResponse), typeof(GetAllAddressesApiResponse), Schema.AddressSchemaName, ConfigTypes.Candidate)]
    [TestFixture(typeof(AddressGroupObject), typeof(GetSingleAddressGroupApiResponse), typeof(GetAllAddressGroupApiResponse), Schema.AddressGroupSchemaName, ConfigTypes.Candidate)]
    public class GetTests<T, TGetSingleDeserializer, TGetAllDeserializer> : BaseConfigTest 
        where T: FirewallObject 
        where TGetSingleDeserializer : ApiResponseForGetSingle
        where TGetAllDeserializer : ApiResponseForGetAll
    {
        private readonly string schemaName;
        private readonly ConfigTypes configType;
        private List<T> sut;
        
        public GetTests(string schemaName, ConfigTypes configType)
        {
            this.schemaName = schemaName;
            this.configType = configType;
        }

        [TestFixtureSetUp]
        public void Setup()
        {
            this.sut = new List<T>
            {
                RandomObjectFactory.GenerateRandomObject<T>(),
                RandomObjectFactory.GenerateRandomObject<T>()
            };

            foreach (var objectUnderTest in this.sut)
            {
                this.ConfigRepository.Set(objectUnderTest);
            }

            if (configType == ConfigTypes.Running)
            {
                this.CommitCandidateConfig();
            }
        }

        [TestFixtureTearDown]
        public void CleanUp()
        {
            foreach (var obj in this.sut)
            {
                this.ConfigRepository.Delete(this.schemaName, obj.Name);

                // If this is a group object, delete its members
                // TODO: Deal with nested Groups
                if (obj is AddressGroupObject)
                {
                    foreach (var member in (obj as AddressGroupObject).Members)
                    {
                        this.ConfigRepository.Delete(Schema.AddressSchemaName, member);
                    }
                }
            }
        }

        [Test]
        public void ShouldGetAllObjects() 
        {
            var result = ConfigRepository.GetAll<TGetAllDeserializer, T>(schemaName, configType);
            foreach (var obj in sut)
            {
                var match = result[obj.Name];
                Assert.IsNotNull(match);
                Assert.IsInstanceOf<T>(match);
                Assert.AreEqual(match, obj);
            }
        }
        
        [Test]
        public void ShouldGetSingleObjectRequestedByName()
        {
            var retrievedObject = ConfigRepository.GetSingle<TGetSingleDeserializer, T>(schemaName, sut.First().Name, configType).Single();
            Assert.AreEqual(this.sut.First(), retrievedObject); 
        }

        [Test]
        public void ShouldNotGetAnythingWhenNonExistingNameSupplied()
        {
            var objectUnderTest = RandomObjectFactory.GenerateRandomObject<T>();
            Assert.AreEqual(ConfigRepository.GetSingle<TGetSingleDeserializer, T>(schemaName, objectUnderTest.Name, configType).Count(), 0);
        }  
    }
}
