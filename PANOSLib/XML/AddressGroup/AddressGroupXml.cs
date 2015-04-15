namespace PANOS
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlType(AnonymousType = true)]
    public class AddressGroupXml
    {
        [XmlArray("static")]
        [XmlArrayItem("member", IsNullable = false)]
        public string[] Members { get; set; }

        [XmlAttributeAttribute("name")]
        public string Name { get; set; }

        public AddressGroupObject AddressGroupObject
        {
            get
            {
                return new AddressGroupObject(Name, new List<string>(Members));
            }
        }
    }
}
