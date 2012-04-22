using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Web.Administration;
using Microsoft.Win32;

namespace Fantasy.Web
{
    public static class MIMETypes
    {

       
        static MIMETypes()
        {
            _mimeTypes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            MimeTypesFormRegistry();
            MimeTypesFormConfig();
        }

        private static void MimeTypesFormRegistry()
        {
            RegistryKey contentTypes = Registry.ClassesRoot.OpenSubKey(@"Mime\Database\Content Type", false);

            foreach (string name in contentTypes.GetSubKeyNames())
            {
                RegistryKey contentType = contentTypes.OpenSubKey(name);
                string extension = (string)contentType.GetValue("Extension");
                if (!string.IsNullOrEmpty(extension))
                {
                    _mimeTypes[extension] = name;
                }
            }
        }

        private static void MimeTypesFormConfig()
        {
            using (ServerManager serverManager = new ServerManager())
            {
                // If interested in global mimeMap:
                Microsoft.Web.Administration.Configuration config = serverManager.GetApplicationHostConfiguration();



                Microsoft.Web.Administration.ConfigurationSection staticContent = config.GetSection("system.webServer/staticContent");
                Microsoft.Web.Administration.ConfigurationElementCollection mimeMap = staticContent.GetCollection();

                // Print all mime types
                foreach (Microsoft.Web.Administration.ConfigurationElement mimeType in mimeMap)
                {
                    _mimeTypes[(string)mimeType["fileExtension"]] = (string)mimeType["mimeType"];

                }
            }
        }


        public static string GetMIMETypeForExtension(string extension)
        {

            return _mimeTypes.GetValueOrDefault(extension, "application/octet-stream ");
        }


        private static Dictionary<string, string> _mimeTypes;
    }
}