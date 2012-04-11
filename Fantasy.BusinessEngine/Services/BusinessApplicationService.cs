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
            this._datas = es.Query<BusinessApplicationData>().ToList();
            this._datas.SortBy(a => a.Id);
           
            base.InitializeService();
        }

        private List<BusinessApplicationData> _datas;
       

        private Type EntityType(BusinessApplicationData data)
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


        private BusinessApplication Create(BusinessApplicationData data, Type type)
        {
            BusinessApplication rs = (BusinessApplication)Activator.CreateInstance(type);
            rs.Data = data;
            return rs;

        }

        #region IBusinessApplicationService Members

        public BusinessApplication Create(Guid id)
        {
            int pos = this._datas.BinarySearchBy(id, a=>a.Id);
            if(pos >= 0)
            {
                BusinessApplicationData data = this._datas[pos];
                return this.Create(data, this.EntityType(data));
            }
            else
            {
                throw new EntityNotFoundException(typeof(BusinessApplication), id);
            }
        }

        public BusinessApplication Create(Type t)
        {
            BusinessApplicationData data = this._datas.FirstOrDefault(a => this.EntityType(a) == t);
            if (data != null)
            {
                return this.Create(data, t);
            }
            else
            {
                throw new ApplicationException(String.Format("Application does not exist for type {0}", t.AssemblyQualifiedName)); 
            }
        }

        #endregion
    }
}
