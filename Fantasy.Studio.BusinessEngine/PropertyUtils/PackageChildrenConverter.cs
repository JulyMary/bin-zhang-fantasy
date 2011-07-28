using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine
{
    class PackageChildrenConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return this.GetChildren((BusinessPackage)value);
        }

        private IEnumerable<IBusinessEntity>  GetChildren(BusinessPackage package)
        {
            if (package != null)
            {
                foreach (BusinessPackage child in package.ChildPackages)
                {
                    yield return child;
                }

                foreach (BusinessClass cls in package.Classes)
                {
                    yield return cls;
                }
            }
            else
            {
                yield break;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
