namespace PANOSLibTest
{
    using System;
    using System.Net;
    using NUnit.Framework;
    using PANOS;

    // TODO: IPV6
    [TestFixture]
    public class SubnetConstructorTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidMaskBoundaryTest()
        {
            var address = IPAddress.Parse("10.10.255.0");
            const uint Mask = 23;
            var subnetMask = new SubnetObject("Test", address, Mask);
        }

        [Test]
        public void ValidMaskBoundaryTest()
        {
            var address = IPAddress.Parse("10.10.254.0");
            const uint Mask = 23;
            var subnetMask = new SubnetObject("Test", address, Mask);
            Assert.IsNotNull(subnetMask);
        }
    }
}
