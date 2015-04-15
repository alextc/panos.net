namespace PANOS
{
    public sealed class UnknownResponse : ResponseFailure
    {
        public UnknownResponse(string message)
            : base(message)
        {
            Data.Add(MessageFiled, message);
        }
    }
}
