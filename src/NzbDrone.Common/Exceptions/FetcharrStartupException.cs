using System;

namespace NzbDrone.Common.Exceptions
{
    public class FetcharrStartupException : NzbDroneException
    {
        public FetcharrStartupException(string message, params object[] args)
            : base("Fetcharr failed to start: " + string.Format(message, args))
        {
        }

        public FetcharrStartupException(string message)
            : base("Fetcharr failed to start: " + message)
        {
        }

        public FetcharrStartupException()
            : base("Fetcharr failed to start")
        {
        }

        public FetcharrStartupException(Exception innerException, string message, params object[] args)
            : base("Fetcharr failed to start: " + string.Format(message, args), innerException)
        {
        }

        public FetcharrStartupException(Exception innerException, string message)
            : base("Fetcharr failed to start: " + message, innerException)
        {
        }

        public FetcharrStartupException(Exception innerException)
            : base("Fetcharr failed to start: " + innerException.Message)
        {
        }
    }
}
