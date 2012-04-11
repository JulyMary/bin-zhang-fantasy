using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Services;

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
            return BusinessEngineContext.Current.GetRequiredService<ISecurityService>().GetObjectSecurity(obj);
        }




    }
}
