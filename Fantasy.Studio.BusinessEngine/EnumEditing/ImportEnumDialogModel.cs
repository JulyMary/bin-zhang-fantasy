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
            IEntityService es = this.Site.GetRequiredService<IEntityService>();

            List<AssemblyNode> assemblies = new List<AssemblyNode>();
            foreach (BusinessAssemblyReference reference in es.GetAssemblyReferenceGroup().References.OrderBy(r=>r.Name))
            {
                Assembly asm = null;
                if (reference.Source != BusinessAssemblyReferenceSources.GAC)
                {
                    string file = LongPath.Combine(Fantasy.BusinessEngine.Properties.Settings.Default.FullReferencesPath, reference.Name + ".dll");
                    if (LongPathFile.Exists(file))
                    {
                        asm = Assembly.ReflectionOnlyLoadFrom(file);
                    }
                }

                if (asm == null)
                {
                    asm = Assembly.Load(reference.FullName); 
                }

                assemblies.Add(new AssemblyNode(asm, this._assemblyLoader));
            }

            this.Assemblies = assemblies.ToArray();

        }

        public AssemblyNode[] Assemblies { get; private set; }

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
