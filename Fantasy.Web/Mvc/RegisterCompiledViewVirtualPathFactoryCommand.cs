using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fantasy.ServiceModel;
using System.Web.WebPages;

namespace Fantasy.Web.Mvc
{
    public class RegisterCompiledViewVirtualPathFactoryCommand : ObjectWithSite, ICommand
    {
        #region ICommand Members

        public object Execute(object args)
        {
            CompiledViewVirtualPathFactory factory = this.Site.GetRequiredService<CompiledViewVirtualPathFactory>();
            VirtualPathFactoryManager.RegisterVirtualPathFactory(factory);
            return null;
        }

        #endregion
    }
}