namespace PANOS
{
    using System.Xml.Serialization;

    [XmlType(AnonymousType = true)]
    public class GetSingleAddressGroupApiResponseResult
    {
        [XmlElement("entry")]
        public AddressGroupXml AddressGroupXml { get; set; }
    }
}
