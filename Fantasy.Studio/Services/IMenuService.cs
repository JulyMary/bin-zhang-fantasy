using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Fantasy.Studio.Services
{
    public interface IMenuService
    {
        UIElement CreateMainMenu(string path, object owner, IServiceProvider services);
        ContextMenu CreateContextMenu(string path, object owner, IServiceProvider services);
    }
}
