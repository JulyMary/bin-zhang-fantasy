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

        private static List<Assembly> _assemblies = null;

        public IEnumerable<Assembly> Assemblies
        {
            get
            {
                if (_assemblies == null)
                {
                    _assemblies = new List<Assembly>();


                    IAssemblyEnum enumerator = AssemblyCache.CreateGACEnum();
                    IAssemblyName ian;

                    StringBuilder sb = new StringBuilder(1024);
                    while (enumerator.GetNextAssembly(IntPtr.Zero, out ian, 0) == 0)
                    {
                        uint len = 1024;

                        ian.GetDisplayName(sb, ref len, ASM_DISPLAY_FLAGS.VERSION | ASM_DISPLAY_FLAGS.CULTURE | ASM_DISPLAY_FLAGS.PUBLIC_KEY_TOKEN);

                        string name = sb.ToString(0, (int)len - 1);

                        try
                        {
                            Assembly assembly = Assembly.ReflectionOnlyLoad(name);
                          
                            _assemblies.Add(assembly);
                        }
                        catch
                        {
                        }
                    }

                    _assemblies.Sort((x, y) =>
                    {
                        AssemblyName xn = x.GetName();
                        AssemblyName yn = y.GetName();

                        int cp = Comparer.Default.Compare(xn.Name, yn.Name);
                        if (cp == 0)
                        {
                            foreach (int[] pair in new int[][] {new int[] {xn.Version.Major, yn.Version.Major},
                            new int[] {xn.Version.Minor, yn.Version.Minor},
                            new int[] {xn.Version.Build, yn.Version.Build},
                            new int[] {xn.Version.Revision, yn.Version.Revision}})
                            {
                                cp = Comparer.Default.Compare(pair[0], pair[1]);
                                if (cp != 0)
                                {
                                    break;
                                }
                            }

                        }

                        return cp;

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
