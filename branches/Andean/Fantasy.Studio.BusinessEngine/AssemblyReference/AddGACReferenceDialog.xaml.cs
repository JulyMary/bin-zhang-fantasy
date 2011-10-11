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

        private Assembly[] _selectedAssemblies = new Assembly[0];
        public Assembly[] SelectedAssemblies
        {
            get
            {
                return _selectedAssemblies;
            }
        }

        static string ProgramFilesx86()
        {
            if (8 == IntPtr.Size
                || (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))
            {
                return Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            }

            return Environment.GetEnvironmentVariable("ProgramFiles");
        }


        private GridViewLayoutSetting _propertyGridLayout = new GridViewLayoutSetting();

        private static ObservableCollection<Assembly> _assemblies = null;
        //private static AssemblyLoader _assemblyloader = new AssemblyLoader();

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
                        List<string> dirs = new List<string>();
                        dirs.Add(Environment.ExpandEnvironmentVariables(ProgramFilesx86() + @"\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0"));
                        RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                        RegistryKey assmex = hklm.OpenSubKey(@"SOFTWARE\Microsoft\.NETFramework\v4.0.30319\AssemblyFoldersEx");
                        foreach (string subKeyName in assmex.GetSubKeyNames())
                        {
                            RegistryKey subKey = assmex.OpenSubKey(subKeyName);
                            dirs.Add((string)subKey.GetValue(string.Empty));
                        }

                        foreach (string dir in dirs)
                        {
                            if (LongPathDirectory.Exists(dir))
                            {
                                foreach (string file in LongPathDirectory.EnumerateFiles(dir, "*.dll"))
                                {
                                    Assembly assembly = null;

                                    try
                                    {
                                        assembly = Assembly.ReflectionOnlyLoadFrom(file);
                                    }
                                    catch
                                    {

                                    }
                                    if (assembly != null)
                                    {
                                        int pos = _assemblies.BinarySearchBy(assembly.FullName, asm => asm.FullName);
                                        if (pos < 0)
                                        {
                                            pos = ~pos;
                                        }
                                        disptacher.Invoke(new Action(() => _assemblies.Insert(pos, assembly)));

                                    }
                                }
                            }
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

        private void AssemblyList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this._selectedAssemblies.Length > 0)
            {
                this.DialogResult = true;
            }
        }
    }
}
