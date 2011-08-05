using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace Syncfusion.Windows.Tools.Controls
{
    public static class VisualUtil
    {
        /// <summary>
        /// Finds the ancestor.
        /// </summary>
        /// <param name="startingfrom">The startingfrom.</param>
        /// <param name="ancestortype">The ancestortype.</param>
        /// <returns></returns>
        public static DependencyObject FindAncestor(DependencyObject startingfrom, Type ancestortype)
        {
            var item = VisualTreeHelper.GetParent(startingfrom);

            while (item != null && !(item.GetType() == ancestortype))
            {
                if (item is DependencyObject)
                {
                    item = VisualTreeHelper.GetParent(item);
                }
                else
                {
                    break;
                }
            }
            return item as DependencyObject;
        }

        public static IEnumerable<DependencyObject> GetVisualDescendants(this DependencyObject element)
        {
            int childCount = VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(element, i);
                yield return child;
                foreach (var descendant in child.GetVisualDescendants())
                {
                    yield return descendant;
                }
            }
        }
    }
}
