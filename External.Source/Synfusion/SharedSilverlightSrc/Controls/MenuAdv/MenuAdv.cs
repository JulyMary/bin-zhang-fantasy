namespace Syncfusion.Windows.Shared
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Net;
    using System.Windows;
    using System.Windows.Controls;
    using Syncfusion.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Ink;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Threading;
    using System.Windows.Shapes;
    using Syncfusion.Windows.Shared;
    using System.Windows.Controls.Primitives;
    using System.ComponentModel;
    using System.Windows.Resources;
    using System.IO;
    using System.Windows.Markup;
#if WPF
using Syncfusion.Licensing;
#endif

    /// <summary>
    /// MenuAdv class
    /// </summary>

#if SILVERLIGHT
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Blend,
        Type = typeof(MenuAdv), XamlResource = "/Syncfusion.Theming.Blend;component/MenuAdv.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2007Blue,
        Type = typeof(MenuAdv), XamlResource = "/Syncfusion.Theming.Office2007Blue;component/MenuAdv.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2007Black,
        Type = typeof(MenuAdv), XamlResource = "/Syncfusion.Theming.Office2007Black;component/MenuAdv.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2007Silver,
        Type = typeof(MenuAdv), XamlResource = "/Syncfusion.Theming.Office2007Silver;component/MenuAdv.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Default,
        Type = typeof(MenuAdv), XamlResource = "/Syncfusion.Theming.Default;component/MenuAdv.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2010Blue,
        Type = typeof(MenuAdv), XamlResource = "/Syncfusion.Theming.Office2010Blue;component/MenuAdv.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2010Black,
        Type = typeof(MenuAdv), XamlResource = "/Syncfusion.Theming.Office2010Black;component/MenuAdv.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2010Silver,
        Type = typeof(MenuAdv), XamlResource = "/Syncfusion.Theming.Office2010Silver;component/MenuAdv.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Windows7,
      Type = typeof(MenuAdv), XamlResource = "/Syncfusion.Theming.Windows7;component/MenuAdv.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.VS2010,
      Type = typeof(MenuAdv), XamlResource = "/Syncfusion.Theming.VS2010;component/MenuAdv.xaml")]
#else
    [SkinType(SkinVisualStyle = Skin.Office2007Blue,
     Type = typeof(MenuAdv), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/MenuAdv/Themes/Office2007BlueStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2007Silver,
    Type = typeof(MenuAdv), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/MenuAdv/Themes/Office2007SilverStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2007Black,
    Type = typeof(MenuAdv), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/MenuAdv/Themes/Office2007BlackStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2010Blue,
     Type = typeof(MenuAdv), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/MenuAdv/Themes/Office2010BlueStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2010Silver,
    Type = typeof(MenuAdv), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/MenuAdv/Themes/Office2010SilverStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2010Black,
    Type = typeof(MenuAdv), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/MenuAdv/Themes/Office2010BlackStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2003,
     Type = typeof(MenuAdv), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/MenuAdv/Themes/Office2003Style.xaml")]
    [SkinType(SkinVisualStyle = Skin.VS2010,
     Type = typeof(MenuAdv), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/MenuAdv/Themes/VS2010Style.xaml")]
    [SkinType(SkinVisualStyle = Skin.Blend,
    Type = typeof(MenuAdv), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/MenuAdv/Themes/BlendStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Default,
     Type = typeof(MenuAdv), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/MenuAdv/Themes/MenuAdvResources.xaml")]
    [SkinType(SkinVisualStyle = Skin.ShinyRed,
     Type = typeof(MenuAdv), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/MenuAdv/Themes/ShinyRedStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.ShinyBlue,
     Type = typeof(MenuAdv), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/MenuAdv/Themes/ShinyBlueStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.SyncOrange,
     Type = typeof(MenuAdv), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/MenuAdv/Themes/SyncOrangeStyle.xaml")]
#endif

    public class MenuAdv : ItemsControl 
    {
        # region Internal Variables

        internal bool IsItemSelected;

        internal bool IsItemMouseOver = false;

        internal bool isMouseOver = false;

        internal bool isInitialOrientation = true;

        internal bool firstClick = false;

        internal double PanelHeight = 0;

        internal double PanelWidth = 0;

        private double menuItemWidth = 0;

        internal bool IsAltKeyPressed = false;

        # endregion

        #region Constructor
        public MenuAdv()
        {
#if WPF
            if (EnvironmentTest.IsSecurityGranted)
            {
                EnvironmentTest.StartValidateLicense(typeof(MenuAdv));
            }
#endif
            DefaultStyleKey = typeof(MenuAdv);
            this.ContainersToItems = new Dictionary<DependencyObject, object>();
        }

#if WPF
        static MenuAdv()
        {
           // EnvironmentTest.ValidateLicense(typeof(MenuAdv));
        }
#endif

        #endregion

        # region Properties

        internal IDictionary<DependencyObject, object> ContainersToItems { get; set; }

        /// <summary>
        /// Gets or sets the orientation.
        /// </summary>
        /// <value>The orientation.</value>
        [Category("Appearance")]
        [Description("Represents the Orientation of the MenuAdv, Which may be Horizontal or Vertical.")]
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the expand mode.
        /// </summary>
        /// <value>The expand mode.</value>
        [Category("Common Properties")]
        [Description("Represents the Expand modes of MenuItems which present in the MenuAdv")]
        public ExpandModes ExpandMode
        {
            get { return (ExpandModes)GetValue(ExpandModesProperty); }
            set { SetValue(ExpandModesProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is menu item scrollability enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is menu item scrollability enabled; otherwise, <c>false</c>.
        /// </value>
        [Category("Common Properties")]
        [Description("Represents menu items present in submenu popup can be scrollable or not")]
        public bool IsScrollEnabled
        {
            get { return (bool)GetValue(IsMenuItemScrollabilityEnabledProperty); }
            set { SetValue(IsMenuItemScrollabilityEnabledProperty, value); }
        }

        /// <summary>
        /// Gets or sets the type of the pop up animation.
        /// </summary>
        /// <value>The type of the pop up animation.</value>
        [Category("Common Properties")]
        [Description("Represents the ANimation type to open the subMenu popup, which may be None, Fade, Slide or Scroll.")]
        public AnimationTypes PopUpAnimationType
        {
            get { return (AnimationTypes)GetValue(PopUpAnimationTypeProperty); }
            set { SetValue(PopUpAnimationTypeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the item container style.
        /// </summary>
        /// <value>The item container style.</value>
        public new Style ItemContainerStyle
        {
            get
            {
                return (Style)GetValue(ItemContainerStyleProperty);
            }

            set
            {
                SetValue(ItemContainerStyleProperty, value);
            }
        }

        # endregion

        # region Dependency Properties

        // Using a DependencyProperty as the backing store for ItemContainerStyle.  This adds style to the menu items...
        public new static readonly DependencyProperty ItemContainerStyleProperty = DependencyProperty.Register("ItemContainerStyle", typeof(Style), typeof(MenuAdv), new PropertyMetadata(null, OnItemContainerStyleChanged));

        // Using a DependencyProperty as the backing store for Scrollability.  This enables scrolling of the submenu items...
        public static readonly DependencyProperty IsMenuItemScrollabilityEnabledProperty = DependencyProperty.Register("IsScrollEnabled", typeof(bool), typeof(MenuAdv), new PropertyMetadata(true));

        // Using a DependencyProperty as the backing store for Expand modes.  This enables the Expand on click or expand on mouse over supports...
        public static readonly DependencyProperty ExpandModesProperty = DependencyProperty.Register("ExpandMode", typeof(ExpandModes), typeof(MenuAdv), new PropertyMetadata(ExpandModes.ExpandOnClick));

        // Using a DependencyProperty as the backing store for Orientation.  This enables the orientation of the MenuAdv...
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(MenuAdv), new PropertyMetadata(Orientation.Horizontal, new PropertyChangedCallback(OnOrientationChanged)));

        // Using a DependencyProperty as the backing store for Popup Animation.  This enables the animation of popup like fade, slide or scroll.
        public static readonly DependencyProperty PopUpAnimationTypeProperty = DependencyProperty.Register("PopUpAnimationType", typeof(AnimationTypes), typeof(MenuAdv), new PropertyMetadata(AnimationTypes.None));

        # endregion

        # region Dp Events

        private static void OnItemContainerStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //MenuAdv source = d as MenuAdv;
            //Style value = e.NewValue as Style;

            //if (value == null)
            //{
            //    return;
            //}

            //Panel itemsHost = source.GetStackPanel();
            //if (itemsHost == null || itemsHost.Children == null)
            //{
            //    return;
            //}

            //foreach (UIElement element in itemsHost.Children)
            //{
            //    FrameworkElement obj = element as FrameworkElement;
            //    if (obj.Style == null)
            //    {
            //        obj.Style = value;
            //    }
            //}
        }

        private static void OnOrientationChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            MenuAdv menuadv = (MenuAdv)obj;
            if (!menuadv.isInitialOrientation)
            {
                while (!(obj is StackPanel) && obj != null)
                {
                    obj = VisualTreeHelper.GetChild(obj, 0);
                }

                if (obj != null)
                {
                    (obj as StackPanel).Orientation = (Orientation)e.NewValue;
                }
            }

            menuadv.CloseAllPopUps();
            menuadv.ChangeExtendButtonVisibility();
        }

        # endregion

        # region Overrides

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes (such as a rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate"/>. In simplest terms, this means the method is called just before a UI element displays in an application. For more information, see Remarks.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            isInitialOrientation = true;            
            
#if SILVERLIGHT
                FrameworkElement element = base.Parent as FrameworkElement;          
                
                var ancestors = element.GetVisualAncestorsAndSelf();                

                foreach (var child in ancestors)
                {                    
                    UIElement ele = child as UIElement;
                    ele.AddHandler(FrameworkElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(RootVisual_MouseLeftButtonDown), true);
                    ele.MouseLeftButtonDown += new MouseButtonEventHandler(this.RootVisual_MouseLeftButtonDown);
                    ele.AddHandler(FrameworkElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(RootVisual_MouseLeftButtonDown), true);
                    ele.MouseLeftButtonDown += new MouseButtonEventHandler(this.RootVisual_MouseLeftButtonDown);                    
                }
          

            //if (System.Windows.Application.Current.RootVisual != null)
            //{              
            //    //System.Windows.Application.Current.RootVisual.AddHandler(FrameworkElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(RootVisual_MouseLeftButtonDown), true);
            //    //System.Windows.Application.Current.RootVisual.MouseLeftButtonDown += new MouseButtonEventHandler(this.RootVisual_MouseLeftButtonDown);
            //    //System.Windows.Application.Current.RootVisual.AddHandler(FrameworkElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(RootVisual_MouseLeftButtonDown), true);
            //    //System.Windows.Application.Current.RootVisual.MouseLeftButtonDown += new MouseButtonEventHandler(this.RootVisual_MouseLeftButtonDown);
            //}           
            

#endif

#if WPF
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.PreviewMouseLeftButtonDown -= new MouseButtonEventHandler(MainWindow_PreviewMouseLeftButtonDown);
                Application.Current.MainWindow.Deactivated -= new EventHandler(MainWindow_Deactivated);
                Application.Current.MainWindow.LocationChanged -= new EventHandler(MainWindow_LocationChanged);
                Application.Current.MainWindow.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(MainWindow_PreviewMouseLeftButtonDown);
                Application.Current.MainWindow.Deactivated += new EventHandler(MainWindow_Deactivated);
                Application.Current.MainWindow.LocationChanged += new EventHandler(MainWindow_LocationChanged);
            }
#endif
        }

        void MainWindow_LocationChanged(object sender, EventArgs e)
        {
            this.CloseAllPopUps();
        }

        void MainWindow_Deactivated(object sender, EventArgs e)
        {
            this.CloseAllPopUps();
        }

        void MainWindow_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsItemMouseOver)
            {
                this.IsItemSelected = false;
                CloseAllPopUps();
            }
        }

        /// <summary>
        /// Determines if the specified item is (or is eligible to be) its own container.
        /// </summary>
        /// <param name="item">The item to check.</param>
        /// <returns>
        /// true if the item is (or is eligible to be) its own container; otherwise, false.
        /// </returns>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return ((item is MenuItemAdv) || (item is MenuItemSeparator));
        }

        /// <summary>
        /// Creates or identifies the element that is used to display the given item.
        /// </summary>
        /// <returns>
        /// The element that is used to display the given item.
        /// </returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MenuItemAdv();
        }

        /// <summary>
        /// Prepares the specified element to display the specified item.
        /// </summary>
        /// <param name="element">The element used to display the specified item.</param>
        /// <param name="item">The item to display.</param>
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            MenuItemAdv menuItemAdv = element as MenuItemAdv;
            if (menuItemAdv != null)
            {
                menuItemAdv.ParentMenuAdv = this;
                menuItemAdv.ParentMenuItemAdv = null;
                menuItemAdv.Parent = this;
            }

            if (!(item is MenuItemSeparator))
            {
                this.ContainersToItems[element] = item;
            }
            base.PrepareContainerForItemOverride(menuItemAdv, item);
        }

        /// <summary>
        /// Called before the <see cref="E:System.Windows.UIElement.MouseEnter"/> event occurs.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            this.IsItemMouseOver = true;
        }

        /// <summary>
        /// Called before the <see cref="E:System.Windows.UIElement.MouseLeave"/> event occurs.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            this.IsItemMouseOver = false;
        }

        # endregion

        # region Events

        void RootVisual_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsItemMouseOver)
            {
                this.IsItemSelected = false;
                CloseAllPopUps();
            }
        }

        # endregion

        # region Methods

        internal StackPanel GetStackPanel()
        {
            Panel itemsHost = null;
            if (itemsHost == null)
            {
                DependencyObject treeviewitem = this.Items[0] as MenuItemAdv; //this.ItemContainerGenerator.ContainerFromIndex(0); //ChildrenToItems.First().Key;
                itemsHost = VisualTreeHelper.GetParent(treeviewitem) as Panel;
            }
            return (StackPanel)itemsHost;
        }

        internal void GetMenuItem(MenuItemAdv menuItem)
        {
            if (menuItem != null)
            {
                if (this.menuItemWidth != menuItem.ActualWidth)
                {
                    this.PanelWidth = menuItem.ActualWidth;
                    menuItem.HandlePopupOpen();
                    menuItem.SelectPopUpAnimation();
                    this.menuItemWidth = menuItem.ActualWidth;
                }
            }
        }

        internal void CloseAllPopUps()
        {
            for (int i = 0; i < this.ContainersToItems.Count; i++)
            {
                if (ItemContainerGenerator.ContainerFromIndex(i) is MenuItemAdv)
                {
                    if (((MenuItemAdv)ItemContainerGenerator.ContainerFromIndex(i)).ParentMenuAdv != null)
                    {
                        ((MenuItemAdv)ItemContainerGenerator.ContainerFromIndex(i)).IsSubMenuOpen = false;
                        MenuItemAdv menuItem = (ItemContainerGenerator.ContainerFromIndex(i) as MenuItemAdv);
                        if (menuItem.IsEnabled == true)
                            VisualStateManager.GoToState(menuItem, "Normal", true);
                        VisualStateManager.GoToState(menuItem.CheckBoxPanel, "Normal", false);
                        VisualStateManager.GoToState(menuItem.RadioButtonPanel, "Normal", false);
                        menuItem.IsBoundaryDetected = false;
                        if (menuItem.PART_BottomScroll != null && menuItem.PART_BottomScroll.Visibility == System.Windows.Visibility.Visible)
                            menuItem.PART_BottomScroll.Visibility = System.Windows.Visibility.Collapsed;

                        if (menuItem.PART_TopScroll != null && menuItem.PART_TopScroll.Visibility == System.Windows.Visibility.Visible)
                            menuItem.PART_TopScroll.Visibility = System.Windows.Visibility.Collapsed;
                        this.IsAltKeyPressed = false;

#if WPF
                        if (menuItem.ParentMenuAdv != null)
                        {
                            foreach (var item in menuItem.ParentMenuAdv.ContainersToItems.Keys)
                            {
                                if (item is MenuItemAdv)
                                {
                                    (item as MenuItemAdv).canFocused = false;
                                    (item as MenuItemAdv).Focus();
                                }
                                break;
                            }
                        }
#endif
                    }
                }
            }

            if (this.ExpandMode == ExpandModes.ExpandOnClick)
            {
                this.firstClick = false;
            }
            this.IsAltKeyPressed = false;
        }

        internal void ChangeExtendButtonVisibility()
        {
            foreach (var menuitem in this.Items)
            {
                if (menuitem is MenuItemAdv)
                {
                    ((MenuItemAdv)menuitem).ChangeExtendButtonVisibility();
                }
            }
        }

        # endregion
    }

    /// <summary>
    /// Menu ExpandModes enumeration
    /// </summary>
    public enum ExpandModes
    {
        /// <summary>
        /// Expands menu items present in menu on mouse hover.
        /// </summary>
        ExpandOnMouseOver,

        /// <summary>
        /// Expands menu items present in menu on mouse click.
        /// </summary>
        ExpandOnClick
    }

    /// <summary>
    /// Popup AnimationType enumeration
    /// </summary>
    public enum AnimationTypes
    {
        None,
        Fade,
        Slide,
        Scroll,
        Custom
    }

    /// <summary>
    /// MenuItem Role enumeration
    /// </summary>
    public enum Role
    {
        SubmenuHeader,
        SubmenuItem,
        TopLevelHeader,
        TopLevelItem
    }
}