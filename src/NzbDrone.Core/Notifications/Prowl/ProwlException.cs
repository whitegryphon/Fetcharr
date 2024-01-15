using System;
using NzbDrone.Common.Exceptions;

namespace NzbDrone.Core.Notifications.Fetch
{
    public class FetchException : NzbDroneException
    {
        public FetchException(string message)
            : base(message)
        {
        }

        public FetchException(string message, Exception innerException, params object[] args)
            : base(message, innerException, args)
        {
        }
    }
}
