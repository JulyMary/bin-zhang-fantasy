﻿using System.ComponentModel;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using System;
using Syncfusion.Windows.Shared;
using System.Windows.Resources;
using System.IO;
using System.Windows.Markup;

namespace Syncfusion.Windows.Shared
{
    //[Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Blend,
    //  Type = typeof(ContextMenuAdv), XamlResource = "/Syncfusion.Theming.Blend;component/ContextMenuAdv.xaml")]
    //[Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2007Blue,
    //    Type = typeof(ContextMenuAdv), XamlResource = "/Syncfusion.Theming.Office2007Blue;component/ContextMenuAdv.xaml")]
    //[Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2007Black,
    //    Type = typeof(ContextMenuAdv), XamlResource = "/Syncfusion.Theming.Office2007Black;component/ContextMenuAdv.xaml")]
    //[Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2007Silver,
    //    Type = typeof(ContextMenuAdv), XamlResource = "/Syncfusion.Theming.Office2007Silver;component/ContextMenuAdv.xaml")]
    //[Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2010Blue,
    //    Type = typeof(ContextMenuAdv), XamlResource = "/Syncfusion.Theming.Office2010Blue;component/ContextMenuAdv.xaml")]
    //[Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2010Black,
    //    Type = typeof(ContextMenuAdv), XamlResource = "/Syncfusion.Theming.Office2010Black;component/ContextMenuAdv.xaml")]
    //[Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2010Silver,
    //    Type = typeof(ContextMenuAdv), XamlResource = "/Syncfusion.Theming.Office2010Silver;component/ContextMenuAdv.xaml")]
    //[Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Default,
    //    Type = typeof(ContextMenuAdv), XamlResource = "/Syncfusion.Theming.Default;component/ContextMenuAdv.xaml")]
    //[Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Windows7,
    //   Type = typeof(ContextMenuAdv), XamlResource = "/Syncfusion.Theming.Windows7;component/ContextMenuAdv.xaml")]
    /// <summary>
    /// Represents a pop-up menu that enables a control to expose functionality that is specific to the context of the control.
    /// </summary>
    /// <QualityBand>Preview</QualityBand>
    public class ContextMenuAdv : MenuBase
    {
        /// <summary>
        /// Gets or sets the visual style.
        /// </summary>
        /// <value>The visual style.</value>
        public VisualStyle VisualStyle
        {
            get { return (VisualStyle)GetValue(VisualStyleProperty); }
            set { SetValue(VisualStyleProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty VisualStyleProperty =
            DependencyProperty.Register("VisualStyle", typeof(VisualStyle), typeof(ContextMenuAdv), new PropertyMetadata(VisualStyle.Default,OnVisualStyleChanged));

        /// <summary>
        /// Called when [visual style changed].
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        public static void OnVisualStyleChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ((ContextMenuAdv)obj).OnVisualStyleChanged(args);
        }

        internal bool OverrideVisualStyle = true;
        /// <summary>
        /// Raises the <see cref="E:VisualStyleChanged"/> event.
        /// </summary>
        /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        protected void OnVisualStyleChanged(DependencyPropertyChangedEventArgs args)
        {
            OverrideVisualStyle = false;

            string uri = "/Syncfusion.Shared.Silverlight;component/Controls/ContextMenuAdv/Themes/" + this.VisualStyle.ToString() + ".xaml";
            StreamResourceInfo StreamResourceInfoObj = Application.GetResourceStream(new Uri(uri, UriKind.RelativeOrAbsolute));

            if (StreamResourceInfoObj != null && StreamResourceInfoObj.Stream != null)
            {
                using (StreamReader StreamReaderObj = new StreamReader(StreamResourceInfoObj.Stream))
                {
                    string resourcemerged = StreamReaderObj.ReadToEnd();

                    if (!string.IsNullOrEmpty(resourcemerged))
                    {
                        this.Resources.Clear();
                        this.Resources = XamlReader.Load(resourcemerged) as ResourceDictionary;

                        //ResourceDictionary rd = new ResourceDictionary()
                        //{
                        //    Source = new Uri("/Syncfusion.Tools.Silverlight;component/Themes/generic.xaml", UriKind.RelativeOrAbsolute)
                        //};

                        if (this.Resources.Contains("ContextMenuAdv"))
                            this.Style = this.Resources["ContextMenuAdv"] as Style;

                        if (this.Resources.Contains("ContextMenuItemAdv"))
                        {
                            for (int i = 0; i < this.Items.Count; i++)
                            {
                                if (this.ItemsSource != null)
                                {
                                    ContextMenuItemAdv m_treeViewItem = this.ItemContainerGenerator.ContainerFromItem(this.Items[i]) as ContextMenuItemAdv;
                                    if (m_treeViewItem != null)
                                    {
                                        if (this.Items[i] is ContextMenuItemAdv)
                                            m_treeViewItem.Style = this.Resources["ContextMenuItemAdv"] as Style;
                                        //m_treeViewItem.Style = this.Resources["ContextMenuItemAdv"] as Style;
                                    }
                                }
                                else
                                {
                                    if (this.Items[i] is ContextMenuItemAdv)
                                        (this.Items[i] as ContextMenuItemAdv).Style = this.Resources["ContextMenuItemAdv"] as Style;
                                }
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Gets or sets the sub menu border thickness.
        /// </summary>
        /// <value>The sub menu border thickness.</value>
        public Thickness SubMenuBorderThickness
        {
            get { return (Thickness)GetValue(SubMenuBorderThicknessProperty); }
            set { SetValue(SubMenuBorderThicknessProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SubMenuBorderThicknessProperty =
            DependencyProperty.Register("SubMenuBorderThickness", typeof(Thickness), typeof(ContextMenuAdv), new PropertyMetadata(new Thickness(1)));

        /// <summary>
        /// Gets or sets the sub menu background.
        /// </summary>
        /// <value>The sub menu background.</value>
        public Brush SubMenuBackground
        {
            get { return (Brush)GetValue(SubMenuBackgroundProperty); }
            set { SetValue(SubMenuBackgroundProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SubMenuBackgroundProperty =
            DependencyProperty.Register("SubMenuBackground", typeof(Brush), typeof(ContextMenuAdv), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        /// <summary>
        /// Gets or sets the sub menu border brush.
        /// </summary>
        /// <value>The sub menu border brush.</value>
        public Brush SubMenuBorderBrush
        {
            get { return (Brush)GetValue(SubMenuBorderBrushProperty); }
            set { SetValue(SubMenuBorderBrushProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SubMenuBorderBrushProperty =
            DependencyProperty.Register("SubMenuBorderBrush", typeof(Brush), typeof(ContextMenuAdv), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        /// <summary>
        /// Gets or sets a value indicating whether [open on click].
        /// </summary>
        /// <value><c>true</c> if [open on click]; otherwise, <c>false</c>.</value>
        public bool OpenOnClick
        {
            get { return (bool)GetValue(OpenOnClickProperty); }
            set { SetValue(OpenOnClickProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty OpenOnClickProperty =
            DependencyProperty.Register("OpenOnClick", typeof(bool), typeof(ContextMenuAdv), new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets the focused element.
        /// </summary>
        /// <value>The focused element.</value>
        public ContextMenuItemAdv FocusedElement
        {
            get { return (ContextMenuItemAdv)GetValue(FocusedElementProperty); }
            set { SetValue(FocusedElementProperty, value); }
        }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty FocusedElementProperty =
            DependencyProperty.Register("FocusedElement", typeof(ContextMenuItemAdv), typeof(ContextMenuAdv), new PropertyMetadata(OnFocusedElementChanged));

        /// <summary>
        /// Called when [focused element changed].
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnFocusedElementChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if ((args.NewValue as ContextMenuItemAdv).ParentMenuBase != null)
            {
                //(args.NewValue as ContextMenuItemAdv).ParentMenuBase.HandleOpenClsoe(args.NewValue as ContextMenuItemAdv,args.OldValue as ContextMenuItemAdv);
            }
        }

        #region Routed Events
        /// <summary>
        /// Occurs when a particular instance of a ContextMenuAdv opens.
        /// </summary>
        public event RoutedEventHandler Opened;

        /// <summary>
        /// Occurs when a particular instance of a ContextMenuAdv closes.
        /// </summary>
        public event RoutedEventHandler Closed;
        #endregion

        #region Internal Members
        /// <summary>
        /// Stores a reference to the current root visual.
        /// </summary>
        private FrameworkElement _rootVisual;

        /// <summary>
        /// Stores the last known mouse position (via MouseMove).
        /// </summary>
        private Point _mousePosition;

        /// <summary>
        /// Stores a reference to the object that owns the ContextMenuAdv.
        /// </summary>
        private DependencyObject _owner;

        /// <summary>
        /// Stores a reference to the current Popup.
        /// </summary>
        private Popup _popup;

        /// <summary>
        /// Stores a reference to the current overlay.
        /// </summary>
        private Panel _overlay;

        /// <summary>
        /// Stores a reference to the current Popup alignment point.
        /// </summary>
        private Point _popupAlignmentPoint;

        /// <summary>
        /// Stores a value indicating whether the IsOpen property is being updated by ContextMenuAdv.
        /// </summary>
        private bool _settingIsOpen;
        #endregion

        /// <summary>
        /// Gets or sets the owning object for the ContextMenuAdv.
        /// </summary>
        public DependencyObject Owner
        {
            get { return _owner; }
            set
            {
                if (null != _owner)
                {
                    FrameworkElement ownerFrameworkElement = _owner as FrameworkElement;
                    if (null != ownerFrameworkElement)
                    {
                        ownerFrameworkElement.MouseRightButtonDown -= new MouseButtonEventHandler(HandleOwnerMouseRightButtonDown);
                    }
                }
                _owner = value;
                if (null != _owner)
                {
                    FrameworkElement ownerFrameworkElement = _owner as FrameworkElement;
                    if (null != ownerFrameworkElement)
                    {
                        ownerFrameworkElement.MouseRightButtonDown += new MouseButtonEventHandler(HandleOwnerMouseRightButtonDown);
                    }
                }
            }
        }

        #region DP Events
        /// <summary>
        /// Gets or sets the horizontal distance between the target origin and the popup alignment point.
        /// </summary>
       // [TypeConverterAttribute(typeof(LengthConverter))]
        public double HorizontalOffset
        {
            get { return (double)GetValue(HorizontalOffsetProperty); }
            set { SetValue(HorizontalOffsetProperty, value); }
        }

        /// <summary>
        /// Identifies the HorizontalOffset dependency property.
        /// </summary>
        public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register("HorizontalOffset", typeof(double), typeof(ContextMenuAdv), new PropertyMetadata(0.0, OnHorizontalVerticalOffsetChanged));

        /// <summary>
        /// Gets or sets the vertical distance between the target origin and the popup alignment point.
        /// </summary>
        //[TypeConverterAttribute(typeof(LengthConverter))]
        public double VerticalOffset
        {
            get { return (double)GetValue(VerticalOffsetProperty); }
            set { SetValue(VerticalOffsetProperty, value); }
        }

        /// <summary>
        /// Identifies the VerticalOffset dependency property.
        /// </summary>
        public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register("VerticalOffset", typeof(double), typeof(ContextMenuAdv), new PropertyMetadata(0.0, OnHorizontalVerticalOffsetChanged));

        /// <summary>
        /// Handles changes to the HorizontalOffset or VerticalOffset DependencyProperty.
        /// </summary>
        /// <param name="o">DependencyObject that changed.</param>
        /// <param name="e">Event data for the DependencyPropertyChangedEvent.</param>
        private static void OnHorizontalVerticalOffsetChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((ContextMenuAdv)o).UpdateContextMenuPlacement();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the ContextMenuAdv is visible.
        /// </summary>
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        /// <summary>
        /// Identifies the IsOpen dependency property.
        /// </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(ContextMenuAdv), new PropertyMetadata(false, OnIsOpenChanged));

        /// <summary>
        /// Handles changes to the IsOpen DependencyProperty.
        /// </summary>
        /// <param name="o">DependencyObject that changed.</param>
        /// <param name="e">Event data for the DependencyPropertyChangedEvent.</param>
        private static void OnIsOpenChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((ContextMenuAdv)o).OnIsOpenChanged((bool)e.OldValue, (bool)e.NewValue);
        }
        #endregion

        /// <summary>
        /// Handles changes to the IsOpen property.
        /// </summary>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private void OnIsOpenChanged(bool oldValue, bool newValue)
        {
            if (!_settingIsOpen)
            {
                if (newValue)
                {
                    OpenPopup(_mousePosition);
                }
                else
                {
                    ClosePopup();
                }
            }
        }

        /// <summary>
        /// Called when the Opened event occurs.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnOpened(RoutedEventArgs e)
        {
            RoutedEventHandler handler = Opened;
            if (null != handler)
            {
                handler.Invoke(this, e);
            }
        }

        /// <summary>
        /// Called when the Closed event occurs.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnClosed(RoutedEventArgs e)
        {
            RoutedEventHandler handler = Closed;
            if (null != handler)
            {
                handler.Invoke(this, e);
            }
        }

        /// <summary>
        /// Initializes a new instance of the ContextMenuAdv class.
        /// </summary>
        public ContextMenuAdv()
        {
            DefaultStyleKey = typeof(ContextMenuAdv);

            // Temporarily hook LayoutUpdated to find out when Application.Current.RootVisual gets set.
            LayoutUpdated += new EventHandler(HandleLayoutUpdated);
        }

        /// <summary>
        /// Called when the left mouse button is pressed.
        /// </summary>
        /// <param name="e">The event data for the MouseLeftButtonDown event.</param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            e.Handled = true;
            base.OnMouseLeftButtonDown(e);
        }

        /// <summary>
        /// Called when the right mouse button is pressed.
        /// </summary>
        /// <param name="e">The event data for the MouseRightButtonDown event.</param>
        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {           
            e.Handled = true;
            base.OnMouseRightButtonDown(e);
        }

        /// <summary>
        /// Responds to the KeyDown event.
        /// </summary>
        /// <param name="e">The event data for the KeyDown event.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    FocusNextItem(false);
                    e.Handled = true;
                    break;
                case Key.Down:
                    FocusNextItem(true);
                    e.Handled = true;
                    break;
                case Key.Escape:
                    ClosePopup();
                    e.Handled = true;
                    break;
                // case Key.Apps: // Key.Apps not defined by Silverlight 4
            }
            base.OnKeyDown(e);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //for (int i = 0; i < this.Items.Count; i++)
            //{
            //    if (this.ItemsSource != null)
            //    {
            //        ContextMenuItemAdv m_treeViewItem = this.ItemContainerGenerator.ContainerFromItem(this.Items[i]) as ContextMenuItemAdv;
            //        if (m_treeViewItem != null)
            //        {
            //            if (this.ItemContainerStyle != null && this.Items[i] is ContextMenuItemAdv)
            //                m_treeViewItem.Style = this.ItemContainerStyle;

            //        }
            //    }
            //    else
            //    {
            //        if (this.ItemContainerStyle != null && this.Items[i] is ContextMenuItemAdv)
            //            (this.Items[i] as ContextMenuItemAdv).Style = this.ItemContainerStyle;
            //    }
            //}
        }

        /// <summary>
        /// Handles the LayoutUpdated event to capture Application.Current.RootVisual.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event arguments.</param>
        private void HandleLayoutUpdated(object sender, EventArgs e)
        {
            if (null != Application.Current.RootVisual)
            {
                // Application.Current.RootVisual is valid
                InitializeRootVisual();
                // Unhook event
                LayoutUpdated -= new EventHandler(HandleLayoutUpdated);
            }
        }

        /// <summary>
        /// Handles the RootVisual's MouseMove event to track the last mouse position.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event arguments.</param>
        private void HandleRootVisualMouseMove(object sender, MouseEventArgs e)
        {
            _mousePosition = e.GetPosition(null);
        }

        /// <summary>
        /// Handles the MouseRightButtonDown event for the owning element.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event arguments.</param>
        private void HandleOwnerMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenPopup(e.GetPosition(null));
            e.Handled = true;
        }

        /// <summary>
        /// Initialize the _rootVisual property (if possible and not already done).
        /// </summary>
        private void InitializeRootVisual()
        {
            if (null == _rootVisual)
            {
                // Try to capture the Application's RootVisual
                _rootVisual = Application.Current.RootVisual as FrameworkElement;
                if (null != _rootVisual)
                {
                    // Ideally, this would use AddHandler(MouseMoveEvent), but MouseMoveEvent doesn't exist
                    _rootVisual.MouseMove += new MouseEventHandler(HandleRootVisualMouseMove);
                }
            }
        }

        /// <summary>
        /// Sets focus to the next item in the ContextMenuAdv.
        /// </summary>
        /// <param name="down">True to move the focus down; false to move it up.</param>
        private void FocusNextItem(bool down)
        {
            int count = Items.Count;
            int startingIndex = down ? -1 : count;
            ContextMenuItemAdv focusedMenuItem = FocusManager.GetFocusedElement() as ContextMenuItemAdv;
            if (null != focusedMenuItem && (this == focusedMenuItem.ParentMenuBase))
            {
                startingIndex = ItemContainerGenerator.IndexFromContainer(focusedMenuItem);
            }
            int index = startingIndex;
            do
            {
                index = (index + count + (down ? 1 : -1)) % count;
                ContextMenuItemAdv container = ItemContainerGenerator.ContainerFromIndex(index) as ContextMenuItemAdv;
                if (null != container)
                {
                    if (container.IsEnabled && container.Focus())
                    {
                        break;
                    }
                }
            }
            while (index != startingIndex);
        }

        /// <summary>
        /// Called when a child ContextMenuItemAdv is clicked.
        /// </summary>
        internal void ChildMenuItemClicked()
        {
            ClosePopup();
        }

        /// <summary>
        /// Handles the SizeChanged event for the ContextMenuAdv or RootVisual.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event arguments.</param>
        private void HandleContextMenuOrRootVisualSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateContextMenuPlacement();
        }

        /// <summary>
        /// Handles the MouseButtonDown events for the overlay.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event arguments.</param>
        private void HandleOverlayMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            ClosePopup();
            e.Handled = true;
        }

        /// <summary>
        /// Updates the location and size of the Popup and overlay.
        /// </summary>
        private void UpdateContextMenuPlacement()
        {
            if ((null != _rootVisual) && (null != _overlay))
            {
                // Start with the current Popup alignment point
                double x = _popupAlignmentPoint.X;
                double y = _popupAlignmentPoint.Y;
                // Adjust for offset
                x += HorizontalOffset;
                y += VerticalOffset;
                // Try not to let it stick out too far to the right/bottom
                x = Math.Min(x, _rootVisual.ActualWidth - ActualWidth);
                y = Math.Min(y, _rootVisual.ActualHeight - ActualHeight);
                // Do not let it stick out too far to the left/top
                x = Math.Max(x, 0);
                y = Math.Max(y, 0);
                // Set the new location
                Canvas.SetLeft(this, x);
                Canvas.SetTop(this, y);
                // Size the overlay to match the new container
                _overlay.Width = _rootVisual.ActualWidth;
                _overlay.Height = _rootVisual.ActualHeight;
            }
        }

        /// <summary>
        /// Opens the Popup.
        /// </summary>
        /// <param name="position">Position to place the Popup.</param>
        public void OpenPopup(Point position)
        {
            _popupAlignmentPoint = position;

            InitializeRootVisual();

            _overlay = new Canvas { Background = new SolidColorBrush(Colors.Transparent) };
            _overlay.MouseLeftButtonDown += new MouseButtonEventHandler(HandleOverlayMouseButtonDown);
            _overlay.MouseRightButtonDown += new MouseButtonEventHandler(HandleOverlayMouseButtonDown);
            _overlay.Children.Add(this);

            _popup = new Popup { Child = _overlay };

            SizeChanged += new SizeChangedEventHandler(HandleContextMenuOrRootVisualSizeChanged);
            if (null != _rootVisual)
            {
                _rootVisual.SizeChanged += new SizeChangedEventHandler(HandleContextMenuOrRootVisualSizeChanged);
            }
            UpdateContextMenuPlacement();

            if (ReadLocalValue(DataContextProperty) == DependencyProperty.UnsetValue)
            {
                DependencyObject dataContextSource = Owner ?? _rootVisual;
                SetBinding(DataContextProperty, new Binding("DataContext") { Source = dataContextSource });
            }

            _popup.IsOpen = true;
            Focus();

            // Update IsOpen
            _settingIsOpen = true;
            IsOpen = true;
            _settingIsOpen = false;

            OnOpened(new RoutedEventArgs());
        }

        /// <summary>
        /// Closes the Popup.
        /// </summary>
        private void ClosePopup()
        {
            this.ClsoeSubPopup();
            if (null != _popup)
            {
                _popup.IsOpen = false;
                _popup.Child = null;
                _popup = null;
            }
            if (null != _overlay)
            {
                _overlay.Children.Clear();
                _overlay = null;
            }
            SizeChanged -= new SizeChangedEventHandler(HandleContextMenuOrRootVisualSizeChanged);
            if (null != _rootVisual)
            {
                _rootVisual.SizeChanged -= new SizeChangedEventHandler(HandleContextMenuOrRootVisualSizeChanged);
            }

            // Update IsOpen
            _settingIsOpen = true;
            IsOpen = false;
            _settingIsOpen = false;

            OnClosed(new RoutedEventArgs());
        }

        internal void ClsoeSubPopup()
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                if (this.Items[i] is ContextMenuItemAdv)
                {
                    if ((this.Items[i] as ContextMenuItemAdv) != null)
                    {
                        ((ContextMenuItemAdv)this.Items[i]).ClosePopup();
                    }
                }
                else
                {
                    ContextMenuItemAdv m_menuitem = this.ItemContainerGenerator.ContainerFromIndex(i) as ContextMenuItemAdv;
                    if (m_menuitem != null)
                    {
                        m_menuitem.ClosePopup();
                    }
                }
            }
        }
    }

}
