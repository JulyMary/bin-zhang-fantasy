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
using System.Collections.ObjectModel;
using Syncfusion.Windows.Shared;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

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
      Type = typeof(MaskedTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Office2007BlueStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2007Black,
    Type = typeof(MaskedTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Office2007BlackStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2007Silver,
    Type = typeof(MaskedTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Office2007SilverStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2010Blue,
     Type = typeof(MaskedTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Office2010BlueStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2010Black,
    Type = typeof(MaskedTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Office2010BlackStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2010Silver,
    Type = typeof(MaskedTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Office2010SilverStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2003,
    Type = typeof(MaskedTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Office2003Style.xaml")]
    [SkinType(SkinVisualStyle = Skin.Blend,
    Type = typeof(MaskedTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/BlendStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.SyncOrange,
    Type = typeof(MaskedTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/SyncOrangeStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.ShinyRed,
    Type = typeof(MaskedTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/ShinyRedStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.ShinyBlue,
    Type = typeof(MaskedTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/ShinyBlueStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Default,
    Type = typeof(MaskedTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Generic.xaml")]
    [SkinType(SkinVisualStyle = Skin.VS2010,
 Type = typeof(MaskedTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/VS2010Style.xaml")]
#endif
#if SILVERLIGHT
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Blend,
      Type = typeof(MaskedTextBox), XamlResource = "/Syncfusion.Theming.Blend;component/Editors/MaskedTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2007Blue,
        Type = typeof(MaskedTextBox), XamlResource = "/Syncfusion.Theming.Office2007Blue;component/Editors/MaskedTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2007Black,
        Type = typeof(MaskedTextBox), XamlResource = "/Syncfusion.Theming.Office2007Black;component/Editors/MaskedTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2007Silver,
        Type = typeof(MaskedTextBox), XamlResource = "/Syncfusion.Theming.Office2007Silver;component/Editors/MaskedTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Default,
        Type = typeof(MaskedTextBox), XamlResource = "/Syncfusion.Theming.Default;component/Editors/MaskedTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2010Blue,
        Type = typeof(MaskedTextBox), XamlResource = "/Syncfusion.Theming.Office2010Blue;component/Editors/MaskedTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2010Black,
        Type = typeof(MaskedTextBox), XamlResource = "/Syncfusion.Theming.Office2010Black;component/Editors/MaskedTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2010Silver,
        Type = typeof(MaskedTextBox), XamlResource = "/Syncfusion.Theming.Office2010Silver;component/Editors/MaskedTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2010Silver,
        Type = typeof(MaskedTextBox), XamlResource = "/Syncfusion.Theming.Office2010Silver;component/Editors/MaskedTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Windows7,
        Type = typeof(MaskedTextBox), XamlResource = "/Syncfusion.Theming.Windows7;component/Editors/MaskedTextBox.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.VS2010,
        Type = typeof(MaskedTextBox), XamlResource = "/Syncfusion.Theming.VS2010;component/Editors/MaskedTextBox.xaml")]  
#endif
    public class MaskedTextBox : TextBox
    {

        #region Events

        /// <summary>
        /// Event than is raised after the <see cref="Mask"/> property has changed.
        /// </summary>
        public event PropertyChangedCallback MaskChanged;
        
        /// <summary>
        /// Event than is raised after the <see cref="ValidationString"/> property has changed.
        /// </summary>
        public event PropertyChangedCallback ValidationStringChanged;

      
        public event PropertyChangedCallback MaskCompletedChanged;
        /// <summary>
        /// Event than is raised after the <see cref="InvalidValueBehavior"/> property has changed.
        /// </summary>
        [Obsolete("Event will not help due to internal arhitecture changes")]
        public event PropertyChangedCallback InvalidValueBehaviorChanged;
        
        /// <summary>
        /// Event than is raised after the <see cref="DateSeparator"/> property has changed.
        /// </summary>
        public event PropertyChangedCallback DateSeparatorChanged;

        /// <summary>
        /// Event than is raised after the <see cref="TimeSeparator"/> property has changed.
        /// </summary>
        public event PropertyChangedCallback TimeSeparatorChanged;

        /// <summary>
        /// Event than is raised after the <see cref="DecimalSeparator"/> property has changed.
        /// </summary>
        public event PropertyChangedCallback DecimalSeparatorChanged;

        /// <summary>
        /// Event than is raised after the <see cref="NumberGroupSeparator"/> property has changed.
        /// </summary>
        public event PropertyChangedCallback NumberGroupSeparatorChanged;

        /// <summary>
        /// Event than is raised after the <see cref="CurrencySymbol"/> property has changed.
        /// </summary>
        public event PropertyChangedCallback CurrencySymbolChanged;

        /// <summary>
        /// Event than is raised after the <see cref="PromptChar"/> property has changed.
        /// </summary>
        public event PropertyChangedCallback PromptCharChanged;

        /// <summary>
        /// Event that is raised when <see cref="WatermarkTextMode"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback WatermarkTextModeChanged;

        /// <summary>
        /// Event that is raised when <see cref="WatermarkText"/> property is changed.
        /// </summary>
        //public event PropertyChangedCallback WatermarkTextChanged;

        /// <summary>
        /// Event that is raised when <see cref="WatermarkTextIsVisible"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback WatermarkTextIsVisibleChanged;

        /// <summary>
        /// Occurs when the control is validating. 
        /// </summary>
        public event CancelEventHandler Validating;
        /// <summary>
        /// Occurs when the control is finished validating. 
        /// </summary>
        public event EventHandler Validated;

        /// <summary>
        /// Occurs when MaskedTextBox has finished validating the current value using the ValidatingString property.
        /// NOTE: event occurs only when ValidatingString is not empty.
        /// </summary>
        public event StringValidationCompletedEventHandler StringValidationCompleted;


       

        /// <summary>
        /// Occurs when [text selection on focus changed].
        /// </summary>
        public event PropertyChangedCallback TextSelectionOnFocusChanged;


       

        /// <summary>
        /// Event that is raised when <see cref="EnterToMoveNext"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback EnterToMoveNextChanged;
        #endregion


        public event PropertyChangedCallback MinLengthChanged;
        public event PropertyChangedCallback WatermarkTemplateChanged;
        public event PropertyChangedCallback WatermarkTextChanged;
        public event PropertyChangedCallback ValueChanged;
        internal string mValue;
        internal bool mIsLoaded = false;
        internal bool mValueChanged = true;
        internal string oldValue;

        internal static EditorBase editorBase = new EditorBase();

        #region WaterMark
        /// <summary>
        /// Gets or sets the water mark template.
        /// </summary>
        /// <value>The water mark template.</value>
        public DataTemplate WatermarkTemplate
        {
            get { return (DataTemplate)GetValue(WatermarkTemplateProperty); }
            set { SetValue(WatermarkTemplateProperty, value); }
        }

        /// <summary>
        /// Identifies the WaterMarkTemplate dependency property.
        /// </summary>
        public static DependencyProperty WatermarkTemplateProperty =
            DependencyProperty.Register("WatermarkTemplate", typeof(DataTemplate), typeof(MaskedTextBox), new PropertyMetadata(OnWaterMarkTemplateChanged));

        /// <summary>
        /// Gets or sets the water mark text.
        /// </summary>
        /// <value>The water mark text.</value>
        public string WatermarkText
        {
            set { SetValue(WatermarkTextProperty, value); }
            get { return (string)GetValue(WatermarkTextProperty); }
        }



        public int MinLength
        {
            get { return (int)GetValue(MinLengthProperty); }
            set { SetValue(MinLengthProperty, value); }
        }

        // Identifies MinLength dependency property

        public static readonly DependencyProperty MinLengthProperty =
            DependencyProperty.Register("MinLength", typeof(int), typeof(MaskedTextBox), new PropertyMetadata(0,new PropertyChangedCallback(OnMinLengthChanged)));

        /// <summary>
        /// Identifies the WaterMarkText dependency property.
        /// </summary>
        public static DependencyProperty WatermarkTextProperty =
            DependencyProperty.Register("WatermarkText", typeof(string), typeof(MaskedTextBox), new PropertyMetadata("Type here...", new PropertyChangedCallback(OnWaterMarkTextChanged)));

        public static void OnWaterMarkTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (((MaskedTextBox)obj) != null)
            {
                ((MaskedTextBox)obj).OnWaterMarkTextChanged(args);
            }
        }

        protected void OnWaterMarkTextChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.WatermarkTextChanged != null)
                this.WatermarkTextChanged(this, args);
        }

        /// <summary>
        /// Gets or sets the water mark visibility.
        /// </summary>
        /// <value>The water mark visibility.</value>
        public Visibility WatermarkVisibility
        {
            set
            {
                value = CoerceWatermarkVisibility(this, value);
                SetValue(WatermarkVisibilityProperty, value);
            }
            get { return (Visibility)GetValue(WatermarkVisibilityProperty); }
        }

        private static Visibility CoerceWatermarkVisibility(DependencyObject d, object baseValue)
        {
            MaskedTextBox maskedTextBox = (MaskedTextBox)d;

            if (maskedTextBox.WatermarkTextIsVisible && (((Visibility)baseValue) == Visibility.Visible))
            {
                maskedTextBox.ContentElementVisibility = Visibility.Collapsed;
                return Visibility.Visible;
            }
            else
            {
                maskedTextBox.ContentElementVisibility = Visibility.Visible;
                return Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Identifies the WaterMarkVisibility dependency property.
        /// </summary>
        public static DependencyProperty WatermarkVisibilityProperty =
            DependencyProperty.Register("WatermarkVisibility", typeof(Visibility), typeof(MaskedTextBox), new PropertyMetadata(Visibility.Visible));

        [EditorBrowsable(EditorBrowsableState.Never)]
        [BrowsableAttribute(false)]
        public Visibility ContentElementVisibility
        {
            get { return (Visibility)GetValue(ContentElementVisibilityProperty); }
            set { SetValue(ContentElementVisibilityProperty, value); }
        }

        public static readonly DependencyProperty ContentElementVisibilityProperty =
            DependencyProperty.Register("ContentElementVisibility", typeof(Visibility), typeof(MaskedTextBox), new PropertyMetadata(Visibility.Visible));


        public Brush WatermarkTextForeground
        {
            get { return (Brush)GetValue(WatermarkTextForegroundProperty); }
            set { SetValue(WatermarkTextForegroundProperty, value); }
        }

        public static readonly DependencyProperty WatermarkTextForegroundProperty =
            DependencyProperty.Register("WatermarkTextForeground", typeof(Brush), typeof(MaskedTextBox), new PropertyMetadata(new SolidColorBrush()));

        public Brush WatermarkBackground
        {
            get { return (Brush)GetValue(WatermarkBackgroundProperty); }
            set { SetValue(WatermarkBackgroundProperty, value); }
        }

        public static readonly DependencyProperty WatermarkBackgroundProperty =
            DependencyProperty.Register("WatermarkBackground", typeof(Brush), typeof(MaskedTextBox), new PropertyMetadata(new SolidColorBrush()));

        public double WatermarkOpacity
        {
            get { return (double)GetValue(WatermarkOpacityProperty); }
            set { SetValue(WatermarkOpacityProperty, value); }
        }

        public static readonly DependencyProperty WatermarkOpacityProperty =
            DependencyProperty.Register("WatermarkOpacity", typeof(double), typeof(MaskedTextBox), new PropertyMetadata((double)0.5));

        public bool WatermarkTextIsVisible
        {
            get { return (bool)GetValue(WatermarkTextIsVisibleProperty); }
            set { SetValue(WatermarkTextIsVisibleProperty, value); }
        }

        public static readonly DependencyProperty WatermarkTextIsVisibleProperty =
            DependencyProperty.Register("WatermarkTextIsVisible", typeof(bool), typeof(MaskedTextBox), new PropertyMetadata(true, OnWatermarkTextIsVisibleChanged));


        #region Property Changed Callback

        public static void OnWatermarkTextIsVisibleChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            MaskedTextBox m = (MaskedTextBox)obj;
            if (m != null)
            {
                m.OnWatermarkTextIsVisibleChanged(args);
            }
        }

        protected void OnWatermarkTextIsVisibleChanged(DependencyPropertyChangedEventArgs args)
        {
            if (!this.WatermarkTextIsVisible)
            {
                this.WatermarkVisibility = Visibility.Collapsed;
                this.ContentElementVisibility = Visibility.Visible;
            }
            else
            {
                this.ContentElementVisibility = Visibility.Collapsed;
                this.WatermarkVisibility = Visibility.Visible;
            }
            if (this.WatermarkTextIsVisibleChanged != null)
            {
                this.WatermarkTextIsVisibleChanged(this, args);
            }
        }

        public static void OnMinLengthChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (((MaskedTextBox)obj) != null)
            {
                ((MaskedTextBox)obj).OnMinLengthChanged(args);
            }
        }

        protected void OnMinLengthChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.MinLengthChanged != null)
            {
                this.MinLengthChanged(this, args);
            }
            if (MinLength > MaxLength)
            {
                throw new InvalidOperationException("MinLength should be less than MaxLength");
            }
        }


        /// <summary>
        /// Called when [water mark template changed].
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        public static void OnWaterMarkTemplateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (((MaskedTextBox)obj) != null)
            {
                ((MaskedTextBox)obj).OnWaterMarkTemplateChanged(args);
            }
        }

        protected void OnWaterMarkTemplateChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.WatermarkTemplateChanged != null)
            {
                this.WatermarkTemplateChanged(this, args);
            }
        }

        #endregion

        #endregion

        #region Constructor
#if WPF
        static MaskedTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaskedTextBox), new FrameworkPropertyMetadata(typeof(MaskedTextBox)));
            //EnvironmentTest.ValidateLicense(typeof(MaskedTextBox));
        }
#endif
        public ICommand pastecommand { get; private set; }
        public ICommand copycommand { get; private set; }
        public ICommand cutcommand { get; private set; }

        public MaskedTextBox()
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
                EnvironmentTest.StartValidateLicense(typeof(MaskedTextBox));
            }
#endif
#if SILVERLIGHT
            this.DefaultStyleKey = typeof(MaskedTextBox);
#endif
            this.Loaded += new RoutedEventHandler(MaskedTextBox_Loaded);
            
            //Binding readonlybinding = new Binding();
            //readonlybinding.Source = this;
            //readonlybinding.Mode = BindingMode.TwoWay;
            //readonlybinding.Path = new PropertyPath("ReadOnly");
            //this.SetBinding(MaskedTextBox.IsReadOnlyProperty, readonlybinding);
            
            //Binding maxlen = new Binding();
            //maxlen.Source = this;
            //maxlen.Mode = BindingMode.TwoWay;
            //maxlen.Path = new PropertyPath("MaxLength");
            //this.SetBinding(MaskedTextBox.MaxCharLengthProperty, maxlen);
            this.LostFocus += new RoutedEventHandler(MaskedTextBox_LostFocus);
            this.TextChanged += new TextChangedEventHandler(MaskedTextBox_TextChanged);
        }
        private void _pastecommand(object parameter)
        {
            MaskHandler.maskHandler.HandlePaste(this);
        }
        private void _copycommand(object parameter)
        {
            copy();
        }
        private void _cutcommand(object parameter)
        {
            cut();
        }
        private void copy()
        {
            Clipboard.SetText(this.SelectedText);
        }
        private bool Canpaste(object parameter)
        {
            return true;
        }
#if WPF
        private void CommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {

            if ((e as ExecutedRoutedEventArgs).Command == ApplicationCommands.Paste)
            {
                MaskHandler.maskHandler.HandlePaste(this);
                e.Handled = true;
            }
            if ((e as ExecutedRoutedEventArgs).Command == ApplicationCommands.Cut)
            {
                cut();
                e.Handled = true;
            }

        }
#endif
        void MaskedTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.Mask))
            {
               this.Value = this.Text;
            }
        }

        [Obsolete("Use IsReadOnly Property")]
        public bool ReadOnly
        {
            get { return (bool)GetValue(ReadOnlyProperty); }
            set { SetValue(ReadOnlyProperty, value); }
        }

        public static readonly DependencyProperty ReadOnlyProperty =
            DependencyProperty.Register("ReadOnly", typeof(bool), typeof(MaskedTextBox), new PropertyMetadata(false));

        void MaskedTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            this.mIsLoaded = true;
            CoerceWatermarkVisibility(this, this.WatermarkVisibility);
            this.LoadTextBox();
        }
        #endregion

        #region EditorBase
        
        
        internal new int CaretIndex
        {
            get { return (int)GetValue(CaretIndexProperty); }
            set 
            {
                SelectionStart = value;
                SetValue(CaretIndexProperty, value); 
            }
        }

        public static readonly DependencyProperty CaretIndexProperty =
            DependencyProperty.Register("CaretIndex", typeof(int), typeof(MaskedTextBox), new PropertyMetadata((int)0));

        [Obsolete("Property will not help due to internal arhitecture changes")]
        public new bool IsUndoEnabled
        {
            get { return (bool)GetValue(IsUndoEnabledProperty); }
            set { SetValue(IsUndoEnabledProperty, value); }
        }

        public new static readonly DependencyProperty IsUndoEnabledProperty =
            DependencyProperty.Register("IsUndoEnabled", typeof(bool), typeof(MaskedTextBox), new   PropertyMetadata(false));

#if WPF
        [Obsolete("Property will not help due to internal arhitecture changes")]
#endif
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(MaskedTextBox), new PropertyMetadata(new CornerRadius(1)));

        [Obsolete("Property will not help due to internal arhitecture changes")]
        public Brush FocusedBackground
        {
            get { return (Brush)GetValue(FocusedBackgroundProperty); }
            set { SetValue(FocusedBackgroundProperty, value); }
        }

        public static readonly DependencyProperty FocusedBackgroundProperty =
            DependencyProperty.Register("FocusedBackground", typeof(Brush), typeof(MaskedTextBox), new PropertyMetadata(new SolidColorBrush()));

        [Obsolete("Property will not help due to internal arhitecture changes")]
        public Brush FocusedForeground
        {
            get { return (Brush)GetValue(FocusedForegroundProperty); }
            set { SetValue(FocusedForegroundProperty, value); }
        }

        public static readonly DependencyProperty FocusedForegroundProperty =
            DependencyProperty.Register("FocusedForeground", typeof(Brush), typeof(MaskedTextBox), new PropertyMetadata(new SolidColorBrush()));

        [Obsolete("Property will not help due to internal arhitecture changes")]
        public Brush FocusedBorderBrush
        {
            get { return (Brush)GetValue(FocusedBorderBrushProperty); }
            set { SetValue(FocusedBorderBrushProperty, value); }
        }

        public static readonly DependencyProperty FocusedBorderBrushProperty =
            DependencyProperty.Register("FocusedBorderBrush", typeof(Brush), typeof(MaskedTextBox), new PropertyMetadata(new SolidColorBrush()));

        [Obsolete("Property will not help due to internal arhitecture changes")]
        public bool IsCaretAnimationEnabled
        {
            get { return (bool)GetValue(IsCaretAnimationEnabledProperty); }
            set { SetValue(IsCaretAnimationEnabledProperty, value); }
        }

        public static readonly DependencyProperty IsCaretAnimationEnabledProperty =
            DependencyProperty.Register("IsCaretAnimationEnabled", typeof(bool), typeof(MaskedTextBox), new PropertyMetadata(false));

        [Obsolete("Property will not help due to internal arhitecture changes")]
        public Brush ReadOnlyBackground
        {
            get { return (Brush)GetValue(ReadOnlyBackgroundProperty); }
            set { SetValue(ReadOnlyBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ReadOnlyBackgroundProperty =
            DependencyProperty.Register("ReadOnlyBackground", typeof(Brush), typeof(MaskedTextBox), new PropertyMetadata(new SolidColorBrush()));

        [Obsolete("Property will not help due to internal arhitecture changes")]
        public Brush SelectionForeground
        {
            get { return (Brush)GetValue(SelectionForegroundProperty); }
            set { SetValue(SelectionForegroundProperty, value); }
        }

        public static readonly DependencyProperty SelectionForegroundProperty =
            DependencyProperty.Register("SelectionForeground", typeof(Brush), typeof(MaskedTextBox), new PropertyMetadata(new SolidColorBrush()));


        public InvalidInputBehavior InvalidValueBehavior
        {
            get { return (InvalidInputBehavior)GetValue(InvalidValueBehaviorProperty); }
            set { SetValue(InvalidValueBehaviorProperty, value); }
        }

        public static readonly DependencyProperty InvalidValueBehaviorProperty =
            DependencyProperty.Register("InvalidValueBehavior", typeof(InvalidInputBehavior), typeof(MaskedTextBox), new PropertyMetadata(InvalidInputBehavior.None, OnInvalidValueBehaviorChanged));

        public static void OnInvalidValueBehaviorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            MaskedTextBox m = (MaskedTextBox)obj;
            if (m != null)
            {
                m.OnInvalidValueBehaviorChanged(args);
            }
        }

        protected void OnInvalidValueBehaviorChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.InvalidValueBehaviorChanged != null)
            {
                InvalidValueBehaviorChanged(this, args);
            }
        }

        #endregion

        #region Properties

        [BrowsableAttribute(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string MaskedText
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                this.SetValue(TextProperty, value);
                SetValue(MaskedTextProperty, value);
            }
        }

        public string Mask
        {
            get { return (string)GetValue(MaskProperty); }
            set { SetValue(MaskProperty, value); }
        }

        private ObservableCollection<CharacterProperties> mCharCollection = new ObservableCollection<CharacterProperties>();
        internal ObservableCollection<CharacterProperties> CharCollection
        {
            get { return mCharCollection; }
            set { mCharCollection = value; }
        }

        public char PromptChar
        {
            get { return (char)GetValue(PromptCharProperty); }
            set { SetValue(PromptCharProperty, value); }
        }

        public CultureInfo Culture
        {
            get { return (CultureInfo)GetValue(CultureProperty); }
            set { SetValue(CultureProperty, value); }
        }

        public string CurrencySymbol
        {
            get { return (string)GetValue(CurrencySymbolProperty); }
            set { SetValue(CurrencySymbolProperty, value); }
        }

        public string DateSeparator
        {
            get { return (string)GetValue(DateSeparatorProperty); }
            set { SetValue(DateSeparatorProperty, value); }
        }

        public string TimeSeparator
        {
            get { return (string)GetValue(TimeSeparatorProperty); }
            set { SetValue(TimeSeparatorProperty, value); }
        }

        public string DecimalSeparator
        {
            get { return (string)GetValue(DecimalSeparatorProperty); }
            set { SetValue(DecimalSeparatorProperty, value); }
        }

        public string NumberGroupSeparator
        {
            get { return (string)GetValue(NumberGroupSeparatorProperty); }
            set { SetValue(NumberGroupSeparatorProperty, value); }
        }

        public WatermarkTextMode WatermarkTextMode
        {
            get { return (WatermarkTextMode)GetValue(WatermarkTextModeProperty); }
            set { SetValue(WatermarkTextModeProperty, value); }
        }

        public MaskFormat TextMaskFormat
        {
            get { return (MaskFormat)GetValue(TextMaskFormatProperty); }
            set { SetValue(TextMaskFormatProperty, value); }
        }

        public string Value
        {
            get 
            {
                return (string)GetValue(ValueProperty); 
            }
            set
            {
                //object coerceValue = CoerceMaskValue(this, value);
                SetValue(ValueProperty, value);
            }
        }

        private static object CoerceMaskValue(DependencyObject d, object baseValue)
        {
            MaskedTextBox maskedTextbox = (MaskedTextBox)d;
            if (!string.IsNullOrEmpty(maskedTextbox.Mask) || baseValue==null)
            {
                if (maskedTextbox.mValueChanged == true && maskedTextbox.mIsLoaded)
                {
                    if (baseValue == null)
                        baseValue = "";
                    maskedTextbox.CharCollection = MaskHandler.maskHandler.CreateRegularExpression(maskedTextbox);

                    string displaytext = MaskHandler.maskHandler.CoerceValue(maskedTextbox, baseValue.ToString(), MaskFormat.IncludePromptAndLiterals);
                    baseValue = MaskHandler.maskHandler.ValueFromMaskedText(maskedTextbox, maskedTextbox.TextMaskFormat, displaytext, maskedTextbox.CharCollection);
                    maskedTextbox.mValue = MaskHandler.maskHandler.ValueFromMaskedText(maskedTextbox, MaskFormat.ExcludePromptAndLiterals, displaytext, maskedTextbox.CharCollection);
                    maskedTextbox.MaskedText = displaytext;
                }
            }
            else
            {
                maskedTextbox.MaskedText = baseValue.ToString();
            }
            return baseValue;
        }
        public StringValidation StringValidation
        {
            get { return (StringValidation)GetValue(StringValidationProperty); }
            set { SetValue(StringValidationProperty, value); }
        }

        [Obsolete("Use MaxLength property")]
        public int MaxCharLength
        {
            get { return (int)GetValue(MaxCharLengthProperty); }
            set { SetValue(MaxCharLengthProperty, value); }
        }

        public string ValidationString
        {
            get { return (string)GetValue(ValidationStringProperty); }
            set { SetValue(ValidationStringProperty, value); }
        }

        public bool EnterToMoveNext
        {
            get { return (bool)GetValue(EnterToMoveNextProperty); }
            set { SetValue(EnterToMoveNextProperty, value); }
        }

        public bool TextSelectionOnFocus
        {
            get { return (bool)GetValue(TextSelectionOnFocusProperty); }
            set { SetValue(TextSelectionOnFocusProperty, value); }
        }

        public static void OnTextSelectionOnFocusChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            MaskedTextBox m = (MaskedTextBox)obj;
            if (m != null)
            {
                m.OnTextSelectionOnFocusChanged(args);
            }
        }

        protected void OnTextSelectionOnFocusChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.TextSelectionOnFocusChanged != null)
            {
                this.TextSelectionOnFocusChanged(this, args);
            }
        }

        #endregion

        #region PropertyChanged Callback

        public static void OnEnterToMoveNextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            MaskedTextBox m = (MaskedTextBox)obj;
            if (m != null)
            {
                m.OnEnterToMoveNextChanged(args);
            }
        }

        protected void OnEnterToMoveNextChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.EnterToMoveNextChanged != null)
            {
                this.EnterToMoveNextChanged(this, args);
            }
        }

        #endregion

        #region DependencyProperties

        public static readonly DependencyProperty EnterToMoveNextProperty =
            DependencyProperty.Register("EnterToMoveNext", typeof(bool), typeof(MaskedTextBox), new PropertyMetadata(false, OnEnterToMoveNextChanged));


        public static readonly DependencyProperty ValidationStringProperty =
            DependencyProperty.Register("ValidationString", typeof(string), typeof(MaskedTextBox), new PropertyMetadata(string.Empty, OnValidationStringChanged));

        public static void OnValidationStringChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            MaskedTextBox m = (MaskedTextBox)obj;
            if (m != null)
            {
                m.OnValidationStringChanged(args);
            }
        }

        protected void OnValidationStringChanged(DependencyPropertyChangedEventArgs args)
        {
            if (ValidationStringChanged != null)
            {
                ValidationStringChanged(this, args);
            }
        }

        public static readonly DependencyProperty MaxCharLengthProperty =
            DependencyProperty.Register("MaxCharLength", typeof(int), typeof(MaskedTextBox), new PropertyMetadata(int.MaxValue));


        public static readonly DependencyProperty StringValidationProperty =
            DependencyProperty.Register("StringValidation", typeof(StringValidation), typeof(MaskedTextBox), new PropertyMetadata(StringValidation.OnLostFocus));

        public static readonly DependencyProperty TextSelectionOnFocusProperty =
            DependencyProperty.Register("TextSelectionOnFocus", typeof(bool), typeof(MaskedTextBox), new PropertyMetadata(true, OnTextSelectionOnFocusChanged));

        public static readonly DependencyProperty PromptCharProperty =
            DependencyProperty.Register("PromptChar", typeof(char), typeof(MaskedTextBox), new PropertyMetadata('_', OnPromptCharChanged));

        public static void OnPromptCharChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            MaskedTextBox m = (MaskedTextBox)obj;
            if (m != null)
            {
                m.OnPromptCharChanged(args);
            }
        }

        protected void OnPromptCharChanged(DependencyPropertyChangedEventArgs args)
        {
            if (PromptCharChanged != null)
            {
                PromptCharChanged(this, args);
            }
            if (mIsLoaded)
            {
                this.LoadTextBox();
            }
        }

        public static readonly DependencyProperty MaskedTextProperty =
             DependencyProperty.Register("MaskedText", typeof(string), typeof(MaskedTextBox), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty MaskProperty =
            DependencyProperty.Register("Mask", typeof(string), typeof(MaskedTextBox), new PropertyMetadata(string.Empty,OnMaskChanged));

        public static void OnMaskChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            MaskedTextBox m = (MaskedTextBox)obj;
            if (m != null)
            {
                m.OnMaskChanged(args);
            }
        }

        protected void OnMaskChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.MaskChanged != null)
            {
                this.MaskChanged(this, args);
            }
            if (mIsLoaded)
            {
                this.LoadTextBox();
            }
            
        }

        public static readonly DependencyProperty CultureProperty =
            DependencyProperty.Register("Culture", typeof(CultureInfo), typeof(MaskedTextBox), new PropertyMetadata(CultureInfo.CurrentCulture));

        public static readonly DependencyProperty CurrencySymbolProperty =
          DependencyProperty.Register("CurrencySymbol", typeof(string), typeof(MaskedTextBox), new PropertyMetadata(string.Empty, OnCurrencySymbolChanged));

        public static void OnCurrencySymbolChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            MaskedTextBox m = (MaskedTextBox)obj;
            if (m != null)
            {
                m.OnCurrencySymbolChanged(args);
            }
        }

        protected void OnCurrencySymbolChanged(DependencyPropertyChangedEventArgs args)
        {
            if (CurrencySymbolChanged != null)
            {
                CurrencySymbolChanged(this, args);
            }
            if (mIsLoaded)
            {
                this.LoadTextBox();
            }
        }

        public static readonly DependencyProperty DateSeparatorProperty =
            DependencyProperty.Register("DateSeparator", typeof(string), typeof(MaskedTextBox), new PropertyMetadata(string.Empty, OnDateSeparatorChanged));

        public static void OnDateSeparatorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            MaskedTextBox m = (MaskedTextBox)obj;
            if (m != null)
            {
                m.OnDateSeparatorChanged(args);
            }
        }

        protected void OnDateSeparatorChanged(DependencyPropertyChangedEventArgs args)
        {
            if (DateSeparatorChanged != null)
            {
                DateSeparatorChanged(this, args);
            }
            if (mIsLoaded)
            {
                this.LoadTextBox();
            }
        }

        public static readonly DependencyProperty TimeSeparatorProperty =
            DependencyProperty.Register("TimeSeparator", typeof(string), typeof(MaskedTextBox), new PropertyMetadata(string.Empty, OnTimeSeparatorChanged));

        public static void OnTimeSeparatorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            MaskedTextBox m = (MaskedTextBox)obj;
            if (m != null)
            {
                m.OnTimeSeparatorChanged(args);
            }
        }

        protected void OnTimeSeparatorChanged(DependencyPropertyChangedEventArgs args)
        {
            if (TimeSeparatorChanged != null)
            {
                TimeSeparatorChanged(this, args);
            }
            if (mIsLoaded)
            {
                this.LoadTextBox();
            }
        }

        public static readonly DependencyProperty DecimalSeparatorProperty =
            DependencyProperty.Register("DecimalSeparator", typeof(string), typeof(MaskedTextBox), new PropertyMetadata(string.Empty, OnDecimalSeparatorChanged));

        public static void OnDecimalSeparatorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            MaskedTextBox m = (MaskedTextBox)obj;
            if (m != null)
            {
                m.OnDecimalSeparatorChanged(args);
            }
        }

        protected void OnDecimalSeparatorChanged(DependencyPropertyChangedEventArgs args)
        {
            if (DecimalSeparatorChanged != null)
            {
                DecimalSeparatorChanged(this, args);
            }
            if (mIsLoaded)
            {
                this.LoadTextBox();
            }
        }



        public bool GroupSeperatorEnabled
        {
            get { return (bool)GetValue(GroupSeperatorEnabledProperty); }
            set { SetValue(GroupSeperatorEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GroupSeperatorEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GroupSeperatorEnabledProperty =
            DependencyProperty.Register("GroupSeperatorEnabled", typeof(bool), typeof(MaskedTextBox), new PropertyMetadata(true,new PropertyChangedCallback(OnNumberGroupSeparatorChanged)));

        

        public static readonly DependencyProperty NumberGroupSeparatorProperty =
            DependencyProperty.Register("NumberGroupSeparator", typeof(string), typeof(MaskedTextBox), new PropertyMetadata(string.Empty, OnNumberGroupSeparatorChanged));

        public static void OnNumberGroupSeparatorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            MaskedTextBox m = (MaskedTextBox)obj;
            if (m != null)
            {
                m.OnNumberGroupSeparatorChanged(args);
            }
        }

        protected void OnNumberGroupSeparatorChanged(DependencyPropertyChangedEventArgs args)
        {
            if (NumberGroupSeparatorChanged != null)
            {
                NumberGroupSeparatorChanged(this, args);
            }
            if (mIsLoaded)
            {
                this.LoadTextBox();
            }
        }
        public static readonly DependencyProperty WatermarkTextModeProperty =
            DependencyProperty.Register("WatermarkTextMode", typeof(WatermarkTextMode), typeof(MaskedTextBox), new PropertyMetadata(WatermarkTextMode.HideTextOnFocus, OnWatermarkTextModeChanged));

        public static void OnWatermarkTextModeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            MaskedTextBox m = (MaskedTextBox)obj;
            if (m != null)
            {
                m.OnWatermarkTextModeChanged(args);
            }
        }

        protected void OnWatermarkTextModeChanged(DependencyPropertyChangedEventArgs args)
        {
            if (WatermarkTextModeChanged != null)
            {
                WatermarkTextModeChanged(this, args);
            }
        }

        public static readonly DependencyProperty TextMaskFormatProperty =
            DependencyProperty.Register("TextMaskFormat", typeof(MaskFormat), typeof(MaskedTextBox), new PropertyMetadata(MaskFormat.IncludeLiterals));
#if WPF
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(string), typeof(MaskedTextBox), new FrameworkPropertyMetadata(string.Empty,FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged, new CoerceValueCallback(CoerceMaskValue)));
#endif
#if SILVERLIGHT
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(string), typeof(MaskedTextBox), new PropertyMetadata(string.Empty, OnValueChanged));
#endif
        #endregion

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

        protected override void OnDrop(DragEventArgs e)
        {
            e.Handled = true;
            base.OnDrop(e);
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (this.TextSelectionOnFocus)
            {
                if (this.IsFocused == false)
                {
                    e.Handled = true;
                    this.Focus();
                }
            }
            base.OnPreviewMouseLeftButtonDown(e);
        }

#endif

        public static void OnValueChanged(DependencyObject obj,DependencyPropertyChangedEventArgs args)
        {
            if (((MaskedTextBox)obj) != null)
            {
                ((MaskedTextBox)obj).OnValueChanged(args);
            }
        }

        protected void OnValueChanged(DependencyPropertyChangedEventArgs args)
        {

#if SILVERLIGHT
            if (mValueChanged && this.mIsLoaded)
            {
                string tempVal = CoerceMaskValue(this, this.Value).ToString();
                if (tempVal != this.Value)
                {
                    this.SetValue(false, tempVal);
                    return;
                }
            }
#endif
            if (this.mIsLoaded)
            {
                if (string.IsNullOrEmpty(this.Mask))
                {
                    if (Regex.IsMatch(this.MaskedText, this.ValidationString))
                        MaskCompleted = true;
                    else
                        MaskCompleted = false;
                }
                else
                {
                    if (MaskHandler.maskHandler.ValueFromMaskedText(this, MaskFormat.IncludeLiterals, this.MaskedText, this.CharCollection) == this.MaskedText)
                        MaskCompleted = true;
                    else
                        MaskCompleted = false;
                }
            }

            if (this.ValueChanged != null)
            {
                this.ValueChanged(this, args);
            }
            oldValue = (string)args.OldValue;
            if (!string.IsNullOrEmpty(this.Value))
            {
                this.WatermarkVisibility = Visibility.Collapsed;
            }
            else
            {
                if(this.Focus() != true)
                this.WatermarkVisibility = Visibility.Visible;
            }
            //if (this.mIsLoaded)
            //{
            //    if (mValueChanged)
            //    {
            //        string displaytext = MaskHandler.maskHandler.CoerceValue(this, this.Value.ToString(), this.TextMaskFormat);
            //        string baseValue = MaskHandler.maskHandler.ValueFromMaskedText(this, this.TextMaskFormat, displaytext, this.CharCollection);
            //        this.mValue = MaskHandler.maskHandler.ValueFromMaskedText(this, MaskFormat.ExcludePromptAndLiterals, displaytext, this.CharCollection);
            //        this.LoadTextBox();
            //    }
            //}
        }

        public bool MaskCompleted
        {
            get { return (bool)GetValue(MaskCompletedProperty); }
            set { SetValue(MaskCompletedProperty, value); }
        }

        public static readonly DependencyProperty MaskCompletedProperty =
            DependencyProperty.Register("MaskCompleted", typeof(bool), typeof(MaskedTextBox), new PropertyMetadata(false, OnMaskCompletedPropertyChanged));

        public static void OnMaskCompletedPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            MaskedTextBox mask = (MaskedTextBox)obj;
            if (mask != null)
                mask.OnMaskCompletedPropertyChanged(args);
        }

        protected void OnMaskCompletedPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.MaskCompletedChanged != null)
            {
                this.MaskCompletedChanged(this, args);
            }
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            if (mMouseLeftButtonDown == false)
            {
                if (string.IsNullOrEmpty(this.Mask))
                {
                    if (!OnValidating(new CancelEventArgs(false)))
                    {
                        string maskedText = this.MaskedText;
                        string validationerror = "";
                        bool validationstatus = true;
                        if (this.MaskedText.Length < this.MinLength && this.MaskedText.Length > 0)
                        {
                            if (oldValue.Length >= this.MinLength)
                                this.MaskedText = oldValue;
                            else
                                this.MaskedText = "";
                        }
                        validationstatus = Regex.IsMatch(maskedText, this.ValidationString);
                        string message = validationstatus ? "String validation succeeded" : "String validation failed";

                        if (!validationstatus)
                        {
                            switch (InvalidValueBehavior)
                            {
                                case InvalidInputBehavior.DisplayErrorMessage:
                                    OnStringValidationCompleted(new StringValidationEventArgs(validationstatus, validationerror, this.ValidationString));
                                    OnValidated(EventArgs.Empty);
                                    MessageBox.Show(message, "Invalid value", MessageBoxButton.OK);
                                    break;
                                case InvalidInputBehavior.None:
                                    OnStringValidationCompleted(new StringValidationEventArgs(validationstatus, validationerror, this.ValidationString));
                                    OnValidated(EventArgs.Empty);
                                    break;
                                case InvalidInputBehavior.ResetValue:
                                    OnStringValidationCompleted(new StringValidationEventArgs(validationstatus, validationerror, this.ValidationString));
                                    OnValidated(EventArgs.Empty);
                                    this.Value = "";
                                    this.WatermarkVisibility = Visibility.Visible;
                                    return;
                            }
                        }
                    }
                }
                base.OnLostFocus(e);
            }
            else
            {
                this.Focus();
            }
            //if (string.IsNullOrEmpty(tempVal))
            //{
            //    if (tempVal.Length <= 0)
            //        this.WatermarkVisibility = Visibility.Visible;
            //}
            //else
            //{
            //    if (mValue != null)
            //    {
            //        if (mValue.Length <= 0)
            //        {
            //            this.WatermarkVisibility = Visibility.Visible;
            //        }
            //    }
            //}

            //if (tempVal != this.Value)
            //{
            //    this.SetValue(false, tempVal);
            //}
        }


        void MaskedTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (mMouseLeftButtonDown == false)
            {
                if (string.IsNullOrEmpty(this.Mask))
                {
                    if (string.IsNullOrEmpty(this.MaskedText))
                    {
                        this.WatermarkVisibility = Visibility.Visible;
                    }
                }
                else
                {
                    string tempVal = MaskHandler.maskHandler.ValueFromMaskedText(this, MaskFormat.ExcludePromptAndLiterals, this.MaskedText, CharCollection);
                    if (string.IsNullOrEmpty(tempVal))
                    {
                        this.WatermarkVisibility = Visibility.Visible;
                    }
                }
            }
        }

        internal void OnStringValidationCompleted(StringValidationEventArgs e)
        {
            if (StringValidationCompleted != null)
            {
                StringValidationCompleted(this, e);
            }
        }

        internal void OnValidated(EventArgs e)
        {
            if (Validated != null)
            {
                Validated(this, e);
            }
        }

        internal bool OnValidating(CancelEventArgs e)
        {
            if (Validating != null)
            {
                Validating(this, e);
                return e.Cancel;
            }
            return false;
        }


        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            if (WatermarkTextMode.HideTextOnFocus == WatermarkTextMode)
                this.WatermarkVisibility = Visibility.Collapsed;
            if (TextSelectionOnFocus)
            {
                this.SelectAll();
            }
        }

        internal void LoadTextBox()
        {
            string tempVal = CoerceMaskValue(this, this.Value).ToString();
            if (this.Value != tempVal)
            {
                this.SetValue(false, tempVal);
            }
        }

        internal CultureInfo GetCulture()
        {
            CultureInfo cultureInfo;
            if (Culture != null)
            {
                cultureInfo = this.Culture.Clone() as CultureInfo;
            }
            else
            {
                cultureInfo = CultureInfo.CurrentCulture.Clone() as CultureInfo;
            }
           
            if (CurrencySymbol != string.Empty)
                cultureInfo.NumberFormat.CurrencySymbol = CurrencySymbol;
            cultureInfo.NumberFormat.CurrencySymbol = cultureInfo.NumberFormat.CurrencySymbol[0].ToString();

            if (DecimalSeparator != string.Empty)
                cultureInfo.NumberFormat.NumberDecimalSeparator = DecimalSeparator;
            cultureInfo.NumberFormat.NumberDecimalSeparator = cultureInfo.NumberFormat.NumberDecimalSeparator[0].ToString();

            if (!GroupSeperatorEnabled)
            {
                cultureInfo.NumberFormat.NumberGroupSeparator = string.Empty;
            }

            if (GroupSeperatorEnabled == true)
            {
                if (NumberGroupSeparator != string.Empty)
                    cultureInfo.NumberFormat.NumberGroupSeparator = NumberGroupSeparator;
                cultureInfo.NumberFormat.NumberGroupSeparator = cultureInfo.NumberFormat.NumberGroupSeparator[0].ToString();
            }

            return cultureInfo;
        }
#if WPF
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
#endif
#if SILVERLIGHT
        string oldselection;
        protected override void OnKeyDown(KeyEventArgs e)
        {
#endif
            if (WatermarkTextMode.HideTextOnTyping == WatermarkTextMode)
                this.WatermarkVisibility = Visibility.Collapsed;

            if (ModifierKeys.Control == Keyboard.Modifiers)
            {
                if (e.Key == Key.V)
                {                    
                    MaskHandler.maskHandler.HandlePaste(this);
                    e.Handled = true;
                }
                if (e.Key == Key.C)
                {

                }
                if (e.Key == Key.Z)
                {
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
                if (e.Key == Key.Space)
                {
                    e.Handled = MaskHandler.maskHandler.MatchWithMask(this, " ");
                    if (e.Handled == true)
                    {
                        if (e.Handled == true)
                        {
                            string tempVal = MaskHandler.maskHandler.CreateValueFromText(this);
                            if (tempVal != this.Value)
                            {
                                this.SetValue(false, tempVal);
                            }
                        }
                    }
                }

                else
                {
                    e.Handled = MaskHandler.maskHandler.HandleKeyDown(this, e);
                    if (e.Handled == true)
                    {
                        string tempVal = MaskHandler.maskHandler.CreateValueFromText(this);
                        if (tempVal != this.Value)
                        {
                            this.SetValue(false, tempVal);
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

        private void cut()
        {
            if (this.SelectionLength > 0)
            {
                Clipboard.SetText(this.SelectedText);
                MaskHandler.maskHandler.HandleDeleteKey(this);
            }
        }

        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            e.Handled = MaskHandler.maskHandler.MatchWithMask(this, e.Text);
            if (!string.IsNullOrEmpty(this.Mask))
            {
                if (e.Handled == true)
                {
                    //string temstr = MaskHandler.maskHandler.CreateDisplayText(this);
                    string tempVal = MaskHandler.maskHandler.CreateValueFromText(this);
                    if (tempVal != this.Value)
                    {
                        this.SetValue(false, tempVal);
                    }
                }
            }
            base.OnTextInput(e);
        }

        internal void SetValue(bool? IsReload, object _Value)
        {
            mValueChanged = false;
            this.Value = _Value.ToString();
            this.mValue = MaskHandler.maskHandler.ValueFromMaskedText(this, MaskFormat.ExcludePromptAndLiterals, this.MaskedText, this.CharCollection);
            mValueChanged = true;
            //this.MaskedText = MaskHandler.maskHandler.CreateDisplayText(this);
        }

        ContentControl PART_Watermark;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PART_Watermark = this.GetTemplateChild("PART_Watermark") as ContentControl;
        }

        //private bool mIsMouseOver = false;
        //protected override void OnMouseEnter(MouseEventArgs e)
        //{
        //    mIsMouseOver = true;
        //    base.OnMouseEnter(e);
        //}
        //protected override void OnMouseLeave(MouseEventArgs e)
        //{
        //    mIsMouseOver = false;
        //    base.OnMouseLeave(e);
        //}

        private bool mMouseLeftButtonDown = false;
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            mMouseLeftButtonDown = true;
            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            mMouseLeftButtonDown = false;
            base.OnMouseLeftButtonUp(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            mMouseLeftButtonDown = false;
            base.OnMouseLeave(e);
        }
    }
}
