namespace PANOSPsTest
{
    using System.Linq;
    using NUnit.Framework;
    using PANOS;

    [TestFixture(typeof(AddressGroupObject), typeof(AddressObject), typeof(GetSingleAddressGroupApiResponse),  "PANOSAddressGroupMember", Schema.AddressGroupSchemaName)]
    public class PsAddMemberTests<TGroup, TMember, TGroupDeserializer> : BasePsTest
        where TGroup : GroupFirewallObject
        where TMember : FirewallObject
        where TGroupDeserializer : ApiResponseForGetSingle
    {
        private readonly string noun;
        private readonly PsTestRunner<TGroup> psTestRunner;
        private readonly ISearchableRepository<TGroup> searchableRepository;
        
        public PsAddMemberTests(string noun, string groupSchemaName)
        {
            this.noun = noun;
            psTestRunner = new PsTestRunner<TGroup>();
            searchableRepository = new SearchableRepository<TGroup>(ConfigCommandFactory, groupSchemaName);
        }

        [Test]
        public void ShouldAddSingleMemberPassedAsParameter()
        {
            // Setup
            var sut = RandomObjectFactory.GenerateRandomObject<TGroup>();
            AddableRepository.Add(sut);
            var newMember = RandomObjectFactory.GenerateRandomObject<TMember>();
            AddableRepository.Add(newMember);
            
            // Test
            var script = string.Format("Add-{0} -GroupName {1} -MemberName {2};", noun, sut.Name, newMember.Name);
            psTestRunner.ExecuteCommand(script);

            // Validate
            var updatedGroup = searchableRepository.GetSingle<TGroupDeserializer>(sut.Name, ConfigTypes.Candidate).Single();
            Assert.IsTrue(updatedGroup.Members.Contains(newMember.Name));
            Assert.AreNotEqual(sut.Members.Count, updatedGroup.Members.Count);
            Assert.IsTrue((updatedGroup.Members.Count - sut.Members.Count) == 1);
            
            // Cleanup
            DeletableRepository.Delete(sut.SchemaName, sut.Name);
            foreach (var member in updatedGroup.Members)
            {
                DeletableRepository.Delete(newMember.SchemaName, member);
            } 
        }
    }
}
