using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PANOSLibTest.API.Subnet
{
    using System.Net;
    using PANOS;

    // TODO: IPV6
    [TestClass]
    public class ConstructorTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), AllowDerivedTypes = true)]
        public void InvalidMaskBoundaryTest()
        {
            var address = IPAddress.Parse("10.10.255.0");
            const uint Mask = 23;
            var subnetMask = new SubnetObject("Test", address, Mask);
        }

        [TestMethod]
        public void ValidMaskBoundaryTest()
        {
            var address = IPAddress.Parse("10.10.254.0");
            const uint Mask = 23;
            var subnetMask = new SubnetObject("Test", address, Mask);
            Assert.IsNotNull(subnetMask);
        }
    }
}
