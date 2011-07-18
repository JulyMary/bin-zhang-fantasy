using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Fantasy.Studio.BusinessEngine
{
    public static class BusinessEntityCommands
    {
        public static readonly RoutedCommand AddBusinessPropertyCommand = new RoutedCommand("AddBusinessProperty", typeof(BusinessEntityCommands));
    }
}
