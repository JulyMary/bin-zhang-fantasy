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
            BusinessApplicationData data = this._datas.First(a => this.EntityType(a) == t, String.Format(Resources.ApplicationByTypeExceptionMessage, t.AssemblyQualifiedName));
           
            return this.Create(data, t);
           
        }


        public BusinessApplication CreateByName(string codeName)
        {
            BusinessApplicationData data = this._datas.Single(a => string.Equals(a.CodeName, codeName, StringComparison.OrdinalIgnoreCase)
                || string.Equals(a.CodeName, codeName + "Application", StringComparison.OrdinalIgnoreCase), String.Format(Resources.ApplicationByNameExceptionMessage, codeName));
            return this.Create(data, this.EntityType(data));
        }

      


        public void ReleaseApplication(BusinessApplication application)
        {
            IDisposable disposable = application as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

       


       

        #endregion

        #region IBusinessApplicationService Members


        public BusinessApplicationData GetApplicationData(Guid id)
        {
            int pos = this._datas.BinarySearchBy(id, a => a.Id);
            if (pos >= 0)
            {
                BusinessApplicationData data = this._datas[pos];
                return data;
            }
            else
            {
                throw new EntityNotFoundException(typeof(BusinessApplicationData), id);
            }
        }



        #endregion

        #region IBusinessApplicationService Members


        public string GetApplicationFriendlyName(Guid id)
        {
            BusinessApplicationData application = this.GetApplicationData(id);
            string rs = application.CodeName;
            if (rs.ToLower().EndsWith("application"))
            {
                rs = rs.Substring(0, rs.Length - "application".Length);
            }
            return rs;
        }

        #endregion
    }
}
