using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Syncfusion.Windows.Diagram
{
    partial class Node
    {

        public static bool GetIsSelectionScope(UIElement obj)
        {
            return (bool)obj.GetValue(IsSelectionScopeProperty);
        }

        public static void SetIsSelectionScope(UIElement obj, bool value)
        {
            obj.SetValue(IsSelectionScopeProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsSelectionScope.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSelectionScopeProperty =
            DependencyProperty.RegisterAttached("IsSelectionScope", typeof(bool), typeof(Node), new UIPropertyMetadata(false));

        public static bool IsInSelectionScope(Node node, DependencyObject child)
        {
            bool rs = false;
            while (child != null && ! object.Equals(node, child) && !rs)
            {
                UIElement ui = child as UIElement;
                if (ui != null)
                {
                    rs = Node.GetIsSelectionScope(ui);
                }

                if (child is Visual || child is System.Windows.Media.Media3D.Visual3D)
                {
                    child = VisualTreeHelper.GetParent(child);
                }
                else
                {
                    child = LogicalTreeHelper.GetParent(child);
                }
            }

            return rs;

        }


    }
}
