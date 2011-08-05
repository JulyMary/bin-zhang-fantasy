#region Copyright
// Copyright Syncfusion Inc. 2001 - 2011. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion

namespace Syncfusion.Windows.Diagram
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Ink;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;
    using Syncfusion.Windows.Diagram;

    /// <summary>
    /// A Class having a single color and its variants colors.
    /// </summary>
#if SyncfusionFramework4_0
    [DesignTimeVisible(false)]
#endif
    public class SymbolPaletteItem : ContentControl
    {
        Border border;

        Border outerborder;
        /// <summary>
        /// Identifies the <see cref="BorderMargin"/>  dependency property.
        /// </summary>
        public static readonly DependencyProperty BorderMarginProperty = DependencyProperty.Register("BorderMargin", typeof(Thickness), typeof(SymbolPaletteItem), new PropertyMetadata(new Thickness(0, 0, 0, 0)));
       
        private Point? dragStartPoint = null;

        /// <summary>
        /// Identifies the <see cref="BorderHeight"/>  dependency property.
        /// </summary>
        public static new readonly DependencyProperty HeightProperty = DependencyProperty.Register("BorderHeight", typeof(double), typeof(SymbolPaletteItem), new PropertyMetadata(0d));
               
        /// <summary>
        /// Identifies the ItemBorderThickness dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemBorderThicknessProperty =
        DependencyProperty.Register("ItemBorderThickness", typeof(Thickness), typeof(SymbolPaletteItem), new PropertyMetadata(new Thickness(1)));

        /// <summary>
        /// Identifies the ItemCheckedBorderBrush dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemCheckedBorderBrushProperty =
        DependencyProperty.Register("ItemCheckedBorderBrush", typeof(Brush), typeof(SymbolPaletteItem), new PropertyMetadata(new SolidColorBrush(Colors.Red)));

        /// <summary>
        /// Identifies the ItemCheckedMouseOverBorderBrush dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemCheckedMouseOverBorderBrushProperty =
        DependencyProperty.Register("ItemCheckedMouseOverBorderBrush", typeof(Brush), typeof(SymbolPaletteItem), new PropertyMetadata(new SolidColorBrush(Colors.Green)));

        /// <summary>
        /// Identifies the ItemMouseOverBorderBrush dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemMouseOverBorderBrushProperty =
        DependencyProperty.Register("ItemMouseOverBorderBrush", typeof(Brush), typeof(SymbolPaletteItem), new PropertyMetadata(new SolidColorBrush(Colors.Orange)));
        
        /// <summary>
        /// Identifies the ItemCheckedBorderBrush dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemCheckedBackgroundBrushProperty =
        DependencyProperty.Register("ItemCheckedBackgroundBrush", typeof(Brush), typeof(SymbolPaletteItem), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        /// <summary>
        /// Identifies the ItemCheckedMouseOverBorderBrush dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemCheckedMouseOverBackgroundBrushProperty =
        DependencyProperty.Register("ItemCheckedMouseOverBackgroundBrush", typeof(Brush), typeof(SymbolPaletteItem), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        /// <summary>
        /// Identifies the ItemMouseOverBorderBrush dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemMouseOverBackgroundBrushProperty =
        DependencyProperty.Register("ItemMouseOverBackgroundBrush", typeof(Brush), typeof(SymbolPaletteItem), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        /// <summary>
        /// Identifies the ItemCornerRadius dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemCornerRadiusProperty =
        DependencyProperty.Register("ItemCornerRadius", typeof(CornerRadius), typeof(SymbolPaletteItem), new PropertyMetadata(new CornerRadius(2)));

        /// <summary>
        /// Identifies the <see cref="ItemMargin"/>  dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemMarginProperty = DependencyProperty.Register("ItemMargin", typeof(Thickness), typeof(SymbolPaletteItem), new PropertyMetadata(new Thickness(2, 0, 2, 0)));

        /// <summary>
        /// Identifies the ItemPadding dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemPaddingProperty =
        DependencyProperty.Register("ItemPadding", typeof(Thickness), typeof(SymbolPaletteItem), new PropertyMetadata(new Thickness(0)));

        /// <summary>
        /// Identifies the PathData dependency property.
        /// </summary>
        public static readonly DependencyProperty PathDataProperty = DependencyProperty.Register("PathData", typeof(string), typeof(SymbolPaletteItem), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Identifies the PathData dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemNameProperty = DependencyProperty.Register("ItemName", typeof(string), typeof(SymbolPaletteItem), new PropertyMetadata(string.Empty));
        
        /// <summary>
        /// Identifies the <see cref="BorderWidth"/>  dependency property.
        /// </summary>
        public static new readonly DependencyProperty WidthProperty = DependencyProperty.Register("BorderWidth", typeof(double), typeof(SymbolPaletteItem), new PropertyMetadata(0d));

        internal bool SelectedFlag = false;

        internal bool m_IsMouseOver;

        internal bool IsSelected;

        private static bool mhasvalue = false;
        static SymbolPaletteItem()
        {
#if WPF
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SymbolPaletteItem), new FrameworkPropertyMetadata(typeof(SymbolPaletteItem)));
#endif
        }
        /// <summary>
        /// Creates the instance of SymbolPaletteItem control
        /// </summary>
        public SymbolPaletteItem()
        {
#if SILVERLIGHT
            DefaultStyleKey = typeof(SymbolPaletteItem);
#endif
#if WPF
            this.SizeChanged += new SizeChangedEventHandler(SymbolPaletteItem_SizeChanged);
#endif
        }

        /// <summary>
        /// Gets or sets the value of the BorderHeight dependency property.
        /// </summary>
        public double BorderHeight
        {
            get
            {
                return (double)GetValue(HeightProperty);
            }

            set
            {
                SetValue(HeightProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of the BorderMargin dependency property
        /// </summary>
        public Thickness BorderMargin
        {
            get
            {
                return (Thickness)GetValue(BorderMarginProperty);
            }

            set
            {
                SetValue(BorderMarginProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of the BorderThickness dependency property
        /// </summary>
        public Thickness BorderThick
        {
            get
            {
                return (Thickness)GetValue(BorderThicknessProperty);
            }

            set
            {
                SetValue(BorderThicknessProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of the BorderWidth dependency property.
        /// </summary>
        public double BorderWidth
        {
            get
            {
                return (double)GetValue(WidthProperty);
            }

            set
            {
                SetValue(WidthProperty, value);
            }
        }

        internal static bool Hasvalue
        {
            get { return mhasvalue; }
            set { mhasvalue = value; }
        }

        /// <summary>
        /// Gets or sets the border brush when the item is checked.
        /// </summary>
        /// <value>The item checked border brush.</value>
        /// <remarks>
        /// Default value is Red.
        /// </remarks>
        /// <example>
        /// C#:
        /// <code language="C#">
        /// DiagramControl diagramControl=new DiagramControl();
        /// diagramControl.SymbolPalette.ItemCheckedBorderBrush=Brushes.Blue;
        /// </code>
        /// </example>
        public Brush ItemCheckedBorderBrush
        {
            get
            {
                return (Brush)GetValue(ItemCheckedBorderBrushProperty);
            }

            set
            {
                SetValue(ItemCheckedBorderBrushProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets border brush when the mouse is over the checked item.
        /// </summary>
        /// <value>The item checked mouse over border brush.</value>
        /// <remarks>
        /// Default value is Green.
        /// </remarks>
        /// <example>
        /// C#:
        /// <code language="C#">
        /// DiagramControl diagramControl=new DiagramControl();
        /// diagramControl.SymbolPalette.ItemCheckedMouseOverBorderBrush=Brushes.Violet;
        /// </code>
        /// </example>
        public Brush ItemCheckedMouseOverBorderBrush
        {
            get
            {
                return (Brush)GetValue(ItemCheckedMouseOverBorderBrushProperty);
            }

            set
            {
                SetValue(ItemCheckedMouseOverBorderBrushProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the border brush when the mouse is over the checked item..
        /// </summary>
        /// <value>The item mouse over border brush.</value>
        /// <remarks>
        /// Default value is Orange.
        /// </remarks>
        /// <example>
        /// C#:
        /// <code language="C#">
        /// DiagramControl diagramControl=new DiagramControl();
        /// diagramControl.SymbolPalette.ItemMouseOverBorderBrush=Brushes.Beige;
        /// </code>
        /// </example>
        public Brush ItemMouseOverBorderBrush
        {
            get
            {
                return (Brush)GetValue(ItemMouseOverBorderBrushProperty);
            }

            set
            {
                SetValue(ItemMouseOverBorderBrushProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the border brush when the item is checked.
        /// </summary>
        /// <value>The item checked border brush.</value>
        /// <remarks>
        /// Default value is Red.
        /// </remarks>
        /// <example>
        /// C#:
        /// <code language="C#">
        /// DiagramControl diagramControl=new DiagramControl();
        /// diagramControl.SymbolPalette.ItemCheckedBorderBrush=Brushes.Blue;
        /// </code>
        /// </example>
        public Brush ItemCheckedBackgroundBrush
        {
            get
            {
                return (Brush)GetValue(ItemCheckedBackgroundBrushProperty);
            }

            set
            {
                SetValue(ItemCheckedBackgroundBrushProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets border brush when the mouse is over the checked item.
        /// </summary>
        /// <value>The item checked mouse over border brush.</value>
        /// <remarks>
        /// Default value is Green.
        /// </remarks>
        /// <example>
        /// C#:
        /// <code language="C#">
        /// DiagramControl diagramControl=new DiagramControl();
        /// diagramControl.SymbolPalette.ItemCheckedMouseOverBorderBrush=Brushes.Violet;
        /// </code>
        /// </example>
        public Brush ItemCheckedMouseOverBackgroundBrush
        {
            get
            {
                return (Brush)GetValue(ItemCheckedMouseOverBackgroundBrushProperty);
            }

            set
            {
                SetValue(ItemCheckedMouseOverBackgroundBrushProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the border brush when the mouse is over the checked item..
        /// </summary>
        /// <value>The item mouse over border brush.</value>
        /// <remarks>
        /// Default value is Orange.
        /// </remarks>
        /// <example>
        /// C#:
        /// <code language="C#">
        /// DiagramControl diagramControl=new DiagramControl();
        /// diagramControl.SymbolPalette.ItemMouseOverBorderBrush=Brushes.Beige;
        /// </code>
        /// </example>
        public Brush ItemMouseOverBackgroundBrush
        {
            get
            {
                return (Brush)GetValue(ItemMouseOverBackgroundBrushProperty);
            }

            set
            {
                SetValue(ItemMouseOverBackgroundBrushProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of the ItemMargin dependency property
        /// </summary>
        public Thickness ItemMargin
        {
            get
            {
                return (Thickness)GetValue(ItemMarginProperty);
            }

            set
            {
                SetValue(ItemMarginProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of the PathData dependency property.
        /// </summary>
        public string PathData
        {
            get
            {
                return (string)GetValue(PathDataProperty);
            }

            set
            {
                SetValue(PathDataProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of the PathData dependency property.
        /// </summary>
        public string ItemName
        {
            get
            {
                return (string)GetValue(ItemNameProperty);
            }

            set
            {
                SetValue(ItemNameProperty, value);
            }
        }


        /// <summary>
        /// Clones the specified obj.
        /// </summary>
        /// <param name="obj">The object to be cloned.</param>
        /// <returns>The cloned object.</returns>
        internal object Clone(object obj, int isPath)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();
            object cloneObj = obj.GetType().GetConstructors()[0].Invoke(null);
            foreach (PropertyInfo property in properties)
            {
                if (!property.Name.Contains("Name"))
                {
                    object value = property.GetValue(obj, null);
                    if (value != null)
                    {
                        try
                        {
                            if (IsPresentationFrameworkCollection(value.GetType()))
                            {
                                object collection = property.GetValue(obj, null);
                                int count = (int)collection.GetType().GetProperty("Count").GetValue(collection, null);
                                for (int i = 0; i < count; i++)
                                {
                                    object child = collection.GetType().GetProperty("Item").GetValue(collection, new object[] { i });
                                    object cloneChild = this.Clone(child, 0);
                                    object cloneCollection = property.GetValue(cloneObj, null);
                                    collection.GetType().InvokeMember("Add", BindingFlags.InvokeMethod, null, cloneCollection, new object[] { cloneChild });
                                }
                            }

                            if (value is UIElement)
                            {
                                object obj2 = property.PropertyType.GetConstructors()[0].Invoke(null);
                                Clone(obj2, 0);
                                property.SetValue(cloneObj, obj2, null);
                            }
                            else if (property.CanWrite)
                            {
                                if (property.ToString().Contains("Data") && isPath > 0)
                                {
                                    PathGeometry geo = value as PathGeometry;
                                    PathGeometry pathgeo = (PathGeometry)this.Clone(geo, 0);
                                    property.SetValue(cloneObj, pathgeo, null);
                                }
                                else
                                {
                                    property.SetValue(cloneObj, value, null);
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }

            return cloneObj;
        }

        /// <summary>
        /// Clones the Path object
        /// </summary>
        /// <param name="element">Element to be clone</param>
        /// <param name="obj">object that is cloned</param>
        /// <returns></returns>
        internal object ClonePath(object element, object obj)
        {
            PropertyInfo[] properties = element.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(element, null);
                if (value != null)
                {
                    try
                    {
                        if (this.IsPresentationFrameworkCollection(value.GetType()))
                        {
                            object collection = property.GetValue(element, null);
                            int count = (int)collection.GetType().GetProperty("Count").GetValue(collection, null);
                            for (int i = 0; i < count; i++)
                            {
                                object child = collection.GetType().GetProperty("Item").GetValue(collection, new object[] { i });
                                object cloneChild = Clone(child, 0);
                                object cloneCollection = property.GetValue(obj, null);
                                collection.GetType().InvokeMember("Add", BindingFlags.InvokeMethod, null, cloneCollection, new object[] { cloneChild });
                            }
                        }

                        if (value is UIElement)
                        {
                            object obj2 = property.PropertyType.GetConstructors()[0].Invoke(null);
                            this.Clone(obj2, 0);
                            property.SetValue(element, obj2, null);
                        }
                        else if (property.CanWrite)
                        {
                            if (!property.ToString().Contains("Data"))
                            {
                                property.SetValue(obj, value, null);
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }

            return obj;
        }
        /// <summary>
        /// Gets the numeric list separator
        /// </summary>
        /// <param name="provider"></param>
        /// <returns>char</returns>
        internal static char GetNumericListSeparator(IFormatProvider provider)
        {
            char numericSeparator = ',';
            NumberFormatInfo numberFormat = NumberFormatInfo.GetInstance(provider);

            if ((numberFormat.NumberDecimalSeparator.Length > 0) && (numericSeparator == numberFormat.NumberDecimalSeparator[0]))
            {
                numericSeparator = ';';
            }

            return numericSeparator;
        }

        /// <summary>
        /// Method to find the parent of given element
        /// </summary>
        /// <param name="element">Element for which parent is to be found</param>
        /// <returns>Parent of type SymbolPaletteGroup</returns>
        internal static SymbolPaletteGroup GetSymbolPaletteGroupFromChildren(FrameworkElement element)
        {
            SymbolPaletteGroup item = null;
            if (element != null)
            {
                item = element as SymbolPaletteGroup;

                if (item == null)
                {
                    while (element != null)
                    {
                        element = VisualTreeHelper.GetParent(element) as FrameworkElement;

                        if (element is SymbolPaletteGroup)
                        {
                            item = (SymbolPaletteGroup)element;
                            break;
                        }
                    }
                }
            }

            return item;
        }
        
        /// <summary>
        /// Called when changes in visual state of border takes place
        /// </summary>
        /// <param name="useTransitions">Indicate whether to apply transition or not</param>
        /// <param name="stateNames">Contain the state name</param>
        internal void GoToState(bool useTransitions, params string[] stateNames)
        {
            if (stateNames != null)
            {
                foreach (string str in stateNames)
                {
                    if (VisualStateManager.GoToState(this, str, useTransitions))
                    {
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether [is presentation framework collection] [the specified type].
        /// </summary>
        /// <param name="type">The type of the object.</param>
        /// <returns>
        /// <c>true</c> if [is presentation framework collection] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsPresentationFrameworkCollection(Type type)
        {
            if (type == typeof(object))
            {
                return false;
            }

            if (type.Name.StartsWith("PresentationFrameworkCollection"))
            {
                return true;
            }

            return this.IsPresentationFrameworkCollection(type.BaseType);
        }

        /// <summary>
        /// Applies the Template for the control
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
#if SILVERLIGHT
            this.border = this.GetTemplateChild("ItemBorder") as Border;
            this.outerborder = this.GetTemplateChild("OuterBorder") as Border;
#endif
#if WPF
            this.border = this.GetTemplateChild("PART_TrippleBorder") as Border;
            this.outerborder = this.GetTemplateChild("OuterBorder") as Border;
            this.m_SymbolPalette = GetSymbolPalette(this);
#endif
            Binding binding1 = new Binding();
            binding1.Source = (this.Parent as SymbolPaletteGroup).Parent;
            binding1.Path = new PropertyPath("ItemCheckedBorderBrush");
            this.SetBinding(SymbolPaletteItem.ItemCheckedBorderBrushProperty, binding1);
            binding1 = new Binding();
            binding1.Source = (this.Parent as SymbolPaletteGroup).Parent;
            binding1.Path = new PropertyPath("ItemCheckedBackgroundBrush");
            this.SetBinding(SymbolPaletteItem.ItemCheckedBackgroundBrushProperty, binding1);
            binding1 = new Binding();
            binding1.Source = (this.Parent as SymbolPaletteGroup).Parent;
            binding1.Path = new PropertyPath("ItemCheckedMouseOverBorderBrush");
            this.SetBinding(SymbolPaletteItem.ItemCheckedMouseOverBorderBrushProperty, binding1);
            binding1 = new Binding();
            binding1.Source = (this.Parent as SymbolPaletteGroup).Parent;
            binding1.Path = new PropertyPath("ItemCheckedMouseOverBackgroundBrush");
            this.SetBinding(SymbolPaletteItem.ItemCheckedMouseOverBackgroundBrushProperty, binding1);
            binding1 = new Binding();
            binding1.Source = (this.Parent as SymbolPaletteGroup).Parent;
            binding1.Path = new PropertyPath("ItemMouseOverBorderBrush");
            this.SetBinding(SymbolPaletteItem.ItemMouseOverBorderBrushProperty, binding1);
            binding1 = new Binding();
            binding1.Source = (this.Parent as SymbolPaletteGroup).Parent;
            binding1.Path = new PropertyPath("ItemMouseOverBackgroundBrush");
            this.SetBinding(SymbolPaletteItem.ItemMouseOverBackgroundBrushProperty, binding1);
            if (this.Background == null)
                this.Background = ((this.Parent as SymbolPaletteGroup).Parent as SymbolPalette).Background;
#if SILVERLIGHT
            if (this.Content is Path)
            {
                if (this.PathData != string.Empty && (this.Content as Path).Data == null)
                {
                    object obj = (UIElement)this.Clone(this.Content, 0);
                    string pathXaml = "<Path xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" Data=\"" + this.PathData + "\"/>";
                    this.Content = (Path)System.Windows.Markup.XamlReader.Load(pathXaml);
                    this.Content = this.ClonePath(obj, this.Content) as Path;
                }
            }
            else if (this.Content is Panel)
            {
                foreach (FrameworkElement ele in (this.Content as Panel).Children)
                {
                    if (ele is Path)
                    {
                        ele.Tag = (ele as Path).Data;
                    }
                }
            }
#endif
        }

        /// <summary>
        /// Event raised when mouse is left from the item.
        /// </summary>
        /// <param name="e">MouseEventArgs</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            this.m_IsMouseOver = false;
            this.UpdateVisualState(true);
        }

        /// <summary>
        /// Event raised when mouse left buttom is down on the item.
        /// </summary>
        /// <param name="e">MouseEventArgs</param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (((this.Parent as SymbolPaletteGroup).Parent as SymbolPalette).SelectedItem != null)
            {
                ((this.Parent as SymbolPaletteGroup).Parent as SymbolPalette).SelectedItem.IsSelected = false;
                ((this.Parent as SymbolPaletteGroup).Parent as SymbolPalette).SelectedItem.UpdateVisualState(false);
            }

            ((this.Parent as SymbolPaletteGroup).Parent as SymbolPalette).SelectedItem = this;
            this.IsSelected = true;
            this.UpdateVisualState(true);
            this.dragStartPoint = new Point?(e.GetPosition(this));
        }

#if SILVERLIGHT

        /// <summary>
        /// Event raised when mouse is moved on the item.
        /// </summary>
        /// <param name="e">MouseEventArgs</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            UIElement element;
            base.OnMouseMove(e);
            this.m_IsMouseOver = true;
            this.UpdateVisualState(true);

            if (this.dragStartPoint.HasValue)
            {
                string s = string.Empty;
                if (this.Content.GetType() == s.GetType())
                {
                    TextBlock text = new TextBlock();
                    text.Text = this.Content as string;
                    element = text;
                }
                else
                {
                    element = (UIElement)this.Content;
                }

                (element as FrameworkElement).Tag = this.ItemName;

                if (element is Path)
                {
                    Path obj = new Path();
                    obj.IsHitTestVisible = false;
                    Style style = (((this.Parent as SymbolPaletteGroup).Parent as SymbolPalette).symbols[this.ItemName]) as Style;
                    if (style == null)
                    {
                        if (this.PathData != string.Empty)
                        {
                            string pathXaml = "<Path xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" Data=\"" + this.PathData + "\"/>";
                            obj = (Path)System.Windows.Markup.XamlReader.Load(pathXaml);
                            obj = this.ClonePath(element, obj) as Path;
                        }
                        else
                        {
                            Path path = element as Path;
                            obj = (Path)this.Clone(element, 1);
                        }
                    }
                    else
                    {
                        obj = (Path)this.Clone(element, 0);
                    }

                    Hasvalue = true;
                    DiagramPage.Pathstring = this.PathData;
                    DiagramPage.Copyitem(obj);
                    this.dragStartPoint = null;

                }
                else
                {
                    UIElement clonedelement = (UIElement)this.Clone(element, 0);
                    object obj = clonedelement;
                    Hasvalue = true;
                    DiagramPage.Copyitem(obj);
                    this.dragStartPoint = null;
                }
            }
        }
#endif

#if WPF
        private SymbolPalette m_SymbolPalette;

        internal static SymbolPalette GetSymbolPalette(DependencyObject element)
        {
            while (element != null && !(element is SymbolPalette))
            {
                element = VisualTreeHelper.GetParent(element);
            }

            return element as SymbolPalette;
        }

        void SymbolPaletteItem_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (m_SymbolPalette == null)
            {
                m_SymbolPalette = GetSymbolPalette(this);
            }
            if (m_SymbolPalette != null)
            {
                if (double.IsNaN(this.Width))
                {
                    this.Width = m_SymbolPalette.ItemWidth;
                }
                if (double.IsNaN(this.Height))
                {
                    this.Height = m_SymbolPalette.ItemHeight;
                }
            }
        }
        

        /// <summary>
        /// Provides class handling for the PreviewMouseDown routed event that occurs when the  
        /// mouse pointer is over this control. 
        /// </summary>
        /// <param name="e">The MouseButtonEventArgs.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            UIElement element;
            base.OnMouseMove(e);
            //this.IsMouseOver = true;
            this.UpdateVisualState(true);
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                this.dragStartPoint = null;
            }

            if (this.dragStartPoint.HasValue)
            {
                if (System.Windows.Interop.BrowserInteropHelper.IsBrowserHosted)
                {
                    string s = string.Empty;
                    if (this.Content.GetType() == s.GetType())
                    {
                        Label text = new Label();
                        text.Content = this.Content as string;
                        element = text;
                    }
                    else
                    {
                        element = (UIElement)this.Content;
                    }

                    if (string.IsNullOrEmpty(element.Uid))
                    {
                        element.Uid = this.Name;
                    }

                    UIElement clonedelement = (UIElement)Clone(element);
                    object obj = clonedelement;
                    Hasvalue = true;
                    DiagramPage.Copyitem(obj);
                }
                else
                {
                    string s = string.Empty;
                    if (this.Content.GetType() == s.GetType())
                    {
                        Label text = new Label();
                        text.Content = this.Content as string;
                        element = text;
                    }
                    else
                    {
                        element = (UIElement)this.Content;
                    }

                    if (string.IsNullOrEmpty(element.Uid))
                    {
                        element.Uid = this.Name;
                    }

                    string xamlString = System.Windows.Markup.XamlWriter.Save(element);
                    DragObject dataObject = new DragObject();
                    dataObject.SerializedItem = xamlString;
                    dataObject.DragItem = this;

                    WrapPanel panel = VisualTreeHelper.GetParent(this) as WrapPanel;
                    if (panel != null)
                    {
                        double scale = 1.3;
                        dataObject.DesiredSize = new Size(panel.ItemWidth * scale, panel.ItemHeight * scale);
                    }

                    DragDrop.DoDragDrop(this, dataObject, DragDropEffects.Copy);
                }

                e.Handled = true;
            }
        }


        /// <summary>
        /// Clones the specified obj.
        /// </summary>
        /// <param name="obj">The object to be cloned.</param>
        /// <returns>The cloned object.</returns>
        private object Clone(object obj)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();
            object cloneObj = obj.GetType().GetConstructors()[0].Invoke(null);
            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(obj, null);
                if (value != null)
                {
                    try
                    {
                        if (IsPresentationFrameworkCollection(value.GetType()))
                        {
                            object collection = property.GetValue(obj, null);
                            int count = (int)collection.GetType().GetProperty("Count").GetValue(collection, null);
                            for (int i = 0; i < count; i++)
                            {
                                object child = collection.GetType().GetProperty("Item").GetValue(collection, new object[] { i });
                                object cloneChild = Clone(child);
                                object cloneCollection = property.GetValue(cloneObj, null);
                                collection.GetType().InvokeMember("Add", BindingFlags.InvokeMethod, null, cloneCollection, new object[] { cloneChild });
                            }
                        }

                        if (value is UIElement)
                        {
                            object obj2 = property.PropertyType.GetConstructors()[0].Invoke(null);
                            Clone(obj2);
                            property.SetValue(cloneObj, obj2, null);
                        }
                        else if (property.CanWrite)
                        {
                            property.SetValue(cloneObj, value, null);
                        }
                    }
                    catch
                    {
                    }
                }
            }

            return cloneObj;
        }
#endif
        /// <summary>
        /// Method is used to update the state of border
        /// </summary>
        /// <param name="useTransitions">Update the state</param>
        internal void UpdateVisualState(bool useTransitions)
        {
            if (this.IsSelected)
            {
                if (this.m_IsMouseOver && this.IsSelected)
                {
                    this.border.Background = this.ItemCheckedMouseOverBackgroundBrush;
                    this.outerborder.BorderBrush = this.ItemCheckedMouseOverBorderBrush;
                    this.GoToState(useTransitions, new string[] { "MouseOverSelected" });
                }
                else
                {
                    this.border.Background = this.ItemCheckedBackgroundBrush;
                    this.outerborder.BorderBrush = this.ItemCheckedBorderBrush;
                    this.GoToState(useTransitions, new string[] { "Selected" });
                }
            }
            else if (this.m_IsMouseOver && !this.IsSelected)
            {
                this.border.Background = this.ItemMouseOverBackgroundBrush;
                this.outerborder.BorderBrush = this.ItemMouseOverBorderBrush;
                this.GoToState(useTransitions, new string[] { "MouseOver" });
            }
            else
            {
                if (!this.IsSelected)
                {
                    this.border.Background = new SolidColorBrush(Colors.Transparent);
                    this.outerborder.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    this.GoToState(useTransitions, new string[] { "Normal" });
                }
            }
        }
    }

#if WPF
    /// <summary>
    /// DragObject class
    /// </summary>
    public class DragObject
    {
    #region Properties

        public SymbolPaletteItem DragItem{get;set;}

        /// <summary>
        /// Gets or sets the XAML
        /// </summary>
        /// <value>The XAML string.</value>
        public string SerializedItem { get; set; }

        /// <summary>
        /// Gets or sets the size of the desired.
        /// </summary>
        /// <value>The size of the desired.</value>
        internal Size? DesiredSize { get; set; }

        #endregion
    }
#endif
}



