using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Management
{
    public interface IJobManagerSettingsReader
    {
        object GetSetting(string name);
        T GetSetting<T>(string name);
    }

    public class JobManagerSettingsReaderService : AbstractService, IJobManagerSettingsReader
    {

        public override void InitializeService()
        {
            base.InitializeService();
        }

        #region IJobManagerSettingsReader Members

        public object GetSetting(string name)
        {
            Type t = typeof(JobManagerSettings);
            PropertyInfo prop = t.GetProperty(name);
            object rs = prop.GetValue(JobManagerSettings.Default, null); 
            //object rs = t.InvokeMember(name, System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.IgnoreCase, null, 
            //    JobManagerSettings.Default, new object[] {});
            return rs;
        }

        public T GetSetting<T>(string name)
        {
            return (T)this.GetSetting(name);
        }

        #endregion
    }
}
