using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Fantasy.BusinessEngine.Services;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.MenuItemEditing
{
    public class ApplicationConverter : IMultiValueConverter
    {

        #region IValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Guid appId = (Guid)values[0];
            MenuEditorModel model = (MenuEditorModel)values[1];
            IEntityService es = model.Site.GetRequiredService<IEntityService>();
            BusinessApplicationData app = es.Get<BusinessApplicationData>(appId);


            switch ((string)parameter)
            {
                case "Name":
                    return app.Name;
                case "Package":
                    return app.Package.FullName; 
                default:
                    throw new NotSupportedException();
            }

           
        }

       

        #endregion

        #region IMultiValueConverter Members


        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }



}
