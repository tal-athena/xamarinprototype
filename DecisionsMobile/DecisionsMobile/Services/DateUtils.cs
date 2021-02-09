using System;

namespace DecisionsMobile.Services
{
    public static class DateUtils
    {
        public static readonly string UI_DATE_FORMAT = "MM/dd/yy H:mm";
        public static string ToUiString(DateTime date)
        {
            return date.ToString(UI_DATE_FORMAT);
        }

        public static string ToUiString(DateTimeOffset offset)
        {
            return Exists(offset) ? offset.ToString(UI_DATE_FORMAT) : "";
        }

        public static bool Exists(DateTimeOffset offset)
        {
            return offset != null && offset.ToUnixTimeMilliseconds() > 0;
        }

    }
}
