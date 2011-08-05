// <copyright file="MonthButton.cs" company="Syncfusion">
// Copyright Syncfusion Inc. 2001 - 2009. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Calendar = System.Globalization.Calendar;

namespace Syncfusion.Windows.Shared
{
    /// <summary>
    /// Represents month name header.
    /// </summary>
#if SyncfusionFramework4_0
    [System.ComponentModel.DesignTimeVisible(false)]
#endif
    public class MonthButton : ContentControl
    {
        #region Initialization

        /// <summary>
        /// Initializes static members of the <see cref="MonthButton"/> class.  It overrides some dependency properties.
        /// </summary>
        static MonthButton()
        {
            // This OverrideMetadata call tells the system that this element wants to provide a style that is different than its base class.
            // This style is defined in themes\generic.xaml
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MonthButton), new FrameworkPropertyMetadata(typeof(MonthButton)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonthButton"/> class.
        /// </summary>
        public MonthButton()
        {
        }

        #endregion

        #region Implementation
        /// <summary>
        /// Initializes MonthButton instance.
        /// </summary>
        /// <param name="data">Current date.</param>
        /// <param name="calendar">Current calendar.</param>
        /// <param name="format">The <see cref="System.Globalization.DateTimeFormatInfo"/> object that belongs to the current culture.</param>
        /// <param name="isAbbreviated">Indicates whether month name should be abbreviated.</param>
        /// <param name="mode">Visual mode of the <see cref="Syncfusion.Windows.Shared.CalendarEdit"/> control.</param>
        protected internal void Initialize(VisibleDate data, Calendar calendar, DateTimeFormatInfo format, bool isAbbreviated, CalendarVisualMode mode)
        {
            int imonth = data.VisibleMonth;
            int iyear = data.VisibleYear;
            Date minDate = new Date(calendar.MinSupportedDateTime, calendar);
            Date maxDate = new Date(calendar.MaxSupportedDateTime, calendar);

            if (mode == CalendarVisualMode.WeekNumbers)
            {
                Content = "Weeks in " + iyear.ToString();
            }  

            if (mode == CalendarVisualMode.Days)
            {
                if (isAbbreviated)
                {
                    Content = format.AbbreviatedMonthNames[imonth - 1] + " " + iyear.ToString();
                }
                else
                {
                    Content = format.MonthNames[imonth - 1] + " " + iyear.ToString();
                }
            }

            if (mode == CalendarVisualMode.Months)
            {
                Content = iyear.ToString();
            }

            if (mode == CalendarVisualMode.Years)
            {
                int startYear = data.VisibleYear;
                int endYear;

                while (startYear % 10 != 0)
                {
                    startYear--;
                }

                endYear = startYear + 9;

                if (startYear < minDate.Year)
                {
                    startYear = minDate.Year;
                }

                if (endYear > maxDate.Year)
                {
                    endYear = maxDate.Year;
                }

                Content = startYear + "-" + endYear;
            }

            if (mode == CalendarVisualMode.YearsRange)
            {
                int startYear = data.VisibleYear;
                int endYear;

                while (startYear % 10 != 0)
                {
                    startYear--;
                }

                while (startYear % 100 != 0)
                {
                    startYear -= 10;
                }

                endYear = startYear + 99;
                //// TODO Modify algorithm here !
                if (startYear < minDate.Year)
                {
                    startYear = minDate.Year;
                }

                if (endYear > maxDate.Year)
                {
                    endYear = maxDate.Year;
                }

                Content = startYear + "-" + endYear;
            }
        }
        #endregion
    }
}
