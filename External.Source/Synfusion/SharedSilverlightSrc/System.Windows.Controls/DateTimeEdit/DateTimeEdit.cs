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
using System.ComponentModel;
using System.Globalization;
using System.Windows.Controls.Primitives;
using System.Diagnostics;
using System.Collections.ObjectModel;
#if WPF
using Syncfusion.Licensing;
using Syncfusion.Windows.Controls;
#endif


#if WPF
namespace Syncfusion.Windows.Shared    
#endif

#if SILVERLIGHT
namespace Syncfusion.Windows.Tools.Controls
#endif
{
#if WPF

    [SkinType(SkinVisualStyle = Skin.Office2007Blue,
      Type = typeof(DateTimeEdit), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/DateTimeEdit/Themes/Office2007BlueStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2007Black,
    Type = typeof(DateTimeEdit), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/DateTimeEdit/Themes/Office2007BlackStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2007Silver,
    Type = typeof(DateTimeEdit), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/DateTimeEdit/Themes/Office2007SilverStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2010Blue,
      Type = typeof(DateTimeEdit), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/DateTimeEdit/Themes/Office2010BlueStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2010Black,
    Type = typeof(DateTimeEdit), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/DateTimeEdit/Themes/Office2010BlackStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2010Silver,
    Type = typeof(DateTimeEdit), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/DateTimeEdit/Themes/Office2010SilverStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2003,
    Type = typeof(DateTimeEdit), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/DateTimeEdit/Themes/Office2003Style.xaml")]
    [SkinType(SkinVisualStyle = Skin.Blend,
    Type = typeof(DateTimeEdit), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/DateTimeEdit/Themes/BlendStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.SyncOrange,
    Type = typeof(DateTimeEdit), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/DateTimeEdit/Themes/SyncOrangeStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.ShinyRed,
    Type = typeof(DateTimeEdit), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/DateTimeEdit/Themes/ShinyRedStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.ShinyBlue,
    Type = typeof(DateTimeEdit), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/DateTimeEdit/Themes/ShinyBlueStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Default,
    Type = typeof(DateTimeEdit), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/DateTimeEdit/Themes/Generic.xaml")]
    [SkinType(SkinVisualStyle = Skin.VS2010,
   Type = typeof(DateTimeEdit), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/DateTimeEdit/Themes/VS2010Style.xaml")]  
#endif
#if SILVERLIGHT
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Blend,
      Type = typeof(DateTimeEdit), XamlResource = "/Syncfusion.Theming.Blend;component/DateTimeEditThemes.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2007Blue,
        Type = typeof(DateTimeEdit), XamlResource = "/Syncfusion.Theming.Office2007Blue;component/DateTimeEditThemes.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2007Black,
        Type = typeof(DateTimeEdit), XamlResource = "/Syncfusion.Theming.Office2007Black;component/DateTimeEditThemes.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2007Silver,
        Type = typeof(DateTimeEdit), XamlResource = "/Syncfusion.Theming.Office2007Silver;component/DateTimeEditThemes.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Default,
        Type = typeof(DateTimeEdit), XamlResource = "/Syncfusion.Theming.Default;component/DateTimeEditThemes.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2010Blue,
        Type = typeof(DateTimeEdit), XamlResource = "/Syncfusion.Theming.Office2010Blue;component/DateTimeEditThemes.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2010Black,
        Type = typeof(DateTimeEdit), XamlResource = "/Syncfusion.Theming.Office2010Black;component/DateTimeEditThemes.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2010Silver,
        Type = typeof(DateTimeEdit), XamlResource = "/Syncfusion.Theming.Office2010Silver;component/DateTimeEditThemes.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Windows7,
        Type = typeof(DateTimeEdit), XamlResource = "/Syncfusion.Theming.Windows7;component/DateTimeEditThemes.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.VS2010,
      Type = typeof(DateTimeEdit), XamlResource = "/Syncfusion.Theming.VS2010;component/DateTimeEditThemes.xaml")]
#endif

    public class DateTimeEdit : DateTimeBase
    {
        #region Event

        ///// <summary>
        ///// Identifies <see cref="CalendarPopupOpened"/> event.
        ///// </summary>
        //public static readonly RoutedEvent CalendarPopupOpenedEvent = EventManager.RegisterRoutedEvent(
        //    "CalendarPopupOpened",
        //    RoutingStrategy.Bubble,
        //    typeof(EventHandler),
        //    typeof(DateTimeEdit));

        ///// <summary>
        ///// Bubbling routed event is fired before CalendarPopupOpened is changed.
        ///// </summary>
        //public event RoutedEventHandler CalendarPopupOpened
        //{
        //    add
        //    {
        //        AddHandler(CalendarPopupOpenedEvent, value);
        //    }

        //    remove
        //    {
        //        RemoveHandler(CalendarPopupOpenedEvent, value);
        //    }
        //}

        public event RoutedEventHandler CalendarPopupOpened;
#if WPF
        public event RoutedEventHandler ClockPopupOpenedEvent;
#endif
           public event RoutedEventHandler IncorrectDateInput;

        ///// <summary>
        ///// Identifies <see cref="ClockPopupOpened"/> event.
        ///// </summary>
        //public static readonly RoutedEvent ClockPopupOpenedEvent = EventManager.RegisterRoutedEvent(
        //    "ClockPopupOpened",
        //    RoutingStrategy.Bubble,
        //    typeof(EventHandler),
        //    typeof(DateTimeEdit));

        ///// <summary>
        ///// Bubbling routed event is fired before ClockPopupOpened is changed.
        ///// </summary>
        //public event RoutedEventHandler ClockPopupOpened
        //{
        //    add
        //    {
        //        AddHandler(ClockPopupOpenedEvent, value);
        //    }

        //    remove
        //    {
        //        RemoveHandler(ClockPopupOpenedEvent, value);
        //    }
        //}


        ///// <summary>
        ///// Identifies DateTimeEdit.IncorrectDateInput event.
        ///// </summary>
        //public static readonly RoutedEvent IncorrectDateInputEvent = EventManager.RegisterRoutedEvent(
        //    "IncorrectDateInput",
        //    RoutingStrategy.Bubble,
        //    typeof(EventHandler),
        //    typeof(DateTimeEdit));

        ///// <summary>
        ///// Bubbling routed event fired when IncorrectDateInput
        ///// is changed, i.e. when IncorrectDateInput is completed.
        ///// </summary>
        //public event RoutedEventHandler IncorrectDateInput
        //{
        //    add
        //    {
        //        AddHandler(IncorrectDateInputEvent, value);
        //    }

        //    remove
        //    {
        //        RemoveHandler(IncorrectDateInputEvent, value);
        //    }
        //}

       
        /// <summary>
        /// Event that is raised when CloseCalendarAction property is
        /// changed.
        /// </summary>
        [Obsolete("Event will not help due to internal arhitecture changes")]
        public event PropertyChangedCallback CloseCalendarActionChanged;

        /// <summary>
        /// Event that is raised when IsWatchEnabled property is changed.
        /// </summary>
         public event PropertyChangedCallback IsWatchEnabledChanged;

#if WPF
        public CloseCalendarAction CloseCalendarAction
        {
            get
            {
                return (CloseCalendarAction)GetValue(CloseCalendarActionProperty);
            }

            set
            {
                SetValue(CloseCalendarActionProperty, value);
            }
        }

        public static readonly DependencyProperty CloseCalendarActionProperty =
            DependencyProperty.Register("CloseCalendarAction",typeof(CloseCalendarAction),typeof(DateTimeEdit),new FrameworkPropertyMetadata(CloseCalendarAction.SingleClick, new PropertyChangedCallback(OnCloseCalendarActionChanged)));

        private static void OnCloseCalendarActionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DateTimeEdit instance = (DateTimeEdit)d;
            instance.OnCloseCalendarActionChanged(e);
        }

        protected virtual void OnCloseCalendarActionChanged(DependencyPropertyChangedEventArgs e)
        {
            if (CloseCalendarActionChanged != null)
            {
                CloseCalendarActionChanged(this, e);
            }
        }
#endif
        #endregion

        #region Events

        /// <summary>
        /// Event that is raised when DateTime property is changed.
        /// </summary>
        public event PropertyChangedCallback DateTimeChanged;

        /// <summary>
        /// Event that is raised when MaxDateTime property is changed.
        /// </summary>
        public event PropertyChangedCallback MaxDateTimeChanged;

        /// <summary>
        /// Event that is raised when MinDateTime property is changed.
        /// </summary>
        public event PropertyChangedCallback MinDateTimeChanged;
#if WPF
        /// <summary>
        /// Event that is raised when DropDownView property is changed.
        /// </summary>
        public event PropertyChangedCallback DropDownViewChanged;
#endif
        #endregion

        #region PrivateMembers
        internal bool mValueChanged = true;
        internal bool mIsLoaded = false;
        internal bool mtextboxclicked = false;
        internal DateTime? mValue;
        internal DateTime? OldValue;
        internal int mSelectedCollection;
        internal bool mTextInputpartended = true;
        internal string checktext;
        internal bool checkday;
        internal bool checkday1;
        internal bool checkday2;
        internal bool checkmonth;
        internal bool checkyear;
        internal string checkyear1;


        private ObservableCollection<DateTimeProperties> mDateTimeProperties = new ObservableCollection<DateTimeProperties>();
        #endregion

        [BrowsableAttribute(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal ObservableCollection<DateTimeProperties> DateTimeProperties
        {
            get { return mDateTimeProperties; }
            set { mDateTimeProperties = value; }
        }

        #region Constructor

#if WPF
        /// <summary>
        /// Initializes the <see cref="DateTimeEdit"/> class.
        /// </summary>
        static DateTimeEdit()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DateTimeEdit), new FrameworkPropertyMetadata(typeof(DateTimeEdit)));
           // EnvironmentTest.ValidateLicense(typeof(DateTimeEdit));
        }
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeEdit"/> class.
        /// </summary>
        public DateTimeEdit()
        {

#if WPF
            if (EnvironmentTest.IsSecurityGranted)
            {
                EnvironmentTest.StartValidateLicense(typeof(DateTimeEdit));
            }
#endif
#if SILVERLIGHT
            DefaultStyleKey = typeof(DateTimeEdit);
#endif
            this.SelectionChanged += new RoutedEventHandler(DateTimeEdit_SelectionChanged);
            this.Loaded+=new RoutedEventHandler(DateTimeEdit_Loaded);
            
        }

       
        #endregion

        /// <summary>
        /// Handles the Loaded event of the DateTimeEdit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        void DateTimeEdit_Loaded(object sender, RoutedEventArgs e)
        {
            this.mIsLoaded = true;
            object _value = CoerceDateTime(this, this.DateTime);
            System.DateTime? tempVal = (DateTime?)_value;
            //System.DateTime? tempVal = CoerceDateTime(this, this.DateTime);
            DateTimeProperties = DateTimeHandler.dateTimeHandler.CreateDateTimePatteren(this);

            this.CheckPopUpStatus(this);

            if (tempVal != this.DateTime)
            {
                this.DateTime = tempVal;
                return;
            }
            this.mValue = this.DateTime;
            this.LoadTextBox();            
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable classic style].
        /// </summary>
        /// <value><c>true</c> if [enable classic style]; otherwise, <c>false</c>.</value>
        public bool EnableClassicStyle
        {
            get { return (bool)GetValue(EnableClassicStyleProperty); }
            set { SetValue(EnableClassicStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable combined style].
        /// </summary>
        /// <value><c>true</c> if [enable combined style]; otherwise, <c>false</c>.</value>
        internal bool EnableCombinedStyle
        {
            get { return (bool)GetValue(EnableCombinedStyleProperty); }
            set { SetValue(EnableCombinedStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EnableClassicStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnableClassicStyleProperty =
            DependencyProperty.Register("EnableClassicStyle", typeof(bool), typeof(DateTimeEdit), new PropertyMetadata(false));

        // Using a DependencyProperty as the backing store for EnableClassicStyle.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty EnableCombinedStyleProperty =
            DependencyProperty.Register("EnableCombinedStyle", typeof(bool), typeof(DateTimeEdit), new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets a value indicating whether to enable delete key
        /// </summary>
        /// <value><c>true</c> if [enable delete key]; otherwise, <c>false</c>.</value>
        public bool EnableDeleteKey
        {
            get { return (bool)GetValue(EnableDeleteKeyProperty); }
            set { SetValue(EnableDeleteKeyProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to enable backspace key
        /// </summary>
        /// <value><c>true</c> if [enable backspace key]; otherwise, <c>false</c>.</value>
        public bool EnableBackspaceKey
        {
            get { return (bool)GetValue(EnableBackspaceKeyProperty); }
            set { SetValue(EnableBackspaceKeyProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty EnableDeleteKeyProperty =
            DependencyProperty.Register("EnableDeleteKey", typeof(bool), typeof(DateTimeEdit), new PropertyMetadata(false));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty EnableBackspaceKeyProperty =
            DependencyProperty.Register("EnableBackspaceKey", typeof(bool), typeof(DateTimeEdit), new PropertyMetadata(false));
        /// <summary>
        /// Gets or sets the on focus behavior.
        /// </summary>
        /// <value>The on focus behavior.</value>
        public OnFocusBehavior OnFocusBehavior
        {
            get { return (OnFocusBehavior)GetValue(OnFocusBehaviorProperty); }
            set { SetValue(OnFocusBehaviorProperty, value); }
        }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty OnFocusBehaviorProperty =
            DependencyProperty.Register("OnFocusBehavior", typeof(OnFocusBehavior), typeof(DateTimeEdit), new PropertyMetadata(OnFocusBehavior.CursorOnFirstCharacter));

        internal bool selectionChanged = true;
        void DateTimeEdit_SelectionChanged(object sender, RoutedEventArgs e)
        {
               if (selectionChanged == true)
                    DateTimeHandler.dateTimeHandler.HandleSelection(this);
                else
                    selectionChanged = true;
           
        }
        
        #region Methods
        public bool lp = false;
        public bool lp1 = false;

        /// <summary>
        /// Gets the culture.
        /// </summary>
        /// <returns></returns>
        internal CultureInfo GetCulture()
        {
            CultureInfo cultureInfo;
            if (CultureInfo != null)
            {
                cultureInfo = this.CultureInfo.Clone() as CultureInfo;
            }
            else
            {
                cultureInfo = CultureInfo.CurrentCulture.Clone() as CultureInfo;
                //cultureInfo = new CultureInfo("en-US");
            }

            if (this.DateTimeFormat != null)
            {
                cultureInfo.DateTimeFormat = this.DateTimeFormat;
               
            }

#if WPF                      
            this.IsCultureRightToLeft = cultureInfo.TextInfo.IsRightToLeft;
            if (!this.IsCultureRightToLeft)
            {
                //// if (lp < 0)

                if (lp == true || lp1 == true)
                {
                    if (this.FlowDirection == FlowDirection.LeftToRight)
                        this.FlowDirection = FlowDirection.RightToLeft;
                    else
                        this.FlowDirection = FlowDirection.LeftToRight;
                    lp = false; lp1 = false;
                }


            }

            if (this.IsCultureRightToLeft)
            {
                if (lp == true || lp1 == true)
                {
                    if (this.FlowDirection == FlowDirection.LeftToRight)
                        this.FlowDirection = FlowDirection.RightToLeft;
                    else
                        this.FlowDirection = FlowDirection.LeftToRight;
                    lp = false; lp1 = false;

                }
                if (this.FlowDirection == FlowDirection.LeftToRight)
                {
                    lp = !lp;

                    this.FlowDirection = FlowDirection.RightToLeft;
                }

                else if
                    (this.FlowDirection == FlowDirection.RightToLeft)
                {
                    lp1 = !lp1;

                    this.FlowDirection = FlowDirection.LeftToRight;
                }
            }
            
#endif 
            return cultureInfo;
        }

        /// <summary>
        /// Reloads the text box.
        /// </summary>
        internal void ReloadTextBox()
        {
            DateTimeProperties = DateTimeHandler.dateTimeHandler.CreateDateTimePatteren(this);
            this.LoadTextBox();
        }

        /// <summary>
        /// Validates the value.
        /// </summary>
        /// <param name="Val">The val.</param>
        /// <returns></returns>
        internal DateTime? ValidateValue(DateTime? Val)
        {
            if (Val != null)
            {
                if (Val > this.MaxDateTime)
                {
                    Val = this.MaxDateTime;
                }
                else if (mValue < this.MinDateTime)
                {
                    Val = this.MinDateTime;
                }
            }
            return Val;
        }

        /// <summary>
        /// Loads the on value changed.
        /// </summary>
        internal void LoadOnValueChanged()
        {
            DateTime preVal;
            if (DateTime.ToString() == "")
                preVal = System.DateTime.Now;
            else
                preVal = (DateTime)this.DateTime;
#if WPF
                int i = mSelectedCollection;
#endif

            this.MaskedText = DateTimeHandler.dateTimeHandler.CreateDisplayText(this);
#if WPF
                this.selectionChanged = false;
                this.Select(this.DateTimeProperties[i].StartPosition, this.DateTimeProperties[i].Lenghth);
                mSelectedCollection = i;
                this.selectionChanged = true;
#endif
#if SILVERLIGHT
            if (mSelectedCollection > -1)
                this.Select(this.DateTimeProperties[mSelectedCollection].StartPosition, this.DateTimeProperties[mSelectedCollection].Lenghth);
#endif
        }

        /// <summary>
        /// Loads the text box.
        /// </summary>
        internal void LoadTextBox()
        {
           
            this.UnderlyingDateTime = this.DateTime;
            if (this.DateTime != null)
            {
#if WPF
                int i = mSelectedCollection;
#endif
                this.MaskedText = DateTimeHandler.dateTimeHandler.CreateDisplayText(this);
#if WPF
                this.selectionChanged = false;
                if (i > -1 && i < this.DateTimeProperties.Count)
                {
                    this.Select(this.DateTimeProperties[i].StartPosition, this.DateTimeProperties[i].Lenghth);
                }
                mSelectedCollection = i;
                this.selectionChanged = true;
#endif
#if SILVERLIGHT
                if (mSelectedCollection > -1)
                {
                    if (this.IsEditable == true)
                        this.Select(this.DateTimeProperties[mSelectedCollection].StartPosition, this.DateTimeProperties[mSelectedCollection].Lenghth);
                }
#endif
                this.WatermarkVisibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                //this.MaskedText = "";
                this.WatermarkVisibility = System.Windows.Visibility.Visible;
            }
            //DateTime preVal;
            //if (DateTime == null)
            //{
            //    preVal = System.DateTime.Now;
            //}
            //else
            //{
            //    System.DateTime.TryParse(this.DateTime.ToString(), out preVal);
            //}
            //DateTimeFormatInfo datetimeFormat = this.GetCulture().DateTimeFormat as DateTimeFormatInfo;
            //mValue = preVal;
            //DateTimeProperties = DateTimeHandler.dateTimeHandler.CreateDateTimePatteren(this);

            //this.MaskedText = preVal.ToString(DateTimeProperties[DateTimeProperties.Count - 1].Pattern, datetimeFormat);
            //this.MaskedText = DateTimeHandler.dateTimeHandler.CreateDisplayText(this);
        }

        #endregion

        #region Popup Events
        ToggleButton PART_DropDown;
        Popup PART_Popup;
        Popup PART_CalenderPopup;
        Grid PART_PopupGrid;
        ToggleButton Button_Today;
        ToggleButton Button_NoDate;
        ToggleButton Button_Calender;
#if WPF
        CheckBox PART_Button_Calendar;
        Border PART_CalendarBorder;

        Grid PART_OptionGrid;
#endif
        Grid PART_PopupGrid_Classic;
        ToggleButton Button_Today_Classic;
        ToggleButton Button_NoDate_Classic;

        Canvas _outsidePopupCanvas;
        Canvas _OutsideCalendarPopupCanvas;
        Canvas _outsideCanvas;
        Grid _popupGrid;
        Grid _root;
#if SILVERLIGHT
        System.Windows.Controls.Calendar _calendar;
        System.Windows.Controls.Calendar Calendar_Classic;
#endif
#if WPF

        Syncfusion.Windows.Controls.Calendar _calendar;
        Syncfusion.Windows.Controls.Calendar _Calendar;

        /// <summary>
        /// Gets or sets the date time calender.
        /// </summary>
        /// <value>The date time calender.</value>
        public Syncfusion.Windows.Controls.Calendar DateTimeCalender
        {
            get
            {
                return _calendar;
            }
            set
            {
                _Calendar = value;
                if (Calendar_Classic != null)
                {
                    Calendar_Classic.CanBlockWeekEnds = _Calendar.CanBlockWeekEnds;
                    Calendar_Classic.BlackoutDates = _Calendar.BlackoutDates;
                }
            }
        }
        Syncfusion.Windows.Controls.Calendar Calendar_Classic;
        Popup PART_WatchPopup;
        Clock clock;
        ToggleButton Button_Clock;
        CheckBox PART_Button_Clock;
        Border PART_ClockBorder;
#endif

        /// <summary>
        /// Popup_s the on apply template.
        /// </summary>
        private void Popup_OnApplyTemplate()
        {

#if WPF
            if (Button_Clock != null)
            {
                this.Button_Clock.Checked -= new RoutedEventHandler(Button_Clock_Checked);
                this.Button_Clock.Unchecked -= new RoutedEventHandler(Button_Clock_Unchecked);
            }

            if (PART_Button_Clock != null)
            {
                this.PART_Button_Clock.Checked -= new RoutedEventHandler(Button_Clock_Checked);
                this.PART_Button_Clock.Unchecked -= new RoutedEventHandler(Button_Clock_Unchecked);
            }

            //if(System.ComponentModel.DesignerProperties.
            if (clock != null)
                clock.DateTimeChanged -= new PropertyChangedCallback(clock_DateTimeChanged);
            
            PART_WatchPopup = this.GetTemplateChild("PART_WatchPopup") as Popup;
            Button_Clock = this.GetTemplateChild("Button_Clock") as ToggleButton;
            PART_Button_Clock = this.GetTemplateChild("Button_Clock") as CheckBox;
            PART_ClockBorder = this.GetTemplateChild("ClockBorder") as Border;
            if (Button_Clock != null)
            {
                this.Button_Clock.Checked += new RoutedEventHandler(Button_Clock_Checked);
                this.Button_Clock.Unchecked += new RoutedEventHandler(Button_Clock_Unchecked);
            }

            if (PART_Button_Clock != null)
            {
                this.PART_Button_Clock.Checked += new RoutedEventHandler(Button_Clock_Checked);
                this.PART_Button_Clock.Unchecked += new RoutedEventHandler(Button_Clock_Unchecked);
            }

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                clock = this.GetTemplateChild("Clock") as Clock;
                if (clock != null)
                    clock.DateTimeChanged += new PropertyChangedCallback(clock_DateTimeChanged);
            }
#endif
            if (this.PART_Popup != null)
            {
                this.PART_Popup.Closed -= new EventHandler(PART_Popup_Closed);
            }

            if (PART_DropDown != null)
            {
                PART_DropDown.Checked -= new RoutedEventHandler(PART_DropDown_Checked);
                PART_DropDown.Unchecked -= new RoutedEventHandler(PART_DropDown_Unchecked);
            }

            if (PART_DropDown != null)
            {
                PART_DropDown.Checked -= new RoutedEventHandler(PART_DropDown_Checked);
                PART_DropDown.Unchecked -= new RoutedEventHandler(PART_DropDown_Unchecked);
            }

            if (Button_Calender != null)
            {
                Button_Calender.Checked -= new RoutedEventHandler(Button_Calender_Checked);
                Button_Calender.Unchecked -= new RoutedEventHandler(Button_Calender_Unchecked);
            }
#if WPF
            if (PART_Button_Calendar != null)
            {
                PART_Button_Calendar.Checked -= new RoutedEventHandler(Button_Calender_Checked);
                PART_Button_Calendar.Unchecked -= new RoutedEventHandler(Button_Calender_Unchecked);
            }
#endif
            if (Button_NoDate != null)
                Button_NoDate.Click -= new RoutedEventHandler(Button_NoDate_Click);

            if (Button_Today_Classic != null)
                Button_Today_Classic.Click -= new RoutedEventHandler(Button_Today_Click);

            if (Button_NoDate_Classic != null)
                Button_NoDate_Classic.Click -= new RoutedEventHandler(Button_NoDate_Click);

            if (_calendar != null)
                _calendar.SelectedDatesChanged -= new EventHandler<SelectionChangedEventArgs>(calender_SelectedDatesChanged);
            
            if (Calendar_Classic != null)
                Calendar_Classic.SelectedDatesChanged -= new EventHandler<SelectionChangedEventArgs>(calender_SelectedDatesChanged);

            if (_outsidePopupCanvas != null)
                _outsidePopupCanvas.MouseLeftButtonDown -= new MouseButtonEventHandler(_outsidePopupCanvas_MouseLeftButtonDown);

            if (this._OutsideCalendarPopupCanvas != null)
                _OutsideCalendarPopupCanvas.MouseLeftButtonDown -= new MouseButtonEventHandler(_OutsideCalendarPopupCanvas_MouseLeftButtonDown);
     
            PART_DropDown = this.GetTemplateChild("PART_DropDown") as ToggleButton;

         
            PART_Popup = this.GetTemplateChild("PART_Popup") as Popup;
            PART_CalenderPopup = this.GetTemplateChild("PART_CalenderPopup") as Popup;
            PART_PopupGrid = this.GetTemplateChild("PART_PopupGrid") as Grid;
            Button_Today = this.GetTemplateChild("Button_Today") as ToggleButton;
            Button_NoDate = this.GetTemplateChild("Button_NoDate") as ToggleButton;
            Button_Calender = this.GetTemplateChild("Button_Calender") as ToggleButton;
#if WPF
            PART_Button_Calendar = this.GetTemplateChild("Button_Calender") as CheckBox;
            PART_CalendarBorder = this.GetTemplateChild("CalendarBorder") as Border;

            PART_OptionGrid = this.GetTemplateChild("PART_OptionGrid") as Grid;
#endif
            PART_PopupGrid_Classic = this.GetTemplateChild("PART_PopupGrid_Classic") as Grid;
            Button_Today_Classic = this.GetTemplateChild("Button_Today_Classic") as ToggleButton;
            Button_NoDate_Classic = this.GetTemplateChild("Button_NoDate_Classic") as ToggleButton;

#if SILVERLIGHT
            Calendar_Classic = this.GetTemplateChild("Calendar_Classic") as System.Windows.Controls.Calendar;
            _calendar = this.GetTemplateChild("Calendar") as System.Windows.Controls.Calendar;
#endif
#if WPF
            _calendar = this.GetTemplateChild("Calendar") as Syncfusion.Windows.Controls.Calendar;
            Calendar_Classic = this.GetTemplateChild("Calendar_Classic") as Syncfusion.Windows.Controls.Calendar;
#endif
#if WPF
            if (_Calendar != null)
            {
                if (_Calendar != null)
                {
                    _calendar.BlackoutDates = _Calendar.BlackoutDates;
                    _calendar.CanBlockWeekEnds = _Calendar.CanBlockWeekEnds;
                }
                if (Calendar_Classic != null)
                {
                    Calendar_Classic.BlackoutDates = _Calendar.BlackoutDates;
                    Calendar_Classic.CanBlockWeekEnds = _Calendar.CanBlockWeekEnds;
                }
            }
#endif
            if (PART_DropDown != null)
            {
                PART_DropDown.Checked += new RoutedEventHandler(PART_DropDown_Checked);
                PART_DropDown.Unchecked += new RoutedEventHandler(PART_DropDown_Unchecked);
            }

            if(Button_Today_Classic !=null)
                Button_Today_Classic.Click += new RoutedEventHandler(Button_Today_Click);

            if (Button_NoDate != null)
                Button_NoDate.Click += new RoutedEventHandler(Button_NoDate_Click);

            if(Button_NoDate_Classic!= null)
                Button_NoDate_Classic.Click += new RoutedEventHandler(Button_NoDate_Click);

            if (Button_Calender != null)
            {
                Button_Calender.Checked += new RoutedEventHandler(Button_Calender_Checked);
                Button_Calender.Unchecked += new RoutedEventHandler(Button_Calender_Unchecked);
            }
#if WPF
            if (PART_Button_Calendar != null)
            {
                PART_Button_Calendar.Checked += new RoutedEventHandler(Button_Calender_Checked);
                PART_Button_Calendar.Unchecked += new RoutedEventHandler(Button_Calender_Unchecked);
            }
#endif
            if (_calendar != null)
                _calendar.SelectedDatesChanged += new EventHandler<SelectionChangedEventArgs>(calender_SelectedDatesChanged);
            
            if(Calendar_Classic != null)
                Calendar_Classic.SelectedDatesChanged += new EventHandler<SelectionChangedEventArgs>(calender_SelectedDatesChanged);

            _root = this.GetTemplateChild("RootElement") as Grid;
            _outsideCanvas = this.GetTemplateChild("OutsideCanvas") as Canvas;
            _outsidePopupCanvas = this.GetTemplateChild("OutsidePopupCanvas") as Canvas;
            _popupGrid = this.GetTemplateChild("PART_PopupGrid") as Grid;
            _OutsideCalendarPopupCanvas = this.GetTemplateChild("OutsideCalendarPopupCanvas") as Canvas;

            if (this._OutsideCalendarPopupCanvas != null)
                _OutsideCalendarPopupCanvas.MouseLeftButtonDown += new MouseButtonEventHandler(_OutsideCalendarPopupCanvas_MouseLeftButtonDown);

            if (this._outsidePopupCanvas != null)
                _outsidePopupCanvas.MouseLeftButtonDown += new MouseButtonEventHandler(_outsidePopupCanvas_MouseLeftButtonDown);

            if (this.PART_Popup != null)
            {
                this.PART_Popup.Closed += new EventHandler(PART_Popup_Closed);
            }
        }

        /// <summary>
        /// Handles the Closed event of the PART_Popup control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void PART_Popup_Closed(object sender, EventArgs e)
        {
            DT.Tick -= new EventHandler(DT_Tick);   
            this.Focus();
            this.PART_DropDown.IsChecked = false;
        }

        System.Windows.Threading.DispatcherTimer DT = new System.Windows.Threading.DispatcherTimer();

        /// <summary>
        /// Handles the Checked event of the PART_DropDown control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        void PART_DropDown_Checked(object sender, RoutedEventArgs e)
        {
            DT.Interval = this.PopupDelay;
            DT.Tick += new EventHandler(DT_Tick);
            DT.Start();
        }

        /// <summary>
        /// Handles the Tick event of the DT control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void DT_Tick(object sender, EventArgs e)
        {
            this.IsDropDownOpen = true;
            this.OpenPopup();
            DT.Stop();
        }

        /// <summary>
        /// Handles the Click event of the Button_NoDate control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        void Button_NoDate_Click(object sender, RoutedEventArgs e)
        {
            this.DateTime = null;
            this.IsDropDownOpen = false;
            ClosePopup();
        }

        /// <summary>
        /// Handles the Click event of the Button_Today control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        void Button_Today_Click(object sender, RoutedEventArgs e)
        {
            this.DateTime = System.DateTime.Today;
            this.IsDropDownOpen = false;
            ClosePopup();
        }

        /// <summary>
        /// Handles the MouseLeftButtonDown event of the _OutsideCalendarPopupCanvas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        void _OutsideCalendarPopupCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.IsDropDownOpen = false;
            ClosePopup();
        }

#if WPF
        /// <summary>
        /// Clock_s the date time changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        void clock_DateTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (this.PART_WatchPopup != null)
            {
                if (this.PART_Popup.IsOpen == true && this.PART_WatchPopup.IsOpen == true)
                {

                    if (this.DateTime != null)
                    {
                        //this.clock.DateTime = (DateTime)this.DateTime;
                        this.DateTime = new DateTime(DateTime.Value.Year, DateTime.Value.Month, DateTime.Value.Day, this.clock.DateTime.Hour, this.clock.DateTime.Minute, this.clock.DateTime.Second, this.clock.DateTime.Millisecond);
                    }
                    else
                    {
                        this.DateTime = this.clock.DateTime;
                    }
                }
            }
            else
            {
                if (this.PART_Popup.IsOpen == true)
                {

                    if (this.DateTime != null)
                    {
                        //this.clock.DateTime = (DateTime)this.DateTime;
                        this.DateTime = new DateTime(DateTime.Value.Year, DateTime.Value.Month, DateTime.Value.Day, this.clock.DateTime.Hour, this.clock.DateTime.Minute, this.clock.DateTime.Second, this.clock.DateTime.Millisecond);
                    }
                    else
                    {
                        this.DateTime = this.clock.DateTime;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Unchecked event of the Button_Clock control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        void Button_Clock_Unchecked(object sender, RoutedEventArgs e)
        {
            if (PART_WatchPopup != null)
            {
                PART_WatchPopup.IsOpen = false;
            }
            if (PART_ClockBorder != null)
            {
                PART_ClockBorder.Visibility = Visibility.Collapsed;
                if (PART_OptionGrid != null)
                {
                    PART_OptionGrid.HorizontalAlignment = HorizontalAlignment.Center;
                }
            }
        }

        /// <summary>
        /// Handles the Checked event of the Button_Clock control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        void Button_Clock_Checked(object sender, RoutedEventArgs e)
        {
            if (!EnableCombinedStyle)
            {
                if (Button_Calender != null)
                    Button_Calender.IsChecked = false;
            }
            if (PART_ClockBorder != null)
            {
                PART_ClockBorder.Visibility = Visibility.Visible;
                if (PART_OptionGrid != null)
                {
                    if (PART_Button_Calendar.IsChecked==true && PART_Button_Clock.IsChecked==true)
                    {
                        PART_OptionGrid.HorizontalAlignment = HorizontalAlignment.Center;
                    }
                }
                else if (PART_Button_Clock != null)
                {
                    if (PART_Button_Clock.IsChecked==true)
                    {
                        PART_OptionGrid.HorizontalAlignment = HorizontalAlignment.Center;
                    }
                }
            }

            if (PART_ClockBorder != null)
            {
                PART_ClockBorder.Visibility = Visibility.Visible;
            }
            if (PART_WatchPopup != null)
            {
                PART_WatchPopup.IsOpen = true;
                if(this.ClockPopupOpenedEvent!=null)
                    this.ClockPopupOpenedEvent(this, new RoutedEventArgs());
            }
        }
#endif

        /// <summary>
        /// Handles the Unchecked event of the Button_Calender control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        void Button_Calender_Unchecked(object sender, RoutedEventArgs e)
        {
#if WPF
            if (PART_CalendarBorder != null)
            {
                PART_CalendarBorder.Visibility = Visibility.Collapsed;
                if (PART_OptionGrid != null)
                {
                    PART_OptionGrid.HorizontalAlignment = HorizontalAlignment.Left;
                }
            }
#endif
            if (this.PART_CalenderPopup != null)
                this.PART_CalenderPopup.IsOpen = false;
        }

        /// <summary>
        /// Handles the Checked event of the Button_Calender control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        void Button_Calender_Checked(object sender, RoutedEventArgs e)
        {
#if WPF
            if (PART_CalendarBorder != null)
            {
                PART_CalendarBorder.Visibility = Visibility.Visible;
                if (PART_OptionGrid != null)
                {

                    if (PART_Button_Calendar != null && PART_Button_Clock != null)
                    {
                        if (PART_Button_Calendar.IsChecked==true && PART_Button_Clock.IsChecked==true)
                        {
                            PART_OptionGrid.HorizontalAlignment = HorizontalAlignment.Center;
                        }
                    }

                    if (PART_Button_Calendar != null)
                    {
                        if (PART_Button_Calendar.IsChecked==true)
                        {
                            PART_OptionGrid.HorizontalAlignment = HorizontalAlignment.Center;
                        }
                    }
                }
            }
#endif
            if (this.PART_CalenderPopup != null)
            {
#if SILVERLIGHT
                if (_calendar != null)
                {
                    _calendar.DisplayMode = DefaultCalendarDisplayMode;
                    if (DateTime == null)
                    {
                        _calendar.DisplayDate = System.DateTime.Now;
                    }
                }
                if (Calendar_Classic != null)
                {
                    Calendar_Classic.DisplayMode = DefaultCalendarDisplayMode;
                    if (DateTime == null)
                    {
                        Calendar_Classic.DisplayDate = System.DateTime.Now;
                    }
                }
#endif
                this.PART_CalenderPopup.IsOpen = true;
                if (this.CalendarPopupOpened != null)
                    this.CalendarPopupOpened(this, new RoutedEventArgs());
            }
#if WPF
            if (!EnableCombinedStyle)
            {
                if (Button_Clock != null)
                    Button_Clock.IsChecked = false;
            }
#endif
        }

        /// <summary>
        /// Handles the Unchecked event of the PART_DropDown control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        void PART_DropDown_Unchecked(object sender, RoutedEventArgs e)
        {
            this.IsDropDownOpen = false;
            ClosePopup();
        }

        /// <summary>
        /// Opens the popup.
        /// </summary>
        public void OpenPopup()
        {

            if (!EnableClassicStyle && this.PART_PopupGrid!=null)
            {
                this.IsDropDownOpen = true;

                this.PART_PopupGrid.Visibility = Visibility.Collapsed;
#if WPF
                if (DropDownView == DropDownViews.Combined)
                {
                    this.PART_PopupGrid.Visibility = Visibility.Visible;
                    PART_Popup.Visibility = Visibility.Visible;
                    PART_Popup.IsOpen = true;
                        if (PART_Button_Calendar != null && PART_OptionGrid!=null)
                        {
                            if (PART_Button_Calendar.IsChecked == true && PART_CalendarBorder!=null)
                            {
                                PART_CalendarBorder.Visibility = Visibility.Visible;
                            }
                            else if(PART_Button_Calendar.IsChecked == false && PART_CalendarBorder!=null)
                            {
                                PART_CalendarBorder.Visibility = Visibility.Collapsed;
                            }
                            PART_OptionGrid.HorizontalAlignment = HorizontalAlignment.Left;
                        }

                        if (PART_Button_Clock != null && PART_OptionGrid!=null)
                        {
                            if (PART_Button_Calendar.IsChecked == true && PART_ClockBorder != null)
                            {
                                PART_ClockBorder.Visibility = Visibility.Visible;
                            }
                            else if (PART_Button_Calendar.IsChecked == false && PART_ClockBorder != null)
                            {
                                PART_ClockBorder.Visibility = Visibility.Collapsed;
                            }
                            PART_OptionGrid.HorizontalAlignment = HorizontalAlignment.Left;
                        }

                        if (PART_Button_Calendar != null && PART_Button_Clock != null&&PART_OptionGrid!=null)
                        {
                            if (PART_Button_Clock.IsChecked==true &&PART_Button_Calendar.IsChecked==true)
                            {
                                PART_OptionGrid.HorizontalAlignment = HorizontalAlignment.Center;
                            }
                        }

                }
                else
                {
#endif
                    if (PART_PopupGrid_Classic != null)
                    {
                        this.PART_PopupGrid_Classic.Visibility = Visibility.Visible;
                        if (IsEmptyDateEnabled)
                            Grid.SetColumnSpan(Button_Today_Classic, 1);
                        else
                            Grid.SetColumnSpan(Button_Today_Classic, 2);
                    }
#if WPF
                }
#endif
            }
            else
            {
                if (this.PART_PopupGrid != null)
                    this.PART_PopupGrid.Visibility = Visibility.Visible;
                if (PART_PopupGrid_Classic != null)
                    this.PART_PopupGrid_Classic.Visibility = Visibility.Collapsed;
            }

            if (this._calendar != null)
            {

                this._calendar.DisplayDateStart = this.MinDateTime;
                this._calendar.DisplayDateEnd = this.MaxDateTime;
                this._calendar.SelectedDate = this.DateTime;
                if (this.DateTime != null)
                    this._calendar.DisplayDate = (DateTime)this.DateTime;
                else
                    this._calendar.DisplayDate = System.DateTime.Now;
            }

            if (this.Calendar_Classic != null)
            {
                this.Calendar_Classic.DisplayDateStart = this.MinDateTime;
                this.Calendar_Classic.DisplayDateEnd = this.MaxDateTime;
                this.Calendar_Classic.SelectedDate = this.DateTime;
                if (this.DateTime != null)
                    this.Calendar_Classic.DisplayDate = (DateTime)this.DateTime;
                else
                    this.Calendar_Classic.DisplayDate = System.DateTime.Now;
            }

#if SILVERLIGHT
            FrameworkElement page = (Application.Current != null) ? Application.Current.RootVisual as FrameworkElement : null;
            if (page != null && this.PART_Popup != null)
            {
                //if (this.IsEmptyDateEnabled == false && this.IsCalendarEnabled)
                //{
                //    SetCalendarPopUpPosition();
                //    this.PART_CalenderPopup.IsOpen = true;
                //    return;
                //}
                SetPopUpPosition();
#endif
#if WPF
            if (this.PART_Popup != null)
            {
#endif
                this.PART_Popup.IsOpen = true;
            }
            if (this.DateTimeProperties.Count > 0)
                this.DateTimeProperties[this.DateTimeProperties.Count - 1].KeyPressCount = 0;
        }

        /// <summary>
        /// Closes the popup.
        /// </summary>
        public void ClosePopup()
        {
            this.IsDropDownOpen = false;

            if (this.PART_DropDown != null)
                this.PART_DropDown.IsChecked = false;

            if (this.PART_Popup != null)
                this.PART_Popup.IsOpen = false;

            if (this.PART_CalenderPopup != null)
                this.PART_CalenderPopup.IsOpen = false;

#if WPF
            if (this.PART_WatchPopup != null)
                this.PART_WatchPopup.IsOpen = false;
#endif
        }

        /// <summary>
        /// Handles the MouseLeftButtonDown event of the _outsidePopupCanvas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        void _outsidePopupCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.IsDropDownOpen = false;
            ClosePopup();
        }

        /// <summary>
        /// Handles the SelectedDatesChanged event of the calender control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.SelectionChangedEventArgs"/> instance containing the event data.</param>
        void calender_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            
#if SILVERLIGHT
            System.Windows.Controls.Calendar _calender = (System.Windows.Controls.Calendar)sender;
#endif
#if WPF
        Syncfusion.Windows.Controls.Calendar _calender = (Syncfusion.Windows.Controls.Calendar)sender;
#endif
            if (_calender.SelectedDate >= this.MinDateTime && _calender.SelectedDate <= this.MaxDateTime)
            {
               
                    if (this.DateTime != null)
                    {
                        this.DateTime = new DateTime(_calender.SelectedDate.Value.Year, _calender.SelectedDate.Value.Month, _calender.SelectedDate.Value.Day,
                            DateTime.Value.Hour, DateTime.Value.Minute, DateTime.Value.Second, DateTime.Value.Millisecond);

                    }
                    else
                    {
                        this.DateTime = _calender.SelectedDate;
                    }
                }

            this.IsDropDownOpen = false;
            ClosePopup();
        }
        #endregion

        #region Overrides

#if WPF
        /// <summary>
        /// Raises the <see cref="E:ContextMenuOpening"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Controls.ContextMenuEventArgs"/> instance containing the event data.</param>
        protected override void OnContextMenuOpening(ContextMenuEventArgs e)
        {
            e.Handled = true;
            base.OnContextMenuOpening(e);
        }

        protected override void OnDrop(DragEventArgs e)
        {
            e.Handled = true;
            base.OnDrop(e);
        }
#endif
        RepeatButton PART_UpButton;
        RepeatButton PART_DownButton;
        //ToggleButton PART_UpArrow;
        //ToggleButton PART_DownArrow;


        /// <summary>
        /// Is called when a control template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

#if SILVERLIGHT
            Calendar_Classic = this.GetTemplateChild("Calendar_Classic") as System.Windows.Controls.Calendar;
            _calendar = this.GetTemplateChild("Calendar") as System.Windows.Controls.Calendar;
#endif
#if WPF
            _calendar = this.GetTemplateChild("Calendar") as Syncfusion.Windows.Controls.Calendar;
            Calendar_Classic = this.GetTemplateChild("Calendar_Classic") as Syncfusion.Windows.Controls.Calendar;
#endif
            if (PART_UpButton != null)
                 PART_UpButton.Click -= new RoutedEventHandler(PART_UpButton_Click);
            if (PART_DownButton != null)
                 PART_DownButton.Click -= new RoutedEventHandler(PART_DownButton_Click);
            //PART_UpArrow = this.GetTemplateChild("PART_UpArrow") as ToggleButton;
            //PART_DownArrow = this.GetTemplateChild("PART_DownArrow") as ToggleButton;
            PART_UpButton = this.GetTemplateChild("PART_UpArrow") as RepeatButton;
            PART_DownButton = this.GetTemplateChild("PART_DownArrow") as RepeatButton;

            if (PART_UpButton != null)
                PART_UpButton.Click += new RoutedEventHandler(PART_UpButton_Click);
            if (PART_DownButton != null)              
                PART_DownButton.Click += new RoutedEventHandler(PART_DownButton_Click);
       
            this.Popup_OnApplyTemplate();
        }

        /// <summary>
        /// Handles the Click event of the PART_DownButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        void PART_DownButton_Click(object sender, RoutedEventArgs e)
        {
            this.mTextInputpartended = false;
            KeyHandler.keyHandler.HandleDownKey(this);          
            if (mSelectedCollection >= 0 && mSelectedCollection <= this.DateTimeProperties.Count)
            {
                if (this.IsEditable == true)
                {
                    this.SelectionStart = this.DateTimeProperties[mSelectedCollection].StartPosition;
                    this.SelectionLength = this.DateTimeProperties[mSelectedCollection].Lenghth;
                }
            }         
        }

        /// <summary>
        /// Handles the Click event of the PART_UpButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        void PART_UpButton_Click(object sender, RoutedEventArgs e)
        {
            this.mTextInputpartended = false;
            KeyHandler.keyHandler.HandleUpKey(this);          
            if (mSelectedCollection >= 0 && mSelectedCollection <= this.DateTimeProperties.Count)
            {
                if (this.IsEditable == true)
                {
                    this.SelectionStart = this.DateTimeProperties[mSelectedCollection].StartPosition;
                    this.SelectionLength = this.DateTimeProperties[mSelectedCollection].Lenghth;
                }
            }          
        }


        /// <summary>
        /// Invoked whenever an unhandled <see cref="E:System.Windows.Input.TextCompositionManager.TextInput"/> attached routed event reaches an element derived from this class in its route. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">Provides data about the event.</param>
        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            e.Handled = DateTimeHandler.dateTimeHandler.MatchWithMask(this, e.Text);
            base.OnTextInput(e);
        }
                
#if WPF
        /// <summary>
        /// Called when the <see cref="E:System.Windows.UIElement.KeyDown"/> occurs.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
#endif
#if SILVERLIGHT
        protected override void OnKeyDown(KeyEventArgs e)
        {
#endif      
            if (ModifierKeys.Control == Keyboard.Modifiers)
            {
                if (e.Key == Key.V)
                {
                    DateTime dt= new DateTime();
                    bool errorflag = false;
                    try
                    {
                        dt = Convert.ToDateTime(Clipboard.GetText());
                    }
                    catch (Exception) {
                        errorflag = true;
                    }
                    if (!errorflag)
                        this.DateTime = dt;
                    e.Handled = true; 
                                 
                }
                if (e.Key == Key.C)
                {
                }
                if (e.Key == Key.Z)
                {
                    this.DateTime = this.OldValue;
                    e.Handled = true;
                }
                if (e.Key == Key.X)
                {
                    e.Handled = true;
                }
            }
            else
            {
                e.Handled = KeyHandler.keyHandler.HandleKeyDown(this, e);
            }
            if (this.Focus() == true)
            {
                if (this.SelectionStart + this.SelectionLength <= this.Text.Length)
                {
                    if (this.SelectionStart > 0)
                    {
                        if (ModifierKeys.Shift == Keyboard.Modifiers)
                        {
                            if (e.Key == Key.Tab)
                            {
                                KeyHandler.keyHandler.HandleLeftKey(this);
                                e.Handled = true;
                            }
                        }

                    }
                    if (this.SelectionStart + this.SelectionLength < this.Text.Length)
                    {
                        if (ModifierKeys.Shift != Keyboard.Modifiers)
                        {
                            if (e.Key == Key.Tab)
                            {
                                KeyHandler.keyHandler.HandleRightKey(this);

                                e.Handled = true;
                            }
                        }
                    }
                }
                
            }
#if WPF
            base.OnPreviewKeyDown(e);
#endif
#if SILVERLIGHT
            base.OnKeyDown(e);
#endif
        }

#if WPF

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.PreviewMouseWheel"/> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseWheelEventArgs"/> that contains the event data.</param>
        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
#endif
#if SILVERLIGHT
        protected override void OnMouseWheel(MouseWheelEventArgs e)
#endif
        {
            if (e.Delta > 0)
            {
                KeyHandler.keyHandler.HandleUpKey(this);
            }
            if (e.Delta < 0)
            {
                KeyHandler.keyHandler.HandleDownKey(this);
            }
            e.Handled = true;
            base.OnMouseWheel(e);
        }
#if WPF

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.UIElement.PreviewMouseLeftButtonDown"/> routed event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs"/> that contains the event data. The event data reports that the left mouse button was pressed.</param>
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            this.mTextInputpartended = true;
            var typeName = e.OriginalSource.GetType().FullName;
            if (typeName == "System.Windows.Controls.TextBoxView")
            {
                mtextboxclicked = true;
            }

            if (this.SelectionLength == this.Text.Length)
            {
                DateTimeHandler.dateTimeHandler.HandleSelection(this);
            }
            
            base.OnPreviewMouseLeftButtonDown(e);
          }
#endif

        
        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.UIElement.MouseLeftButtonDown"/> routed event is raised on this element. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs"/> that contains the event data. The event data reports that the left mouse button was pressed.</param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            var typeName = e.OriginalSource.GetType().FullName;
#if WPF
            if (typeName == "System.Windows.Controls.TextBoxView")
            {
                mtextboxclicked = true;
            }
#endif
#if SILVERLIGHT
            if (typeName == "MS.Internal.TextBoxView")
            {
                mtextboxclicked = true;
            }
#endif
            if (this.IsPopupEnabled == true)
                this.OpenPopup();
        }


        /// <summary>
        /// Invoked whenever an unhandled <see cref="E:System.Windows.UIElement.GotFocus"/> event reaches this element in its route.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.RoutedEventArgs"/> that contains the event data.</param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            if (this.OnFocusBehavior == OnFocusBehavior.CursorOnFirstCharacter)
            {
                if (this.IsEditable == true)
                {
                    this.SelectionStart = 0;
                }
            }
            else if (this.OnFocusBehavior == OnFocusBehavior.CursorAtEnd)
            {
                if (this.IsEditable == true)
                    this.SelectionStart = this.Text.Length;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.UIElement.LostFocus"/> event (using the provided arguments).
        /// </summary>
        /// <param name="e">Provides data about the event.</param>
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            //Debug.WriteLine("Last Focus");
        }


        /// <summary>
        /// Bases the properties changed.
        /// </summary>
        protected override void BasePropertiesChanged()
        {
            base.BasePropertiesChanged();
            if (this.IsEmptyDateEnabled == false)
            {
                object _value = CoerceDateTime(this, this.DateTime);
                System.DateTime? datetime = (DateTime?)_value;
                //System.DateTime? datetime = CoerceDateTime(this, this.DateTime);
                if (datetime != this.DateTime)
                {
                    this.DateTime = datetime;
                }
            }
            if (mIsLoaded)
                this.ReloadTextBox();
        }
        #endregion

        #region Popup Position
#if SILVERLIGHT

        private void SetPopUpPosition()
        {
            if (this._calendar != null && Application.Current != null && Application.Current.Host != null && Application.Current.Host.Content != null)
            {
                double pageHeight = Application.Current.Host.Content.ActualHeight;
                double pageWidth = Application.Current.Host.Content.ActualWidth;
                //double calendarHeight = this.calender.ActualHeight;
                double calendarHeight = this.PART_PopupGrid.ActualHeight;
                double actualHeight = this.ActualHeight;

                if (this._root != null)
                {
                    GeneralTransform gt = this._root.TransformToVisual(null);

                    if (gt != null)
                    {
                        Point point00 = new Point(0, 0);
                        Point point10 = new Point(1, 0);
                        Point point01 = new Point(0, 1);
                        Point transform00 = gt.Transform(point00);
                        Point transform10 = gt.Transform(point10);
                        Point transform01 = gt.Transform(point01);

                        double dpX = transform00.X;
                        double dpY = transform00.Y;

                        double calendarX = dpX;
                        double calendarY = dpY + actualHeight;

                        // if the page height is less then the total height of
                        // the PopUp + DatePicker or if we can fit the PopUp
                        // inside the page, we want to place the PopUp to the
                        // bottom
                        if (pageHeight < calendarY + calendarHeight)
                        {
                            calendarY = dpY - calendarHeight;
                        }
                        this.PART_Popup.HorizontalOffset = 0;
                        this.PART_Popup.VerticalOffset = 0;
                        this._outsidePopupCanvas.Width = pageWidth;
                        this._outsidePopupCanvas.Height = pageHeight;
                        this.PART_Popup.HorizontalAlignment = HorizontalAlignment.Left;
                        this.PART_Popup.VerticalAlignment = VerticalAlignment.Top;
                        Canvas.SetLeft(this.PART_Popup, calendarX - dpX);
                        Canvas.SetTop(this.PART_Popup, calendarY - dpY);

                        // Transform the invisible canvas to plugin coordinate
                        // space origin
                        Matrix transformToRootMatrix = Matrix.Identity;
                        transformToRootMatrix.M11 = transform10.X - transform00.X;
                        transformToRootMatrix.M12 = transform10.Y - transform00.Y;
                        transformToRootMatrix.M21 = transform01.X - transform00.X;
                        transformToRootMatrix.M22 = transform01.Y - transform00.Y;
                        transformToRootMatrix.OffsetX = transform00.X;
                        transformToRootMatrix.OffsetY = transform00.Y;
                        MatrixTransform mt = new MatrixTransform();
                        InvertMatrix(ref transformToRootMatrix);
                        mt.Matrix = transformToRootMatrix;
                        this._outsidePopupCanvas.RenderTransform = mt;
                    }
                }
            }
        }

        private void SetCalendarPopUpPosition()
        {
            if (this._calendar != null && Application.Current != null && Application.Current.Host != null && Application.Current.Host.Content != null)
            {
                double pageHeight = Application.Current.Host.Content.ActualHeight;
                double pageWidth = Application.Current.Host.Content.ActualWidth;
                //double calendarHeight = this.calender.ActualHeight;
                double calendarHeight = this.PART_PopupGrid.ActualHeight;
                double actualHeight = this.ActualHeight;

                if (this._root != null)
                {
                    GeneralTransform gt = this._root.TransformToVisual(null);

                    if (gt != null)
                    {
                        Point point00 = new Point(0, 0);
                        Point point10 = new Point(1, 0);
                        Point point01 = new Point(0, 1);
                        Point transform00 = gt.Transform(point00);
                        Point transform10 = gt.Transform(point10);
                        Point transform01 = gt.Transform(point01);

                        double dpX = transform00.X;
                        double dpY = transform00.Y;

                        double calendarX = dpX;
                        double calendarY = dpY + actualHeight;

                        // if the page height is less then the total height of
                        // the PopUp + DatePicker or if we can fit the PopUp
                        // inside the page, we want to place the PopUp to the
                        // bottom
                        if (pageHeight < calendarY + calendarHeight)
                        {
                            calendarY = dpY - calendarHeight;
                        }
                        this.PART_CalenderPopup.HorizontalOffset = 0;
                        this.PART_CalenderPopup.VerticalOffset = 0;
                        
                        this._OutsideCalendarPopupCanvas.Width = pageWidth;
                        this._OutsideCalendarPopupCanvas.Height = pageHeight;
                        
                        this.PART_CalenderPopup.HorizontalAlignment = HorizontalAlignment.Left;
                        this.PART_CalenderPopup.VerticalAlignment = VerticalAlignment.Top;
                        Canvas.SetLeft(this.PART_CalenderPopup, calendarX);// - dpX);
                        Canvas.SetTop(this.PART_CalenderPopup, calendarY);// - dpY);

                        // Transform the invisible canvas to plugin coordinate
                        // space origin
                        Matrix transformToRootMatrix = Matrix.Identity;
                        transformToRootMatrix.M11 = transform10.X - transform00.X;
                        transformToRootMatrix.M12 = transform10.Y - transform00.Y;
                        transformToRootMatrix.M21 = transform01.X - transform00.X;
                        transformToRootMatrix.M22 = transform01.Y - transform00.Y;
                        transformToRootMatrix.OffsetX = transform00.X;
                        transformToRootMatrix.OffsetY = transform00.Y;
                        MatrixTransform mt = new MatrixTransform();
                        InvertMatrix(ref transformToRootMatrix);
                        mt.Matrix = transformToRootMatrix;
                        this._outsidePopupCanvas.RenderTransform = mt;
                    }
                }
            }
        }

        /// <summary>
        /// Sets the matrix to its inverse.
        /// </summary>
        /// <param name="matrix">Matrix to be inverted.</param>
        /// <returns>
        /// True if the Matrix is invertible, false otherwise.
        /// </returns>
        private static bool InvertMatrix(ref Matrix matrix)
        {
            double determinant = matrix.M11 * matrix.M22 - matrix.M12 * matrix.M21;

            if (determinant == 0.0)
            {
                return false;
            }

            Matrix matCopy = matrix;
            matrix.M11 = matCopy.M22 / determinant;
            matrix.M12 = -1 * matCopy.M12 / determinant;
            matrix.M21 = -1 * matCopy.M21 / determinant;
            matrix.M22 = matCopy.M11 / determinant;
            matrix.OffsetX = (matCopy.OffsetY * matCopy.M21 - matCopy.OffsetX * matCopy.M22) / determinant;
            matrix.OffsetY = (matCopy.OffsetX * matCopy.M12 - matCopy.OffsetY * matCopy.M11) / determinant;

            return true;
        }

#endif
        #endregion

#if SILVERLIGHT
        [TypeConverter(typeof(DateTimeTypeConverter))]
#endif
        /// <summary>
        /// Gets or sets the null value.
        /// </summary>
        /// <value>The null value.</value>
        public DateTime? NullValue
        {
            get { return (DateTime?)GetValue(NullValueProperty); }
            set { SetValue(NullValueProperty, value); }
        }

#if SILVERLIGHT
        [TypeConverter(typeof(DateTimeTypeConverter))]
#endif
        /// <summary>
        /// Gets or sets the date time.
        /// </summary>
        /// <value>The date time.</value>
        public DateTime? DateTime
        {
            get { return (DateTime?)GetValue(DateTimeProperty); }
            set 
            {               
                //value = CoerceDateTime(this, value);
                SetValue(DateTimeProperty, value);             
            }
        }

        /// <summary>
        /// Gets or sets the default date part.
        /// </summary>
        /// <value>The default date part.</value>
        public DateParts DefaultDatePart
        {
            get { return (DateParts)GetValue(DefaultDatePartProperty); }
            set
            {
                
                //value = CoerceDateTime(this, value);
                SetValue(DefaultDatePartProperty, value);               
            }
        }

#if SILVERLIGHT
        [TypeConverter(typeof(DateTimeTypeConverter))]
#endif
        /// <summary>
        /// Gets or sets the min date time.
        /// </summary>
        /// <value>The min date time.</value>
        public DateTime MinDateTime
        {
            get { return (DateTime)GetValue(MinDateTimeProperty); }
            set { SetValue(MinDateTimeProperty, value); }
        }

#if SILVERLIGHT
        [TypeConverter(typeof(DateTimeTypeConverter))]
#endif
        /// <summary>
        /// Gets or sets the max date time.
        /// </summary>
        /// <value>The max date time.</value>
        public DateTime MaxDateTime
        {
            get { return (DateTime)GetValue(MaxDateTimeProperty); }
            set { SetValue(MaxDateTimeProperty, value); }
        }
#if WPF
        /// <summary>
        /// Gets or sets the drop down view.
        /// </summary>
        /// <value>The drop down view.</value>
        public DropDownViews DropDownView
        {
            get { return (DropDownViews)GetValue(DropDownViewProperty); }
            set { SetValue(DropDownViewProperty, value); }
        }
#endif

#if SILVERLIGHT
        public CalendarMode DefaultCalendarDisplayMode
        {
            get { return (CalendarMode)GetValue(DefaultCalendarDisplayModeProperty); }
            set { SetValue(DefaultCalendarDisplayModeProperty, value); }
        }
#endif

        /// <summary>
        /// Gets or sets a value indicating whether this instance is null.
        /// </summary>
        /// <value><c>true</c> if this instance is null; otherwise, <c>false</c>.</value>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [BrowsableAttribute(false)]
        public bool IsNull
        {
            get { return (bool)GetValue(IsNullProperty); }
            set { SetValue(IsNullProperty, value); }
        }

        public static readonly DependencyProperty IsNullProperty =
            DependencyProperty.Register("IsNull", typeof(bool), typeof(DateTimeEdit), new PropertyMetadata(false));

        public static readonly DependencyProperty NullValueProperty =
            DependencyProperty.Register("NullValue", typeof(DateTime?), typeof(DateTimeEdit), new PropertyMetadata(null));

#if WPF
        public static readonly DependencyProperty DateTimeProperty =
    DependencyProperty.Register("DateTime", typeof(DateTime?), typeof(DateTimeEdit), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnDateTimeChanged), new CoerceValueCallback(CoerceDateTime)));
        
        public static readonly DependencyProperty MinDateTimeProperty =
            DependencyProperty.Register("MinDateTime", typeof(DateTime), typeof(DateTimeEdit), new FrameworkPropertyMetadata(System.DateTime.MinValue,FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,new PropertyChangedCallback(OnMinDateTimeChanged)));

        public static readonly DependencyProperty MaxDateTimeProperty =
            DependencyProperty.Register("MaxDateTime", typeof(DateTime), typeof(DateTimeEdit), new FrameworkPropertyMetadata(System.DateTime.MaxValue,FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,new PropertyChangedCallback(OnMaxDateTimeChanged)));

#endif

        public static readonly DependencyProperty DefaultDatePartProperty =
DependencyProperty.Register("DefaultDatePart", typeof(DateParts), typeof(DateTimeEdit), new PropertyMetadata(DateParts.None));
#if WPF
        public static readonly DependencyProperty CalendarStyleProperty =
           DependencyProperty.Register("CalendarStyle", typeof(Style), typeof(DateTimeEdit), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(OnCalendarStyleChanged)));
#endif
#if SILVERLIGHT
         public static readonly DependencyProperty CalendarStyleProperty =
           DependencyProperty.Register("CalendarStyle", typeof(Style), typeof(DateTimeEdit), new PropertyMetadata(null, new PropertyChangedCallback(OnCalendarStyleChanged)));
#endif
        /// <summary>
        /// Gets or sets the calendar style.
        /// </summary>
        /// <value>The calendar style.</value>
        public Style CalendarStyle
        {
            get
            {
                return (Style)GetValue(CalendarStyleProperty);
            }

            set
            {
                SetValue(CalendarStyleProperty, value);
            }
        }
        private static void OnCalendarStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

#if SILVERLIGHT
        public static readonly DependencyProperty DefaultCalendarDisplayModeProperty =
            DependencyProperty.Register("DefaultCalendarDisplayMode", typeof(CalendarMode), typeof(DateTimeEdit), new PropertyMetadata(CalendarMode.Month));

        public static readonly DependencyProperty DateTimeProperty =
    DependencyProperty.Register("DateTime", typeof(DateTime?), typeof(DateTimeEdit), new PropertyMetadata(null, new PropertyChangedCallback(OnDateTimeChanged)));

        public static readonly DependencyProperty MinDateTimeProperty =
           DependencyProperty.Register("MinDateTime", typeof(DateTime), typeof(DateTimeEdit), new PropertyMetadata(System.DateTime.MinValue, new PropertyChangedCallback(OnMinDateTimeChanged)));

        public static readonly DependencyProperty MaxDateTimeProperty =
            DependencyProperty.Register("MaxDateTime", typeof(DateTime), typeof(DateTimeEdit), new PropertyMetadata(System.DateTime.MaxValue, new PropertyChangedCallback(OnMaxDateTimeChanged)));
#endif

#if WPF
        public static readonly DependencyProperty DropDownViewProperty =
    DependencyProperty.Register("DropDownView", typeof(DropDownViews), typeof(DateTimeEdit), new PropertyMetadata(DropDownViews.Classic, new PropertyChangedCallback(OnDropDownViewChanged)));
#endif
        

       
        #region PropertyChangedCallbacks

        /// <summary>
        /// Called when [date time changed].
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        public static void OnDateTimeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if ((DateTimeEdit)obj != null)
            {
               
                ((DateTimeEdit)obj).OnDateTimeChanged(args);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:DateTimeChanged"/> event.
        /// </summary>
        /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        protected void OnDateTimeChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.DateTimeChanged != null)
            {
                if (args.NewValue != null)
                    this.Text = args.NewValue.ToString();
                else
                    this.Text = this.NoneDateText;

#if WPF
                if(this.clock!=null && this.DateTime != null)

                this.clock.DateTime = (DateTime)this.DateTime;
#endif
                this.DateTimeChanged(this, args);
            }
            
#if SILVERLIGHT
            object _value = CoerceDateTime(this, this.DateTime);
            if (this.DateTime != (DateTime?)_value)
            {
                this.DateTime = (DateTime?)_value;
                return;
            }
#endif      
            if (this.DateTime != null)
            {
                this.WatermarkVisibility = Visibility.Collapsed;
            }

            mValue = DateTime;
            OldValue = (DateTime?)args.OldValue;
            
            mValue = this.DateTime;
            if (mIsLoaded)
            {
                //if (mValueChanged == true)
                LoadTextBox();
                //if (mValueChanged)
                //{
                //    mValue = DateTime;
                //    this.LoadTextBox();

                //}
                //else
                //{
                //    mValue = DateTime;
                //    this.LoadOnValueChanged();
                //}
            }
        }

        /// <summary>
        /// Called when [min date time changed].
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        public static void OnMinDateTimeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if ((DateTimeEdit)obj != null)
            {
                ((DateTimeEdit)obj).OnMinDateTimeChanged(args);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:MinDateTimeChanged"/> event.
        /// </summary>
        /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        protected void OnMinDateTimeChanged(DependencyPropertyChangedEventArgs args)
        {
            if (MinDateTimeChanged != null)
            {
                MinDateTimeChanged(this, args);
            }

            if (this.MaxDateTime < this.MinDateTime)
                this.MaxDateTime = this.MinDateTime;

            if (this.DateTime != ValidateValue(this.DateTime))
            {
                this.DateTime = ValidateValue(this.DateTime);
            }
        }
#if WPF
        /// <summary>
        /// Called when [drop down view changed].
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        public static void OnDropDownViewChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if ((DateTimeEdit)obj != null)
            {
                ((DateTimeEdit)obj).OnDropDownViewChanged(args);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:DropDownViewChanged"/> event.
        /// </summary>
        /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        protected void OnDropDownViewChanged(DependencyPropertyChangedEventArgs args)
        {
            if (DropDownViewChanged != null)
            {
                DropDownViewChanged(this, args);
            }

            if (DropDownView == DropDownViews.Classic)
            {
                this.EnableClassicStyle = true;
                EnableCombinedStyle = false;
            }
            else if (DropDownView == DropDownViews.Calendar)
            {
                this.EnableClassicStyle = false;
                EnableCombinedStyle = false;
            }
            else if (DropDownView == DropDownViews.Combined)
            {
                EnableCombinedStyle = true;
            }
        }
#endif
        /// <summary>
        /// Called when [max date time changed].
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        public static void OnMaxDateTimeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if ((DateTimeEdit)obj != null)
            {
                ((DateTimeEdit)obj).OnMaxDateTimeChanged(args);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:MaxDateTimeChanged"/> event.
        /// </summary>
        /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        protected void OnMaxDateTimeChanged(DependencyPropertyChangedEventArgs args)
        {
            if (MaxDateTimeChanged != null)
            {
                MaxDateTimeChanged(this, args);
            }

            if (this.MinDateTime > this.MaxDateTime)
                this.MinDateTime = this.MaxDateTime;

            if (this.DateTime != ValidateValue(this.DateTime))
            {
                this.DateTime = ValidateValue(this.DateTime);
            }
        }

        #endregion

        #region Coerce

        /// <summary>
        /// Coerces the date time.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="baseValue">The base value.</param>
        /// <returns></returns>
        private static object CoerceDateTime(DependencyObject d, object baseValue)
        {
          
            DateTimeEdit dateTimeEdit = (DateTimeEdit)d;
            if (baseValue != null)
            {
                dateTimeEdit.IsNull = false;
                DateTime? value = (DateTime?)baseValue;
                //if (dateTimeEdit.mValueChanged == true)
                //{
                    if (value > dateTimeEdit.MaxDateTime)
                    {
                        value = dateTimeEdit.MaxDateTime;
                    }
                    else if (value < dateTimeEdit.MinDateTime)
                    {
                        value = dateTimeEdit.MinDateTime;
                    }
                //}
                return value;
            }
            else
            {
                if (dateTimeEdit.IsEmptyDateEnabled)
                {
                    dateTimeEdit.IsNull = true;
                    return dateTimeEdit.NullValue;
                }
                else
                {
                    DateTime? value;
                
                    dateTimeEdit.IsNull = false;
                    if (dateTimeEdit.Text.Equals(dateTimeEdit.NoneDateText))
                    {
                        dateTimeEdit.IsNull = true;
                        return dateTimeEdit.NullValue;
                    }
                    else
                    {
                        value = System.DateTime.Today;
                        //if (dateTimeEdit.mValueChanged == true)
                        //{
                        if (value > dateTimeEdit.MaxDateTime)
                        {
                            value = dateTimeEdit.MaxDateTime;
                        }
                        if (value < dateTimeEdit.MinDateTime)
                        {
                            value = dateTimeEdit.MinDateTime;
                        }
                    }
                    //}
                    return value;
                }
            }
        }

        /// <summary>
        /// Coerces the min value.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="baseValue">The base value.</param>
        /// <returns></returns>
        private static object CoerceMinValue(DependencyObject d, object baseValue)
        {
            DateTimeEdit dateTimeEdit = (DateTimeEdit)d;
            DateTime minDateTime = (DateTime)baseValue;
            if (minDateTime > dateTimeEdit.MaxDateTime)
            {
                return dateTimeEdit.MaxDateTime;
            }
            return baseValue;
        }

        /// <summary>
        /// Coerces the max value.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="baseValue">The base value.</param>
        /// <returns></returns>
        private static object CoerceMaxValue(DependencyObject d, object baseValue)
        {
            DateTimeEdit dateTimeEdit = (DateTimeEdit)d;
            DateTime maxDateTime = (DateTime)baseValue;
            if (dateTimeEdit.MinDateTime > maxDateTime)
            {
                return dateTimeEdit.MinDateTime;
            }
            return baseValue;
        }

        #endregion

    }

    public enum OnFocusBehavior
    {
        /// <summary>
        /// 
        /// </summary>
        Default,
        /// <summary>
        /// 
        /// </summary>
        CursorOnFirstCharacter,
        /// <summary>
        /// 
        /// </summary>
        CursorAtEnd
    }
}
