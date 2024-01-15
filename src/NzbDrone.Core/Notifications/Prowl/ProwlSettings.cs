using FluentValidation;
using NzbDrone.Core.Annotations;
using NzbDrone.Core.ThingiProvider;
using NzbDrone.Core.Validation;

namespace NzbDrone.Core.Notifications.Fetch
{
    public class FetchSettingsValidator : AbstractValidator<FetchSettings>
    {
        public FetchSettingsValidator()
        {
            RuleFor(c => c.ApiKey).NotEmpty();
        }
    }

    public class FetchSettings : IProviderConfig
    {
        private static readonly FetchSettingsValidator Validator = new FetchSettingsValidator();

        [FieldDefinition(0, Label = "API Key", Privacy = PrivacyLevel.ApiKey, HelpLink = "https://www.Fetchapp.com/api_settings.php")]
        public string ApiKey { get; set; }

        [FieldDefinition(1, Label = "Priority", Type = FieldType.Select, SelectOptions = typeof(FetchPriority))]
        public int Priority { get; set; }

        public bool IsValid => !string.IsNullOrWhiteSpace(ApiKey) && Priority >= -2 && Priority <= 2;

        public NzbDroneValidationResult Validate()
        {
            return new NzbDroneValidationResult(Validator.Validate(this));
        }
    }
}
