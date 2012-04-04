using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.BusinessEngine.Services;
using System.Reflection;
using Fantasy.BusinessEngine;
using System.Windows.Threading;
using Fantasy.Reflection;

namespace Fantasy.Studio.BusinessEngine.AssemblyReference
{
    public class AddGACReferenceCommand : ObjectWithSite, ICommand
    {
        public object Execute(object args)
        {

            List<BusinessAssemblyReference> rs = new List<BusinessAssemblyReference>();

            AddGACReferenceDialog dlg = new AddGACReferenceDialog();
            if (dlg.ShowDialog() == true)
            {
                IEntityService es = this.Site.GetRequiredService<IEntityService>();
               

                foreach (AssemblyModel model in dlg.SelectedAssemblies)
                {

                    BusinessAssemblyReference reference = es.AddGACAssemblyReference(model.Location);
                    rs.Add(reference); 
                }


            }

            return rs.ToArray();
        }


    }
}
