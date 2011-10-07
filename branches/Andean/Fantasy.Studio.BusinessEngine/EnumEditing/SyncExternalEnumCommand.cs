using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.BusinessEngine;
using Fantasy.Reflection;
using Fantasy.IO;
using System.Reflection;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.Studio.BusinessEngine.EnumEditing
{
    public class SyncExternalEnumCommand : ObjectWithSite, ICommand
    {
        #region ICommand Members

        public object Execute(object args)
        {
            BusinessEnum @enum = (BusinessEnum)args;
            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            using(AssemblyLoader loader = new AssemblyLoader())
            {

                
                BusinessAssemblyReference reference = es.GetAssemblyReferenceGroup().References.Where(r=>r.Name == @enum.ExternalAssemblyName).SingleOrDefault();
                if (reference != null)
                {
                    Assembly asm = null;
                    if (reference.Source != BusinessAssemblyReferenceSources.GAC)
                    {
                        string path = LongPath.Combine(Fantasy.BusinessEngine.Properties.Settings.Default.FullReferencesPath, @enum.ExternalAssemblyName + ".dll");
                        if (LongPathFile.Exists(path))
                        {
                            asm = loader.ReflectionOnlyLoadFrom(path);
                        }


                    }

                    if (asm == null)
                    {
                        asm = loader.ReflectionOnlyLoad(reference.FullName);
                    }

                    Type enumType = loader.ReflectionOnlyGetType(asm, @enum.FullCodeName);

                    es.SyncExternalEnum(@enum, enumType);
                }
            }

            return null;
        }

        

        #endregion
    }
}
