using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.BusinessEngine;
using Fantasy.Adaption;
using Fantasy.BusinessEngine.Services;
using Fantasy.Studio.Services;

namespace Fantasy.Studio.BusinessEngine.CodeEditing
{
    public class AddScriptCommand : ObjectWithSite, ICommand
    {

        #region ICommand Members

        public object Execute(object args)
        {
            BusinessScript[] rs = null;
            BusinessPackage package = this.Site.GetRequiredService<IAdapterManager>().GetAdapter<BusinessPackage>(args);
            AddScriptDialogModel model = new AddScriptDialogModel(this.Site, package);

            model.SelectedItem = model.Items.SingleOrDefault(t => string.Equals(this.Template, t.Name, StringComparison.OrdinalIgnoreCase));
            if (model.SelectedItem == null)
            {
                model.SelectedItem = model.Items.FirstOrDefault();
            }
            AddScriptDialog dialog = new AddScriptDialog();
            dialog.DataContext = model;
            if (dialog.ShowDialog() == true)
            {
                IEntityService es = this.Site.GetRequiredService<IEntityService>();
                rs = es.AddBusinessScript(package, model.SelectedItem, model.Name);

                IEditingService documentService = ServiceManager.Services.GetRequiredService<IEditingService>();
                for (int i = 0; i < model.SelectedItem.Items.Count; i++)
                {
                    if (model.SelectedItem.Items[i].AutoOpen)
                    {
                        documentService.OpenView(rs[i]);
                    }
                }
            }
            return rs;
            
          
        }

        #endregion


        public string Template { get; set; }
    }
}
