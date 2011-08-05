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
using Syncfusion.Windows.Shared;
using System.Windows.Controls.Primitives;
using System.ComponentModel;
#if WPF
using Syncfusion.Licensing;
#endif

namespace Syncfusion.Windows.Tools.Controls
{
    /// <summary>
    /// Represents the DropDownMenu Item
    /// </summary>
    [TemplateVisualState(GroupName = "RibbonButtonStates", Name = "Normal")]
    [TemplateVisualState(GroupName = "RibbonMenuItemStates", Name = "MouseOver")]
    [TemplateVisualState(GroupName = "RibbonMenuItemStates", Name = "DropDownOpened")]
    [TemplateVisualState(GroupName = "RibbonMenuItemStates", Name = "DropDownClosed")]
    [TemplateVisualState(GroupName = "RibbonMenuItemStates", Name = "Checked")]
    [TemplateVisualState(GroupName = "RibbonMenuItemStates", Name = "UnChecked")]
#if WPF
    [SkinType(SkinVisualStyle = Skin.Office2007Blue,
      Type = typeof(DropDownMenuItem), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/ButtonControls/DropDownButton/Themes/Office2007BlueStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2007Black,
    Type = typeof(DropDownMenuItem), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/ButtonControls/DropDownButton/Themes/Office2007BlackStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2007Silver,
    Type = typeof(DropDownMenuItem), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/ButtonControls/DropDownButton/Themes/Office2007SilverStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2010Blue,
      Type = typeof(DropDownMenuItem), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/ButtonControls/DropDownButton/Themes/Office2010BlueStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2010Black,
    Type = typeof(DropDownMenuItem), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/ButtonControls/DropDownButton/Themes/Office2010BlackStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2010Silver,
    Type = typeof(DropDownMenuItem), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/ButtonControls/DropDownButton/Themes/Office2010SilverStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Blend,
    Type = typeof(DropDownMenuItem), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/ButtonControls/DropDownButton/Themes/BlendStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.VS2010,
    Type = typeof(DropDownMenuItem), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/ButtonControls/DropDownButton/Themes/VS2010Style.xaml")]
    [SkinType(SkinVisualStyle = Skin.ShinyRed,
    Type = typeof(DropDownMenuItem), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/ButtonControls/DropDownButton/Themes/ShinyRedStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.ShinyBlue,
    Type = typeof(DropDownMenuItem), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/ButtonControls/DropDownButton/Themes/ShinyBlueStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Default,
    Type = typeof(DropDownMenuItem), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/ButtonControls/DropDownButton/Themes/DropDownButton.xaml")]
#endif
    public class DropDownMenuItem : HeaderedItemsControl, ICommandSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DropDownMenuItem"/> class.
        /// </summary>
        public DropDownMenuItem()
        {
#if WPF
            if (EnvironmentTest.IsSecurityGranted)
            {
                EnvironmentTest.StartValidateLicense(typeof(DropDownMenuItem));
            }
#endif
            DefaultStyleKey = typeof(DropDownMenuItem);
            this.KeyDown += new KeyEventHandler(DropDownMenuItem_KeyDown);
        }

#if WPF
        static DropDownMenuItem()
        {
            //EnvironmentTest.ValidateLicense(typeof(DropDownMenuItem));
        }
#endif

#if SILVERLIGHT
        /// <summary>
        /// Initializes the <see cref="DropDownMenuItem"/> class.
        /// </summary>
        static DropDownMenuItem()
        {
            if (DesignerProperties.IsInDesignTool)
            {
                LoadDependentAssemblies load = new LoadDependentAssemblies();
                load = null;
            }
        }
#endif

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>The icon.</value>
        public object Icon
        {
            get { return (object)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        internal Popup _dropdown;

        internal Border _host;

        

        public event PropertyChangedCallback IsCheckedChanged;

        #region Dependency Properties

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(object), typeof(DropDownMenuItem), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a value indicating whether this instance has items.
        /// </summary>
        /// <value><c>true</c> if this instance has items; otherwise, <c>false</c>.</value>
        public new bool HasItems
        {
            get { return (bool)GetValue(HasItemsProperty); }
            internal set { SetValue(HasItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HasItems.  This enables animation, styling, binding, etc...
        public new static readonly DependencyProperty HasItemsProperty =
            DependencyProperty.Register("HasItems", typeof(bool), typeof(DropDownMenuItem), new PropertyMetadata(false));


        /// <summary>
        /// Gets or sets a value indicating whether this instance is checkable.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is checkable; otherwise, <c>false</c>.
        /// </value>
        [Category("Common Properties")]
        [Description("Represents the value, whether the element can be checkable or not")]
        public bool IsCheckable
        {
            get { return (bool)GetValue(IsCheckableProperty); }
            set { SetValue(IsCheckableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsCheckable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCheckableProperty =
            DependencyProperty.Register("IsCheckable", typeof(bool), typeof(DropDownMenuItem), new PropertyMetadata(false, new PropertyChangedCallback(OnIsCheckableChanged)));


        /// <summary>
        /// Gets or sets a value indicating whether this instance is checked.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is checked; otherwise, <c>false</c>.
        /// </value>
        [Category("Common Properties")]
        [Description("Represents the value, whether the element is checked or not")]
        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsChecked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(DropDownMenuItem), new PropertyMetadata(false, new PropertyChangedCallback(OnIsCheckedChanged)));

        #endregion

        #region Implementation

        private static void OnIsCheckableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = d as DropDownMenuItem;
            sender.OnIsCheckableChanged(e);
        }

        private void OnIsCheckableChanged(DependencyPropertyChangedEventArgs e)
        {
            if (!((bool)e.NewValue))
            {
                if (IsChecked)
                    IsChecked = false;
            }
            OnIsCheckedChanged();
        }

        private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = d as DropDownMenuItem;
            if (sender.IsCheckedChanged != null)
            {
                sender.IsCheckedChanged(sender, e);
            }
            sender.OnIsCheckedChanged();
        }

        private void OnIsCheckedChanged()
        {

        }

        internal DropDownMenuItem ParentRibbonMenuItem
        {
            get;
            set;
        }

        internal DropDownButtonAdv ParentDropDown
        {
            get
            {
#if SILVERLIGHT
                var item = VisualUtil.FindAncestor(this, typeof(DropDownMenuGroup));
#else
                var item = VisualUtils.FindAncestor(this, typeof(DropDownMenuGroup));
#endif
                if (item != null)
                    return ((DropDownMenuGroup)item).Parent as DropDownButtonAdv;
                return null;
            }
        }

        protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (Items.Count > 0)
            {
                HasItems = true;
            }
            else
            {
                HasItems = false;
            }
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                DropDownMenuItem menuItem = e.NewItems[0] as DropDownMenuItem;
                if (menuItem != null)
                {
                    menuItem.ParentRibbonMenuItem = this;
                }
            }
            base.OnItemsChanged(e);
        }

        void DropDownMenuItem_KeyDown(object sender, KeyEventArgs e)
        {
#if SILVERLIGHT
            if (e.Key == Key.Enter)
            {
                if (IsCheckable)
                {
                    if (IsChecked)
                        IsChecked = false;
                    else
                        IsChecked = true;
                }
                OnClick();
            }

            if (e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Home || e.Key == Key.End)
            {
                DropDownMenuGroup menuGroup = this.Parent as DropDownMenuGroup;
                if (menuGroup != null)
                    menuGroup.FindAndFocusItem(this, e.Key);
            }
#endif
        }

        #endregion

        #region ICommandSource Members

        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        /// <value>The command.</value>
        [Category("Common Properties")]
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(DropDownMenuItem), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the command parameter.
        /// </summary>
        /// <value>The command parameter.</value>
        [Category("Common Properties")]
        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(DropDownMenuItem), new PropertyMetadata(null));

#if WPF
        public IInputElement CommandTarget
        {
            get { return (IInputElement)GetValue(CommandTargetProperty); }
            set { SetValue(CommandTargetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandTarget.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandTargetProperty =
            DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(DropDownMenuItem), new UIPropertyMetadata(null));
#endif

#if SILVERLIGHT
        /// <summary>
        /// Gets or sets a value indicating whether this instance is mouse over.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is mouse over; otherwise, <c>false</c>.
        /// </value>
        [Category("Common Properties")]
        public bool IsMouseOver
        {
            get { return (bool)GetValue(IsMouseOverProperty); }
            protected internal set { SetValue(IsMouseOverProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsMouseOver.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsMouseOverProperty =
            DependencyProperty.Register("IsMouseOver", typeof(bool), typeof(DropDownMenuItem), new PropertyMetadata(false, OnMouseOverChanged));
#endif

        public static void OnMouseOverChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var senderItem = (DropDownMenuItem)sender;
            senderItem.GotoVisualStates();
        }

        private void GotoVisualStates()
        {
            if (IsMouseOver)
                VisualStateManager.GoToState(this, "MouseOver", false);
            else
                VisualStateManager.GoToState(this, "Normal", false);
        }

        /// <summary>
        /// Occurs when [click].
        /// </summary>
        public event RoutedEventHandler Click;

        /// <summary>
        /// Called when [click].
        /// </summary>
        protected virtual void OnClick()
        {
            if (Click != null)
            {
                Click(this, new RoutedEventArgs());
            }

            if (Command != null)
            {
                if (Command.CanExecute(CommandParameter))
                {
                    Command.Execute(CommandParameter);
                }
            }
            if (ParentRibbonMenuItem != null)
            {
                ParentRibbonMenuItem._dropdown.IsOpen = false;
                if (ParentRibbonMenuItem.ParentDropDown != null)
                {
                    VisualStateManager.GoToState(ParentRibbonMenuItem, "Normal", true);
                    if (Items.Count == 0)
                    {
                        ParentRibbonMenuItem.ParentDropDown._dropdown.IsOpen = false;
                    }
                }
            }
            if (ParentDropDown != null)
            {
                if (Items.Count == 0)
                {
                    ParentDropDown._dropdown.IsOpen = false;
                    VisualStateManager.GoToState(this, "Normal", true);
                }
            }
            if (this.Parent != null)
            {
                DropDownMenuItem menuItem = this.Parent as DropDownMenuItem;
                if (menuItem != null)
                {
                    menuItem._dropdown.IsOpen = false;
                }
            }
        }
        /// <summary>
        /// Called before the <see cref="E:System.Windows.UIElement.MouseLeftButtonDown"/> event occurs.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
        }


        /// <summary>
        /// Called before the <see cref="E:System.Windows.UIElement.MouseLeftButtonUp"/> event occurs.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            if (IsCheckable)
            {
                if (IsChecked)
                {
                    IsChecked = false;
                }
                else
                {
                    IsChecked = true;
                }
            }
            OnClick();
        }
        /// <summary>
        /// Builds the visual tree for the <see cref="T:System.Windows.Controls.HeaderedItemsControl"/> when a new template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            _dropdown = GetTemplateChild("PART_DropDown") as Popup;
            base.OnApplyTemplate();
            _host = GetTemplateChild("Bd") as Border;
            if (_host != null)
            {
                _host.MouseEnter += new MouseEventHandler(b_MouseEnter);
                _host.MouseLeave += new MouseEventHandler(b_MouseLeave);
            }
            VisualStateManager.GoToState(this, "Normal", true);
        }

        /// <summary>
        /// Called before the <see cref="E:System.Windows.UIElement.MouseEnter"/> event occurs.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            this.Focus();
            if (this.Items.Count <= 0)
            {
                this._dropdown.IsOpen = false;
            }
            var menuGroup = ((DropDownMenuItem)this).Parent as DropDownMenuGroup;

#if SILVERLIGHT
            if (menuGroup != null)
                menuGroup.HideMouseOverItems();

            this.IsMouseOver = true;


            _dropdown.HorizontalOffset = ActualWidth - 5;
#endif
            if (!_dropdown.IsOpen && Items.Count > 0)
            {
                _dropdown.IsOpen = true;
            }
            if (ParentRibbonMenuItem != null)
            {
                VisualStateManager.GoToState(ParentRibbonMenuItem, "MouseOver", true);
                ParentRibbonMenuItem._dropdown.IsOpen = true;
            }

            //VisualStateManager.GoToState(this, "MouseOver", true);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
#if SILVERLIGHT
            this.IsMouseOver = false;
#endif
            VisualStateManager.GoToState(this, "Normal", false);
            if (_dropdown.IsOpen)
            {
                //_dropdown.IsOpen = false;
            }

        }

        void b_MouseLeave(object sender, MouseEventArgs e)
        {
            base.OnMouseLeave(e);
        }

        void b_MouseEnter(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }
        #endregion
    }
}
