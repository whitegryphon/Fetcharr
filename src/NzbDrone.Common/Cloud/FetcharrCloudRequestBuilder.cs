using NzbDrone.Common.Http;

namespace NzbDrone.Common.Cloud
{
    public interface IFetcharrCloudRequestBuilder
    {
        IHttpRequestBuilderFactory Services { get; }
        IHttpRequestBuilderFactory Releases { get; }
    }

    public class FetcharrCloudRequestBuilder : IFetcharrCloudRequestBuilder
    {
        public FetcharrCloudRequestBuilder()
        {
            Services = new HttpRequestBuilder("https://fetcharr.servarr.com/v1/")
                .CreateFactory();

            Releases = new HttpRequestBuilder("https://releases.servarr.com/v1/")
                .CreateFactory();
        }

        public IHttpRequestBuilderFactory Services { get; private set; }

        public IHttpRequestBuilderFactory Releases { get; private set; }
    }
}
