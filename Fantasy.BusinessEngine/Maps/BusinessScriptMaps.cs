using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Collections;

namespace Fantasy.BusinessEngine.Maps
{

    public class BusinessScriptMap : BusinessEntityMap<BusinessScript>
    {
        public BusinessScriptMap()
        {
            this.Table("BUSINESSEXTRASCRIPT");
            this.References(x => x.Package).Column("PackageID");
            this.Map(x => x.BuildAction);
            this.Map(x => x.MetaData);
            this.Map(x => x.Script);
            this.Map(x => x.Name);
           

        }

    }

   
 
}
