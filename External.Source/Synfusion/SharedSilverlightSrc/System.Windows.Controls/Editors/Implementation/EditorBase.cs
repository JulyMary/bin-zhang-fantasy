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
using System.Windows.Data;


#if WPF
namespace Syncfusion.Windows.Shared
#endif

#if SILVERLIGHT
namespace Syncfusion.Windows.Tools.Controls
#endif
{
    public class EditorBase : TextBox
    {
        #region Events

        public event PropertyChangedCallback CultureChanged;
        public event PropertyChangedCallback NumberFormatChanged;
        public event PropertyChangedCallback WaterMarkTemplateChanged;
        public event PropertyChangedCallback WaterMarkTextChanged;

        /// <summary>
        /// Occurs when <see cref="IsUndoEnabled"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback IsUndoEnabledChanged;

        /// <summary>
        /// Occurs when [text selection on focus changed].
        /// </summary>
        public event PropertyChangedCallback TextSelectionOnFocusChanged;


       
        /// <summary>
        /// Event that is raised when <see cref="NegativeForeground"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback NegativeForegroundChanged;

        /// <summary>
        /// Event that is raised when <see cref="IsValueNegativeChanged"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback IsValueNegativeChanged;

        /// <summary>
        /// Event that is raised when <see cref="EnterToMoveNext"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback EnterToMoveNextChanged;

        #endregion

        internal bool minusPressed = false;
      

        public EditorBase()
        {
           
            //Binding foregroundbinding = new Binding();
            //foregroundbinding.Source = this;
            //foregroundbinding.Mode = BindingMode.TwoWay;
            //foregroundbinding.Path = new PropertyPath("Foreground");
            //this.SetBinding(EditorBase.EditorForegroundProperty, foregroundbinding);

            //Binding readonlybinding = new Binding();
            //readonlybinding.Source = this;
            //readonlybinding.Mode = BindingMode.TwoWay;
            //readonlybinding.Path = new PropertyPath("IsReadOnly");
            //this.SetBinding(EditorBase.ReadOnlyProperty, readonlybinding);
#if WPF
            this.MouseDoubleClick += new MouseButtonEventHandler(EditorBase_MouseDoubleClick);
#endif
        }      

#if WPF
        
        void EditorBase_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.SelectAll();
        }

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
#endif

        [Obsolete("Use IsReadOnly Property")]
        public bool ReadOnly
        {
            get { return (bool)GetValue(ReadOnlyProperty); }
            set { SetValue(ReadOnlyProperty, value); }
        }

        public static readonly DependencyProperty ReadOnlyProperty =
            DependencyProperty.Register("ReadOnly", typeof(bool), typeof(EditorBase), new PropertyMetadata(false));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(EditorBase), new PropertyMetadata(OnCornerRadiusChanged));

        public static void OnCornerRadiusChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
        }

       // [Obsolete("Property will not help due to internal arhitecture changes")]
       internal Brush FocusedBackground
        {
            get { return (Brush)GetValue(FocusedBackgroundProperty); }
            set { SetValue(FocusedBackgroundProperty, value); }
        }

        public static readonly DependencyProperty FocusedBackgroundProperty =
            DependencyProperty.Register("FocusedBackground", typeof(Brush), typeof(EditorBase), new PropertyMetadata(OnFocusedBackgroundChanged));

        public static void OnFocusedBackgroundChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
        }

        //[Obsolete("Property will not help due to internal arhitecture changes")]
        internal Brush FocusedForeground
        {
            get { return (Brush)GetValue(FocusedForegroundProperty); }
            set { SetValue(FocusedForegroundProperty, value); }
        }

        public static readonly DependencyProperty FocusedForegroundProperty =
            DependencyProperty.Register("FocusedForeground", typeof(Brush), typeof(EditorBase), new PropertyMetadata(OnFocusedForegroundChanged));

        public static void OnFocusedForegroundChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            EditorBase e = (EditorBase)obj;
            if (e != null)
            {
                e.OnFocusedForegroundChanged(args);
            }
        }

        private void OnFocusedForegroundChanged(DependencyPropertyChangedEventArgs args)
        {
            //if (args.NewValue != null)
            //{
            //    if (this.IsFocused == true)
            //    {
            //        this.Foreground = this.FocusedForeground;
            //    }
            //}
        }

        [Obsolete("Property will not help due to internal arhitecture changes")]
        public Brush FocusedBorderBrush
        {
            get { return (Brush)GetValue(FocusedBorderBrushProperty); }
            set { SetValue(FocusedBorderBrushProperty, value); }
        }

        public static readonly DependencyProperty FocusedBorderBrushProperty =
            DependencyProperty.Register("FocusedBorderBrush", typeof(Brush), typeof(EditorBase), new PropertyMetadata(OnFocusedBorderBrushChanged));

        public static void OnFocusedBorderBrushChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
        }


        [Obsolete("Property will not help due to internal arhitecture changes")]
        public Brush ReadOnlyBackground
        {
            get { return (Brush)GetValue(ReadOnlyBackgroundProperty); }
            set { SetValue(ReadOnlyBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ReadOnlyBackgroundProperty =
            DependencyProperty.Register("ReadOnlyBackground", typeof(Brush), typeof(EditorBase), new PropertyMetadata(OnReadOnlyBackgroundChanged));

        public static void OnReadOnlyBackgroundChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
        }

        [Obsolete("Property will not help due to internal arhitecture changes")]
        public Brush SelectionForeground
        {
            get { return (Brush)GetValue(SelectionForegroundProperty); }
            set { SetValue(SelectionForegroundProperty, value); }
        }

        public static readonly DependencyProperty SelectionForegroundProperty =
            DependencyProperty.Register("SelectionForeground", typeof(Brush), typeof(EditorBase), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        /// <summary>
        /// Gets or sets a value indicating whether [enable focus colors].
        /// </summary>
        /// <value><c>true</c> if [enable focus colors]; otherwise, <c>false</c>.</value>
        public bool EnableFocusColors
        {
            get { return (bool)GetValue(EnableFocusColorsProperty); }
            set { SetValue(EnableFocusColorsProperty, value); }
        }

        public static readonly DependencyProperty EnableFocusColorsProperty =
            DependencyProperty.Register("EnableFocusColors", typeof(bool), typeof(EditorBase), new PropertyMetadata(false));

        
        #region Properties

        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        /// <value>The culture.</value>
        public CultureInfo Culture
        {
            get { return (CultureInfo)GetValue(CultureProperty); }
            set { SetValue(CultureProperty, value); }
        }

        /// <summary>
        /// Gets or sets the number format.
        /// </summary>
        /// <value>The number format.</value>
        public NumberFormatInfo NumberFormat
        {
            get { return (NumberFormatInfo)GetValue(NumberFormatProperty); }
            set { SetValue(NumberFormatProperty, value); }
        }

        /// <summary>
        /// Gets or sets the editor foreground.
        /// </summary>
        /// <value>The editor foreground.</value>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [BrowsableAttribute(false)]
        public Brush EditorForeground
        {
            get { return (Brush)GetValue(EditorForegroundProperty); }
            set { SetValue(EditorForegroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the positive foreground.
        /// </summary>
        /// <value>The positive foreground.</value>
        public Brush PositiveForeground
        {
            get { return (Brush)GetValue(PositiveForegroundProperty); }
            set { SetValue(PositiveForegroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is enable negative.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is enable negative; otherwise, <c>false</c>.
        /// </value>
        public bool ApplyNegativeForeground
        {
            get { return (bool)GetValue(ApplyNegativeForegroundProperty); }
            set { SetValue(ApplyNegativeForegroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is negative.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is negative; otherwise, <c>false</c>.
        /// </value>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [BrowsableAttribute(false)]
        public bool IsNegative
        {
            get { return (bool)GetValue(IsNegativeProperty); }
            set { SetValue(IsNegativeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the negative foreground.
        /// </summary>
        /// <value>The negative foreground.</value>
        public Brush NegativeForeground
        {
            get { return (Brush)GetValue(NegativeForegroundProperty); }
            set { SetValue(NegativeForegroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is zero.
        /// </summary>
        /// <value><c>true</c> if this instance is zero; otherwise, <c>false</c>.</value>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [BrowsableAttribute(false)]
        public bool IsZero
        {
            get { return (bool)GetValue(IsZeroProperty); }
            set { SetValue(IsZeroProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is apply zero color.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is apply zero color; otherwise, <c>false</c>.
        /// </value>
        public bool ApplyZeroColor
        {
            get { return (bool)GetValue(ApplyZeroColorProperty); }
            set { SetValue(ApplyZeroColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the zero.
        /// </summary>
        /// <value>The color of the zero.</value>
        public Brush ZeroColor
        {
            get { return (Brush)GetValue(ZeroColorProperty); }
            set { SetValue(ZeroColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [use null option].
        /// </summary>
        /// <value><c>true</c> if [use null option]; otherwise, <c>false</c>.</value>
        public bool UseNullOption
        {
            get { return (bool)GetValue(UseNullOptionProperty); }
            set { SetValue(UseNullOptionProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is null.
        /// </summary>
        /// <value><c>true</c> if this instance is null; otherwise, <c>false</c>.</value>
        [BrowsableAttribute(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsNull
        {
            get { return (bool)GetValue(IsNullProperty); }
            set { SetValue(IsNullProperty, value); }
        }

        /// <summary>
        /// Gets or sets the max validation.
        /// </summary>
        /// <value>The max validation.</value>
        public MaxValidation MaxValidation
        {
            get { return (MaxValidation)GetValue(MaxValidationProperty); }
            set { SetValue(MaxValidationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the min validation.
        /// </summary>
        /// <value>The min validation.</value>
        public MinValidation MinValidation
        {
            get { return (MinValidation)GetValue(MinValidationProperty); }
            set { SetValue(MinValidationProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [max value on exceed max digit].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [max value on exceed max digit]; otherwise, <c>false</c>.
        /// </value>
        public bool MaxValueOnExceedMaxDigit
        {
            get { return (bool)GetValue(MaxValueOnExceedMaxDigitProperty); }
            set { SetValue(MaxValueOnExceedMaxDigitProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [min value on exceed min digit].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [min value on exceed min digit]; otherwise, <c>false</c>.
        /// </value>
        public bool MinValueOnExceedMinDigit
        {
            get { return (bool)GetValue(MinValueOnExceedMinDigitProperty); }
            set { SetValue(MinValueOnExceedMinDigitProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is undo enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is undo enabled; otherwise, <c>false</c>.
        /// </value>
        public new bool IsUndoEnabled
        {
            get { return (bool)GetValue(IsUndoEnabledProperty); }
            set { SetValue(IsUndoEnabledProperty, value); }
        }

        /// <summary>
        /// Gets or sets the masked text.
        /// </summary>
        /// <value>The masked text.</value>
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

        #endregion

        #region DependencyProperty

        public static readonly DependencyProperty PositiveForegroundProperty =
            DependencyProperty.Register("PositiveForeground", typeof(Brush), typeof(EditorBase), new PropertyMetadata(OnPositiveForegroundChanged));

        public static void OnPositiveForegroundChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            EditorBase e = (EditorBase)obj;
            if (e != null)
            {
                e.OnPositiveForegroundChanged(args);
            }
        }

        protected void OnPositiveForegroundChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.IsZero && this.ApplyZeroColor)
            {
                return;
            }
            else if (this.IsNegative && this.ApplyNegativeForeground)
            {
                return;
            }
            this.Foreground = this.PositiveForeground;
        }

        public static readonly DependencyProperty EditorForegroundProperty =
            DependencyProperty.Register("EditorForeground", typeof(Brush), typeof(EditorBase), new PropertyMetadata(new SolidColorBrush(Colors.Black), OnForegroundChanged));

        /// <summary>
        /// Identifies the NegativeForegroundProperty.
        /// </summary>
        public static readonly DependencyProperty NegativeForegroundProperty =
            DependencyProperty.Register("NegativeForeground", typeof(Brush), typeof(EditorBase), new PropertyMetadata(new SolidColorBrush(Colors.Red), OnNegativeForegroundChanged));

        /// <summary>
        /// Identifies the IsApplyNegativeColorProperty.
        /// </summary>
        public static readonly DependencyProperty ApplyNegativeForegroundProperty =
            DependencyProperty.Register("ApplyNegativeForeground", typeof(bool), typeof(EditorBase), new PropertyMetadata(false, OnApplyNegativeForegroundChanged));

        public static void OnApplyNegativeForegroundChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            EditorBase b = (EditorBase)obj;
            if (b != null)
                b.OnApplyNegativeForegroundChanged(args);
        }

        protected void OnApplyNegativeForegroundChanged(DependencyPropertyChangedEventArgs args)
        {
            this.SetForeground();
        }

        /// <summary>
        /// Identifies the IsNegativeProperty.
        /// </summary>
        public static readonly DependencyProperty IsNegativeProperty =
            DependencyProperty.Register("IsNegative", typeof(bool), typeof(EditorBase), new PropertyMetadata(false, OnIsNegativeChanged));

        /// <summary>
        /// Identifies the IsZeroProperty.
        /// </summary>
        public static readonly DependencyProperty IsZeroProperty =
            DependencyProperty.Register("IsZero", typeof(bool), typeof(EditorBase), new PropertyMetadata(false, OnIsZeroChanged));

        public static readonly DependencyProperty MaxValidationProperty =
            DependencyProperty.Register("MaxValidation", typeof(MaxValidation), typeof(EditorBase), new PropertyMetadata(MaxValidation.OnKeyPress));

        public static readonly DependencyProperty MinValidationProperty =
            DependencyProperty.Register("MinValidation", typeof(MinValidation), typeof(EditorBase), new PropertyMetadata(MinValidation.OnKeyPress, OnMinValidationChanged));

        public static void OnMinValidationChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
        }

        public static readonly DependencyProperty MaxValueOnExceedMaxDigitProperty =
            DependencyProperty.Register("MaxValueOnExceedMaxDigit", typeof(bool), typeof(EditorBase), new PropertyMetadata(false));

        public static readonly DependencyProperty MinValueOnExceedMinDigitProperty =
            DependencyProperty.Register("MinValueOnExceedMinDigit", typeof(bool), typeof(EditorBase), new PropertyMetadata(false));

        public static readonly DependencyProperty IsNullProperty =
            DependencyProperty.Register("IsNull", typeof(bool), typeof(EditorBase), new PropertyMetadata(false,new PropertyChangedCallback(OnIsNullChanged)));

        public static readonly DependencyProperty UseNullOptionProperty =
            DependencyProperty.Register("UseNullOption", typeof(bool), typeof(EditorBase), new PropertyMetadata(false));

        public static readonly DependencyProperty CultureProperty =
            DependencyProperty.Register("Culture", typeof(CultureInfo), typeof(EditorBase), new PropertyMetadata(CultureInfo.CurrentCulture, new PropertyChangedCallback(OnCultureChanged)));

        /// <summary>
        /// Identifies the ZeroColorProperty.
        /// </summary>
        public static readonly DependencyProperty ZeroColorProperty =
            DependencyProperty.Register("ZeroColor", typeof(Brush), typeof(EditorBase), new PropertyMetadata(new SolidColorBrush(Colors.Green), OnZeroNegativeColorChanged));

        public static void OnZeroNegativeColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            EditorBase b = (EditorBase)obj;
            if (b != null)
                b.OnZeroNegativeColorChanged(args);
                
        }

        protected void OnZeroNegativeColorChanged(DependencyPropertyChangedEventArgs args)
        {
            this.SetForeground();
        }

        /// <summary>
        /// Identifies the IsApplyZeroColorProperty.
        /// </summary>
        public static readonly DependencyProperty ApplyZeroColorProperty =
            DependencyProperty.Register("ApplyZeroColor", typeof(bool), typeof(EditorBase), new PropertyMetadata(false,OnApplyZeroColorChanged));

        public static void OnApplyZeroColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            EditorBase b = (EditorBase)obj;
            if (b != null)
                b.OnApplyZeroColorChanged(args);
        }

        protected void OnApplyZeroColorChanged(DependencyPropertyChangedEventArgs args)
        {
            this.SetForeground();
        }

        public static readonly DependencyProperty NumberFormatProperty =
            DependencyProperty.Register("NumberFormat", typeof(NumberFormatInfo), typeof(EditorBase), new PropertyMetadata(null, new PropertyChangedCallback(OnNumberFormatChanged)));

        public new static readonly DependencyProperty IsUndoEnabledProperty =
            DependencyProperty.Register("IsUndoEnabled", typeof(bool), typeof(EditorBase), new PropertyMetadata(true, OnIsUndoEnabledChanged));

        public static readonly DependencyProperty MaskedTextProperty =
             DependencyProperty.Register("MaskedText", typeof(string), typeof(EditorBase), new PropertyMetadata(string.Empty));

        #endregion

        #region Methods
#if SILVERLIGHT
        private bool IsFocused = false;
#endif
        internal void SetForeground()
        {
            if (this.IsFocused == true && this.EnableFocusColors)
            {
                this.Foreground = this.FocusedForeground;
            }
            else if (this.ApplyZeroColor && this.IsZero)
            {
                this.Foreground = this.ZeroColor;
            }
            else if (this.ApplyNegativeForeground && this.IsNegative)
            {
                this.Foreground = this.NegativeForeground;
            }
            else
            {
                this.Foreground = this.PositiveForeground;
            }
        }

        #endregion

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
            DependencyProperty.Register("WatermarkTemplate", typeof(DataTemplate), typeof(EditorBase), new PropertyMetadata(OnWaterMarkTemplateChanged));

        /// <summary>
        /// Called when [water mark template changed].
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        public static void OnWaterMarkTemplateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (((EditorBase)obj) != null)
            {
                ((EditorBase)obj).OnWaterMarkTemplateChanged(args);
            }
        }

        protected void OnWaterMarkTemplateChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.WaterMarkTemplateChanged != null)
            {
                this.WaterMarkTemplateChanged(this, args);
            }
        }

        /// <summary>
        /// Gets or sets the water mark text.
        /// </summary>
        /// <value>The water mark text.</value>
        public string WatermarkText
        {
            set { SetValue(WatermarkTextProperty, value); }
            get { return (string)GetValue(WatermarkTextProperty); }
        }

        /// <summary>
        /// Identifies the WaterMarkText dependency property.
        /// </summary>
        public static DependencyProperty WatermarkTextProperty =
            DependencyProperty.Register("WatermarkText", typeof(string), typeof(EditorBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnWaterMarkTextChanged)));

        public static void OnWaterMarkTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (((EditorBase)obj) != null)
            {
                ((EditorBase)obj).OnWaterMarkTextChanged(args);
            }
        }

        protected void OnWaterMarkTextChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.WaterMarkTextChanged != null)
                this.WaterMarkTextChanged(this, args);
        }

        /// <summary>
        /// Gets or sets the water mark visibility.
        /// </summary>
        /// <value>The water mark visibility.</value>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [BrowsableAttribute(false)]
        public Visibility WatermarkVisibility
        {
            set 
            {
                //CoerceWatermarkVisibility(this, value);
                SetValue(WatermarkVisibilityProperty, value); 
            }
            get { return (Visibility)GetValue(WatermarkVisibilityProperty); }
        }

        private static object CoerceWatermarkVisibility(DependencyObject d, object baseValue)
        {
            EditorBase editorBase = (EditorBase)d;

            if (editorBase.WatermarkTextIsVisible && (((Visibility)baseValue) == Visibility.Visible))
            {
                editorBase.ContentElementVisibility = Visibility.Collapsed;
                return Visibility.Visible;
            }
            else
            {
                editorBase.ContentElementVisibility = Visibility.Visible;
                return Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Identifies the WaterMarkVisibility dependency property.
        /// </summary>
#if WPF
        public static DependencyProperty WatermarkVisibilityProperty = 
            DependencyProperty.Register("WatermarkVisibility", typeof(Visibility), typeof(EditorBase), new PropertyMetadata(Visibility.Collapsed,OnWatermarkVisibilityPropertyChanged,new CoerceValueCallback(CoerceWatermarkVisibility)));

#endif
#if SILVERLIGHT
        public static DependencyProperty WatermarkVisibilityProperty = 
            DependencyProperty.Register("WatermarkVisibility", typeof(Visibility), typeof(EditorBase), new PropertyMetadata(Visibility.Collapsed));
#endif
        public static void OnWatermarkVisibilityPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            EditorBase _editorbase= (EditorBase)obj;
            if (_editorbase != null)
                _editorbase.OnWatermarkVisibilityPropertyChanged(args);
        }

        protected void OnWatermarkVisibilityPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
#if SILVERLIGHT
            object _value = CoerceWatermarkVisibility(this, this.WatermarkVisibility);
            if (this.WatermarkVisibility != (Visibility)_value)
            {
                this.WatermarkVisibility = (Visibility)_value;
            }
#endif
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [BrowsableAttribute(false)]
        public Visibility ContentElementVisibility
        {
            get { return (Visibility)GetValue(ContentElementVisibilityProperty); }
            set { SetValue(ContentElementVisibilityProperty, value); }
        }

        public static readonly DependencyProperty ContentElementVisibilityProperty =
            DependencyProperty.Register("ContentElementVisibility", typeof(Visibility), typeof(EditorBase), new PropertyMetadata(Visibility.Visible));


        public Brush WatermarkTextForeground
        {
            get { return (Brush)GetValue(WatermarkTextForegroundProperty); }
            set { SetValue(WatermarkTextForegroundProperty, value); }
        }

        public static readonly DependencyProperty WatermarkTextForegroundProperty =
            DependencyProperty.Register("WatermarkTextForeground", typeof(Brush), typeof(EditorBase), new PropertyMetadata(new SolidColorBrush()));

        [EditorBrowsable(EditorBrowsableState.Never)]
        [BrowsableAttribute(false)]
        public Brush WatermarkBackground
        {
            get { return (Brush)GetValue(WatermarkBackgroundProperty); }
            set { SetValue(WatermarkBackgroundProperty, value); }
        }

        public static readonly DependencyProperty WatermarkBackgroundProperty =
            DependencyProperty.Register("WatermarkBackground", typeof(Brush), typeof(EditorBase), new PropertyMetadata(new SolidColorBrush()));

        public double WatermarkOpacity
        {
            get { return (double)GetValue(WatermarkOpacityProperty); }
            set { SetValue(WatermarkOpacityProperty, value); }
        }

        public static readonly DependencyProperty WatermarkOpacityProperty =
            DependencyProperty.Register("WatermarkOpacity", typeof(double), typeof(EditorBase), new PropertyMetadata((double)0.5));

        public bool WatermarkTextIsVisible
        {
            get { return (bool)GetValue(WatermarkTextIsVisibleProperty); }
            set { SetValue(WatermarkTextIsVisibleProperty, value); }
        }

        public static readonly DependencyProperty WatermarkTextIsVisibleProperty =
            DependencyProperty.Register("WatermarkTextIsVisible", typeof(bool), typeof(EditorBase), new PropertyMetadata(false));


        protected override void OnLostFocus(RoutedEventArgs e)
        {
#if SILVERLIGHT
            IsFocused = false;
#endif
            SetForeground();
            //SetBackground();
            base.OnLostFocus(e);
            if (this.IsNull)
            {
                this.WatermarkVisibility = Visibility.Visible;
            }
        }

#if WPF
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

        protected override void OnGotFocus(RoutedEventArgs e)
        {
#if SILVERLIGHT
            IsFocused = true;
#endif
            this.SetForeground();
#if SILVERLIGHT
            base.OnGotFocus(e);
#endif
            this.WatermarkVisibility = Visibility.Collapsed;
            this.SelectAll();
        }

        #endregion

        #region Property ChangedCallbacks

        public static void OnIsUndoEnabledChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            EditorBase e = (EditorBase)obj;
            if (e != null)
            {
                e.OnIsUndoEnabledChanged(args);
            }
        }

        protected void OnIsUndoEnabledChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.IsUndoEnabledChanged != null)
            {
                this.IsUndoEnabledChanged(this, args);
            }
        }

        public static void OnNegativeForegroundChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            EditorBase e = (EditorBase)obj;
            if (e != null)
            {
                e.OnNegativeForegroundChanged(args);
            }
        }

        protected void OnNegativeForegroundChanged(DependencyPropertyChangedEventArgs args)
        {
            if (NegativeForegroundChanged != null)
            {
                this.NegativeForegroundChanged(this, args);
            }
        }

        public static void OnEnterToMoveNextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            EditorBase editorbase = (EditorBase)obj;
            if (editorbase != null)
            {
                editorbase.OnEnterToMoveNextChanged(args);
            }
        }

        protected void OnEnterToMoveNextChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.EnterToMoveNextChanged != null)
                this.EnterToMoveNextChanged(this, args);
        }

        public static void OnForegroundChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (((EditorBase)obj) != null)
            {
                ((EditorBase)obj).OnForegroundChanged(args);
            }
        }

        protected void OnForegroundChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.IsZero && this.ApplyZeroColor)
            {
                return;
            }
            else if (this.IsNegative && this.ApplyNegativeForeground)
            {
                return;
            }
            //this.PositiveForeground = this.EditorForeground;
        }

        public static void OnIsNegativeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (((EditorBase)obj) != null)
            {
                ((EditorBase)obj).OnIsNegativeChanged(args);
            }
        }

        protected void OnIsNegativeChanged(DependencyPropertyChangedEventArgs args)
        {
            this.SetForeground();
            if (this.IsValueNegativeChanged != null)
                this.IsValueNegativeChanged(this, args);
        }

        public static void OnIsZeroChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (((EditorBase)obj) != null)
            {
                ((EditorBase)obj).OnIsZeroChanged(args);
            }
        }

        protected void OnIsZeroChanged(DependencyPropertyChangedEventArgs args)
        {
            this.SetForeground();
        }

        public static void OnIsNullChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (((EditorBase)obj) != null)
            {
                ((EditorBase)obj).OnIsNullChanged(args);
            }
        }

        protected void OnIsNullChanged(DependencyPropertyChangedEventArgs args)
        {
        }

        /// <summary>
        /// Called when [culture changed].
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        public static void OnCultureChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if ((EditorBase)obj != null)
            {
                ((EditorBase)obj).OnCultureChanged(args);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:CultureChanged"/> event.
        /// </summary>
        /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        protected void OnCultureChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.CultureChanged != null)
                this.CultureChanged(this, args);
            this.OnCultureChanged();
        }
        
        /// <summary>
        /// Called when [number format changed].
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        public static void OnNumberFormatChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if ((EditorBase)obj != null)
            {
                ((EditorBase)obj).OnNumberFormatChanged(args);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:NumberFormatChanged"/> event.
        /// </summary>
        /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        protected void OnNumberFormatChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.NumberFormatChanged != null)
                this.NumberFormatChanged(this, args);
            this.OnNumberFormatChanged();
        }

        public static void OnTextSelectionOnFocusChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            EditorBase e = (EditorBase)obj;
            if (e != null)
            {
                e.OnTextSelectionOnFocusChanged(args);
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


        public bool IsScrollingOnCircle
        {
            get { return (bool)GetValue(IsScrollingOnCircleProperty); }
            set { SetValue(IsScrollingOnCircleProperty, value); }
        }

        public static readonly DependencyProperty IsScrollingOnCircleProperty =
            DependencyProperty.Register("IsScrollingOnCircle", typeof(bool), typeof(EditorBase), new PropertyMetadata(true));

        

        public bool EnterToMoveNext
        {
            get { return (bool)GetValue(EnterToMoveNextProperty); }
            set { SetValue(EnterToMoveNextProperty, value); }
        }

        public static readonly DependencyProperty EnterToMoveNextProperty =
            DependencyProperty.Register("EnterToMoveNext", typeof(bool), typeof(EditorBase), new PropertyMetadata(true, OnEnterToMoveNextChanged));

        public bool TextSelectionOnFocus
        {
            get { return (bool)GetValue(TextSelectionOnFocusProperty); }
            set { SetValue(TextSelectionOnFocusProperty, value); }
        }

        public static readonly DependencyProperty TextSelectionOnFocusProperty =
            DependencyProperty.Register("TextSelectionOnFocus", typeof(bool), typeof(EditorBase), new PropertyMetadata(true, OnTextSelectionOnFocusChanged));

#if WPF
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
#endif
#if SILVERLIGHT       
        /// <summary>
        /// Called when <see cref="E:System.Windows.UIElement.KeyDown"/> event occurs.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
#endif
            if (e.Key == Key.Enter)
            {
                if (this.SelectionStart + 1 <= this.MaskedText.Length)
                    this.SelectionStart = this.SelectionStart + 1;
            }
            if (ModifierKeys.Control == Keyboard.Modifiers)
            {
                if (e.Key == Key.C)
                {
                   
                }
                if (e.Key == Key.V)
                {
                    
                }
                if (e.Key == Key.X)
                {
                   
                }
            }

        }

       

        #region VirtualMethods

        internal virtual void OnCultureChanged()
        {
        }

        internal virtual void OnNumberFormatChanged()
        {
        }
        #endregion

       

        [Obsolete("Property will not help due to internal arhitecture changes")]
        public bool IsCaretAnimationEnabled
        {
            get { return (bool)GetValue(IsCaretAnimationEnabledProperty); }
            set { SetValue(IsCaretAnimationEnabledProperty, value); }
        }

        public static readonly DependencyProperty IsCaretAnimationEnabledProperty =
            DependencyProperty.Register("IsCaretAnimationEnabled", typeof(bool), typeof(EditorBase), new PropertyMetadata(false));


       // [Obsolete("Use SelectionStart Property")]
       new internal int CaretIndex
        {
            get 
            {
                return SelectionStart;
                //return (int)GetValue(CaretIndexProperty); 
            }
            set
            {
                this.SelectionStart = value;
                SetValue(CaretIndexProperty, value);
            }
        }

        public  static readonly DependencyProperty CaretIndexProperty =
            DependencyProperty.Register("CaretIndex", typeof(int), typeof(EditorBase), new PropertyMetadata((int)0));

        
    }

}
