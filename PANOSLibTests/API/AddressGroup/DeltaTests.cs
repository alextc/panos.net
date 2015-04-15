﻿namespace PANOSLibTest.API.AddressGroup
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using PANOS;

    [TestClass]
    public class DeltaTests
    {
        [TestMethod]
        public void EqualGroupsTest()
        {
            var sourceMemberList = new List<FirewallObject> { new AddressObject("host1", IPAddress.Parse("10.10.10.1")), new AddressObject("host2", IPAddress.Parse("10.10.10.2")) };
            var targetMemberList = new List<FirewallObject> { new AddressObject("host1", IPAddress.Parse("10.10.10.1")), new AddressObject("host2", IPAddress.Parse("10.10.10.2")) };
            var sourceGroup = new AddressGroupObject("group", sourceMemberList.Select(a => a.Name).ToList())
            {
                MemberObjects = sourceMemberList
            };
            var targetGroup = new AddressGroupObject("group", targetMemberList.Select(a => a.Name).ToList())
            {
                MemberObjects = targetMemberList
            };

            Assert.IsNull(sourceGroup.GetDelta(targetGroup));
        }

        [TestMethod]
        public void IpMismatchGroupsTest()
        {
            var sourceMemberList = new List<FirewallObject>
            {
                new AddressObject("host1", IPAddress.Parse("10.10.10.3")),
                new AddressObject("host2", IPAddress.Parse("10.10.10.2"))
            };
            var targetMemberList = new List<FirewallObject>
            {
                new AddressObject("host1", IPAddress.Parse("10.10.10.1")),
                new AddressObject("host2", IPAddress.Parse("10.10.10.2"))
            };
            var sourceGroup = new AddressGroupObject("group", sourceMemberList.Select(a => a.Name).ToList())
            {
                MemberObjects = sourceMemberList
            };
            var targetGroup = new AddressGroupObject("group", targetMemberList.Select(a => a.Name).ToList())
            {
                MemberObjects = targetMemberList
            };

            var delta = sourceGroup.GetDelta(targetGroup);
            Assert.IsNotNull(delta);
            Assert.IsTrue(delta.Count == 1);
            Assert.AreEqual(delta[0], new AddressObject("host1", IPAddress.Parse("10.10.10.3")));
        }

        [TestMethod]
        public void ExtraMembersInTargetAreNotIncludedInTheResultTest()
        {
            var sourceMemberList = new List<FirewallObject>
            {
                new AddressObject("host1", IPAddress.Parse("10.10.10.1")),
                new AddressObject("host2", IPAddress.Parse("10.10.10.2"))
            };
            var targetMemberList = new List<FirewallObject>
            {
                new AddressObject("host1", IPAddress.Parse("10.10.10.1")),
                new AddressObject("host2", IPAddress.Parse("10.10.10.2")),
                new AddressObject("host3", IPAddress.Parse("10.10.10.3"))
            };
            var sourceGroup = new AddressGroupObject("group", sourceMemberList.Select(a => a.Name).ToList())
            {
                MemberObjects = sourceMemberList
            };
            var targetGroup = new AddressGroupObject("group", targetMemberList.Select(a => a.Name).ToList())
            {
                MemberObjects = targetMemberList
            };

            var delta = sourceGroup.GetDelta(targetGroup);
            Assert.IsNotNull(delta);
            Assert.IsTrue(delta.Count == 0);
        }
    }
}
