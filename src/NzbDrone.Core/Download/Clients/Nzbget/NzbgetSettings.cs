using FluentValidation;
using NzbDrone.Common.Extensions;
using NzbDrone.Core.Annotations;
using NzbDrone.Core.ThingiProvider;
using NzbDrone.Core.Validation;

namespace NzbDrone.Core.Download.Clients.Nzbget
{
    public class NzbgetSettingsValidator : AbstractValidator<NzbgetSettings>
    {
        public NzbgetSettingsValidator()
        {
            RuleFor(c => c.Host).ValidHost();
            RuleFor(c => c.Port).InclusiveBetween(1, 65535);
            RuleFor(c => c.UrlBase).ValidUrlBase().When(c => c.UrlBase.IsNotNullOrWhiteSpace());

            RuleFor(c => c.Username).NotEmpty().When(c => !string.IsNullOrWhiteSpace(c.Password));
            RuleFor(c => c.Password).NotEmpty().When(c => !string.IsNullOrWhiteSpace(c.Username));

            RuleFor(c => c.Category).NotEmpty().WithMessage("A category is recommended").AsWarning();
        }
    }

    public class NzbgetSettings : IProviderConfig
    {
        private static readonly NzbgetSettingsValidator Validator = new NzbgetSettingsValidator();

        public NzbgetSettings()
        {
            Host = "localhost";
            Port = 6789;
            Category = "Fetcharr";
            Username = "nzbget";
            Password = "tegbzn6789";
            Priority = (int)NzbgetPriority.Normal;
        }

        [FieldDefinition(0, Label = "Host", Type = FieldType.Textbox)]
        public string Host { get; set; }

        [FieldDefinition(1, Label = "Port", Type = FieldType.Textbox)]
        public int Port { get; set; }

        [FieldDefinition(2, Label = "Use SSL", Type = FieldType.Checkbox, HelpText = "Use secure connection when connecting to NZBGet")]
        public bool UseSsl { get; set; }

        [FieldDefinition(3, Label = "Url Base", Type = FieldType.Textbox, Advanced = true, HelpText = "Adds a prefix to the NZBGet url, e.g. http://[host]:[port]/[urlBase]/jsonrpc")]
        public string UrlBase { get; set; }

        [FieldDefinition(4, Label = "Username", Type = FieldType.Textbox, Privacy = PrivacyLevel.UserName)]
        public string Username { get; set; }

        [FieldDefinition(5, Label = "Password", Type = FieldType.Password, Privacy = PrivacyLevel.Password)]
        public string Password { get; set; }

        [FieldDefinition(6, Label = "Default Category", Type = FieldType.Textbox, HelpText = "Default fallback category if no mapped category exists for a release. Adding a category specific to Fetcharr avoids conflicts with unrelated downloads, but it's optional")]
        public string Category { get; set; }

        [FieldDefinition(7, Label = "Priority", Type = FieldType.Select, SelectOptions = typeof(NzbgetPriority), HelpText = "Priority for items added from Fetcharr")]
        public int Priority { get; set; }

        [FieldDefinition(8, Label = "Add Paused", Type = FieldType.Checkbox, HelpText = "This option requires at least NZBGet version 16.0")]
        public bool AddPaused { get; set; }

        public NzbDroneValidationResult Validate()
        {
            return new NzbDroneValidationResult(Validator.Validate(this));
        }
    }
}
