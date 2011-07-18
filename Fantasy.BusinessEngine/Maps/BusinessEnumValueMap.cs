using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Maps
{
    public class BusinessEnumValueMap : BusinessEntityMap<BusinessEnumValue>
    {
        public BusinessEnumValueMap()
        {
            this.Table("BusinessEnumValue");
            this.Map(x => x.Name).Not.Nullable();
            this.Map(x => x.CodeName).Not.Nullable();
            this.Map(x => x.Value);
            this.References<BusinessEnum>(x => x.Enum).Column("ENUMID");
        }
    }
}
