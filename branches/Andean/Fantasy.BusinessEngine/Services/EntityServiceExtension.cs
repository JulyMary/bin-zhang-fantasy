using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using System.Data;

namespace Fantasy.BusinessEngine.Services
{
    public static class EntityServiceExtension
    {

        

        public static BusinessClass GetRootClass(this IEntityService es)
        {
            return es.Get<BusinessClass>(BusinessClass.RootClassId);
        }

        public static BusinessPackage GetRootPackage(this IEntityService es)
        {
            return es.Get<BusinessPackage>(BusinessPackage.RootPackageId);  
        }

        public static BusinessAssemblyReferenceGroup GetAssemblyReferenceGroup(this IEntityService es)
        {
            return es.Get<BusinessAssemblyReferenceGroup>(BusinessAssemblyReferenceGroup.RootId);
        }
    }
}
