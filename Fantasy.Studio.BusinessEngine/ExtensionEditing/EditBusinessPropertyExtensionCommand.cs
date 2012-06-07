using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.BusinessEngine;
using Fantasy.Studio.Services;


namespace Fantasy.Studio.BusinessEngine.ExtensionEditing
{
    public class EditBusinessPropertyExtensionCommand : ObjectWithSite, ICommand
    {
        #region ICommand Members

        public object Execute(object args)
        {

            BusinessProperty[] properties = ((IEnumerable<BusinessProperty>)args).ToArray();
            IEditingService documentService = this.Site.GetRequiredService<IEditingService>();
            IViewContent content = null;
            foreach (BusinessProperty prop in properties)
            {
               

                content = documentService.OpenView(new PropertyExtensionData(prop));
               
            }

            if (content != null)
            {
                content.WorkbenchWindow.Select();
            }  

            return null;

        }

        #endregion
    }
}
