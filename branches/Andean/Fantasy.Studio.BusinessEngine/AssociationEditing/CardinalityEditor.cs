using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Descriptor;

namespace Fantasy.Studio.BusinessEngine.AssociationEditing
{
    public class CardinalityEditor : ListBoxDropDownTypeEditor, IObjectWithSite
    {

        private static readonly object[] _items = new object[] { "0..1", "1", "*", "1..*" };

        protected override object[] Items
        {
            get { return _items; }
        }

        public IServiceProvider Site { get; set; }

    }
}
