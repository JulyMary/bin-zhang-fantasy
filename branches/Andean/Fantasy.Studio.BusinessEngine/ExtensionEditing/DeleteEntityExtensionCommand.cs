using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.BusinessEngine.EntityExtensions;

namespace Fantasy.Studio.BusinessEngine.ExtensionEditing
{
    public class DeleteEntityExtensionCommand : ObjectWithSite, ICommand
    {
        #region ICommand Members

        public object Execute(object args)
        {
            EntityExtensionEditorModel model = (EntityExtensionEditorModel)args;
            IEntityExtension[] extensions = model.Selected.ToArray();
            foreach (IEntityExtension ext in extensions)
            {
                model.ExtensionData.Extensions.Remove(ext);
            }
            return null;
        }

        #endregion
    }
}
