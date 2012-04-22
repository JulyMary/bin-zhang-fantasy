using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fantasy.ServiceModel;
using System.Web.WebPages;

namespace Fantasy.Web.Mvc
{
    public class RegisterCompiledContentHandlerFactoryCommand : ObjectWithSite, ICommand
    {
        #region ICommand Members

        public object Execute(object args)
        {
            CompiledContentHandlerFactory factory = this.Site.GetRequiredService<CompiledContentHandlerFactory>();
            VirtualPathFactoryManager.RegisterVirtualPathFactory(factory);
            return null;
        }

        #endregion
    }
}