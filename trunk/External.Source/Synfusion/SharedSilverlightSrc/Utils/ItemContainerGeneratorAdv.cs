﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Syncfusion.Windows.Shared
{
    public class ItemContainerGeneratorAdv
    {
        private ItemsControl parent
        {
            get;
            set;
        }
        public IDictionary<DependencyObject, object> ChildrenToItems
        {   get;
            set;
        }
        private Panel itemsHost;

        internal Panel ItemsHost
        {
            get
            {

                if (itemsHost == null)
                {

                    if (ChildrenToItems.Count <= 0)
                    {
                        return null;
                    }
                    DependencyObject treeviewitem = ChildrenToItems.First().Key;


                    itemsHost = VisualTreeHelper.GetParent(treeviewitem) as Panel;
                }

                return itemsHost;
            }
        }

        public void  ClearItems()
        {
            itemsHost = null;
            ChildrenToItems.Clear();
        }

        public ItemContainerGeneratorAdv(ItemsControl itemscontrol)
        {
            this.parent = itemscontrol;
            ChildrenToItems = new Dictionary<DependencyObject, object>();
        }

        private Binding GetBinding(HierarchicalDataTemplate template,HeaderedItemsControl item)
        {
            Binding binding = new Binding();
            binding.Converter = template.ItemsSource.Converter;
            binding.ConverterCulture = template.ItemsSource.ConverterCulture;
            binding.ConverterParameter = template.ItemsSource.ConverterParameter;
            binding.Mode = template.ItemsSource.Mode;
            binding.NotifyOnValidationError = template.ItemsSource.NotifyOnValidationError;
            binding.Path = template.ItemsSource.Path;
            binding.Source = item.Header;
            binding.ValidatesOnExceptions = template.ItemsSource.ValidatesOnExceptions;
            return binding;
        }

        public void ApplyPropertiesTochild(DependencyObject element, object item, Style parentItemContainerStyle)
        {

            if (element == null)
            {
                return;
            }

            ChildrenToItems[element] = item;

            HeaderedItemsControl child = element as HeaderedItemsControl;
            if (child != null)
            {
                if (child != item)
                {
                    
                   
                    DataTemplate parentItemTemplate = parent.ItemTemplate;
                    if (parentItemTemplate != null)
                    {
                        child.SetValue(HeaderedItemsControl.ItemTemplateProperty, parentItemTemplate);
                    }

                    
                    if (parentItemContainerStyle != null)
                    {
                        child.SetValue(HeaderedItemsControl.ItemContainerStyleProperty, parentItemContainerStyle);
                    }


                    child.Header = item;
                        

                    
                    if (parentItemTemplate != null)
                    {
                        child.SetValue(HeaderedItemsControl.HeaderTemplateProperty, parentItemTemplate);
                    }

                    
                    if (parentItemContainerStyle != null && child.Style == null)
                    {
                        child.SetValue(HeaderedItemsControl.StyleProperty, parentItemContainerStyle);
                    }

                  
                    
                    HierarchicalDataTemplate hierarchicaltemplate = parentItemTemplate as HierarchicalDataTemplate;
                    if (hierarchicaltemplate != null)
                    {
                        if (hierarchicaltemplate.ItemsSource != null)
                        {
                            child.SetBinding(HeaderedItemsControl.ItemsSourceProperty, GetBinding(hierarchicaltemplate, child));

                           
                        }

                    }
                }
            }
        }

       



        public DependencyObject childfromindex(int index)
        {
            Panel panel = ItemsHost;
            if (panel == null || panel.Children == null || index < 0 || index >= panel.Children.Count)
            {
                return null;
            }

            return panel.Children[index];
        }


        public DependencyObject childfromitem(object item)
        {

            foreach (KeyValuePair<DependencyObject, object> mapping in ChildrenToItems)
            {
                if (object.Equals(item, mapping.Value))
                {
                    return mapping.Key;
                }
            }
            return null;
        }


        public int indexfromchild(DependencyObject child)
        {
            if (child == null)
            {
                throw new ArgumentNullException();
            }

            UIElement element = child as UIElement;
            if (element == null)
            {
                return -1;
            }

            Panel panel = ItemsHost;
            if (panel == null || panel.Children == null)
            {
                return -1;
            }

            return panel.Children.IndexOf(element);
        }


        public object itemfromchild(DependencyObject child)
        {
            if (child == null)
            {
                return null;
            }

            object item = child;
            ChildrenToItems.TryGetValue(child, out item);
            return item;
        }

        public void UpdateItemContainerStyle(Style itemContainerStyle)
        {
            if (itemContainerStyle == null)
            {
                return;
            }

            Panel itemsHost = ItemsHost;
            if (itemsHost == null || itemsHost.Children == null)
            {
                return;
            }

            foreach (UIElement element in itemsHost.Children)
            {
                FrameworkElement obj = element as FrameworkElement;
                if (obj.Style == null)
                {
                    obj.Style = itemContainerStyle;
                }
            }
        }

       

    }
}
