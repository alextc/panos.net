namespace PANOS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Xml.Serialization;

    [XmlTypeAttribute(AnonymousType = true)]
    public class GetAllAddressesApiResponseResult 
    {
        [XmlArray("address")]
        [XmlArrayItemAttribute("entry", IsNullable = false)]
        public AddressXml[] AddressesXml { get; set; }

        // Panos Object Names are Case-Sensitive (i.e. PA will allow you to create object namded Address1 and address1.
        // Therefore when comparing strings take case into consideration.
        [XmlIgnore]
        private Dictionary<string, AddressObject> AddressObjects => 
            AddressesXml
            .Where(a => a.Netmask != null && !a.Netmask.Contains("/"))
            .ToDictionary(a => a.Name, a => new AddressObject(a.Name, IPAddress.Parse(a.Netmask), a.Description));
            
        [XmlIgnore]
        private Dictionary<string, SubnetObject> SubnetObjects => 
            AddressesXml
            .Where(a => a.Netmask != null && a.Netmask.Contains("/"))
            .ToDictionary(a => a.Name, a => new SubnetObject(
                        a.Name, 
                        IPAddress.Parse(a.Netmask.Split('/')[0]),
                        uint.Parse(a.Netmask.Split('/')[1]),
                        a.Description));
            
        [XmlIgnore]
        private Dictionary<string, AddressRangeObject> AddressRangeObjects => 
            AddressesXml.Where(a => a.Range != null)
            .ToDictionary(a => a.Name, a => new AddressRangeObject(
                            a.Name,
                            IPAddress.Parse(a.Range.Split('-')[0]),
                            IPAddress.Parse(a.Range.Split('-')[1]),
                            a.Description));
           
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var field in AddressesXml)
            {
                sb.AppendFormat("\t{0}{1}", field, Environment.NewLine);
            }

            return sb.ToString();
        }

        public Dictionary<string, FirewallObject> GetPayload()
        {
            var combinedDictionary = new Dictionary<string, FirewallObject>();
            AddressObjects.ToList().ForEach(x => combinedDictionary.Add(x.Key, x.Value));
            SubnetObjects.ToList().ForEach(x => combinedDictionary.Add(x.Key, x.Value));
            AddressRangeObjects.ToList().ForEach(x => combinedDictionary.Add(x.Key, x.Value));
            return combinedDictionary;
        }
    }
}
