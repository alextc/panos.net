namespace PANOSLibTest
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using PANOS;

    [TestClass]
    public class CompareTests
    {
        [TestMethod]
        public void ShallowEqualsTest()
        {
            // Identical members
            var membersSource = new List<string> { "host1", "host2" };
            var membersTarget = new List<string> { "host1", "host2" };
            var groupSource = new AddressGroupObject("test", membersSource);
            var groupTarget = new AddressGroupObject("test", membersTarget);
            Assert.AreEqual(groupSource, groupTarget);

            // Identical members but potentially different order
            groupSource.Members = new List<string> { "host1", "host2" };
            groupTarget.Members = new List<string> { "host2", "host1" };
            Assert.AreEqual(groupSource, groupTarget);

            // Identical members, but different case
            // Case Sensetive !!!
            groupSource.Members = new List<string> { "host1", "host2" };
            groupTarget.Members = new List<string> { "Host1", "Host2" };
            Assert.AreNotEqual(groupSource, groupTarget);

            // Missing member
            groupSource.Members = new List<string> { "host1", "host2" };
            groupTarget.Members = new List<string> { "host1"};
            Assert.AreNotEqual(groupSource, groupTarget);

            // Different member
            groupSource.Members = new List<string> { "host1", "host2" };
            groupTarget.Members = new List<string> { "host1", "host3" };
            Assert.AreNotEqual(groupSource, groupTarget);
        }

        [TestMethod]
        public void AddressObjectListsCompareTestIdenticalObjects()
        {
            var sourceMemberList = new List<FirewallObject> {new AddressObject("host1", IPAddress.Parse("10.10.10.1")), new AddressObject("host2", IPAddress.Parse("10.10.10.2"))};
            var targetMemberList = new List<FirewallObject> {new AddressObject("host1", IPAddress.Parse("10.10.10.1")), new AddressObject("host2", IPAddress.Parse("10.10.10.2"))};
            var sourceGroup = new AddressGroupObject("group", sourceMemberList.Select(a => a.Name).ToList())
            {
                MemberObjects = sourceMemberList
            };
            var targetGroup = new AddressGroupObject("group", targetMemberList.Select(a => a.Name).ToList())
            {
                MemberObjects = targetMemberList
            };

            Assert.IsTrue(sourceGroup.DeepCompare(targetGroup));
        }

        [TestMethod]
        public void AddressObjectListsCompareTestIdenticalObjectsDifferentOrder()
        {
            var sourceMemberList = new List<FirewallObject> { new AddressObject("host1", IPAddress.Parse("10.10.10.1")), new AddressObject("host2", IPAddress.Parse("10.10.10.2")) };
            var targetMemberList = new List<FirewallObject> { new AddressObject("host2", IPAddress.Parse("10.10.10.2")), new AddressObject("host1", IPAddress.Parse("10.10.10.1")) };
            var sourceGroup = new AddressGroupObject("group", sourceMemberList.Select(a => a.Name).ToList())
            {
                MemberObjects = sourceMemberList
            };
            var targetGroup = new AddressGroupObject("group", targetMemberList.Select(a => a.Name).ToList())
            {
                MemberObjects = targetMemberList
            };

            Assert.IsTrue(sourceGroup.DeepCompare(targetGroup));
        }

        [TestMethod]
        public void AddressObjectListsCompareTestIpMismatch()
        {
            var sourceMemberList = new List<FirewallObject> { new AddressObject("host1", IPAddress.Parse("10.10.10.1")), new AddressObject("host2", IPAddress.Parse("10.10.10.2")) };
            var targetMemberList = new List<FirewallObject> { new AddressObject("host1", IPAddress.Parse("10.10.10.1")), new AddressObject("host2", IPAddress.Parse("10.10.10.3")) };
            var sourceGroup = new AddressGroupObject("group", sourceMemberList.Select(a => a.Name).ToList())
            {
                MemberObjects = sourceMemberList
            };
            var targetGroup = new AddressGroupObject("group", targetMemberList.Select(a => a.Name).ToList())
            {
                MemberObjects = targetMemberList
            };

            Assert.IsFalse(sourceGroup.DeepCompare(targetGroup));
        }

        [TestMethod]
        public void AddressObjectListsCompareTestNameMismatch()
        {
            var sourceMemberList = new List<FirewallObject> { new AddressObject("host1", IPAddress.Parse("10.10.10.1")), new AddressObject("host2", IPAddress.Parse("10.10.10.2")) };
            var targetMemberList = new List<FirewallObject> { new AddressObject("host1", IPAddress.Parse("10.10.10.1")), new AddressObject("host3", IPAddress.Parse("10.10.10.2")) };
            var sourceGroup = new AddressGroupObject("group", sourceMemberList.Select(a => a.Name).ToList())
            {
                MemberObjects = sourceMemberList
            };
            var targetGroup = new AddressGroupObject("group", targetMemberList.Select(a => a.Name).ToList())
            {
                MemberObjects = targetMemberList
            };

            Assert.IsFalse(sourceGroup.DeepCompare(targetGroup));
        }

        [TestMethod]
        public void AddressObjectListsCompareTestMissingMember()
        {
            var sourceMemberList = new List<FirewallObject> { new AddressObject("host1", IPAddress.Parse("10.10.10.1")), new AddressObject("host2", IPAddress.Parse("10.10.10.2")) };
            var targetMemberList = new List<FirewallObject> { new AddressObject("host1", IPAddress.Parse("10.10.10.1"))};
            var sourceGroup = new AddressGroupObject("group", sourceMemberList.Select(a => a.Name).ToList())
            {
                MemberObjects = sourceMemberList
            };
            var targetGroup = new AddressGroupObject("group", targetMemberList.Select(a => a.Name).ToList())
            {
                MemberObjects = targetMemberList
            };

            Assert.IsFalse(sourceGroup.DeepCompare(targetGroup));
        }

        [TestMethod]
        public void AddressObjectListsCompareTestExtraMember()
        {
            var sourceMemberList = new List<FirewallObject> { new AddressObject("host1", IPAddress.Parse("10.10.10.1")), new AddressObject("host2", IPAddress.Parse("10.10.10.2")), new AddressObject("host3", IPAddress.Parse("10.10.10.3")) };
            var targetMemberList = new List<FirewallObject> { new AddressObject("host1", IPAddress.Parse("10.10.10.1")), new AddressObject("host2", IPAddress.Parse("10.10.10.2")) };
            var sourceGroup = new AddressGroupObject("group", sourceMemberList.Select(a => a.Name).ToList())
            {
                MemberObjects = sourceMemberList
            };
            var targetGroup = new AddressGroupObject("group", targetMemberList.Select(a => a.Name).ToList())
            {
                MemberObjects = targetMemberList
            };

            Assert.IsFalse(sourceGroup.DeepCompare(targetGroup));
        }
    }
}
