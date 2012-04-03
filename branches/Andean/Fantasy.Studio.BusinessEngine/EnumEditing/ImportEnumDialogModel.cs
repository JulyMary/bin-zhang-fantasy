using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Windows;
using Fantasy.Studio.Controls;
using Fantasy.BusinessEngine.Services;
using Fantasy.BusinessEngine;
using System.Reflection;
using Fantasy.IO;
using Fantasy.Reflection;

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Fantasy.Studio.BusinessEngine.EnumEditing
{
    public class ImportEnumDialogModel : NotifyPropertyChangedObject, IObjectWithSite, IDisposable
    {
        public ImportEnumDialogModel()
        {

        }

       
        private AssemblyLoader _assemblyLoader = new AssemblyLoader();

        public ImportEnumDialogModel(IServiceProvider site)
        {
           
            this.Site = site;
            

           
        }


        private ObservableCollection<AssemblyNode> _assemblies;
        public ObservableCollection<AssemblyNode> Assemblies
        {
            get
            {
                if (_assemblies == null)
                {
                    _assemblies = new ObservableCollection<AssemblyNode>();
                    IEntityService es = this.Site.GetRequiredService<IEntityService>();
                    System.Windows.Threading.Dispatcher disptacher = System.Windows.Threading.Dispatcher.CurrentDispatcher;

                    Task.Factory.StartNew(() =>
                    {

                        foreach (BusinessAssemblyReference reference in es.GetAssemblyReferenceGroup().References.OrderBy(r => r.Name))
                        {
                            AssemblyName asm = null;

                            switch (reference.Source)
                            {

                                case BusinessAssemblyReferenceSources.Local:
                                    {
                                        string file = LongPath.Combine(Fantasy.BusinessEngine.Properties.Settings.Default.FullReferencesPath, reference.Name + ".dll");
                                        if (LongPathFile.Exists(file))
                                        {
                                            asm = _assemblyLoader.LoadFrom(file);
                                        }
                                    }
                                    break;
                                case BusinessAssemblyReferenceSources.GAC:
                                    asm = _assemblyLoader.Load(reference.FullName);
                                    break;
                                case BusinessAssemblyReferenceSources.System:
                                    {
                                        string file = LongPath.Combine(Fantasy.BusinessEngine.Properties.Settings.Default.FullSystemReferencesPath, reference.Name + ".dll");
                                        if (LongPathFile.Exists(file))
                                        {
                                            asm = _assemblyLoader.LoadFrom(file);
                                        }
                                    }
                                    break;

                            }


                            disptacher.Invoke(DispatcherPriority.Background, new Action(() => { _assemblies.Add(new AssemblyNode(asm, this._assemblyLoader)); })); ;
                        }
                    });
                }
                return _assemblies;
            }
        }

        private EnumNode _selectedEnum;

        public EnumNode SelectedEnum
        {
            get { return _selectedEnum; }
            set
            {
                if (_selectedEnum != value)
                {
                    _selectedEnum = value;
                    this.OnPropertyChanged("SelectedEnum");
                }
            }
        }

        public IServiceProvider Site { get; set; }

        #region IDisposable Members

        public void Dispose()
        {
            this._assemblyLoader.Dispose();
        }

        #endregion
    }
}
