namespace PANOSLibTest
{
    using System.Collections.Generic;
    using System.Linq;
    using PANOS;

    using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

    public class GetTests<T, TGetSingleDeserializer, TGetAllDeserializer> : BaseConfigTest 
        where T: FirewallObject 
        where TGetSingleDeserializer : ApiResponseForGetSingle
        where TGetAllDeserializer : ApiResponseForGetAll
    {
        private readonly IRandomFirewallObjectGenerator<T> randomFirewallObjectGenerator;
        private readonly string schemaName;
        private readonly ConfigTypes configType;
        private List<T> sut;
        
        public GetTests(IRandomFirewallObjectGenerator<T> randomFirewallObjectGenerator, string schemaName, ConfigTypes configType)
        {
            this.randomFirewallObjectGenerator = randomFirewallObjectGenerator;
            this.schemaName = schemaName;
            this.configType = configType;
            this.Setup();
        }

        
        public void ShouldGetAllObjects() 
        {
            var result = this.ConfigRepository.GetAll<TGetAllDeserializer, T>(schemaName, configType);
            foreach (var obj in this.sut)
            {
                var match = result[obj.Name];
                Assert.IsNotNull(match);
                Assert.IsInstanceOfType(match, typeof(T));
                Assert.AreEqual(match, obj);
            }
        }
        
        public void ShouldGetSingleObjectRequestedByName()
        {
            var retrievedObject = ConfigRepository.GetSingle<TGetSingleDeserializer, T>(schemaName, this.sut.First().Name, configType).Single();
            Assert.AreEqual(this.sut.First(), retrievedObject); 
        }

        public void ShouldNotGetAnythingWhenNonExistingNameSupplied()
        {
            var objectUnderTest = randomFirewallObjectGenerator.Generate();
            Assert.AreEqual(ConfigRepository.GetSingle<TGetSingleDeserializer, T>(schemaName, objectUnderTest.Name, configType).Count(), 0);
        }

        private void Setup()
        {
            this.sut = new List<T>
            {
                randomFirewallObjectGenerator.Generate(),
                randomFirewallObjectGenerator.Generate()
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

        private void CleanUp()
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
    }
}
