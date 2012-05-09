using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fantasy.BusinessEngine;

namespace Fantasy.Web.Mvc.Html
{
    partial class PropertyGrid
    {

        


        public PropertySort Sort { get; set; }


        private IEnumerable<BusinessPropertyDescriptor> GetSortedProperties(BusinessObjectRender<BusinessObject> render)
        {
            var query = from property in render.Descriptor.Properties where property.CanRead && property.MemberType == BusinessObjectMemberTypes.Property select property;

            switch (this.Sort)
            {
                case PropertySort.Category:
                   
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