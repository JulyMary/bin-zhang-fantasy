using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Windows;

namespace Fantasy.Studio
{
    public class ICommandData : NotifyPropertyChangedObject
    {
        private object _value;

        public object Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    this.OnPropertyChanged("Value");
                }
            }
        }
    }


   
}
