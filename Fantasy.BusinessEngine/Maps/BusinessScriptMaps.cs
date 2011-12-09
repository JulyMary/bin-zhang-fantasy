using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Maps
{
    public class BusinessScriptBaseMap<T> : BusinessEntityMap<T> where T : BusinessScriptBase
    {
        public BusinessScriptBaseMap()
        {
            this.Map(x => x.BuildAction);
            this.Map(x => x.MetaData);
            this.Map(x => x.Script);
            this.Map(x => x.Name);
        }
    }

    public class BusinessScriptMap : BusinessScriptBaseMap<BusinessScript>
    {
        public BusinessScriptMap()
        {
            this.Table("BUSINESSEXTRASCRIPT");
            this.References(x => x.Package);
        }

    }

    public class BusinessWebScriptMap : BusinessScriptBaseMap<BusinessWebScript>
    {
        public BusinessWebScriptMap()
        {
            this.Table("BUSINESSWEBSCRIPT");
            this.References(x => x.WebFolder);
        }
    }
}
