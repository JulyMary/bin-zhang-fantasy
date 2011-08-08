using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Fantasy.BusinessEngine.Services;
using Fantasy.ServiceModel;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.ClassEditing
{
    class SaveBusinessClassViewContentCommand : ObjectWithSite, ICommand
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


                IDDLService dll = this.Site.GetRequiredService<IDDLService>();

                dll.CreateClassTable((BusinessClass)vwr.Entity); 

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
