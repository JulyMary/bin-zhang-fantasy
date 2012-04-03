using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Reflection;
using Fantasy.IO;
using System.IO;

namespace Fantasy.Reflection
{
    public static class GlobalAssemblyCache
    {
        public static string[] GetGACFolders()
        {
            List<string> dirs = new List<string>();
            dirs.Add(FrameworkFolder);
            RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
            RegistryKey assmex = hklm.OpenSubKey(String.Format(@"SOFTWARE\Microsoft\.NETFramework\{0}\AssemblyFoldersEx", typeof(string).Assembly.ImageRuntimeVersion));
            foreach (string subKeyName in assmex.GetSubKeyNames())
            {
                RegistryKey subKey = assmex.OpenSubKey(subKeyName);
                dirs.Add((string)subKey.GetValue(string.Empty));
                subKey.Close();
            }
            hklm.Close();
            assmex.Close();
            return dirs.ToArray();
        }


        public static bool IsInFramework(AssemblyName assembly)
        {
            DirectoryInfo d1 = new DirectoryInfo(ProgramFilesx86());
            DirectoryInfo d2 = new DirectoryInfo(LongPath.GetDirectoryName(assembly.CodeBase));

            bool rs = d1 == d2;
            return rs;
        }

        public static String FrameworkFolder
        {
            get
            {
                return Environment.ExpandEnvironmentVariables(ProgramFilesx86() + @"\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0");
            }
        }

        private static string ProgramFilesx86()
        {
            if (8 == IntPtr.Size
                || (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))
            {
                return Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            }

            return Environment.GetEnvironmentVariable("ProgramFiles");
        }
    }
}
