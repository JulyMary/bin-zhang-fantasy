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
            es.DefaultSession.BeginUpdate();
            try
            {
                association.Package.Associations.Remove(association);
                association.Package = null;

                if (association.EntityState != EntityState.New && association.EntityState != EntityState.Deleted)
                {
                    es.DefaultSession.Delete(association);
                }

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

                es.DefaultSession.EndUpdate(true);
                this.Site.GetRequiredService<IEditingService>().CloseView(association, true);
            }
            catch
            {
                es.DefaultSession.EndUpdate(false);
            }

            return null;
        }


    }
}
