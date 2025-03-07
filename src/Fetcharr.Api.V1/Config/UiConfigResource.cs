using NzbDrone.Core.Configuration;
using Fetcharr.Http.REST;

namespace Fetcharr.Api.V1.Config
{
    public class UiConfigResource : RestResource
    {
        //Calendar
        public int FirstDayOfWeek { get; set; }
        public string CalendarWeekColumnHeader { get; set; }

        //Dates
        public string ShortDateFormat { get; set; }
        public string LongDateFormat { get; set; }
        public string TimeFormat { get; set; }
        public bool ShowRelativeDates { get; set; }

        public bool EnableColorImpairedMode { get; set; }
        public string UILanguage { get; set; }
        public string Theme { get; set; }
    }

    public static class UiConfigResourceMapper
    {
        public static UiConfigResource ToResource(IConfigFileProvider config, IConfigService model)
        {
            return new UiConfigResource
            {
                FirstDayOfWeek = model.FirstDayOfWeek,
                CalendarWeekColumnHeader = model.CalendarWeekColumnHeader,

                ShortDateFormat = model.ShortDateFormat,
                LongDateFormat = model.LongDateFormat,
                TimeFormat = model.TimeFormat,
                ShowRelativeDates = model.ShowRelativeDates,

                EnableColorImpairedMode = model.EnableColorImpairedMode,
                UILanguage = model.UILanguage,
                Theme = config.Theme
            };
        }
    }
}
