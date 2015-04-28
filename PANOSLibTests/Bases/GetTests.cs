namespace PANOSLibTest
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PANOS;

    public class GetTests : BaseConfigTest
    {
        public void GetAllObjects<TDeserializer, TObject>(
            string schemaName, ConfigTypes configType,
            IRandomFirewallObjectGenerator<TObject> randomObjectFactory) 
            where TDeserializer : ApiResponseForGetAll where TObject : FirewallObject
        {
            // Setup - Ensure that at least 2 addresses are present
            var objectsUnderTest = new List<TObject>
                {
                    randomObjectFactory.Generate(),
                    randomObjectFactory.Generate()
                };
            ConfigRepository.Set(objectsUnderTest[0]);
            ConfigRepository.Set(objectsUnderTest[1]);

            if (configType == ConfigTypes.Running)
            {
                CommitCandidateConfig();
            }

            // Test
            var result = this.ConfigRepository.GetAll<TDeserializer, TObject>(schemaName, configType);
            foreach (var obj in objectsUnderTest)
            {
                var match = result[obj.Name];
                Assert.IsNotNull(match);
                Assert.IsInstanceOfType(match, typeof(TObject));
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

        public void GetSingleObject<TDeserializer, TObject>(
            string schemaName,
            ConfigTypes configType, 
            IRandomFirewallObjectGenerator<TObject> randomObjectFactory)
            where TDeserializer : ApiResponseForGetSingle
            where TObject : FirewallObject
        {
            // Setup
            var objectUnderTest = randomObjectFactory.Generate();
            ConfigRepository.Set(objectUnderTest);

            if (configType == ConfigTypes.Running)
            {
                CommitCandidateConfig();
            }
            
            // Test
            var retrievedObject = ConfigRepository.GetSingle<TDeserializer, TObject>(schemaName, objectUnderTest.Name, configType).Single();
            Assert.AreEqual(objectUnderTest, retrievedObject);

            // Clean-up
            ConfigRepository.Delete(schemaName, objectUnderTest.Name);
        }

        public void GetNonExistingObject<TDeserializer, TObject>(
            string schemaName, ConfigTypes configType,
            IRandomFirewallObjectGenerator<TObject> randomObjectFactory)
            where TDeserializer : ApiResponseForGetSingle
            where TObject : FirewallObject
        {
            var objectUnderTest = randomObjectFactory.Generate();
            Assert.AreEqual(ConfigRepository.GetSingle<TDeserializer, TObject>(schemaName, objectUnderTest.Name, configType).Count(), 0);
        }
    }
}
