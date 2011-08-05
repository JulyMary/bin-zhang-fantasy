using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections;

namespace Syncfusion.Windows.Tools.Controls
{
#if !WPF
    public class ComboBoxItemAdv : ComboBoxItem
#else
    public class ComboBoxItemAdv : ComboBoxItem
#endif
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComboBoxItemAdv"/> class.
        /// </summary>
        public ComboBoxItemAdv()
        {
            DefaultStyleKey = typeof(ComboBoxItemAdv);
            
        }

        internal new ComboBoxAdv Parent;

        internal CheckBox CheckBox;


        public new bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSelected.  This enables animation, styling, binding, etc...
        public new static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(ComboBoxItemAdv), new PropertyMetadata(false, new PropertyChangedCallback(OnIsSelectedChanged)));


        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Element.MouseLeftButtonDown"/> routed event is raised on this element. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs"/> that contains the event data. The event data reports that the left mouse button was pressed.</param>
        protected override void OnMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (!IsSelected)
            {
                IsSelected = true;
                base.IsSelected = true;
                if (!Parent.AllowMultiSelect)
                {
                    for (int i = 0; i < Parent.Items.Count; i++)
                    {
                        ComboBoxItemAdv boxItem = Parent.ItemContainerGenerator.ContainerFromIndex(i) as ComboBoxItemAdv;
                        if (boxItem == null)
                            boxItem = Parent.Items[i] as ComboBoxItemAdv;
                        if (boxItem != null && boxItem != this)
                        {
                            boxItem.IsSelected = false;
                        }
                    }
                   
                }
                
            }
            else
            {
              
              
                IList selItems = Parent.SelectedItems as IList;
                if (selItems != null)
                {
                    object item = Parent.ItemContainerGenerator.ItemFromContainer(this) as object;
                    if (item != null)
                    {
                        if (selItems.Contains(item))
                        {
                            selItems.Remove(item);
                        }
                    }
                }
                IsSelected = false;
                base.IsSelected = false;
            }
            if (!Parent.AllowMultiSelect)
            {
                if (Parent.IsDropDownOpen)
                {
                    Parent.IsDropDownOpen = false;
                }
            }
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            VisualStateManager.GoToState(this, "Focussed", true);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            CheckBox = GetTemplateChild("PART_CheckBox") as CheckBox;

            if (Parent != null)
            {
                Parent.UpdateSelectMode();
                if (Parent.SelectedItems != null && Parent.AllowMultiSelect)
                {
                    IList selItems = Parent.SelectedItems as IList;
                    
                    foreach (var item in selItems)
                    {
                        ComboBoxItemAdv boxItem = Parent.ItemContainerGenerator.ContainerFromItem(item) as ComboBoxItemAdv;
                        if (boxItem != null)
                        {
                            boxItem.IsSelected = true;
                        }
                    }
                }
                    
            }

        }

        protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseMove(e);
#if !WPF
            if (Parent != null)
            {
                Parent.Items.ToList().ForEach(i =>
                    {
                        ComboBoxItemAdv item=Parent.ItemContainerGenerator.ContainerFromItem(i) as ComboBoxItemAdv;
                        VisualStateManager.GoToState(item, "Normal", true);
                    });
            }
#endif
            VisualStateManager.GoToState(this, "MouseOver", true);
        }

        protected override void OnMouseLeave(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            VisualStateManager.GoToState(this, "Normal", true);
        }

        private static void OnIsSelectedChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            ComboBoxItemAdv instance = sender as ComboBoxItemAdv;
            if (instance != null)
            {
                if (instance.Parent != null)
                {
                    if (instance.Parent.SelectedItems != null)
                    {
                        IList selItems = instance.Parent.SelectedItems as IList;
                        // selItems.Clear();
                        for (int i = 0; i < instance.Parent.Items.Count; i++)
                        {
                            ComboBoxItemAdv boxItem = instance.Parent.ItemContainerGenerator.ContainerFromIndex(i) as ComboBoxItemAdv;
                            if (boxItem != null)
                            {
                                if (boxItem.IsSelected)
                                {
                                    VisualStateManager.GoToState(boxItem, "Selected", true);
                                    if (!selItems.Contains(instance.Parent.Items[i]))
                                    {
                                        selItems.Add(instance.Parent.Items[i]);
                                        instance.Parent.SelectedIndex = i;
                                    }
                                }
                                else
                                {

                                    VisualStateManager.GoToState(boxItem, "Unselected", true);
                                }
                            }
                        }
                    }
                    instance.Parent.UpdateSelectionBox();
                }
            }
        }

    }
}


