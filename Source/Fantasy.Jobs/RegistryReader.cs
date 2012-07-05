using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace Fantasy.Jobs
{
    public class RegistryReader : ITagValueProvider
    {
        public string GetTagValue(string key, IDictionary<string, object> context)
        {
            string rs = string.Empty;
            if (key == null)
            {
                throw new ArgumentNullException(key);
            }
            if (key == string.Empty)
            {
                throw new ArgumentException("Key can not be empty.", "key");
            }

            Regex regex = new Regex(@"(?<root>[^\\]*)\\(?<sub>[^@]*)(@(?<name>.*))+");

            Match m = regex.Match(key);

            RegistryKey root;
            switch (m.Groups["root"].Value.ToUpper())
            {
                case "HKLM":
                    root = Registry.LocalMachine;
                    break;
                case "HKCU":
                    root = Registry.CurrentUser;
                    break;
                case "HKCR":
                    root = Registry.ClassesRoot;
                    break;
                case "HKCC":
                    root = Registry.CurrentConfig;
                    break;
                case "HKPD":
                    root = Registry.PerformanceData;
                    break;
                case "HKU":
                    root = Registry.Users;
                    break;
                default:
                    throw new ArgumentException("Invalid key name.", "key");

            }

            root.OpenSubKey(m.Groups["sub"].Value);
            if (root != null)
            {
                string name = m.Groups["name"].Success ? m.Groups["name"].Value : null;

                rs = root.GetValue(name, "").ToString();
                root.Close();
            }

            return rs;

        }

        public bool HasTag(string tag, IDictionary<string, object> context)
        {
            if (tag == null)
            {
                throw new ArgumentNullException(tag);
            }

            tag = tag.ToUpper();
            foreach (string root in RegRoots)
            {
                if (tag.StartsWith(root))
                {
                    return true;
                }
            }

            return false;

        }

        private static readonly string[] RegRoots = new string[] { "HKLM\\", "HKCU\\", "HKCR\\", "HKCC\\" };

        #region ITagValueProvider Members

        public char Prefix
        {
            get { return '$'; }
        }

        #endregion

        #region ITagValueProvider Members


        public bool IsEnabled(IDictionary<string, object> context)
        {
            return true;
        }

        #endregion
    }
}
