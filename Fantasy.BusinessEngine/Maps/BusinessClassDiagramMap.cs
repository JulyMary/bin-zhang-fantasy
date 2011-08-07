using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Maps
{
    public class BusinessClassDiagramMap : BusinessEntityMap<BusinessClassDiagram>
    {
        public BusinessClassDiagramMap()
        {
            this.Table("BusinessClassDiagram");
            this.Map(x => x.Name).Not.Nullable();
            this.Map(x => x.Diagram);
            this.References(x => x.Package).Column("PackageId");


        }
    }
}
