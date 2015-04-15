namespace PANOS
{
    public sealed class AnotherCommitInProgress : ResponseFailure
    {
        public AnotherCommitInProgress(string message)
            : base(message)
        {
            Data.Add(MessageFiled, message);
        }
    }
}
