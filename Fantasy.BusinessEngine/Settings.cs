﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using Fantasy.IO;

namespace Fantasy.BusinessEngine.Properties
{
    partial class Settings 
    {
        private string ExtractToFullPath(string value)
        {
            string rs = Environment.ExpandEnvironmentVariables(value);

            if (!LongPath.IsPathRooted(rs))
            {
                return LongPath.Combine(AppDomain.CurrentDomain.BaseDirectory, rs);
            }

            return new Uri(rs).LocalPath;
        }

        public string FullReferencesPath
        {
            get
            {
                return this.ExtractToFullPath(this.ReferencesPath);
            }
        }
    }
}