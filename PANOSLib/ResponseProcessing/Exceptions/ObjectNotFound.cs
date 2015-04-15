namespace PANOS
{
    public sealed class ObjectNotFound : ResponseFailure
    {
        public ObjectNotFound(string message)
            : base(message)
        {
            Data.Add(MessageFiled, message);
        }
    }
}
