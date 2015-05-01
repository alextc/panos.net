﻿namespace PANOSLibTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PANOS;

    public class RenameTests : BaseConfigTest
    {
        public bool RenameObject<TDeserializer, TObject>()
            where TObject : FirewallObject
            where TDeserializer : ApiResponseForGetSingle
        {
            // Setup
            var obj = RandomObjectFactory.GenerateRandomObject<TObject>();
            ConfigRepository.Set(obj);

            // Test
            var newName = RandomObjectFactory.GenerateRandomName();
           ConfigRepository.Rename(obj.SchemaName, obj.Name, newName);

            // Postcondition
            Assert.IsNotNull(this.ConfigRepository.GetSingle<TDeserializer, TObject>(obj.SchemaName, newName, ConfigTypes.Candidate));
            Assert.IsNull(this.ConfigRepository.GetSingle<TDeserializer, TObject>(obj.SchemaName, obj.Name, ConfigTypes.Candidate));

            // Clean-up
            ConfigRepository.Delete(obj.SchemaName, newName);
            // Commit Changes --- effectively backing-out
            // CommitCandidateConfig();

            return true;
        }


        public void RenameNonExistingTest<TObject>() where TObject : FirewallObject
            
        {
            // Precondition
            var obj = RandomObjectFactory.GenerateRandomObject<TObject>();

            // Test
            ConfigRepository.Rename(obj.SchemaName, obj.Name, "foo");
        }
    }
}