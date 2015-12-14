namespace PANOS
{
    using System.Xml.Serialization;

    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", ElementName = "response", IsNullable = false)]
    public class ApiEnqueuedResponse : ApiResponse
    {
        [XmlElement("result")]
        public ApiEnqueuedResponseResult Result { get; set; }

        [XmlIgnore]
        public Job Job => new Job(Result.JobId, Result.Message.Line, Status);
    }
}
