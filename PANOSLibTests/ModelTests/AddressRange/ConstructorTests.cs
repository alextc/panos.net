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
        public void StartIsGreaterThanEnd()
        {
            var addressStartOfRange = IPAddress.Parse("10.10.255.5");
            var addressEndOfRange = IPAddress.Parse("10.10.255.4");
            Assert.That(
                () => new AddressRangeObject("Test", addressStartOfRange, addressEndOfRange), 
                Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void StartIsEqualToEnd()
        {
            var addressStartOfRange = IPAddress.Parse("10.10.255.5");
            var addressEndOfRange = IPAddress.Parse("10.10.255.5");
            Assert.That(
                () => 
                    new AddressRangeObject("Test", addressStartOfRange, addressEndOfRange),
                    Throws.TypeOf<ArgumentException>());
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
