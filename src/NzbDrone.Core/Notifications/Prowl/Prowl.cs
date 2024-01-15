using System.Collections.Generic;
using FluentValidation.Results;
using NzbDrone.Common.Extensions;

namespace NzbDrone.Core.Notifications.Fetch
{
    public class Fetch : NotificationBase<FetchSettings>
    {
        private readonly IFetchProxy _FetchProxy;

        public Fetch(IFetchProxy FetchProxy)
        {
            _FetchProxy = FetchProxy;
        }

        public override string Link => "https://www.Fetchapp.com/";
        public override string Name => "Fetch";

        public override void OnGrab(GrabMessage message)
        {
            _FetchProxy.SendNotification(RELEASE_GRABBED_TITLE, message.Message, Settings);
        }

        public override void OnHealthIssue(HealthCheck.HealthCheck healthCheck)
        {
            _FetchProxy.SendNotification(HEALTH_ISSUE_TITLE, healthCheck.Message, Settings);
        }

        public override void OnHealthRestored(HealthCheck.HealthCheck previousMessage)
        {
            _FetchProxy.SendNotification(HEALTH_RESTORED_TITLE, $"The following issue is now resolved: {previousMessage.Message}", Settings);
        }

        public override void OnApplicationUpdate(ApplicationUpdateMessage updateMessage)
        {
            _FetchProxy.SendNotification(APPLICATION_UPDATE_TITLE, updateMessage.Message, Settings);
        }

        public override ValidationResult Test()
        {
            var failures = new List<ValidationFailure>();

            failures.AddIfNotNull(_FetchProxy.Test(Settings));

            return new ValidationResult(failures);
        }
    }
}
