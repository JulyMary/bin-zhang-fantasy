using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Reflection;
using System.GAC;
using System.Collections;
using Fantasy.Studio.Controls;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Fantasy.IO;

namespace Fantasy.Studio.BusinessEngine.AssemblyReference
{
    /// <summary>
    /// Interaction logic for AddGACReferenceDialog.xaml
    /// </summary>
    public partial class AddGACReferenceDialog : Window
    {
        public AddGACReferenceDialog()
        {
            InitializeComponent();
            this.Icon = Application.Current.MainWindow.Icon;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private Assembly[] _selectedAssemblies = new Assembly[0];
        public Assembly[] SelectedAssemblies
        {
            get
            {
                return _selectedAssemblies;
            }
        }

       

        private GridViewLayoutSetting _propertyGridLayout = new GridViewLayoutSetting();

        private static ObservableCollection<Assembly> _assemblies = null;

        public ObservableCollection<Assembly> Assemblies
        {
            get
            {
                if (_assemblies == null)
                {
                    _assemblies = new ObservableCollection<Assembly>();
                    System.Windows.Threading.Dispatcher disptacher = System.Windows.Threading.Dispatcher.CurrentDispatcher;
                    Task.Factory.StartNew(() => 
                    { 
                        string dir = Environment.ExpandEnvironmentVariables(@"%windir%\Microsoft.NET\assembly\GAC_MSIL");
                        foreach(string file in LongPathDirectory.EnumerateAllFiles(dir, "*.dll"))
                        {
                            Assembly assembly = Assembly.ReflectionOnlyLoadFrom(file);
                            int pos = _assemblies.BinarySearchBy(assembly.FullName, asm => asm.FullName);
                            if (pos < 0)
                            {
                                pos = ~pos;
                            }

                            disptacher.Invoke(new Action(() => _assemblies.Insert(pos, assembly)));
                            
                        }
                    });


                }
                return _assemblies;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.AddGACReferenceDialogGridViewLayout.LoadLayout(this.AssemblyGridView);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this._propertyGridLayout.SaveLayout(this.AssemblyGridView);
            Properties.Settings.Default.AddGACReferenceDialogGridViewLayout = this._propertyGridLayout; 
        }

        private void AssemblyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this._selectedAssemblies = this.AssemblyList.SelectedItems.Cast<Assembly>().ToArray();
        }
    }
}
