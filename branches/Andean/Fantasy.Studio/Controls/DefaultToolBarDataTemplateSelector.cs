using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace Fantasy.Studio.Controls
{
    public class DefaultToolBarDataTemplateSelector : DataTemplateSelector
    {
        public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null && item is ButtonModel)
            {
                ButtonModel model = item as ButtonModel;
                string name = null;
                if (model.Text == "-")
                {
                    name = "seperator";
                }
                else if (model.IsCheckable)
                {
                    name = "toggleButton";
                }
                else
                {
                    name = "button";
                }

                return (System.Windows.DataTemplate)element.FindResource(name);
               
            }

            return null;

        }
    }
}
