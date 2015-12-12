namespace PANOSLibTest.SchemaValidation
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;
    using NUnit.Framework;

    [TestFixture]
    public class SchemaValidationTests
    {
        [Test]
        public void AllAddressesResponseXsdEmbededInAssembly()
        {
            var schemas = CreateSchemaSetFromXSDResource("PANOS.XML.Schema.AllAddressesResponse.xsd");
            Assert.IsNotNull(schemas);
        }

        [Test]
        public void ProperAllAddressResponseValidationDoesNotThrowException()
        {
            var allAddressesResponseSchemaSet = CreateSchemaSetFromXSDResource("PANOS.XML.Schema.AllAddressesResponse.xsd");
            var allAddressesResponseDocument = LoadXMLDocument("AllAddressesResponse.xml");
            allAddressesResponseDocument.Validate(allAddressesResponseSchemaSet, null);
        }

        [Test]
        public void NonAllAddressResponseValidationThrowsException()
        {
            var allAddressesResponseSchemaSet = CreateSchemaSetFromXSDResource("PANOS.XML.Schema.AllAddressesResponse.xsd");
            var allAddressesResponseDocument = LoadXMLDocument("SingleAddressResponse.xml");
            Assert.That(
                () => allAddressesResponseDocument.Validate(allAddressesResponseSchemaSet, null),
                Throws.TypeOf<XmlSchemaValidationException>());
        }

        private XDocument LoadXMLDocument(string name)
        {
            Assert.IsTrue(File.Exists(name));
            return XDocument.Load(name);
        }

        private XmlSchemaSet CreateSchemaSetFromXSDResource(string xsdResource)
        {
            var currentDomain = AppDomain.CurrentDomain;
            var panosAssembly = currentDomain.Load("PANOS");
            Assert.IsNotNull(panosAssembly);
            Assert.IsTrue(panosAssembly.GetManifestResourceNames().ToList().Contains(xsdResource));
            var stream = panosAssembly.GetManifestResourceStream(xsdResource);
            Assert.IsNotNull(stream);

            var schemas = new XmlSchemaSet();
            schemas.Add(string.Empty, XmlReader.Create(stream));
            return schemas;
        }
    }
}
