using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using System.Reflection;
using System.IO;
using System.Windows;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;
using Fantasy.IO;
using Fantasy.Studio.BusinessEngine.Properties;
using Fantasy.Reflection;

namespace Fantasy.Studio.BusinessEngine.AssemblyReference
{
    public class AddReferenceCommand : ObjectWithSite, ICommand
    {
        #region ICommand Members



        private AssemblyLoader _loader;

        public object Execute(object args)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
           
            dlg.DefaultExt = ".dll"; // Default file extension
            dlg.Filter = Resources.AddReferenceDialogFilter; // Filter files by extension
            dlg.Title = Resources.AddReferenceDialogTitle;
            dlg.Multiselect = true;

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {

                IEntityService es = this.Site.GetRequiredService<IEntityService>();
                es.BeginUpdate();
               
                    _loader = new AssemblyLoader();

                    foreach (string filename in dlg.FileNames)
                    {

                        try
                        {
                            Assembly assembly = _loader.ReflectionOnlyLoadFrom(filename);
                            this.CreateAssemblyReference(assembly, true);
                        }
                        catch (Exception error)
                        {
                            MessageBox.Show(error.Message, Application.Current.MainWindow.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    _loader.Dispose();
                   

            }


            return null;

        }


        

        private void CreateAssemblyReference(Assembly assembly, bool throwError)
        {

            try
            {
                IEntityService es = this.Site.GetRequiredService<IEntityService>();

                string name = assembly.GetName().Name;

                BusinessAssemblyReferenceGroup group = es.GetAssemblyReferenceGroup();

                if (!group.References.Any(r => string.Equals(name, r.Name, StringComparison.OrdinalIgnoreCase) ))
                {
                    BusinessAssemblyReference reference = es.CreateEntity<BusinessAssemblyReference>();
                    reference.FullName = assembly.FullName;
                    reference.Group = group;
                    group.References.Add(reference);

                    if (!assembly.GlobalAssemblyCache)
                    {
                        reference.CopyLocal = true;
                        string filename = new Uri(assembly.CodeBase).LocalPath;
                        byte[] bytes = File.ReadAllBytes(filename);
                        reference.RawAssembly = bytes;

                        string copyPath = LongPath.Combine(Fantasy.BusinessEngine.Properties.Settings.Default.FullReferencesPath, LongPath.GetFileName(filename));
                        LongPathFile.Copy(filename, copyPath, true);
                    }
                    else
                    {
                        reference.CopyLocal = false;
                    }
                    es.SaveOrUpdate(reference);

                    if (reference.CopyLocal)
                    {

                        this.AddReferenceAssemblies(assembly);
                    }

                }
            }
            catch
            {
                if (throwError)
                {
                    throw;
                }
            }


        }

        private void AddReferenceAssemblies(Assembly assembly)
        {
            string dir = LongPath.GetDirectoryName(assembly.CodeBase);
            foreach(AssemblyName ran in assembly.GetReferencedAssemblies())
            {
                string file = LongPath.Combine(dir, ran.Name + ".dll");
                Assembly refAsm = null;
                if (LongPathFile.Exists(file))
                {
                    try
                    {
                        refAsm = this._loader.ReflectionOnlyLoadFrom(file);
                    }
                    catch
                    {
                    }
                }

                if (refAsm == null )
                {
                    try
                    {
                        refAsm = this._loader.ReflectionOnlyLoad(ran.FullName);
                    }
                    catch
                    {
                    }
                }

                if (refAsm != null && !refAsm.GlobalAssemblyCache)
                {
                    this.CreateAssemblyReference(refAsm, false); 
                }
            }
        }


       

        #endregion
    }
}
