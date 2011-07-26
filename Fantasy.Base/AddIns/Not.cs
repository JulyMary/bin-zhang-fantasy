using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows;
namespace Fantasy.AddIns
{
    [ContentProperty("Condition")]
    public class Not : ConditionBase
    {
        public override bool IsValid(object args)
        {
            if (Condition != null)
            {
                return !ConditionBase.IsValidWithMember(Condition, args);
            }
            else
            {
                return true;
            }
        }

      



        public ICondition Condition
        {
            get { return (ICondition)GetValue(ConditionProperty); }
            set { SetValue(ConditionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Condition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConditionProperty =
            DependencyProperty.Register("Condition", typeof(ICondition), typeof(Not), new PropertyMetadata(null));

        
    }
}
