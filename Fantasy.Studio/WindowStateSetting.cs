using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Fantasy.Studio
{
    public class WindowStateSetting
    {

        public WindowStateSetting()
        {
            WindowState = System.Windows.WindowState.Normal;
            Width = 800;
            Height = 600;
            
        }

        public WindowState WindowState { get; set; }

        private double _width;

        public double Width
        {
            get { return _width; }
            set { _width = value; }
        }


        private double _height;

        public double Height
        {
            get { return _height; }
            set { _height = value; }
        }

    }
}
