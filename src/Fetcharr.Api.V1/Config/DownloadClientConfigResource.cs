using NzbDrone.Core.Configuration;
using Fetcharr.Http.REST;

namespace Fetcharr.Api.V1.Config
{
    public class DownloadClientConfigResource : RestResource
    {
    }

    public static class DownloadClientConfigResourceMapper
    {
        public static DownloadClientConfigResource ToResource(IConfigService model)
        {
            return new DownloadClientConfigResource
            {
            };
        }
    }
}
