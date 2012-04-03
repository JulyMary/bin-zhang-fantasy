using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Reflection;
using Fantasy.IO;
using System.IO;

namespace Fantasy.GAC
{
    public static class GlobalAssemblyCache
    {
        public static string[] GetGACFolders()
        {
            List<string> dirs = new List<string>();
            dirs.Add(Environment.ExpandEnvironmentVariables(ProgramFilesx86() + @"\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0"));
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


        public static bool IsInFramework(Assembly assembly)
        {
            DirectoryInfo d1 = new DirectoryInfo(ProgramFilesx86());
            DirectoryInfo d2 = new DirectoryInfo(Path.GetDirectoryName(assembly.Location));

            bool rs = d1 == d2;
            return rs;
        }


        public static Assembly ReflectOnlyLoad(AssemblyName assemblyRef)
        {
            //var query = from dir in GetGACFolders()
            //                 let file = LongPath.Combine(dir, assemblyRef.Name + ".dll")
            //                 where LongPathFile.Exists(file)
            //                 let assembly = Assembly.ReflectionOnlyLoadFrom(file)
            //                 orderby GetMatchRate(assemblyRef, assembly.GetName()) descending
            //                 select assembly;
            //return query.FirstOrDefault();

            return Assembly.ReflectionOnlyLoad(assemblyRef.FullName);

        }

        private static int GetMatchRate(AssemblyName source, AssemblyName target)
        {
            int rate = 0;
            if (source.Version == target.Version)
            {
                rate += 100;
            }
            string token1 = source.GetPublicKeyToken() != null ? BitConverter.ToString(source.GetPublicKeyToken()) : null;
            string token2 = target.GetPublicKeyToken() != null ? BitConverter.ToString(target.GetPublicKeyToken()) : null;
            if (token1 == token2)
            {
                rate += 10;
            }
            if (source.CultureInfo == target.CultureInfo)
            {
                rate += 1;
            }

            return rate;

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
