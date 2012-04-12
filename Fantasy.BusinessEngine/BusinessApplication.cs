using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Services;
using Fantasy.BusinessEngine.Security;

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




    }
}
