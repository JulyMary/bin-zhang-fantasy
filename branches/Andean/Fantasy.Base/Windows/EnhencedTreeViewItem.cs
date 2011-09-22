using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Controls;
using Fantasy.Windows;
using System.Windows;
using System.Collections.Specialized;

namespace Fantasy.Windows
{
    public static class EnhencedTreeViewItem 
    {

        static EnhencedTreeViewItem()
        {
            _siblingChanged = new WeakEventListener((type, sender, e) =>
                {
                    TreeViewItem[] array = ((ItemCollection)sender).Cast<TreeViewItem>().ToArray();
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i].SetValue(IsFirstItemKey, i == 0);
                        
                    }
                    return true;
                });
        }

        public static readonly DependencyPropertyKey IsRootPropertyKey = DependencyProperty.RegisterAttachedReadOnly("IsRoot", typeof(Boolean),typeof(EnhencedTreeViewItem), new UIPropertyMetadata(false));
       
        public static Boolean GetIsRoot(DependencyObject item)
        {
            PrepareValue((TreeViewItem)item);

            return (Boolean)item.GetValue(IsRootPropertyKey.DependencyProperty);
        }


        public static readonly DependencyPropertyKey IsFirstItemKey = DependencyProperty.RegisterAttachedReadOnly("IsFirstItem", typeof(bool), typeof(EnhencedTreeViewItem), new UIPropertyMetadata(false));

        public static bool GetIsFirstItem(TreeViewItem item)
        {
            PrepareValue(item);
            return (bool)item.GetValue(IsFirstItemKey.DependencyProperty); 
        }

        public static readonly DependencyPropertyKey IsLastItemKey = DependencyProperty.RegisterAttachedReadOnly("IsLastItem", typeof(bool), typeof(EnhencedTreeViewItem), new UIPropertyMetadata(false));

        public static bool GetIsLastItem(TreeViewItem item)
        {
            PrepareValue(item);
            return (bool)item.GetValue(IsLastItemKey.DependencyProperty);
        }

        private static void PrepareValue(TreeViewItem item)
        {
            if (item.Parent == null)
            {
                item.Loaded += new RoutedEventHandler(TreeViewItemLoaded);
                item.Unloaded += new RoutedEventHandler(TreeViewItem_Unloaded);
            }
            else
            {
                EvaluateValues(item);
            }
        }

        private static void EvaluateValues(TreeViewItem item)
        {
            ItemsControl container = item.Parent.GetAncestor<ItemsControl>();
            CollectionChangedEventManager.AddListener(container.Items, _siblingChanged);
            item.SetValue(IsRootPropertyKey, !(container is TreeViewItem));
            item.SetValue(IsFirstItemKey, container.Items.Cast<TreeViewItem>().FirstOrDefault() == item);
            item.SetValue(IsLastItemKey, container.Items.Cast<TreeViewItem>().LastOrDefault() == item);
        }

        static void TreeViewItem_Unloaded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            item.Loaded -= new RoutedEventHandler(TreeViewItemLoaded);
            item.Unloaded -= new RoutedEventHandler(TreeViewItem_Unloaded);
        }


        private static WeakEventListener _siblingChanged;

        static void TreeViewItemLoaded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            EvaluateValues(item);

        }

    }
}
