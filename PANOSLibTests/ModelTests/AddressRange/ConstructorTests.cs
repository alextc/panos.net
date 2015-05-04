namespace PANOSLibTest
{
    using System;
    using System.Net;
    using NUnit.Framework;
    using PANOS;

    [TestFixture]
    public class ConstructorTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void StartIsGreaterThanEnd()
        {
            var addressStartOfRange = IPAddress.Parse("10.10.255.5");
            var addressEndOfRange = IPAddress.Parse("10.10.255.4");
            var addressRange = new AddressRangeObject("Test", addressStartOfRange, addressEndOfRange);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void StartIsEqualToEnd()
        {
            var addressStartOfRange = IPAddress.Parse("10.10.255.5");
            var addressEndOfRange = IPAddress.Parse("10.10.255.5");
            var addressRange = new AddressRangeObject("Test", addressStartOfRange, addressEndOfRange);
        }

        [Test]
        public void ValidMaskBoundaryTest()
        {
            var addressStartOfRange = IPAddress.Parse("10.10.255.5");
            var addressEndOfRange = IPAddress.Parse("10.10.255.6");
            var addressRange = new AddressRangeObject("Test", addressStartOfRange, addressEndOfRange);
            Assert.IsNotNull(addressRange);
        }
    }
}
