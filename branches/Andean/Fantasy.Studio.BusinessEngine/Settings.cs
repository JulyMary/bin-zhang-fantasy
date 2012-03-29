using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using Fantasy.Studio.BusinessEngine.CodeEditing;
using System.Configuration;

namespace Fantasy.Studio.BusinessEngine.Properties
{
    partial class Settings 
    {
        public static string ExtractToFullPath(string value)
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


        private List<FileSyntaxHighlight> _fileSyntax = new List<FileSyntaxHighlight>();
        [UserScopedSetting]
        public List<FileSyntaxHighlight> FileSyntax
        {
            get
            {
                return _fileSyntax;
            }
        }


        private List<FileSyntaxHighlight> _systemFileSyntax = new List<FileSyntaxHighlight>();
        [ApplicationScopedSetting] 
        public List<FileSyntaxHighlight> SystemFileSyntax
        {
            get
            {
                return _systemFileSyntax;
            }
        }

       
    }
}
