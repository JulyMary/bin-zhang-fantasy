using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Fantasy.Studio.BusinessEngine.ObjectSecurityEditing
{
    class CheckBoxEx : CheckBox
    {

        public static readonly RoutedEvent IsCheckedChangedEvent = EventManager.RegisterRoutedEvent("IsCheckedChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CheckBoxEx));

        public event RoutedEventHandler IsCheckedChanged
        {
            add { AddHandler(IsCheckedChangedEvent, value); }
            remove { RemoveHandler(IsCheckedChangedEvent, value); }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
           

            base.OnPropertyChanged(e);

            if (e.Property == ToggleButton.IsCheckedProperty)
            {
                RoutedEventArgs args = new RoutedEventArgs(IsCheckedChangedEvent);
                RaiseEvent(args);
            }
        }
    }
}
