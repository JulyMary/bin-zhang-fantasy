using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.BusinessEngine.Services;
using Fantasy.IO;
using Fantasy.BusinessEngine.Properties;
using System.IO;
using System.Security.Cryptography;

namespace Fantasy.BusinessEngine.Services
{
    public class UpdateLocalAssembliesService : ServiceBase
    {


        public override void InitializeService()
        {

            if (!LongPathDirectory.Exists(Settings.Default.FullReferencesPath))
            {
                LongPathDirectory.Create(Settings.Default.FullReferencesPath);
            }

            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            BusinessAssemblyReferenceGroup group = es.GetAssemblyReferenceGroup();
            foreach (BusinessAssemblyReference reference in group.References)
            {
                this.UpdateReferenceAssembly(reference);

            }
            base.InitializeService();
        }

        private void UpdateReferenceAssembly(BusinessAssemblyReference reference)
        {
            if (reference.Source == BusinessAssemblyReferenceSources.Local)
            {
                string fileName = LongPath.Combine(Settings.Default.FullReferencesPath, reference.Name + ".dll");
                bool needCopy = false;

                if (!LongPathFile.Exists(fileName))
                {
                    needCopy = true;
                }
                else
                {
                    FileStream fs = LongPathFile.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    try
                    {
                        using (MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider())
                        {
                            byte[] bitHash = provider.ComputeHash(fs);
                            string rowHash = BitConverter.ToString(bitHash).Replace("-", string.Empty);
                            needCopy = rowHash != reference.RawHash;
                        }
                    }
                    finally
                    {
                        fs.Close();
                    }
                }

                if (needCopy && reference.RawAssembly != null)
                {
                    FileStream fs = LongPathFile.Open(fileName, FileMode.Create, FileAccess.ReadWrite);
                    try
                    {
                        byte[] raw = reference.RawAssembly;
                        fs.Write(raw, 0, raw.Length);
                    }
                    finally
                    {
                        fs.Close();
                    }
                }
            }
        }
    }
}
