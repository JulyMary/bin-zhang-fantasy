using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Services;
using Fantasy.BusinessEngine.Security;
using Fantasy.Web;

namespace Fantasy.BusinessEngine
{
    public class BusinessApplication 
    {
       
        public BusinessApplicationData Data { get; internal set; }


        private BusinessObject _entryObject = null;

        public BusinessObject EntryObject
        {
            get 
            {
                if (this._entryObject == null && this.Data.EntryObjectId != null)
                {
                    this._entryObject = BusinessEngineContext.Current.GetRequiredService<IEntityService>().Get<BusinessObject>(this.Data.EntryObjectId);
                }
                return _entryObject; 
            }
            set 
            {
                _entryObject = value;
                
            }
        }


        public virtual void Load()
        {

        }


        public virtual void Unload()
        {

        }


        public virtual BusinessObjectSecurity GetObjectSecurity(BusinessObject obj)
        {

            BusinessUser user = BusinessEngineContext.Current.User;
            ObjectSecurityArgs args = new ObjectSecurityArgs() { Application = this, Object = obj, User = user };

            return BusinessEngineContext.Current.GetRequiredService<ISecurityService>().GetObjectSecurity(args);
        }


        public virtual BusinessObjectSecurity GetClassSecurity(BusinessClass @class)
        {
            BusinessUser user = BusinessEngineContext.Current.User;
            ClassSecurityArgs args = new ClassSecurityArgs() { Application = this, Class=@class, User = user };
            return BusinessEngineContext.Current.GetRequiredService<ISecurityService>().GetClassSecurity(args);
        }


        public virtual INavigationViewController GetNaviationView()
        {
            Type t = Type.GetType("Fantasy.Web.Controllers.StandardNavigationController, Fantasy.Web", true);
            return (INavigationViewController)Activator.CreateInstance(t);
        }


        public virtual IScalarViewController GetScalarView(BusinessObject obj)
        {
            throw new NotImplementedException();
        }

        public virtual ICollectionViewController GetCollectionView(BusinessObject obj, string propertyName, IEnumerable<BusinessObject> collection)
        {
            throw new NotImplementedException();
        }

    }
}
