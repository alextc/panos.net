namespace PANOS
{
    public class Job
    {
        public string Message { get; set; }

        public string Status { get; set; }

        public uint Id { get; set; }

        public Job(uint id, string message, string status)
        {
            Id = id;
            Message = message;
            Status = status;
        }
    }
}
