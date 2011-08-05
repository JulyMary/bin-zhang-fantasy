using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Globalization;

namespace Syncfusion.Windows.Shared.Resources
{
    public class ResourceWrapper
    {
        const string AccessCalendarTextName = "AccessCalendarText";
        const string AccessWatchTextName = "AccessWatchText";
        const string AccessEmptyDateTextName = "AccessEmptyDateText";
        const string AccessTodayTextName = "AccessTodayText";
        const string MinimizeTooltipName = "MinimizeTooltip";
        const string MaximizeTooltipName = "MaximizeTooltip";
        const string CloseTooltipName = "CloseTooltip";
        const string RestoreTooltipName = "RestoreTooltip";
        const string TodayLabelName = "TodayLabel";
        const string AccessClockTextName = "AccessClockText";


        public ResourceWrapper()
        {
            CultureInfo ci = CultureInfo.CurrentUICulture;

            AccessCalendarText = SR.GetString(ci, AccessCalendarTextName);
            AccessWatchText = SR.GetString(ci, AccessWatchTextName);
            AccessEmptyDateText = SR.GetString(ci, AccessEmptyDateTextName);
            AccessTodayText = SR.GetString(ci, AccessTodayTextName);
            MinimizeTooltip = SR.GetString(ci, MinimizeTooltipName);
            MaximizeTooltip = SR.GetString(ci, MaximizeTooltipName);
            CloseTooltip = SR.GetString(ci, CloseTooltipName);
            RestoreTooltip = SR.GetString(ci, RestoreTooltipName);
            TodayLabel = SR.GetString(ci, TodayLabelName);
            AccessClockText = SR.GetString(ci, AccessClockTextName);
        }

        public string  AccessCalendarText { get; set; }

        public string AccessWatchText { get; set; }

        public string AccessEmptyDateText { get; set; }

        public string AccessTodayText { get; set; }

        public string MinimizeTooltip { get; set; }

        public string MaximizeTooltip { get; set; }

        public string CloseTooltip { get; set; }

        public string RestoreTooltip { get; set; }

        public string TodayLabel { get; set; }

        public string AccessClockText { get; set; }

    }
}
