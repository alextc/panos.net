namespace PANOS
{
    using System;
    using System.Net;
    using System.Text;
    using System.Xml.Serialization;

    [XmlTypeAttribute(AnonymousType = true)]
    public class AddressXml 
    {
        [XmlElementAttribute("ip-range")]
        public string Range { get; set; }

        [XmlElementAttribute("ip-netmask")]
        public string Netmask { get; set; }

        [XmlElementAttribute("description")]
        public string Description { get; set; }

        [XmlAttributeAttribute("name")]
        public string Name { get; set; }

        public Tag Tag { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (string.IsNullOrEmpty(Range))
            {
                sb.AppendFormat("Name:{0}, IPNetMask:{1}", Name, Netmask);
            }
            else
            {
                sb.AppendFormat("Name:{0}, IPRange:{1}", Name, Range);
            }

            return sb.ToString();
        }

        public FirewallObject GetPayload()
        {
            if (!string.IsNullOrEmpty(Netmask) && Netmask.Contains("/"))
            {
                return new SubnetObject(
                    Name,
                    IPAddress.Parse(Netmask.Split('/')[0]),
                    UInt16.Parse(Netmask.Split('/')[1]),
                    Description);
            }

            if (!string.IsNullOrEmpty(Range))
            {
                return new AddressRangeObject(
                    Name,
                    IPAddress.Parse(Range.Split('-')[0]),
                    IPAddress.Parse(Range.Split('-')[1]),
                    Description);
            }

            return new AddressObject(Name, IPAddress.Parse(Netmask), Description);
        }
    }
}