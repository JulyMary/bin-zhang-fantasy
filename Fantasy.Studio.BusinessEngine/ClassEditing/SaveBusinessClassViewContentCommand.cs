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
            IEntityService es = this.Site.GetRequiredService<IEntityService>();

            EntityEditingViewContent vwr = (EntityEditingViewContent)caller;
            es.BeginUpdate();
            try
            {

                vwr.Save();


                IDDLService dll = this.Site.GetRequiredService<IDDLService>();

                dll.CreateClassTable((BusinessClass)vwr.Entity); 

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
