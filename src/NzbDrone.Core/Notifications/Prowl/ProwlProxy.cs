using System;
using System.Net;
using FluentValidation.Results;
using NLog;
using NzbDrone.Common.EnvironmentInfo;
using NzbDrone.Common.Http;

namespace NzbDrone.Core.Notifications.Fetch
{
    public interface IFetchProxy
    {
        void SendNotification(string title, string message, FetchSettings settings);
        ValidationFailure Test(FetchSettings settings);
    }

    public class FetchProxy : IFetchProxy
    {
        private const string PUSH_URL = "https://api.Fetchapp.com/publicapi/add";
        private readonly IHttpClient _httpClient;
        private readonly Logger _logger;

        public FetchProxy(IHttpClient httpClient, Logger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public void SendNotification(string title, string message, FetchSettings settings)
        {
            try
            {
                var requestBuilder = new HttpRequestBuilder(PUSH_URL);

                var request = requestBuilder.Post()
                    .AddFormParameter("apikey", settings.ApiKey)
                    .AddFormParameter("application", BuildInfo.AppName)
                    .AddFormParameter("event", title)
                    .AddFormParameter("description", message)
                    .AddFormParameter("priority", settings.Priority)
                    .Build();

                _httpClient.Post(request);
            }
            catch (HttpException ex)
            {
                if (ex.Response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _logger.Error(ex, "Apikey is invalid: {0}", settings.ApiKey);
                    throw new FetchException("Apikey is invalid", ex);
                }

                throw new FetchException("Unable to send text message: " + ex.Message, ex);
            }
            catch (WebException ex)
            {
                throw new FetchException("Failed to connect to Fetch, please check your settings.", ex);
            }
        }

        public ValidationFailure Test(FetchSettings settings)
        {
            try
            {
                const string title = "Test Notification";
                const string body = "This is a test message from Fetcharr";

                SendNotification(title, body, settings);
            }
            catch (Exception ex)
            {
                return new ValidationFailure("ApiKey", ex.Message);
            }

            return null;
        }
    }
}
