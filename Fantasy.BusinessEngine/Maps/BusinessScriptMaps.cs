using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Collections;

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
            this.References(x => x.Package).Column("PackageID");
           

        }

    }

    public class BusinessWebControllerMap : BusinessScriptBaseMap<BusinessWebController>
    {
        public BusinessWebControllerMap()
        {
            this.Table("BUSINESSWEBCONTROLLER");
            this.References(x => x.Package).Column("PackageID");
            this.HasMany(x => x.PersistedViews).CollectionType<ObservableList<BusinessWebView>>().KeyColumn("ControllerID").Cascade.None().Inverse().LazyLoad();
        }

    }

    public class BusinessWebViewMap : BusinessScriptBaseMap<BusinessWebView>
    {
        public BusinessWebViewMap()
        {
            this.Table("BUSINESSWEBVIEW");
            this.References(x => x.Controller).Column("ControllerID");

        }
    }


 
}
