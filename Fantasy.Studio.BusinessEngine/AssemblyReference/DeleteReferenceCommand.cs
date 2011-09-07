using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.BusinessEngine.Services;
using Fantasy.BusinessEngine;
using Fantasy.IO;

namespace Fantasy.Studio.BusinessEngine.AssemblyReference
{
    public class DeleteReferenceCommand : ObjectWithSite, ICommand
    {
        #region ICommand Members

        public object Execute(object args)
        {
            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            BusinessAssemblyReference reference = (BusinessAssemblyReference)args;
            es.DefaultSession.BeginUpdate();
            try
            {
                reference.Group.References.Remove(reference);
                reference.Group = null;

                if (reference.EntityState != EntityState.New && reference.EntityState != EntityState.Deleted)
                {
                    es.DefaultSession.Delete(reference);
                }


                if (reference.CopyLocal)
                {
                    string fileName = LongPath.Combine(Fantasy.BusinessEngine.Properties.Settings.Default.FullReferencesPath, reference.Name + ".dll");
                   
                    if (LongPathFile.Exists(fileName))
                    {
                        LongPathFile.Delete(fileName);
                    }
                }


                es.DefaultSession.EndUpdate(true);
               
            }
            catch
            {
                es.DefaultSession.EndUpdate(false);
            }

            return null;
        }

        #endregion
    }
}
