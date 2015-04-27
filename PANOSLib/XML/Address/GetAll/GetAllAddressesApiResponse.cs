namespace PANOS
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml.Serialization;

    [XmlType(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", ElementName = "response", IsNullable = false)]
    public class GetAllAddressesApiResponse : ApiResponseForGetAll
    {
        [XmlElement("result")]
        public GetAllAddressesApiResponseResult Result { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Status:{0}{1}Response:{2}{3}", Status, Environment.NewLine, Environment.NewLine, Result);
            return sb.ToString();
        }

        public override Dictionary<string, FirewallObject> GetPayload()
        {
            return Result.GetPayload();
        }
    }
}