using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fantasy.BusinessEngine;
using System.Web.Mvc;

namespace Fantasy.Web.Mvc.Html
{
    partial class PropertyGrid
    {
        public PropertySort Sort { get; set; }

        protected override void PreExecute()
        {
            if (!this.Attributes.ContainsKey("id"))
            {
                this.Attributes.Add("id", "_propertyGrid__" + Guid.NewGuid().ToString("N"));
            }

            base.PreExecute();
        }


        

        private IEnumerable<BusinessPropertyDescriptor> GetSortedProperties(BusinessObjectRender<BusinessObject> render)
        {
            var query = from property in render.Descriptor.Properties where property.CanRead && property.IsScalar select property;

            switch (this.Sort)
            {
                case PropertySort.Category:

                    var query2 = from property in query
                                 group property by property.Category into g
                                 orderby g.Min(p => p.DisplayOrder)
                                 from prop2 in g
                                 select prop2;
                    return query2;

                case PropertySort.DisplayOrder:
                    return query;
                   
                case PropertySort.Alphabetical:
                    return query.OrderBy(p => p.Name);
                   
                default:
                    throw new NotSupportedException();
            }
                   
        }
    }


    
}