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
using Fantasy.BusinessEngine.Services;

namespace Fantasy.Studio.BusinessEngine
{
    class AddPropertyCommand : ObjectWithSite, ICommand
    {
        public  object Execute(object args)
        {
            BusinessClass cls = (BusinessClass)args;
            IEntityService svc = this.Site.GetRequiredService<IEntityService>();
            BusinessProperty property = svc.CreateEntity<BusinessProperty>();
            property.Name = UniqueNameGenerator.GetName(Resources.DefaultNewBusinessPropertyName, cls.Properties.Select(p => p.Name));
            property.CodeName = property.FieldName = UniqueNameGenerator.GetCodeName(property.Name);
            property.Class = cls;
            cls.Properties.Add(property);
            return property;
        }
    }
}
