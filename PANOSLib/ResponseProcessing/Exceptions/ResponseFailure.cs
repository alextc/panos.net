namespace PANOS
{
    using System;

    public class ResponseFailure : Exception
    {
        public static readonly string MessageFiled = "ResponseError";
        protected ResponseFailure(string message)
            : base(message)
        {
        }
    }
}
