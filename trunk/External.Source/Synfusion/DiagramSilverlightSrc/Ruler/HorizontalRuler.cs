#region Copyright Syncfusion Inc. 2001 - 2011
// Copyright Syncfusion Inc. 2001 - 2011. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Syncfusion.Windows.Diagram
{
    public class HorizontalRuler : Ruler
    {
        public HorizontalRuler()
            : base(string.Empty)
        {
            this.DefaultStyleKey = typeof(HorizontalRuler);
            this.Loaded += new RoutedEventHandler(HorizontalRuler_Loaded);
        }

        public HorizontalRuler(string name)
            : base(name)
        {
            this.DefaultStyleKey = typeof(HorizontalRuler);
            this.Loaded += new RoutedEventHandler(HorizontalRuler_Loaded);
        }

        void HorizontalRuler_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= HorizontalRuler_Loaded;
            if (this.sv != null)
            {
                this.sv.MouseMove += new MouseEventHandler(sv_MouseMove);
            }
        }

        void sv_MouseMove(object sender, MouseEventArgs e)
        {
            this.MarkerPosition = e.GetPosition(this.dv.Page).X;
        }
    }
}
