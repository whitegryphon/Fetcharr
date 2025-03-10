using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NzbDrone.Common.Serializer;
using Fetcharr.Http.Exceptions;

namespace Fetcharr.Http.ErrorManagement
{
    public class ErrorModel
    {
        public string Message { get; set; }
        public string Description { get; set; }
        public object Content { get; set; }

        public ErrorModel(ApiException exception)
        {
            Message = exception.Message;
            Content = exception.Content;
        }

        public ErrorModel()
        {
        }

        public Task WriteToResponse(HttpResponse response, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            response.StatusCode = (int)statusCode;
            response.ContentType = "application/json";
            return STJson.SerializeAsync(this, response.Body);
        }
    }
}
