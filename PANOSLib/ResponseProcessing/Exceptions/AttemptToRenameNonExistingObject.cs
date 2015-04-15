namespace PANOS
{
    public sealed class AttemptToRenameNonExistingObject : ResponseFailure
    {
        public AttemptToRenameNonExistingObject(string message)
            : base(message)
        {
            Data.Add(MessageFiled, message);
        }
    }
}
