using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fantasy.ServiceModel;
using System.Web.Mvc;

namespace Fantasy.Web.Mvc
{
    public class RegisterCompiledViewEngineCommand : ObjectWithSite, ICommand
    {
        #region ICommand Members

        public object Execute(object args)
        {
            CompiledViewEngine engine = this.Site.GetRequiredService<CompiledViewEngine>();
            //ViewEngines.Engines.Insert(0, engine);
            ViewEngines.Engines.Add(engine);
            return null;
        }

        #endregion
    }
}