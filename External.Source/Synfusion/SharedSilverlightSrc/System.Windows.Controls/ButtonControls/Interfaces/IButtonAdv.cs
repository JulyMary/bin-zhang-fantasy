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

namespace Syncfusion.Windows.Tools.Controls
{
    public interface IButtonAdv
    {
        string Label
        {
            get;
            set;
        }

        ImageSource LargeIcon
        {
            get;
            set;
        }

        ImageSource SmallIcon
        {
            get;
            set;
        }

        bool IsMultiLine
        {
            get;
            set;
        }

        SizeMode SizeMode
        {
            get;
            set;
        }
    }

}
