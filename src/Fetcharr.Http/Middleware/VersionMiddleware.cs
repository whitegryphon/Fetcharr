using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NzbDrone.Common.EnvironmentInfo;
using Fetcharr.Http.Extensions;

namespace Fetcharr.Http.Middleware
{
    public class VersionMiddleware
    {
        private const string VERSIONHEADER = "X-Application-Version";

        private readonly RequestDelegate _next;
        private readonly string _version;

        public VersionMiddleware(RequestDelegate next)
        {
            _next = next;
            _version = BuildInfo.Version.ToString();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.IsApiRequest() && !context.Response.Headers.ContainsKey(VERSIONHEADER))
            {
                context.Response.Headers.Add(VERSIONHEADER, _version);
            }

            await _next(context);
        }
    }
}
