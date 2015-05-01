using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PANOSLibTest.API.AddressRange
{
    using System.Net;
    using PANOS;

    [TestClass]
    public class ConstructorTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), AllowDerivedTypes = true)]
        public void StartIsGreaterThanEnd()
        {
            var addressStartOfRange = IPAddress.Parse("10.10.255.5");
            var addressEndOfRange = IPAddress.Parse("10.10.255.4");
            var addressRange = new AddressRangeObject("Test", addressStartOfRange, addressEndOfRange);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), AllowDerivedTypes = true)]
        public void StartIsEqualToEnd()
        {
            var addressStartOfRange = IPAddress.Parse("10.10.255.5");
            var addressEndOfRange = IPAddress.Parse("10.10.255.5");
            var addressRange = new AddressRangeObject("Test", addressStartOfRange, addressEndOfRange);
        }

        [TestMethod]
        public void ValidMaskBoundaryTest()
        {
            var addressStartOfRange = IPAddress.Parse("10.10.255.5");
            var addressEndOfRange = IPAddress.Parse("10.10.255.6");
            var addressRange = new AddressRangeObject("Test", addressStartOfRange, addressEndOfRange);
            Assert.IsNotNull(addressRange);
        }
    }
}
