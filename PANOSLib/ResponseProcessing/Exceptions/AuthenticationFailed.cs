namespace PANOS
{
    using System;

    public sealed class AuthenticationFailed : ResponseFailure
    {
        public AuthenticationFailed(string message) : base(message)
        {
            Data.Add(MessageFiled, message);
        }
    }
}
