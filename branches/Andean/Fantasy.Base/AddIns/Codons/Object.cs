using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows;

namespace Fantasy.AddIns
{
    [System.Windows.Markup.ContentProperty("Template")]
    public class Object : ObjectBuilder, ICodon
    {

        #region ICodon Members

        public ICondition Conditional
        {
            get;
            set;
        }

        public bool HandleCondition
        {
            get { return false; }
        }

        public string ID
        {
            get;
            set;
        }

        public string InsertAfter
        {
            get;
            set;
        }

        public string InsertBefore
        {
            get;
            set;
        }

        public object BuildItem(object owner, System.Collections.IEnumerable subItems, ConditionCollection condition)
        {
            return base.Build();
        }

        private IList<ICodon> _codons = new List<ICodon>();
        IList<ICodon> ICodon.Codons
        {
            get { return _codons; }
        }

        #endregion
    }
}
