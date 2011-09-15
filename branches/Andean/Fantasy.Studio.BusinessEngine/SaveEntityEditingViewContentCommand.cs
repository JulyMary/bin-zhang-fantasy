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
    class SaveEntityEditingViewContentCommand : ObjectWithSite, ICommand
    {
        #region ICommand Members

        public object Execute(object caller)
        {
            IEntityService es = this.Site.GetRequiredService<IEntityService>();

            EntityEditingViewContent vwr = (EntityEditingViewContent)caller;
            es.BeginUpdate();
            try
            {

                vwr.Save();
                es.EndUpdate(true);
            }
            catch
            {
                es.EndUpdate(false);
                throw;
            }
            
            return null;
        }

        #endregion
    }
}
