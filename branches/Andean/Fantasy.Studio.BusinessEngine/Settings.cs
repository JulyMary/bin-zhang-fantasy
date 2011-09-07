using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace Fantasy.Studio.BusinessEngine.Properties
{
    partial class Settings 
    {
        private string ExtractToFullPath(string value)
        {
            string rs = Environment.ExpandEnvironmentVariables(value);

            if (!Path.IsPathRooted(rs))
            {
                Assembly asm = Assembly.GetEntryAssembly();
                string entryPath = Path.GetDirectoryName(asm.Location);
                rs = entryPath + Path.DirectorySeparatorChar + rs;
            }

            return new Uri(rs).LocalPath;
        }

       
    }
}
