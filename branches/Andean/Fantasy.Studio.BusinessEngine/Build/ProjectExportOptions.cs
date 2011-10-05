using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.IO;

namespace Fantasy.Studio.BusinessEngine.Build
{
    public class ProjectExportOptions
    {
        public Projects Project { get; set; }

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


    }
}
