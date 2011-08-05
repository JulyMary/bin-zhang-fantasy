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
      Type = typeof(DoubleTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Office2007BlueStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2007Black,
    Type = typeof(DoubleTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Office2007BlackStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2007Silver,
    Type = typeof(DoubleTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Office2007SilverStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2010Blue,
     Type = typeof(DoubleTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Office2010BlueStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2010Black,
    Type = typeof(DoubleTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Office2010BlackStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2010Silver,
    Type = typeof(DoubleTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Office2010SilverStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2003,
    Type = typeof(DoubleTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Office2003Style.xaml")]
    [SkinType(SkinVisualStyle = Skin.Blend,
    Type = typeof(DoubleTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/BlendStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.SyncOrange,
    Type = typeof(DoubleTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/SyncOrangeStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.ShinyRed,
    Type = typeof(DoubleTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/ShinyRedStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.ShinyBlue,
    Type = typeof(DoubleTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/ShinyBlueStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Default,
    Type = typeof(DoubleTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Generic.xaml")]
    [SkinType(SkinVisualStyle = Skin.VS2010,
 Type = typeof(DoubleTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/VS2010Style.xaml")]
#endif
#if SILVERLIGHT
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Blend,
       Type = typeof(DoubleTextBox), XamlResource = "/Syncfusion.Theming.Blend;component/Editors/DoubleTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2007Blue,
        Type = typeof(DoubleTextBox), XamlResource = "/Syncfusion.Theming.Office2007Blue;component/Editors/DoubleTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2007Black,
        Type = typeof(DoubleTextBox), XamlResource = "/Syncfusion.Theming.Office2007Black;component/Editors/DoubleTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2007Silver,
        Type = typeof(DoubleTextBox), XamlResource = "/Syncfusion.Theming.Office2007Silver;component/Editors/DoubleTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Default,
        Type = typeof(DoubleTextBox), XamlResource = "/Syncfusion.Theming.Default;component/Editors/DoubleTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2010Blue,
        Type = typeof(DoubleTextBox), XamlResource = "/Syncfusion.Theming.Office2010Blue;component/Editors/DoubleTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2010Black,
        Type = typeof(DoubleTextBox), XamlResource = "/Syncfusion.Theming.Office2010Black;component/Editors/DoubleTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2010Silver,
        Type = typeof(DoubleTextBox), XamlResource = "/Syncfusion.Theming.Office2010Silver;component/Editors/DoubleTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Windows7,
       Type = typeof(DoubleTextBox), XamlResource = "/Syncfusion.Theming.Windows7;component/Editors/DoubleTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.VS2010,
      Type = typeof(DoubleTextBox), XamlResource = "/Syncfusion.Theming.VS2010;component/Editors/DoubleTextBox.xaml")]  
#endif
    public class DoubleTextBox : EditorBase
    {
        #region Events
        /// <summary>
        /// Delegate used to handle the ValueChanging event
        /// </summary>
        public delegate void ValueChangingEventHandler(object sender, ValueChangingEventArgs e);

        /// <summary>
        /// Event that is raised when <see cref="MinValue"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback MinValueChanged;

        /// <summary>
        /// Event that is raised when <see cref="Value"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback ValueChanged;

        /// <summary>
        /// Event that is raised before the <see cref="Value"/> property is changed.
        /// </summary>
        public event ValueChangingEventHandler ValueChanging;

        /// <summary>
        /// Event that is raised when <see cref="MaxValue"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback MaxValueChanged;

        /// <summary>
        /// Event that is raised when <see cref="NumberDecimalDigits"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback NumberDecimalDigitsChanged;

        /// <summary>
        /// Event that is raised when <see cref="NumberDecimalSeparator"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback NumberDecimalSeparatorChanged;

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

        internal double? OldValue;
        internal double? mValue;
        internal bool? mValueChanged = true;
        internal bool mIsLoaded = false;
        internal int count = 1;
        internal bool negativeFlag = false;
        internal string checktext = "";
        #endregion

        #region Constructor

#if WPF
        static DoubleTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DoubleTextBox), new FrameworkPropertyMetadata(typeof(DoubleTextBox)));
            //  EnvironmentTest.ValidateLicense(typeof(DoubleTextBox));
        }
#endif
        public ICommand pastecommand { get; private set; }
        public ICommand copycommand { get; private set; }
        public ICommand cutcommand { get; private set; }

        public DoubleTextBox()
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
                EnvironmentTest.StartValidateLicense(typeof(DoubleTextBox));
            }
#endif

#if SILVERLIGHT
            this.DefaultStyleKey = typeof(DoubleTextBox);
#endif
            this.Loaded += DoubleTextbox_Loaded;
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

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            if (e.Delta > 0)
            {
                DoubleValueHandler.doubleValueHandler.HandleUpKey(this);
            }
            else if (e.Delta < 0)
            {
                DoubleValueHandler.doubleValueHandler.HandleDownKey(this);
            }
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
                    //System.Windows.Clipboard.SetText(this.mValue.ToString());
                    //e.Handled = true;
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
                e.Handled = DoubleValueHandler.doubleValueHandler.HandleKeyDown(this, e);
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
                DoubleValueHandler.doubleValueHandler.HandleDeleteKey(this);
            }
        }

        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            e.Handled = DoubleValueHandler.doubleValueHandler.MatchWithMask(this, e.Text);
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

            double? Val = this.Value;
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
                    bool cancelValueChanging = this.TriggerValueChangingEvent(new ValueChangingEventArgs() { OldValue = this.Value, NewValue = Val });
                    if (!cancelValueChanging)
                        this.Value = Val;
                }
            }
            base.OnLostFocus(e);
            this.checktext = "";
        }

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
            if (this.Value != null && !(double.IsNaN((double)this.Value))) 
            {
                if (mValue == 0 && this.Value == null)
                {
                    if (this.UseNullOption)
                        this.SetValue(true, null);
                    else
                    {
                        this.SetValue(true, 0.0);
                        NumberFormatInfo numberFormat = this.GetCulture().NumberFormat;
                        this.MaskedText = ((double)mValue).ToString("N", numberFormat);
                    }
                }
                else
                {
                    NumberFormatInfo numberFormat = this.GetCulture().NumberFormat;
                    this.MaskedText = ((double)mValue).ToString("N", numberFormat);
                }
            }
            else
            {
                this.MaskedText = "";
            }
        }

        internal bool SetValue(bool? IsReload, double? _Value)
        {
            if (IsReload == false)
            {
                mValueChanged = false;

                bool cancelValueChanging = this.TriggerValueChangingEvent(new ValueChangingEventArgs() { OldValue = this.Value, NewValue = _Value });
                if (!cancelValueChanging)
                {
                    this.Value = _Value;
                    mValueChanged = true;
                    return true;
                }
                else
                {
                    if (this.Value != null)
                    {
                        double val = (double)this.Value;
                        this.MaskedText = val.ToString("N", NumberFormat);
                    }
                    return true; 
                }
            }
            else if (IsReload == true)
            {
                bool cancelValueChanging = this.TriggerValueChangingEvent(new ValueChangingEventArgs() { OldValue = this.Value, NewValue = _Value });
                if (!cancelValueChanging)
                {
                    this.Value = _Value;
                    return true;
                }
                else
                {
                    if (this.Value != null)
                    {
                        double val = (double)this.Value;
                        this.MaskedText = val.ToString("N", NumberFormat);
                    }
                    return true;
                }
            }
            return false;
        }

        internal bool TriggerValueChangingEvent(ValueChangingEventArgs args)
        {
            if (this.ValueChanging != null)
            {
                ValueChanging(this, args);                
                return args.Cancel;
            }
            return false; 
        }

        internal double? ValidateValue(double? Val)
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

            if (!GroupSeperatorEnabled)
            {
                cultureInfo.NumberFormat.NumberGroupSeparator = string.Empty;
            }

            if (GroupSeperatorEnabled == true)
            {
                if (!NumberGroupSeparator.Equals(string.Empty))
                    cultureInfo.NumberFormat.NumberGroupSeparator = NumberGroupSeparator;
            }

            if (NumberDecimalDigits >= 0)
                cultureInfo.NumberFormat.NumberDecimalDigits = this.NumberDecimalDigits;

            if (!NumberDecimalSeparator.Equals(string.Empty))
                cultureInfo.NumberFormat.NumberDecimalSeparator = this.NumberDecimalSeparator;

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

            //Repeated code commented  - Internally Raised issue.

            //if (GroupSeperatorEnabled == true)
            //{
            //    if (!cultureInfo.NumberFormat.NumberGroupSeparator.Equals(String.Empty))
            //    {
            //        cultureInfo.NumberFormat.NumberGroupSeparator = cultureInfo.NumberFormat.NumberGroupSeparator;//.NumberGroupSeparator[0].ToString();
            //    }
            //}

            //if (cultureInfo.NumberFormat.NumberDecimalSeparator != String.Empty)
            //{
            //    cultureInfo.NumberFormat.NumberDecimalSeparator = cultureInfo.NumberFormat.NumberDecimalSeparator;//.NumberDecimalSeparator[0].ToString();
            //}

            return cultureInfo;
        }

        private static object CoerceValue(DependencyObject d, object baseValue)
        {
            DoubleTextBox doubleTextBox = (DoubleTextBox)d;
            if (baseValue != null)
            {
                double? value = (double?)baseValue;
                if (doubleTextBox.mValueChanged == true)
                {
                    if (value > doubleTextBox.MaxValue)
                    {
                        value = doubleTextBox.MaxValue;
                    }
                    else if (value < doubleTextBox.MinValue)
                    {
                        value = doubleTextBox.MinValue;
                    }
                }
                if (value != null)
                {
                    doubleTextBox.IsNegative = value < 0 ? true : false;
                    doubleTextBox.IsZero = value == 0 ? true : false;
                    doubleTextBox.IsNull = false;
                }
                return value;
            }
            else
            {
                if (doubleTextBox.UseNullOption)
                {
                    doubleTextBox.IsNull = true;
                    doubleTextBox.IsNegative = false;
                    doubleTextBox.IsZero = false;
                    return doubleTextBox.NullValue;
                    //return baseValue;
                }
                else
                {
                    double value = 0L;
                    if (doubleTextBox.mValueChanged == true)
                    {
                        if (value > doubleTextBox.MaxValue)
                        {
                            value = doubleTextBox.MaxValue;
                        }
                        if (value < doubleTextBox.MinValue)
                        {
                            value = doubleTextBox.MinValue;
                        }
                    }
                    doubleTextBox.IsNegative = value < 0 ? true : false;
                    doubleTextBox.IsZero = value == 0 ? true : false;
                    doubleTextBox.IsNull = false;
                    return value;
                }
            }
        }

        private static object CoerceMinValue(DependencyObject d, object baseValue)
        {
            DoubleTextBox doubleTextBox = (DoubleTextBox)d;
            if (doubleTextBox.MinValue > doubleTextBox.MaxValue)
            {
                return doubleTextBox.MaxValue;
            }
            return baseValue;
        }

        private static object CoerceMaxValue(DependencyObject d, object baseValue)
        {
            DoubleTextBox doubleTextBox = (DoubleTextBox)d;
            if (doubleTextBox.MinValue > doubleTextBox.MaxValue)
            {
                return doubleTextBox.MinValue;
            }
            return baseValue;
        }

        #endregion
        private void copy()
        {
            Clipboard.SetText(this.SelectedText);           
        }
        private new void Paste()
        {
            if (this.IsReadOnly == false)
            {
                double val1;
                double oldval =(double)this.Value;
                string oldselection = Clipboard.GetText();
                int index1 = this.SelectionStart;
                NumberFormatInfo numberFormat = this.GetCulture().NumberFormat;
                string copiedValue = string.Empty;
                string afterseperator = string.Empty;
                int seperatorindex = 0;
               // int seperatorCount = 0;
                for (int i = 0; i < oldselection.Length; i++)
                {
                    if (numberFormat != null)
                    {
                        if (numberFormat.NumberDecimalSeparator != null)
                        {
                            if (char.IsDigit(oldselection[i]) && i == seperatorindex)
                            {
                                seperatorindex = i + 1;
                                copiedValue += oldselection[i];
                            }
                            else if (oldselection[i].ToString() == numberFormat.NumberDecimalSeparator)
                            {
                                seperatorindex = i;
                            }
                            else if (char.IsDigit(oldselection[i]))
                            {
                                afterseperator += oldselection[i];
                            }
                            else
                            {
                                if (i <= seperatorindex)
                                {
                                    seperatorindex = i + 1;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (char.IsDigit(oldselection[i]) && i == seperatorindex)
                        {
                            seperatorindex = i + 1;
                            copiedValue += oldselection[i];
                        }
                    }

                }

                if (this.SelectionLength > 0)
                {
                    if (oldselection == string.Empty)
                    {
                        if (this.OldValue != null)
                        {
                            this.SetValue(false, this.OldValue);
                            double val3 = (double)this.OldValue;
                            this.Text = val3.ToString("N", numberFormat);
                        }
                    }
                    else
                    {
                        if (this.SelectedText.Length == this.Text.Length || this.Text.Contains(this.SelectedText) || this.SelectedText.Contains(numberFormat.NumberDecimalSeparator))
                        {
                            this.Text = this.Text.Replace(this.SelectedText, copiedValue);
                            if (numberFormat != null)
                            {
                                if (numberFormat.NumberDecimalSeparator != null)
                                {
                                    if (afterseperator != string.Empty && !this.Text.Contains(numberFormat.NumberDecimalSeparator))
                                    {
                                        this.Text = this.Text + numberFormat.NumberDecimalSeparator + afterseperator;
                                    }
                                    else if (this.Text.Contains(numberFormat.NumberDecimalSeparator))
                                    {
                                        for (int i = 0; i < this.Text.Length; i++)
                                        {
                                            if (this.Text[i].ToString() == numberFormat.NumberDecimalSeparator)
                                            {
                                                seperatorindex = i;
                                            }
                                        }
                                        this.Text = this.Text.Insert(seperatorindex + 1, afterseperator);
                                    }
                                }
                            }
                        }
                    }
                }


                else
                {
                    this.Text = this.Text.Insert(this.SelectionStart, copiedValue);
                    if (numberFormat != null)
                    {
                        if (numberFormat.NumberDecimalSeparator != null)
                        {
                            for (int i = 0; i < this.Text.Length; i++)
                            {
                                if (this.Text[i].ToString() == numberFormat.NumberDecimalSeparator)
                                {
                                    seperatorindex = i;
                                }
                            }
                            this.Text = this.Text.Insert(seperatorindex + 1, afterseperator);
                        }
                    }
                }
                double.TryParse(this.Text, out val1);
                if ((val1 > this.MaxValue) && (this.MaxValidation == MaxValidation.OnKeyPress) || val1 < this.MinValue && (this.MinValidation == MinValidation.OnKeyPress))
                {
                    if ((val1 > this.MaxValue) && (this.MaxValidation == MaxValidation.OnKeyPress))
                    {
                        if (this.MaxValueOnExceedMaxDigit)
                        {
                            val1 = this.MaxValue;
                            this.SetValue(false, val1);
                            this.Text = val1.ToString("N", numberFormat);
                            this.CaretIndex = index1;
                            double.TryParse(this.Text, out val1);
                            this.SetValue(false, val1);
                        }
                        else
                        {
                            this.SetValue(false, oldval);
                            double val3 = oldval;
                            this.Text = val3.ToString("N", numberFormat);
                        }                                                        
                        
                    }
                    if (val1 < this.MinValue && (this.MinValidation == MinValidation.OnKeyPress))
                    {
                        if (this.MinValueOnExceedMinDigit)
                        {
                            val1 = this.MinValue;
                            this.SetValue(false, val1);
                            this.Text = val1.ToString("N", numberFormat);
                            this.CaretIndex = index1;
                            double.TryParse(this.Text, out val1);
                            this.SetValue(false, val1);
                        }
                        else
                        {
                            this.SetValue(false, oldval);
                            double val3 = oldval;
                            this.Text = val3.ToString("N", numberFormat);
                        }                           
                    }
                }
                else
                {
                    this.SetValue(false, val1);
                    this.Text = val1.ToString("N", numberFormat);
                    this.CaretIndex = index1 + copiedValue.Length;
                    double.TryParse(this.Text, out val1);
                    this.SetValue(false, val1);
                }
            }
        }
        void DoubleTextbox_Loaded(object sender, RoutedEventArgs e)
        {
            mIsLoaded = true;
            object tempObj = CoerceValue(this, Value);
            double? tempVal = (double?)tempObj;
            if (this.IsNull == true)
            {
                this.WatermarkVisibility = Visibility.Visible;
            }
            if (tempVal != Value)
            {
                bool cancelValueChanging = this.TriggerValueChangingEvent(new ValueChangingEventArgs() { OldValue = this.Value, NewValue = tempVal });
                if (!cancelValueChanging)
                {
                    if (UseNullOption)
                    {
                        if (Value != null)
                            Value = tempVal;
                    }
                    else
                        Value = tempVal;
                }
            }
            else
            {
                FormatText();
            }
            if (this.TextSelectionOnFocus)
            {
                base.OnGotFocus(e);                             
            }
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

        #region Properties

        public double? Value
        {
            get { return (double?)GetValue(ValueProperty); }
            set
            {
                //object coerceValue = CoerceValue(this, value);
                SetValue(ValueProperty, value);
            }
        }

        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public string NumberGroupSeparator
        {
            get { return (string)GetValue(NumberGroupSeparatorProperty); }
            set { SetValue(NumberGroupSeparatorProperty, value); }
        }

#if SILVERLIGHT
        public int[] NumberGroupSizes
        {
            get { return (int[])GetValue(NumberGroupSizesProperty); }
            set { SetValue(NumberGroupSizesProperty, value); }
        }

        public static readonly DependencyProperty NumberGroupSizesProperty =
            DependencyProperty.Register("NumberGroupSizes", typeof(int[]), typeof(DoubleTextBox), new PropertyMetadata(null,OnNumberGroupSizesChanged));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double?), typeof(DoubleTextBox), new PropertyMetadata(null, new PropertyChangedCallback(OnValueChanged)));
#endif
#if WPF
        public Int32Collection NumberGroupSizes
        {
            get { return (Int32Collection)GetValue(NumberGroupSizesProperty); }
            set { SetValue(NumberGroupSizesProperty, value); }
        }

        public static readonly DependencyProperty NumberGroupSizesProperty =
            DependencyProperty.Register("NumberGroupSizes", typeof(Int32Collection), typeof(DoubleTextBox), new PropertyMetadata(new Int32Collection(), OnNumberGroupSizesChanged));

        // In WPF Value property acts with TwoWay Binding by default.
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double?), typeof(DoubleTextBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnValueChanged), new CoerceValueCallback(CoerceValue)));

#endif
        public int NumberDecimalDigits
        {
            get { return (int)GetValue(NumberDecimalDigitsProperty); }
            set { SetValue(NumberDecimalDigitsProperty, value); }
        }

        public string NumberDecimalSeparator
        {
            get { return (string)GetValue(NumberDecimalSeparatorProperty); }
            set { SetValue(NumberDecimalSeparatorProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(double), typeof(DoubleTextBox), new PropertyMetadata(double.MinValue, new PropertyChangedCallback(OnMinValueChanged)));

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(DoubleTextBox), new PropertyMetadata(double.MaxValue, new PropertyChangedCallback(OnMaxValueChanged)));

        public static readonly DependencyProperty NumberGroupSeparatorProperty =
            DependencyProperty.Register("NumberGroupSeparator", typeof(string), typeof(DoubleTextBox), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnNumberGroupSeparatorChanged)));

        public static readonly DependencyProperty NumberDecimalDigitsProperty =
            DependencyProperty.Register("NumberDecimalDigits", typeof(int), typeof(DoubleTextBox), new PropertyMetadata(-1, OnNumberDecimalDigitsChanged));

        public static readonly DependencyProperty NumberDecimalSeparatorProperty =
            DependencyProperty.Register("NumberDecimalSeparator", typeof(string), typeof(DoubleTextBox), new PropertyMetadata(string.Empty, OnNumberDecimalSeparatorChanged));
        #endregion


        public bool GroupSeperatorEnabled
        {
            get { return (bool)GetValue(GroupSeperatorEnabledProperty); }
            set { SetValue(GroupSeperatorEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GroupSeperatorEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GroupSeperatorEnabledProperty =
            DependencyProperty.Register("GroupSeperatorEnabled", typeof(bool), typeof(DoubleTextBox), new PropertyMetadata(true, new PropertyChangedCallback(OnNumberGroupSeparatorChanged)));




        public double ScrollInterval
        {
            get { return (double)GetValue(ScrollIntervalProperty); }
            set { SetValue(ScrollIntervalProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScrollInterval.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScrollIntervalProperty =
            DependencyProperty.Register("ScrollInterval", typeof(double), typeof(DoubleTextBox), new PropertyMetadata(1.0));


        public double Step
        {
            get { return (double)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Step.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register("Step", typeof(double), typeof(DoubleTextBox), new PropertyMetadata(1d));



        #region PropertyChanged Callbacks


        public static void OnNumberGroupSizesChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            DoubleTextBox d = (DoubleTextBox)obj;
            if (d != null)
            {
                d.OnNumberGroupSizesChanged(args);
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

        public static void OnNumberDecimalSeparatorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            DoubleTextBox d = (DoubleTextBox)obj;
            if (d != null)
            {
                d.OnNumberDecimalSeparatorChanged(args);
            }

        }

        protected void OnNumberDecimalSeparatorChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.NumberDecimalSeparatorChanged != null)
            {
                this.NumberDecimalSeparatorChanged(this, args);
            }
            if (mIsLoaded)
            {
                this.FormatText();
            }
        }

        public static void OnNumberDecimalDigitsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            DoubleTextBox d = (DoubleTextBox)obj;
            if (d != null)
            {
                d.OnNumberDecimalDigitsChanged(args);
            }
        }

        protected void OnNumberDecimalDigitsChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.NumberDecimalDigitsChanged != null)
            {
                this.NumberDecimalDigitsChanged(this, args);
            }
            if (mIsLoaded)
            {
                this.FormatText();
            }
        }

        public static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if ((DoubleTextBox)obj != null)
                ((DoubleTextBox)obj).OnValueChanged(args);
        }

        protected void OnValueChanged(DependencyPropertyChangedEventArgs args)
        {

#if SILVERLIGHT
            object coerceValue = CoerceValue(this, this.Value);
            if (this.Value != (double?)coerceValue)
            {
                this.Value = (double?)coerceValue;
                return;
            }
#endif
            if (this.Value != null && !(double.IsNaN((double)this.Value))) 
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

            OldValue = (double?)args.OldValue;
            mValue = this.Value;

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
            if ((DoubleTextBox)obj != null)
            {
                ((DoubleTextBox)obj).OnMinValueChanged(args);
            }
        }

        protected void OnMinValueChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.MinValueChanged != null)
                this.MinValueChanged(this, args);

            if (this.MaxValue < this.MinValue)
                this.MaxValue = this.MinValue;

            if (this.Value != this.ValidateValue(this.Value))
            {
                double? tempVal = this.ValidateValue(this.Value);

                bool cancelValueChanging = this.TriggerValueChangingEvent(new ValueChangingEventArgs() { OldValue = this.Value, NewValue = tempVal });
                if (!cancelValueChanging)
                    this.Value = tempVal;
            }
        }

        public static void OnMaxValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if ((DoubleTextBox)obj != null)
            {
                ((DoubleTextBox)obj).OnMaxValueChanged(args);
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
                double? tempVal = this.ValidateValue(this.Value);

                bool cancelValueChanging = this.TriggerValueChangingEvent(new ValueChangingEventArgs() { OldValue = this.Value, NewValue = tempVal });
                if (!cancelValueChanging)

                    this.Value = tempVal;
            }
        }

        private static void OnNumberGroupSeparatorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if ((DoubleTextBox)obj != null)
                ((DoubleTextBox)obj).OnNumberGroupSeparatorChanged(args);
        }

        protected virtual void OnNumberGroupSeparatorChanged(DependencyPropertyChangedEventArgs e)
        {
            if (this.NumberGroupSeparatorChanged != null)
                this.NumberGroupSeparatorChanged(this, e);
            if (mIsLoaded)
            {
                this.FormatText();
            }
        }

        #endregion


        public double? NullValue
        {
            get { return (double?)GetValue(NullValueProperty); }
            set { SetValue(NullValueProperty, value); }
        }

        public static readonly DependencyProperty NullValueProperty =
            DependencyProperty.Register("NullValue", typeof(double?), typeof(DoubleTextBox), new PropertyMetadata(null, OnNullValueChanged));

        public static void OnNullValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if ((DoubleTextBox)obj != null)
                ((DoubleTextBox)obj).OnNullValueChanged(args);
        }

        protected void OnNullValueChanged(DependencyPropertyChangedEventArgs args)
        {
        }
    }

    public class ValueChangingEventArgs : CancelEventArgs
    {
        public ValueChangingEventArgs()
        { }
        // Summary:
        //     Gets the value of the property after the change.
        //
        // Returns:
        //     The property value after the change.
        public object NewValue { get; set; }
        //
        // Summary:
        //     Gets the value of the property before the change.
        //
        // Returns:
        //     The property value before the change.
        public object OldValue { get; set; }
    }

}
