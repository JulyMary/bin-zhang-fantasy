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

#if WPF
namespace Syncfusion.Windows.Shared
#endif

#if SILVERLIGHT
namespace Syncfusion.Windows.Tools.Controls
#endif
{
    internal class DateTimeProperties
    {
        /// <summary>
        /// Gets or sets the start position.
        /// </summary>
        /// <value>The start position.</value>
        public int StartPosition
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the lenghth.
        /// </summary>
        /// <value>The lenghth.</value>
        public int Lenghth
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the is read only.
        /// </summary>
        /// <value>The is read only.</value>
        public bool? IsReadOnly
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public DateTimeType Type
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public string Content
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the pattern.
        /// </summary>
        /// <value>The pattern.</value>
        public string Pattern
        {
            get;
            set;
        }

        private int _KeyPressCount = 0;

        /// <summary>
        /// Gets or sets the key press count.
        /// </summary>
        /// <value>The key press count.</value>
        public int KeyPressCount
        {
            get { return _KeyPressCount; }
            set { _KeyPressCount = value; }
        }

        /// <summary>
        /// Gets or sets the name of the month.
        /// </summary>
        /// <value>The name of the month.</value>
        public string MonthName
        {
            get;
            set;
        }
    }

    /// <summary>
    /// This enum classifies DropDownViews Type.
    /// </summary>
    public enum DropDownViews
    {
        /// <summary>
        /// Specifies values representing that the object holding this
        /// value is Classic view.
        /// </summary>
        Classic,

        /// <summary>
        /// Specifies values representing that the object holding this
        /// value is Combined View.
        /// </summary>
        Combined,

        /// <summary>
        /// Specifies values representing that the object holding this
        /// value is Calendar View.
        /// </summary>
        Calendar
    }

    /// <summary>
    /// This enum classifies Default DateParts type.
    /// </summary>
    public enum DateParts
    {
        /// <summary>
        /// Specifies values representing that the object holding this
        /// value is year.
        /// </summary>
        Year = 0,

        /// <summary>
        /// Specifies values representing that the object holding this
        /// value is month.
        /// </summary>
        Month,

        /// <summary>
        /// Specifies values representing that the object holding this
        /// value is day.
        /// </summary>
        Day,

        /// <summary>
        /// Specifies values representing that the object holding this
        /// value is None.
        /// </summary>
        None
    }

    public enum DateTimeType
    {
        //f
        Fraction,
        //s
        Second,
        //m
        Minutes,
        //h
        Hour12,
        //H
        Hour24,
        //d
        Day,
        //ddd and dddd
        Dayname,//0 t0 30 || Mon || Monday
        //MM
        Month,
        //MMM or More
        monthname,
        year,
        //gg A.D or B.D
        period,
        //tt designator
        designator,
        //zz z zzzz
        TimeZone,
        others

        //timezoneoffset,// like +5.30 and -6.30
        //designator,//Am || PM
        //period,//period or era A.D and B.D
        //kind//utc || local


    }
}
