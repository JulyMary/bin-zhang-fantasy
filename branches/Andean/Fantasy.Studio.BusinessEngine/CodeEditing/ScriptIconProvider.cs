using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;
using Fantasy.BusinessEngine;
using System.ComponentModel;
using System.Windows;
using System.Drawing;
using Fantasy.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace Fantasy.Studio.BusinessEngine.CodeEditing
{
    public class ScriptIconProvider : IValueProvider, IWeakEventListener
    {

        public ScriptIconProvider()
        {

        }

        #region IValueProvider Members

        private BusinessScript _source;
        public object Source
        {
            get
            {
                return _source;
            }
            set
            {
                BusinessScript newSource = (BusinessScript)value;

                if (newSource != _source)
                {
                    if (_source != null)
                    {
                        PropertyChangedEventManager.RemoveListener(_source, this, "Name");
                    }
                    _source = newSource;
                    if (_source != null)
                    {
                        PropertyChangedEventManager.AddListener(_source, this, "Name");
                    }
                    EvalValue();
                }
            }
        }

        private void EvalValue()
        {
            if (_source == null || _source.Name == null)
            {
                this.Value = null;
            }
            else
            {
                Icon icon = IconReader.GetFileIcon(_source.Name, IconReader.IconSize.Small, false);
                BitmapSource image = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(icon.Handle, System.Windows.Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                this.Value = image;

            }
            if (this.ValueChanged != null)
            {
                this.ValueChanged(this, EventArgs.Empty);
            }
        }


        public object Value
        {
            get;
            private set;
        }

        public event EventHandler ValueChanged;

        #endregion

        #region IWeakEventListener Members

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            EvalValue();
            return true;
        }

        #endregion
    }
}
