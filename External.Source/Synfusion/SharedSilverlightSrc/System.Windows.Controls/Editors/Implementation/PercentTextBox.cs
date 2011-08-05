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
      Type = typeof(PercentTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Office2007BlueStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2007Black,
    Type = typeof(PercentTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Office2007BlackStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2007Silver,
    Type = typeof(PercentTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Office2007SilverStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2010Blue,
    Type = typeof(PercentTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Office2010BlueStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2010Black,
    Type = typeof(PercentTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Office2010BlackStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2010Silver,
    Type = typeof(PercentTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Office2010SilverStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2003,
    Type = typeof(PercentTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Office2003Style.xaml")]
    [SkinType(SkinVisualStyle = Skin.Blend,
    Type = typeof(PercentTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/BlendStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.SyncOrange,
    Type = typeof(PercentTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/SyncOrangeStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.ShinyRed,
    Type = typeof(PercentTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/ShinyRedStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.ShinyBlue,
    Type = typeof(PercentTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/ShinyBlueStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Default,
    Type = typeof(PercentTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Generic.xaml")]
    [SkinType(SkinVisualStyle = Skin.VS2010,
 Type = typeof(CurrencyTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/VS2010Style.xaml")]
#endif
#if SILVERLIGHT
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Blend,
      Type = typeof(PercentTextBox), XamlResource = "/Syncfusion.Theming.Blend;component/Editors/PercentTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2007Blue,
        Type = typeof(PercentTextBox), XamlResource = "/Syncfusion.Theming.Office2007Blue;component/Editors/PercentTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2007Black,
        Type = typeof(PercentTextBox), XamlResource = "/Syncfusion.Theming.Office2007Black;component/Editors/PercentTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2007Silver,
        Type = typeof(PercentTextBox), XamlResource = "/Syncfusion.Theming.Office2007Silver;component/Editors/PercentTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Default,
        Type = typeof(PercentTextBox), XamlResource = "/Syncfusion.Theming.Default;component/Editors/PercentTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2010Blue,
        Type = typeof(PercentTextBox), XamlResource = "/Syncfusion.Theming.Office2010Blue;component/Editors/PercentTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2010Black,
        Type = typeof(PercentTextBox), XamlResource = "/Syncfusion.Theming.Office2010Black;component/Editors/PercentTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2010Silver,
        Type = typeof(PercentTextBox), XamlResource = "/Syncfusion.Theming.Office2010Silver;component/Editors/PercentTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Windows7,
        Type = typeof(PercentTextBox), XamlResource = "/Syncfusion.Theming.Windows7;component/Editors/PercentTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.VS2010,
      Type = typeof(PercentTextBox), XamlResource = "/Syncfusion.Theming.VS2010;component/Editors/PercentTextBox.xaml")] 
#endif
    public class PercentTextBox : EditorBase
    {

        #region Events
        /// <summary>
        /// Event that is raised when <see cref="PercentageSymbol"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback PercentageSymbolChanged;

        /// <summary>
        /// Event that is raised when <see cref="PercentEditMode"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback PercentEditModeChanged;

        /// <summary>
        /// Event that is raised when <see cref="PercentValue"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback PercentValueChanged;

        /// <summary>
        /// Event that is raised when <see cref="MinValue"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback MinValueChanged;

        /// <summary>
        /// Event that is raised when <see cref="MaxValue"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback MaxValueChanged;

        /// <summary>
        /// Event that is raised when <see cref="PercentDecimalDigits"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback PercentDecimalDigitsChanged;

        /// <summary>
        /// Event that is raised when <see cref="PercentDecimalSeparator"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback PercentDecimalSeparatorChanged;

        
        /// <summary>
        /// Event that is raised when <see cref="PercentGroupSeparator"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback PercentGroupSeparatorChanged;

        /// <summary>
        /// Event that is raised when <see cref="PercentGroupSizes"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback PercentGroupSizesChanged;



        
        #endregion

        #region Members

        internal double? OldValue;
        internal double? mValue;
        internal bool? mValueChanged = true;
        internal bool mIsLoaded = false;
        internal string checktext = "";
        #endregion

        #region Constructor

#if WPF
        static PercentTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PercentTextBox), new FrameworkPropertyMetadata(typeof(PercentTextBox)));
            //EnvironmentTest.ValidateLicense(typeof(PercentTextBox));
        }
#endif
        public ICommand pastecommand { get; private set; }
        public ICommand copycommand { get; private set; }
        public ICommand cutcommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PercentTextBox"/> class.
        /// </summary>
        public PercentTextBox()
        {
            pastecommand = new DelegateCommand(_pastecommand, Canpaste);
            copycommand = new DelegateCommand(_copycommmand, Canpaste);
            cutcommand = new DelegateCommand(_cutcommmand, Canpaste);
#if WPF
            this.AddHandler(CommandManager.PreviewExecutedEvent,
new ExecutedRoutedEventHandler(CommandExecuted), true);
#endif
#if WPF
            if (EnvironmentTest.IsSecurityGranted)
            {
                EnvironmentTest.StartValidateLicense(typeof(PercentTextBox));
            }
#endif
#if SILVERLIGHT
            this.DefaultStyleKey = typeof(PercentTextBox);
#endif
            this.Loaded += IntegerTextbox_Loaded;
            this.SelectionChanged += new RoutedEventHandler(PercentTextBox_SelectionChanged);
            
        }
        private void _pastecommand(object parameter)
        {
            Paste();
        }
        private void _copycommmand(object parameter)
        {
            copy();
        }
        private void _cutcommmand(object parameter)
        {
             cut();
        }
        private void copy()
        {
            Clipboard.SetText(this.SelectedText);
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
                string oldtext = this.Text;
                string copiedValue = string.Empty;
                string afterseperator = string.Empty;
                int seperatorindex = 0;
                double oldval = (double)this.PercentValue;
                double val1;
                NumberFormatInfo numberFormat = this.GetCulture().NumberFormat;
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
                            if (this.PercentEditMode == PercentEditMode.PercentMode)
                                this.Text = val3.ToString("P", numberFormat);
                            if (this.PercentEditMode == PercentEditMode.DoubleMode)
                                this.Text = (val3 / 100).ToString("P", numberFormat);
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
                    if (this.Text.Contains(numberFormat.PercentSymbol))
                    {
                        for (int x = 0; x < this.Text.Length; x++)
                        {
                            if (this.Text[x].ToString() == numberFormat.PercentSymbol)
                            {
                                this.Text = this.Text.Remove(x, 1);
                            }
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
                            this.CaretIndex = index1;
                            this.SetValue(false, val1);
                            if (this.PercentEditMode == PercentEditMode.PercentMode)
                                this.Text = val1.ToString("P", numberFormat);
                            if (this.PercentEditMode == PercentEditMode.DoubleMode)
                                this.Text = (val1 / 100).ToString("P", numberFormat);
                        }
                        else
                        {
                            this.SetValue(false, oldval);
                            double val3 = oldval;
                            if (this.PercentEditMode == PercentEditMode.PercentMode)
                                this.Text = val3.ToString("P", numberFormat);
                            if (this.PercentEditMode == PercentEditMode.DoubleMode)
                                this.Text = (val3 / 100).ToString("P", numberFormat);
                        }

                    }
                    if (val1 < this.MinValue && (this.MinValidation == MinValidation.OnKeyPress))
                    {
                        if (this.MinValueOnExceedMinDigit)
                        {
                            val1 = this.MinValue;
                            this.CaretIndex = index1;
                            this.SetValue(false, val1);
                            if (this.PercentEditMode == PercentEditMode.PercentMode)
                                this.Text = val1.ToString("P", numberFormat);
                            if (this.PercentEditMode == PercentEditMode.DoubleMode)
                                this.Text = (val1 / 100).ToString("P", numberFormat);
                        }
                        else
                        {
                            this.SetValue(false, oldval);
                            double val3 = oldval;
                            if (this.PercentEditMode == PercentEditMode.PercentMode)
                                this.Text = val3.ToString("P", numberFormat);
                            if (this.PercentEditMode == PercentEditMode.DoubleMode)
                                this.Text = (val3 / 100).ToString("P", numberFormat);
                        }
                    }
                }
                else
                {
                    this.SetValue(false, val1);
                    this.CaretIndex = index1 + copiedValue.Length;
                    if (this.PercentEditMode == PercentEditMode.PercentMode)
                        this.Text = val1.ToString("P", numberFormat);
                    if (this.PercentEditMode == PercentEditMode.DoubleMode)
                        this.Text = (val1 / 100).ToString("P", numberFormat);
                }
            }
        }
       
        private bool Canpaste(object parameter)
        {
            return true;
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
        void PercentTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            
        }
        #endregion

       
        #region overide

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes (such as a rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate"/>. In simplest terms, this means the method is called just before a UI element displays in an application. For more information, see Remarks.
        /// </summary>
        ScrollViewer PART_ContentHost;
        public override void OnApplyTemplate()
        {
#if WPF
            PART_ContentHost = this.GetTemplateChild("PART_ContentHost") as ScrollViewer;
#endif
#if SILVERLIGHT
            PART_ContentHost = this.GetTemplateChild("ContentElement") as ScrollViewer;
#endif
            base.OnApplyTemplate();
        }

        /// <summary>
        /// Called before the <see cref="E:System.Windows.UIElement.MouseWheel"/> event occurs to provide handling for the event in a derived class without attaching a delegate.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Input.MouseWheelEventArgs"/> that contains the event data.</param>
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            if (e.Delta > 0)
            {
                PercentValueHandler.percentValueHandler.HandleUpKey(this);
            }
            else if (e.Delta < 0)
            {
                PercentValueHandler.percentValueHandler.HandleDownKey(this);
            }
        }
        
#if SILVERLIGHT
        /// <summary>
        /// Raises the <see cref="E:KeyDown"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
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
                e.Handled = PercentValueHandler.percentValueHandler.HandleKeyDown(this, e);
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
                PercentValueHandler.percentValueHandler.HandleDeleteKey(this);
            }
        }

        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            e.Handled = PercentValueHandler.percentValueHandler.MatchWithMask(this, e.Text);
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

        /// <summary>
        /// Raises the <see cref="E:LostFocus"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            if (this.EnableFocusColors && this.PART_ContentHost != null)
                this.PART_ContentHost.Background = this.Background;

            double? Val = this.PercentValue;
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
                if (Val != this.PercentValue)
                {
                    this.PercentValue = Val;
                }
            }
            base.OnLostFocus(e);
            this.checktext = "";
        }

        /// <summary>
        /// Raises the <see cref="E:GotFocus"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
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
		    if (this.PercentValue != null && !(double.IsNaN((double)this.PercentValue)))
            {
                NumberFormatInfo numberFormat = this.GetCulture().NumberFormat;
                if (this.PercentEditMode == PercentEditMode.DoubleMode)
                    this.MaskedText = (((double)mValue) / 100).ToString("P", numberFormat);
                else
                {
                    //if (percentModeFlag == true)
                        this.MaskedText = ((double)mValue).ToString("P", numberFormat);
                    //else
                      //  this.MaskedText = (((double)mValue) / 100).ToString("P", numberFormat);
                }
            }
            else
            {
                this.MaskedText = "";
            }
        }

        //internal bool percentModeFlag = true;

        internal void SetValue(bool? IsReload, double? _Value)
        {
            if (IsReload == false)
            {
                mValueChanged = false;
                this.PercentValue = _Value;
                mValueChanged = true;
            }
            else if (IsReload == true)
            {
                //percentModeFlag = false;
                this.PercentValue = _Value;
                //percentModeFlag = true;
            }
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

            //PercentDecimalDigits
            //PercentDecimalSeparator
            //PercentGroupSeparator
            //PercentGroupSizes
            //PercentNegativePattern
            //PercentPositivePattern
            //PercentSymbol

            if (PercentDecimalDigits >= 0)
                cultureInfo.NumberFormat.PercentDecimalDigits = this.PercentDecimalDigits;

            if (!PercentDecimalSeparator.Equals(string.Empty))
                cultureInfo.NumberFormat.PercentDecimalSeparator = this.PercentDecimalSeparator;

            if (!GroupSeperatorEnabled)
            {
                cultureInfo.NumberFormat.PercentGroupSeparator = string.Empty;
            }
            if (GroupSeperatorEnabled == true)
            {
                if (!PercentGroupSeparator.Equals(string.Empty))
                    cultureInfo.NumberFormat.PercentGroupSeparator = this.PercentGroupSeparator;
            }

#if SILVERLIGHT
            if (PercentGroupSizes != null)
                cultureInfo.NumberFormat.PercentGroupSizes = this.PercentGroupSizes;
#endif

#if WPF
            int count = this.PercentGroupSizes.Count;
            if (count > 0)
            {
                int[] ngs = new int[count];

                for (int i = 0; i < count; i++)
                {
                    ngs[i] = this.PercentGroupSizes[i];
                }
                cultureInfo.NumberFormat.PercentGroupSizes = ngs;
            }
#endif

            if (PercentNegativePattern >= 0)
                cultureInfo.NumberFormat.PercentNegativePattern = this.PercentNegativePattern;

            if (PercentPositivePattern >= 0)
                cultureInfo.NumberFormat.PercentPositivePattern = this.PercentPositivePattern;

            if (!PercentageSymbol.Equals(string.Empty))
                cultureInfo.NumberFormat.PercentSymbol = this.PercentageSymbol;

            cultureInfo.NumberFormat.PercentDecimalSeparator = cultureInfo.NumberFormat.PercentDecimalSeparator[0].ToString();
            if (GroupSeperatorEnabled == true)
            {
                cultureInfo.NumberFormat.PercentGroupSeparator = cultureInfo.NumberFormat.PercentGroupSeparator[0].ToString();
            }
            cultureInfo.NumberFormat.PercentSymbol = cultureInfo.NumberFormat.PercentSymbol[0].ToString();

            return cultureInfo;
        }

        private static object CoerceValue(DependencyObject d, object baseValue)
        {
            PercentTextBox percentTextBox = (PercentTextBox)d;
            if (baseValue != null)
            {
                double? value = (double?)baseValue;
                if (percentTextBox.mValueChanged == true)
                {
                    if (value > percentTextBox.MaxValue)
                    {
                        value = percentTextBox.MaxValue;
                    }
                    else if (value < percentTextBox.MinValue)
                    {
                        value = percentTextBox.MinValue;
                    }
                }
                if (value != null)
                {
                    percentTextBox.IsNegative = value < 0 ? true : false;
                    percentTextBox.IsZero = value == 0 ? true : false;
                    percentTextBox.IsNull = false;
                }
                return value;
            }
            else
            {
                if (percentTextBox.UseNullOption)
                {
                    percentTextBox.IsNull = true;
                    percentTextBox.IsNegative = false;
                    percentTextBox.IsZero = false;
                    return percentTextBox.NullValue;
                    //return baseValue;
                }
                else
                {
                    double value = 0L;
                    if (percentTextBox.mValueChanged == true)
                    {
                        if (value > percentTextBox.MaxValue)
                        {
                            value = percentTextBox.MaxValue;
                        }
                        if (value < percentTextBox.MinValue)
                        {
                            value = percentTextBox.MinValue;
                        }
                    }
                    percentTextBox.IsNegative = value < 0 ? true : false;
                    percentTextBox.IsZero = value == 0 ? true : false;
                    percentTextBox.IsNull = false;
                    return value;
                }
            }
        }

        private static object CoerceMinValue(DependencyObject d, object baseValue)
        {
            PercentTextBox percentTextBox = (PercentTextBox)d;
            if (percentTextBox.MinValue > percentTextBox.MaxValue)
            {
                return percentTextBox.MaxValue;
            }
            return baseValue;
        }

        private static object CoerceMaxValue(DependencyObject d, object baseValue)
        {
            PercentTextBox percentTextBox = (PercentTextBox)d;
            if (percentTextBox.MinValue > percentTextBox.MaxValue)
            {
                return percentTextBox.MinValue;
            }
            return baseValue;
        }

        #endregion


        /// <summary>
        /// Handles the Loaded event of the IntegerTextbox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        void IntegerTextbox_Loaded(object sender, RoutedEventArgs e)
        {
            mIsLoaded = true;
            object tempObj = CoerceValue(this, PercentValue);
            double? tempVal = (double?)tempObj;
            if (this.IsNull == true)
            {
                this.WatermarkVisibility = Visibility.Visible;
            }
            if (tempVal != PercentValue)
            {
                PercentValue = tempVal;
            }
            else
            {
                FormatText();
            }
        }

        #region Properties

       
        /// The boolean to enable or disable to validation on lost focus.  
        /// </value>
        public bool ValidationOnLostFocus
        {
            get
            {
                return (bool)GetValue(ValidationOnLostFocusProperty);
            }

            set
            {
                SetValue(ValidationOnLostFocusProperty, value);
            }
        }
        
        /// <summary>
        /// Event that is raised when <see cref="ValidationOnLostFocus"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback ValidationOnLostFocusChanged;

#if WPF
        /// <summary>
        /// Identifies the <see cref="ValidationOnLostFocus"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValidationOnLostFocusProperty =
            DependencyProperty.Register("ValidationOnLostFocus", typeof(bool), typeof(PercentTextBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault ,new PropertyChangedCallback(OnValidationOnLostFocusChanged)));
#endif

#if SILVERLIGHT
        /// <summary>
        /// Identifies the <see cref="ValidationOnLostFocus"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValidationOnLostFocusProperty =
           DependencyProperty.Register("ValidationOnLostFocus", typeof(bool), typeof(PercentTextBox), new PropertyMetadata(false, new PropertyChangedCallback(OnValidationOnLostFocusChanged)));

#endif
        public PercentEditMode PercentEditMode
        {
            get { return (PercentEditMode)GetValue(PercentEditModeProperty); }
            set { SetValue(PercentEditModeProperty, value); }
        }



        public bool GroupSeperatorEnabled
        {
            get { return (bool)GetValue(GroupSeperatorEnabledProperty); }
            set { SetValue(GroupSeperatorEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GroupSeperatorEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GroupSeperatorEnabledProperty =
            DependencyProperty.Register("GroupSeperatorEnabled", typeof(bool), typeof(PercentTextBox), new PropertyMetadata(true, new PropertyChangedCallback(OnPercentGroupSeparatorChanged)));

        
        public static readonly DependencyProperty PercentEditModeProperty =
            DependencyProperty.Register("PercentEditMode", typeof(PercentEditMode), typeof(PercentTextBox), new PropertyMetadata(PercentEditMode.DoubleMode, OnPercentEditModeChanged));


        public double? PercentValue
        {
            get { return (double?)GetValue(PercentValueProperty); }
            set
            {
                //object coerceValue = CoerceValue(this, value);
                SetValue(PercentValueProperty, value);
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

        //PercentDecimalDigits
        //PercentDecimalSeparator
        //PercentGroupSeparator
        //PercentGroupSizes
        //PercentNegativePattern
        //PercentPositivePattern
        //PercentSymbol

#if WPF
        // In WPF Percent Value property acts with Two Binding by default.
        public static readonly DependencyProperty PercentValueProperty =
      DependencyProperty.Register("PercentValue", typeof(double?), typeof(PercentTextBox), new FrameworkPropertyMetadata(null,FrameworkPropertyMetadataOptions.BindsTwoWayByDefault ,new PropertyChangedCallback(OnValueChanged)));
#endif

#if SILVERLIGHT
        public static readonly DependencyProperty PercentValueProperty =
     DependencyProperty.Register("PercentValue", typeof(double?), typeof(PercentTextBox), new PropertyMetadata(null,new PropertyChangedCallback(OnValueChanged)));
#endif
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(double), typeof(PercentTextBox), new PropertyMetadata(double.MinValue, new PropertyChangedCallback(OnMinValueChanged)));

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(PercentTextBox), new PropertyMetadata(double.MaxValue, new PropertyChangedCallback(OnMaxValueChanged)));


        public int PercentDecimalDigits
        {
            get { return (int)GetValue(PercentDecimalDigitsProperty); }
            set { SetValue(PercentDecimalDigitsProperty, value); }
        }

        public static readonly DependencyProperty PercentDecimalDigitsProperty =
            DependencyProperty.Register("PercentDecimalDigits", typeof(int), typeof(PercentTextBox), new PropertyMetadata((int)-1, OnPercentDecimalDigitsChanged));

        public string PercentDecimalSeparator
        {
            get { return (string)GetValue(PercentDecimalSeparatorProperty); }
            set { SetValue(PercentDecimalSeparatorProperty, value); }
        }

        public static readonly DependencyProperty PercentDecimalSeparatorProperty =
            DependencyProperty.Register("PercentDecimalSeparator", typeof(string), typeof(PercentTextBox), new PropertyMetadata(string.Empty, OnPercentDecimalSeparatorChanged));

        public string PercentGroupSeparator
        {
            get { return (string)GetValue(PercentGroupSeparatorProperty); }
            set { SetValue(PercentGroupSeparatorProperty, value); }
        }

        public static readonly DependencyProperty PercentGroupSeparatorProperty =
            DependencyProperty.Register("PercentGroupSeparator", typeof(string), typeof(PercentTextBox), new PropertyMetadata(string.Empty, OnPercentGroupSeparatorChanged));



        public double ScrollInterval
        {
            get { return (double)GetValue(ScrollIntervalProperty); }
            set { SetValue(ScrollIntervalProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScrollInterval.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScrollIntervalProperty =
            DependencyProperty.Register("ScrollInterval", typeof(double), typeof(PercentTextBox), new PropertyMetadata(1.0));



#if SILVERLIGHT
        public int[] PercentGroupSizes
        {
            get { return (int[])GetValue(PercentGroupSizesProperty); }
            set { SetValue(PercentGroupSizesProperty, value); }
        }

        public static readonly DependencyProperty PercentGroupSizesProperty =
            DependencyProperty.Register("PercentGroupSizes", typeof(int[]), typeof(PercentTextBox), new PropertyMetadata(null, OnPercentGroupSizesChanged));
#endif

#if WPF
        public Int32Collection PercentGroupSizes
        {
            get { return (Int32Collection)GetValue(PercentGroupSizesProperty); }
            set { SetValue(PercentGroupSizesProperty, value); }
        }

        public static readonly DependencyProperty PercentGroupSizesProperty =
            DependencyProperty.Register("PercentGroupSizes", typeof(Int32Collection), typeof(PercentTextBox), new PropertyMetadata(new Int32Collection(), OnPercentGroupSizesChanged));
#endif
        

        public int PercentNegativePattern
        {
            get { return (int)GetValue(PercentNegativePatternProperty); }
            set { SetValue(PercentNegativePatternProperty, value); }
        }

        public static readonly DependencyProperty PercentNegativePatternProperty =
            DependencyProperty.Register("PercentNegativePattern", typeof(int), typeof(PercentTextBox), new PropertyMetadata((int)-1, OnPercentNegativePatternChanged));

        public static void OnPercentNegativePatternChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            PercentTextBox p = (PercentTextBox)obj;
            if (p != null)
            {
                p.OnPercentNegativePatternChanged(args);
            }
        }

        protected void OnPercentNegativePatternChanged(DependencyPropertyChangedEventArgs args)
        {
            
        }

        public int PercentPositivePattern
        {
            get { return (int)GetValue(PercentPositivePatternProperty); }
            set { SetValue(PercentPositivePatternProperty, value); }
        }

        public static readonly DependencyProperty PercentPositivePatternProperty =
            DependencyProperty.Register("PercentPositivePattern", typeof(int), typeof(PercentTextBox), new PropertyMetadata(-1, OnPercentPositivePatternChanged));

        public static void OnPercentPositivePatternChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            PercentTextBox p = (PercentTextBox)obj;
            if (p != null)
            {
                p.OnPercentPositivePatternChanged(args);
            }
        }

        protected void OnPercentPositivePatternChanged(DependencyPropertyChangedEventArgs args)
        {
        }

        public string PercentageSymbol
        {
            get { return (string)GetValue(PercentageSymbolProperty); }
            set { SetValue(PercentageSymbolProperty, value); }
        }

        public static readonly DependencyProperty PercentageSymbolProperty =
            DependencyProperty.Register("PercentageSymbol", typeof(string), typeof(PercentTextBox), new PropertyMetadata(string.Empty, OnPercentageSymbolChanged));

        #endregion

        #region PropertyChanged Callbacks

        public static void OnPercentDecimalDigitsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            PercentTextBox p = (PercentTextBox)obj;
            if (p != null)
                p.OnPercentDecimalDigitsChanged(args);
        }

        protected void OnPercentDecimalDigitsChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.PercentDecimalDigitsChanged != null)
            {
                this.PercentDecimalDigitsChanged(this, args);
            }
            if (mIsLoaded)
            {
                this.FormatText();
            }
        }

         /// <summary>
        /// Calls OnValidationOnLostFocusChanged method of the instance, notifies of the dependency property value changes.
        /// </summary>
        /// <param name="d">Dependency object, the change occurs on.</param>
        /// <param name="e">
        /// Property change details, such as old value and new value.</param>
        private static void OnValidationOnLostFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PercentTextBox instance = (PercentTextBox)d;
            instance.OnValidationOnLostFocusChanged(e);
        }

        /// <summary>
        /// Updates property value cache and raises <see cref="OnValidationOnLostFocusChanged"/> event.
        /// </summary>
        /// <param name="e">
        /// Property change details, such as old value and new value.</param>
        protected virtual void OnValidationOnLostFocusChanged(DependencyPropertyChangedEventArgs e)
        {
            if (ValidationOnLostFocusChanged != null)
            {
                ValidationOnLostFocusChanged(this, e);
            }
        }

        public static void OnPercentDecimalSeparatorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            PercentTextBox p = (PercentTextBox)obj;
            if (p != null)
            {
                p.OnPercentDecimalSeparatorChanged(args);
            }
        }

        protected void OnPercentDecimalSeparatorChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.PercentDecimalSeparatorChanged != null)
            {
                this.PercentDecimalSeparatorChanged(this, args);
            }
            if (mIsLoaded)
            {
                this.FormatText();
            }
        }


        public static void OnPercentGroupSeparatorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            PercentTextBox p = (PercentTextBox)obj;
            if (p != null)
            {
                p.OnPercentGroupSeparatorChanged(args);
            }
        }

        protected void OnPercentGroupSeparatorChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.PercentDecimalSeparatorChanged != null)
            {
                this.PercentDecimalSeparatorChanged(this, args);
            }
            if (mIsLoaded)
            {
                this.FormatText();
            }
        }

        public static void OnPercentGroupSizesChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            PercentTextBox p = (PercentTextBox)obj;
            if (p != null)
            {
                p.OnPercentGroupSizesChanged(args);
            }
        }

        protected void OnPercentGroupSizesChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.PercentGroupSizesChanged != null)
            {
                this.PercentGroupSizesChanged(this, args);
            }
            if (mIsLoaded)
            {
                FormatText();
            }
        }

        public static void OnPercentageSymbolChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            PercentTextBox p = (PercentTextBox)obj;
            if (p != null)
            {
                p.OnPercentageSymbolChanged(args);
            }
        }

        protected void OnPercentageSymbolChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.PercentageSymbolChanged != null)
            {
                this.PercentageSymbolChanged(this, args);
            }
            if (mIsLoaded)
            {
                FormatText();
            }
        }

        public static void OnPercentEditModeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            PercentTextBox p = (PercentTextBox)obj;
            if (p != null)
            {
                p.OnPercentEditModeChanged(args);
            }
        }

        protected void OnPercentEditModeChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.PercentEditModeChanged != null)
            {
                this.PercentEditModeChanged(this, args);
            }
            if (mIsLoaded)
            {
                this.FormatText();
            }
        }

        public static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if ((PercentTextBox)obj != null)
                ((PercentTextBox)obj).OnValueChanged(args);
        }

        protected void OnValueChanged(DependencyPropertyChangedEventArgs args)
        {

#if SILVERLIGHT
            object coerceValue = CoerceValue(this, this.PercentValue);
            if (this.PercentValue != (double?)coerceValue)
            {
                this.PercentValue = (double?)coerceValue;
                return;
            }
#endif
            if (this.PercentValue != null && !(double.IsNaN((double)this.PercentValue)))
            {
                this.IsNegative = this.PercentValue < 0 ? true : false;
                this.IsZero = this.PercentValue == 0 ? true : false;
                this.IsNull = false;
                
            }
            else
            {
                this.IsNull = true;
                this.IsNegative = false;
                this.IsZero = false;
            }

            //if (ValidationOnLostFocus == true)
            //{
            //    MaxValidation = MaxValidation.OnLostFocus;
            //    MinValidation = MinValidation.OnLostFocus;
            //}
            //else
            //{
            //    if (MaxValueOnExceedMaxDigit)
            //    {
            //        MaxValidation = MaxValidation.OnKeyPress;
            //    }
            //    else
            //    {
            //        MinValidation = MinValidation.OnKeyPress;
            //    }
            //}
            OldValue = (double?)args.OldValue;
            mValue = this.PercentValue;

            if (this.PercentValue != null)
                this.WatermarkVisibility = System.Windows.Visibility.Collapsed;

            if (PercentValueChanged != null)
                PercentValueChanged(this, args);
            if (this.PercentValue > this.MinValue && this.MinValidation == MinValidation.OnKeyPress)
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
            if ((PercentTextBox)obj != null)
            {
                ((PercentTextBox)obj).OnMinValueChanged(args);
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

            if (ValidationOnLostFocus == false)
            {
                if (this.PercentValue != this.ValidateValue(this.PercentValue))
                {
                    this.PercentValue = this.ValidateValue(this.PercentValue);
                }
            }
          
        }

        public static void OnMaxValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if ((PercentTextBox)obj != null)
            {
                ((PercentTextBox)obj).OnMaxValueChanged(args);
            }
        }

        protected void OnMaxValueChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.MaxValueChanged != null)
            {
                this.MaxValueChanged(this, args);
            }

            if (this.MinValue > this.MaxValue)
                this.MinValue = this.MaxValue;

            if (ValidationOnLostFocus == false)
            {
                if (this.PercentValue != this.ValidateValue(this.PercentValue))
                {
                    this.PercentValue = this.ValidateValue(this.PercentValue);
                }
            }
        }


        #endregion


        public double? NullValue
        {
            get { return (double?)GetValue(NullValueProperty); }
            set { SetValue(NullValueProperty, value); }
        }

        public static readonly DependencyProperty NullValueProperty =
            DependencyProperty.Register("NullValue", typeof(double?), typeof(PercentTextBox), new PropertyMetadata(null, OnNullValueChanged));

        public static void OnNullValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if ((PercentTextBox)obj != null)
                ((PercentTextBox)obj).OnNullValueChanged(args);
        }

        protected void OnNullValueChanged(DependencyPropertyChangedEventArgs args)
        {
        }
    }
}
