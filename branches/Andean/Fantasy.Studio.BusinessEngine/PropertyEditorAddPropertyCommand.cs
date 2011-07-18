using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using NHibernate;
using Fantasy.ServiceModel;
using Fantasy.Utils;
using Fantasy.Studio.BusinessEngine.Properties;
using Fantasy.AddIns;

namespace Fantasy.Studio.BusinessEngine
{
    class PropertyEditorAddPropertyCommand : ICommand
    {
        public  object Execute(object caller)
        {
            PropertyEditor editor = (PropertyEditor)caller;
            IEntityService svc = ServiceManager.Services.GetRequiredService<IEntityService>();
            BusinessProperty property = svc.CreateEntity<BusinessProperty>();
            property.Name = UniqueNameGenerator.GetName(Resources.DefaultNewBusinessPropertyName, editor.Entity.Properties.Select(p => p.Name));
            property.Class = editor.Entity;
            editor.Entity.Properties.Add(property);
            return property;
        }

        
    }
}
