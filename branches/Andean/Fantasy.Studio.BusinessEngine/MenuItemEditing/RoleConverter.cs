using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Fantasy.BusinessEngine.Services;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.MenuItemEditing
{
    public class RoleConverter : IMultiValueConverter
    {

        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            Guid roleId = (Guid)values[0];
            MenuEditorModel model = (MenuEditorModel)values[1];
            IEntityService es = model.Site.GetRequiredService<IEntityService>();
            BusinessRoleData role = es.Get<BusinessRoleData>(roleId);

            switch ((string)parameter)
            {
                case "Name":
                    return role.Name;
                case "Description":
                    return role.Description;
                default:
                    throw new NotSupportedException();
            }

            
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
