using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine
{
    public class BusinessPropertyDescriptorCollection : List<BusinessPropertyDescriptor>
    {

        internal BusinessPropertyDescriptorCollection(BusinessObjectDescriptor owner)
        {
            this._owner = owner; 
        }

        private BusinessObjectDescriptor _owner;

        public BusinessPropertyDescriptor this[string propertyCodeName]
        {
            get
            {
                BusinessPropertyDescriptor rs = this.SingleOrDefault(p => string.Equals(p.CodeName, propertyCodeName, StringComparison.OrdinalIgnoreCase));
                if (rs == null)
                {
                    throw new PropertyNotFoundException(this._owner.Class, propertyCodeName);
                }

                return rs;
            }
        }
    }
}
