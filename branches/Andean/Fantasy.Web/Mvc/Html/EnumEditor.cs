using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fantasy.Web.Mvc.Html
{
    public class EnumEditor : SingleSelectList
    {
        public override string PropertyName
        {
            get
            {
                return base.PropertyName;
            }
            protected internal set
            {


                base.PropertyName = value;
                this.AddItems();
            }
        }

        private void AddItems()
        {
            var query = from ev in this.PropertyDescriptor.ReferencedEnum.EnumValues
                        select new { Value = (Int32)ev.Value, Text = ev.Name };
            this.Items.AddRange(query);

            this.OptionsText = "Text";
            this.OptionsValue = "Value";
                          
        }

    }
}