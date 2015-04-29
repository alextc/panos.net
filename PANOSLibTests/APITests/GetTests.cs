namespace PANOSLibTest
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PANOS;

    public class GetTests<T, TGetSingleDeserializer, TGetAllDeserializer> : BaseConfigTest 
        where T: FirewallObject 
        where TGetSingleDeserializer : ApiResponseForGetSingle
        where TGetAllDeserializer : ApiResponseForGetAll
    {
        private readonly IRandomFirewallObjectGenerator<T> randomFirewallObjectGenerator;

        private readonly string schemaName;

        public GetTests(IRandomFirewallObjectGenerator<T> randomFirewallObjectGenerator, string schemaName )
        {
            this.randomFirewallObjectGenerator = randomFirewallObjectGenerator;
            this.schemaName = schemaName;
        }

        public void GetAllObjects(ConfigTypes configType) 
        {
            // Setup - Ensure that at least 2 addresses are present
            var objectsUnderTest = new List<T>
                {
                    randomFirewallObjectGenerator.Generate(),
                    randomFirewallObjectGenerator.Generate()
                };
            ConfigRepository.Set(objectsUnderTest[0]);
            ConfigRepository.Set(objectsUnderTest[1]);

            if (configType == ConfigTypes.Running)
            {
                CommitCandidateConfig();
            }

            // Test
            var result = this.ConfigRepository.GetAll<TGetAllDeserializer, T>(schemaName, configType);
            foreach (var obj in objectsUnderTest)
            {
                var match = result[obj.Name];
                Assert.IsNotNull(match);
                Assert.IsInstanceOfType(match, typeof(T));
                Assert.AreEqual(match, obj);
            }

            // Clean-up
            foreach (var obj in objectsUnderTest)
            {
                ConfigRepository.Delete(schemaName, obj.Name);

                // If this is a group object, delete its members
                // TODO: Deal with nested Groups
                if (obj is AddressGroupObject)
                {
                    foreach (var member in (obj as AddressGroupObject).Members)
                    {
                        ConfigRepository.Delete(Schema.AddressSchemaName, member);
                    }
                }
            }
        }

        public void GetSingleObject(ConfigTypes configType)
        {
            // Setup
            var objectUnderTest = randomFirewallObjectGenerator.Generate();
            ConfigRepository.Set(objectUnderTest);

            if (configType == ConfigTypes.Running)
            {
                CommitCandidateConfig();
            }
            
            // Test
            var retrievedObject = ConfigRepository.GetSingle<TGetSingleDeserializer, T>(schemaName, objectUnderTest.Name, configType).Single();
            Assert.AreEqual(objectUnderTest, retrievedObject);

            // Clean-up
            ConfigRepository.Delete(schemaName, objectUnderTest.Name);
        }

        public void GetNonExistingObject(ConfigTypes configType)
           
        {
            var objectUnderTest = randomFirewallObjectGenerator.Generate();
            Assert.AreEqual(ConfigRepository.GetSingle<TGetSingleDeserializer, T>(schemaName, objectUnderTest.Name, configType).Count(), 0);
        }
    }
}
