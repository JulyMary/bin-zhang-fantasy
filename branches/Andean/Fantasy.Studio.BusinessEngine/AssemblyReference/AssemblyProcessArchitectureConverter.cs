using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Reflection;

namespace Fantasy.Studio.BusinessEngine.AssemblyReference
{
    public class AssemblyProcessArchitectureConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Assembly assembly = (Assembly)value;
            ProcessorArchitecture arch = assembly.GetName().ProcessorArchitecture;
            switch (arch)
            {
                case ProcessorArchitecture.Amd64:
                    return "x64";
                case ProcessorArchitecture.IA64:
                    return "Itanium";
                case ProcessorArchitecture.MSIL:
                    return "Any CPU";
                default:
                    return "Unknown";
                  
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
