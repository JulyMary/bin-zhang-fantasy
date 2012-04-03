﻿using System;
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

            AddGACReferenceDialog dlg = new AddGACReferenceDialog();
            if (dlg.ShowDialog() == true)
            {
                IEntityService es = this.Site.GetRequiredService<IEntityService>();
               

                foreach (AssemblyModel model in dlg.SelectedAssemblies)
                {
                    BusinessAssemblyReferenceGroup group = es.GetAssemblyReferenceGroup();
                    string name = model.AssemblyName.Name;
                    
                    if (!group.References.Any(r => string.Equals(name, r.Name, StringComparison.OrdinalIgnoreCase)))
                    {
                        BusinessAssemblyReference reference = es.CreateEntity<BusinessAssemblyReference>();
                        reference.FullName = model.Location.ToLower().StartsWith(GlobalAssemblyCache.FrameworkFolder.ToLower()) ? name : model.AssemblyName.FullName; 
                        reference.Group = group;
                        group.References.Add(reference);
                        reference.Source = BusinessAssemblyReferenceSources.GAC;
                        es.SaveOrUpdate(reference);
                    }
                }


            }

            return null;
        }


    }
}
