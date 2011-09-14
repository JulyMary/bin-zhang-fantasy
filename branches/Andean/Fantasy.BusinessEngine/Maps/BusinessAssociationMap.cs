using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Maps
{
    public class BusinessAssociationMap : BusinessEntityMap<BusinessAssociation>
    {
        public BusinessAssociationMap()
	    {
            this.Table("BUSINESSASSOCIATION");
            this.Map(x => x.Name).Not.Nullable();
            this.Map(x => x.CodeName).Not.Nullable();
            this.References(x => x.Package).Column("PackageId");
            this.Map(x => x.TableName).Not.Nullable();
            this.Map(x => x.TableSchema).Not.Nullable();
            this.Map(x => x.TableSpace);

            this.References(x => x.LeftClass).Column("LEFTCLASSID").Not.Nullable();
            this.Map(x => x.LeftCardinality).Not.Nullable();
            this.Map(x => x.LeftRoleName);
            this.Map(x => x.LeftRoleCode);
            this.Map(x => x.LeftNavigatable).Not.Nullable();
            this.Map(x => x.LeftRoleDisplayOrder); 

            this.References(x => x.RightClass).Column("RIGHTCLASSID").Not.Nullable();
            this.Map(x => x.RightCardinality).Not.Nullable();
            this.Map(x => x.RightRoleName);
            this.Map(x => x.RightRoleCode);
            this.Map(x => x.RightNavigatable);
            this.Map(x => x.RightRoleDisplayOrder); 


           
	    }
    }
}
