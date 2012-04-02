using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.IO;
using Fantasy.BusinessEngine;
using System.Xml.Linq;

namespace Fantasy.Studio.BusinessEngine.Build
{
    public class ProjectImportOptions
    {
        public ProjectImportOptions()
        {
            this.ReadFile = true;
        }

       

        public string SolutionPath { get; set; }

        public string SolutionDirectory
        {
            get
            {
                return LongPath.GetDirectoryName(SolutionPath);
            }
        }

        public string ProjectPath { get; set; }

        public string ProjectDirectory
        {
            get
            {
                return LongPath.GetDirectoryName(ProjectPath);
            }
        }

        public BusinessPackage Package { get; set; }

        public XElement ItemElement { get; set; }


        public bool Handled { get; set; }

        public bool ReadFile { get; set; }
    }
}
