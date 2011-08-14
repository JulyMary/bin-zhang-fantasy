using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows;

namespace Fantasy.AddIns
{
    [ContentProperty("Conditions")]
    public class Or : ConditionBase
    {
        public Or()
        {
            this.SetValue(ConditionsProperty, new List<ICondition>());
        }

        public override bool IsValid(object args, IServiceProvider services)
        {
            if (Conditions != null)
            {
                foreach (ICondition child in Conditions)
                {
                    if (ConditionBase.IsValidWithMember(child, args, services))
                    {
                        return true;
                    }
                }

                return false;
            }
            else
            {
                return true;
            }
        }

        public IList<ICondition> Conditions
        {
            get { return (IList<ICondition>)GetValue(ConditionsProperty.DependencyProperty); }
           
        }

        // Using a DependencyProperty as the backing store for Conditions.  This enables animation, styling, binding, etc...
        public static readonly DependencyPropertyKey ConditionsProperty =
            DependencyProperty.RegisterReadOnly("Conditions", typeof(IList<ICondition>), typeof(Or), new PropertyMetadata(new List<ICondition>()));

    }
}
