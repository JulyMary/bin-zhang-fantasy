using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.BusinessEngine.Properties;

namespace Fantasy.BusinessEngine.Services
{
    public class BusinessApplicationService : ServiceBase, IBusinessApplicationService
    {

        public override void InitializeService()
        {
            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            BusinessApplicationData[] datas = es.Query<BusinessApplicationData>().ToArray();
            foreach (BusinessApplicationData data in datas)
            {
                
            }


            base.InitializeService();
        }

        private static Type EntityType(this BusinessApplicationData data)
        {
            Type rs;
            switch (data.ScriptOptions)
            {
                case ScriptOptions.Default:
                    rs = Type.GetType(data.FullCodeName + ", " + Settings.Default.BusinessDataAssemblyName, true);
                    break;
                case ScriptOptions.None:
                    rs = typeof(BusinessApplication); 
                    break;
                case ScriptOptions.External:
                    rs = Type.GetType(data.ExternalType, true);
                    break;
                default :
                    throw new Exception("Impossible.");
               
            }


            return rs;
        }



       

        #region IBusinessApplicationService Members

        public IEnumerable<BusinessApplication> AllApplications
        {
            get { return _allApps; }
        }

        public BusinessApplication FindBusinessApplication(Guid id)
        {
            int pos = _allApps.BinarySearchBy(id, a => a.Data.Id);
            return pos >= 0 ? _allApps[pos] : null;
        }

        public BusinessApplication FindBusinessApplicationForType(Type t)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
