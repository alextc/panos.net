namespace PANOS
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;

    [XmlType(AnonymousType = true)]
    public class GetAllAddressGroupApiResponseResult
    {
        [XmlArray("address-group")]
        [XmlArrayItemAttribute("entry", IsNullable = false)]
        public AddressGroupXml[] AddressGroupXml { get; set; }

        [XmlIgnore]
        public Dictionary<string, AddressGroupObject> AddressGroupObjects
        {
            get
            {
                return AddressGroupXml.
                    ToDictionary(
                        addressGroup => addressGroup.Name,
                        addressGroup => new AddressGroupObject(addressGroup.Name, new List<string>(addressGroup.Members)));
            }
        }
    }
}
