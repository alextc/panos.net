namespace PANOSLibTest
{
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PANOS;

    public class SetTests : BaseConfigTest
    {
        public bool AddObject<TDeserializer, TObject>(string schemaName) where TObject : FirewallObject where TDeserializer : ApiResponse, IPayload
        {
            // Precondition
            var newObj = RandomObjectFactory.GenerateRandomObject<TObject>();

            // Test
            ConfigRepository.Set(newObj);

            // Postcondition
            var result = this.ConfigRepository.GetSingle<TDeserializer, TObject>(schemaName, newObj.Name, ConfigTypes.Candidate).Single();
            Assert.AreEqual(result, newObj);

            // Clean-up
            Assert.IsNotNull(this.ConfigRepository.Delete(schemaName, result.Name));
            // Commit Changes --- effectively backing-out
            // CommitCandidateConfig();

            return true;
        }
    }
}
