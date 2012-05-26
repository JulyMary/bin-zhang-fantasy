using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.BusinessEngine.Services;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.EntityExtensions;

namespace Fantasy.Studio.BusinessEngine
{
    public class CategoryNameProvider : ServiceBase
    {

        public void AddCategory(string category)
        {
            if (!string.IsNullOrWhiteSpace(category))
            {
                int pos = this.CategoryNames.BinarySearch(category);
                if (pos < 0)
                {
                    this.CategoryNames.Insert(~pos, category);
                }
            }
        }


        private List<string> _categoryNames;
        public List<string> CategoryNames
        {
            get
            {

                if (_categoryNames == null)
                {
                    IEntityService es = this.Site.GetRequiredService<IEntityService>();
                    var q1 =  (from prop in es.Query<BusinessProperty>().ToArray()
                                from cate in prop.Extensions.OfType<Category>()
                                where cate.Value != null 
                                select cate.Value).Distinct();
                    var q2 = (from assn in es.Query<BusinessAssociation>().ToArray()
                             let ex = assn.LeftExtensions.Union(assn.RightExtensions)
                             from cate in ex.OfType<Category>()
                             where cate.Value != null
                             select cate.Value).Distinct();

                    var q3 = from name in q1.Union(q2)
                             orderby name
                             select name;

                    this._categoryNames = q3.Distinct().ToList();

                }

                return _categoryNames;
            }
        }
    }
}
