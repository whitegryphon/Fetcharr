using System.Net;
using Fetcharr.Http.Exceptions;

namespace Fetcharr.Http.REST
{
    public class MethodNotAllowedException : ApiException
    {
        public MethodNotAllowedException(object content = null)
            : base(HttpStatusCode.MethodNotAllowed, content)
        {
        }
    }
}
