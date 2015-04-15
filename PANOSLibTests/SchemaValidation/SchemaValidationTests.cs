namespace PANOSLibTest.SchemaValidation
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SchemaValidationTests
    {
        [TestMethod]
        public void AllAddressesResponseXsdEmbededInAssembly()
        {
            var schemas = CreateSchemaSetFromXSDResource("PANOS.XML.Schema.AllAddressesResponse.xsd");
            Assert.IsNotNull(schemas);
        }

        [TestMethod]
        public void ProperAllAddressResponseValidationDoesNotThrowException()
        {
            var allAddressesResponseSchemaSet = CreateSchemaSetFromXSDResource("PANOS.XML.Schema.AllAddressesResponse.xsd");
            var allAddressesResponseDocument = LoadXMLDocument("AllAddressesResponse.xml");
            allAddressesResponseDocument.Validate(allAddressesResponseSchemaSet, null);
        }

        [TestMethod]
        [ExpectedException(typeof(XmlSchemaValidationException))]
        public void NonAllAddressResponseValidationThrowsException()
        {
            var allAddressesResponseSchemaSet = CreateSchemaSetFromXSDResource("PANOS.XML.Schema.AllAddressesResponse.xsd");
            var allAddressesResponseDocument = LoadXMLDocument("SingleAddressResponse.xml");
            allAddressesResponseDocument.Validate(allAddressesResponseSchemaSet, null);
        }

        private XDocument LoadXMLDocument(string name)
        {
            Assert.IsTrue(File.Exists(name));
            return XDocument.Load(name);
        }

        private XmlSchemaSet CreateSchemaSetFromXSDResource(string xsdResource)
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            var panosAssembly = currentDomain.Load("PANOS");
            Assert.IsNotNull(panosAssembly);
            Assert.IsTrue(panosAssembly.GetManifestResourceNames().ToList().Contains(xsdResource));
            Stream stream = panosAssembly.GetManifestResourceStream(xsdResource);
            Assert.IsNotNull(stream);

            var schemas = new XmlSchemaSet();
            schemas.Add(string.Empty, XmlReader.Create(stream));
            return schemas;
        }
    }
}
