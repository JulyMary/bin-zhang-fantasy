using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Maps
{
    public class BusinessDataTypeMap : BusinessEntityMap<BusinessDataType>
    {
        public BusinessDataTypeMap ()
	    {
            this.Table("BusinessDataType");
            this.Map(x => x.Name).Not.Nullable();
            this.Map(x => x.CodeName).Not.Nullable();
            this.Map(x => x.DefaultDatabaseType).Column("DEFAULTDBTYPE").Not.Nullable();
            this.Map(x => x.DefaultLength);
            this.Map(x => x.DefaultPrecision); 
	    }
       
    }
}
