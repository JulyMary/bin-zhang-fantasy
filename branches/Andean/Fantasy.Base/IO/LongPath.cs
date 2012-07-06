using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Fantasy.IO.Interop;
using System.Text.RegularExpressions;

namespace Fantasy.IO
{
    public static class LongPath
    {
        public static string GetRelativePath(string pathFrom, string pathTo)
        {
            Uri uri = new Uri(pathFrom);
            string str = Uri.UnescapeDataString(uri.MakeRelativeUri(new Uri(pathTo)).ToString()).Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            if (!str.Contains(Path.DirectorySeparatorChar.ToString()))
            {
                str = "." + Path.DirectorySeparatorChar + str;
            }

            if (str.StartsWith(@"file:\\"))
            {
                str = str.Substring("file:".Length);
            }

            if (str.StartsWith(@"\\\"))
            {
                str = str.Substring(@"\\\".Length);
            }

            return str;
        }

        public static string GetDirectoryName(string path)
        {

            Uri uri = new Uri(path);

            string rs = uri.LocalPath;


            if (rs.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                rs.Remove(rs.Length - 1);
            }

            int index = rs.LastIndexOf(Path.DirectorySeparatorChar);
            if (index > 0)
            {
                rs = rs.Remove(index);
            }

            if (rs.StartsWith(@"file:\\"))
            {
                rs = rs.Substring("file:".Length);
            }

            return rs;
        }

        public static string Combine(string path1, string path2)
        {
            if (path1 == null)
            {
                throw new ArgumentNullException("root");
            }
            if (path2 == null)
            {
                throw new ArgumentNullException("relative");
            }
            if (!path1.EndsWith("\\") && !path1.EndsWith("//"))
            {
                path1 += "\\";
            }
            Uri r;
            Uri rs;
            try
            {
                r = new Uri(path1);
            }
            catch (UriFormatException error)
            {
                throw new UriFormatException(error.Message + " Uri:" + path1);
            }

            try
            {
                rs = new Uri(r, path2);
            }
            catch (UriFormatException error)
            {
                throw new UriFormatException(error.Message + " Uri:" + path2);
            }

            return rs.LocalPath;
            //return Path.Combine(path1, path2);
        }

        public static string Combine(string[] paths)
        {
            if (paths == null)
            {
                throw new ArgumentNullException("paths");
            }
            if (paths.Length == 0)
            {
                throw new ArgumentException("path");
            }

            string rs = paths[0];
            for (int i = 1; i < paths.Length; i++)
            {
                rs = Combine(rs, paths[i]);
            }
            return rs;
        }

        public static string Combine(string path1, string path2, string path3)
        {
            return Combine(Combine(path1, path2), path3);
        }

        public static string Combine(string path1, string path2, string path3, string path4)
        {
            return Combine(Combine(Combine(path1, path2), path3), path4);
        }


        public static string GetShortPathName(string path)
        {
            StringBuilder rs = new StringBuilder(255);
            NativeMethods.GetShortPathName(path, rs, 255);

            return rs.ToString();
        }

        public static string GetLongPathName(string path)
        {
            StringBuilder rs = new StringBuilder(1024);
            int len = NativeMethods.GetLongPathName(path, rs, 1024);
            if (len > 1024)
            {
                rs = new StringBuilder(len);
                len = NativeMethods.GetLongPathName(path, rs, len);
            }

            return rs.ToString(0, len);
        }



        public static string GetExtension(string path)
        {
            string file = GetFileName(path);
            int pos = file.LastIndexOf('.');
            if (pos == -1)
            {
                return "";
            }
            else
            {
                return file.Substring(pos);
            }
        }

        public static string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        public static string GetFileNameWithoutExtension(string path)
        {
            path = GetFileName(path);
            if (path == null)
            {
                return null;
            }
            int length = path.LastIndexOf('.');
            if (length == -1)
            {
                return path;
            }
            return path.Substring(0, length);
        }

        public static string GetFullPath(string path)
        {
            Uri uri = new Uri(path);
            if (uri.IsFile)
            {
                return LongPathCommon.NormalizeLongPath(path);
            }
            else
            {
                return path;
            }
        }

        public static string ChangeExtension(string path, string extension)
        {
            return Path.ChangeExtension(path, extension);
        }


        public static char[] GetInvalidFileNameChars()
        {
            return Path.GetInvalidFileNameChars();
        }

        public static char[] GetInvalidPathChars()
        {
            return Path.GetInvalidPathChars();
        }

        /*private static Regex _pathRootRegex = new Regex(@"^\\\\\?\\(?<letter>[a-z])\:", RegexOptions.IgnoreCase); 

        public static string GetPathRoot(string path)
        {
            if (path != null)
            {
                path = LongPathCommon.NormalizeLongPath(path);
                Match m = _pathRootRegex.Match(path);
                if (m.Success)
                {
                    return m.Groups["letter"].Value;
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return null;
            }

        }*/


        public static bool IsPathRooted(string path)
        {
            return Path.IsPathRooted(path);
        }

        public static bool HasExtension(string path)
        {
            return Path.HasExtension(path);
        }

        private static Regex escapeRegex = new Regex(@"[\\\/\:\*\?\""\<\>\|]");
        public static string EscapeFileName(string fileName, string replacement = "")
        {
            return escapeRegex.Replace(fileName, replacement).Trim();
        }
    }
}
