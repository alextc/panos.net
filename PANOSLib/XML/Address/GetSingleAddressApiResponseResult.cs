namespace PANOS
{
    using System.Xml.Serialization;

    [XmlType(AnonymousType = true)]
    public class GetSingleAddressApiResponseResult
    {
        [XmlElement("entry")]
        public AddressXml AddressXml { get; set; }
    }
}
