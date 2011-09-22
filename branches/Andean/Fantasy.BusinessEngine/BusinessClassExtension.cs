using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine
{
    public static class BusinessClassExtension
    {
        public static IEnumerable<BusinessProperty>  AllProperties(this BusinessClass @class)
        {
            var query = from ancestor in @class.Flatten(x => x.ParentClass).Reverse()
                        from property in ancestor.Properties
                        select property;

            return query;
        }

        public static IEnumerable<BusinessAssociation> AllLeftAssociations(this BusinessClass @class)
        {
            var query = from ancestor in @class.Flatten(x => x.ParentClass).Reverse()
                        from association in ancestor.LeftAssociations
                        select association;
            return query;
        }

        public static IEnumerable<BusinessAssociation> AllRightAssociations(this BusinessClass @class)
        {
            var query = from ancestor in @class.Flatten(x => x.ParentClass).Reverse()
                        from association in ancestor.RightAssociations
                        select association;
            return query;
        }
    }
}
