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
        const string _Close = "Close";
        const string _Move = "Move";
        const string _Restore = "Restore";
        const string _Minimize = "Minimize";
        const string _Maximize = "Maximize";
        const string _Size = "Size";

        #region Predefined Alerts
        const string _Ok = "Ok";
        const string _Cancel = "Cancel";
        const string _Abort = "Abort";
        const string _Retry = "Retry";
        const string _Yes = "Yes";
        const string _No = "No";
        const string _Apply = "Apply";
#endregion
        
        const string _CalendarLabel = "CalendarLabel";
        const string _TodayLabel = "TodayLabel";
        const string _NoneLabel = "NoneLabel";

        public ResourceWrapper()
        {
            CultureInfo ci = CultureInfo.CurrentUICulture;

            close = SR.GetString(ci, _Close);
            if(SR.GetString(ci, _Move) != null)
                move = SR.GetString(ci, _Move);

            restore = SR.GetString(ci, _Restore);
            minimize = SR.GetString(ci, _Minimize);
            maximize = SR.GetString(ci, _Maximize);
            sized = SR.GetString(ci, _Size);

            ok = SR.GetString(ci, _Ok);
            cancel = SR.GetString(ci, _Cancel);
            abort = SR.GetString(ci, _Abort);
            retry = SR.GetString(ci, _Retry);
            yes = SR.GetString(ci, _Yes);
            no = SR.GetString(ci, _No);
            apply = SR.GetString(ci, _Apply);
            
            calendarLabel = SR.GetString(ci, _CalendarLabel);
            todayLabel = SR.GetString(ci, _TodayLabel);
            noneLabel = SR.GetString(ci, _NoneLabel);
        }

        string calendarLabel;
        public string CalendarLabel
        {
            get { return calendarLabel; }
            set { calendarLabel = value; }
        }

        string todayLabel;
        public string TodayLabel
        {
            get { return todayLabel; }
            set { todayLabel = value; }
        }

        string noneLabel;
        public string NoneLabel
        {
            get { return noneLabel; }
            set { noneLabel = value; }
        }

        public string Sized
        {
            get { return sized; }
            set { sized = value; }
        }
        string sized;

        public string Maximize
        {
            get { return maximize; }
            set { maximize = value; }
        }
        string maximize;

        public string Minimize
        {
            get { return minimize; }
            set { minimize = value; }
        }
        string minimize;

        public string Restore
        {
            get { return restore; }
            set { restore = value; }
        }
        string restore;

        public string Close
        {
            get { return close; }
            set { close = value; }
        }
        string close;

        public string Move
        {
            get { return move; }
            set { move = value; }
        }
        string move;

        public string Ok
        {
            get { return ok; }
            set { ok = value; }
        }
        string ok;

        public string Cancel
        {
            get { return cancel; }
            set { cancel = value; }
        }
        string cancel;

        public string Apply
        {
            get { return apply; }
            set { apply = value; }
        }
        string apply;

        public string Yes
        {
            get { return yes; }
            set { yes = value; }
        }
        string yes;

        public string No
        {
            get { return no; }
            set { no = value; }
        }
        string no;

        public string Abort
        {
            get { return abort; }
            set { abort = value; }
        }
        string abort;

        public string Retry
        {
            get { return retry; }
            set { retry = value; }
        }
        string retry;
    }
}
