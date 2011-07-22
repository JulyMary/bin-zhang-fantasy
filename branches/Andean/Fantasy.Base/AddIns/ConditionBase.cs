﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Fantasy.AddIns
{
    public abstract class ConditionBase : DependencyObject, ICondition
    {
        public ConditionFailedAction Action
        {
            get { return (ConditionFailedAction)GetValue(ActionProperty); }
            set { SetValue(ActionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Action.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActionProperty =
            DependencyProperty.Register("Action", typeof(ConditionFailedAction), typeof(ConditionBase), new PropertyMetadata(ConditionFailedAction.Exclude));

        public abstract bool IsValid(object caller);
      
    }
}