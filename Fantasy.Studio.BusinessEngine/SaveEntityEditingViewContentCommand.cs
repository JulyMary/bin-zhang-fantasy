using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Fantasy.ServiceModel;
using Fantasy.BusinessEngine;
using Fantasy.AddIns;

namespace Fantasy.Studio.BusinessEngine
{
    public class SaveEntityEditingViewContentCommand : ICommand
    {
        #region ICommand Members

        public object Execute(object caller)
        {
            EntityEditingViewContent vwr = (EntityEditingViewContent)caller;
            vwr.Save();
            ISession session = ServiceManager.Services.GetRequiredService<IEntityService>().DefaultSession;
            session.Flush();
            return null;
        }

        #endregion
    }
}
