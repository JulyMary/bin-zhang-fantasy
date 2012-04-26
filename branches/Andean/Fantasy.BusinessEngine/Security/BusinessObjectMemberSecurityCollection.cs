using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Fantasy.BusinessEngine.Properties;

namespace Fantasy.BusinessEngine.Security
{
	public class BusinessObjectMemberSecurityCollection : ObservableCollection<BusinessObjectMemberSecurity>
	{

        public BusinessObjectMemberSecurity this[string propertyCodeName]
        {
            get
            {
                return this.Single(p => String.Equals(p.Name, propertyCodeName, StringComparison.OrdinalIgnoreCase), String.Format(Resources.SecurityObjectPropertyNotFoundMessage, propertyCodeName)); 
            }
        }
	}
}
