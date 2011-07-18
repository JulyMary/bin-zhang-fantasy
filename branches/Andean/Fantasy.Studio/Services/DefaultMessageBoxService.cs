using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using System.Windows;

namespace Fantasy.Studio.Services
{
    public class DefaultMessageBoxService : ServiceBase, IMessageBoxService
    {
        #region IMessageBoxService Members

        public System.Windows.MessageBoxResult Show(string messageBoxText, string caption = "", System.Windows.MessageBoxButton button = MessageBoxButton.OK, System.Windows.MessageBoxImage icon = MessageBoxImage.None, System.Windows.MessageBoxResult defaultResult = MessageBoxResult.OK)
        {
            if (caption == string.Empty)
            {
                caption = ServiceManager.Services.GetRequiredService<IWorkbench>().ApplicationTitle;
            }
            return MessageBox.Show(messageBoxText, caption, button, icon, defaultResult);
        }

        #endregion
    }
}
