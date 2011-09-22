using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using Fantasy.Windows;

namespace Fantasy.Studio.BusinessEngine.ApplicationEditing
{
    public class VerticalTreeViewItem : TreeViewItem
    {




        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.IsRoot = ItemsControl.ItemsControlFromItemContainer(this) is TreeView;
           

        }

        private void RefreshItemPosition()
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                VerticalTreeViewItem child = (VerticalTreeViewItem)this.ItemContainerGenerator.ContainerFromItem(this.Items[i]);
                if (child != null)
                {
                    child.IsFirstItem = i == 0;
                    child.IsLastItem = i == this.Items.Count - 1;
                }
            }
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            RefreshItemPosition();
            
        }


        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
            if (oldParent != null)
            {
                VerticalTreeViewItem container = oldParent.GetAncestor<ItemsControl>() as VerticalTreeViewItem;
                if (container != null)
                {
                    container.RefreshItemPosition();
                }
            }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new VerticalTreeViewItem();
        }


        public bool IsRoot
        {
            get { return (bool)GetValue(IsRootProperty); }
            private set { SetValue(IsRootPropertyKey, value); }
        }

        // Using a DependencyProperty as the backing store for IsRoot.  This enables animation, styling, binding, etc...
        internal static readonly DependencyPropertyKey IsRootPropertyKey =
            DependencyProperty.RegisterReadOnly("IsRoot", typeof(bool), typeof(VerticalTreeViewItem), new UIPropertyMetadata(false));

        public static readonly DependencyProperty IsRootProperty = IsRootPropertyKey.DependencyProperty;




        public bool IsFirstItem
        {
            get { return (bool)GetValue(IsFirstItemProperty); }
            private set { SetValue(IsFirstItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsFirstItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsFirstItemProperty =
            DependencyProperty.Register("IsFirstItem", typeof(bool), typeof(VerticalTreeViewItem), new UIPropertyMetadata(false));




        public bool IsLastItem
        {
            get { return (bool)GetValue(IsLastItemProperty); }
            private set { SetValue(IsLastItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsLastItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLastItemProperty =
            DependencyProperty.Register("IsLastItem", typeof(bool), typeof(VerticalTreeViewItem), new UIPropertyMetadata(false));



    }
}
