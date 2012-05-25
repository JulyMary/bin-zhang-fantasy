using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Descriptor;
using Fantasy.BusinessEngine.Services;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.EntityExtensions;

namespace Fantasy.Studio.BusinessEngine.PropertyEditing
{
    public class PropertyCategoryEditor : ListBoxDropDownTypeEditor, IObjectWithSite
    {

        public IServiceProvider Site { get; set; }


        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {

            string str = (string)value;
            if (!string.IsNullOrWhiteSpace(str))
            {
                int pos = this.InnerItems.BinarySearch(str);
                if (pos < 0)
                {
                    this.InnerItems.Insert(~pos, str);
                }
            }
            return  (string) base.EditValue(context, provider, value);
            
            
        }

        protected override object[] Items
        {
            get 
            {
                return this.InnerItems.ToArray();
            }
        }


        private static List<string> _innerItems = null;
        public List<string> InnerItems
        {
            get
            {
                if (_innerItems == null)
                {
                    IEntityService es = this.Site.GetRequiredService<IEntityService>();
                    var query = from prop in es.Query<BusinessProperty>().ToArray()
                                from cate in prop.Extensions.OfType<Category>()
                                where cate.Value != null orderby cate.Value
                                select cate.Value;

                    _innerItems = query.Distinct().ToList();
                }
                return _innerItems;
            }
        }
    }
}
