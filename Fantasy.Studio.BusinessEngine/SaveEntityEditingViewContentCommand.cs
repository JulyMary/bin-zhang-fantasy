using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Fantasy.ServiceModel;
using Fantasy.BusinessEngine;
using Fantasy.AddIns;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.Studio.BusinessEngine
{
    public class SaveEntityEditingViewContentCommand : ObjectWithSite, ICommand
    {
        #region ICommand Members

        public object Execute(object caller)
        {
            ISession session = this.Site.GetRequiredService<IEntityService>().DefaultSession;

            EntityEditingViewContent vwr = (EntityEditingViewContent)caller;
            session.BeginUpdate();
            try
            {

                vwr.Save();
                session.EndUpdate(true);
            }
            catch
            {
                session.EndUpdate(false);
                throw;
            }
            

            
            return null;
        }

        #endregion
    }
}
