using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.BusinessEngine.Services;
using Fantasy.BusinessEngine;
using Fantasy.Studio.Services;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing
{
    public class DeleteClassDiagramCommand : ObjectWithSite, ICommand
    {
       

        public object Execute(object args)
        {
            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            BusinessClassDiagram diagram = (BusinessClassDiagram)args;
            es.BeginUpdate();
            try
            {
                diagram.Package.ClassDiagrams.Remove(diagram);
                diagram.Package = null;

                if (diagram.EntityState != EntityState.New && diagram.EntityState != EntityState.Deleted)
                {
                    es.Delete(diagram);
                }

                es.EndUpdate(true);
                this.Site.GetRequiredService<IEditingService>().CloseView(diagram, true);
            }
            catch
            {
                es.EndUpdate(false);
            }

            return null;
        }

       
    }
}
