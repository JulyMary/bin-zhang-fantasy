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
using System.ComponentModel;
using System.Globalization;
using Syncfusion.Windows.Controls;

#if WPF
using Syncfusion.Licensing;
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
      Type = typeof(IntegerTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Office2007BlueStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2007Black,
    Type = typeof(IntegerTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Office2007BlackStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2007Silver,
    Type = typeof(IntegerTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Office2007SilverStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2010Blue,
      Type = typeof(IntegerTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Office2010BlueStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2010Black,
    Type = typeof(IntegerTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Office2010BlackStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2010Silver,
    Type = typeof(IntegerTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Office2010SilverStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2003,
    Type = typeof(IntegerTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Office2003Style.xaml")]
    [SkinType(SkinVisualStyle = Skin.Blend,
    Type = typeof(IntegerTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/BlendStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.SyncOrange,
    Type = typeof(IntegerTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/SyncOrangeStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.ShinyRed,
    Type = typeof(IntegerTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/ShinyRedStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.ShinyBlue,
    Type = typeof(IntegerTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/ShinyBlueStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Default,
    Type = typeof(IntegerTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Generic.xaml")]
    [SkinType(SkinVisualStyle = Skin.VS2010,
 Type = typeof(IntegerTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/VS2010Style.xaml")]
#endif
#if SILVERLIGHT
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Blend,
       Type = typeof(IntegerTextBox), XamlResource = "/Syncfusion.Theming.Blend;component/Editors/IntegerTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2007Blue,
        Type = typeof(IntegerTextBox), XamlResource = "/Syncfusion.Theming.Office2007Blue;component/Editors/IntegerTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2007Black,
        Type = typeof(IntegerTextBox), XamlResource = "/Syncfusion.Theming.Office2007Black;component/Editors/IntegerTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2007Silver,
        Type = typeof(IntegerTextBox), XamlResource = "/Syncfusion.Theming.Office2007Silver;component/Editors/IntegerTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Default,
        Type = typeof(IntegerTextBox), XamlResource = "/Syncfusion.Theming.Default;component/Editors/IntegerTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2010Blue,
        Type = typeof(IntegerTextBox), XamlResource = "/Syncfusion.Theming.Office2010Blue;component/Editors/IntegerTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2010Black,
        Type = typeof(IntegerTextBox), XamlResource = "/Syncfusion.Theming.Office2010Black;component/Editors/IntegerTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2010Silver,
        Type = typeof(IntegerTextBox), XamlResource = "/Syncfusion.Theming.Office2010Silver;component/Editors/IntegerTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Windows7,
        Type = typeof(IntegerTextBox), XamlResource = "/Syncfusion.Theming.Windows7;component/Editors/IntegerTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.VS2010,
       Type = typeof(IntegerTextBox), XamlResource = "/Syncfusion.Theming.VS2010;component/Editors/IntegerTextBox.xaml")]  
#endif
    public class IntegerTextBox : EditorBase
    {
        #region Events

        /// <summary>
        /// Event that is raised when <see cref="MinValue"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback MinValueChanged;

        /// <summary>
        /// Event that is raised when <see cref="Value"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback ValueChanged;

        /// <summary>
        /// Event that is raised when <see cref="MaxValue"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback MaxValueChanged;

        /// <summary>
        /// Event that is raised when <see cref="NumberGroupSizes"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback NumberGroupSizesChanged;

        /// <summary>
        /// Event that is raised when <see cref="NumberGroupSeparator"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback NumberGroupSeparatorChanged;

        #endregion

        #region Members

        internal Int64? OldValue;
        internal Int64? mValue;
        internal bool? mValueChanged = true;
        internal bool mIsLoaded = false;
        internal int count = 1;
        internal string checktext = "";
        #endregion

        #region Constructor

#if WPF
        static IntegerTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IntegerTextBox), new FrameworkPropertyMetadata(typeof(IntegerTextBox)));
          //  EnvironmentTest.ValidateLicense(typeof(IntegerTextBox));
        }
#endif
        public ICommand pastecommand { get; private set; }
        public ICommand copycommand { get; private set; }
        public ICommand cutcommand { get; private set; }

        public IntegerTextBox()
        {
            pastecommand = new DelegateCommand(_pastecommand, Canpaste);
            copycommand = new DelegateCommand(_copycommand, Canpaste);
            cutcommand = new DelegateCommand(_cutcommand, Canpaste);
#if WPF
            this.AddHandler(CommandManager.PreviewExecutedEvent,
new ExecutedRoutedEventHandler(CommandExecuted), true);
#endif
#if WPF
            if (EnvironmentTest.IsSecurityGranted)
            {
                EnvironmentTest.StartValidateLicense(typeof(IntegerTextBox));
            }
#endif
#if SILVERLIGHT
            this.DefaultStyleKey = typeof(IntegerTextBox);
#endif
            this.Loaded += IntegerTextbox_Loaded;
        }
        #endregion
        private void _pastecommand(object parameter)
        {           
            Paste();
        }
        private void _copycommand(object parameter)
        {
            copy();
        }
        private void _cutcommand(object parameter)
        {
            cut();
        }

        private bool Canpaste(object parameter)
        {
            return true;
        }
        #region overide
        ScrollViewer PART_ContentHost;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
#if WPF
            PART_ContentHost = this.GetTemplateChild("PART_ContentHost") as ScrollViewer;
#endif
#if SILVERLIGHT
            PART_ContentHost = this.GetTemplateChild("ContentElement") as ScrollViewer;
#endif
        }
#if WPF
        private void CommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {

            if ((e as ExecutedRoutedEventArgs).Command == ApplicationCommands.Paste)
            {
                
                Paste();
                e.Handled = true;
            }
            if ((e as ExecutedRoutedEventArgs).Command == ApplicationCommands.Cut)
            {
                cut();
                e.Handled = true;
            }
        }
#endif
        private void copy()
        {
            Clipboard.SetText(this.SelectedText);
        }
        private new void Paste()
        {
            if (this.IsReadOnly == false)
            {
                string oldselection = Clipboard.GetText();
                int index1 = this.SelectionStart;
                string type = this.GetType().ToString();
                NumberFormatInfo numberFormat = this.GetCulture().NumberFormat;
                double oldval = (double)this.Value;
                double val1;
                long val2;
                for (int i = 0; i < oldselection.Length; i++)
                {
                    if (char.IsDigit(oldselection[i]))
                    {
                    }
                    else
                    {
                        oldselection = oldselection.Remove(i, 1);
                        i--;
                    }
                }
                if (this.SelectionLength > 0)
                {
                    if (oldselection == string.Empty)
                    {
                        if (this.OldValue != null)
                        {
                            this.SetValue(false, this.OldValue);
                            long val3 = (long)this.OldValue;
                            this.Text = val3.ToString("N", numberFormat);
                        }
                    }
                    else
                        this.Text = this.Text.Replace(this.SelectedText, oldselection);
                }
                else
                {
                    this.Text = this.Text.Insert(this.SelectionStart, oldselection);
                }
                double.TryParse(this.Text, out val1);
                if ((val1 > this.MaxValue) && (this.MaxValidation == MaxValidation.OnKeyPress) || val1 < this.MinValue && (this.MinValidation == MinValidation.OnKeyPress))
                {
                    if ((val1 > this.MaxValue) && (this.MaxValidation == MaxValidation.OnKeyPress))
                    {
                        if (this.MaxValueOnExceedMaxDigit)
                        {
                            val1 =this.MaxValue;
                            this.CaretIndex = index1;
                            this.SetValue(false, (long)val1);
                            this.Text = val1.ToString("N", numberFormat);
                        }
                        else
                        {
                            this.SetValue(false,(long)oldval);
                            double val3 = oldval;
                            this.Text = val3.ToString("N", numberFormat);
                        }

                    }
                    if (val1 < this.MinValue && (this.MinValidation == MinValidation.OnKeyPress))
                    {
                        if (this.MinValueOnExceedMinDigit)
                        {
                            val1 = this.MinValue;
                            this.CaretIndex = index1;
                            this.SetValue(false,(long)val1);
                            this.Text = val1.ToString("C", numberFormat);
                        }
                        else
                        {
                            this.SetValue(false,(long)oldval);
                            double val3 = oldval;
                            this.Text = val3.ToString("N", numberFormat);
                        }
                    }
                }
                else
                {
                    this.SetValue(false,(long)val1);
                    this.CaretIndex = index1 + oldselection.Length;
                    this.Text = val1.ToString("N", numberFormat);
                }
                //val2 = (long)val1;
                //this.SetValue(false, val2);
                //this.Text = val2.ToString("N", numberFormat);
                //this.CaretIndex = index1 + oldselection.Length;
            }  
        }
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            if (e.Delta > 0)
            {
                IntegerValueHandler.integerValueHandler.HandleUpKey(this);
            }
            else if (e.Delta < 0)
            {
                IntegerValueHandler.integerValueHandler.HandleDownKey(this);
            }
        }

#if SILVERLIGHT
        protected override void OnKeyDown(KeyEventArgs e)
#endif
#if WPF
        protected override void OnPreviewKeyDown(KeyEventArgs e)
#endif
        {
            if (ModifierKeys.Control == Keyboard.Modifiers)
            {
                if (e.Key == Key.V)
                {
                    Paste();
                    e.Handled = true;
                }
                if (e.Key == Key.C)
                {
                    copy();                   
                }
                if (e.Key == Key.Z)
                {
                    if (IsUndoEnabled)
                    {
                        this.SetValue(true, this.OldValue);
                    }
                    e.Handled = true;
                }

                if (e.Key == Key.X)
                {
                    cut();
                    e.Handled = true;
                }
            }
            else
            {
                e.Handled = IntegerValueHandler.integerValueHandler.HandleKeyDown(this, e);
            }
#if WPF
            base.OnPreviewKeyDown(e);
#endif
#if SILVERLIGHT
            base.OnKeyDown(e);
#endif
        }

        private void cut()
        {
            if (this.SelectionLength > 0)
            {
                Clipboard.SetText(this.SelectedText);
                this.count = 1;
                IntegerValueHandler.integerValueHandler.HandleDeleteKey(this);
            }
        }

        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            e.Handled = IntegerValueHandler.integerValueHandler.MatchWithMask(this, e.Text);
            base.OnTextInput(e);
        }

        internal override void OnCultureChanged()
        {
            base.OnCultureChanged();
            if (this.mIsLoaded)
            {
                this.FormatText();
            }
        }

        internal override void OnNumberFormatChanged()
        {
            base.OnNumberFormatChanged();
            if (this.mIsLoaded)
            {
                this.FormatText();
            }
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            if (this.EnableFocusColors && this.PART_ContentHost != null)
                this.PART_ContentHost.Background = this.Background;
            Int64? Val = this.Value;
            if (Val != null)
            {
                if (Val > this.MaxValue)
                {
                    Val = this.MaxValue;
                }
                else if (mValue < this.MinValue)
                {
                    Val = this.MinValue;
                }
                if (Val != this.Value)
                {
                    this.Value = Val;
                }
            }
            base.OnLostFocus(e);
            this.checktext = "";
        }       
#if WPF
           protected override void OnContextMenuOpening(ContextMenuEventArgs e)
        {
            if (this.IsReadOnly == true)
            {
                e.Handled = true;
                base.OnContextMenuOpening(e);
            }
            else
            {
            }
        }
#endif
       
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            if (this.EnableFocusColors && this.PART_ContentHost != null)
                this.PART_ContentHost.Background = this.FocusedBackground;           
            base.OnGotFocus(e);
        }

        #endregion

        #region Internal Methods

        internal void FormatText()
        {
            if (this.Value != null)
            {
                NumberFormatInfo numberFormat = this.GetCulture().NumberFormat;
                this.MaskedText = ((Int64)mValue).ToString("N", numberFormat);
            }
            else
            {
                this.MaskedText = "";
            }
            //if (this.mValue == null)
            //{
            //    if (UseNullOption)
            //    {
            //        this.MaskedText = "";
            //        this.IsNull = true;
            //        return;
            //    }
            //    else
            //    {
            //        this.SetValue(true, 0);
            //        return;
            //    }
            //}
            //else
            //{
            //    NumberFormatInfo numberFormat = this.GetCulture().NumberFormat;
            //    this.MaskedText = ((Int64)mValue).ToString("N", numberFormat);
            //}
        }

        internal void SetValue(bool? IsReload, long? _Value)
        {
            if (IsReload == false)
            {
                mValueChanged = false;
                this.Value = _Value;
                mValueChanged = true;
            }
            else if (IsReload == true)
            {
                this.Value = _Value;
            }
        }

        internal Int64? ValidateValue(Int64? Val)
        {
            if (Val != null)
            {
                if (Val > this.MaxValue)
                {
                    Val = this.MaxValue;
                }
                else if (mValue < this.MinValue)
                {
                    Val = this.MinValue;
                }
            }
            return Val;
        }

        internal CultureInfo GetCulture()
        {
            CultureInfo cultureInfo;
            if (Culture != null)
                cultureInfo = this.Culture.Clone() as CultureInfo;
            else
                cultureInfo = CultureInfo.CurrentCulture.Clone() as CultureInfo;

            if (NumberFormat != null)
                cultureInfo.NumberFormat = NumberFormat;

            cultureInfo.NumberFormat.NumberDecimalDigits = 0;
            if (!GroupSeperatorEnabled)
            {
                cultureInfo.NumberFormat.NumberGroupSeparator = string.Empty;
            }

            if (GroupSeperatorEnabled == true)
            {
                if (!NumberGroupSeparator.Equals(string.Empty))
                    cultureInfo.NumberFormat.NumberGroupSeparator = NumberGroupSeparator;
            }

#if SILVERLIGHT
            if (NumberGroupSizes != null)
                cultureInfo.NumberFormat.NumberGroupSizes = this.NumberGroupSizes;
#endif

#if WPF
            int count = this.NumberGroupSizes.Count;
            if (count > 0)
            {
                int[] ngs = new int[count];

                for (int i = 0; i < count; i++)
                {
                    ngs[i] = this.NumberGroupSizes[i];
                }
                cultureInfo.NumberFormat.NumberGroupSizes = ngs;
            }
#endif
#if SILVERLIGHT
            if (GroupSeperatorEnabled == true)
            {
                //Regex rgxUrl = new Regex("[^a-zA-Z0-9]");
                //string checkspecialcharacters = rgxUrl.IsMatch()               
                cultureInfo.NumberFormat.NumberGroupSeparator = cultureInfo.NumberFormat.NumberGroupSeparator[0].ToString();
            }
#endif
            return cultureInfo;
        }

        private static object CoerceValue(DependencyObject d, object baseValue)
        {
            IntegerTextBox integerTextBox = (IntegerTextBox)d;
            if (baseValue != null)
            {
                Int64? value = (Int64?)baseValue;
                if (integerTextBox.mValueChanged == true)
                {
                    if (value > integerTextBox.MaxValue)
                    {
                        value = integerTextBox.MaxValue;
                    }
                    if (value < integerTextBox.MinValue)
                    {
                        value = integerTextBox.MinValue;
                    }
                }
                if (value != null)
                {
                    integerTextBox.IsNegative = value < 0 ? true : false;
                    integerTextBox.IsZero = value == 0 ? true : false;
                    integerTextBox.IsNull = false;
                }
                return value;
            }
            else
            {
                if (integerTextBox.UseNullOption)
                {
                    integerTextBox.IsNull = true;
                    integerTextBox.IsNegative = false;
                    integerTextBox.IsZero = false;
                    return integerTextBox.NullValue;
                    //return baseValue;
                }
                else
                {
                    Int64 value = 0L;
                    if (integerTextBox.mValueChanged == true)
                    {
                        if (value > integerTextBox.MaxValue)
                        {
                            value = integerTextBox.MaxValue;
                        }
                        if (value < integerTextBox.MinValue)
                        {
                            value = integerTextBox.MinValue;
                        }
                    }
                    integerTextBox.IsNegative = value < 0 ? true : false;
                    integerTextBox.IsZero = value == 0 ? true : false;
                    integerTextBox.IsNull = false;
                    return value;
                }
            }
        }

        private static object CoerceMinValue(DependencyObject d, object baseValue)
        {
            IntegerTextBox integerTextBox = (IntegerTextBox)d;
            if (integerTextBox.MinValue > integerTextBox.MaxValue)
            {
                baseValue = integerTextBox.MaxValue;
            }
            return baseValue;
        }

        private static object CoerceMaxValue(DependencyObject d, object baseValue)
        {
            IntegerTextBox integerTextBox = (IntegerTextBox)d;
            if (integerTextBox.MinValue > integerTextBox.MaxValue)
            {
                return integerTextBox.MinValue;
            }
            return baseValue;
        }

        #endregion

        void IntegerTextbox_Loaded(object sender, RoutedEventArgs e)
        {
            mIsLoaded = true;
            object tempObj = CoerceValue(this, Value);
            Int64? tempVal = (Int64?)tempObj;
            if (this.IsNull == true)
            {
                this.WatermarkVisibility = Visibility.Visible;
            }
            if (tempVal != Value)
            {
                Value = tempVal;
            }
            else
            {
                this.mValue = this.Value;
                FormatText();
            }
        }

        #region Properties

#if SILVERLIGHT
        [TypeConverter(typeof(IntTypeConverter))]
#endif
        public Int64? Value
        {
            get { return (Int64?)GetValue(ValueProperty); }
            set
            {
                //object coerceValue = CoerceValue(this, value);
                SetValue(ValueProperty, value);
            }
        }

#if SILVERLIGHT
        [TypeConverter(typeof(IntTypeConverter))]
#endif
        public Int64 MinValue
        {
            get { return (Int64)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

#if SILVERLIGHT
        [TypeConverter(typeof(IntTypeConverter))]
#endif
        public Int64 MaxValue
        {
            get { return (Int64)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public string NumberGroupSeparator
        {
            get { return (string)GetValue(NumberGroupSeparatorProperty); }
            set { SetValue(NumberGroupSeparatorProperty, value); }
        }

        //private int[] _numberGroupSizes;
        //public int[] NumberGroupSizes
        //{
        //    set { _numberGroupSizes = value; }
        //    get { return _numberGroupSizes; }
        //}
        public bool GroupSeperatorEnabled
        {
            get { return (bool)GetValue(GroupSeperatorEnabledProperty); }
            set { SetValue(GroupSeperatorEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GroupSeperatorEnabled.  This enables animation, styling, binding, etc...
#if WPF
        public static readonly DependencyProperty GroupSeperatorEnabledProperty =
            DependencyProperty.Register("GroupSeperatorEnabled", typeof(bool), typeof(IntegerTextBox), new PropertyMetadata(true, new PropertyChangedCallback(OnNumberGroupSeparatorChanged)));
#endif
#if SILVERLIGHT
        public static readonly DependencyProperty GroupSeperatorEnabledProperty =
            DependencyProperty.Register("GroupSeperatorEnabled", typeof(bool), typeof(IntegerTextBox), new PropertyMetadata(true, new PropertyChangedCallback(OnNumberGroupSeparatorChanged)));

        public int[] NumberGroupSizes
        {
            get { return (int[])GetValue(NumberGroupSizesProperty); }
            set { SetValue(NumberGroupSizesProperty, value); }
        }

        public static readonly DependencyProperty NumberGroupSizesProperty =
            DependencyProperty.Register("NumberGroupSizes", typeof(int[]), typeof(IntegerTextBox), new PropertyMetadata(null,OnNumberGroupSizesChanged));


        
#endif
#if WPF

        public Int32Collection NumberGroupSizes
        {
            get { return (Int32Collection)GetValue(NumberGroupSizesProperty); }
            set { SetValue(NumberGroupSizesProperty, value); }
        }

        public static readonly DependencyProperty NumberGroupSizesProperty =
            DependencyProperty.Register("NumberGroupSizes", typeof(Int32Collection), typeof(IntegerTextBox), new PropertyMetadata(new Int32Collection(),OnNumberGroupSizesChanged));
        
#endif

#if WPF
        //In WPF Value property acts with TwoWay binding by default.
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(Int64?), typeof(IntegerTextBox), new FrameworkPropertyMetadata(null,FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,new PropertyChangedCallback(OnValueChanged), new CoerceValueCallback(CoerceValue)));
#endif

#if SILVERLIGHT
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(Int64?), typeof(IntegerTextBox), new PropertyMetadata(null, new PropertyChangedCallback(OnValueChanged)));
#endif

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(Int64), typeof(IntegerTextBox), new PropertyMetadata(Int64.MinValue, new PropertyChangedCallback(OnMinValueChanged)));

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(Int64), typeof(IntegerTextBox), new PropertyMetadata(Int64.MaxValue, new PropertyChangedCallback(OnMaxValueChanged)));

        public static readonly DependencyProperty NumberGroupSeparatorProperty =
            DependencyProperty.Register("NumberGroupSeparator", typeof(string), typeof(IntegerTextBox), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnNumberGroupSeparatorChanged)));






        public int ScrollInterval 
        {
            get { return (int)GetValue(ScrollIntervalProperty); }
            set { SetValue(ScrollIntervalProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScrollInterval.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScrollIntervalProperty =
            DependencyProperty.Register("ScrollInterval", typeof(int), typeof(IntegerTextBox), new PropertyMetadata(1));




        

        #endregion

        #region PropertyChanged Callbacks


        public static void OnNumberGroupSizesChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            IntegerTextBox i = (IntegerTextBox)obj;
            if (i != null)
            {
                if (i != null)
                {
                    i.OnNumberGroupSizesChanged(args);
                }
            }
        }

        protected void OnNumberGroupSizesChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.NumberGroupSizesChanged != null)
            {
                this.NumberGroupSizesChanged(this, args);
            }
            if (mIsLoaded)
            {
                this.FormatText();
            }
        }

        public static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if ((IntegerTextBox)obj != null)
                ((IntegerTextBox)obj).OnValueChanged(args);
        }

        protected void OnValueChanged(DependencyPropertyChangedEventArgs args)
        {

#if SILVERLIGHT
            object coerceValue = CoerceValue(this, this.Value);
            if (this.Value != (Int64?)coerceValue)
            {
                this.Value = (Int64?)coerceValue;
                return;
            }
#endif
            if (this.Value != null)
            {
                this.IsNegative = this.Value < 0 ? true : false;
                this.IsZero = this.Value == 0 ? true : false;
                this.IsNull = false;
            }
            else
            {
                this.IsNull = true;
                this.IsNegative = false;
                this.IsZero = false;
            }

            OldValue = (Int64?)args.OldValue;
            mValue = this.Value;
            if (this.Value != null)
                this.WatermarkVisibility = System.Windows.Visibility.Collapsed;

            if (ValueChanged != null)
                ValueChanged(this, args);
            if (this.Value > this.MinValue && this.MinValidation == MinValidation.OnKeyPress)
            {
                this.checktext = "";
            }

            if (mIsLoaded)
            {
                if (mValueChanged == true)
                    FormatText();
            }
        }


        public static void OnMinValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if ((IntegerTextBox)obj != null)
            {
                ((IntegerTextBox)obj).OnMinValueChanged(args);
            }
        }

        protected void OnMinValueChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.MinValueChanged != null)
            {
                this.MinValueChanged(this, args);
            }

            if (this.MaxValue < this.MinValue)
                this.MaxValue = this.MinValue;

            if (this.Value != this.ValidateValue(this.Value))
            {
                this.Value = this.ValidateValue(this.Value);
            }
        }

        public static void OnMaxValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if ((IntegerTextBox)obj != null)
            {
                ((IntegerTextBox)obj).OnMaxValueChanged(args);
            }
        }

        protected void OnMaxValueChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.MaxValueChanged != null)
                this.MaxValueChanged(this, args);

            if (this.MinValue > this.MaxValue)
                this.MinValue = this.MaxValue;

            if (this.Value != this.ValidateValue(this.Value))
            {
                this.Value = this.ValidateValue(this.Value);
            }
        }

        private static void OnNumberGroupSeparatorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if ((IntegerTextBox)obj != null)
                ((IntegerTextBox)obj).OnNumberGroupSeparatorChanged(args);
        }

        protected virtual void OnNumberGroupSeparatorChanged(DependencyPropertyChangedEventArgs e)
        {
            if (this.NumberGroupSeparatorChanged != null)
            {
                this.NumberGroupSeparatorChanged(this, e);
            }
            if (mIsLoaded)
            {
                this.FormatText();
            }
        }

        #endregion

#if SILVERLIGHT
        [TypeConverter(typeof(IntTypeConverter))]
#endif
        public Int64? NullValue
        {
            get { return (Int64?)GetValue(NullValueProperty); }
            set { SetValue(NullValueProperty, value); }
        }

        public static readonly DependencyProperty NullValueProperty =
            DependencyProperty.Register("NullValue", typeof(Int64?), typeof(IntegerTextBox), new PropertyMetadata(null, OnNullValueChanged));

        public static void OnNullValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if ((IntegerTextBox)obj != null)
                ((IntegerTextBox)obj).OnNullValueChanged(args);
        }

        protected void OnNullValueChanged(DependencyPropertyChangedEventArgs args)
        {
        }
    
    }
}
