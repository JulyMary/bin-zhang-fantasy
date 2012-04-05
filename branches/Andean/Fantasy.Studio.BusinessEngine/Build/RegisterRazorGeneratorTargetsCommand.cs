using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.Studio.BusinessEngine.Properties;
using System.Runtime.InteropServices; 

namespace Fantasy.Studio.BusinessEngine.Build
{
    public class RegisterRazorGeneratorTargetsCommand : ObjectWithSite, ICommand
    {
        #region ICommand Members

        public object Execute(object args)
        {
            string path = Settings.ExtractToFullPath(Settings.Default.RazorGeneratorTargetsPath);

            Environment.SetEnvironmentVariable("RazorGeneratorTargetsPath", path, EnvironmentVariableTarget.User);
            Environment.SetEnvironmentVariable("RazorGeneratorTargetsPath", path, EnvironmentVariableTarget.Process);
            return null;
        }

        #endregion

 
   

    }
}
