using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Windows;
using System.Windows.Media;
using Fantasy.Studio.BusinessEngine.Properties;

namespace Fantasy.Studio.BusinessEngine.CodeEditing
{
    public class FontAndColorsOptionPanelModel : NotifyPropertyChangedObject
    {
        public FontAndColorsOptionPanelModel()
        {
            this.FontFamilies = System.Drawing.FontFamily.Families.Select(f => new FontFamily(f.Name)).ToArray();

            List<double> sizes = new List<double>();
            for (int i = 6; i <= 24; i++)
            {
                sizes.Add(i);
            }
            this.Sizes = sizes.ToArray();

            this._fontFamily = new FontFamily(Settings.Default.CodeEditorFontFamily);
            this._size = Settings.Default.CodeEditorFontSize; 
        }


        private FontFamily _fontFamily;

        public FontFamily FontFamily
        {
            get { return _fontFamily; }
            set
            {
                if (_fontFamily != value)
                {
                    _fontFamily = value;
                    this.OnPropertyChanged("FontFamily");
                }
            }
        }

        public FontFamily[] FontFamilies { get; private set; }

        private double _size;

        public double Size
        {
            get { return _size; }
            set
            {
                if (_size != value)
                {
                    _size = value;
                    this.OnPropertyChanged("Size");
                }
            }
        }

        public double[] Sizes { get; private set; }


       
       
    }
}
