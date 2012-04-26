using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web.Configuration;
using System.Configuration;
using System.Reflection;

namespace RazorGenerator.Core
{
    public class WebConfigDefaultNamespaceLoader : MarshalByRefObject
    {
        public static string[] GetDefaultNamespaces(string projectDirectory)
        {
            string[] rs; 
            string configFile = Path.Combine(projectDirectory, "web.config");


            FileInfo fi = new FileInfo(configFile);
            DateTime oldTime;

            if (!_modificationTime.TryGetValue(configFile, out oldTime) || fi.LastWriteTime > oldTime)
            {
                if (File.Exists(configFile))
                {
                    ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap()
                    {
                        ExeConfigFilename = configFile
                    };
                    Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                    PagesSection pagesSection = (PagesSection)config.GetSection("system.web/pages");
                    return pagesSection.Namespaces.Cast<NamespaceInfo>().Select(i => i.Namespace).ToArray();
                }
                else
                {
                    rs = new string[] { };
                }

                _modificationTime[configFile] = fi.LastWriteTime;
                _namespaces[configFile] = rs;
            }
            else
            {
                rs = _namespaces[configFile];
            }


           

            return rs;
        }


        private static Dictionary<string, string[]> _namespaces = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

        private static Dictionary<string, DateTime> _modificationTime = new Dictionary<string, DateTime>(StringComparer.OrdinalIgnoreCase); 

     

        
    }
}
