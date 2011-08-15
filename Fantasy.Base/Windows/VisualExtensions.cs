using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

namespace Fantasy.Windows
{
    public static class VisualExtensions
    {

        private static DependencyObject GetAncestor(DependencyObject childObject, Type parentType, Visual top)
        {
            DependencyObject child = childObject;
            while ((child != null) && !(parentType.IsInstanceOfType(child)) && child != top)
            {
                // VisualTreeHelper works with objects of type Visual or Visual3D.
                // If the current object is not derived from Visual or Visual3D,
                // then use the LogicalTreeHelper to find the parent element.
                if (child is Visual || child is System.Windows.Media.Media3D.Visual3D)
                    child = VisualTreeHelper.GetParent(child);
                else
                    child = LogicalTreeHelper.GetParent(child);
            }

            return parentType.IsInstanceOfType(child) ? child : null;
        }

        public static object GetAncestor(this DependencyObject childObject, Type parentType)
        {
            return GetAncestor(childObject, parentType, null);
        }

        public static T GetAncestor<T>(this DependencyObject childObject) where T : DependencyObject
        {
            return (T)GetAncestor(childObject, typeof(T));
        }

        public static bool IsMouseOver(this Visual reference, Type childType)
        {
            Point ptMouse = MouseUtilities.GetMousePosition(reference);
            HitTestResult res = VisualTreeHelper.HitTest(reference, ptMouse);
            if (res == null)
                return false;
            DependencyObject depObj = res.VisualHit;
            return GetAncestor(depObj, childType, reference) != null;
        }

        public static bool IsMouseOver<T>(this Visual reference) where T: Visual 
        {
            return IsMouseOver(reference, typeof(T));
        }

        public static object GetObjectAtPoint<ItemContainer>(this ItemsControl control, Point p) where ItemContainer : DependencyObject 
        {        
            // ItemContainer - can be ListViewItem, or TreeViewItem and so on(depends on control)
            ItemContainer obj = GetContainerAtPoint<ItemContainer>(control, p); 
            if (obj == null)            
                return null;        
            return control.ItemContainerGenerator.ItemFromContainer(obj);    
        }    
        public static ItemContainer GetContainerAtPoint<ItemContainer>(this ItemsControl control, Point p) where ItemContainer : DependencyObject
        {        
            HitTestResult result = VisualTreeHelper.HitTest(control, p);       
            DependencyObject obj = result.VisualHit;
            return (ItemContainer)GetAncestor<ItemContainer>(obj);
                 
        }
        
    }
}
