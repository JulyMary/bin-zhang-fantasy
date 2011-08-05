using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Syncfusion.Windows.Shared
{
    /// <summary>
    /// Class which loads dependent assemblies in designer.
    /// </summary>
    public class LoadDependentAssemblies
    {

        /// <summary>
        /// Initializes a new instance of the LoadDependentAssemblies class.
        /// </summary>
        public LoadDependentAssemblies()
        {
            System.Windows.Controls.Button button = new System.Windows.Controls.Button();
            System.Windows.Controls.Calendar calender = new System.Windows.Controls.Calendar();
            System.Windows.Controls.DataGrid dataGrid = new DataGrid();
            button = null;
            calender = null;
            dataGrid = null;
        }
    }
}
