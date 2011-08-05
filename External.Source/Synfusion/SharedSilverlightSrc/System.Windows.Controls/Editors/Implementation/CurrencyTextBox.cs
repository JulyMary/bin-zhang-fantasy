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
      Type = typeof(CurrencyTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Office2007BlueStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2007Black,
    Type = typeof(CurrencyTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Office2007BlackStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2007Silver,
    Type = typeof(CurrencyTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Office2007SilverStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2010Blue,
     Type = typeof(CurrencyTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Office2010BlueStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2010Black,
    Type = typeof(CurrencyTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Office2010BlackStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2010Silver,
    Type = typeof(CurrencyTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Office2010SilverStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2003,
    Type = typeof(CurrencyTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Office2003Style.xaml")]
    [SkinType(SkinVisualStyle = Skin.Blend,
    Type = typeof(CurrencyTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/BlendStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.SyncOrange,
    Type = typeof(CurrencyTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/SyncOrangeStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.ShinyRed,
    Type = typeof(CurrencyTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/ShinyRedStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.ShinyBlue,
    Type = typeof(CurrencyTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/ShinyBlueStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Default,
    Type = typeof(CurrencyTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Generic.xaml")]
    [SkinType(SkinVisualStyle = Skin.VS2010,
   Type = typeof(CurrencyTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/VS2010Style.xaml")]
#endif
#if SILVERLIGHT
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Blend,
       Type = typeof(CurrencyTextBox), XamlResource = "/Syncfusion.Theming.Blend;component/Editors/CurrencyTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2007Blue,
        Type = typeof(CurrencyTextBox), XamlResource = "/Syncfusion.Theming.Office2007Blue;component/Editors/CurrencyTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2007Black,
        Type = typeof(CurrencyTextBox), XamlResource = "/Syncfusion.Theming.Office2007Black;component/Editors/CurrencyTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2007Silver,
        Type = typeof(CurrencyTextBox), XamlResource = "/Syncfusion.Theming.Office2007Silver;component/Editors/CurrencyTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Default,
        Type = typeof(CurrencyTextBox), XamlResource = "/Syncfusion.Theming.Default;component/Editors/CurrencyTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2010Blue,
        Type = typeof(CurrencyTextBox), XamlResource = "/Syncfusion.Theming.Office2010Blue;component/Editors/CurrencyTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2010Black,
        Type = typeof(CurrencyTextBox), XamlResource = "/Syncfusion.Theming.Office2010Black;component/Editors/CurrencyTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2010Silver,
        Type = typeof(CurrencyTextBox), XamlResource = "/Syncfusion.Theming.Office2010Silver;component/Editors/CurrencyTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Windows7,
        Type = typeof(CurrencyTextBox), XamlResource = "/Syncfusion.Theming.Windows7;component/Editors/CurrencyTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.VS2010,
        Type = typeof(CurrencyTextBox), XamlResource = "/Syncfusion.Theming.VS2010;component/Editors/CurrencyTextBox.xaml")]  
#endif
    public class CurrencyTextBox : EditorBase
    {
        #region Events
        /// <summary>
        /// Event that is raised when <see cref="MinValue"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback MinValueChanged;

        /// <summary>
        /// Event that is raised when <see cref="CurrencySymbolPosition"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback CurrencySymbolPositionChanged;

        /// <summary>
        /// Event that is raised when <see cref="Value"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback ValueChanged;

        /// <summary>
        /// Event that is raised when <see cref="MaxValue"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback MaxValueChanged;

       

        /// <summary>
        /// Event that is raised when <see cref="CurrencyDecimalDigits"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback CurrencyDecimalDigitsChanged;

        /// <summary>
        /// Event that is raised when <see cref="CurrencyDecimalSeparator"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback CurrencyDecimalSeparatorChanged;

        /// <summary>
        /// Event that is raised when <see cref="CurrencyGroupSeparator"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback CurrencyGroupSeparatorChanged;

        /// <summary>
        /// Event that is raised when <see cref="CurrencyGroupSizes"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback CurrencyGroupSizesChanged;

        /// <summary>
        /// Event that is raised when <see cref="CurrencyNegativePattern"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback CurrencyNegativePatternChanged;

        /// <summary>
        /// Event that is raised when <see cref="CurrencyPositivePattern"/>x property is changed.
        /// </summary>
        public event PropertyChangedCallback CurrencyPositivePatternChanged;

        /// <summary>
        /// Event that is raised when <see cref="CurrencySymbol"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback CurrencySymbolChanged;
        #endregion

        #region Members

        internal decimal? OldValue;
        internal decimal? mValue;
        internal bool? mValueChanged = true;
        internal bool mIsLoaded = false;
        internal string checktext="";
        #endregion

        #region Constructor

#if WPF
        static CurrencyTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CurrencyTextBox), new FrameworkPropertyMetadata(typeof(CurrencyTextBox)));
            EnvironmentTest.ValidateLicense(typeof(CurrencyTextBox));
        }
#endif
        public ICommand pastecommand { get; private set; }
        public ICommand copycommand { get; private set; }
        public ICommand cutcommand { get; private set; }
        public CurrencyTextBox()
        {
            pastecommand = new DelegateCommand(_pastecommand, Canpaste);
            copycommand = new DelegateCommand(_copycommand, Canpaste);
            cutcommand = new DelegateCommand(_cutcommand, Canpaste);
#if WPF
            this.AddHandler(CommandManager.PreviewExecutedEvent,
new ExecutedRoutedEventHandler(CommandExecuted), true);
#endif

#if SILVERLIGHT
            this.DefaultStyleKey = typeof(CurrencyTextBox);
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
        Border Focused_Border=null;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
#if WPF
            PART_ContentHost = this.GetTemplateChild("PART_ContentHost") as ScrollViewer;
#endif
#if SILVERLIGHT
            PART_ContentHost = this.GetTemplateChild("ContentElement") as ScrollViewer;
#endif
            //Focused_Border = this.GetTemplateChild("Border") as Border;
            //if (Focused_Border != null)
            //{
            //    Focused_Border.Background = this.Background;
            //}
            
            
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


private new void Paste()
{
    if (this.IsReadOnly == false)
    {
        string oldselection = Clipboard.GetText();
        int index1 = this.SelectionStart;
        NumberFormatInfo numberFormat = this.GetCulture().NumberFormat;
        string oldtext = this.Text;
        CurrencyTextBox currencybox = this as CurrencyTextBox;
        string copiedValue = string.Empty;
        string afterseperator = string.Empty;
        int seperatorindex = 0;
        decimal oldval = (decimal)this.Value;
        decimal val1;
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
                    currencybox.SetValue(false, currencybox.OldValue);
                    decimal val3 = (decimal)currencybox.OldValue;
                    this.Text = val3.ToString("C", numberFormat);
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
        if (numberFormat != null)
        {
            if (this.Text.Contains(numberFormat.CurrencySymbol))
            {
                for (int x = 0; x < this.Text.Length; x++)
                {
                    if (this.Text[x].ToString() == numberFormat.CurrencySymbol)
                    {
                        this.Text = this.Text.Remove(x, 1);
                    }
                }
            }
        }
        decimal.TryParse(this.Text, out val1);
        if ((val1 > this.MaxValue) && (this.MaxValidation == MaxValidation.OnKeyPress) || val1 < this.MinValue && (this.MinValidation == MinValidation.OnKeyPress))
        {
            if ((val1 > this.MaxValue) && (this.MaxValidation == MaxValidation.OnKeyPress))
            {
                if (this.MaxValueOnExceedMaxDigit)
                {
                    val1 = this.MaxValue;
                    this.CaretIndex = index1;
                    this.SetValue(false, val1);
                    this.Text = val1.ToString("C", numberFormat);
                }
                else
                {
                    this.SetValue(false, oldval);
                    decimal val3 = oldval;
                    this.Text = val3.ToString("C", numberFormat);
                }

            }
            if (val1 < this.MinValue && (this.MinValidation == MinValidation.OnKeyPress))
            {
                if (this.MinValueOnExceedMinDigit)
                {
                    val1 = this.MinValue;
                    this.CaretIndex = index1;
                    this.SetValue(false, val1);
                    this.Text = val1.ToString("C", numberFormat);
                }
                else
                {
                    this.SetValue(false, oldval);
                    decimal val3 = oldval;
                    this.Text = val3.ToString("C", numberFormat);
                }
            }
        }
        else
        {
            this.SetValue(false, val1);
            this.CaretIndex = index1 + copiedValue.Length;
            this.Text = val1.ToString("C", numberFormat);
        }
    }
}


        private void copy()
        {
            Clipboard.SetText(this.SelectedText);
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
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            if (e.Delta > 0)
            {
                CurrencyValueHandler.currencyValueHandler.HandleUpKey(this);
            }
            else if (e.Delta < 0)
            {
                CurrencyValueHandler.currencyValueHandler.HandleDownKey(this);
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
                e.Handled = CurrencyValueHandler.currencyValueHandler.HandleKeyDown(this, e);
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
                CurrencyValueHandler.currencyValueHandler.HandleDeleteKey(this);
            }
        }

        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            e.Handled = CurrencyValueHandler.currencyValueHandler.MatchWithMask(this, e.Text);
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
            if (this.Focused_Border != null)
            {
                this.Focused_Border.Background = this.Background;
            }
            this.checktext = "";
            decimal? Val = this.Value;
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
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            if (Focused_Border != null)
            {
                Focused_Border.Background = FocusedBackground;
            }
            if (this.EnableFocusColors && this.PART_ContentHost != null)
            {
                this.PART_ContentHost.Background = this.FocusedBackground;
            }

            
            base.OnGotFocus(e);
        }

        #endregion

        #region Internal Methods

        internal void FormatText()
        {
		 if (this.Value != null)
            {
                NumberFormatInfo numberFormat = this.GetCulture().NumberFormat;
                this.Text = ((decimal)mValue).ToString("C", numberFormat);
                this.MaskedText = this.Text;
            }
            else
            {
                this.MaskedText = "";
            }
        }

        internal void SetValue(bool? IsReload, decimal? _Value)
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

        internal decimal? ValidateValue(decimal? Val)
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

            //CurrencyDecimalDigits
            //CurrencyDecimalSeparator
            //CurrencyGroupSeparator
            //CurrencyGroupSizes
            //CurrencyNegativePattern
            //CurrencyPositivePattern
            //CurrencySymbol

            if (CurrencyDecimalDigits >= 0)
                cultureInfo.NumberFormat.CurrencyDecimalDigits = this.CurrencyDecimalDigits;

            if (!CurrencyDecimalSeparator.Equals(string.Empty))
                cultureInfo.NumberFormat.CurrencyDecimalSeparator = this.CurrencyDecimalSeparator;

            if (!GroupSeperatorEnabled)
            {
                cultureInfo.NumberFormat.CurrencyGroupSeparator = string.Empty;
            }
            if (GroupSeperatorEnabled == true)
            {
                if (!CurrencyGroupSeparator.Equals(string.Empty))
                    cultureInfo.NumberFormat.CurrencyGroupSeparator = this.CurrencyGroupSeparator;
            }
#if WPF
                int count = this.CurrencyGroupSizes.Count;
                if (count > 0)
                {
                    int[] ngs = new int[count];

                    for (int i = 0; i < count; i++)
                    {
                        ngs[i] = this.CurrencyGroupSizes[i];
                    }
                    cultureInfo.NumberFormat.CurrencyGroupSizes = ngs;
                }
#endif

#if SILVERLIGHT
              if (CurrencyGroupSizes != null)
                  cultureInfo.NumberFormat.CurrencyGroupSizes = this.CurrencyGroupSizes;
#endif
                if (this.CurrencyNegativePattern > -1 && this.CurrencyNegativePattern < 15)
                {
                    cultureInfo.NumberFormat.CurrencyNegativePattern = this.CurrencyNegativePattern;
                }

                if (this.CurrencyPositivePattern > -1 && this.CurrencyPositivePattern < 4)
                {
                    cultureInfo.NumberFormat.CurrencyPositivePattern = this.CurrencyPositivePattern;
                }
                
                //if (CurrencySymbolPosition == CurrencySymbolPosition.Left)
                //{
                //    if (this.CurrencyNegativePattern == 0 || this.CurrencyNegativePattern == 1 || this.CurrencyNegativePattern == 2 ||
                //        this.CurrencyNegativePattern == 3 || this.CurrencyNegativePattern == 9 || this.CurrencyNegativePattern == 11 ||
                //        this.CurrencyNegativePattern == 12 || this.CurrencyNegativePattern == 14)
                //    { }
                //    else
                //    {
                //        cultureInfo.NumberFormat.CurrencyNegativePattern = 0;
                //    }

                //    if (this.CurrencyPositivePattern == 0 || this.CurrencyPositivePattern == 2)
                //    { }
                //    else
                //    {
                //        cultureInfo.NumberFormat.CurrencyPositivePattern = 2;
                //    }
                //}
                //else
                //{
                //    if (this.CurrencyNegativePattern == 4 || this.CurrencyNegativePattern == 5 || this.CurrencyNegativePattern == 6 ||
                //        this.CurrencyNegativePattern == 7 || this.CurrencyNegativePattern == 8 || this.CurrencyNegativePattern == 10 ||
                //        this.CurrencyNegativePattern == 13 || this.CurrencyNegativePattern == 15)
                //    { }
                //    else
                //    {
                //        cultureInfo.NumberFormat.CurrencyNegativePattern = 4;
                //    }

                //    if (this.CurrencyPositivePattern == 1 || this.CurrencyPositivePattern == 3)
                //    { }
                //    else
                //    {
                //        cultureInfo.NumberFormat.CurrencyPositivePattern = 3;
                //    }
                //}

                if (!CurrencySymbol.Equals(string.Empty))
                    cultureInfo.NumberFormat.CurrencySymbol = this.CurrencySymbol;
               

            cultureInfo.NumberFormat.CurrencyDecimalSeparator = cultureInfo.NumberFormat.CurrencyDecimalSeparator[0].ToString();

            if (!GroupSeperatorEnabled)
            {
                cultureInfo.NumberFormat.CurrencyGroupSeparator = string.Empty;
            }
            if (GroupSeperatorEnabled == true)
            {
                cultureInfo.NumberFormat.CurrencyGroupSeparator = cultureInfo.NumberFormat.CurrencyGroupSeparator[0].ToString();
            }
            //cultureInfo.NumberFormat.CurrencySymbol = cultureInfo.NumberFormat.CurrencySymbol[0].ToString();

            return cultureInfo;
        }

        private static object CoerceValue(DependencyObject d, object baseValue)
        {
            CurrencyTextBox currencyTextBox = (CurrencyTextBox)d;
            if (baseValue != null)
            {
                decimal? value = (decimal?)baseValue;
                if (currencyTextBox.mValueChanged == true)
                {
                    if (value > currencyTextBox.MaxValue)
                    {
                        value = currencyTextBox.MaxValue;
                    }
                    else if (value < currencyTextBox.MinValue)
                    {
                        value = currencyTextBox.MinValue;
                    }                   
                }
                if (value != null)
                {
                    currencyTextBox.IsNegative = value < 0 ? true : false;
                    currencyTextBox.IsZero = value == 0 ? true : false;
                    currencyTextBox.IsNull = false;
                }
                return value;
            }
            else
            {
                if (currencyTextBox.UseNullOption)
                {
                    currencyTextBox.IsNull = true;
                    currencyTextBox.IsNegative = false;
                    currencyTextBox.IsZero = false;
                    return currencyTextBox.NullValue;
                    //return baseValue;
                }
                else
                {
                    decimal value = 0L;
                    if (currencyTextBox.mValueChanged == true)
                    {
                        if (value > currencyTextBox.MaxValue)
                        {
                            value = currencyTextBox.MaxValue;
                        }
                        if (value < currencyTextBox.MinValue)
                        {
                            value = currencyTextBox.MinValue;
                        }
                    }
                    currencyTextBox.IsNegative = value < 0 ? true : false;
                    currencyTextBox.IsZero = value == 0 ? true : false;
                    currencyTextBox.IsNull = false;
                    return value;
                }
            }
        }

        private static object CoerceMinValue(DependencyObject d, object baseValue)
        {
            CurrencyTextBox currencyTextBox = (CurrencyTextBox)d;
            if (currencyTextBox.MinValue > currencyTextBox.MaxValue)
            {
                return currencyTextBox.MaxValue;
            }
            return baseValue;
        }

        private static object CoerceMaxValue(DependencyObject d, object baseValue)
        {
            CurrencyTextBox currencyTextBox = (CurrencyTextBox)d;
            if (currencyTextBox.MinValue > currencyTextBox.MaxValue)
            {
                return currencyTextBox.MinValue;
            }
            return baseValue;
        }

        #endregion

        
        void IntegerTextbox_Loaded(object sender, RoutedEventArgs e)
        {
            mIsLoaded = true;
            object tempObj = CoerceValue(this, Value);
            decimal? tempVal = (decimal?)tempObj;
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
                FormatText();
            }
        }

        #region Properties

#if SILVERLIGHT
        [TypeConverter(typeof(DecimalTypeConverter))]
#endif
        public decimal? Value
        {
            get { return (decimal?)GetValue(ValueProperty); }
            set
            {
                //object coerceValue = CoerceValue(this, value);
                SetValue(ValueProperty, value);
            }
        }

#if SILVERLIGHT
        [TypeConverter(typeof(DecimalTypeConverter))]
#endif
        public decimal MinValue
        {
            get { return (decimal)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

#if SILVERLIGHT
        [TypeConverter(typeof(DecimalTypeConverter))]
#endif
        public decimal MaxValue
        {
            get { return (decimal)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

#if WPF

        // In WPF Value property acts with TwoWay binding by default.
        public static readonly DependencyProperty ValueProperty =
      DependencyProperty.Register("Value", typeof(decimal?), typeof(CurrencyTextBox), new FrameworkPropertyMetadata(null,FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnValueChanged), new CoerceValueCallback(CoerceValue)));
#endif
#if SILVERLIGHT
        public static readonly DependencyProperty ValueProperty =
    DependencyProperty.Register("Value", typeof(decimal?), typeof(CurrencyTextBox), new PropertyMetadata(null, new PropertyChangedCallback(OnValueChanged)));
#endif
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(decimal), typeof(CurrencyTextBox), new PropertyMetadata(decimal.MinValue, new PropertyChangedCallback(OnMinValueChanged)));

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(decimal), typeof(CurrencyTextBox), new PropertyMetadata(decimal.MaxValue, new PropertyChangedCallback(OnMaxValueChanged)));

        public int CurrencyDecimalDigits
        {
            get { return (int)GetValue(CurrencyDecimalDigitsProperty); }
            set { SetValue(CurrencyDecimalDigitsProperty, value); }
        }

        public static readonly DependencyProperty CurrencyDecimalDigitsProperty =
            DependencyProperty.Register("CurrencyDecimalDigits", typeof(int), typeof(CurrencyTextBox), new PropertyMetadata((int)-1, OnCurrencyDecimalDigitsChanged));

        public string CurrencyDecimalSeparator
        {
            get { return (string)GetValue(CurrencyDecimalSeparatorProperty); }
            set { SetValue(CurrencyDecimalSeparatorProperty, value); }
        }

        public static readonly DependencyProperty CurrencyDecimalSeparatorProperty =
            DependencyProperty.Register("CurrencyDecimalSeparator", typeof(string), typeof(CurrencyTextBox), new PropertyMetadata(string.Empty, OnCurrencyDecimalSeparatorChanged));

        public string CurrencyGroupSeparator
        {
            get { return (string)GetValue(CurrencyGroupSeparatorProperty); }
            set { SetValue(CurrencyGroupSeparatorProperty, value); }
        }
        
        public static readonly DependencyProperty CurrencyGroupSeparatorProperty =
            DependencyProperty.Register("CurrencyGroupSeparator", typeof(string), typeof(CurrencyTextBox), new PropertyMetadata(string.Empty, OnCurrencyGroupSeparatorChanged));



        public bool GroupSeperatorEnabled
        {
            get { return (bool)GetValue(GroupSeperatorEnabledProperty); }
            set { SetValue(GroupSeperatorEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GroupSeperatorEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GroupSeperatorEnabledProperty =
            DependencyProperty.Register("GroupSeperatorEnabled", typeof(bool), typeof(CurrencyTextBox), new PropertyMetadata(true,new PropertyChangedCallback(OnCurrencyGroupSeparatorChanged)));

        

#if SILVERLIGHT

        public int[] CurrencyGroupSizes
        {
            get { return (int[])GetValue(CurrencyGroupSizesProperty); }
            set { SetValue(CurrencyGroupSizesProperty, value); }
        }

        public static readonly DependencyProperty CurrencyGroupSizesProperty =
            DependencyProperty.Register("CurrencyGroupSizes", typeof(int[]), typeof(CurrencyTextBox), new PropertyMetadata(null, OnCurrencyGroupSizesChanged));
#endif

#if WPF
        public Int32Collection CurrencyGroupSizes
        {
            get { return (Int32Collection)GetValue(CurrencyGroupSizesProperty); }
            set { SetValue(CurrencyGroupSizesProperty, value); }
        }

        public static readonly DependencyProperty CurrencyGroupSizesProperty =
            DependencyProperty.Register("CurrencyGroupSizes", typeof(Int32Collection), typeof(CurrencyTextBox), new UIPropertyMetadata(new Int32Collection(), OnCurrencyGroupSizesChanged));

#endif

        public int CurrencyNegativePattern
        {
            get { return (int)GetValue(CurrencyNegativePatternProperty); }
            set { SetValue(CurrencyNegativePatternProperty, value); }
        }

        public static readonly DependencyProperty CurrencyNegativePatternProperty =
            DependencyProperty.Register("CurrencyNegativePattern", typeof(int), typeof(CurrencyTextBox), new PropertyMetadata((int)-1, OnCurrencyNegativePatternChanged));

        public int CurrencyPositivePattern
        {
            get { return (int)GetValue(CurrencyPositivePatternProperty); }
            set { SetValue(CurrencyPositivePatternProperty, value); }
        }

        public static readonly DependencyProperty CurrencyPositivePatternProperty =
            DependencyProperty.Register("CurrencyPositivePattern", typeof(int), typeof(CurrencyTextBox), new PropertyMetadata(-1, OnCurrencyPositivePatternChanged));

        public string CurrencySymbol
        {
            get { return (string)GetValue(CurrencySymbolProperty); }
            set { SetValue(CurrencySymbolProperty, value); }
        }

        public static readonly DependencyProperty CurrencySymbolProperty =
            DependencyProperty.Register("CurrencySymbol", typeof(string), typeof(CurrencyTextBox), new PropertyMetadata(string.Empty, OnCurrencySymbolChanged));



        public double ScrollInterval
        {
            get { return (double)GetValue(ScrollIntervalProperty); }
            set { SetValue(ScrollIntervalProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScrollInterval.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScrollIntervalProperty =
            DependencyProperty.Register("ScrollInterval", typeof(double), typeof(CurrencyTextBox), new PropertyMetadata(1.0));



        

        #endregion

        #region PropertyChanged Callbacks

        public static void OnCurrencyDecimalDigitsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            CurrencyTextBox c = (CurrencyTextBox)obj;
            if (c != null)
                c.OnCurrencyDecimalDigitsChanged(args);
        }

        protected void OnCurrencyDecimalDigitsChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.CurrencyDecimalDigitsChanged != null)
            {
                this.CurrencyDecimalDigitsChanged(this, args);
            }
            if (mIsLoaded)
                this.FormatText();
        }


        public static void OnCurrencyDecimalSeparatorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            CurrencyTextBox c = (CurrencyTextBox)obj;
            if (c != null)
            {
                c.OnCurrencyDecimalSeparatorChanged(args);
            }
        }

        protected void OnCurrencyDecimalSeparatorChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.CurrencyDecimalSeparatorChanged != null)
                this.CurrencyDecimalSeparatorChanged(this, args);
            if (mIsLoaded)
                this.FormatText();
        }


        public static void OnCurrencyGroupSizesChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            CurrencyTextBox c = (CurrencyTextBox)obj;
            if (c != null)
            {
                c.OnCurrencyGroupSizesChanged(args);
            }
        }

        protected void OnCurrencyGroupSizesChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.CurrencyGroupSizesChanged != null)
            {
                this.CurrencyGroupSizesChanged(this, args);
            }
            if (mIsLoaded)
                this.FormatText();
        }


        public static void OnCurrencyGroupSeparatorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            CurrencyTextBox c = (CurrencyTextBox)obj;
            if (c != null)
                c.OnCurrencyGroupSeparatorChanged(args);
        }

        protected void OnCurrencyGroupSeparatorChanged(DependencyPropertyChangedEventArgs args)
        {
            if (CurrencyGroupSeparatorChanged != null)
            {
                this.CurrencyGroupSeparatorChanged(this, args);
            }
            if (mIsLoaded)
                this.FormatText();
        }


        public static void OnCurrencyNegativePatternChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            CurrencyTextBox c = (CurrencyTextBox)obj;
            if (c != null)
                c.OnCurrencyNegativePatternChanged(args);
        }

        protected void OnCurrencyNegativePatternChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.CurrencyNegativePatternChanged != null)
            {
                this.CurrencyNegativePatternChanged(this, args);
            }
            if (mIsLoaded)
                this.FormatText();
        }


        public static void OnCurrencyPositivePatternChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            CurrencyTextBox c = (CurrencyTextBox)obj;
            if (c != null)
            {
                c.OnCurrencyPositivePatternChanged(args);
            }
        }

        protected void OnCurrencyPositivePatternChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.CurrencyPositivePatternChanged != null)
            {
                this.CurrencyPositivePatternChanged(this, args);
            }
            if (mIsLoaded)
                this.FormatText();
        }


        public static void OnCurrencySymbolChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            CurrencyTextBox c = (CurrencyTextBox)obj;
            if (c != null)
            {
                c.OnCurrencySymbolChanged(args);
            }
        }

        protected void OnCurrencySymbolChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.CurrencySymbolChanged != null)
                this.CurrencySymbolChanged(this, args);
            if (mIsLoaded)
                this.FormatText();
        }

        public static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if ((CurrencyTextBox)obj != null)
                ((CurrencyTextBox)obj).OnValueChanged(args);
        }

        protected void OnValueChanged(DependencyPropertyChangedEventArgs args)
        {

#if SILVERLIGHT            

            object coerceValue = CoerceValue(this, this.Value);
            if (this.Value != (decimal?)coerceValue)
            {
                this.Value = (decimal?)coerceValue;
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

            OldValue = (decimal?)args.OldValue;
            mValue = this.Value;

            if (ValueChanged != null)
                ValueChanged(this, args);

            if (this.Value != null)
                this.WatermarkVisibility = System.Windows.Visibility.Collapsed;

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
            if ((CurrencyTextBox)obj != null)
            {
                ((CurrencyTextBox)obj).OnMinValueChanged(args);
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
                this.Value = this.ValidateValue(this.Value);
            }
        }

        public static void OnMaxValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if ((CurrencyTextBox)obj != null)
            {
                ((CurrencyTextBox)obj).OnMaxValueChanged(args);
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
        
        #endregion

        [Obsolete("Property will not help due to internal arhitecture changes")]
        public CurrencySymbolPosition CurrencySymbolPosition
        {
            get { return (CurrencySymbolPosition)GetValue(CurrencySymbolPositionProperty); }
            set { SetValue(CurrencySymbolPositionProperty, value); }
        }

        public static readonly DependencyProperty CurrencySymbolPositionProperty =
            DependencyProperty.Register("CurrencySymbolPosition ", typeof(CurrencySymbolPosition), typeof(CurrencyTextBox), new PropertyMetadata(CurrencySymbolPosition.Left,OnCurrencySymbolPositionChanged));

        public static void OnCurrencySymbolPositionChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            CurrencyTextBox c = (CurrencyTextBox)obj;
            if (c != null)
                c.OnCurrencySymbolPositionChanged(args);
        }

        protected void OnCurrencySymbolPositionChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.CurrencySymbolPositionChanged != null)
            {
                this.CurrencySymbolPositionChanged(this, args);
            }
            if (mIsLoaded)
                this.FormatText();
        }
        

#if SILVERLIGHT
        [TypeConverter(typeof(DecimalTypeConverter))]
#endif
        public decimal? NullValue
        {
            get { return (decimal?)GetValue(NullValueProperty); }
            set { SetValue(NullValueProperty, value); }
        }

        public static readonly DependencyProperty NullValueProperty =
            DependencyProperty.Register("NullValue", typeof(decimal?), typeof(CurrencyTextBox), new PropertyMetadata(null, OnNullValueChanged));

        public static void OnNullValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if ((CurrencyTextBox)obj != null)
                ((CurrencyTextBox)obj).OnNullValueChanged(args);
        }

        protected void OnNullValueChanged(DependencyPropertyChangedEventArgs args)
        {
        }

    }

    
}
