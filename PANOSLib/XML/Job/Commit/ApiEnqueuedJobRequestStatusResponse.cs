namespace PANOS
{
    using System;
    using System.Xml.Serialization;

    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", ElementName = "response", IsNullable = false)]
    public class ApiEnqueuedJobRequestStatusResponse : ApiResponse
    {
        [XmlElement("result")]
        public ApiEnqueuedLogRequestStatusResponseResult Result { get; set; }
    }

    [XmlTypeAttribute(AnonymousType = true)]
    public class ApiEnqueuedLogRequestStatusResponseResult
    {
        [XmlElement("job")]
        public JobStatus Job { get; set; }
    }

    [XmlTypeAttribute(AnonymousType = true)]
    public class JobStatus
    {
        [XmlElement(DataType = "time", ElementName = "tenq")]
        public string TimeEnqueued { get; set; }

        [XmlElement("id")]
        public byte Id { get; set; }

        [XmlElement("type")]
        public string JobType { get; set; }

        [XmlElement("status")]
        public string Status { get; set; }

        [XmlElement("stoppable")]
        public string Stoppable { get; set; }

        [XmlElement("result")]
        public string Result { get; set; }

        [XmlElement(DataType = "time", ElementName = "tfin")]
        public DateTime TimeCompleted { get; set; }

        [XmlElementAttribute("progress")]
        public DateTime Progress { get; set; }

        [XmlElement("details")]
        public JobStatusDetails Details { get; set; }

        [XmlElement("warning")]
        public string Warnings { get; set; }
    }

    [XmlTypeAttribute(AnonymousType = true)]
    public class JobStatusDetails
    {
        [XmlElement("line")]
        public string Line { get; set; }
    }

}


