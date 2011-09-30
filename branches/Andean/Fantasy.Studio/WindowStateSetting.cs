using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Fantasy.Windows;

namespace Fantasy.Studio
{
    public class WindowStateSetting : NotifyPropertyChangedObject
    {

        public WindowStateSetting()
        {
         
            
        }


        public System.Drawing.Rectangle GetBounds()
        {
            return new System.Drawing.Rectangle((int)Left, (int)Top, (int)Width, (int)Height);  
        }

        private WindowState _windowState = WindowState.Normal;

        public WindowState WindowState
        {
            get { return _windowState; }
            set
            {
                if (_windowState != value)
                {
                    _windowState = value;
                    this.OnPropertyChanged("WindowState");
                }
            }
        }

        private double _width = 800;

        public double Width
        {
            get { return _width; }
            set
            {
                if (_width != value)
                {
                    _width = value;
                    this.OnPropertyChanged("Width");
                }
            }
        }


        private double _height = 600;

        public double Height
        {
            get { return _height; }
            set
            {
                if (_height != value)
                {
                    _height = value;
                    this.OnPropertyChanged("Height");
                }
            }
        }

        private double _left;

        public double Left
        {
            get { return _left; }
            set
            {
                if (_left != value)
                {
                    _left = value;
                    this.OnPropertyChanged("Left");
                }
            }
        }

        private double _top;

        public double Top
        {
            get { return _top; }
            set
            {
                if (_top != value)
                {
                    _top = value;
                    this.OnPropertyChanged("Top");
                }
            }
        }


    }
}
