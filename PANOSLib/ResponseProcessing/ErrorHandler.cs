namespace PANOS
{
    using System;

    public static class ErrorHandler
    {
        public static Exception GenerateException(string response)
        {
            if (response.Contains("response status = 'error' code = '403"))
            {
                return new AuthenticationFailed(response);
            }

            if (response.Contains("No such node"))
            {
                return new ObjectNotFound(response);
            }

            if (response.Contains("Can rename only one obj at a time"))
            {
                return  new AttemptToRenameNonExistingObject(response);
            }

            if (response.Contains("Another commit/validate is in progress."))
            {
                return new AnotherCommitInProgress(response);
            }
            
            // TODO: Handle invalid/unreachable host name

            return new UnknownResponse(response);
        }
    }
}
