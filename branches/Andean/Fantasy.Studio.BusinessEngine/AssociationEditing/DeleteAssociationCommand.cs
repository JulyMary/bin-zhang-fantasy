using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.BusinessEngine.Services;
using Fantasy.BusinessEngine;
using Fantasy.Studio.Services;

namespace Fantasy.Studio.BusinessEngine.AssociationEditing
{
    public class DeleteAssociationCommand : ObjectWithSite, ICommand
    {

        public object Execute(object args)
        {
            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            BusinessAssociation association = (BusinessAssociation)args;
            IDDLService ddl = this.Site.GetRequiredService<IDDLService>();
            es.BeginUpdate();
            try
            {
                association.Package.Associations.Remove(association);
                association.Package = null;

                
               
                if (association.LeftClass != null)
                {
                    association.LeftClass.LeftAssociations.Remove(association);
                    association.LeftClass = null;
                }

                if (association.RightClass != null)
                {
                    association.RightClass.RightAssociations.Remove(association);
                    association.RightClass = null;
                }
               
                ddl.DeleteAssociationTable(association);
                es.Delete(association);
                es.EndUpdate(true);
                this.Site.GetRequiredService<IEditingService>().CloseView(association, true);
            }
            catch
            {
                es.EndUpdate(false);
            }

            return null;
        }


    }
}
