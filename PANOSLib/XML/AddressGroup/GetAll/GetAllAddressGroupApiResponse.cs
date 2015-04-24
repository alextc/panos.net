namespace PANOS
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;

    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", ElementName = "response", IsNullable = false)]
    public class GetAllAddressGroupApiResponse : ApiResponse, IDictionaryPayload
    {
        [XmlElement("result")]
        public GetAllAddressGroupApiResponseResult Result { get; set; }

        public Dictionary<string, FirewallObject> GetPayload()
        {
            var resultDictionary = new Dictionary<string, FirewallObject>();
            Result.AddressGroupObjects.ToList().ForEach(x => resultDictionary.Add(x.Key, x.Value));
            return resultDictionary;
        }
    }
}





