using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Fantasy.Studio.Services
{
    public interface IMessageBoxService
    {
        MessageBoxResult Show(string messageBoxText, string caption = "", 
            MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.None,
            MessageBoxResult defaultResult = MessageBoxResult.OK);

    }
}
