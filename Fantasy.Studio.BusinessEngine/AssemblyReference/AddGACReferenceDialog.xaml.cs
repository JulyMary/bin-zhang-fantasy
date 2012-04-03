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

using System.Collections;
using Fantasy.Studio.Controls;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Fantasy.IO;
using Fantasy.Reflection;
using Microsoft.Win32;


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

        private AssemblyModel[] _selectedAssemblies = new AssemblyModel[0];
        public AssemblyModel[] SelectedAssemblies
        {
            get
            {
                return _selectedAssemblies;
            }
        }

        


        private GridViewLayoutSetting _propertyGridLayout = new GridViewLayoutSetting();

        private static ObservableCollection<AssemblyModel> _assemblies = null;


        public ObservableCollection<AssemblyModel> Assemblies
        {
            get
            {
                if (_assemblies == null)
                {
                    _assemblies = new ObservableCollection<AssemblyModel>();
                    System.Windows.Threading.Dispatcher disptacher = System.Windows.Threading.Dispatcher.CurrentDispatcher;

                    Task.Factory.StartNew(() =>
                    {

                        AssemblyLoader assemblyloader = new AssemblyLoader();
                        try
                        {
                            foreach (string dir in GlobalAssemblyCache.GetGACFolders())
                            {
                                if (LongPathDirectory.Exists(dir))
                                {
                                    foreach (string file in LongPathDirectory.EnumerateFiles(dir, "*.dll"))
                                    {
                                        
                                        AssemblyModel model = null;

                                        try
                                        {
                                            AssemblyName assemblyRef = assemblyloader.LoadFrom(file);
                                            string version = assemblyloader.GetImageRuntimeVersion(assemblyRef);
                                            model = new AssemblyModel() { AssemblyName = assemblyRef, ImageRuntimeVersion = version, Location = file };
                                            
                                        }
                                        catch
                                        {

                                        }
                                        if (model != null)
                                        {
                                            int pos = _assemblies.BinarySearchBy(model.AssemblyName.FullName, asm => asm.AssemblyName.FullName);
                                            if (pos < 0)
                                            {
                                                pos = ~pos;
                                            }
                                            disptacher.Invoke(new Action(() => _assemblies.Insert(pos, model)));

                                        }
                                    }
                                }
                            }
                        }
                        finally
                        {
                            assemblyloader.Dispose();
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
            this._selectedAssemblies = this.AssemblyList.SelectedItems.Cast<AssemblyModel>().ToArray();
        }

        private void AssemblyList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this._selectedAssemblies.Length > 0)
            {
                this.DialogResult = true;
            }
        }


    }
}
