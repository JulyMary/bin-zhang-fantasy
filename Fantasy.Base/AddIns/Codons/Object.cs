using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows;

namespace Fantasy.AddIns
{
    [ContentProperty("Instance")]
    public class Object : CodonBase
    {
        public object Instance
        {
            get { return (object)GetValue(InstanceProperty); }
            set { SetValue(InstanceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Instance.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InstanceProperty =
            DependencyProperty.Register("Instance", typeof(object), typeof(Object), new PropertyMetadata(null));

        

        public override object BuildItem(object owner, System.Collections.IEnumerable subItems, ConditionCollection condition)
        {
            return Instance;
        }
    }
}
