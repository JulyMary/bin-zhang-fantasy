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
        public BusinessObjectMemberSecurity this[string name]
        {
            get
            {
                return this.Single(p => String.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase), string.Format(Resources.SecurityObjectPropertyNotFoundMessage, name)); 
            }
        }
    }
}
