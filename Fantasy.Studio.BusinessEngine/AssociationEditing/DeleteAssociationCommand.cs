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


            es.Delete(association);
           
            this.Site.GetRequiredService<IEditingService>().CloseView(association, true);


            return null;
        }


    }
}
