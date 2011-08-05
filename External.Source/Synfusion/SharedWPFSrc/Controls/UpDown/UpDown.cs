// <copyright file="UpDown.cs" company="Syncfusion">
// Copyright Syncfusion Inc. 2001 - 2009. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
// </copyright>

using System.Diagnostics;
using System.Globalization;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Windows.Input;
using System.Windows.Media;
using Syncfusion.Windows.Tools.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using Syncfusion.Windows.Shared;
using System.ComponentModel;
using Syncfusion.Licensing;

#if SILVERLIGHT
namespace Syncfusion.Windows.Controls
#else
namespace Syncfusion.Windows.Shared
#endif
{
#if WPF
    [SkinType(SkinVisualStyle = Skin.Office2007Blue,
      Type = typeof(UpDown), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/UpDown/Themes/Office2007BlueStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2007Black,
    Type = typeof(UpDown), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/UpDown/Themes/Office2007BlackStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2007Silver,
    Type = typeof(UpDown), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/UpDown/Themes/Office2007SilverStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2010Blue,
     Type = typeof(UpDown), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/UpDown/Themes/Office2010BlueStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2010Black,
    Type = typeof(UpDown), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/UpDown/Themes/Office2010BlackStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2010Silver,
    Type = typeof(UpDown), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/UpDown/Themes/Office2010SilverStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2003,
    Type = typeof(UpDown), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/UpDown/Themes/Office2003Style.xaml")]
    [SkinType(SkinVisualStyle = Skin.Blend,
    Type = typeof(UpDown), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/UpDown/Themes/BlendStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.SyncOrange,
    Type = typeof(UpDown), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/UpDown/Themes/SyncOrangeStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.ShinyRed,
    Type = typeof(UpDown), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/UpDown/Themes/ShinyRedStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.ShinyBlue,
    Type = typeof(UpDown), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/UpDown/Themes/ShinyBlueStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Default,
    Type = typeof(UpDown), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/UpDown/Themes/Generic.xaml")]
      [SkinType(SkinVisualStyle = Skin.VS2010 ,
    Type = typeof(UpDown), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/UpDown/Themes/VS2010Style.xaml")]

#endif
#if SILVERLIGHT
    public class NumericUpDown : Control
#else
#if SyncfusionFramework4_0
    [System.ComponentModel.DesignTimeVisible(true)]
#endif
    public class UpDown : Control
#endif
    {

        #region constants
        /// <summary>
        /// Default number format.
        /// </summary>
        private const string DEF_NUMBERFORMAT = "N";
        #endregion

        #region private fields
        /// <summary>
        /// Command for down button.
        /// </summary>
#if WPF
        public static RoutedCommand m_downValue;
#endif

        /// <summary>
        /// Command for up button.
        /// </summary>
#if WPF 
        public static RoutedCommand m_upValue;
#endif
        /// <summary>
        /// Contains the UpDown Control Border.
        /// </summary>
        private Border border;
      
#if WPF
        /// <summary>
        /// Contains the UpDown Control DoubleTextBox.
        /// </summary>
        private DoubleTextBox t1;

        /// <summary>
        /// Contains the UpDown Control DoubleTextBox.
        /// </summary>
        private DoubleTextBox secondBlock;
       
#endif
        /// <summary>
        /// Contains the UpDown Control DoubleTextBox.
        /// </summary>
        private DoubleTextBox textbox;

        private TextBox Nulltextbox;

        /// <summary>
        /// Contains the UpDown Control Up Button.
        /// </summary>
        private RepeatButton Upbutton;

        /// <summary>
        /// Contains the UpDown Control Down Button.
        /// </summary>
        private RepeatButton Downbutton;

#if SILVERLIGHT
        /// <summary>
        /// Contains the NumericUpDown Control TextBox.
        /// </summary>
        private TextBox secondBlock;

        /// <summary>
        /// Contains the NumericUpDown Control TextBox.
        /// </summary>
        private TextBox t1;

        /// <summary>
        /// Contains the NumericUpDown Control TextBox.
        /// </summary>
        private TextBox block;

        /// <summary>
        /// Contains the NumericUpDown Control TextBox.
        /// </summary>
        private TextBox block2;

        /// <summary>
        /// Contains the NumericUpDown Control RectangleGeometry.
        /// </summary>
        private RectangleGeometry clipgeo;
#endif
#if WPF
        /// <summary>
        /// Current Control Value
        /// </summary>
        private double? m_value;

        /// <summary>
        /// Previous Control Value
        /// </summary>
        private double? m_exvalue;
#else
        /// <summary>
        /// Current Control Value
        /// </summary>
        private double m_value;

        /// <summary>
        /// Previous Control Value
        /// </summary>
        private double m_exvalue;
#endif
        #endregion

        #region Class Initialize/Finalize methods
#if WPF
        /// <summary>
        /// Initializes static members of the <see cref="UpDown"/> class.
        /// </summary>
        static UpDown()
        {
            //// This OverrideMetadata call tells the system that this element wants to provide a style that is different than its base class.
            //// This style is defined in themes\generic.xaml
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UpDown), new FrameworkPropertyMetadata(typeof(UpDown)));
            m_downValue = new RoutedCommand();
            m_upValue = new RoutedCommand();
            //EnvironmentTest.ValidateLicense(typeof(UpDown));
        }
#endif

#if SILVERLIGHT
        /// <summary>
        /// Initializes a new instance of the <see cref="NumericUpDown"/> class.
        /// </summary>
        public NumericUpDown()
        {
        DefaultStyleKey = typeof(NumericUpDown);
#else
        /// <summary>
        /// Initializes a new instance of the <see cref="UpDown"/> class.
        /// </summary>
        public UpDown()
        {

            DefaultStyleKey = typeof(UpDown);
           
#endif
#if WPF
            CommandBinding downValueBinding = new CommandBinding(m_downValue);
            downValueBinding.Executed += new ExecutedRoutedEventHandler(ChangeDownValue);
            CommandBinding upValueBinding = new CommandBinding(m_upValue);
            upValueBinding.Executed += new ExecutedRoutedEventHandler(ChangeUpValue);
            CommandBindings.Add(downValueBinding);
            CommandBindings.Add(upValueBinding);
#endif
        }
        #endregion

        #region Public events
        /// <summary>
        /// Event that is raised when <see cref="AlloEdit"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback AllowEditChanged;

        /// <summary>
        /// Event that is raised when <see cref="Step"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback StepChanged;

        /// <summary>
        /// Event that is raised when <see cref="UseNullOption"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback UseNullOptionChanged;

        /// <summary>
        /// Event that is raised when <see cref="Value"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback ValueChanged;

        public delegate void ValueChangingEventHandler(object sender, ValueChangingEventArgs e);

        public event ValueChangingEventHandler ValueChanging;

        /// <summary>
        /// Event that is raised when <see cref="MinValue"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback MinValueChanged;

        /// <summary>
        /// Event that is raised when <see cref="MaxValue"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback MaxValueChanged;

        /// <summary>
        /// Event that is raised when <see cref="NumberFormatInfo"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback NumberFormatInfoChanged;

        /// <summary>
        /// Event that is raised when <see cref="ZeroColor"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback ZeroColorChanged;

        /// <summary>
        /// Event that is raised when <see cref="NegativeForeground"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback NegativeForegroundChanged;

        /// <summary>
        /// Event that is raised when <see cref="MinValidation"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback MinValidationChanged;

        /// <summary>
        /// Event that is raised when <see cref="MaxValidation"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback MaxValidationChanged;

#if WPF
        /// <summary>
        /// Event that is raised when <see cref="ControlTemplate"/> property is
        /// changed.
        /// </summary>
        [Obsolete("Event will not help due to internal arhitecture changes")]
        public event PropertyChangedCallback CursorTemplateChanged;       

        /// <summary>
        /// Event that is raised when <see cref="IsValueNegativeChanged"/> property is changed.
        /// </summary>
        [Obsolete("Event will not help due to internal arhitecture changes")]
        public event PropertyChangedCallback IsValueNegativeChanged;

        public event PropertyChangedCallback NullValueTextChanged;
#endif

        /// <summary>
        /// Event that is raised when <see cref="FocusedBackground"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback FocusedBackgroundChanged;
      
        /// <summary>
        /// Event that is raised when <see cref="FocusedForeground"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback FocusedForegroundChanged;

        /// <summary>
        /// Event that is raised when <see cref="FocusedBorderBrush"/> property is changed.
        /// </summary>
        public event PropertyChangedCallback FocusedBorderBrushChanged;

        #endregion

        #region Properties
#if WPF
        /// <summary>
        /// Gets or sets the animation shift.
        /// </summary>
        /// <value>The animation shift.</value>
        [Obsolete("Property will not help due to internal arhitecture changes")]       
        internal double AnimationShift
        {
            get { return (double)GetValue(AnimationShiftProperty); }
            set { SetValue(AnimationShiftProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is value negative.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is value negative; otherwise, <c>false</c>.
        /// </value>
        [Obsolete("Property will not help due to internal arhitecture changes")]
        [System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public bool IsValueNegative
        {
            get { return (bool)GetValue(IsValueNegativeProperty); }
            set { SetValue(IsValueNegativeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the cursor background.
        /// </summary>
        /// <value>The cursor background.</value>
        [Obsolete("Property will not help due to internal arhitecture changes")]
        public Brush CursorBackground
        {
            get { return (Brush)GetValue(CursorBackgroundProperty); }
            set { SetValue(CursorBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the cursor border brush.
        /// </summary>
        /// <value>The cursor border brush.</value>
        [Obsolete("Property will not help due to internal arhitecture changes")]
        public Brush CursorBorderBrush
        {
            get { return (Brush)GetValue(CursorBorderBrushProperty); }
            set { SetValue(CursorBorderBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the width of the cursor.
        /// </summary>
        /// <value>The width of the cursor.</value>
        [Obsolete("Property will not help due to internal arhitecture changes")]
        public double CursorWidth
        {
            get { return (double)GetValue(CursorWidthProperty); }
            set { SetValue(CursorWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the cursor border thickness.
        /// </summary>
        /// <value>The cursor border thickness.</value>
        [Obsolete("Property will not help due to internal arhitecture changes")]
        public Thickness CursorBorderThickness
        {
            get { return (Thickness)GetValue(CursorBorderThicknessProperty); }
            set { SetValue(CursorBorderThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the cursor template. This is a dependency property.
        /// </summary>
        /// <value>
        /// Type: <see cref="ControlTemplate"/>
        /// Default value is null.
        /// </value>
        /// <seealso cref="ControlTemplate"/>
        [Obsolete("Property will not help due to internal arhitecture changes")]
        public ControlTemplate CursorTemplate
        {
            get { return (ControlTemplate)GetValue(CursorTemplateProperty); }
            set { SetValue(CursorTemplateProperty, value); }
        }

        /// <summary>
        /// Checks for cursor visibility.
        /// </summary>
        /// <value>The cursor visibility.</value>
        [Obsolete("Property will not help due to internal arhitecture changes")]
        [System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public bool CursorVisible
        {
            get { return (bool)GetValue(CursorVisibleProperty); }
            set { SetValue(CursorVisibleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the cursor position.
        /// </summary>
        /// <value>The cursor position.</value>
        [Obsolete("Property will not help due to internal arhitecture changes")]
        internal Thickness CursorPosition
        {
            get { return (Thickness)GetValue(CursorPositionProperty); }
            set { SetValue(CursorPositionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selection brush.
        /// </summary>
        /// <value>The selection brush.</value>
        [Obsolete("Property will not help due to internal arhitecture changes")]
        public Brush SelectionBrush
        {
            get { return (Brush)GetValue(SelectionBrushProperty); }
            set { SetValue(SelectionBrushProperty, value); }
        }
#endif
        /// <summary>
        /// Gets or sets the UpDown Foreground
        /// </summary>
        /// <value>The UpDown Foreground.</value>
        [System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public Brush UpDownForeground
        {
            get { return (Brush)GetValue(UpDownForegroundProperty); }
            set { SetValue(UpDownForegroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the UpDown Background
        /// </summary>
        /// <value>The UpDown Background.</value>
        [System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public Brush UpDownBackground
        {
            get { return (Brush)GetValue(UpDownBackgroundProperty); }
            set { SetValue(UpDownBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the UpDown BorderBrush
        /// </summary>
        /// <value>The UpDown BorderBrush.</value>
        [System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public Brush UpDownBorderBrush
        {
            get { return (Brush)GetValue(UpDownBorderBrushProperty); }
            set { SetValue(UpDownBorderBrushProperty, value); }
        }

        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        public bool ApplyZeroColor
        {
            get { return (bool)GetValue(ApplyZeroColorProperty); }
            set { SetValue(ApplyZeroColorProperty, value); }
        }
        
        public bool EnableNegativeColors
        {
            get { return (bool)GetValue(EnableNegativeColorsProperty); }
            set { SetValue(EnableNegativeColorsProperty, value); }
        }

        public CultureInfo Culture
        {
            get { return (CultureInfo)GetValue(CultureProperty); }
            set { SetValue(CultureProperty, value); }
        }

        public bool EnableFocusedColors
        {
            get { return (bool)GetValue(EnableFocusedColorsProperty); }
            set { SetValue(EnableFocusedColorsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the background when control is focused. This is a dependency property. 
        /// </summary>
        /// <value>
        /// Type: <see cref="Brush"/>
        /// Default value is Brushes.White.
        /// </value>
        /// <seealso cref="Brush"/>
        public Brush FocusedBackground
        {
            get { return (Brush)GetValue(FocusedBackgroundProperty); }
            set { SetValue(FocusedBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the foreground when control is focused. This is a dependency property. 
        /// </summary>
        /// <value>
        /// Type: <see cref="Brush"/>
        /// Default value is Brushes.Black.
        /// </value>
        /// <seealso cref="Brush"/>
        public Brush FocusedForeground
        {
            get { return (Brush)GetValue(FocusedForegroundProperty); }
            set { SetValue(FocusedForegroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the borderBrush when control is focused. This is a dependency property. 
        /// </summary>
        /// <value>
        /// Type: <see cref="Brush"/>
        /// Default value is Brushes.Black.
        /// </value>
        /// <seealso cref="Brush"/>
        public Brush FocusedBorderBrush
        {
            get { return (Brush)GetValue(FocusedBorderBrushProperty); }
            set { SetValue(FocusedBorderBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets background of the control when it's value is negative. This is a dependency property.
        /// </summary>
        /// <value>Type: <see cref="Brush"/></value>
        public Brush NegativeBackground
        {
            get { return (Brush)GetValue(NegativeBackgroundProperty); }
            set { SetValue(NegativeBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the negative border brush.
        /// </summary>
        /// <value>The negative border brush.</value>
        public Brush NegativeBorderBrush
        {
            get { return (Brush)GetValue(NegativeBorderBrushProperty); }
            set { SetValue(NegativeBorderBrushProperty, value); }
        }

        public bool AllowEdit
        {
            get { return (bool)GetValue(AllowEditProperty); }
            set { SetValue(AllowEditProperty, value); }
        }

        public MinValidation MinValidation
        {
            get { return (MinValidation)GetValue(MinValidationProperty); }
            set { SetValue(MinValidationProperty, value); }
        }

        public MaxValidation MaxValidation
        {
            get { return (MaxValidation)GetValue(MaxValidationProperty); }
            set { SetValue(MaxValidationProperty, value); }
        }

        public bool MinValueOnExceedMinDigit
        {
            get { return (bool)GetValue(MinValueOnExceedMinDigitProperty); }
            set { SetValue(MinValueOnExceedMinDigitProperty, value); }
        }

        public bool MaxValueOnExceedMaxDigit
        {
            get { return (bool)GetValue(MaxValueOnExceedMaxDigitProperty); }
            set { SetValue(MaxValueOnExceedMaxDigitProperty, value); }
        }

        /// <summary>
        /// Gets or sets foreground of the control when it's value is negative. This is a dependency property.
        /// </summary>
        /// <value>Type: <see cref="Brush"/></value>
        public Brush NegativeForeground
        {
            get { return (Brush)GetValue(NegativeForegroundProperty); }
            set { SetValue(NegativeForegroundProperty, value); }
        }

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
        /// Gets or sets the <see cref="System.Globalization.NumberFormatInfo"/> object that is used for formatting the number value.
        /// This is a dependency property.
        /// </summary>
        /// <value>
        /// Type: <see cref="NumberFormatInfo"/>
        /// Default value is null.
        /// </value>
        /// <seealso cref="NumberFormatInfo"/>
        /// <example>
        /// <code>
        ///    // Create a new instance of the UpDown
        ///    UpDown upDown1 = new UpDown();
        ///    // Add UpDown to grid
        ///    grid1.Children.Add( upDown1 );
        ///    // Set value of the control
        ///    upDown1.Value = 1000000;
        ///    // Set count of decimal digits that should be displayed 
        ///    upDown1.NumberFormatInfo.NumberDecimalDigits = 2;
        ///    // Set decimal separator
        ///    upDown1.NumberFormatInfo.NumberDecimalSeparator = ",";
        ///    // Set string that separates groups of digits to the left of the decimal
        ///    upDown1.NumberFormatInfo.NumberGroupSeparator = ":";        
        /// Result:
        ///  Value will be displayed like '1:000:000,00'  
        ///  </code>
        /// </example>
        public NumberFormatInfo NumberFormatInfo
        {
            get { return (NumberFormatInfo)GetValue(NumberFormatInfoProperty); }
            set { SetValue(NumberFormatInfoProperty, value); }
        }

        public double CornerRadius
        {
            get { return (double)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public double? NullValue
        {
            get { return (double?)GetValue(NullValueProperty); }
            set { SetValue(NullValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value assigned to the control. This is a dependency property.
        /// </summary>
        /// <value>
        /// Type: <see cref="double"/>
        /// Default value is 0.
        /// </value>
        /// <seealso cref="double"/>
        public double? Value
        {
            get { return (double?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        ///  Gets or sets the minimum value for the control. This is a dependency property.
        /// </summary>
        /// <value>
        /// Type: <see cref="double"/>
        /// Default value is double.MinValue.
        /// </value>
        /// <seealso cref="double"/>
        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum value for the control. This is a dependency property.
        /// </summary>
        /// <value>
        /// Type: <see cref="double"/>
        /// Default value is double.MaxValue.
        /// </value>
        /// <seealso cref="double"/>
        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the step to increment or decrement the value of the control
        /// when the up or down button is clicked. This is a dependency property.
        /// </summary>
        /// <value>
        /// Type: <see cref="double"/>
        /// Default value is 1.
        /// </value>
        /// <seealso cref="double"/>
        public double Step
        {
            get { return (double)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }

        /// <summary>
        /// Gets or sets the animation speed of value change. This is a dependency property.
        /// </summary>
        /// <value>
        /// Type: <see cref="double"/>
        /// Default value is 0.1.
        /// </value>
        /// <seealso cref="double"/>
        public double AnimationSpeed
        {
            get { return (double)GetValue(AnimationSpeedProperty); }
            set { SetValue(AnimationSpeedProperty, value); }
        }

        #endregion

        #region Dependency Properties
#if WPF



        public string NullValueText
        {
            get { return (string)GetValue(NullValueTextProperty); }
            set { SetValue(NullValueTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NullValueText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NullValueTextProperty =
            DependencyProperty.Register("NullValueText", typeof(string), typeof(UpDown), new FrameworkPropertyMetadata(string.Empty,OnNullValueTextChanged));
        
        
        /// <summary>
        /// Identifies <see cref="AnimationShift"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AnimationShiftProperty =
            DependencyProperty.Register("AnimationShift", typeof(double), typeof(UpDown), new UIPropertyMetadata(0d));

        /// <summary>
        /// Identifies the <see cref="IsValueNegative"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsValueNegativeProperty =
            DependencyProperty.Register("IsValueNegative", typeof(bool), typeof(UpDown), new PropertyMetadata(false, OnIsValueNegativeChanged));

        /// <summary>
        /// Identifies <see cref="CursorBackground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CursorBackgroundProperty =
            DependencyProperty.Register("CursorBackground", typeof(Brush), typeof(UpDown), new PropertyMetadata(null));

        /// <summary>
        /// Identifies <see cref="CursorBorderBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CursorBorderBrushProperty =
            DependencyProperty.Register("CursorBorderBrush", typeof(Brush), typeof(UpDown), new PropertyMetadata(null));

        /// <summary>
        /// Identifies <see cref="CursorWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CursorWidthProperty =
            DependencyProperty.Register("CursorWidth", typeof(double), typeof(UpDown), new PropertyMetadata(null));

        /// <summary>
        /// Identifies <see cref="CursorBorderThickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CursorBorderThicknessProperty =
            DependencyProperty.Register("CursorBorderThickness", typeof(Thickness), typeof(UpDown), new PropertyMetadata(null));

        /// <summary>
        /// Identifies <see cref="CursorTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CursorTemplateProperty =
            DependencyProperty.Register("CursorTemplate", typeof(ControlTemplate), typeof(UpDown), new PropertyMetadata(null, OnCursorTemplateChanged));

        /// <summary>
        /// Identifies <see cref="CursorVisible"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CursorVisibleProperty =
            DependencyProperty.Register("CursorVisible", typeof(bool), typeof(UpDown), new PropertyMetadata(null));

        /// <summary>
        /// Identifies <see cref="CursorPosition"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CursorPositionProperty =
            DependencyProperty.Register("CursorPosition", typeof(Thickness), typeof(UpDown), new PropertyMetadata(null));

        /// <summary>
        /// Identifies <see cref="SelectionBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectionBrushProperty =
            DependencyProperty.Register("SelectionBrush", typeof(Brush), typeof(UpDown), new PropertyMetadata(null));
#endif       

#if WPF
        public static readonly DependencyProperty UpDownForegroundProperty =
            DependencyProperty.Register("UpDownForeground", typeof(Brush), typeof(UpDown), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));
#else
        public static readonly DependencyProperty UpDownForegroundProperty =
            DependencyProperty.Register("UpDownForeground", typeof(Brush), typeof(NumericUpDown), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));
#endif       

#if WPF
        public static readonly DependencyProperty UpDownBackgroundProperty =
            DependencyProperty.Register("UpDownBackground", typeof(Brush), typeof(UpDown), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));
#else
        public static readonly DependencyProperty UpDownBackgroundProperty =
            DependencyProperty.Register("UpDownBackground", typeof(Brush), typeof(NumericUpDown), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));
#endif       

#if WPF
        public static readonly DependencyProperty UpDownBorderBrushProperty =
            DependencyProperty.Register("UpDownBorderBrush", typeof(Brush), typeof(UpDown), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));
#else
        public static readonly DependencyProperty UpDownBorderBrushProperty =
            DependencyProperty.Register("UpDownBorderBrush", typeof(Brush), typeof(NumericUpDown), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));
#endif
      
#if WPF
        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(UpDown), new PropertyMetadata(TextAlignment.Center));
#else
        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(NumericUpDown), new PropertyMetadata(TextAlignment.Center));
#endif       

#if WPF
        public static readonly DependencyProperty ApplyZeroColorProperty =
            DependencyProperty.Register("ApplyZeroColor", typeof(bool), typeof(UpDown), new PropertyMetadata(true));
#else
        public static readonly DependencyProperty ApplyZeroColorProperty =
            DependencyProperty.Register("ApplyZeroColor", typeof(bool), typeof(NumericUpDown), new PropertyMetadata(true));
#endif      

#if WPF
        public static readonly DependencyProperty EnableNegativeColorsProperty =
            DependencyProperty.Register("EnableNegativeColors", typeof(bool), typeof(UpDown), new PropertyMetadata(true));
#else 
        public static readonly DependencyProperty EnableNegativeColorsProperty =
            DependencyProperty.Register("EnableNegativeColors", typeof(bool), typeof(NumericUpDown), new PropertyMetadata(true));
#endif
       
#if WPF
        public static readonly DependencyProperty CultureProperty =
            DependencyProperty.Register("Culture", typeof(CultureInfo), typeof(UpDown), new PropertyMetadata(CultureInfo.CurrentCulture));
#else
        public static readonly DependencyProperty CultureProperty =
           DependencyProperty.Register("Culture", typeof(CultureInfo), typeof(NumericUpDown), new PropertyMetadata(CultureInfo.CurrentCulture));
#endif       
      
#if WPF
        public static readonly DependencyProperty EnableFocusedColorsProperty =
            DependencyProperty.Register("EnableFocusedColors", typeof(bool), typeof(UpDown), new PropertyMetadata(false));
#else
        public static readonly DependencyProperty EnableFocusedColorsProperty =
            DependencyProperty.Register("EnableFocusedColors", typeof(bool), typeof(NumericUpDown), new PropertyMetadata(false));
#endif      
       
        /// <summary>
        /// Identifies the <see cref="FocusedBackground"/> dependency property.
        /// </summary>
#if WPF
        public static readonly DependencyProperty FocusedBackgroundProperty =
            DependencyProperty.Register("FocusedBackground", typeof(Brush), typeof(UpDown), new PropertyMetadata(Brushes.White, OnFocusedBackgroundChanged));
#else
        public static readonly DependencyProperty FocusedBackgroundProperty =
            DependencyProperty.Register("FocusedBackground", typeof(Brush), typeof(NumericUpDown), new PropertyMetadata(null));
#endif       
      
        /// <summary>
        /// Identifies the <see cref="FocusedForeground"/> dependency property.
        /// </summary>
#if WPF
        public static readonly DependencyProperty FocusedForegroundProperty =
            DependencyProperty.Register("FocusedForeground", typeof(Brush), typeof(UpDown), new PropertyMetadata(Brushes.Black, OnFocusedForegroundChanged));
#else
        public static readonly DependencyProperty FocusedForegroundProperty =
            DependencyProperty.Register("FocusedForeground", typeof(Brush), typeof(NumericUpDown), new PropertyMetadata(null));
#endif

        /// <summary>
        /// Identifies the <see cref="FocusedBorderBrush"/> dependency property.
        /// </summary>
#if WPF
        public static readonly DependencyProperty FocusedBorderBrushProperty =
            DependencyProperty.Register("FocusedBorderBrush", typeof(Brush), typeof(UpDown), new PropertyMetadata(Brushes.Black, OnFocusedBorderBrushChanged));
#else
        public static readonly DependencyProperty FocusedBorderBrushProperty =
            DependencyProperty.Register("FocusedBorderBrush", typeof(Brush), typeof(NumericUpDown), new PropertyMetadata(null));
#endif

        /// <summary>
        /// Identifies the <see cref="NegativeBackground"/> dependency property.
        /// </summary>
#if WPF
           public static readonly DependencyProperty NegativeBackgroundProperty =
            DependencyProperty.Register("NegativeBackground", typeof(Brush), typeof(UpDown), new PropertyMetadata(null));
#else
        public static readonly DependencyProperty NegativeBackgroundProperty =
         DependencyProperty.Register("NegativeBackground", typeof(Brush), typeof(NumericUpDown), new PropertyMetadata(null));
#endif        
       
           /// <summary>
           /// Identifies the <see cref="NegativeBorderBrush"/> dependency property.
           /// </summary>
#if WPF
        public static readonly DependencyProperty NegativeBorderBrushProperty =
            DependencyProperty.Register("NegativeBorderBrush", typeof(Brush), typeof(UpDown), new PropertyMetadata(null));
#else
        public static readonly DependencyProperty NegativeBorderBrushProperty =
            DependencyProperty.Register("NegativeBorderBrush", typeof(Brush), typeof(NumericUpDown), new PropertyMetadata(null));
#endif       
       
#if WPF
        public static readonly DependencyProperty AllowEditProperty =
            DependencyProperty.Register("AllowEdit", typeof(bool), typeof(UpDown), new PropertyMetadata(true, new PropertyChangedCallback(OnAllowEditChanged)));
#else
        public static readonly DependencyProperty AllowEditProperty =
          DependencyProperty.Register("AllowEdit", typeof(bool), typeof(NumericUpDown), new PropertyMetadata(true, new PropertyChangedCallback(OnAllowEditChanged)));
#endif        
      
#if WPF
        public static readonly DependencyProperty MinValidationProperty =
            DependencyProperty.Register("MinValidation", typeof(MinValidation), typeof(UpDown), new PropertyMetadata(MinValidation.OnKeyPress, new PropertyChangedCallback(OnMinValidationChanged)));
#else
        public static readonly DependencyProperty MinValidationProperty =
            DependencyProperty.Register("MinValidation", typeof(MinValidation), typeof(NumericUpDown), new PropertyMetadata(MinValidation.OnKeyPress, new PropertyChangedCallback(OnMinValidationChanged)));
#endif
        
#if WPF
        public static readonly DependencyProperty MaxValidationProperty =
            DependencyProperty.Register("MaxValidation", typeof(MaxValidation), typeof(UpDown), new PropertyMetadata(MaxValidation.OnKeyPress, new PropertyChangedCallback(OnMaxValidationChanged)));
#else
        public static readonly DependencyProperty MaxValidationProperty =
            DependencyProperty.Register("MaxValidation", typeof(MaxValidation), typeof(NumericUpDown), new PropertyMetadata(MaxValidation.OnLostFocus, new PropertyChangedCallback(OnMaxValidationChanged)));
#endif
       
#if WPF
        public static readonly DependencyProperty MinValueOnExceedMinDigitProperty =
            DependencyProperty.Register("MinValueOnExceedMinDigit", typeof(bool), typeof(UpDown), new PropertyMetadata(true));
#else
        public static readonly DependencyProperty MinValueOnExceedMinDigitProperty =
            DependencyProperty.Register("MinValueOnExceedMinDigit", typeof(bool), typeof(NumericUpDown), new PropertyMetadata(true));
#endif
       
#if WPF
        public static readonly DependencyProperty MaxValueOnExceedMaxDigitProperty =
            DependencyProperty.Register("MaxValueOnExceedMaxDigit", typeof(bool), typeof(UpDown), new PropertyMetadata(true));
#else
        public static readonly DependencyProperty MaxValueOnExceedMaxDigitProperty =
            DependencyProperty.Register("MaxValueOnExceedMaxDigit", typeof(bool), typeof(NumericUpDown), new PropertyMetadata(true));
#endif       

        /// <summary>
        /// Identifies the <see cref="NegativeForeground"/> dependency property.
        /// </summary>
#if WPF
        public static readonly DependencyProperty NegativeForegroundProperty =
            DependencyProperty.Register("NegativeForeground", typeof(Brush), typeof(UpDown), new PropertyMetadata(new SolidColorBrush(Colors.Red), new PropertyChangedCallback(OnNegativeForegroundChanged)));
#else
        public static readonly DependencyProperty NegativeForegroundProperty =
            DependencyProperty.Register("NegativeForeground", typeof(Brush), typeof(NumericUpDown), new PropertyMetadata(new SolidColorBrush(Colors.Red), new PropertyChangedCallback(OnNegativeForegroundChanged)));
#endif       

#if WPF
        public static readonly DependencyProperty ZeroColorProperty =
            DependencyProperty.Register("ZeroColor", typeof(Brush), typeof(UpDown), new PropertyMetadata(new SolidColorBrush(Colors.Green), new PropertyChangedCallback(OnZeroColorChanged)));
#else
        public static readonly DependencyProperty ZeroColorProperty =
            DependencyProperty.Register("ZeroColor", typeof(Brush), typeof(NumericUpDown), new PropertyMetadata(new SolidColorBrush(Colors.Green), new PropertyChangedCallback(OnZeroColorChanged)));
#endif       

        /// <summary>
        /// Identifies <see cref="UseNullOption"/> dependency property.
        /// </summary>
#if WPF
        public static readonly DependencyProperty UseNullOptionProperty =
            DependencyProperty.Register("UseNullOption", typeof(bool), typeof(UpDown), new PropertyMetadata(false, new PropertyChangedCallback(OnUseNullOptionChanged)));
#else
        public static readonly DependencyProperty UseNullOptionProperty =
            DependencyProperty.Register("UseNullOption", typeof(bool), typeof(NumericUpDown), new PropertyMetadata(false, new PropertyChangedCallback(OnUseNullOptionChanged)));
#endif

        /// <summary>
        /// Identifies <see cref="NumberFormatInfo"/> dependency property.
        /// </summary>
#if WPF
        public static readonly DependencyProperty NumberFormatInfoProperty =
            DependencyProperty.Register("NumberFormatInfo", typeof(NumberFormatInfo), typeof(UpDown), new PropertyMetadata(null, new PropertyChangedCallback(OnNumberFormatInfoChanged)));
#else
        public static readonly DependencyProperty NumberFormatInfoProperty =
            DependencyProperty.Register("NumberFormatInfo", typeof(NumberFormatInfo), typeof(NumericUpDown), new PropertyMetadata(null, new PropertyChangedCallback(OnNumberFormatInfoChanged)));
#endif

       
#if WPF
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(double), typeof(UpDown), new PropertyMetadata(1d));
#else
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(double), typeof(NumericUpDown), new PropertyMetadata(1d));
#endif

      
#if WPF
        public static readonly DependencyProperty NullValueProperty =
            DependencyProperty.Register("NullValue", typeof(double?), typeof(UpDown), new PropertyMetadata(null));
#else
        public static readonly DependencyProperty NullValueProperty =
            DependencyProperty.Register("NullValue", typeof(double?), typeof(NumericUpDown), new PropertyMetadata(null));
#endif

        /// <summary>
        /// Identifies <see cref="Value"/> dependency property.
        /// </summary>
#if WPF
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double?), typeof(UpDown), new FrameworkPropertyMetadata(0d,FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnValueChanged)));
#else
         public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double?), typeof(NumericUpDown), new PropertyMetadata(0d, new PropertyChangedCallback(OnValueChanged)));
#endif

        /// <summary>
        /// Identifies <see cref="MinValue"/> dependency property.
        /// </summary>
#if WPF
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(double), typeof(UpDown), new PropertyMetadata(double.MinValue, new PropertyChangedCallback(OnMinValueChanged)));
#else
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(double), typeof(NumericUpDown), new PropertyMetadata(double.MinValue, new PropertyChangedCallback(OnMinValueChanged)));
#endif

        /// <summary>
        /// Identifies <see cref="MaxValue"/> dependency property.
        /// </summary>
#if WPF
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(UpDown), new PropertyMetadata(double.MaxValue, new PropertyChangedCallback(OnMaxValueChanged)));
#else
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(NumericUpDown), new PropertyMetadata(double.MaxValue, new PropertyChangedCallback(OnMaxValueChanged)));
#endif

        /// <summary>
        /// Identifies <see cref="Step"/> dependency property.
        /// </summary>
#if WPF
        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register("Step", typeof(double), typeof(UpDown), new PropertyMetadata(1d, new PropertyChangedCallback(OnStepChanged)));
#else
        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register("Step", typeof(double), typeof(NumericUpDown), new PropertyMetadata(0.1d, new PropertyChangedCallback(OnStepChanged)));
#endif

        /// <summary>
        /// Identifies <see cref="AnimationSpeed"/> dependency property.
        /// </summary>
#if WPF
        public static readonly DependencyProperty AnimationSpeedProperty =
            DependencyProperty.Register("AnimationSpeed", typeof(double), typeof(UpDown), new PropertyMetadata(.1d));
#else
        public static readonly DependencyProperty AnimationSpeedProperty =
            DependencyProperty.Register("AnimationSpeed", typeof(double), typeof(NumericUpDown), new PropertyMetadata(.5d));
#endif
        #endregion

        #region Static Methods

        /// <summary>
        /// Calls OnFocusedBackgroundChanged method of the instance, notifies of the dependency property value changes.
        /// </summary>
        /// <param name="d">Dependency object, the change occurs on.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> that contains the event data.</param>
        private static void OnFocusedBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpDown instance = (UpDown)d;
            instance.OnFocusedBackgroundChanged(e);
        }

        /// <summary>
        /// Calls OnFocusedForegroundChanged method of the instance, notifies of the dependency property value changes.
        /// </summary>
        /// <param name="d">Dependency object, the change occurs on.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> that contains the event data.</param>
        private static void OnFocusedForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpDown instance = (UpDown)d;
            instance.OnFocusedForegroundChanged(e);
        }

        /// <summary>
        /// Calls OnFocusedBorderBrushChanged method of the instance, notifies of the dependency property value changes.
        /// </summary>
        /// <param name="d">Dependency object, the change occurs on.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> that contains the event data.</param>
        private static void OnFocusedBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpDown instance = (UpDown)d;
            instance.OnFocusedBorderBrushChanged(e);
        }


#if WPF
        /// <summary>
        /// Called when [is value negative changed].
        /// </summary>
        /// <param name="d">The d value.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnIsValueNegativeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpDown instance = (UpDown)d;
            instance.OnIsValueNegativeChanged(e);
        }

        private static void OnNullValueTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpDown instance = (UpDown)d;
            instance.OnNullValueTextChanged(e);
        }

        /// <summary>
        /// Calls OnControlTemplateChanged method of the instance,
        /// notifies of the dependency property value changes.
        /// </summary>
        /// <param name="d">Dependency object, the change occurs on.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> that contains the event data.</param>
        private static void OnCursorTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpDown instance = (UpDown)d;
            instance.OnCursorTemplateChanged(e);
        }
#endif

        private static void OnAllowEditChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
#if WPF
            UpDown source = (UpDown)d;
#else
            NumericUpDown source = (NumericUpDown)d;
#endif
            source.OnAllowEditChanged(e);
        }
        
        private static void OnNegativeForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
#if WPF
            UpDown source = (UpDown)d;
#else
            NumericUpDown source = (NumericUpDown)d;
#endif
            source.OnNegativeForegroundChanged(e);
        }
        
        private static void OnMinValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
#if WPF
            UpDown source = (UpDown)d;
#else
            NumericUpDown source = (NumericUpDown)d;
#endif
            source.OnMinValueChanged(e);
        }

        /// <summary>
        /// Calls OnMaxValueChanged method of the instance, notifies of
        /// the dependency property value changes.
        /// </summary>
        /// <param name="d">Dependency object, the change occurs on.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> that contains the event data.</param>
        private static void OnMaxValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
#if WPF
            UpDown source = (UpDown)d;
#else
            NumericUpDown source = (NumericUpDown)d;
#endif
            source.OnMaxValueChanged(e);
        }
       
        private static void OnNumberFormatInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
#if WPF
            UpDown source = (UpDown)d;
#else
            NumericUpDown source = (NumericUpDown)d;
#endif
            source.OnNumberFormatInfoChanged(e);
        }
        
        private static void OnZeroColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
#if WPF
            UpDown source = (UpDown)d;
#else
            NumericUpDown source = (NumericUpDown)d;
#endif
            source.OnZeroColorChanged(e);
        }
        
        private static void OnMaxValidationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
#if WPF
            UpDown source = (UpDown)d;
#else
            NumericUpDown source = (NumericUpDown)d;
#endif
            source.OnMaxValidationChanged(e);
        }
        
        private static void OnMinValidationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
#if WPF
            UpDown source = (UpDown)d;
#else
            NumericUpDown source = (NumericUpDown)d;
#endif
            source.OnMinValidationChanged(e);
        }
        
        private static void OnUseNullOptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
#if WPF
            UpDown source = (UpDown)d;
#else
            NumericUpDown source = (NumericUpDown)d;
#endif
            source.OnUseNullOptionChanged(e);
        }
        
        private static void OnStepChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
#if WPF
            UpDown source = (UpDown)d;
#else
            NumericUpDown source = (NumericUpDown)d;
#endif
            source.OnStepChanged(e);
        }
       
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
#if WPF
            UpDown source = (UpDown)d;
#else
            NumericUpDown source = (NumericUpDown)d;
#endif
            source.OnValueChanged(e);
        }
        
#endregion

        #region Internal Methods

        /// <summary>
        /// Updates property value cache and raises <see cref="FocusedBackgroundChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> that contains the event data.</param>
        protected virtual void OnFocusedBackgroundChanged(DependencyPropertyChangedEventArgs e)
        {
            if (FocusedBackgroundChanged != null)
            {
                FocusedBackgroundChanged(this, e);
            }
        }

        /// <summary>
        /// Updates property value cache and raises <see cref="FocusedForegroundChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> that contains the event data.</param>
        protected virtual void OnFocusedForegroundChanged(DependencyPropertyChangedEventArgs e)
        {
            if (FocusedForegroundChanged != null)
            {
                FocusedForegroundChanged(this, e);
            }
        }

        /// <summary>
        /// Updates property value cache and raises <see cref="FocusedBorderBrushChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> that contains the event data.</param>
        protected virtual void OnFocusedBorderBrushChanged(DependencyPropertyChangedEventArgs e)
        {
            if (FocusedBorderBrushChanged != null)
            {
                FocusedBorderBrushChanged(this, e);
            }
        }

#if WPF
        /// <summary>
        /// Updates property value cache and raises
        /// <see cref="CursorTemplateChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> that contains the event data.</param>
        protected virtual void OnCursorTemplateChanged(DependencyPropertyChangedEventArgs e)
        {
            if (CursorTemplateChanged != null)
            {
                CursorTemplateChanged(this, e);
            }
        }


        /// <summary>
        /// Updates property value cache and raises <see cref="NegativeForegroundChanged"/> event.
        /// </summary>
        /// <param name="e">
        /// Property change details, such as old value and new value.</param>
        protected virtual void OnIsValueNegativeChanged(DependencyPropertyChangedEventArgs e)
        {            
            if (IsValueNegativeChanged != null)
            {
                IsValueNegativeChanged(this, e);
            }
        }
        protected virtual void OnNullValueTextChanged(DependencyPropertyChangedEventArgs e)
        {
            if (NullValueTextChanged != null)
            {
                NullValueTextChanged(this, e);
            }
        }
#endif

        protected virtual void OnAllowEditChanged(DependencyPropertyChangedEventArgs e)
        {
            setAllowEditProperty();

            if (this.AllowEditChanged != null)
            {
                this.AllowEditChanged(this, e);
            }
        }

        protected virtual void OnNegativeForegroundChanged(DependencyPropertyChangedEventArgs e)
        {
            if (this.textbox != null)
            {
                if (this.Value < 0)
                {
                    this.textbox.Foreground = this.NegativeForeground;
                }
                this.textbox.NegativeForeground = this.NegativeForeground;
            }
            if (this.NegativeForegroundChanged != null)
                this.NegativeForegroundChanged(this, e);
        }

        protected virtual void OnMinValueChanged(DependencyPropertyChangedEventArgs e)
        {
            if (this.MinValueChanged != null)
            {
                this.MinValueChanged(this, e);
            }
        }

        protected virtual void OnMaxValueChanged(DependencyPropertyChangedEventArgs e)
        {
            if (this.MaxValueChanged != null)
            {
                this.MaxValueChanged(this, e);
            }
        }

        protected virtual void OnNumberFormatInfoChanged(DependencyPropertyChangedEventArgs e)
        {

            if (this.NumberFormatInfoChanged != null)
            {
                this.NumberFormatInfoChanged(this, e);
            }
        }

        protected virtual void OnZeroColorChanged(DependencyPropertyChangedEventArgs e)
        {
            if (this.ZeroColorChanged != null)
                this.ZeroColorChanged(this, e);
        }

        protected virtual void OnMaxValidationChanged(DependencyPropertyChangedEventArgs e)
        {
            if (this.MaxValidationChanged != null)
                this.MaxValidationChanged(this, e);
        }

        protected virtual void OnMinValidationChanged(DependencyPropertyChangedEventArgs e)
        {
            if (this.MinValidationChanged != null)
                this.MinValidationChanged(this, e);
        }

        protected virtual void OnUseNullOptionChanged(DependencyPropertyChangedEventArgs e)
        {
            if (this.UseNullOptionChanged != null)
            {
                this.UseNullOptionChanged(this, e);
            }
        }

        protected virtual void OnStepChanged(DependencyPropertyChangedEventArgs e)
        {
            if(this.textbox != null)
            this.textbox.ScrollInterval = this.Step;
            if (this.StepChanged != null)
            {
                this.StepChanged(this, e);
            }
        }
     
        protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e)
        {
            m_exvalue =(double?)e.OldValue;
            m_value = (double?)e.NewValue;
            double val = 0.0;
            
            if (m_value < MinValue)
            {
               val = this.MinValue;                   
            }
            if(m_value > MaxValue)
            {
               val = this.MaxValue;
            }
           
            if (val < 0 || this.Value < 0)
            {
                if (EnableNegativeColors)
                {
                    if (this.textbox != null)
                    {
                        this.textbox.Foreground = this.NegativeForeground;
                        this.textbox.NegativeForeground = this.NegativeForeground;
                    }
                }
            }
            this.UpdateBackground();
            if (this.Nulltextbox != null)
            {
                this.Nulltextbox.Text = this.NullValueText;
            }

            if (this.Upbutton != null && this.Downbutton != null)
            {
                if (this.Upbutton.IsPressed == true || this.Downbutton.IsPressed == true)
                    Animation();
            }           
            if (this.ValueChanged != null)
            {
                this.ValueChanged(this, e);
            }         
        }      

#if WPF
        private void ChangeUpValue(object sender, ExecutedRoutedEventArgs e)
#else
        private void ChangeUpValue(object parameter)
#endif
        {
            ChangeValue(true);
        }
#if WPF
        private void ChangeDownValue(object sender, ExecutedRoutedEventArgs e)
#else
        private void ChangeDownValue(object parameter)
#endif
        {
            ChangeValue(false);
        }

        private void ChangeValue(bool IsUp)
        {            
            if (this.textbox != null)
            {
                this.textbox.ScrollInterval = this.Step;
            }
            if (this.Value != null && !double.IsNaN((double)this.Value))
            {
                if (IsUp)
                {
                    if (this.Value + this.Step <= this.MaxValue)
                        DoubleValueHandler.doubleValueHandler.HandleUpKey(this.textbox);
                    else
                        this.Value = this.MaxValue;
                }
                else
                {
                    if (this.Value - this.Step >= this.MinValue)
                        DoubleValueHandler.doubleValueHandler.HandleDownKey(this.textbox);
                    else
                        this.Value = this.MinValue;
                }
            }
        }

#if SILVERLIGHT
        private void ContentFormating(double value)
        {
            string numeric = value.ToString("N", this.NumberFormatInfo);
            this.textbox.Text = numeric;
        }
#endif
        void UpdateBackground()
        {
            if (textbox != null)
            {
#if WPF
                if (((Upbutton.IsPressed || Downbutton.IsPressed) || textbox.IsFocused) && EnableFocusedColors)
#else
                if ((Upbutton.IsPressed || Downbutton.IsPressed) && EnableFocusedColors)
#endif
                {
                    if (this.FocusedBackground != null)
                        this.UpDownBackground = this.FocusedBackground;
                    if (FocusedBorderBrush != null)
                        this.UpDownBorderBrush = this.FocusedBorderBrush;
                }
                else if (this.Value < 0 && this.EnableNegativeColors == true)
                {
                    if (this.NegativeBackground != null)
                        this.UpDownBackground = this.NegativeBackground;
                    if (this.NegativeBorderBrush != null)
                        this.UpDownBorderBrush = this.NegativeBorderBrush;
                }
                else
                    this.UpDownBackground = this.Background;
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            if (this.textbox != null)
                this.textbox.ScrollInterval = this.Step;

        }
        private void setAllowEditProperty()
        {
            if (this.textbox != null)
            {
                if (this.AllowEdit == true)
                {
                    this.textbox.IsReadOnly = false;
                }
                if (this.AllowEdit == false)
                {
                    this.textbox.IsReadOnly = true;
                }
            }
        }

#if SILVERLIGHT
        void Downbutton_Click(object sender, RoutedEventArgs e)
        {
            IsUpButtonPressed = false;
            IsDownButtonPressed = true;
            this.ChangeValue(false);
            Animation();
            //this.textbox.Value = this.Value;
        }

        void Upbutton_Click(object sender, RoutedEventArgs e)
        {
            IsUpButtonPressed = true;
            IsDownButtonPressed = false;
            this.ChangeValue(true);
            Animation();
            //this.textbox.Value = this.Value;
        }
#endif
        void UpDown_LostFocus(object sender, RoutedEventArgs e)
        {
            if (EnableFocusedColors)
            {
                this.UpDownBackground = this.Background;
                this.UpDownForeground = this.Foreground;
                this.UpDownBorderBrush = this.BorderBrush;
            }
            if (this.textbox.IsNegative && EnableNegativeColors && this.textbox != null)
            {
                if(this.NegativeBackground != null)
                    this.UpDownBackground = this.NegativeBackground;
                if(this.NegativeBorderBrush != null)
                    this.UpDownBorderBrush = this.NegativeBorderBrush;
            }
        }

        void UpDown_GotFocus(object sender, RoutedEventArgs e)
        {           
            if (EnableFocusedColors)
            {		
                if (this.FocusedBackground != null)
                    this.UpDownBackground = this.FocusedBackground;
                if (this.FocusedForeground != null)
                    this.UpDownForeground = this.FocusedForeground;
                if (this.FocusedBorderBrush != null)
                    this.UpDownBorderBrush = this.FocusedBorderBrush;
            } 
            if(this.textbox.Text!=null && this.textbox.TextSelectionOnFocus)
            {
                if (this.IsFocused == true)
                {
                    e.Handled = true;
                    textbox.Focus();
                }
            }          
            
        }

#if SILVERLIGHT
        private void Animation()
        {
            textbox.Visibility = Visibility.Visible;
            textbox.Opacity = 1;
            secondBlock.Opacity = 1;
            secondBlock.Visibility = Visibility.Visible;
            secondBlock.Background = textbox.Background;
            this.textbox.RenderTransform = new TranslateTransform();
            this.secondBlock.RenderTransform = new TranslateTransform();
            this.block2.RenderTransform = new TranslateTransform();
            this.block.RenderTransform = new TranslateTransform();
            this.t1.RenderTransform = new TranslateTransform();
            this.secondBlock.Foreground = this.textbox.Foreground;
            this.t1.Foreground = this.textbox.Foreground;
            this.t1.Background = this.textbox.Background;
            TranslateTransform pretranslate = this.textbox.RenderTransform as TranslateTransform;
            TranslateTransform curtranslate = this.secondBlock.RenderTransform as TranslateTransform;
            TranslateTransform uptranslate = this.block.RenderTransform as TranslateTransform;
            TranslateTransform downtranslate = this.block2.RenderTransform as TranslateTransform;
            TranslateTransform trans = this.t1.RenderTransform as TranslateTransform;
            Storyboard storyboard = new Storyboard();
            DoubleAnimation preanimation = new DoubleAnimation()
            {
                Duration = new Duration(TimeSpan.FromSeconds(AnimationSpeed))
            };
            DoubleAnimation curanimation = new DoubleAnimation()
            {
                Duration = new Duration(TimeSpan.FromSeconds(AnimationSpeed))
            };
            DoubleAnimation upanimation = new DoubleAnimation()
            {
                Duration = new Duration(TimeSpan.FromSeconds(AnimationSpeed))
            };
            DoubleAnimation downanimation = new DoubleAnimation()
            {
                Duration = new Duration(TimeSpan.FromSeconds(AnimationSpeed))
            };
            DoubleAnimation transm = new DoubleAnimation()
            {
                Duration = new Duration(TimeSpan.FromSeconds(AnimationSpeed))
            };
            storyboard.Children.Add(upanimation);
            storyboard.Children.Add(preanimation);
            storyboard.Children.Add(downanimation);
            storyboard.Children.Add(curanimation);
            storyboard.Children.Add(transm);
            Storyboard.SetTarget(transm, trans);
            Storyboard.SetTargetProperty(transm, new PropertyPath("(TranslateTransform.Y)"));
            Storyboard.SetTarget(preanimation, pretranslate);
            Storyboard.SetTargetProperty(preanimation, new PropertyPath("(TranlateTransform.Y)"));
            Storyboard.SetTarget(curanimation, curtranslate);
            Storyboard.SetTargetProperty(curanimation, new PropertyPath("(TranlateTransform.Y)"));
            Storyboard.SetTarget(upanimation, uptranslate);
            Storyboard.SetTargetProperty(upanimation, new PropertyPath("(TranlateTransform.Y)"));
            Storyboard.SetTarget(downanimation, downtranslate);
            Storyboard.SetTargetProperty(downanimation, new PropertyPath("(TranlateTransform.Y)"));
            if (Upbutton.IsPressed == true)
            {
                this.border.Opacity = 1;
                this.t1.Visibility = Visibility.Visible;
                this.block.Visibility = Visibility.Visible;
                this.block2.Visibility = Visibility.Visible;
                t1.Text = m_exvalue.ToString(DEF_NUMBERFORMAT, NumberFormatInfo);
                //textbox.Text = m_value.ToString(DEF_NUMBERFORMAT, NumberFormatInfo);
                secondBlock.Text = m_exvalue.ToString(DEF_NUMBERFORMAT, NumberFormatInfo);
                preanimation.From = this.border.ActualHeight;
                preanimation.To = 0;
                upanimation.From = (this.border.ActualHeight);
                upanimation.To = (this.border.ActualHeight);
                transm.From = 0;
                transm.To = -(this.border.ActualHeight);
                downanimation.From = (this.border.ActualHeight);
                downanimation.To = (this.border.ActualHeight);

                this.secondBlock.Visibility = Visibility.Collapsed;
                storyboard.Begin();
            }
            if (Downbutton.IsPressed == true)
            {
                this.border.Opacity = 1;
                this.secondBlock.Visibility = Visibility.Visible;
                this.block.Visibility = Visibility.Visible;
                this.block2.Visibility = Visibility.Visible;
                t1.Text = m_exvalue.ToString(DEF_NUMBERFORMAT, NumberFormatInfo);
                //textbox.Text = m_value.ToString(DEF_NUMBERFORMAT, NumberFormatInfo);
                secondBlock.Text = m_exvalue.ToString(DEF_NUMBERFORMAT, NumberFormatInfo);
                preanimation.From = -(this.border.ActualHeight);
                preanimation.To = 0;
                downanimation.From = -(this.border.ActualHeight);
                downanimation.To = -(this.border.ActualHeight) / 2;
                curanimation.From = 0;
                curanimation.To = (this.border.ActualHeight);
                upanimation.From = (this.border.ActualHeight);
                upanimation.To = (this.border.ActualHeight);
                this.t1.Visibility = Visibility.Collapsed;
                storyboard.Begin();
            }


            m_exvalue = m_value;
        }
#endif
#if WPF
        private void Animation()
        {
            if (IsLoaded)
            {
                textbox.Visibility = Visibility.Visible;
                textbox.Opacity = 1;
                secondBlock.Opacity = 1;
                secondBlock.Visibility = Visibility.Visible;
                secondBlock.Background = textbox.Background;
                this.textbox.RenderTransform = new TranslateTransform();
                this.secondBlock.RenderTransform = new TranslateTransform();
                this.t1.RenderTransform = new TranslateTransform();
                this.secondBlock.Foreground = this.textbox.Foreground;
                this.t1.Foreground = this.textbox.Foreground;
                Storyboard storyboard = new Storyboard();
                DoubleAnimation preanimation = new DoubleAnimation();
                TranslateTransform pretranslate = (TranslateTransform)textbox.RenderTransform;
                DoubleAnimation curanimation = new DoubleAnimation();
                TranslateTransform curtranslate = (TranslateTransform)secondBlock.RenderTransform;
                DoubleAnimation transm = new DoubleAnimation();
                TranslateTransform trans = (TranslateTransform)t1.RenderTransform;
                storyboard.Children.Add(preanimation);
                storyboard.Children.Add(curanimation);
                storyboard.Children.Add(transm);
                if (Upbutton.IsPressed == true)
                {
                    this.t1.Visibility = Visibility.Visible;
                    this.t1.Background = this.textbox.Background;
                    t1.Value = m_exvalue;
                    secondBlock.Value = m_exvalue;
                    preanimation.Duration = new Duration(TimeSpan.FromSeconds(AnimationSpeed));
                    preanimation.From = this.border.ActualHeight;
                    preanimation.To = 0;
                    pretranslate.BeginAnimation(TranslateTransform.YProperty, preanimation);
                    transm.Duration = new Duration(TimeSpan.FromSeconds(AnimationSpeed));
                    transm.From = 0;
                    transm.To = -(this.border.ActualHeight);
                    trans.BeginAnimation(TranslateTransform.YProperty, transm);
                    this.secondBlock.Visibility = Visibility.Collapsed;
                }
                else if (Downbutton.IsPressed == true)
                {
                    this.secondBlock.Visibility = Visibility.Visible;
                    t1.Value = m_exvalue;
                    secondBlock.Value = m_exvalue;
                    textbox.Visibility = Visibility.Visible;
                    preanimation.Duration = new Duration(TimeSpan.FromSeconds(AnimationSpeed));
                    preanimation.From = -(this.border.ActualHeight);
                    preanimation.To = 0;
                    pretranslate.BeginAnimation(TranslateTransform.YProperty, preanimation);
                    curanimation.Duration = new Duration(TimeSpan.FromSeconds(AnimationSpeed));
                    curanimation.From = 0;
                    curanimation.To = (this.border.ActualHeight);
                    curtranslate.BeginAnimation(TranslateTransform.YProperty, curanimation);
                    this.t1.Visibility = Visibility.Collapsed;
                }

                m_exvalue = m_value;
            }
        }
#endif
        #endregion

        #region Overrides
        public override void OnApplyTemplate()
        {
            if (this.textbox != null)
                this.textbox.ValueChanging -= new DoubleTextBox.ValueChangingEventHandler(textbox_ValueChanging);

            this.GotFocus -= new RoutedEventHandler(UpDown_GotFocus);
            this.LostFocus -= new RoutedEventHandler(UpDown_LostFocus);

            base.OnApplyTemplate();

#if SILVERLIGHT
            if (this.Upbutton != null)
                this.Upbutton.Click -= new RoutedEventHandler(Upbutton_Click);
            if (this.Downbutton != null)
                this.Downbutton.Click -= new RoutedEventHandler(Downbutton_Click);
#endif
            this.Nulltextbox = (TextBox)base.GetTemplateChild("text");
            this.textbox = (DoubleTextBox)base.GetTemplateChild("DoubleTextBox");
            this.Upbutton = (RepeatButton)base.GetTemplateChild("upbutton");
            this.Downbutton = (RepeatButton)base.GetTemplateChild("downbutton");
            this.border = (Border)base.GetTemplateChild("Border");
#if WPF
            this.t1 = (DoubleTextBox)base.GetTemplateChild("textBox");
            this.secondBlock = (DoubleTextBox)base.GetTemplateChild("SecondBlock");
#endif
#if SILVERLIGHT
            this.block = (TextBox)base.GetTemplateChild("Block");
            this.block2 = (TextBox)base.GetTemplateChild("Block2");
            this.t1 = (TextBox)base.GetTemplateChild("textBox");
            this.secondBlock = (TextBox)base.GetTemplateChild("SecondBlock");
            this.block2.Height = this.border.Height + this.border.Height;
            this.block.Height = this.border.Height + this.border.Height;
            this.clipgeo = (RectangleGeometry)base.GetTemplateChild("clipgeo");
            clipgeo.Rect = new Rect(0, 0, this.border.Width, this.border.Height);
#endif           
            this.GotFocus += new RoutedEventHandler(UpDown_GotFocus);
            this.LostFocus += new RoutedEventHandler(UpDown_LostFocus);
#if SILVERLIGHT
            if (this.Upbutton != null)
                this.Upbutton.Click += new RoutedEventHandler(Upbutton_Click);
            if (this.Downbutton != null)
                this.Downbutton.Click += new RoutedEventHandler(Downbutton_Click);
#endif    
            this.UpDownBackground = this.Background;
            this.UpDownBorderBrush = this.BorderBrush;
            this.UpDownForeground = this.Foreground;

            setAllowEditProperty();
            if (this.textbox != null)
                this.textbox.ValueChanging += new DoubleTextBox.ValueChangingEventHandler(textbox_ValueChanging);
            if (this.Nulltextbox != null)
            {
                this.Nulltextbox.PreviewTextInput += new TextCompositionEventHandler(Nulltextbox_PreviewTextInput);
            }
        }

        void Nulltextbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (this.textbox != null)
                e.Handled = DoubleValueHandler.doubleValueHandler.MatchWithMask(this.textbox, e.Text);
            base.OnTextInput(e);
        }

        void textbox_ValueChanging(object sender, ValueChangingEventArgs e)
        {
            if (this.ValueChanging != null)
                ValueChanging(this, e);
        }
        #endregion

    } 
}
