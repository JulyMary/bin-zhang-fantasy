using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.AddIns
{
    public class StringValueProvider : IValueProvider
    {
        public string Value { get; set; }

        #region IValueProvider Members

        object IValueProvider.Source
        {
            get;
            set;
        }

       

        object IValueProvider.Value
        {
            get { return this.Value; }
        }

        event EventHandler IValueProvider.ValueChanged
        {
            add { }
            remove { }
        }

        #endregion
    }
}
