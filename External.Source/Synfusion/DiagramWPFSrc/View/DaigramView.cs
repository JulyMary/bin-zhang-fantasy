// <copyright file="DaigramView.cs" company="Syncfusion">
// Copyright Syncfusion Inc. 2001 - 2011. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Windows.Themes;
using System.Windows.Data;

namespace Syncfusion.Windows.Diagram
{
    /// <summary>
    /// Represents the Diagram View.
    /// <para>The view obtains data from the model and presents them to the user. It typically manages the overall layout of the data obtained from model.
    /// Apart from presenting the data, view also handles navigation between the items, and some aspects of item selection. 
    /// The views also implements basic user interface features, such as rulers, and drag and drop. 
    /// It handles the events, which occur on the objects, obtained from the model. 
    /// Command mechanism is also implemented by the view.
    /// </para>
    /// </summary>
    /// <example>
    /// <para/>The following example shows how to create a <see cref="DiagramView"/> in XAML.
    /// <code language="XAML">
    /// &lt;Window x:Class="RulersAndUnits.Window1"
    /// xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    /// xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    /// xmlns:syncfusion="http://schemas.syncfusion.com/wpf"
    /// Title="Rulers And Units Demo"  WindowState="Maximized" Name="mainwindow" 
    ///  xmlns:local="clr-namespace:Sample" FontWeight="Bold"
    ///  Icon="Images/App.ico" &gt;
    ///   &lt;syncfusion:DiagramControl Grid.Column="1" Name="diagramControl" 
    ///                                IsSymbolPaletteEnabled="True" 
    ///                                Background="WhiteSmoke"&gt;
    ///           &lt;syncfusion:DiagramControl.View&gt;
    ///              &lt;syncfusion:DiagramView  IsPageEditable="True" 
    ///                                        Background="LightGray"  
    ///                                        Bounds="0,0,12,12"  
    ///                                        ShowHorizontalGridLine="False" 
    ///                                        ShowVerticalGridLine="False"
    ///                                        Name="diagramView"  &gt;
    ///             &lt;syncfusion:HorizontalRuler Name="horizontalRuler" /&gt;
    ///          &lt;/syncfusion:DiagramView.HorizontalRuler&gt;
    ///           &lt;syncfusion:DiagramView.VerticalRuler&gt;
    ///               &lt;syncfusion:VerticalRuler    Name="verticalRuler" /&gt;
    ///           &lt;/syncfusion:DiagramView.VerticalRuler &gt;
    ///       &lt;/syncfusion:DiagramView&gt;
    ///    &lt;/syncfusion:DiagramControl.View&gt;
    /// &lt;/syncfusion:DiagramControl&gt;
    /// &lt;/Window&gt;
    /// </code>
    /// <para/>The following example shows how to create a <see cref="DiagramView"/> in C#.
    /// <code language="C#">
    /// using System;
    /// using System.Collections.Generic;
    /// using System.Linq;
    /// using System.Text;
    /// using System.Windows;
    /// using System.Windows.Controls;
    /// using System.Windows.Data;
    /// using System.Windows.Documents;
    /// using System.Windows.Input;
    /// using System.Windows.Media;
    /// using System.Windows.Media.Imaging;
    /// using System.Windows.Navigation;
    /// using System.Windows.Shapes;
    /// using System.ComponentModel;
    /// using Syncfusion.Core;
    /// using Syncfusion.Windows.Diagram;
    /// namespace WpfApplication1
    /// {
    /// public partial class Window1 : Window
    /// {
    ///    public DiagramControl Control;
    ///    public DiagramModel Model;
    ///    public DiagramView View;
    ///    public Window1 ()
    ///    {
    ///       InitializeComponent ();
    ///       Control = new DiagramControl ();
    ///       View = new DiagramView ();
    ///       Control.View = View;
    ///       HorizontalRuler hruler = new HorizontalRuler();
    ///       View.HorizontalRuler = hruler;
    ///       View.ShowHorizontalGridLine = false;
    ///       View.ShowVerticalGridLine = false;
    ///       VerticalRuler vruler = new VerticalRuler();
    ///       View.VerticalRuler = vruler;
    ///       View.Bounds = new Thickness (0, 0, 1000, 1000);
    ///       View.IsPageEditable = true;
    ///    }
    ///    }
    ///    }
    /// </code>
    /// </example>
    /// <seealso cref="HorizontalRuler"/>
    /// <seealso cref="VerticalRuler"/>
    /// <seealso cref="DiagramPage"/>
#if SyncfusionFramework4_0
    [DesignTimeVisible(false)]
#endif
    public partial class DiagramView : ContentControl, IView, INotifyPropertyChanged
    {

        #region Drawing Tools
        private System.Windows.Shapes.Path temppath;
        private PathGeometry tempbezier;
        private PolyLineSegment temppoly;
        private double[] Dimensions = new double[2];
        private double oldx;
        private Node n1;
        private double oldy;
        private BezierSegment bs;
        private Rect rt = new Rect();
        private System.Windows.Shapes.Path draw_ell = null;
        private System.Windows.Shapes.Path p;
        private PathFigure pf;
        private Point point1;
        private Point point2;
        private Point point3;
        private Point ortho1 = new Point();
        private Point ortho2 = new Point();
        private Point ortho3 = new Point();
    # endregion
        #region Class fields

        public Brush PageBackground
        {
            get { return (Brush)GetValue(PageBackgroundProperty); }
            set { SetValue(PageBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PageBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageBackgroundProperty =
            DependencyProperty.Register("PageBackground", typeof(Brush), typeof(DiagramView));        

        /// <summary>
        /// Used to store copypastemanager.
        /// </summary>
        internal CopyPasteManager CPManager;

        /// <summary>
        /// Used to store the cursor used.
        /// </summary>
        private Cursor m_cursor;

        ///// <summary>
        ///// Used to store the translate transform
        ///// </summary>
        //private TranslateTransform translateTransform;

        /// <summary>
        /// Used to store the scrollviewer instance
        /// </summary>
        private ScrollViewer scrollview;

        /// <summary>
        /// Used to store the IsPageEditable property
        /// </summary>
        private static bool ispagedit = true;

        /// <summary>
        ///  Used to store the other event's state
        /// </summary>
        private static bool otherevents = false;

        /// <summary>
        /// Used to store the View grid state
        /// </summary>
        private static bool isvieworiginchanged = false;

        /// <summary>
        /// Used to store the node drag information.
        /// </summary>
        private bool isdrag = false;

        /// <summary>
        /// Used to store the screen start point
        /// </summary>
        private Point screenStartPoint = new Point(0, 0);

        /// <summary>
        /// Used to store  the scale transform
        /// </summary>
        private ScaleTransform zoomTransform;

        ///// <summary>
        ///// Used to store the transform group
        ///// </summary>
        //private TransformGroup transformGroup;

        /// <summary>
        /// Checks if node was deleted.
        /// </summary>
        private bool nodedel = false;

        /// <summary>
        /// Checks if line was deleted.
        /// </summary>
        private bool linedel = false;

        ///// <summary>
        ///// Used to store the x value of the page origin with respect to the view.
        ///// </summary>
        //private double oldxviewgrid = 0;

        ///// <summary>
        ///// Used to store the y value of the page origin with respect to the view.
        ///// </summary>
        //private double oldyviewgrid = 0;

        /// <summary>
        /// Checks if deletion was done.
        /// </summary>
        private bool isdupdel;

        /// <summary>
        /// Used to store internal edges collection.
        /// </summary>
        private CollectionExt intedges = new CollectionExt();

        /// <summary>
        /// Used to store the start offset
        /// </summary>
        //private Point startOffset;

        internal ScrollableGrid ScrollGrid;

        /// <summary>
        /// Used to store the view.
        /// </summary>
        //private static DiagramView dview;

        /// <summary>
        /// Used to store the origin point
        /// </summary>
        private System.Drawing.Point mOrigin;

        /// <summary>
        /// Used to store ShowRulers property
        /// </summary>
        private bool showRuler;

        /// <summary>
        /// Used to store ShowPage property
        /// </summary>
        private bool showPage;

        /// <summary>
        /// Used to store the model
        /// </summary>
        private IModel mModel;

        /// <summary>
        /// Used to store the bounds
        /// </summary>
        private Thickness mBounds;

        /// <summary>
        /// Used to store the DiagramViewGrid.
        /// </summary>
        internal DiagramViewGrid mViewGrid;


    
        /// <summary>
        /// Used to store the OnReset command value.
        /// </summary>
        private bool onReset = false;

        /// <summary>
        ///  Used to store the view grid
        /// </summary>
        private Grid viewgrid;

        /// <summary>
        ///  Used to store horizontal tickbar
        /// </summary>
        private TickBar hortickbar;

        /// <summary>
        ///  Used to store vertical tickbar
        /// </summary>
        private TickBar vertickbar;

        ///// <summary>
        /////  Used to store the origin point
        ///// </summary>
        ////private Point vieworigin = new Point(0, 0);

        ///// <summary>
        /////  Used to store the view grid origin
        ///// </summary>
        //private Point viewgridorigin = new Point(0, 0);

        /// <summary>
        ///  Used to store diagram Control instance.
        /// </summary>
        internal DiagramControl dc;

        /// <summary>
        /// Used to check if node is deleted
        /// </summary>
        //private bool isdel = false;

        /// <summary>
        /// Used to store boolean value on zoom factor becoming less than one.
        /// </summary>
        private bool lessthanone = true;

        /// <summary>
        /// Used to store boolean value on zoom factor becoming more than one.
        /// </summary>
        private bool morethanone = true;

        /// <summary>
        /// Used to store boolean value on executing zoom for the first time.
        /// </summary>
        private bool firstzoom = false;

        /// <summary>
        /// Used to store a double value
        /// </summary>
        private int i = 1;

        /// <summary>
        /// Used to store a double value
        /// </summary>
        private int j = 1;

        /// <summary>
        /// Used to store count value
        /// </summary>
        private int count = 1;

        ///// <summary>
        ///// Used to check if mouse is up.
        ///// </summary>
        //private bool muponly = false;

        /// <summary>
        /// Used to check if node is dragged.
        /// </summary>
        private bool isdragged = false;

        /// <summary>
        /// Used to store pixel interval
        /// </summary>
        private double pixelvalue = 50;

        /// <summary>
        /// Used to store boolean value on executing level one once
        /// </summary>
        private bool leveloneexe = false;

        /// <summary>
        /// Used to store boolean value on executing zoom once
        /// </summary>
        private bool zoominexe = false;

        ///// <summary>
        ///// Used to store scroll thumb click boolean value.
        ///// </summary>
        //private bool scrollthumb = false;

        ///// <summary>
        ///// Used to store old horizontal offset
        ///// </summary>
        //private double oldhoffset = 0;

        ///// <summary>
        /////  Used to store old vertical offset
        ///// </summary>
        //private double oldvoffset = 0;

        ///// <summary>
        /////  Used to store scroll vertical thumb offset
        ///// </summary>
        //private double scrollverthumb = 0;

        ///// <summary>
        /////  Used to store boolean value on executing on x axis once.
        ///// </summary>
        //private bool exeonce = false;

        ///// <summary>
        ///// Used to store boolean value on executing on y axis once.
        ///// </summary>
        //private bool exeoncey = false;

        ///// <summary>
        ///// Used to store horizontal offset
        ///// </summary>
        //private double hf = 0;

        ///// <summary>
        ///// Used to store vertical offset
        ///// </summary>
        //private double vf = 0;

        ///// <summary>
        ///// Used to store horizontal thumb offset value.
        ///// </summary>
        //private double x = 0;

        ///// <summary>
        ///// Used to store vertical thumb offset value.
        ///// </summary>
        //private double y = 0;

        ///// <summary>
        ///// Used to store the vertical offset
        ///// </summary>
        //private double vdoff = 0;

        ///// <summary>
        ///// Used to store y coordinate value.
        ///// </summary>
        //private double ycoordinate = 0;

        ///// <summary>
        ///// Used to store x coordinate value.
        ///// </summary>
        //private double xcoordinate = 0;

        /// <summary>
        /// Used to store the translate point
        /// </summary>
        //private Point translatepoint = new Point();

        ///// <summary>
        ///// Used to store pan constant
        ///// </summary>
        //private double panconst = 0;

        ///// <summary>
        ///// Used to store scroll thumb instance
        ///// </summary>
        //private double scrollhorthumb = 0;

        ///// <summary>
        ///// Used to check if scrolled right
        ///// </summary>
        //private bool scrolledright = false;

        ///// <summary>
        ///// Used to check if scrolled bottom
        ///// </summary>
        //private bool scrolledbottom = false;

        ///// <summary>
        ///// Used to check if scrolled 
        ///// </summary>
        //private bool scrollchk = false;

        ///// <summary>
        ///// Used to check if mouse is scrolled 
        ///// </summary>
        //private bool scrolled = false;

        ///// <summary>
        ///// Used to store horizontal thumb instance
        ///// </summary>
        //private double horthumb = 0;

        ///// <summary>
        ///// Used to check if offsety is greater than zero
        ///// </summary>
        //private bool offygtz = false;

        ///// <summary>
        ///// Used to store vertical thumb instance
        ///// </summary>
        //private double verthumb = 0;

        ///// <summary>
        ///// Used to check if mouse is just wheeled
        ///// </summary>
        //private bool justwheeled = true;

        ///// <summary>
        ///// Used to check if mouse is scrolled
        ///// </summary>
        //private bool justscrolled = false;

        ///// <summary>
        ///// Used to check if mouse is wheeled
        ///// </summary>
        //private bool mousew = false;

        /// <summary>
        /// Used to check if offsetx is greater than zero
        /// </summary>
        //private bool offgtz = false;

        ///// <summary>
        ///// Used to check if mouse is pressed
        ///// </summary>
        //private bool mousep = false;

        ///// <summary>
        ///// Used to check view grid origin position
        ///// </summary>
        //private bool vieworiginchanged2 = false;

        ///// <summary>
        ///// Used to check is mouse is wheeled
        ///// </summary>
        //private bool ismousewheel = false;

        ///// <summary>
        ///// Used to store the line drag in y coordinate.
        ///// </summary>
        //private double linedragy = 0;

        ///// <summary>
        ///// Used to store the line drag in x coordinate.
        ///// </summary>
        //private double linedragx = 0;

        ///// <summary>
        ///// Used to check line drag in y coordinate.
        ///// </summary>
        //private bool islinedraggedy = false;

        ///// <summary>
        ///// Used to check line drag in x coordinate.
        ///// </summary>
        //private bool islinedragged = false;

        ///// <summary>
        ///// Used to store the new viewport width value.
        ///// </summary>
        //private double newviewportwidth = 0;

        ///// <summary>
        /////  Used to store the old viewport width value.
        ///// </summary>
        //private double oldviewportwidth = 0;

        ///// <summary>
        ///// Used to check if executed once.
        ///// </summary>
        //private bool exe = false;

        ///// <summary>
        ///// Used to check if scrollbars were moved.
        ///// </summary>
        //private bool mscrolled = false;

        /// <summary>
        /// Used to store the groups in the page.
        /// </summary>
        private CollectionExt m_groups = new CollectionExt();

        /// <summary>
        /// Checks if ScrolledToNode method was called.
        /// </summary>
        //private bool scrolledtonode = false;

        /// <summary>
        /// Checks if MeasureOveride  was called.
        /// </summary>
        private bool measured = false;

        /// <summary>
        /// Checks if Undo command was executed.
        /// </summary>
        internal bool undo = false;

        /// <summary>
        /// Checks if Redo command was executed.
        /// </summary>
        internal bool redo = false;

        /// <summary>
        /// Checks if <see cref="Node"/> was dragged.
        /// </summary>
        private bool dragged = false;

        /// <summary>
        /// Checks if <see cref="Node"/> was resized.
        /// </summary>
        private bool resized = false;

        /// <summary>
        /// Checks if <see cref="Node"/> was resized as a resule of undo operation.
        /// </summary>
        private bool undoresize = false;

        /// <summary>
        /// Checks if <see cref="Node"/> was resized as a resule of redo operation.
        /// </summary>
        private bool redoresize = false;

        /// <summary>
        /// Checks if Delete command was executed.
        /// </summary>
        private bool deletecommandexe = false;

        /// <summary>
        /// Used to store the delete count.
        /// </summary>
        private int delcount = 0;

        /// <summary>
        /// Used to store the node drag count.
        /// </summary>
        private int nodedragcount = 1;

        /// <summary>
        /// Used to store the values in the selection list.
        /// </summary>
        private ObservableCollection<ICommon> oldselectionlist = new ObservableCollection<ICommon>();

        /// <summary>
        /// Used to store the count of the <see cref="Node"/> been resized.
        /// </summary>
        private int resizecount = 0;

        /// <summary>
        /// Used to store the count of the <see cref="Node"/> been rotated.
        /// </summary>
        private int rotatecount = 1;

        /// <summary>
        /// Refers to the undo stack.
        /// </summary>
        private Stack<object> undocommandstack = new Stack<object>();

        /// <summary>
        /// Refers to the redo stack.
        /// </summary>
        private Stack<object> redocommandstack = new Stack<object>();

        /// <summary>
        /// Checks if automatic layout is used.
        /// </summary>
        private bool layout = false;

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes static members of the <see cref="DiagramView"/> class.
        /// </summary>
        static DiagramView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DiagramView), new FrameworkPropertyMetadata(typeof(DiagramView)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagramView"/> class.
        /// </summary>
        public DiagramView()
        {
            this.Loaded += new RoutedEventHandler(DiagramView_Loaded);
            this.Page = new DiagramPage();
            this.Page.Loaded += new RoutedEventHandler(Page_Loaded);
            this.Page.Focusable = true;
            FocusManager.SetIsFocusScope(this.Page, false);
            this.Page.Focus();
            //DiagramCommandManager d = new DiagramCommandManager(this);
            this.AllowDrop = true;
            this.Unloaded += new RoutedEventHandler(DiagramView_Unloaded);
            if (this.CPManager == null)
            {
                CPManager = new CopyPasteManager(this);
            }
            this.AddHandler(Control.MouseDownEvent, new MouseButtonEventHandler(DiagramView_MouseDown), true);
            InitDiagramProperties();
            this.SizeChanged += new SizeChangedEventHandler(DiagramView_SizeChanged);
        }

        void DiagramView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateViewGridOrigin();
            if (vertickbar != null)
            {
                vertickbar.InvalidateVisual();
            }

            if (hortickbar != null)
            {
                hortickbar.InvalidateVisual();
            }
        }

        #region Dirty Flag

        private void InitDiagramProperties()
        {
            List<string> nodePropList = new List<string>()
            {  
               "MinHeight", "MinWidth", "Height", "Width", "ActualHeight", "ActualWidth", "IsLabelEditable", "Label", "LabelVisibility", "LabelHorizontalAlignment", 
               "LabelVerticalAlignment", "HorizontalContentAlignment", "VerticalContentAlignment", "LabelAngle", "Shape", "CustomPathStyle", "Level", "OffsetX", "OffsetY", 
               "Content", "AllowMove", "AllowSelect", "AllowRotate", "AllowResize", "LabelTextTrimming", "LabelForeground", "LabelBackground", "LabelFontStyle", "LabelFontFamily", 
               "LabelTextAlignment", "LabelFontSize", "LabelFontWeight", "LabelTextWrapping", "LabelWidth", "EnableMultilineLabel", 
               "Name", "ZIndex"               
            };
            List<string> lineConnectorPropList = new List<string>()
            {  
               "EnableConnection", "IntermediatePoints", "LabelTemplate", "ConnectionEndSpace", "ConnectorType", "HeadNode", "TailNode", "HeadDecoratorShape", 
               "TailDecoratorShape", "HeadDecoratorStyle", "TailDecoratorStyle", "CustomHeadDecoratorStyle", "CustomTailDecoratorStyle", "LineStyle", 
               "LineBridgingEnabled", "FirstSegmentLength", "LastSegmentLength", "AutoAdjustPoints", "VertexStyle", "DecoratorAdornerStyle", "IsVertexVisible",
               "IsVertexMovable", "Name", "Height", "Width", "StartPointPosition", "EndPointPosition", "Label", "IsLabelEditable", "Label", "LabelVisibility",
               "LabelHorizontalAlignment", "LabelVerticalAlignment", "HorizontalContentAlignment", "VerticalContentAlignment", "LabelAngle", "CustomPathStyle", "Level", 
               "Content", "AllowMove", "AllowSelect", "AllowResize", "LabelTextTrimming", "LabelForeground", "LabelBackground", "LabelFontStyle", "LabelFontFamily", 
               "LabelTextAlignment", "LabelFontSize", "LabelFontWeight", "LabelTextWrapping", "LabelWidth", "EnableMultilineLabel"
            };
            List<string> connectionPortPropList = new List<string>()
            {  
               "Left", "Top", "Node", "Port", "PortShape", "PortStyle", "AllowPortDrag", "ConnectionHeadPort", "ConnectionTailPort", "Name", "Height", "Width"
            };

            AddDiagramProperties(DiagramProperties, typeof(Node), nodePropList);
            AddDiagramProperties(DiagramProperties, typeof(ConnectorBase), lineConnectorPropList);
            AddDiagramProperties(DiagramProperties, typeof(ConnectionPort), connectionPortPropList);
        }

        private void AddDiagramProperties(ObservableCollection<DiagramProperty> dp, Type type, List<string> properties)
        {
            foreach (string s in properties)
            {
                dp.Add(new DiagramProperty() { ObjectType = type, PropertyName = s });
            }
        }

        /// <summary>
        /// It is used to store the Node, Lineconnector types and Properties.
        /// </summary>
        public ObservableCollection<DiagramProperty> DiagramProperties = new ObservableCollection<DiagramProperty>();

        /// <summary>
        /// Identifies the ViewGridOrigin property.This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty IsPageSavedProperty =
            DependencyProperty.Register("IsPageSaved", typeof(bool), typeof(DiagramView), new PropertyMetadata(true));

        /// <summary>
        ///  Gets or sets a value indicating whether [Is Page Saved].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [Is Page Saved]; otherwise, <c>false</c>.
        /// </value>
        public bool IsPageSaved
        {
            get
            {
                return (bool)GetValue(IsPageSavedProperty);
            }

            set
            {
                SetValue(IsPageSavedProperty, value);
            }
        }

        #endregion

        void Scrollviewer_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            UpdateViewGridOrigin();
        }

        //private static void SetUnitBinding(string PropName, DependencyProperty dependencyProperty, FrameworkElement src)
        //{
        //    MultiBinding UnitPixelBind = new MultiBinding();
        //    UnitPixelBind.Mode = BindingMode.TwoWay;
        //    UnitPixelBind.Converter = new PixelUnitConverter();
        //    Binding UnitValue = new Binding(PropName);
        //    UnitValue.Mode = BindingMode.TwoWay;
        //    UnitValue.Source = src;
        //    UnitPixelBind.Bindings.Add(UnitValue);
        //    Binding MeasureValue = new Binding();
        //    MeasureValue.Mode = BindingMode.TwoWay;
        //    MeasureValue.Source = src;
        //    UnitPixelBind.Bindings.Add(MeasureValue);
        //    src.SetBinding(dependencyProperty, UnitPixelBind);
        //}

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.FrameworkElement.Initialized"/> event. This method is invoked whenever <see cref="P:System.Windows.FrameworkElement.IsInitialized"/> is set to true internally.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.RoutedEventArgs"/> that contains the event data.</param>
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            //if (Page != null)
            //{
            //    PxBounds = MeasureUnitsConverter.ToPixels(PxBounds, (Page as DiagramPage).MeasurementUnits);
            //    PxSnapOffsetX = MeasureUnitsConverter.ToPixels(SnapOffsetX, (Page as DiagramPage).MeasurementUnits);
            //    PxSnapOffsetY = MeasureUnitsConverter.ToPixels(SnapOffsetY, (Page as DiagramPage).MeasurementUnits);
            //    PxGridHorizontalOffset = MeasureUnitsConverter.ToPixels(GridHorizontalOffset, (Page as DiagramPage).MeasurementUnits);
            //    PxGridVerticalOffset = MeasureUnitsConverter.ToPixels(GridVerticalOffset, (Page as DiagramPage).MeasurementUnits);
            //}
            NameScope.SetNameScope(this, null);
        }

        /// <summary>
        /// Invoked when the Diagram View is loaded.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void DiagramView_Loaded(object sender, RoutedEventArgs e)
        {

            Binding MarginBinding = new Binding("PageMargin");
            MarginBinding.Source = this;
            (this.Page as DiagramPage).SetBinding(DiagramPage.MarginProperty, MarginBinding);
            //Node.SetUnitBinding("PxBounds", "Page.MeasurementUnits", DiagramView.BoundsProperty, this);
            //Node.SetUnitBinding("PxSnapOffsetX", "Page.MeasurementUnits", DiagramView.SnapOffsetXProperty, this);
            //Node.SetUnitBinding("PxSnapOffsetY", "Page.MeasurementUnits", DiagramView.SnapOffsetYProperty, this);
            //Node.SetUnitBinding("PxGridHorizontalOffset", "Page.MeasurementUnits", DiagramView.GridHorizontalOffsetProperty, this);
            //Node.SetUnitBinding("PxGridVerticalOffset", "Page.MeasurementUnits", DiagramView.GridVerticalOffsetProperty, this);

            dc = DiagramPage.GetDiagramControl(this);
            //dview = dc.View;
            if (dc.IsUnloaded)
            {
                Scrollviewer = GetTemplateChild("PART_ScrollViewer") as ScrollViewer;
                //(this.Page as DiagramPage).HorValue = Scrollviewer.HorizontalOffset / CurrentZoom;
                //(this.Page as DiagramPage).VerValue = Scrollviewer.VerticalOffset / CurrentZoom;
            }

            if (this.HorizontalRuler != null)
            {
                try
                {
                    Border b1 = (Border)VisualTreeHelper.GetChild(this.HorizontalRuler, 0);
                    Grid g2 = (Grid)VisualTreeHelper.GetChild(b1, 0);
                    Grid g3 = (Grid)VisualTreeHelper.GetChild(g2, 0);
                    hortickbar = (TickBar)VisualTreeHelper.GetChild(g3, 0);
                }
                catch
                {
                }
            }

            if (this.VerticalRuler != null)
            {
                try
                {
                    Border b = (Border)VisualTreeHelper.GetChild(this.VerticalRuler, 0);
                    Grid g = (Grid)VisualTreeHelper.GetChild(b, 0);
                    Grid g1 = (Grid)VisualTreeHelper.GetChild(g, 0);
                    vertickbar = (TickBar)VisualTreeHelper.GetChild(g1, 0);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Handles the Unloaded event of the DiagramView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void DiagramView_Unloaded(object sender, RoutedEventArgs e)
        {
            //if (this.Page != null)
            //{
            //    (this.Page as DiagramPage).HorValue = Scrollviewer.HorizontalOffset / CurrentZoom;
            //    (this.Page as DiagramPage).VerValue = Scrollviewer.VerticalOffset / CurrentZoom;
            //}
        }
        #endregion

        #region Event Handlers


        /// <summary>
        /// 
        /// </summary>
        public static readonly RoutedEvent LineMovedEvent = EventManager.RegisterRoutedEvent(
        "LineMoved", RoutingStrategy.Bubble, typeof(LineNudgeEventHandler), typeof(DiagramView));

        /// <summary>
        /// Occurs when [line moved].
        /// </summary>
        public event LineNudgeEventHandler LineMoved
        {
            add { AddHandler(LineMovedEvent, value); }
            remove { RemoveHandler(LineMovedEvent, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly RoutedEvent NodeMovedEvent = EventManager.RegisterRoutedEvent(
        "NodeMoved", RoutingStrategy.Bubble, typeof(NodeNudgeEventHandler), typeof(DiagramView));

        /// <summary>
        /// Occurs when [node moved].
        /// </summary>
        public event NodeNudgeEventHandler NodeMoved
        {
            add { AddHandler(NodeMovedEvent, value); }
            remove { RemoveHandler(NodeMovedEvent, value); }
        }

        /// <summary>
        ///  NodeSelected Routed event. Is raised when the node is deleted.
        /// </summary>
        public static readonly RoutedEvent NodeSelectedEvent = EventManager.RegisterRoutedEvent(
        "NodeSelected", RoutingStrategy.Bubble, typeof(NodeEventHandler), typeof(DiagramView));

        /// <summary>
        /// NodeSelected Event Handler
        /// </summary>
        /// Type: <see cref="NodeEventHandler"/>
        public event NodeEventHandler NodeSelected
        {
            add { AddHandler(NodeSelectedEvent, value); }
            remove { RemoveHandler(NodeSelectedEvent, value); }
        }

        /// <summary>
        ///  NodeUnSelected Routed event. Is raised when the node is deleted.
        /// </summary>
        public static readonly RoutedEvent NodeUnSelectedEvent = EventManager.RegisterRoutedEvent(
        "NodeUnSelected", RoutingStrategy.Bubble, typeof(NodeEventHandler), typeof(DiagramView));

        /// <summary>
        /// NodeUnSelected Event Handler
        /// </summary>
        /// Type: <see cref="NodeEventHandler"/>
        public event NodeEventHandler NodeUnSelected
        {
            add { AddHandler(NodeUnSelectedEvent, value); }
            remove { RemoveHandler(NodeUnSelectedEvent, value); }
        }

        /// <summary>
        ///  NodeDeleted Routed event. Is raised when the node is deleted.
        /// </summary>
        public static readonly RoutedEvent NodeDeletedEvent = EventManager.RegisterRoutedEvent(
        "NodeDeleted", RoutingStrategy.Bubble, typeof(NodeDeleteEventHandler), typeof(DiagramView));

        /// <summary>
        /// NodeDeleted Event Handler
        /// </summary>
        /// Type: <see cref="NodeDeleteEventHandler"/>
        public event NodeDeleteEventHandler NodeDeleted
        {
            add { AddHandler(NodeDeletedEvent, value); }
            remove { RemoveHandler(NodeDeletedEvent, value); }
        }

        /// <summary>
        ///  NodeDeleted Routed event. Is raised when the node is deleted.
        /// </summary>
        public static readonly RoutedEvent NodeDeletingEvent = EventManager.RegisterRoutedEvent(
        "NodeDeleting", RoutingStrategy.Bubble, typeof(NodeDeleteEventHandler), typeof(DiagramView));

        /// <summary>
        /// NodeDeleted Event Handler
        /// </summary>
        /// Type: <see cref="NodeDeleteEventHandler"/>
        public event NodeDeleteEventHandler NodeDeleting
        {
            add { AddHandler(NodeDeletingEvent, value); }
            remove { RemoveHandler(NodeDeletingEvent, value); }
        }

        /// <summary>
        ///  ConnectorDeleted Routed event. Is raised when the connector is deleted.
        /// </summary>
        public static readonly RoutedEvent ConnectorDeletedEvent = EventManager.RegisterRoutedEvent(
        "ConnectorDeleted", RoutingStrategy.Bubble, typeof(ConnectionDeleteEventHandler), typeof(DiagramView));

        /// <summary>
        /// ConnectorDeleted Event Handler
        /// </summary>
        /// Type: <see cref="ConnectionDeleteEventHandler"/>
        public event ConnectionDeleteEventHandler ConnectorDeleted
        {
            add { AddHandler(ConnectorDeletedEvent, value); }
            remove { RemoveHandler(ConnectorDeletedEvent, value); }
        }

        /// <summary>
        ///  ConnectorDeleted Routed event. Is raised when the connector is deleted.
        /// </summary>
        public static readonly RoutedEvent ConnectorDeletingEvent = EventManager.RegisterRoutedEvent(
        "ConnectorDeleting", RoutingStrategy.Bubble, typeof(ConnectionDeleteEventHandler), typeof(DiagramView));

        /// <summary>
        /// ConnectorDeleted Event Handler
        /// </summary>
        /// Type: <see cref="ConnectionDeleteEventHandler"/>
        public event ConnectionDeleteEventHandler ConnectorDeleting
        {
            add { AddHandler(ConnectorDeletingEvent, value); }
            remove { RemoveHandler(ConnectorDeletingEvent, value); }
        }

        /// <summary>
        ///  PreviewNodeDrop Routed event. Is raised when a node is dropped and just before a node object is created.
        /// </summary>
        public static readonly RoutedEvent PreviewNodeDropEvent = EventManager.RegisterRoutedEvent(
        "PreviewNodeDrop", RoutingStrategy.Bubble, typeof(PreviewNodeDropEventHandler), typeof(DiagramView));

        /// <summary>
        /// PreviewNodeDrop Event Handler
        /// </summary>
        /// Type: <see cref="PreviewNodeDropEventHandler"/>
        public event PreviewNodeDropEventHandler PreviewNodeDrop
        {
            add { AddHandler(PreviewNodeDropEvent, value); }
            remove { RemoveHandler(PreviewNodeDropEvent, value); }
        }

        /// <summary>
        ///  PreviewConnectorDrop Routed event. Is raised when the connector is dropped and before a line object is created.
        /// </summary>
        public static readonly RoutedEvent PreviewConnectorDropEvent = EventManager.RegisterRoutedEvent(
        "PreviewConnectorDrop", RoutingStrategy.Bubble, typeof(PreviewConnectorDropEventHandler), typeof(DiagramView));

        /// <summary>
        /// PreviewConnectorDrop Event Handler
        /// </summary>
        /// Type: <see cref="PreviewConnectorDropEventHandler"/>
        public event PreviewConnectorDropEventHandler PreviewConnectorDrop
        {
            add { AddHandler(PreviewConnectorDropEvent, value); }
            remove { RemoveHandler(PreviewConnectorDropEvent, value); }
        }

        /// <summary>
        ///  NodeDragStart Routed event. Is raised when the node drag is started.
        /// </summary>
        public static readonly RoutedEvent NodeDragStartEvent = EventManager.RegisterRoutedEvent(
        "NodeDragStart", RoutingStrategy.Bubble, typeof(NodeEventHandler), typeof(DiagramView));

        /// <summary>
        ///  NodeDragStart Event Handler .
        /// </summary>
        /// Type: <see cref="NodeEventHandler"/>
        public event NodeEventHandler NodeDragStart
        {
            add { AddHandler(NodeDragStartEvent, value); }
            remove { RemoveHandler(NodeDragStartEvent, value); }
        }

        /// <summary>
        ///  NodeDragEnd Routed event. Is raised when the node drag is completed.
        /// </summary>
        public static readonly RoutedEvent NodeDragEndEvent = EventManager.RegisterRoutedEvent(
        "NodeDragEnd", RoutingStrategy.Bubble, typeof(NodeEventHandler), typeof(DiagramView));

        /// <summary>
        ///  NodeDragStart Event Handler .
        /// </summary>
        /// Type: <see cref="NodeEventHandler"/>
        public event NodeEventHandler NodeDragEnd
        {
            add { AddHandler(NodeDragEndEvent, value); }
            remove { RemoveHandler(NodeDragEndEvent, value); }
        }

        /// <summary>
        ///  NodeClick Routed event. Is raised when the node is clicked.
        /// </summary>
        public static readonly RoutedEvent NodeClickEvent = EventManager.RegisterRoutedEvent(
          "NodeClick", RoutingStrategy.Bubble, typeof(NodeEventHandler), typeof(DiagramView));


        public static readonly RoutedEvent ConnectorClickEvent = EventManager.RegisterRoutedEvent(
            "ConnectorClick", RoutingStrategy.Bubble, typeof(ConnectorRoutedEventHandler), typeof(DiagramView));
        /// <summary>
        ///  NodeClick Event Handler .
        /// </summary>
        /// Type: <see cref="NodeEventHandler"/>
        public event NodeEventHandler NodeClick
        {
            add { AddHandler(NodeClickEvent, value); }
            remove { RemoveHandler(NodeClickEvent, value); }
        }

        public event ConnectorRoutedEventHandler ConnectorClick
        {
            add { AddHandler(ConnectorClickEvent, value); }
            remove {  RemoveHandler(ConnectorClickEvent, value);}
        }

        /// <summary>
        ///  NodeDoubleClick Routed event. Is raised when the node is clicked twice in succession.
        /// </summary>
        public static readonly RoutedEvent NodeDoubleClickEvent = EventManager.RegisterRoutedEvent(
       "NodeDoubleClick", RoutingStrategy.Bubble, typeof(NodeEventHandler), typeof(DiagramView));

        /// <summary>
        ///  NodeDoubleClick Event Handler .
        /// </summary>
        /// Type: <see cref="NodeEventHandler"/>
        public event NodeEventHandler NodeDoubleClick
        {
            add { AddHandler(NodeDoubleClickEvent, value); }
            remove { RemoveHandler(NodeDoubleClickEvent, value); }
        }

        /// <summary>
        ///  ConnectorDoubleClick Routed event. Is raised when the Connector is clicked twice in succession.
        /// </summary>
        public static readonly RoutedEvent ConnectorDoubleClickEvent = EventManager.RegisterRoutedEvent(
      "ConnectorDoubleClick", RoutingStrategy.Bubble, typeof(ConnChangedEventHandler), typeof(DiagramView));

        /// <summary>
        ///  ConnectorDoubleClick Event Handler .
        /// </summary>
        /// Type: <see cref="ConnChangedEventHandler"/>
        public event ConnChangedEventHandler ConnectorDoubleClick
        {
            add { AddHandler(ConnectorDoubleClickEvent, value); }
            remove { RemoveHandler(ConnectorDoubleClickEvent, value); }
        }

        /// <summary>
        ///  NodeDrop Routed event. Is raised when the node is dropped on the page from the symbol palette.
        /// </summary>
        public static readonly RoutedEvent NodeDropEvent = EventManager.RegisterRoutedEvent(
     "NodeDrop", RoutingStrategy.Bubble, typeof(NodeDroppedEventHandler), typeof(DiagramView));

        /// <summary>
        ///  NodeDropped Event Handler .
        /// </summary>
        /// Type: <see cref="NodeDroppedEventHandler"/>
        public event NodeDroppedEventHandler NodeDrop
        {
            add { AddHandler(NodeDropEvent, value); }
            remove { RemoveHandler(NodeDropEvent, value); }
        }

        /// <summary>
        ///  ConnectorDrop Routed event. Is raised when the Connector is dropped on the page from the symbol palette.
        /// </summary>
        public static readonly RoutedEvent ConnectorDropEvent = EventManager.RegisterRoutedEvent(
     "ConnectorDrop", RoutingStrategy.Bubble, typeof(ConnectorDroppedEventHandler), typeof(DiagramView));

        /// <summary>
        ///  ConnectorDropped Event Handler .
        /// </summary>
        /// Type: <see cref="ConnectorDroppedEventHandler"/>
        public event ConnectorDroppedEventHandler ConnectorDrop
        {
            add { AddHandler(ConnectorDropEvent, value); }
            remove { RemoveHandler(ConnectorDropEvent, value); }
        }

        /// <summary>
        ///  NodeResized Routed event. Is raised when the node is resized.
        /// </summary>
        public static readonly RoutedEvent NodeResizedEvent = EventManager.RegisterRoutedEvent(
      "NodeResized", RoutingStrategy.Bubble, typeof(NodeEventHandler), typeof(DiagramView));

        /// <summary>
        ///  NodeResized Event Handler .
        /// </summary>
        /// Type: <see cref="NodeEventHandler"/>
        public event NodeEventHandler NodeResized
        {
            add { AddHandler(NodeResizedEvent, value); }
            remove { RemoveHandler(NodeResizedEvent, value); }
        }

        /// <summary>
        ///  NodeResized Routed event. Is raised when the node is being resized.
        /// </summary>
        public static readonly RoutedEvent NodeResizingEvent = EventManager.RegisterRoutedEvent(
    "NodeResizing", RoutingStrategy.Bubble, typeof(NodeEventHandler), typeof(DiagramView));

        /// <summary>
        ///  NodeResizing Event Handler .
        /// </summary>
        /// Type: <see cref="NodeEventHandler"/>
        public event NodeEventHandler NodeResizing
        {
            add { AddHandler(NodeResizingEvent, value); }
            remove { RemoveHandler(NodeResizingEvent, value); }
        }

        /// <summary>
        ///  NodeRotationChanged Routed event. Is raised when the node is  rotated.
        /// </summary>
        public static readonly RoutedEvent NodeRotationChangedEvent = EventManager.RegisterRoutedEvent(
   "NodeRotationChanged", RoutingStrategy.Bubble, typeof(NodeEventHandler), typeof(DiagramView));

        /// <summary>
        ///  NodeRotationChanged Event Handler .
        /// </summary>
        /// Type: <see cref="NodeEventHandler"/>
        public event NodeEventHandler NodeRotationChanged
        {
            add { AddHandler(NodeRotationChangedEvent, value); }
            remove { RemoveHandler(NodeRotationChangedEvent, value); }
        }

        /// <summary>
        ///  NodeRotationChanging Routed event. Is raised when the node is being  rotated.
        /// </summary>
        public static readonly RoutedEvent NodeRotationChangingEvent = EventManager.RegisterRoutedEvent(
  "NodeRotationChanging", RoutingStrategy.Bubble, typeof(NodeEventHandler), typeof(DiagramView));

        /// <summary>
        ///  NodeRotationChanging Event Handler .
        /// </summary>
        /// Type: <see cref="NodeEventHandler"/>
        public event NodeEventHandler NodeRotationChanging
        {
            add { AddHandler(NodeRotationChangingEvent, value); }
            remove { RemoveHandler(NodeRotationChangingEvent, value); }
        }

        /// <summary>
        ///  ConnectorDragStart Routed event. Is raised when the connector drag is started.
        /// </summary>
        public static readonly RoutedEvent ConnectorDragStartEvent = EventManager.RegisterRoutedEvent(
      "ConnectorDragStart", RoutingStrategy.Bubble, typeof(ConnDragChangedEventHandler), typeof(DiagramView));

        /// <summary>
        ///  ConnectorDragStart Event Handler .
        /// </summary>
        /// Type: <see cref="ConnDragChangedEventHandler"/>
        public event ConnDragChangedEventHandler ConnectorDragStart
        {
            add { AddHandler(ConnectorDragStartEvent, value); }
            remove { RemoveHandler(ConnectorDragStartEvent, value); }
        }

        /// <summary>
        ///  ConnectorDragEnd Routed event. Is raised when the connector drag is completed.
        /// </summary>
        public static readonly RoutedEvent ConnectorDragEndEvent = EventManager.RegisterRoutedEvent(
     "ConnectorDragEnd", RoutingStrategy.Bubble, typeof(ConnDragEndChangedEventHandler), typeof(DiagramView));

        /// <summary>
        ///  ConnectorDragEnd Event Handler .
        /// </summary>
        /// Type: <see cref="ConnDragEndChangedEventHandler"/>
        public event ConnDragEndChangedEventHandler ConnectorDragEnd
        {
            add { AddHandler(ConnectorDragEndEvent, value); }
            remove { RemoveHandler(ConnectorDragEndEvent, value); }
        }

        /// <summary>
        ///  HeadNodeChanged Routed event. Is raised when the connector's head node is changed.
        /// </summary>
        public static readonly RoutedEvent HeadNodeChangedEvent = EventManager.RegisterRoutedEvent(
    "HeadNodeChanged", RoutingStrategy.Bubble, typeof(NodeChangedEventHandler), typeof(DiagramView));

        /// <summary>
        ///  HeadNodeChanged Event Handler .
        /// </summary>
        /// Type: <see cref="NodeChangedEventHandler"/>
        public event NodeChangedEventHandler HeadNodeChanged
        {
            add { AddHandler(HeadNodeChangedEvent, value); }
            remove { RemoveHandler(HeadNodeChangedEvent, value); }
        }

        /// <summary>
        ///  TailNodeChanged Routed event. Is raised when the connector's tail node is changed.
        /// </summary>
        public static readonly RoutedEvent TailNodeChangedEvent = EventManager.RegisterRoutedEvent(
   "TailNodeChanged", RoutingStrategy.Bubble, typeof(NodeChangedEventHandler), typeof(DiagramView));

        /// <summary>
        ///  TailNodeChanged Event Handler .
        /// </summary>
        /// Type: <see cref="NodeChangedEventHandler"/>
        public event NodeChangedEventHandler TailNodeChanged
        {
            add { AddHandler(TailNodeChangedEvent, value); }
            remove { RemoveHandler(TailNodeChangedEvent, value); }
        }

        /// <summary>
        ///  NodeLabelChanged Routed event. Is raised when the node's label is changed.
        /// </summary>
        public static readonly RoutedEvent NodeLabelChangedEvent = EventManager.RegisterRoutedEvent(
   "NodeLabelChanged", RoutingStrategy.Bubble, typeof(LabelChangedEventHandler), typeof(DiagramView));

        /// <summary>
        ///  NodeLabelChanged Event Handler .
        /// </summary>
        /// Type: <see cref="LabelChangedEventHandler"/>
        public event LabelChangedEventHandler NodeLabelChanged
        {
            add { AddHandler(NodeLabelChangedEvent, value); }
            remove { RemoveHandler(NodeLabelChangedEvent, value); }
        }

        /// <summary>
        ///  NodeStartLabelEdit Routed event. Is raised when the node's label is started to be edited.
        /// </summary>
        public static readonly RoutedEvent NodeStartLabelEditEvent = EventManager.RegisterRoutedEvent(
"NodeStartLabelEdit", RoutingStrategy.Bubble, typeof(LabelChangedEventHandler), typeof(DiagramView));

        /// <summary>
        ///  LabelChangedEventHandler Event Handler .
        /// </summary>
        /// Type: <see cref="LabelChangedEventHandler"/>
        public event LabelChangedEventHandler NodeStartLabelEdit
        {
            add { AddHandler(NodeStartLabelEditEvent, value); }
            remove { RemoveHandler(NodeStartLabelEditEvent, value); }
        }

        /// <summary>
        ///  ConnectorLabelChanged Routed event. Is invoked when the connector's label is changed.
        /// </summary>
        public static readonly RoutedEvent ConnectorLabelChangedEvent = EventManager.RegisterRoutedEvent(
"ConnectorLabelChanged", RoutingStrategy.Bubble, typeof(LabelConnChangedEventHandler), typeof(DiagramView));

        /// <summary>
        ///  LabelChangedEventHandler Event Handler .
        /// </summary>
        /// Type: <see cref="LabelConnChangedEventHandler"/>
        public event LabelConnChangedEventHandler ConnectorLabelChanged
        {
            add { AddHandler(ConnectorLabelChangedEvent, value); }
            remove { RemoveHandler(ConnectorLabelChangedEvent, value); }
        }

        /// <summary>
        ///  ConnectorStartLabelEdit Routed event. Is raised when the connections's label is started to be edited.
        /// </summary>
        public static readonly RoutedEvent ConnectorStartLabelEditEvent = EventManager.RegisterRoutedEvent(
"ConnectorStartLabelEdit", RoutingStrategy.Bubble, typeof(LabelEditConnChangedEventHandler), typeof(DiagramView));

        /// <summary>
        ///  ConnectorStartLabelEdit Event Handler .
        /// </summary>
        /// Type: <see cref="LabelEditConnChangedEventHandler"/>
        public event LabelEditConnChangedEventHandler ConnectorStartLabelEdit
        {
            add { AddHandler(ConnectorStartLabelEditEvent, value); }
            remove { RemoveHandler(ConnectorStartLabelEditEvent, value); }
        }

        /// <summary>
        ///  BeforeConnectionCreate Routed event. Is raised just before the user starts to create a new connection.
        /// </summary>
        public static readonly RoutedEvent BeforeConnectionCreateEvent = EventManager.RegisterRoutedEvent(
      "BeforeConnectionCreate", RoutingStrategy.Bubble, typeof(BeforeCreateConnectionEventHandler), typeof(DiagramView));

        /// <summary>
        ///  BeforeConnectionCreate Event Handler .
        /// </summary>
        /// Type: <see cref="BeforeCreateConnectionEventHandler"/>
        public event BeforeCreateConnectionEventHandler BeforeConnectionCreate
        {
            add { AddHandler(BeforeConnectionCreateEvent, value); }
            remove { RemoveHandler(BeforeConnectionCreateEvent, value); }
        }

        /// <summary>
        ///  AfterConnectionCreate Routed event. Is raised when a new connection has been made.
        /// </summary>
        public static readonly RoutedEvent AfterConnectionCreateEvent = EventManager.RegisterRoutedEvent(
      "AfterConnectionCreate", RoutingStrategy.Bubble, typeof(ConnDragEndChangedEventHandler), typeof(DiagramView));

        /// <summary>
        ///  AfterConnectionCreate Event Handler .
        /// </summary>
        /// Type: <see cref="BeforeCreateConnectionEventHandler"/>
        public event ConnDragEndChangedEventHandler AfterConnectionCreate
        {
            add { AddHandler(AfterConnectionCreateEvent, value); }
            remove { RemoveHandler(AfterConnectionCreateEvent, value); }
        }
        public static readonly RoutedEvent GroupingEvent = EventManager.RegisterRoutedEvent(
            "Grouping", RoutingStrategy.Bubble, typeof(GroupEventHandler), typeof(DiagramView));


        public event GroupEventHandler Grouping
        {
            add { AddHandler(GroupingEvent, value); }
            remove { RemoveHandler(GroupingEvent, value); }
        }

        public static readonly RoutedEvent GroupedEvent = EventManager.RegisterRoutedEvent(
            "Grouped", RoutingStrategy.Bubble, typeof(GroupEventHandler), typeof(DiagramView));


        public event GroupEventHandler Grouped
        {
            add { AddHandler(GroupedEvent, value); }
            remove { RemoveHandler(GroupedEvent, value); }
        }

        public static readonly RoutedEvent UngroupingEvent = EventManager.RegisterRoutedEvent(
            "Ungrouping", RoutingStrategy.Bubble, typeof(UnGroupEventHandler), typeof(DiagramView));


        public event UnGroupEventHandler Ungrouping
        {
            add { AddHandler(UngroupingEvent, value); }
            remove { RemoveHandler(UngroupingEvent, value); }
        }
        public static readonly RoutedEvent UngroupedEvent = EventManager.RegisterRoutedEvent(
            "Ungrouped", RoutingStrategy.Bubble, typeof(UnGroupEventHandler), typeof(DiagramView));


        public event UnGroupEventHandler Ungrouped
        {
            add { AddHandler(UngroupedEvent, value); }
            remove { RemoveHandler(UngroupedEvent, value); }
        }

        public static readonly RoutedEvent ConnectorSelectedEvent = EventManager.RegisterRoutedEvent("ConnectorSelected", RoutingStrategy.Bubble, typeof(ConnectorSelectedEventHandler), typeof(DiagramView));

        public event ConnectorSelectedEventHandler ConnectorSelected
        {
            add { AddHandler(ConnectorSelectedEvent, value); }
            remove { RemoveHandler(ConnectorSelectedEvent, value); }
        }

        public static readonly RoutedEvent ConnectorUnSelectedEvent = EventManager.RegisterRoutedEvent("ConnectorUnSelected", RoutingStrategy.Bubble, typeof(ConnectorUnSelectedEventHandler), typeof(DiagramView));

        public event ConnectorUnSelectedEventHandler ConnectorUnSelected
        {
            add { AddHandler(ConnectorUnSelectedEvent, value); }
            remove { RemoveHandler(ConnectorUnSelectedEvent, value); }
        }


        #endregion

        #region Properties


        /// <summary>
        /// Gets or sets the HorizontalOffset value of the grid .
        /// </summary>
        /// <value>
        /// Type: <see cref="double"/>
        /// Offset value.
        /// </value>
        /// <remarks>
        /// Default value is 25d.</remarks>
        /// <example>
        /// <code language="C#">
        /// using Syncfusion.Core;
        /// using Syncfusion.Windows.Diagram;
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        ///    public DiagramControl Control;
        ///    public DiagramModel Model;
        ///    public DiagramView View;
        ///    public Window1 ()
        ///    {
        ///       InitializeComponent ();
        ///       Control = new DiagramControl ();
        ///       View = new DiagramView ();
        ///       Control.View = View;
        ///       HorizontalRuler hruler = new HorizontalRuler();
        ///       View.HorizontalRuler = hruler;
        ///       View.ShowHorizontalGridLine = false;
        ///       View.ShowVerticalGridLine = false;
        ///       VerticalRuler vruler = new VerticalRuler();
        ///       View.VerticalRuler = vruler;
        ///       View.Bounds = new Thickness (0, 0, 1000, 1000);
        ///       View.IsPageEditable = true;
        ///       (View.Page as DiagramPage).GridHorizontalOffset=100;
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        
        /// <summary>
        /// Gets or sets the VerticalOffset value of the grid .
        /// </summary>
        /// <value>
        /// Type: <see cref="double"/>
        /// Offset value.
        /// </value>
        /// <remarks>
        /// Default value is 25d.</remarks>
        /// <example>
        /// <code language="C#">
        /// using Syncfusion.Core;
        /// using Syncfusion.Windows.Diagram;
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        ///    public DiagramControl Control;
        ///    public DiagramModel Model;
        ///    public DiagramView View;
        ///    public Window1 ()
        ///    {
        ///       InitializeComponent ();
        ///       Control = new DiagramControl ();
        ///       View = new DiagramView ();
        ///       Control.View = View;
        ///       HorizontalRuler hruler = new HorizontalRuler();
        ///       View.HorizontalRuler = hruler;
        ///       View.ShowHorizontalGridLine = false;
        ///       View.ShowVerticalGridLine = false;
        ///       VerticalRuler vruler = new VerticalRuler();
        ///       View.VerticalRuler = vruler;
        ///       View.Bounds = new Thickness (0, 0, 1000, 1000);
        ///       View.IsPageEditable = true;
        ///       (View.Page as DiagramPage).GridVerticalOffset=100;
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
       
        internal static readonly DependencyProperty PageMarginProperty = DependencyProperty.Register("PageMargin", typeof(Thickness), typeof(DiagramView), new PropertyMetadata());

        public Thickness PageMargin
        {
            get
            {
                return (Thickness)GetValue(PageMarginProperty);
            }

            set
            {
                SetValue(PageMarginProperty, value);
            }

        }

        public double SnapOffsetX
        {
            get
            {
                return (double)GetValue(SnapOffsetXProperty);
            }

            set
            {
                SetValue(SnapOffsetXProperty, value);
            }
        }
                
        public double SnapOffsetY
        {
            get
            {
                return (double)GetValue(SnapOffsetYProperty);
            }

            set
            {
                SetValue(SnapOffsetYProperty, value);
            }
        }

        public bool SnapToHorizontalGrid
        {
            get
            {
                return (bool)GetValue(SnapToHorizontalGridProperty);
            }

            set
            {
                SetValue(SnapToHorizontalGridProperty, value);
            }
        }

        public bool SnapToVerticalGrid
        {
            get
            {
                return (bool)GetValue(SnapToVerticalGridProperty);
            }

            set
            {
                SetValue(SnapToVerticalGridProperty, value);
            }
        }

        public bool IsCutEnabled
        {
            get
            {
                return (bool)GetValue(IsCutEnabledProperty);
            }

            set
            {
                SetValue(IsCutEnabledProperty, value);
            }
        }

        

        public bool EnableDrawingTools
        {
             get
            {
                return (bool)GetValue(EnableDrawingToolsProperty );
            }

            set
            {
                SetValue(EnableDrawingToolsProperty , value);
            }
        }
        public bool IsCopyEnabled
        {
            get
            {
                return (bool)GetValue(IsCopyEnabledProperty);
            }

            set
            {
                SetValue(IsCopyEnabledProperty, value);
            }
        }

        public bool IsPasteEnabled
        {
            get
            {
                return (bool)GetValue(IsPasteEnabledProperty);
            }

            set
            {
                SetValue(IsPasteEnabledProperty, value);
            }
        }

        /*static void l_LayerPropertyChanged(object sender, LineNudgeEventArgs evtArgs)
        {
        }*/

        /// <summary>
        /// Gets or sets the port visibility.
        /// </summary>
        /// <value>The port visibility.</value>
        /// <remarks>
        /// Setting PortVisibility from DiagramView applies to all the nodes in the page. However if any node has specifically set PortVisibility, then the node's PortVisibility property will be taken into account only for that node. So even if DiagramView's PortVisibility is set to false, if Node's <see cref="T:Syncfusion.Windows.Diagram.Node.PortVisibility"/> is set to true then the ports will be displayed for that node.
        /// </remarks>
        /// <example>
        /// <code language="XAML">
        /// &lt;Window x:Class="RulersAndUnits.Window1"
        /// xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        /// xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        /// xmlns:syncfusion="http://schemas.syncfusion.com/wpf"
        /// Title="Rulers And Units Demo"  WindowState="Maximized" Name="mainwindow" 
        ///  xmlns:local="clr-namespace:Sample" FontWeight="Bold"
        ///  Icon="Images/App.ico" &gt;
        ///   &lt;syncfusion:DiagramControl Grid.Column="1" Name="diagramControl" 
        ///                                IsSymbolPaletteEnabled="True" 
        ///                                Background="WhiteSmoke"&gt;
        ///           &lt;syncfusion:DiagramControl.View&gt;
        ///              &lt;syncfusion:DiagramView  IsPageEditable="True" 
        ///                                        Background="LightGray"  
        ///                                        Bounds="0,0,12,12" 
        ///                                        PortVisibility="False"
        ///                                        Name="diagramView"  &gt;
        ///             &lt;syncfusion:HorizontalRuler Name="horizontalRuler" /&gt;
        ///          &lt;/syncfusion:DiagramView.HorizontalRuler&gt;
        ///           &lt;syncfusion:DiagramView.VerticalRuler&gt;
        ///               &lt;syncfusion:VerticalRuler    Name="verticalRuler" /&gt;
        ///           &lt;/syncfusion:DiagramView.VerticalRuler &gt;
        ///       &lt;/syncfusion:DiagramView&gt;
        ///    &lt;/syncfusion:DiagramControl.View&gt;
        /// &lt;/syncfusion:DiagramControl&gt;
        /// &lt;/Window&gt;
        /// </code>
        /// <code language="C#">
        /// using Syncfusion.Windows.Diagram;
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        ///    public DiagramControl Control;
        ///    public DiagramModel Model;
        ///    public DiagramView View;
        ///    public Window1 ()
        ///    {
        ///       InitializeComponent ();
        ///       Control = new DiagramControl ();
        ///       View = new DiagramView ();
        ///       Control.View = View;
        ///       HorizontalRuler hruler = new HorizontalRuler();
        ///       View.HorizontalRuler = hruler;
        ///       VerticalRuler vruler = new VerticalRuler();
        ///       View.VerticalRuler = vruler;
        ///       View.Bounds = new Thickness (0, 0, 1000, 1000);
        ///       View.PortVisibility=false;
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        public Visibility PortVisibility
        {
            get
            {
                return (Visibility)GetValue(PortVisibilityProperty);
            }

            set
            {
                SetValue(PortVisibilityProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable connection].
        /// </summary>
        /// <value><c>true</c> if [enable connection]; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// This property is generally placed in the click event handler for setting the <see cref="T:Syncfusion.Windows.Diagram.LineConnector.ConnectorType"/>.
        /// </remarks>
        /// <example>
        /// <code language="XAML">
        /// &lt;Window x:Class="RulersAndUnits.Window1"
        /// xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        /// xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        /// xmlns:syncfusion="http://schemas.syncfusion.com/wpf"
        /// Title="Rulers And Units Demo"  WindowState="Maximized" Name="mainwindow" 
        ///  xmlns:local="clr-namespace:Sample" FontWeight="Bold"
        ///  Icon="Images/App.ico" &gt;
        ///   &lt;syncfusion:DiagramControl Grid.Column="1" Name="diagramControl" 
        ///                                IsSymbolPaletteEnabled="True" 
        ///                                Background="WhiteSmoke"&gt;
        ///           &lt;syncfusion:DiagramControl.View&gt;
        ///              &lt;syncfusion:DiagramView EnableConnection="True"
        ///                                        Bounds="0,0,12,12"  
        ///                                        Name="diagramView"  &gt;
        ///             &lt;syncfusion:HorizontalRuler Name="horizontalRuler" /&gt;
        ///          &lt;/syncfusion:DiagramView.HorizontalRuler&gt;
        ///           &lt;syncfusion:DiagramView.VerticalRuler&gt;
        ///               &lt;syncfusion:VerticalRuler    Name="verticalRuler" /&gt;
        ///           &lt;/syncfusion:DiagramView.VerticalRuler &gt;
        ///       &lt;/syncfusion:DiagramView&gt;
        ///    &lt;/syncfusion:DiagramControl.View&gt;
        /// &lt;/syncfusion:DiagramControl&gt;
        /// &lt;/Window&gt;
        /// </code>
        /// <code language="C#">
        /// using System;
        /// using System.Collections.Generic;
        /// using System.Linq;
        /// using System.Text;
        /// using System.Windows;
        /// using System.Windows.Controls;
        /// using System.Windows.Data;
        /// using System.Windows.Documents;
        /// using System.Windows.Input;
        /// using System.Windows.Media;
        /// using System.Windows.Media.Imaging;
        /// using System.Windows.Navigation;
        /// using System.Windows.Shapes;
        /// using System.ComponentModel;
        /// using Syncfusion.Core;
        /// using Syncfusion.Windows.Diagram;
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        ///    public DiagramControl Control;
        ///    public DiagramModel Model;
        ///    public DiagramView View;
        ///    public Window1 ()
        ///    {
        ///       InitializeComponent ();
        ///       Control = new DiagramControl ();
        ///       View = new DiagramView ();
        ///       Control.View = View;
        ///       HorizontalRuler hruler = new HorizontalRuler();
        ///       View.HorizontalRuler = hruler;
        ///       View.ShowHorizontalGridLine = false;
        ///       View.ShowVerticalGridLine = false;
        ///       VerticalRuler vruler = new VerticalRuler();
        ///       View.VerticalRuler = vruler;
        ///       View.Bounds = new Thickness (0, 0, 1000, 1000);
        ///       View.IsPageEditable = true;
        ///       View.EnableConnection=true;
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        public bool EnableConnection
        {
            get
            {
                return (bool)GetValue(EnableConnectionProperty);
            }

            set
            {
                SetValue(EnableConnectionProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the type of shape to be used.
        /// </summary>
        /// <value>
        /// Type: <see cref="DrawingTools"/>
        /// Enum specifying the type of the Shape to be used.
        /// </value>
        /// <example>
        /// <para/>This example shows how to set DrawingTools in C#.
        /// <code language="C#">
        /// diagramView.DrawingTool = DrawingTools.Ellipse;
        /// </code>
        /// </example>

        public DrawingTools DrawingTool
        {
            get { return (DrawingTools)GetValue(DrawingToolsProperty); }
            set { SetValue(DrawingToolsProperty, value); }
        }


        /// <summary>DrawingToolsProperty
        /// Gets or sets the page's position (0,0) with respect to the View.
        /// </summary>
        internal Point ViewGridOrigin
        {
            get
            {
                return (Point)GetValue(ViewGridOriginProperty);
            }

            set
            {
                SetValue(ViewGridOriginProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the current zoom.
        /// </summary>
        internal double CurrentZoom
        {
            get
            {
                return (double)GetValue(CurrentZoomProperty);
            }

            set
            {
                SetValue(CurrentZoomProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the ZoomFactor .
        /// </summary>
        /// <value>
        /// Type: <see cref="double"/>
        /// Zoom Factor in pixels.
        /// </value>
        /// <remarks>
        /// Default value is 0.2d .
        /// </remarks>
        /// <example>
        /// <para/>The following example shows how to create a <see cref="DiagramView"/> in XAML.
        /// <code language="XAML">
        /// &lt;Window x:Class="RulersAndUnits.Window1"
        /// xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        /// xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        /// xmlns:syncfusion="http://schemas.syncfusion.com/wpf"
        /// Title="Rulers And Units Demo"  WindowState="Maximized" Name="mainwindow" 
        ///  xmlns:local="clr-namespace:Sample" FontWeight="Bold"
        ///  Icon="Images/App.ico" &gt;
        ///   &lt;syncfusion:DiagramControl Grid.Column="1" Name="diagramControl" 
        ///                                IsSymbolPaletteEnabled="True" 
        ///                                Background="WhiteSmoke"&gt;
        ///           &lt;syncfusion:DiagramControl.View&gt;
        ///              &lt;syncfusion:DiagramView ZoomFactor="1"
        ///                                        Bounds="0,0,12,12"  
        ///                                        Name="diagramView"  &gt;
        ///             &lt;syncfusion:HorizontalRuler Name="horizontalRuler" /&gt;
        ///          &lt;/syncfusion:DiagramView.HorizontalRuler&gt;
        ///           &lt;syncfusion:DiagramView.VerticalRuler&gt;
        ///               &lt;syncfusion:VerticalRuler    Name="verticalRuler" /&gt;
        ///           &lt;/syncfusion:DiagramView.VerticalRuler &gt;
        ///       &lt;/syncfusion:DiagramView&gt;
        ///    &lt;/syncfusion:DiagramControl.View&gt;
        /// &lt;/syncfusion:DiagramControl&gt;
        /// &lt;/Window&gt;
        /// </code>
        /// <para/>The following example shows how to create a <see cref="DiagramView"/> in C#.
        /// <code language="C#">
        /// using System;
        /// using System.Collections.Generic;
        /// using System.Linq;
        /// using System.Text;
        /// using System.Windows;
        /// using System.Windows.Controls;
        /// using System.Windows.Data;
        /// using System.Windows.Documents;
        /// using System.Windows.Input;
        /// using System.Windows.Media;
        /// using System.Windows.Media.Imaging;
        /// using System.Windows.Navigation;
        /// using System.Windows.Shapes;
        /// using System.ComponentModel;
        /// using Syncfusion.Core;
        /// using Syncfusion.Windows.Diagram;
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        ///    public DiagramControl Control;
        ///    public DiagramModel Model;
        ///    public DiagramView View;
        ///    public Window1 ()
        ///    {
        ///       InitializeComponent ();
        ///       Control = new DiagramControl ();
        ///       View = new DiagramView ();
        ///       Control.View = View;
        ///       HorizontalRuler hruler = new HorizontalRuler();
        ///       View.HorizontalRuler = hruler;
        ///       View.ShowHorizontalGridLine = false;
        ///       View.ShowVerticalGridLine = false;
        ///       VerticalRuler vruler = new VerticalRuler();
        ///       View.VerticalRuler = vruler;
        ///       View.Bounds = new Thickness (0, 0, 1000, 1000);
        ///       View.IsPageEditable = true;
        ///       View.ZoomFactor=1;
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        public double ZoomFactor
        {
            get
            {
                return (double)GetValue(ZoomFactorProperty);
            }

            set
            {
                SetValue(ZoomFactorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is zoom enabled.
        /// </summary>
        /// <value>
        /// Type: <see cref="Boolean"/>
        /// True, if zooming is enabled, false otherwise.
        /// </value>
        /// <example>
        /// <code language="XAML">
        /// &lt;Window x:Class="RulersAndUnits.Window1"
        /// xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        /// xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        /// xmlns:syncfusion="http://schemas.syncfusion.com/wpf"
        /// Title="Rulers And Units Demo"  WindowState="Maximized" Name="mainwindow" 
        ///  xmlns:local="clr-namespace:Sample" FontWeight="Bold"
        ///  Icon="Images/App.ico" &gt;
        ///   &lt;syncfusion:DiagramControl Grid.Column="1" Name="diagramControl" 
        ///                                IsSymbolPaletteEnabled="True" 
        ///                                Background="WhiteSmoke"&gt;
        ///           &lt;syncfusion:DiagramControl.View&gt;
        ///              &lt;syncfusion:DiagramView  IsZoomEnabled="True" 
        ///                                        Background="LightGray"  
        ///                                        Bounds="0,0,12,12" 
        ///                                        Name="diagramView"  &gt;
        ///             &lt;syncfusion:HorizontalRuler Name="horizontalRuler" /&gt;
        ///          &lt;/syncfusion:DiagramView.HorizontalRuler&gt;
        ///           &lt;syncfusion:DiagramView.VerticalRuler&gt;
        ///               &lt;syncfusion:VerticalRuler    Name="verticalRuler" /&gt;
        ///           &lt;/syncfusion:DiagramView.VerticalRuler &gt;
        ///       &lt;/syncfusion:DiagramView&gt;
        ///    &lt;/syncfusion:DiagramControl.View&gt;
        /// &lt;/syncfusion:DiagramControl&gt;
        /// &lt;/Window&gt;
        /// </code>
        /// <code language="C#">
        /// using Syncfusion.Windows.Diagram;
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        ///    public DiagramControl Control;
        ///    public DiagramModel Model;
        ///    public DiagramView View;
        ///    public Window1 ()
        ///    {
        ///       InitializeComponent ();
        ///       Control = new DiagramControl ();
        ///       View = new DiagramView ();
        ///       Control.View = View;
        ///       HorizontalRuler hruler = new HorizontalRuler();
        ///       View.HorizontalRuler = hruler;
        ///       VerticalRuler vruler = new VerticalRuler();
        ///       View.VerticalRuler = vruler;
        ///       View.Bounds = new Thickness (0, 0, 1000, 1000);
        ///       View.IsZoomEnabled=true;
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        public bool IsZoomEnabled
        {
            get
            {
                return (bool)GetValue(IsZoomEnabledProperty);
            }

            set
            {
                SetValue(IsZoomEnabledProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is page editable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is page editable; otherwise, <c>false</c>.
        /// </value>
        /// <summary>
        /// Gets or sets a value indicating whether [enable connection].
        /// </summary>
        /// <value><c>true</c> if [enable connection]; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// This property is generally placed in the click event handler for setting the <see cref="T:Syncfusion.Windows.Diagram.LineConnector.ConnectorType"/>.
        /// </remarks>
        /// <example>
        /// <code language="XAML">
        /// &lt;Window x:Class="RulersAndUnits.Window1"
        /// xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        /// xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        /// xmlns:syncfusion="http://schemas.syncfusion.com/wpf"
        /// Title="Rulers And Units Demo"  WindowState="Maximized" Name="mainwindow" 
        ///  xmlns:local="clr-namespace:Sample" FontWeight="Bold"
        ///  Icon="Images/App.ico" &gt;
        ///   &lt;syncfusion:DiagramControl Grid.Column="1" Name="diagramControl" 
        ///                                IsSymbolPaletteEnabled="True" 
        ///                                Background="WhiteSmoke"&gt;
        ///           &lt;syncfusion:DiagramControl.View&gt;
        ///              &lt;syncfusion:DiagramView IsPageEditable="True"
        ///                                        Bounds="0,0,12,12"  
        ///                                        Name="diagramView"  &gt;
        ///             &lt;syncfusion:HorizontalRuler Name="horizontalRuler" /&gt;
        ///          &lt;/syncfusion:DiagramView.HorizontalRuler&gt;
        ///           &lt;syncfusion:DiagramView.VerticalRuler&gt;
        ///               &lt;syncfusion:VerticalRuler    Name="verticalRuler" /&gt;
        ///           &lt;/syncfusion:DiagramView.VerticalRuler &gt;
        ///       &lt;/syncfusion:DiagramView&gt;
        ///    &lt;/syncfusion:DiagramControl.View&gt;
        /// &lt;/syncfusion:DiagramControl&gt;
        /// &lt;/Window&gt;
        /// </code>
        /// <code language="C#">
        /// using System;
        /// using System.Collections.Generic;
        /// using System.Linq;
        /// using System.Text;
        /// using System.Windows;
        /// using System.Windows.Controls;
        /// using System.Windows.Data;
        /// using System.Windows.Documents;
        /// using System.Windows.Input;
        /// using System.Windows.Media;
        /// using System.Windows.Media.Imaging;
        /// using System.Windows.Navigation;
        /// using System.Windows.Shapes;
        /// using System.ComponentModel;
        /// using Syncfusion.Core;
        /// using Syncfusion.Windows.Diagram;
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        ///    public DiagramControl Control;
        ///    public DiagramModel Model;
        ///    public DiagramView View;
        ///    public Window1 ()
        ///    {
        ///       InitializeComponent ();
        ///       Control = new DiagramControl ();
        ///       View = new DiagramView ();
        ///       Control.View = View;
        ///       HorizontalRuler hruler = new HorizontalRuler();
        ///       View.HorizontalRuler = hruler;
        ///       View.ShowHorizontalGridLine = false;
        ///       View.ShowVerticalGridLine = false;
        ///       VerticalRuler vruler = new VerticalRuler();
        ///       View.VerticalRuler = vruler;
        ///       View.Bounds = new Thickness (0, 0, 1000, 1000);
        ///       View.IsPageEditable = true;
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        public bool IsPageEditable
        {
            get
            {
                return (bool)GetValue(IsPageEditableProperty);
            }

            set
            {
                SetValue(IsPageEditableProperty, value);
            }
        }


        public bool EnableVirtualization
        {
            get
            {
                return (bool)GetValue(EnableVirtualizationProperty);
            }

            set
            {
                SetValue(EnableVirtualizationProperty, value);
            }
        }

        public bool EnableCaching
        {
            get
            {
                return (bool)GetValue(EnableCachingProperty);
            }

            set
            {
                SetValue(EnableCachingProperty, value);
            }
 
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is pan enabled.
        /// </summary>
        /// <value>
        /// Type: <see cref="Boolean"/>
        /// True, if panning is enabled, false otherwise.
        /// </value>
        /// <summary>
        /// Gets or sets a value indicating whether [enable connection].
        /// </summary>
        /// <value><c>true</c> if [enable connection]; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// This property is generally placed in the click event handler for setting the <see cref="T:Syncfusion.Windows.Diagram.LineConnector.ConnectorType"/>.
        /// </remarks>
        /// <example>
        /// <para/>The following example shows how to create a <see cref="DiagramView"/> in XAML.
        /// <code language="XAML">
        /// &lt;Window x:Class="RulersAndUnits.Window1"
        /// xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        /// xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        /// xmlns:syncfusion="http://schemas.syncfusion.com/wpf"
        /// Title="Rulers And Units Demo"  WindowState="Maximized" Name="mainwindow" 
        ///  xmlns:local="clr-namespace:Sample" FontWeight="Bold"
        ///  Icon="Images/App.ico" &gt;
        ///   &lt;syncfusion:DiagramControl Grid.Column="1" Name="diagramControl" 
        ///                                IsSymbolPaletteEnabled="True" 
        ///                                Background="WhiteSmoke"&gt;
        ///           &lt;syncfusion:DiagramControl.View&gt;
        ///              &lt;syncfusion:DiagramView IsPanEnabled="False"
        ///                                        Bounds="0,0,12,12"  
        ///                                        Name="diagramView"  &gt;
        ///             &lt;syncfusion:HorizontalRuler Name="horizontalRuler" /&gt;
        ///          &lt;/syncfusion:DiagramView.HorizontalRuler&gt;
        ///           &lt;syncfusion:DiagramView.VerticalRuler&gt;
        ///               &lt;syncfusion:VerticalRuler    Name="verticalRuler" /&gt;
        ///           &lt;/syncfusion:DiagramView.VerticalRuler &gt;
        ///       &lt;/syncfusion:DiagramView&gt;
        ///    &lt;/syncfusion:DiagramControl.View&gt;
        /// &lt;/syncfusion:DiagramControl&gt;
        /// &lt;/Window&gt;
        /// </code>
        /// <code language="C#">
        /// using System;
        /// using System.Collections.Generic;
        /// using System.Linq;
        /// using System.Text;
        /// using System.Windows;
        /// using System.Windows.Controls;
        /// using System.Windows.Data;
        /// using System.Windows.Documents;
        /// using System.Windows.Input;
        /// using System.Windows.Media;
        /// using System.Windows.Media.Imaging;
        /// using System.Windows.Navigation;
        /// using System.Windows.Shapes;
        /// using System.ComponentModel;
        /// using Syncfusion.Core;
        /// using Syncfusion.Windows.Diagram;
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        ///    public DiagramControl Control;
        ///    public DiagramModel Model;
        ///    public DiagramView View;
        ///    public Window1 ()
        ///    {
        ///       InitializeComponent ();
        ///       Control = new DiagramControl ();
        ///       View = new DiagramView ();
        ///       Control.View = View;
        ///       HorizontalRuler hruler = new HorizontalRuler();
        ///       View.HorizontalRuler = hruler;
        ///       View.ShowHorizontalGridLine = false;
        ///       View.ShowVerticalGridLine = false;
        ///       VerticalRuler vruler = new VerticalRuler();
        ///       View.VerticalRuler = vruler;
        ///       View.Bounds = new Thickness (0, 0, 1000, 1000);
        ///       View.IsPanEnabled=false;
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        public bool IsPanEnabled
        {
            get
            {
                return (bool)GetValue(IsPanEnabledProperty);
            }

            set
            {
                SetValue(IsPanEnabledProperty, value);
            }
        }

        /// <summary>
        /// Gets the currently selected item. .
        /// </summary>
        /// <value>
        /// Type: <see cref="object"/>
        /// Selected item.
        /// </value>
        public object SelectedItem
        {
            get
            {
                return this.GetValue(SelectedItemProperty);
            }
        }

        /// <summary>
        /// Gets or sets the page.
        /// </summary>
        /// <value>
        /// Type: <see cref="Panel"/>
        /// Panel instance.
        /// </value>
        [Browsable(false)]
        public Panel Page
        {
            get
            {
                return (Panel)GetValue(PageProperty);
            }

            set
            {
                SetValue(PageProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the horizontal grid line style.
        /// </summary>
        /// <value>
        /// Type: <see cref="Pen"/>
        /// Line Style.
        /// </value>
        public System.Windows.Media.Pen HorizontalGridLineStyle
        {
            get
            {
                return (System.Windows.Media.Pen)GetValue(HorizontalGridLineStyleProperty);
            }

            set
            {
                SetValue(HorizontalGridLineStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the vertical grid line style.
        /// </summary>
        /// <value>
        /// Type: <see cref="Pen"/>
        /// Line Style.
        /// </value>
        public System.Windows.Media.Pen VerticalGridLineStyle
        {
            get
            {
                return (System.Windows.Media.Pen)GetValue(VerticalGridLineStyleProperty);
            }

            set
            {
                SetValue(VerticalGridLineStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show horizontal grid line].
        /// </summary>
        /// <value>
        /// Type: <see cref="Boolean"/>
        /// True, if it is to be displayed, false otherwise.
        /// </value>
        /// <example>
        /// <code language="XAML">
        /// &lt;Window x:Class="RulersAndUnits.Window1"
        /// xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        /// xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        /// xmlns:syncfusion="http://schemas.syncfusion.com/wpf"
        /// Title="Rulers And Units Demo"  WindowState="Maximized" Name="mainwindow" 
        ///  xmlns:local="clr-namespace:Sample" FontWeight="Bold"
        ///  Icon="Images/App.ico" &gt;
        ///   &lt;syncfusion:DiagramControl Grid.Column="1" Name="diagramControl" 
        ///                                IsSymbolPaletteEnabled="True" 
        ///                                Background="WhiteSmoke"&gt;
        ///           &lt;syncfusion:DiagramControl.View&gt;
        ///              &lt;syncfusion:DiagramView  ShowHorizontalGridLine="False" 
        ///                                        Bounds="0,0,12,12"  
        ///                                        Name="diagramView"  &gt;
        ///             &lt;syncfusion:HorizontalRuler Name="horizontalRuler" /&gt;
        ///          &lt;/syncfusion:DiagramView.HorizontalRuler&gt;
        ///           &lt;syncfusion:DiagramView.VerticalRuler&gt;
        ///               &lt;syncfusion:VerticalRuler    Name="verticalRuler" /&gt;
        ///           &lt;/syncfusion:DiagramView.VerticalRuler &gt;
        ///       &lt;/syncfusion:DiagramView&gt;
        ///    &lt;/syncfusion:DiagramControl.View&gt;
        /// &lt;/syncfusion:DiagramControl&gt;
        /// &lt;/Window&gt;
        /// </code>
        /// <code language="C#">
        /// using System;
        /// using System.Collections.Generic;
        /// using System.Linq;
        /// using System.Text;
        /// using System.Windows;
        /// using System.Windows.Controls;
        /// using System.Windows.Data;
        /// using System.Windows.Documents;
        /// using System.Windows.Input;
        /// using System.Windows.Media;
        /// using System.Windows.Media.Imaging;
        /// using System.Windows.Navigation;
        /// using System.Windows.Shapes;
        /// using System.ComponentModel;
        /// using Syncfusion.Core;
        /// using Syncfusion.Windows.Diagram;
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        ///    public DiagramControl Control;
        ///    public DiagramModel Model;
        ///    public DiagramView View;
        ///    public Window1 ()
        ///    {
        ///       InitializeComponent ();
        ///       Control = new DiagramControl ();
        ///       View = new DiagramView ();
        ///       Control.View = View;
        ///       HorizontalRuler hruler = new HorizontalRuler();
        ///       View.HorizontalRuler = hruler;
        ///       View.ShowHorizontalGridLine = false;
        ///       View.ShowVerticalGridLine = false;
        ///       VerticalRuler vruler = new VerticalRuler();
        ///       View.VerticalRuler = vruler;
        ///       View.Bounds = new Thickness (0, 0, 1000, 1000);
        ///       View.IsPageEditable = true;
        ///       View.ShowHorizontalGridLine=false;
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        public bool ShowHorizontalGridLine
        {
            get
            {
                return (bool)GetValue(ShowHorizontalGridLineProperty);
            }

            set
            {
                SetValue(ShowHorizontalGridLineProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show vertical grid line].
        /// </summary>
        /// <value>
        /// Type: <see cref="Boolean"/>
        /// True, if it is to be displayed, false otherwise.
        /// </value>
        /// <example>
        /// <code language="XAML">
        /// &lt;Window x:Class="RulersAndUnits.Window1"
        /// xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        /// xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        /// xmlns:syncfusion="http://schemas.syncfusion.com/wpf"
        /// Title="Rulers And Units Demo"  WindowState="Maximized" Name="mainwindow" 
        ///  xmlns:local="clr-namespace:Sample" FontWeight="Bold"
        ///  Icon="Images/App.ico" &gt;
        ///   &lt;syncfusion:DiagramControl Grid.Column="1" Name="diagramControl" 
        ///                                IsSymbolPaletteEnabled="True" 
        ///                                Background="WhiteSmoke"&gt;
        ///           &lt;syncfusion:DiagramControl.View&gt;
        ///              &lt;syncfusion:DiagramView  ShowVerticalGridLine="False" 
        ///                                        Bounds="0,0,12,12"  
        ///                                        Name="diagramView"  &gt;
        ///             &lt;syncfusion:HorizontalRuler Name="horizontalRuler" /&gt;
        ///          &lt;/syncfusion:DiagramView.HorizontalRuler&gt;
        ///           &lt;syncfusion:DiagramView.VerticalRuler&gt;
        ///               &lt;syncfusion:VerticalRuler    Name="verticalRuler" /&gt;
        ///           &lt;/syncfusion:DiagramView.VerticalRuler &gt;
        ///       &lt;/syncfusion:DiagramView&gt;
        ///    &lt;/syncfusion:DiagramControl.View&gt;
        /// &lt;/syncfusion:DiagramControl&gt;
        /// &lt;/Window&gt;
        /// </code>
        /// <code language="C#">
        /// using System;
        /// using System.Collections.Generic;
        /// using System.Linq;
        /// using System.Text;
        /// using System.Windows;
        /// using System.Windows.Controls;
        /// using System.Windows.Data;
        /// using System.Windows.Documents;
        /// using System.Windows.Input;
        /// using System.Windows.Media;
        /// using System.Windows.Media.Imaging;
        /// using System.Windows.Navigation;
        /// using System.Windows.Shapes;
        /// using System.ComponentModel;
        /// using Syncfusion.Core;
        /// using Syncfusion.Windows.Diagram;
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        ///    public DiagramControl Control;
        ///    public DiagramModel Model;
        ///    public DiagramView View;
        ///    public Window1 ()
        ///    {
        ///       InitializeComponent ();
        ///       Control = new DiagramControl ();
        ///       View = new DiagramView ();
        ///       Control.View = View;
        ///       HorizontalRuler hruler = new HorizontalRuler();
        ///       View.HorizontalRuler = hruler;
        ///       View.ShowHorizontalGridLine = false;
        ///       View.ShowVerticalGridLine = false;
        ///       VerticalRuler vruler = new VerticalRuler();
        ///       View.VerticalRuler = vruler;
        ///       View.Bounds = new Thickness (0, 0, 1000, 1000);
        ///       View.IsPageEditable = true;
        ///       View.ShowVerticalGridLine=false;
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        public bool ShowVerticalGridLine
        {
            get
            {
                return (bool)GetValue(ShowVerticalGridLineProperty);
            }

            set
            {
                SetValue(ShowVerticalGridLineProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show horizontal rulers].
        /// </summary>
        /// <value>
        /// Type: <see cref="Boolean"/>
        /// True, if it is to be displayed, false otherwise.
        /// </value>
        /// <example>
        /// <code language="XAML">
        /// &lt;Window x:Class="RulersAndUnits.Window1"
        /// xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        /// xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        /// xmlns:syncfusion="http://schemas.syncfusion.com/wpf"
        /// Title="Rulers And Units Demo"  WindowState="Maximized" Name="mainwindow" 
        ///  xmlns:local="clr-namespace:Sample" FontWeight="Bold"
        ///  Icon="Images/App.ico" &gt;
        ///   &lt;syncfusion:DiagramControl Grid.Column="1" Name="diagramControl" 
        ///                                IsSymbolPaletteEnabled="True" 
        ///                                Background="WhiteSmoke"&gt;
        ///           &lt;syncfusion:DiagramControl.View&gt;
        ///              &lt;syncfusion:DiagramView  ShowHorizontalRulers="False" 
        ///                                        Bounds="0,0,12,12"  
        ///                                        Name="diagramView"  &gt;
        ///             &lt;syncfusion:HorizontalRuler Name="horizontalRuler" /&gt;
        ///          &lt;/syncfusion:DiagramView.HorizontalRuler&gt;
        ///           &lt;syncfusion:DiagramView.VerticalRuler&gt;
        ///               &lt;syncfusion:VerticalRuler    Name="verticalRuler" /&gt;
        ///           &lt;/syncfusion:DiagramView.VerticalRuler &gt;
        ///       &lt;/syncfusion:DiagramView&gt;
        ///    &lt;/syncfusion:DiagramControl.View&gt;
        /// &lt;/syncfusion:DiagramControl&gt;
        /// &lt;/Window&gt;
        /// </code>
        /// <code language="C#">
        /// using System;
        /// using System.Collections.Generic;
        /// using System.Linq;
        /// using System.Text;
        /// using System.Windows;
        /// using System.Windows.Controls;
        /// using System.Windows.Data;
        /// using System.Windows.Documents;
        /// using System.Windows.Input;
        /// using System.Windows.Media;
        /// using System.Windows.Media.Imaging;
        /// using System.Windows.Navigation;
        /// using System.Windows.Shapes;
        /// using System.ComponentModel;
        /// using Syncfusion.Core;
        /// using Syncfusion.Windows.Diagram;
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        ///    public DiagramControl Control;
        ///    public DiagramModel Model;
        ///    public DiagramView View;
        ///    public Window1 ()
        ///    {
        ///       InitializeComponent ();
        ///       Control = new DiagramControl ();
        ///       View = new DiagramView ();
        ///       Control.View = View;
        ///       HorizontalRuler hruler = new HorizontalRuler();
        ///       View.HorizontalRuler = hruler;
        ///       View.ShowHorizontalGridLine = false;
        ///       View.ShowVerticalGridLine = false;
        ///       VerticalRuler vruler = new VerticalRuler();
        ///       View.VerticalRuler = vruler;
        ///       View.Bounds = new Thickness (0, 0, 1000, 1000);
        ///       View.IsPageEditable = true;
        ///       View.ShowHorizontalRulers=false;
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        /// <seealso cref="HorizontalRuler"/>
        public bool ShowHorizontalRulers
        {
            get
            {
                return (bool)GetValue(ShowHorizontalRulerProperty);
            }

            set
            {
                SetValue(ShowHorizontalRulerProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show vertical rulers].
        /// </summary>
        /// <value>
        /// Type: <see cref="Boolean"/>
        /// True, if it is to be displayed, false otherwise.
        /// </value>
        /// <example>
        /// <code language="XAML">
        /// &lt;Window x:Class="RulersAndUnits.Window1"
        /// xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        /// xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        /// xmlns:syncfusion="http://schemas.syncfusion.com/wpf"
        /// Title="Rulers And Units Demo"  WindowState="Maximized" Name="mainwindow" 
        ///  xmlns:local="clr-namespace:Sample" FontWeight="Bold"
        ///  Icon="Images/App.ico" &gt;
        ///   &lt;syncfusion:DiagramControl Grid.Column="1" Name="diagramControl" 
        ///                                IsSymbolPaletteEnabled="True" 
        ///                                Background="WhiteSmoke"&gt;
        ///           &lt;syncfusion:DiagramControl.View&gt;
        ///              &lt;syncfusion:DiagramView  ShowVerticalRulers="False" 
        ///                                        Bounds="0,0,12,12"  
        ///                                        Name="diagramView"  &gt;
        ///             &lt;syncfusion:HorizontalRuler Name="horizontalRuler" /&gt;
        ///          &lt;/syncfusion:DiagramView.HorizontalRuler&gt;
        ///           &lt;syncfusion:DiagramView.VerticalRuler&gt;
        ///               &lt;syncfusion:VerticalRuler    Name="verticalRuler" /&gt;
        ///           &lt;/syncfusion:DiagramView.VerticalRuler &gt;
        ///       &lt;/syncfusion:DiagramView&gt;
        ///    &lt;/syncfusion:DiagramControl.View&gt;
        /// &lt;/syncfusion:DiagramControl&gt;
        /// &lt;/Window&gt;
        /// </code>
        /// <code language="C#">
        /// using System;
        /// using System.Collections.Generic;
        /// using System.Linq;
        /// using System.Text;
        /// using System.Windows;
        /// using System.Windows.Controls;
        /// using System.Windows.Data;
        /// using System.Windows.Documents;
        /// using System.Windows.Input;
        /// using System.Windows.Media;
        /// using System.Windows.Media.Imaging;
        /// using System.Windows.Navigation;
        /// using System.Windows.Shapes;
        /// using System.ComponentModel;
        /// using Syncfusion.Core;
        /// using Syncfusion.Windows.Diagram;
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        ///    public DiagramControl Control;
        ///    public DiagramModel Model;
        ///    public DiagramView View;
        ///    public Window1 ()
        ///    {
        ///       InitializeComponent ();
        ///       Control = new DiagramControl ();
        ///       View = new DiagramView ();
        ///       Control.View = View;
        ///       HorizontalRuler hruler = new HorizontalRuler();
        ///       View.HorizontalRuler = hruler;
        ///       View.ShowHorizontalGridLine = false;
        ///       View.ShowVerticalGridLine = false;
        ///       VerticalRuler vruler = new VerticalRuler();
        ///       View.VerticalRuler = vruler;
        ///       View.Bounds = new Thickness (0, 0, 1000, 1000);
        ///       View.IsPageEditable = true;
        ///       View.ShowVerticalRulers=false;
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        /// <seealso cref="VerticalRuler"/>
        public bool ShowVerticalRulers
        {
            get
            {
                return (bool)GetValue(ShowVerticalRulerProperty);
            }

            set
            {
                SetValue(ShowVerticalRulerProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the horizontal ruler.
        /// </summary>
        /// <value>
        /// Type: <see cref="HorizontalRuler"/>
        /// HorizontalRuler instance.
        /// </value>
        /// <example>
        /// <code language="XAML">
        /// &lt;Window x:Class="RulersAndUnits.Window1"
        /// xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        /// xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        /// xmlns:syncfusion="http://schemas.syncfusion.com/wpf"
        /// Title="Rulers And Units Demo"  WindowState="Maximized" Name="mainwindow" 
        ///  xmlns:local="clr-namespace:Sample" FontWeight="Bold"
        ///  Icon="Images/App.ico" &gt;
        ///   &lt;syncfusion:DiagramControl Grid.Column="1" Name="diagramControl" 
        ///                                IsSymbolPaletteEnabled="True" 
        ///                                Background="WhiteSmoke"&gt;
        ///           &lt;syncfusion:DiagramControl.View&gt;
        ///              &lt;syncfusion:DiagramView  ShowHorizontalRulers="False" 
        ///                                        Bounds="0,0,12,12"  
        ///                                        Name="diagramView"  &gt;
        ///             &lt;syncfusion:HorizontalRuler Name="horizontalRuler" /&gt;
        ///          &lt;/syncfusion:DiagramView.HorizontalRuler&gt;
        ///       &lt;/syncfusion:DiagramView&gt;
        ///    &lt;/syncfusion:DiagramControl.View&gt;
        /// &lt;/syncfusion:DiagramControl&gt;
        /// &lt;/Window&gt;
        /// </code>
        /// <code language="C#">
        /// using Syncfusion.Core;
        /// using Syncfusion.Windows.Diagram;
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        ///    public DiagramControl Control;
        ///    public DiagramModel Model;
        ///    public DiagramView View;
        ///    public Window1 ()
        ///    {
        ///       InitializeComponent ();
        ///       Control = new DiagramControl ();
        ///       View = new DiagramView ();
        ///       Control.View = View;
        ///       HorizontalRuler hruler = new HorizontalRuler();
        ///       View.HorizontalRuler = hruler;
        ///       View.Bounds = new Thickness (0, 0, 1000, 1000);
        ///       View.IsPageEditable = true;
        ///       View.ShowHorizontalRulers=false;
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        /// <seealso cref="HorizontalRuler"/>
        public HorizontalRuler HorizontalRuler
        {
            get
            {
                return (HorizontalRuler)GetValue(HorizontalRulerProperty);
            }

            set
            {
                SetValue(HorizontalRulerProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the vertical ruler.
        /// </summary>
        /// <value>
        /// Type: <see cref="VerticalRuler"/>
        /// VerticalRuler instance.
        /// </value>
        /// <example>
        /// <code language="XAML">
        /// &lt;Window x:Class="RulersAndUnits.Window1"
        /// xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        /// xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        /// xmlns:syncfusion="http://schemas.syncfusion.com/wpf"
        /// Title="Rulers And Units Demo"  WindowState="Maximized" Name="mainwindow" 
        ///  xmlns:local="clr-namespace:Sample" FontWeight="Bold"
        ///  Icon="Images/App.ico" &gt;
        ///   &lt;syncfusion:DiagramControl Grid.Column="1" Name="diagramControl" 
        ///                                IsSymbolPaletteEnabled="True" 
        ///                                Background="WhiteSmoke"&gt;
        ///           &lt;syncfusion:DiagramControl.View&gt;
        ///              &lt;syncfusion:DiagramView  ShowHorizontalRulers="False" 
        ///                                        Bounds="0,0,12,12"  
        ///                                        Name="diagramView"  &gt;
        ///             &lt;syncfusion:HorizontalRuler Name="horizontalRuler" /&gt;
        ///          &lt;/syncfusion:DiagramView.HorizontalRuler&gt;
        ///       &lt;/syncfusion:DiagramView&gt;
        ///    &lt;/syncfusion:DiagramControl.View&gt;
        /// &lt;/syncfusion:DiagramControl&gt;
        /// &lt;/Window&gt;
        /// </code>
        /// <code language="C#">
        /// using Syncfusion.Core;
        /// using Syncfusion.Windows.Diagram;
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        ///    public DiagramControl Control;
        ///    public DiagramModel Model;
        ///    public DiagramView View;
        ///    public Window1 ()
        ///    {
        ///       InitializeComponent ();
        ///       Control = new DiagramControl ();
        ///       View = new DiagramView ();
        ///       Control.View = View;
        ///       VerticalRuler vruler = new VerticalRuler();
        ///       View.VerticalRuler = vruler;
        ///       View.Bounds = new Thickness (0, 0, 1000, 1000);
        ///       View.IsPageEditable = true;
        ///       View.ShowVerticalRulers=false;
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        /// <seealso cref="HorizontalRuler"/>
        public VerticalRuler VerticalRuler
        {
            get
            {
                return (VerticalRuler)GetValue(VerticalRulerProperty);
            }

            set
            {
                SetValue(VerticalRulerProperty, value);
            }
        }

        /// <summary>
        /// Gets the selection list.
        /// </summary>
        /// <value>
        /// Type: <see cref="NodeCollection"/>
        /// NodeCollection items.
        /// </value>
        /// <example>
        /// <code language="C#">
        /// using Syncfusion.Core;
        /// using Syncfusion.Windows.Diagram;
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        ///    public DiagramControl Control;
        ///    public DiagramModel Model;
        ///    public DiagramView View;
        ///    public Window1 ()
        ///    {
        ///       InitializeComponent ();
        ///       Control = new DiagramControl ();
        ///       View = new DiagramView ();
        ///       Control.View = View;
        ///       HorizontalRuler hruler = new HorizontalRuler();
        ///       View.HorizontalRuler = hruler;
        ///       View.ShowHorizontalGridLine = false;
        ///       View.ShowVerticalGridLine = false;
        ///       VerticalRuler vruler = new VerticalRuler();
        ///       View.VerticalRuler = vruler;
        ///       View.Bounds = new Thickness (0, 0, 1000, 1000);
        ///       View.IsPageEditable = true;
        ///       View.ShowVerticalRulers=false;
        ///       Node n = new Node(Guid.NewGuid(), "Start");
        ///        n.Shape = Shapes.FlowChart_Start;
        ///        n.IsLabelEditable = true;
        ///        n.Label = "Start";
        ///        n.Level = 1;
        ///        n.OffsetX = 150;
        ///        n.OffsetY = 25;
        ///        n.Width = 150;
        ///        n.Height = 75;
        ///        n.ToolTip="Start Node";
        ///        Model.Nodes.Add(n);
        ///        View.SelectionList.Add(n);
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        /// <seealso cref="NodeCollection"/>
        public NodeCollection SelectionList
        {
            get
            {
                if (Page == null)
                {
                    return null;
                }
                else
                {
                    return (Page as DiagramPage).SelectionList;
                }
            }
        }

        /// <summary>
        /// Gets or sets the node context menu.
        /// </summary>
        /// <value>The node context menu.
        /// Type: <see cref="ContextMenu"/>
        /// </value>
        /// <example>
        /// <code language="C#">
        /// using Syncfusion.Core;
        /// using Syncfusion.Windows.Diagram;
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        ///    public DiagramControl Control;
        ///    public DiagramModel Model;
        ///    public DiagramView View;
        ///    public Window1 ()
        ///    {
        ///       InitializeComponent ();
        ///       Control = new DiagramControl ();
        ///       View = new DiagramView ();
        ///       Control.View = View;
        ///       HorizontalRuler hruler = new HorizontalRuler();
        ///       View.HorizontalRuler = hruler;
        ///       View.ShowHorizontalGridLine = false;
        ///       View.ShowVerticalGridLine = false;
        ///       VerticalRuler vruler = new VerticalRuler();
        ///       View.VerticalRuler = vruler;
        ///       View.Bounds = new Thickness (0, 0, 1000, 1000);
        ///       View.IsPageEditable = true;
        ///       View.ShowVerticalRulers=false;
        ///       Node n = new Node(Guid.NewGuid(), "Start");
        ///        n.Shape = Shapes.FlowChart_Start;
        ///        n.IsLabelEditable = true;
        ///        n.Label = "Start";
        ///        n.Level = 1;
        ///        n.OffsetX = 150;
        ///        n.OffsetY = 25;
        ///        n.Width = 150;
        ///        n.Height = 75;
        ///        n.ToolTip="Start Node";
        ///        Model.Nodes.Add(n);
        ///        ContextMenu menu = new ContextMenu();
        ///        MenuItem m1 = new MenuItem();
        ///        m1.Header = "item1";
        ///        MenuItem m2 = new MenuItem();
        ///        m2.Header = "item2";
        ///        menu.Items.Add(m1);
        ///        menu.Items.Add(m2);
        ///        View.NodeContextMenu=menu;
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        /// <seealso cref="ContextMenu"/>
        /// <seealso cref="Node"/>
        public ContextMenu NodeContextMenu
        {
            get
            {
                return (ContextMenu)GetValue(NodeContextMenuProperty);
            }

            set
            {
                SetValue(NodeContextMenuProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the LineConnector context menu.
        /// </summary>
        /// <value>The lineConnector context menu.
        /// Type: <see cref="ContextMenu"/>
        /// </value>
        /// <example>
        /// <code language="C#">
        /// using Syncfusion.Core;
        /// using Syncfusion.Windows.Diagram;
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        ///    public DiagramControl Control;
        ///    public DiagramModel Model;
        ///    public DiagramView View;
        ///    public Window1 ()
        ///    {
        ///       InitializeComponent ();
        ///       Control = new DiagramControl ();
        ///       View = new DiagramView ();
        ///       Control.View = View;
        ///       HorizontalRuler hruler = new HorizontalRuler();
        ///       View.HorizontalRuler = hruler;
        ///       View.ShowHorizontalGridLine = false;
        ///       View.ShowVerticalGridLine = false;
        ///       VerticalRuler vruler = new VerticalRuler();
        ///       View.VerticalRuler = vruler;
        ///       View.Bounds = new Thickness (0, 0, 1000, 1000);
        ///       View.IsPageEditable = true;
        ///       View.ShowVerticalRulers=false;
        ///        ContextMenu menu = new ContextMenu();
        ///        MenuItem m1 = new MenuItem();
        ///        m1.Header = "item1";
        ///        MenuItem m2 = new MenuItem();
        ///        m2.Header = "item2";
        ///        menu.Items.Add(m1);
        ///        menu.Items.Add(m2);
        ///        View.LineConnectorContextMenu=menu;
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        /// <seealso cref="ContextMenu"/>
        /// <seealso cref="LineConnector"/>
        public ContextMenu LineConnectorContextMenu
        {
            get
            {
                return (ContextMenu)GetValue(LineConnectorContextMenuProperty);
            }

            set
            {
                SetValue(LineConnectorContextMenuProperty, value);
            }
        }

        /// <summary>
        /// Gets the vertical scroll bar visibility.
        /// </summary>
        /// <value>
        /// The  vertical scroll bar visibility. Default value is Auto.
        /// Type: <see cref="ScrollBarVisibility"/>
        /// </value>
        /// <example>
        /// <code language="C#">
        /// using Syncfusion.Core;
        /// using Syncfusion.Windows.Diagram;
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public Window1 ()
        /// {
        /// InitializeComponent ();
        /// Control = new DiagramControl ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// HorizontalRuler hruler = new HorizontalRuler();
        /// View.HorizontalRuler = hruler;
        /// View.ShowHorizontalGridLine = false;
        /// View.ShowVerticalGridLine = false;
        /// VerticalRuler vruler = new VerticalRuler();
        /// View.VerticalRuler = vruler;
        /// View.Bounds = new Thickness (0, 0, 1000, 1000);
        /// View.IsPageEditable = true;
        /// View.ShowVerticalRulers=false;
        /// ScrollBarVisibility Vvisible=View.GetVerticalScrollBarVisibility;
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="ScrollBarVisibility"/>
        public ScrollBarVisibility GetVerticalScrollBarVisibility
        {
            get
            {
                return (ScrollBarVisibility)GetValue(GetVerticalScrollBarVisibilityProperty);
            }

            internal set
            {
                SetValue(GetVerticalScrollBarVisibilityProperty, value);
            }
        }

        /// <summary>
        /// Gets the horizontal scroll bar visibility.
        /// </summary>
        /// <value>
        /// The  horizontal scroll bar visibility. Default value is Auto.
        /// Type: <see cref="ScrollBarVisibility"/>
        /// </value>
        /// <example>
        /// <code language="C#">
        /// using Syncfusion.Core;
        /// using Syncfusion.Windows.Diagram;
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public Window1 ()
        /// {
        /// InitializeComponent ();
        /// Control = new DiagramControl ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// HorizontalRuler hruler = new HorizontalRuler();
        /// View.HorizontalRuler = hruler;
        /// View.ShowHorizontalGridLine = false;
        /// View.ShowVerticalGridLine = false;
        /// VerticalRuler vruler = new VerticalRuler();
        /// View.VerticalRuler = vruler;
        /// View.Bounds = new Thickness (0, 0, 1000, 1000);
        /// View.IsPageEditable = true;
        /// View.ShowVerticalRulers=false;
        /// ScrollBarVisibility hvisible=View.GetHorizontalScrollBarVisibility;
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="ScrollBarVisibility"/>
        public ScrollBarVisibility GetHorizontalScrollBarVisibility
        {
            get
            {
                return (ScrollBarVisibility)GetValue(GetHorizontalScrollBarVisibilityProperty);
            }

            internal set
            {
                SetValue(GetHorizontalScrollBarVisibilityProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the horizontal scroll bar visibility.
        /// </summary>
        /// <value>
        /// The  horizontal scroll bar visibility. Default value is Auto.
        /// Type: <see cref="ScrollBarVisibility"/>
        /// </value>
        /// <example>
        /// <code language="C#">
        /// using Syncfusion.Core;
        /// using Syncfusion.Windows.Diagram;
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public Window1 ()
        /// {
        /// InitializeComponent ();
        /// Control = new DiagramControl ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// HorizontalRuler hruler = new HorizontalRuler();
        /// View.HorizontalRuler = hruler;
        /// View.ShowHorizontalGridLine = false;
        /// View.ShowVerticalGridLine = false;
        /// VerticalRuler vruler = new VerticalRuler();
        /// View.VerticalRuler = vruler;
        /// View.Bounds = new Thickness (0, 0, 1000, 1000);
        /// View.IsPageEditable = true;
        /// View.ShowVerticalRulers=false;
        /// ScrollBarVisibility hvisible=View.HorizontalScrollBarVisibility;
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="ScrollBarVisibility"/>
        public ScrollBarVisibility HorizontalScrollBarVisibility
        {
            get
            {
                return (ScrollBarVisibility)GetValue(HorizontalScrollBarVisibilityProperty);
            }

            set
            {
                SetValue(HorizontalScrollBarVisibilityProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the vertical scroll bar visibility.
        /// </summary>
        /// <value>
        /// The  vertical scroll bar visibility. Default value is Auto.
        /// Type: <see cref="ScrollBarVisibility"/>
        /// </value>
        /// <example>
        /// <code language="C#">
        /// using Syncfusion.Core;
        /// using Syncfusion.Windows.Diagram;
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public Window1 ()
        /// {
        /// InitializeComponent ();
        /// Control = new DiagramControl ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// HorizontalRuler hruler = new HorizontalRuler();
        /// View.HorizontalRuler = hruler;
        /// View.ShowHorizontalGridLine = false;
        /// View.ShowVerticalGridLine = false;
        /// VerticalRuler vruler = new VerticalRuler();
        /// View.VerticalRuler = vruler;
        /// View.Bounds = new Thickness (0, 0, 1000, 1000);
        /// View.IsPageEditable = true;
        /// View.ShowVerticalRulers=false;
        /// ScrollBarVisibility Vvisible=View.VerticalScrollBarVisibility;
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="ScrollBarVisibility"/>
        public ScrollBarVisibility VerticalScrollBarVisibility
        {
            get
            {
                return (ScrollBarVisibility)GetValue(VerticalScrollBarVisibilityProperty);
            }

            set
            {
                SetValue(VerticalScrollBarVisibilityProperty, value);
            }
        }
        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the <see cref="Node"/> resized count.
        /// </summary>
        /// <value>The node resized count.</value>
        internal int NodeResizedCount
        {
            get { return resizecount; }
            set { resizecount = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Node"/> rotate count.
        /// </summary>
        /// <value>The node rotate count.</value>
        internal int NodeRotateCount
        {
            get { return rotatecount; }
            set { rotatecount = value; }
        }

        /// <summary>
        /// Gets or sets the undo stack.
        /// </summary>
        /// <value>The undo stack.</value>
        internal Stack<object> UndoStack
        {
            get { return undocommandstack; }
            set { undocommandstack = value; }
        }

        /// <summary>
        /// Gets or sets the redo stack.
        /// </summary>
        /// <value>The redo stack.</value>
        internal Stack<object> RedoStack
        {
            get { return redocommandstack; }
            set { redocommandstack = value; }
        }

        ///// <summary>
        ///// Gets or sets a value indicating whether the scrollbar were moved.
        ///// </summary>
        ///// <value>
        ///// <c>true</c> if mouse scrolled; otherwise, <c>false</c>.
        ///// </value>
        //internal bool IsMouseScrolled
        //{
        //    get { return mscrolled; }
        //    set { mscrolled = value; }
        //}

        /// <summary>
        /// Gets or sets the view grid which contains the page.
        /// </summary>
        /// <value>The view grid.</value>
        internal Grid ViewGrid
        {
            get { return viewgrid; }
            set { viewgrid = value; }
        }

        /// <summary>
        /// Gets the internal groups collection.
        /// </summary>
        /// <value>The internal groups collection.</value>
        internal CollectionExt InternalGroups
        {
            get { return m_groups; }
        }

        ///// <summary>
        ///// Gets or sets the old horizontal offset.
        ///// </summary>
        ///// <value>The old old horizontal offset.</value>
        //internal double OldHoroffset
        //{
        //    get { return oldhoffset; }
        //    set { oldhoffset = value; }
        //}

        /// <summary>
        /// Gets or sets a value indicating whether a <see cref="Node"/> is deleted.
        /// </summary>
        /// <value><c>true</c> if node is deleted; otherwise, <c>false</c>.</value>
        internal bool Isnodedeleted
        {
            get { return nodedel; }
            set { nodedel = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="LineConnector"/> is deleted.
        /// </summary>
        /// <value><c>true</c> if is line deleted; otherwise, <c>false</c>.</value>
        internal bool Islinedeleted
        {
            get { return linedel; }
            set { linedel = value; }
        }

        /// <summary>
        /// Gets the internal edges.
        /// </summary>
        /// <value>The internal edges.</value>
        internal CollectionExt InternalEdges
        {
            get { return intedges; }
        }

        ///// <summary>
        ///// Gets or sets the horizontal thumb drag offset.
        ///// </summary>
        ///// <value>The horizontal thumb drag offset.</value>
        //internal double HorThumbDragOffset
        //{
        //    get { return scrollhorthumb; }
        //    set { scrollhorthumb = value; }
        //}

        ///// <summary>
        ///// Gets or sets the old vertical offset.
        ///// </summary>
        ///// <value>The old vertical offset.</value>
        //internal double OldVeroffset
        //{
        //    get { return oldvoffset; }
        //    set { oldvoffset = value; }
        //}

        ///// <summary>
        ///// Gets or sets the vertical thumb drag offset.
        ///// </summary>
        ///// <value>The vertical thumb drag offset.</value>
        //internal double VerThumbDragOffset
        //{
        //    get { return scrollverthumb; }
        //    set { scrollverthumb = value; }
        //}

        ///// <summary>
        ///// Gets or sets a value indicating whether this instance is scroll thumb.
        ///// </summary>
        ///// <value>
        ///// <c>true</c> if this instance is scroll thumb; otherwise, <c>false</c>.
        ///// </value>
        //internal bool IsScrollThumb
        //{
        //    get { return scrollthumb; }
        //    set { scrollthumb = value; }
        //}

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="Node"/> position is changed.
        /// </summary>
        /// <value><c>true</c> if position is changed; otherwise, <c>false</c>.</value>
        internal bool Ispositionchanged
        {
            get { return isdrag; }
            set { isdrag = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is zoom changed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is zoom changed; otherwise, <c>false</c>.
        /// </value>
        internal static bool ViewGridOriginChanged
        {
            get { return isvieworiginchanged; }
            set { isvieworiginchanged = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is other event.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is other event; otherwise, <c>false</c>.
        /// </value>
        internal static bool IsOtherEvent
        {
            get { return otherevents; }
            set { otherevents = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [page edit].
        /// </summary>
        /// <value><c>true</c> if [page edit]; otherwise, <c>false</c>.</value>
        internal static bool PageEdit
        {
            get { return ispagedit; }
            set { ispagedit = value; }
        }

        /// <summary>
        /// Gets or sets the scrollviewer.
        /// </summary>
        /// <value>The scrollviewer.</value>
        internal ScrollViewer Scrollviewer
        {
            get { return scrollview; }
            set { scrollview = value; }
        }

        ///// <summary>
        ///// Gets or sets a value indicating whether this instance is exe once.
        ///// </summary>
        ///// <value>
        ///// <c>true</c> if this instance is exe once; otherwise, <c>false</c>.
        ///// </value>
        //internal bool IsExeOnce
        //{
        //    get { return exeonce; }
        //    set { exeonce = value; }
        //}

        ///// <summary>
        ///// Gets or sets a value indicating whether this instance is exe once Y.
        ///// </summary>
        ///// <value>
        ///// <c>true</c> if this instance is exe once Y; otherwise, <c>false</c>.
        ///// </value>
        //internal bool IsExeOnceY
        //{
        //    get { return exeoncey; }
        //    set { exeoncey = value; }
        //}

        ///// <summary>
        ///// Gets or sets a value indicating whether mouse up is fired.
        ///// </summary>
        ///// <value>
        ///// <c>true</c> if mouse up, otherwise, <c>false</c>.
        ///// </value>
        //internal bool IsMouseUponly
        //{
        //    get { return muponly; }
        //    set { muponly = value; }
        //}

        /// <summary>
        /// Gets or sets a value indicating whether automatic layout is used.
        /// </summary>
        /// <value><c>true</c> if automatic layout is used; otherwise, <c>false</c>.</value>
        internal bool IsLayout
        {
            get { return layout; }
            set { layout = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether node is dragged.
        /// </summary>
        /// <value><c>true</c> if is dragged; otherwise, <c>false</c>.</value>
        internal bool Isdragdelta
        {
            get { return isdragged; }
            set { isdragged = value; }
        }

        ///// <summary>
        ///// Gets or sets a value indicating whether connector is dragged.
        ///// </summary>
        ///// <value><c>true</c> if line is dragged; otherwise, <c>false</c>.</value>
        //internal bool IsLinedragdelta
        //{
        //    get { return islinedragged; }
        //    set { islinedragged = value; }
        //}

        ///// <summary>
        ///// Gets or sets a value indicating whether connector is dragged.
        ///// </summary>
        ///// <value><c>true</c> if line is dragged; otherwise, <c>false</c>.</value>
        //internal bool IsLinedragdeltay
        //{
        //    get { return islinedraggedy; }
        //    set { islinedraggedy = value; }
        //}

        ///// <summary>
        ///// Gets or sets the line horizontal drag offset.
        ///// </summary>
        ///// <value>The line horizontal offset.</value>
        //internal double Linehoffset
        //{
        //    get { return linedragx; }
        //    set { linedragx = value; }
        //}

        ///// <summary>
        ///// Gets or sets the line vertical drag offset.
        ///// </summary>
        ///// <value>The line vertical offset.</value>
        //internal double Linevoffset
        //{
        //    get { return linedragy; }
        //    set { linedragy = value; }
        //}

        ///// <summary>
        ///// Gets or sets a value indicating whether this <see cref="Node"/> or <see cref="LineConnector"/> is deleted.
        ///// </summary>
        ///// <value><c>true</c> if deleted; otherwise, <c>false</c>.</value>
        //internal bool Deleted
        //{
        //    get { return isdel; }
        //    set { isdel = value; }
        //}

        /// <summary>
        /// Gets or sets a value indicating whether delete is done.
        /// </summary>
        /// <value><c>true</c> if [deleted]; otherwise, <c>false</c>.</value>
        internal bool DupDeleted
        {
            get { return isdupdel; }
            set { isdupdel = value; }
        }

        ///// <summary>
        ///// Gets or sets the X coordinate.
        ///// </summary>
        ///// <value>The x value</value>
        //internal double X
        //{
        //    get { return x; }
        //    set { x = value; }
        //}

        ///// <summary>
        ///// Gets or sets the view origin Y coordinate.
        ///// </summary>
        ///// <value>The Y value.</value>
        //internal double Y
        //{
        //    get { return y; }
        //    set { y = value; }
        //}

        ///// <summary>
        ///// Gets or sets the panned value.
        ///// </summary>
        ///// <value>The value by which the page is panned.</value>
        //internal double PanConstant
        //{
        //    get { return panconst; }
        //    set { panconst = value; }
        //}

        ///// <summary>
        ///// Gets or sets a value indicating whether this instance is scrolled right.
        ///// </summary>
        ///// <value>
        ///// <c>true</c> if this instance is scrolled right; otherwise, <c>false</c>.
        ///// </value>
        //internal bool IsScrolledRight
        //{
        //    get { return scrolledright; }
        //    set { scrolledright = value; }
        //}

        ///// <summary>
        ///// Gets or sets a value indicating whether this instance is scrolled bottom.
        ///// </summary>
        ///// <value>
        ///// <c>true</c> if this instance is scrolled bottom; otherwise, <c>false</c>.
        ///// </value>
        //internal bool IsScrolledBottom
        //{
        //    get { return scrolledbottom; }
        //    set { scrolledbottom = value; }
        //}

        ///// <summary>
        ///// Gets or sets a value indicating whether this instance is scroll check.
        ///// </summary>
        ///// <value>
        ///// <c>true</c> if this instance is scroll check; otherwise, <c>false</c>.
        ///// </value>
        //internal bool IsScrollCheck
        //{
        //    get { return scrollchk; }
        //    set { scrollchk = value; }
        //}

        ///// <summary>
        ///// Gets or sets a value indicating whether this instance is scrolled.
        ///// </summary>
        ///// <value>
        ///// <c>true</c> if this instance is scrolled; otherwise, <c>false</c>.
        ///// </value>
        //internal bool IsScrolled
        //{
        //    get { return scrolled; }
        //    set { scrolled = value; }
        //}

        ///// <summary>
        ///// Gets or sets the duplicate horizontal thumb drag.
        ///// </summary>
        ///// <value>The duplicate horizontal thumb drag.</value>
        //internal double DupHorthumbdrag
        //{
        //    get { return horthumb; }
        //    set { horthumb = value; }
        //}

        ///// <summary>
        ///// Gets or sets the duplicate vertical thumb drag.
        ///// </summary>
        ///// <value>The duplicate vertical thumb drag..</value>
        //internal double DupVerthumbdrag
        //{
        //    get { return verthumb; }
        //    set { verthumb = value; }
        //}

        ///// <summary>
        ///// Gets or sets a value indicating whether this instance is just scrolled.
        ///// </summary>
        ///// <value>
        ///// <c>true</c> if this instance is just scrolled; otherwise, <c>false</c>.
        ///// </value>
        //internal bool IsJustScrolled
        //{
        //    get { return justscrolled; }
        //    set { justscrolled = value; }
        //}

        ///// <summary>
        ///// Gets or sets a value indicating whether this instance is just wheeled.
        ///// </summary>
        ///// <value>
        ///// <c>true</c> if this instance is just wheeled; otherwise, <c>false</c>.
        ///// </value>
        //internal bool IsJustWheeled
        //{
        //    get { return justwheeled; }
        //    set { justwheeled = value; }
        //}
        internal string DragDelta = "No";

        ///// <summary>
        ///// Gets or sets a value indicating whether this <see cref="DiagramView"/> is isvieworiginchanged2.
        ///// </summary>
        ///// <value><c>true</c> if isvieworiginchanged2; otherwise, <c>false</c>.</value>
        //internal bool Isvieworiginchanged2
        //{
        //    get { return vieworiginchanged2; }
        //    set { vieworiginchanged2 = value; }
        //}

        ///// <summary>
        ///// Gets or sets a value indicating whether this instance is mouse wheeled.
        ///// </summary>
        ///// <value>
        ///// <c>true</c> if this instance is mouse wheeled; otherwise, <c>false</c>.
        ///// </value>
        //internal bool IsMouseWheeled
        //{
        //    get { return ismousewheel; }
        //    set { ismousewheel = value; }
        //}

        ///// <summary>
        ///// Gets or sets a value indicating whether this instance is dup mouse wheeled.
        ///// </summary>
        ///// <value>
        ///// <c>true</c> if this instance is dup mouse wheeled; otherwise, <c>false</c>.
        ///// </value>
        //internal bool IsDupMouseWheeled
        //{
        //    get { return mousew; }
        //    set { mousew = value; }
        //}

        ///// <summary>
        ///// Gets or sets a value indicating whether this instance is dup mouse pressed.
        ///// </summary>
        ///// <value>
        ///// <c>true</c> if this instance is dup mouse pressed; otherwise, <c>false</c>.
        ///// </value>
        //internal bool IsDupMousePressed
        //{
        //    get { return mousep; }
        //    set { mousep = value; }
        //}

        ///// <summary>
        ///// Gets or sets a value indicating whether the offsetx is positive.
        ///// </summary>
        ///// <value>
        ///// <c>true</c> if offsetx is positive; otherwise, <c>false</c>.
        ///// </value>
        //internal bool IsOffsetxpositive
        //{
        //    get { return offgtz; }
        //    set { offgtz = value; }
        //}

        ///// <summary>
        ///// Gets or sets a value indicating whether the offsety is positive.
        ///// </summary>
        ///// <value>
        ///// <c>true</c> if offsety is positive; otherwise, <c>false</c>.
        ///// </value>
        //internal bool IsOffsetypositive
        //{
        //    get { return offygtz; }
        //    set { offygtz = value; }
        //}

        /// <summary>
        /// Gets or sets a value indicating whether [undo redo  is enabled].
        /// </summary>
        /// <value><c>true</c> if [undo redo is enabled]; otherwise, <c>false</c>.</value>
        public bool UndoRedoEnabled
        {
            get
            {
                if (CPManager != null)
                {
                    CPManager.invalidateSelection = true;
                }
                return (bool)GetValue(UndoRedoEnabledProperty);
            }

            set
            {
                SetValue(UndoRedoEnabledProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Undo command is executed.
        /// </summary>
        /// <value><c>true</c> if undone; otherwise, <c>false</c>.</value>
        internal bool Undone
        {
            get { return undo; }
            set { undo = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Redo command is executed.
        /// </summary>
        /// <value><c>true</c> if redone; otherwise, <c>false</c>.</value>
        internal bool Redone
        {
            get { return redo; }
            set { redo = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Node"/> is dragged.
        /// </summary>
        /// <value>
        /// <c>true</c> if <see cref="Node"/>  is dragged; otherwise, <c>false</c>.
        /// </value>
        internal bool IsDragged
        {
            get { return dragged; }
            set { dragged = value; }
        }

        internal bool IsKeyDragged = false;
        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Node"/> is resized.
        /// </summary>
        /// <value>
        /// <c>true</c> if <see cref="Node"/> is resized; otherwise, <c>false</c>.
        /// </value>
        internal bool IsResized
        {
            get { return resized; }
            set { resized = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Node"/> is resized as a result of undo operation.
        /// </summary>
        /// <value>
        /// <c>true</c> if <see cref="Node"/> is resized undone; otherwise, <c>false</c>.
        /// </value>
        internal bool IsResizedUndone
        {
            get { return undoresize; }
            set { undoresize = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Node"/> is resized as a result of redo operation.
        /// </summary>
        /// <value>
        /// <c>true</c> if <see cref="Node"/> is resized redone; otherwise, <c>false</c>.
        /// </value>
        internal bool IsResizedRedone
        {
            get { return redoresize; }
            set { redoresize = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the delete command is executed.
        /// </summary>
        /// <value>
        /// <c>true</c> if delete command is executed; otherwise, <c>false</c>.
        /// </value>
        internal bool IsDeleteCommandExecuted
        {
            get { return deletecommandexe; }
            set { deletecommandexe = value; }
        }

        /// <summary>
        /// Gets or sets the delete count.
        /// </summary>
        /// <value>The delete count.</value>
        internal int DeleteCount
        {
            get { return delcount; }
            set { delcount = value; }
        }

        /// <summary>
        /// Gets or sets the node drag count.
        /// </summary>
        /// <value>The node drag count.</value>
        internal int NodeDragCount
        {
            get { return nodedragcount; }
            set { nodedragcount = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the MeasureOverride of <see cref="DiagramPage"/> is called.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is measure called; otherwise, <c>false</c>.
        /// </value>
        internal bool IsMeasureCalled
        {
            get { return measured; }
            set { measured = value; }
        }

        #endregion

        #region Dependency Property

        public static readonly DependencyProperty EnableCachingProperty = DependencyProperty.Register("VirtualizationRelatedToLine", typeof(bool), typeof(DiagramView), new UIPropertyMetadata(true, new PropertyChangedCallback(OnVirtualizationRelatedToLineChanged)));
        public static readonly DependencyProperty EnableVirtualizationProperty = DependencyProperty.Register("IsVirtualizationEnabled", typeof(bool), typeof(DiagramView), new UIPropertyMetadata(false, new PropertyChangedCallback(OnIsVirtualizationEnabled)));

        public static readonly DependencyProperty SnapToHorizontalGridProperty = DependencyProperty.Register("SnapToHorizontalGrid", typeof(bool), typeof(DiagramView), new PropertyMetadata(false));
        public static readonly DependencyProperty SnapToVerticalGridProperty = DependencyProperty.Register("SnapToVerticalGrid", typeof(bool), typeof(DiagramView), new PropertyMetadata(false));
        public static readonly DependencyProperty SnapOffsetXProperty = DependencyProperty.Register("SnapOffsetX", typeof(double), typeof(DiagramView), new PropertyMetadata(25d));
        public static readonly DependencyProperty SnapOffsetYProperty = DependencyProperty.Register("SnapOffsetY", typeof(double), typeof(DiagramView), new PropertyMetadata(25d));

        public static readonly DependencyProperty IsCutEnabledProperty = DependencyProperty.Register("IsCutEnabledProperty", typeof(bool), typeof(DiagramView), new UIPropertyMetadata(true));
        public static readonly DependencyProperty IsCopyEnabledProperty = DependencyProperty.Register("IsCopyEnabledProperty", typeof(bool), typeof(DiagramView), new UIPropertyMetadata(true));
        public static readonly DependencyProperty IsPasteEnabledProperty = DependencyProperty.Register("IsPasteEnabledProperty", typeof(bool), typeof(DiagramView), new UIPropertyMetadata(true));

        /// <summary>
        /// Identifies the EnableConnection dependency property.
        /// </summary>
        public static readonly DependencyProperty EnableConnectionProperty = DependencyProperty.Register("EnableConnection", typeof(bool), typeof(DiagramView), new UIPropertyMetadata(false,new PropertyChangedCallback(OnEnableConnectionChanged)));

        /// <summary>
        /// Identifies the IsZoomEnabled dependency property.
        /// </summary>
        public static readonly DependencyProperty IsZoomEnabledProperty = DependencyProperty.Register("IsZoomEnabled", typeof(bool), typeof(DiagramView), new UIPropertyMetadata(true));

        /// <summary>
        /// Identifies the IsPanEnabled dependency property.
        /// </summary>
        public static readonly DependencyProperty IsPanEnabledProperty = DependencyProperty.Register("IsPanEnabled", typeof(bool), typeof(DiagramView), new UIPropertyMetadata(false, new PropertyChangedCallback(OnIsPanEnableChanged)));

        /// <summary>
        /// Identifies the PortVisibility dependency property.
        /// </summary>
        public static readonly DependencyProperty PortVisibilityProperty = DependencyProperty.Register("PortVisibility", typeof(Visibility), typeof(DiagramView), new UIPropertyMetadata(Visibility.Collapsed));

        public static readonly DependencyProperty UndoRedoEnabledProperty = DependencyProperty.Register("UndoRedoEnabled", typeof(bool), typeof(DiagramView), new UIPropertyMetadata(true));

        /// <summary>
        /// Identifies the Selected item.This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty IsPageEditableProperty = DependencyProperty.Register("IsPageEditable", typeof(bool), typeof(DiagramView), new PropertyMetadata(true, new PropertyChangedCallback(OnIsPageEditableChanged)));

        /// <summary>
        /// Identifies the Selected item.This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(DiagramView));

        /// <summary>
        /// Identifies the Page property.This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty PageProperty = DependencyProperty.Register("Page", typeof(IDiagramPage), typeof(DiagramView));

        /// <summary>
        /// Identifies the Show Grid property.This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowGridProperty = DependencyProperty.Register("ShowGrid", typeof(bool), typeof(DiagramView), new PropertyMetadata(true, new PropertyChangedCallback(OnShowGridChanged)));

        /// <summary>
        /// Identifies the HorizontalGridLineStyle property.This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty HorizontalGridLineStyleProperty = DependencyProperty.Register("HorizontalGridLineStyle", typeof(Pen), typeof(DiagramView), new PropertyMetadata(new Pen(Brushes.DarkGray, 0.3d), new PropertyChangedCallback(OnHorizontalLineStyleChanged)));

        /// <summary>
        /// Identifies the VerticalGridLineStyle property.This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty VerticalGridLineStyleProperty = DependencyProperty.Register("VerticalGridLineStyle", typeof(Pen), typeof(DiagramView), new PropertyMetadata(new Pen(Brushes.DarkGray, 0.3d), new PropertyChangedCallback(OnVerticalLineStyleChanged)));

        /// <summary>
        /// Identifies the HorizontalRuler property.This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty HorizontalRulerProperty = DependencyProperty.Register("HorizontalRuler", typeof(HorizontalRuler), typeof(DiagramView), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the VerticalRuler property.This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty VerticalRulerProperty = DependencyProperty.Register("VerticalRuler", typeof(VerticalRuler), typeof(DiagramView), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the ShowHorizontalGridLine property.This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowHorizontalGridLineProperty = DependencyProperty.Register("ShowHorizontalGridLine", typeof(bool), typeof(DiagramView), new PropertyMetadata(false, new PropertyChangedCallback(OnShowHLineChanged)));

        /// <summary>
        /// Identifies the ShowVerticalGridLine property.This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowVerticalGridLineProperty = DependencyProperty.Register("ShowVerticalGridLine", typeof(bool), typeof(DiagramView), new PropertyMetadata(false, new PropertyChangedCallback(OnShowVLineChanged)));

        /// <summary>
        /// Identifies the ShowHorizontalRuler property.This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowHorizontalRulerProperty = DependencyProperty.Register("ShowHorizontalRulers", typeof(bool), typeof(DiagramView), new PropertyMetadata(true, new PropertyChangedCallback(OnShowHRulerChanged)));

        /// <summary>
        /// Identifies the ShowVerticalRuler property.This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowVerticalRulerProperty = DependencyProperty.Register("ShowVerticalRulers", typeof(bool), typeof(DiagramView), new PropertyMetadata(true, new PropertyChangedCallback(OnShowVRulerChanged)));

        /// <summary>
        /// Identifies the ZoomFactor property.This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty ZoomFactorProperty = DependencyProperty.Register("ZoomFactor", typeof(double), typeof(DiagramView), new PropertyMetadata(.2d, new PropertyChangedCallback(OnZoomFactorChanged)));

        /// <summary>
        /// Identifies the CurrentZoom property.This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty CurrentZoomProperty = DependencyProperty.Register("CurrentZoom", typeof(double), typeof(DiagramView), new PropertyMetadata(1d, new PropertyChangedCallback(OnCurrentZoomChanged)));

        /// <summary>
        /// Identifies the ViewGridOrigin property.This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewGridOriginProperty = DependencyProperty.Register("ViewGridOrigin", typeof(Point), typeof(DiagramView), new PropertyMetadata(new Point(0, 0), new PropertyChangedCallback(OnViewGridOriginChanged)));

        /// <summary>
        /// Identifies the NodeContextMenu property.This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty NodeContextMenuProperty = DependencyProperty.Register("NodeContextMenu", typeof(ContextMenu), typeof(DiagramView), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the LineConnectorContextMenu property.This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty LineConnectorContextMenuProperty = DependencyProperty.Register("LineConnectorContextMenu", typeof(ContextMenu), typeof(DiagramView), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the HorizontalScrollBarVisibility dependency property.
        /// </summary>
        public static readonly DependencyProperty GetHorizontalScrollBarVisibilityProperty = DependencyProperty.Register("GetHorizontalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(DiagramView), new UIPropertyMetadata(ScrollBarVisibility.Auto, new PropertyChangedCallback(OnHorizontalScrollBarVisibilityChanged)));

        /// <summary>
        /// Identifies the VerticalScrollBarVisibility dependency property.
        /// </summary>
        public static readonly DependencyProperty GetVerticalScrollBarVisibilityProperty = DependencyProperty.Register("GetVerticalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(DiagramView), new UIPropertyMetadata(ScrollBarVisibility.Auto, new PropertyChangedCallback(OnVerticalScrollBarVisibilityChanged)));

        /// <summary>
        /// Identifies the HorizontalScrollBarVisibility dependency property.
        /// </summary>
        public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty = DependencyProperty.Register("HorizontalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(DiagramView), new UIPropertyMetadata(ScrollBarVisibility.Auto));

        /// <summary>
        /// Identifies the VerticalScrollBarVisibility dependency property.
        /// </summary>
        public static readonly DependencyProperty VerticalScrollBarVisibilityProperty = DependencyProperty.Register("VerticalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(DiagramView), new UIPropertyMetadata(ScrollBarVisibility.Auto));
        ///<summary>
        ///Enables the Drawingtool dependency property
        ///</summary>
        public static readonly DependencyProperty EnableDrawingToolsProperty = DependencyProperty.Register("EnableDrawingTools", typeof(bool), typeof(DiagramView), new PropertyMetadata(false, new PropertyChangedCallback(OnEnableDrawingToolsChanged)));

        public static readonly DependencyProperty DrawingToolsProperty =
           DependencyProperty.Register("DrawingTool", typeof(DrawingTools), typeof(DiagramView), new UIPropertyMetadata(DrawingTools.Ellipse, new PropertyChangedCallback(OnDrawingToolsChanged)));

        #endregion

        #region Events

        private static void OnEnableConnectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
          
            DiagramView view=(DiagramView)d;
           
            if(view.EnableConnection)
            {
                view.EnableDrawingTools =false;
                (view.Page as DiagramPage).IsPolyLineEnabled =false;
            }
           

        }

        private static void OnEnableDrawingToolsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramView view=(DiagramView)d;
            if(view.EnableDrawingTools )
            {
                view.EnableConnection  =false;
                (view.Page as DiagramPage).IsPolyLineEnabled =false;
            }
           
        }

        private static void OnDrawingToolsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramView view = (DiagramView)d;
            if (view != null)
            {
                view.temppath=null; 
            }
        }

        /// <summary>
        /// Called when [is page editable changed].
        /// </summary>
        /// <param name="d">The DependencyObject.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnIsPageEditableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramView.PageEdit = true;
            DiagramView view = (DiagramView)d;
            if (!view.IsPageEditable)
            {
                DiagramView.PageEdit = false;
                (view.Page as DiagramPage).SelectionList.Clear();
            }
            (view.Page as DiagramPage).InvalidateVisual();
        }

        private static void OnIsPanEnableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
           
            DiagramView view = (DiagramView)d;
            if (!view.IsPanEnabled)
            {
                (view.Page as DiagramPage).InvalidateVisual();
            }
            
        }

        /// <summary>
        /// Calls OnShowGridChanged method of the instance, notifies of the dependency property value changes.
        /// </summary>
        /// <param name="d">Dependency object, the change occurs on.</param>
        /// <param name="e">Property change details, such as old value and new value.</param>
        private static void OnShowGridChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramView view = (DiagramView)d;
            if (view.mViewGrid != null)
            {
                view.mViewGrid.InvalidateVisual();
            }
        }

        /// <summary>
        /// Calls OnGridHOffsetChanged method of the instance, notifies of the dependency property value changes.
        /// </summary>
        /// <param name="d">Dependency object, the change occurs on.</param>
        /// <param name="e">Property change details, such as old value and new value.</param>
        private static void OnGridHOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramView view = (DiagramView)d;
            if (view.mViewGrid != null)
            {
                view.mViewGrid.InvalidateVisual();
            }
        }

        /// <summary>
        /// Calls OnGridVOffsetChanged method of the instance, notifies of the dependency property value changes.
        /// </summary>
        /// <param name="d">Dependency object, the change occurs on.</param>
        /// <param name="e">Property change details, such as old value and new value.</param>
        private static void OnGridVOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramView view = (DiagramView)d;
            if (view.mViewGrid != null)
            {
                view.mViewGrid.InvalidateVisual();
            }
        }

        /// <summary>
        /// Calls OnHorizontalLineStyleChanged method of the instance, notifies of the dependency property value changes.
        /// </summary>
        /// <param name="d">Dependency object, the change occurs on.</param>
        /// <param name="e">Property change details, such as old value and new value.</param>
        private static void OnHorizontalLineStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramView view = (DiagramView)d;
            if (view.mViewGrid != null)
            {
                view.mViewGrid.InvalidateVisual();
            }
        }
        private static void OnVirtualizationRelatedToLineChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramView view = (DiagramView)d;
            if(view!=null)
            {
                if (view.EnableCaching)
                {
                    if (view.ScrollGrid != null)
                    {
                       // view.ScrollGrid.VirtualizationRelatedToLine();
                    }
                }
            }
        }
        private static void OnIsVirtualizationEnabled(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramView view = (DiagramView)d;
            if (view!=null&&view.EnableVirtualization)
            {
                if (view.ScrollGrid != null)
                {
                    view.ScrollGrid.callCalculate();
                }
            }
            else if (view != null&&view.ScrollGrid!=null)
            {
                view.ScrollGrid.CallNormailzation();
            }
        }
        /// <summary>
        /// Calls OnVerticalLineStyleChanged method of the instance, notifies of the dependency property value changes.
        /// </summary>
        /// <param name="d">Dependency object, the change occurs on.</param>
        /// <param name="e">Property change details, such as old value and new value.</param>
        private static void OnVerticalLineStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramView view = (DiagramView)d;
            if (view.mViewGrid != null)
            {
                view.mViewGrid.InvalidateVisual();
            }
        }

        /// <summary>
        /// Calls OnShowHLineChanged method of the instance, notifies of the dependency property value changes.
        /// </summary>
        /// <param name="d">Dependency object, the change occurs on.</param>
        /// <param name="e">Property change details, such as old value and new value.</param>
        private static void OnShowHLineChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramView view = (DiagramView)d;
            if (view.mViewGrid != null)
            {
                view.mViewGrid.InvalidateVisual();
            }
        }

        /// <summary>
        /// Calls OnShowVLineChanged method of the instance, notifies of the dependency property value changes.
        /// </summary>
        /// <param name="d">Dependency object, the change occurs on.</param>
        /// <param name="e">Property change details, such as old value and new value.</param>
        private static void OnShowVLineChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramView view = (DiagramView)d;
            if (view.mViewGrid != null)
            {
                view.mViewGrid.InvalidateVisual();
            }
        }

        /// <summary>
        /// Calls OnShowHRulerChanged method of the instance, notifies of the dependency property value changes.
        /// </summary>
        /// <param name="d">Dependency object, the change occurs on.</param>
        /// <param name="e">Property change details, such as old value and new value.</param>
        private static void OnShowHRulerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {


            ////DiagramView view = (DiagramView)d;
            ////if (view.IsPageEditable)
            ////{
            ////    if (view.HorizontalRuler != null && view.VerticalRuler != null)
            ////    {
            ////        if (!view.ShowHorizontalRulers)
            ////        {
            ////            view.HorizontalRuler.Visibility = Visibility.Collapsed;
            ////        }
            ////        else
            ////        {
            ////            view.HorizontalRuler.Visibility = Visibility.Visible;
            ////        }
            ////    }
            ////}
        }

        /// <summary>
        /// Calls OnShowVRulerChanged method of the instance, notifies of the dependency property value changes.
        /// </summary>
        /// <param name="d">Dependency object, the change occurs on.</param>
        /// <param name="e">Property change details, such as old value and new value.</param>
        private static void OnShowVRulerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ////DiagramView view = (DiagramView)d;

            ////if (view.IsPageEditable)
            ////{
            ////    if (view.HorizontalRuler != null && view.VerticalRuler != null)
            ////    {
            ////        if (!view.ShowVerticalRulers)
            ////        {
            ////            view.VerticalRuler.Visibility = Visibility.Collapsed;
            ////        }
            ////        else
            ////        {
            ////            view.VerticalRuler.Visibility = Visibility.Visible;
            ////        }
            ////    }
            ////}
        }

        /// <summary>
        /// Calls OnZoomFactorChanged method of the instance, notifies of the dependency property value changes.
        /// </summary>
        /// <param name="d">Dependency object, the change occurs on.</param>
        /// <param name="e">Property change details, such as old value and new value.</param>
        private static void OnZoomFactorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramView view = (DiagramView)d;
            view.InvalidateVisual();
        }

        /// <summary>
        /// Calls OnCurrentZoomChanged method of the instance, notifies of the dependency property value changes.
        /// </summary>
        /// <param name="d">Dependency object, the change occurs on.</param>
        /// <param name="e">Property change details, such as old value and new value.</param>
        private static void OnCurrentZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramView view = (DiagramView)d;
            view.UpdateRuler(view);
        }

        internal void InvalidateViewGrid()
        {
            this.LayoutUpdated += new EventHandler(DiagramView_LayoutUpdated);
        }

        void DiagramView_LayoutUpdated(object sender, EventArgs e)
        {
            this.LayoutUpdated -= new EventHandler(DiagramView_LayoutUpdated);
            UpdateViewGridOrigin();
        }

        internal void UpdateViewGridOrigin()
        {
            //Point pt = new Point((Page as DiagramPage).Left - Scrollviewer.HorizontalOffset, (Page as DiagramPage).Top - Scrollviewer.VerticalOffset);
            //ViewGridOrigin = new Point(pt.X * CurrentZoom, pt.Y * CurrentZoom);
            ViewGridOrigin = Page.TranslatePoint(new Point(0, 0), Scrollviewer);
        }

        internal Point panPoint = new Point(0, 0);

        internal void Pan(Point delta)
        {
            Debug.WriteLine(delta);
            (Scrollviewer.Content as ScrollableGrid)._Offset.X = -delta.X;
            (Scrollviewer.Content as ScrollableGrid)._Offset.Y = -delta.Y;
            if ((Scrollviewer.Content as ScrollableGrid)._Offset.X < 0)
            {
                panPoint.X = -(Scrollviewer.Content as ScrollableGrid)._Offset.X;
            }
            if ((Scrollviewer.Content as ScrollableGrid)._Offset.Y < 0)
            {
                panPoint.Y = -(Scrollviewer.Content as ScrollableGrid)._Offset.Y;
            }
            (Scrollviewer.Content as ScrollableGrid).InvalidateArrange();
            Page.InvalidateMeasure();
        }

        /// <summary>
        /// Gets the rounding value for the measurement units.
        /// </summary>
        /// <returns>The rounding value</returns>
        private double GetRounding()
        {
            //double c = 50;// MeasureUnitsConverter.FromPixels(50, (this.Page as DiagramPage).MeasurementUnits);
            switch ((this.Page as DiagramPage).MeasurementUnits)
            {
                case MeasureUnits.Centimeter:
                    return 1;
                case MeasureUnits.Display:
                    return 40;
                case MeasureUnits.Document:
                    return 150;
                case MeasureUnits.EighthInch:
                    return 4;
                case MeasureUnits.Foot:
                    return .04;
                case MeasureUnits.HalfInch:
                    return 1;
                case MeasureUnits.Inch:
                    return .5;
                case MeasureUnits.Kilometer:
                    return 0.00002;
                case MeasureUnits.Meter:
                    return 0.01;
                case MeasureUnits.Mile:
                    return .00001;
                case MeasureUnits.Millimeter:
                    return 13;
                case MeasureUnits.Pixel:
                    return 50;
                case MeasureUnits.Point:
                    return 40;
                case MeasureUnits.QuarterInch:
                    return 2;
                case MeasureUnits.SixteenthInch:
                    return 8;
                case MeasureUnits.Yard:
                    return .01;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Gets the position when the interval is default.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <returns>The position</returns>
        private double GetDefaultPosition(double x)
        {
            double c = MeasureUnitsConverter.FromPixels(50, (this.Page as DiagramPage).MeasurementUnits);

            double mul = (50 * x) / c;
            switch ((this.Page as DiagramPage).MeasurementUnits)
            {
                case MeasureUnits.Centimeter:
                    return mul * 1;
                case MeasureUnits.Display:
                    return mul * 40;
                case MeasureUnits.Document:
                    return mul * 150;
                case MeasureUnits.EighthInch:
                    return mul * 4;
                case MeasureUnits.Foot:
                    return mul * 0.04;
                case MeasureUnits.HalfInch:
                    return mul * 1;
                case MeasureUnits.Inch:
                    return mul * .5;
                case MeasureUnits.Kilometer:
                    return mul * 0.00002;
                case MeasureUnits.Meter:
                    return mul * 0.01;
                case MeasureUnits.Mile:
                    return mul * .00001;
                case MeasureUnits.Millimeter:
                    return mul * 13;
                case MeasureUnits.Pixel:
                    return mul * 50;
                case MeasureUnits.Point:
                    return mul * 40;
                case MeasureUnits.QuarterInch:
                    return mul * 2;
                case MeasureUnits.SixteenthInch:
                    return mul * 8;
                case MeasureUnits.Yard:
                    return mul * .01;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Gets the position in the current unit interval.
        /// </summary>
        /// <returns>The position </returns>
        private double GetPosition()
        {
            //dview = DiagramPage.GetDiagramControl(this).View;
            double mul = 0;
            double c = MeasureUnitsConverter.FromPixels(50, (this.Page as DiagramPage).MeasurementUnits);

            mul = this.pixelvalue / c;
            switch ((this.Page as DiagramPage).MeasurementUnits)
            {
                case MeasureUnits.Centimeter:
                    return mul * 1;
                case MeasureUnits.Display:
                    return mul * 40;
                case MeasureUnits.Document:
                    return mul * 150;
                case MeasureUnits.EighthInch:
                    return mul * 4;
                case MeasureUnits.Foot:
                    return mul * 0.04;
                case MeasureUnits.HalfInch:
                    return mul;
                case MeasureUnits.Inch:
                    return mul * .5;
                case MeasureUnits.Kilometer:
                    return mul * 0.00002;
                case MeasureUnits.Meter:
                    return mul * 0.01;
                case MeasureUnits.Mile:
                    return mul * .00001;
                case MeasureUnits.Millimeter:
                    return mul * 13;
                case MeasureUnits.Pixel:
                    return mul * 50;
                case MeasureUnits.Point:
                    return mul * 40;
                case MeasureUnits.QuarterInch:
                    return mul * 2;
                case MeasureUnits.SixteenthInch:
                    return mul * 8;
                case MeasureUnits.Yard:
                    return mul * .01;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Gets the vertical ruler position
        /// </summary>
        /// <returns>The vertical ruler position</returns>
        private double GetVerticalPosition()
        {
            //dview = DiagramPage.GetDiagramControl(this).View;
            double mul = 0;
            double c = MeasureUnitsConverter.FromPixels(50, (this.Page as DiagramPage).MeasurementUnits);

            mul = this.pixelvalue / c;
            switch ((this.Page as DiagramPage).MeasurementUnits)
            {
                case MeasureUnits.Centimeter:
                    return mul * 1;
                case MeasureUnits.Display:
                    return mul * 40;
                case MeasureUnits.Document:
                    return mul * 150;
                case MeasureUnits.EighthInch:
                    return mul * 4;
                case MeasureUnits.Foot:
                    return mul * 0.04;
                case MeasureUnits.HalfInch:
                    return mul;
                case MeasureUnits.Inch:
                    return mul * .5;
                case MeasureUnits.Kilometer:
                    return mul * 0.00002;
                case MeasureUnits.Meter:
                    return mul * 0.01;
                case MeasureUnits.Mile:
                    return mul * .00001;
                case MeasureUnits.Millimeter:
                    return mul * 13;
                case MeasureUnits.Pixel:
                    return mul * 50;
                case MeasureUnits.Point:
                    return mul * 40;
                case MeasureUnits.QuarterInch:
                    return mul * 2;
                case MeasureUnits.SixteenthInch:
                    return mul * 8;
                case MeasureUnits.Yard:
                    return mul * .01;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Converts the value from pixels to the current measurement unit.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The Converted value</returns>
        internal double ConvertValue(double value)
        {
            double b = MeasureUnitsConverter.FromPixels(value, (this.Page as DiagramPage).MeasurementUnits);
            return b;
        }

        /// <summary>
        /// Updates the rulers.
        /// </summary>
        /// <param name="view">DiagramView instance</param>
        public void UpdateRuler(DiagramView view)
        {
            if (view.onReset)
            {
                view.i = 1;
                view.j = 1;
                view.morethanone = true;
            }

            try
            {
                if (this.VerticalRuler != null && VisualTreeHelper.GetChildrenCount(this.VerticalRuler) > 0)
                {
                    Border b = (Border)VisualTreeHelper.GetChild(this.VerticalRuler, 0);
                    Grid g = (Grid)VisualTreeHelper.GetChild(b, 0);
                    Grid g1 = (Grid)VisualTreeHelper.GetChild(g, 0);
                    vertickbar = (TickBar)VisualTreeHelper.GetChild(g1, 0);
                }

                if (this.HorizontalRuler != null && VisualTreeHelper.GetChildrenCount(this.HorizontalRuler) > 0)
                {
                    Border b1 = (Border)VisualTreeHelper.GetChild(this.HorizontalRuler, 0);
                    Grid g2 = (Grid)VisualTreeHelper.GetChild(b1, 0);
                    Grid g3 = (Grid)VisualTreeHelper.GetChild(g2, 0);
                    hortickbar = (TickBar)VisualTreeHelper.GetChild(g3, 0);
                }

                //if ((view.Page as DiagramPage).IsUnitChanged)
                //{
                //    if (view.hortickbar != null)
                //    {
                //        view.hortickbar.OriginalInterval = GetPosition();
                //    }

                //    if (view.vertickbar != null)
                //    {
                //        view.vertickbar.OriginalInterval = GetVerticalPosition();
                //    }
                //}

                //(view.Page as DiagramPage).IsUnitChanged = false;
                view.CurrentZoom = Math.Round(view.CurrentZoom, 4);

                if (view.CurrentZoom >= 1 && view.CurrentZoom < 3)
                {
                    if (view.hortickbar != null)
                    {
                        view.hortickbar.OriginalInterval = GetDefaultPosition(1);
                    }

                    if (view.vertickbar != null)
                    {
                        view.vertickbar.OriginalInterval = GetDefaultPosition(1);
                    }

                    view.lessthanone = true;
                    view.pixelvalue = 50;
                    view.firstzoom = false;
                    leveloneexe = true;
                    if (view.morethanone)
                    {
                        if (j != 0)
                        {
                            j++;
                        }

                        if (count != 0)
                        {
                            count++;
                        }

                        view.morethanone = false;
                    }
                }

                if (view.CurrentZoom < 1)
                {
                    leveloneexe = true;
                    if (view.hortickbar != null)
                    {
                        view.hortickbar.OriginalInterval = GetDefaultPosition(3);
                    }

                    if (view.vertickbar != null)
                    {
                        view.vertickbar.OriginalInterval = GetDefaultPosition(3);
                    }

                    view.morethanone = true;
                    view.firstzoom = true;
                    view.pixelvalue = 50;

                    if (view.lessthanone)
                    {
                        i = 1;
                        j = 0;
                        view.lessthanone = false;
                    }
                }

                if (view.i <= 3 && !(view.i < 0) && !leveloneexe)
                {
                    if ((view.CurrentZoom >= (3 * view.i)) && (view.CurrentZoom < 30))
                    {
                        if (view.firstzoom)
                        {
                            if (view.hortickbar != null)
                            {
                                view.hortickbar.OriginalInterval = view.hortickbar.OriginalInterval / 4;
                            }

                            if (view.vertickbar != null)
                            {
                                view.vertickbar.OriginalInterval = view.vertickbar.OriginalInterval / 4;
                            }

                            view.firstzoom = false;
                            view.pixelvalue /= 4;
                        }
                        else
                        {
                            if (view.hortickbar != null)
                            {
                                view.hortickbar.OriginalInterval = view.hortickbar.OriginalInterval / 2;
                            }

                            if (view.vertickbar != null)
                            {
                                view.vertickbar.OriginalInterval = view.vertickbar.OriginalInterval / 2;
                            }

                            view.pixelvalue /= 2;
                        }

                        if (count != 0)
                        {
                            view.count--;
                        }

                        view.i++;
                        if (view.j != 0)
                        {
                            view.j--;
                        }

                        view.morethanone = true;
                        view.lessthanone = true;
                        zoominexe = true;
                    }
                }

                if (view.j <= 3 && !(view.j < 0) && (!zoominexe) && !leveloneexe)
                {
                    if ((view.CurrentZoom <= (3 * (view.i - 1))) && view.CurrentZoom > 1)
                    {
                        if (view.hortickbar != null)
                        {
                            view.hortickbar.OriginalInterval = view.hortickbar.OriginalInterval * 2;
                        }

                        if (view.vertickbar != null)
                        {
                            view.vertickbar.OriginalInterval = view.vertickbar.OriginalInterval * 2;
                        }

                        view.pixelvalue = 50;

                        view.j++;
                        view.count--;
                        if (view.i != 0)
                        {
                            view.i--;
                        }

                        view.lessthanone = true;
                    }
                }

                zoominexe = false;
                leveloneexe = false;
                if (view.hortickbar != null)
                {
                    view.hortickbar.Interval = view.hortickbar.OriginalInterval * view.CurrentZoom;
                }

                if (view.vertickbar != null)
                {
                    view.vertickbar.Interval = view.vertickbar.OriginalInterval * view.CurrentZoom;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Called when [view grid origin changed].
        /// </summary>
        /// <param name="d">The DependencyObject.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnViewGridOriginChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramView view = (DiagramView)d;
            ViewGridOriginChanged = true;
            view.InvalidateVisual();
            if (view.vertickbar != null)
            {
                view.vertickbar.InvalidateVisual();
            }

            if (view.hortickbar != null)
            {
                view.hortickbar.InvalidateVisual();
            }
        }

        /// <summary>
        /// Called when [horizontal scroll bar visibility changed].
        /// </summary>
        /// <param name="d">The DependencyObject.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnHorizontalScrollBarVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramView view = (DiagramView)d;
            view.UpdateViewGridOrigin();
            //DiagramPage page = view.Page as DiagramPage;
            //if (page.Ver < 0 && view.HorizontalScrollBarVisibility != ScrollBarVisibility.Hidden)
            //{
            //    if ((ScrollBarVisibility)e.OldValue == ScrollBarVisibility.Visible)
            //    {
            //        if ((ScrollBarVisibility)e.NewValue == ScrollBarVisibility.Hidden)
            //        {
            //            page.ScrollX = 17.0;
            //        }
            //    }
            //}
        }

        /// <summary>
        /// Called when [vertical scroll bar visibility changed].
        /// </summary>
        /// <param name="d">The DependencyObject.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnVerticalScrollBarVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramView view = (DiagramView)d;
            view.UpdateViewGridOrigin();
            //DiagramPage page = view.Page as DiagramPage;
            //if (page.Hor < 0 && view.VerticalScrollBarVisibility != ScrollBarVisibility.Hidden)
            //{
            //    if ((ScrollBarVisibility)e.OldValue == ScrollBarVisibility.Visible)
            //    {
            //        if ((ScrollBarVisibility)e.NewValue == ScrollBarVisibility.Hidden)
            //        {
            //            page.ScrollY = 17.0;
            //        }
            //    }
            //}
        }

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Calls property changed event handler.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raised when the appropriate property changes.
        /// </summary>
        /// <param name="name">The property name</param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Scrolls to the specified node.
        /// </summary>
        /// <param name="node">The node object.</param>
        public void ScrollToNode(Node node)
        {
            node.BringIntoView();
            //scrolledtonode = true;
        }

        /// <summary>
        /// Invoked when ZoomIn Command is Executed.
        /// </summary>
        /// <param name="dview">The diagramview instance.</param>
        public void ZoomIn(DiagramView dview)
        {
            if (dview.IsPageEditable && this.IsZoomEnabled)
            {
                //dview.Scrollviewer.ScrollToHorizontalOffset(dview.hf * dview.CurrentZoom);
                //dview.Scrollviewer.ScrollToVerticalOffset(dview.vf * dview.CurrentZoom);
                if (dview.onReset)
                {
                    dview.CurrentZoom = 1;
                    dview.onReset = false;
                }

                dview.CurrentZoom += dview.ZoomFactor;
                if (dview.CurrentZoom >= 30)
                {
                    dview.CurrentZoom = 30;
                }

                dview.zoomTransform = new ScaleTransform(dview.CurrentZoom, dview.CurrentZoom);
                dview.ViewGrid.LayoutTransform = dview.zoomTransform;
                //dview.Scrollviewer.ScrollToHorizontalOffset(dview.hf * dview.CurrentZoom);
                //dview.Scrollviewer.ScrollToVerticalOffset(dview.vf * dview.CurrentZoom);
                //dview.ViewGridOrigin = new Point(dview.X * dview.CurrentZoom + dview.xcoordinate, dview.Y * dview.CurrentZoom + dview.ycoordinate);
                dview.UpdateLayout();


                //double hrt = (dview.Page as DiagramPage).CurrentMinY < (dview.Page as DiagramPage).Dragtop ? (dview.Page as DiagramPage).CurrentMinY : (dview.Page as DiagramPage).Dragtop;
                //hrt += dview.Scrollviewer.VerticalOffset / dview.CurrentZoom;
                //double vrt = (dview.Page as DiagramPage).CurrentMinX < (dview.Page as DiagramPage).Dragleft ? (dview.Page as DiagramPage).CurrentMinX : (dview.Page as DiagramPage).Dragleft;
                //vrt += dview.Scrollviewer.HorizontalOffset / dview.CurrentZoom;
                //dview.ViewGridOrigin = new Point(-vrt * dview.CurrentZoom, -hrt * dview.CurrentZoom);
                dview.UpdateViewGridOrigin();
            }
        }

        /// <summary>
        /// Invoked when ZoomOut Command is Executed.
        /// </summary>
        /// <param name="dview">The diagramview instance.</param>
        public void ZoomOut(DiagramView dview)
        {
            if (dview.IsPageEditable && this.IsZoomEnabled)
            {
                if (this.onReset)
                {
                    this.onReset = false;
                }

                double oldzoom = CurrentZoom;
                CurrentZoom -= ZoomFactor;

                if (CurrentZoom < .3)
                {
                    CurrentZoom = .3;
                }

                if (this.CurrentZoom >= 30)
                {
                    this.CurrentZoom = 30;
                }

                this.zoomTransform = new ScaleTransform(CurrentZoom, CurrentZoom);
                viewgrid.LayoutTransform = zoomTransform;

                //Scrollviewer.ScrollToHorizontalOffset(hf * CurrentZoom);
                //Scrollviewer.ScrollToVerticalOffset(vf * CurrentZoom);
                Page.UpdateLayout();
                dview.UpdateLayout();


                //double hrt = (dview.Page as DiagramPage).CurrentMinY < (dview.Page as DiagramPage).Dragtop ? (dview.Page as DiagramPage).CurrentMinY : (dview.Page as DiagramPage).Dragtop;
                //hrt += dview.Scrollviewer.VerticalOffset / dview.CurrentZoom;
                //double vrt = (dview.Page as DiagramPage).CurrentMinX < (dview.Page as DiagramPage).Dragleft ? (dview.Page as DiagramPage).CurrentMinX : (dview.Page as DiagramPage).Dragleft;
                //vrt += dview.Scrollviewer.HorizontalOffset / dview.CurrentZoom;
                //dview.ViewGridOrigin = new Point(-vrt * dview.CurrentZoom, -hrt * dview.CurrentZoom);
               
            }
            dview.UpdateViewGridOrigin();
        }

        /// <summary>
        /// Invoked when Reset Command is Executed.
        /// </summary>
        /// <param name="dview">The diagramview instance.</param>
        public void Reset(DiagramView dview)
        {
            dview.onReset = true;
            if (dview.IsPageEditable)
            {
                if (dview.HorizontalRuler != null && dview.VerticalRuler != null)
                {
                    ////double hrt = (dview.Page as DiagramPage).CurrentMinY;
                    ////double vrt = (dview.Page as DiagramPage).CurrentMinX;
                    //double hrt = (dview.Page as DiagramPage).CurrentMinY < (dview.Page as DiagramPage).Dragtop ? (dview.Page as DiagramPage).CurrentMinY : (dview.Page as DiagramPage).Dragtop;
                    //double vrt = (dview.Page as DiagramPage).CurrentMinX < (dview.Page as DiagramPage).Dragleft ? (dview.Page as DiagramPage).CurrentMinX : (dview.Page as DiagramPage).Dragleft;
                    //Point p = dview.viewgrid.TranslatePoint(new Point(0, 0), dview);
                    //double tx = Math.Abs((dview.Page as DiagramPage).CurrentMinX) - (dview.VerticalRuler.RulerThickness - 8.5);
                    //double ty = Math.Abs((dview.Page as DiagramPage).CurrentMinY) - (dview.HorizontalRuler.RulerThickness - 8.5);

                    ////dview.xcoordinate = 0;
                    ////dview.ycoordinate = 0;
                    ////dview.X = -(dview.Page as DiagramPage).CurrentMinX;
                    ////dview.Y = -(dview.Page as DiagramPage).CurrentMinY;
                    //dview.ViewGridOrigin = new Point(-vrt, -hrt);
                }
                dview.panPoint = new Point(0, 0);
                dview.Scrollviewer.ScrollToHorizontalOffset(0);
                dview.Scrollviewer.ScrollToVerticalOffset(0);
                dview.CurrentZoom = 1;
                dview.zoomTransform = new ScaleTransform(1, 1);
                dview.ViewGrid.LayoutTransform = dview.zoomTransform;
                //dview.translateTransform = new TranslateTransform(0, 0);
                //dview.ViewGrid.RenderTransform = dview.translateTransform;
                dview.UpdateLayout();
                dview.Page.InvalidateMeasure();
                //dview.Scrollviewer.ScrollToHorizontalOffset(0);
                //dview.Scrollviewer.ScrollToVerticalOffset(0);
                //(dview.Page as DiagramPage).HorValue = 0;
                //(dview.Page as DiagramPage).VerValue = 0;
                //dview.hf = 0;
                //dview.vf = 0;
                //(dview.Page as DiagramPage).Hor = 0;
                //(dview.Page as DiagramPage).Ver = 0;
            }
            dview.InvalidateViewGrid();
        }

        /// <summary>
        /// Invoked whenever a key is pressed and this control has the focus.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
        private void DiagramView_KeyDown(object sender, KeyEventArgs e)
        {
            if (IsPageEditable)
            {
                if (e.Key == Key.Escape)
                {
                    Reset(sender as DiagramView);
                }
                else if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.Z)
                {
                    //DiagramCommandManager.UndoCommand(sender as DiagramView);
                    DiagramCommandManager.Undo.Execute(this.Page, this);
                }
                else if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.Y)
                {
                    DiagramCommandManager.RedoCommand(sender as DiagramView);
                }
            }
        }

        /// <summary>
        /// Invoked when MoveUp Command is Executed.
        /// </summary>
        /// <param name="dview">The DiagramView instance.</param>
        public static void MoveUp(DiagramView dview)
        {
            if (dview.IsPageEditable)
            {
                foreach (UIElement shape in dview.SelectionList)
                {
                    double one = 1;// MeasureUnitsConverter.FromPixels(1, (dview.Page as DiagramPage).MeasurementUnits);
                    if (shape is Group)
                    {
                        foreach (INodeGroup n in (shape as Group).NodeChildren)
                        {
                            ProcessEvent(n, 1, dview);
                            if (n is Node)
                            {
                                (n as Node).PxOffsetY -= 1;// MeasureUnitsConverter.FromPixels(1, DiagramPage.Munits);
                                (n as Node).Focus();
                            }
                            //else
                            //{
                            //    if ((n as LineConnector).HeadNode == null && (n as LineConnector).TailNode == null)
                            //    {
                            //        (n as LineConnector).PxEndPointPosition = new Point((n as LineConnector).PxEndPointPosition.X, (n as LineConnector).PxEndPointPosition.Y - one);
                            //        (n as LineConnector).PxStartPointPosition = new Point((n as LineConnector).PxStartPointPosition.X, (n as LineConnector).PxStartPointPosition.Y - one);
                            //        (n as LineConnector).UpdateConnectorPathGeometry();
                            //        (n as LineConnector).Focus();
                            //    }
                            //    else if ((n as LineConnector).HeadNode != null && (n as LineConnector).TailNode == null)
                            //    {
                            //        (n as LineConnector).PxEndPointPosition = new Point((n as LineConnector).PxEndPointPosition.X, (n as LineConnector).PxEndPointPosition.Y - one);
                            //        (n as LineConnector).UpdateConnectorPathGeometry();
                            //        (n as LineConnector).Focus();
                            //    }
                            //    else if ((n as LineConnector).HeadNode == null && (n as LineConnector).TailNode != null)
                            //    {
                            //        (n as LineConnector).PxStartPointPosition = new Point((n as LineConnector).PxStartPointPosition.X, (n as LineConnector).PxStartPointPosition.Y - one);
                            //        (n as LineConnector).UpdateConnectorPathGeometry();
                            //        (n as LineConnector).Focus();
                            //    }
                            //}
                            else if (n is LineConnector)
                            {
                                (n as LineConnector).PxEndPointPosition = new Point((n as LineConnector).PxEndPointPosition.X, (n as LineConnector).PxEndPointPosition.Y - one);
                                (n as LineConnector).PxStartPointPosition = new Point((n as LineConnector).PxStartPointPosition.X, (n as LineConnector).PxStartPointPosition.Y - one);
                                if ((n as LineConnector).IntermediatePoints != null && (n as LineConnector).IntermediatePoints.Count > 0)
                                {
                                    for (int i = 0; i < (n as LineConnector).IntermediatePoints.Count; i++)
                                    {
                                        (n as LineConnector).IntermediatePoints[i] = new Point((n as LineConnector).IntermediatePoints[i].X, (n as LineConnector).IntermediatePoints[i].Y - one);
                                    }
                                }
                                (n as LineConnector).UpdateConnectorPathGeometry();
                                (n as LineConnector).Focus();
                            }
                            //(dview.Page as DiagramPage).Ver = -dview.Scrollviewer.VerticalOffset / dview.CurrentZoom;
                            (dview.Page as DiagramPage).InvalidateMeasure();
                            (dview.Page as DiagramPage).InvalidateArrange();
                        }
                    }
                    else
                    {
                        ProcessEvent(shape, 1, dview);
                        if (shape is Node)
                        {
                            (shape as Node).PxOffsetY -= 1;// MeasureUnitsConverter.FromPixels(1, DiagramPage.Munits);
                            (shape as Node).Focus();
                        }
                        //else
                        //{
                        //    if ((shape as LineConnector).HeadNode == null && (shape as LineConnector).TailNode == null)
                        //    {
                        //        (shape as LineConnector).PxEndPointPosition = new Point((shape as LineConnector).PxEndPointPosition.X, (shape as LineConnector).PxEndPointPosition.Y - one);
                        //        (shape as LineConnector).PxStartPointPosition = new Point((shape as LineConnector).PxStartPointPosition.X, (shape as LineConnector).PxStartPointPosition.Y - one);
                        //        (shape as LineConnector).UpdateConnectorPathGeometry();
                        //        (shape as LineConnector).Focus();
                        //    }
                        //    else if ((shape as LineConnector).HeadNode != null && (shape as LineConnector).TailNode == null)
                        //    {
                        //        LineConnector lc = (shape as LineConnector);
                        //        if (lc.IntermediatePoints != null && lc.IntermediatePoints.Count > 2)
                        //        {
                        //            if (
                        //                lc.IntermediatePoints[0].Equals(lc.IntermediatePoints[1])
                        //                )
                        //            {
                        //                lc.IntermediatePoints[1] = new Point(lc.IntermediatePoints[1].X + one, lc.IntermediatePoints[1].Y);
                        //                lc.IntermediatePoints[2] = new Point(lc.IntermediatePoints[2].X + one, lc.IntermediatePoints[2].Y);
                        //            }
                        //        }

                        //        (shape as LineConnector).PxEndPointPosition = new Point((shape as LineConnector).PxEndPointPosition.X, (shape as LineConnector).PxEndPointPosition.Y - one);
                        //        (shape as LineConnector).UpdateConnectorPathGeometry();
                        //        (shape as LineConnector).Focus();
                        //    }
                        //    else if ((shape as LineConnector).HeadNode == null && (shape as LineConnector).TailNode != null)
                        //    {
                        //        LineConnector lc = (shape as LineConnector);
                        //        if (lc.IntermediatePoints != null && lc.IntermediatePoints.Count > 2)
                        //        {
                        //            if (lc.IntermediatePoints[lc.IntermediatePoints.Count - 2].Equals(lc.IntermediatePoints[lc.IntermediatePoints.Count - 1]))
                        //            {
                        //                lc.IntermediatePoints[lc.IntermediatePoints.Count - 2] = new Point(lc.IntermediatePoints[lc.IntermediatePoints.Count - 2].X + one, lc.IntermediatePoints[lc.IntermediatePoints.Count - 2].Y);
                        //                lc.IntermediatePoints[lc.IntermediatePoints.Count - 3] = new Point(lc.IntermediatePoints[lc.IntermediatePoints.Count - 3].X + one, lc.IntermediatePoints[lc.IntermediatePoints.Count - 3].Y);
                        //            }
                        //        }

                        //        (shape as LineConnector).PxStartPointPosition = new Point((shape as LineConnector).PxStartPointPosition.X, (shape as LineConnector).PxStartPointPosition.Y - one);
                        //        (shape as LineConnector).UpdateConnectorPathGeometry();
                        //        (shape as LineConnector).Focus();
                        //    }
                        //    else
                        //    {
                        //        continue;
                        //        //  return;
                        //    }
                        //}
                        else if (shape is LineConnector)
                        {
                            (shape as LineConnector).PxEndPointPosition = new Point((shape as LineConnector).PxEndPointPosition.X, (shape as LineConnector).PxEndPointPosition.Y - one);
                            (shape as LineConnector).PxStartPointPosition = new Point((shape as LineConnector).PxStartPointPosition.X, (shape as LineConnector).PxStartPointPosition.Y - one);
                            if ((shape as LineConnector).IntermediatePoints != null)
                                for (int i = 0; i < (shape as LineConnector).IntermediatePoints.Count; i++)
                                {
                                    (shape as LineConnector).IntermediatePoints[i] = new Point((shape as LineConnector).IntermediatePoints[i].X, (shape as LineConnector).IntermediatePoints[i].Y - one);
                                }
                            (shape as LineConnector).UpdateConnectorPathGeometry();
                            (shape as LineConnector).Focus();
                        }

                        //(dview.Page as DiagramPage).Ver = -dview.Scrollviewer.VerticalOffset / dview.CurrentZoom;
                        (dview.Page as DiagramPage).InvalidateMeasure();
                        (dview.Page as DiagramPage).InvalidateArrange();
                    }
                }
            }
        }

        /// <summary>
        /// Clears the undo redo stack.
        /// </summary>
        public void ClearUndoRedoStack()
        {
            this.UndoStack.Clear();
            this.RedoStack.Clear();
        }

        private static void ProcessEvent(object n, int p, DiagramView dview)
        {
            if (n is Node)
            {
                NodeNudgeEventArgs nudge = new NodeNudgeEventArgs(n as Node, p);
                nudge.RoutedEvent = DiagramView.NodeMovedEvent;
                dview.RaiseEvent(nudge);
            }
            else if (n is LineConnector)
            {
                if ((n as LineConnector).HeadNode == null || (n as LineConnector).TailNode == null)
                {
                    LineNudgeEventArgs nudge = new LineNudgeEventArgs();
                    nudge = new LineNudgeEventArgs();
                    nudge.RoutedEvent = DiagramView.LineMovedEvent;
                    dview.RaiseEvent(nudge);
                }
            }
        }

        /*private static void ProvessEvent(object n)
        {
            NudgeEventArgs nudge = new NudgeEventArgs();
            if (n is Node)
            {
                nudge = new NudgeEventArgs(n as Node);
            }
            else if (n is LineConnector)
            {
                nudge = new NudgeEventArgs(n as LineConnector);
            }
            nudge.RoutedEvent = DiagramView.NodeMovedEvent;
            dview.RaiseEvent(nudge);
        }*/

        /// <summary>
        /// Invoked when MoveUp Command is Executed.
        /// </summary>
        /// <param name="dview">The DiagramView instance.</param>
        public static void MoveDown(DiagramView dview)
        {
            if (dview.IsPageEditable)
            {
                foreach (UIElement shape in dview.SelectionList)
                {
                    double one = 1;// MeasureUnitsConverter.FromPixels(1, (dview.Page as DiagramPage).MeasurementUnits);
                    if (shape is Group)
                    {
                        foreach (INodeGroup n in (shape as Group).NodeChildren)
                        {
                            ProcessEvent(n, 3, dview);
                            if (n is Node)
                            {
                                (n as Node).PxOffsetY += 1;// MeasureUnitsConverter.FromPixels(1, DiagramPage.Munits);
                                (n as Node).Focus();
                            }
                            //else
                            //{
                            //    if ((n as LineConnector).HeadNode == null && (n as LineConnector).TailNode == null)
                            //    {
                            //        (n as LineConnector).PxEndPointPosition = new Point((n as LineConnector).PxEndPointPosition.X, (n as LineConnector).PxEndPointPosition.Y + one);
                            //        (n as LineConnector).PxStartPointPosition = new Point((n as LineConnector).PxStartPointPosition.X, (n as LineConnector).PxStartPointPosition.Y + one);
                            //        (n as LineConnector).UpdateConnectorPathGeometry();
                            //        (n as LineConnector).Focus();
                            //    }
                            //    else
                            //        if ((n as LineConnector).HeadNode != null && (n as LineConnector).TailNode == null)
                            //        {

                            //            (n as LineConnector).PxEndPointPosition = new Point((n as LineConnector).PxEndPointPosition.X, (n as LineConnector).PxEndPointPosition.Y + one);
                            //            (n as LineConnector).UpdateConnectorPathGeometry();
                            //            (n as LineConnector).Focus();
                            //        }
                            //        else
                            //            if ((n as LineConnector).HeadNode == null && (n as LineConnector).TailNode != null)
                            //            {
                            //                (n as LineConnector).PxStartPointPosition = new Point((n as LineConnector).PxStartPointPosition.X, (n as LineConnector).PxStartPointPosition.Y + one);
                            //                (n as LineConnector).UpdateConnectorPathGeometry();
                            //                (n as LineConnector).Focus();
                            //            }
                            //}

                            else if (n is LineConnector)
                            {
                                (n as LineConnector).PxEndPointPosition = new Point((n as LineConnector).PxEndPointPosition.X, (n as LineConnector).PxEndPointPosition.Y + one);
                                (n as LineConnector).PxStartPointPosition = new Point((n as LineConnector).PxStartPointPosition.X, (n as LineConnector).PxStartPointPosition.Y + one);
                                if ((n as LineConnector).IntermediatePoints != null && (n as LineConnector).IntermediatePoints.Count > 0)
                                    for (int i = 0; i < (n as LineConnector).IntermediatePoints.Count; i++)
                                    {
                                        (n as LineConnector).IntermediatePoints[i] = new Point((n as LineConnector).IntermediatePoints[i].X, (n as LineConnector).IntermediatePoints[i].Y + one);
                                    }
                                (n as LineConnector).UpdateConnectorPathGeometry();
                                (n as LineConnector).Focus();
                            }
                            //(dview.Page as DiagramPage).Ver = -dview.Scrollviewer.VerticalOffset / dview.CurrentZoom;
                            (dview.Page as DiagramPage).InvalidateMeasure();
                            (dview.Page as DiagramPage).InvalidateArrange();
                        }
                    }
                    else
                    {
                        ProcessEvent(shape, 3, dview);
                        if (shape is Node)
                        {
                            (shape as Node).PxOffsetY += 1;// MeasureUnitsConverter.FromPixels(1, DiagramPage.Munits);
                            (shape as Node).Focus();
                        }
                        //else
                        //{
                        //    if ((shape as LineConnector).HeadNode == null && (shape as LineConnector).TailNode == null)
                        //    {
                        //        (shape as LineConnector).PxEndPointPosition = new Point((shape as LineConnector).PxEndPointPosition.X, (shape as LineConnector).PxEndPointPosition.Y + one);
                        //        (shape as LineConnector).PxStartPointPosition = new Point((shape as LineConnector).PxStartPointPosition.X, (shape as LineConnector).PxStartPointPosition.Y + one);
                        //        (shape as LineConnector).UpdateConnectorPathGeometry();
                        //        (shape as LineConnector).Focus();
                        //    }
                        //    else
                        //        if ((shape as LineConnector).HeadNode != null && (shape as LineConnector).TailNode == null)
                        //        {
                        //            LineConnector lc = (shape as LineConnector);
                        //            if (lc.IntermediatePoints != null && lc.IntermediatePoints.Count > 2)
                        //            {
                        //                if (
                        //                    lc.IntermediatePoints[0].Equals(lc.IntermediatePoints[1])
                        //                    )
                        //                {
                        //                    lc.IntermediatePoints[1] = new Point(lc.IntermediatePoints[1].X + one, lc.IntermediatePoints[1].Y);
                        //                    lc.IntermediatePoints[2] = new Point(lc.IntermediatePoints[2].X + one, lc.IntermediatePoints[2].Y);
                        //                }
                        //            }

                        //            (shape as LineConnector).PxEndPointPosition = new Point((shape as LineConnector).PxEndPointPosition.X, (shape as LineConnector).PxEndPointPosition.Y + one);
                        //            (shape as LineConnector).UpdateConnectorPathGeometry();
                        //            (shape as LineConnector).Focus();
                        //        }
                        //        else
                        //            if ((shape as LineConnector).HeadNode == null && (shape as LineConnector).TailNode != null)
                        //            {
                        //                LineConnector lc = (shape as LineConnector);
                        //                if (lc.IntermediatePoints != null && lc.IntermediatePoints.Count > 2)
                        //                {
                        //                    if (
                        //                        lc.IntermediatePoints[lc.IntermediatePoints.Count - 2].Equals(lc.IntermediatePoints[lc.IntermediatePoints.Count - 1])
                        //                        )
                        //                    {
                        //                        lc.IntermediatePoints[lc.IntermediatePoints.Count - 2] = new Point(lc.IntermediatePoints[lc.IntermediatePoints.Count - 2].X + one, lc.IntermediatePoints[lc.IntermediatePoints.Count - 2].Y);
                        //                        lc.IntermediatePoints[lc.IntermediatePoints.Count - 3] = new Point(lc.IntermediatePoints[lc.IntermediatePoints.Count - 3].X + one, lc.IntermediatePoints[lc.IntermediatePoints.Count - 3].Y);
                        //                    }
                        //                }

                        //                (shape as LineConnector).PxStartPointPosition = new Point((shape as LineConnector).PxStartPointPosition.X, (shape as LineConnector).PxStartPointPosition.Y + one);
                        //                (shape as LineConnector).UpdateConnectorPathGeometry();
                        //                (shape as LineConnector).Focus();
                        //            }
                        //            else
                        //            {
                        //                continue;
                        //                // return;
                        //            }
                        //}

                        else if (shape is LineConnector)
                        {
                            (shape as LineConnector).PxEndPointPosition = new Point((shape as LineConnector).PxEndPointPosition.X, (shape as LineConnector).PxEndPointPosition.Y + one);
                            (shape as LineConnector).PxStartPointPosition = new Point((shape as LineConnector).PxStartPointPosition.X, (shape as LineConnector).PxStartPointPosition.Y + one);
                            if ((shape as LineConnector).IntermediatePoints != null)
                                for (int i = 0; i < (shape as LineConnector).IntermediatePoints.Count; i++)
                                {
                                    (shape as LineConnector).IntermediatePoints[i] = new Point((shape as LineConnector).IntermediatePoints[i].X, (shape as LineConnector).IntermediatePoints[i].Y + one);
                                }
                            (shape as LineConnector).UpdateConnectorPathGeometry();
                            (shape as LineConnector).Focus();
                        }
                        //(dview.Page as DiagramPage).Ver = -dview.Scrollviewer.VerticalOffset / dview.CurrentZoom;
                        (dview.Page as DiagramPage).InvalidateMeasure();
                        (dview.Page as DiagramPage).InvalidateArrange();
                    }
                }
            }
        }

        /// <summary>
        /// Invoked when MoveUp Command is Executed.
        /// </summary>
        /// <param name="dview">The DiagramView instance.</param>
        public static void MoveLeft(DiagramView dview)
        {
            if (dview.IsPageEditable)
            {
                foreach (UIElement shape in dview.SelectionList)
                {
                    double one = 1;// MeasureUnitsConverter.FromPixels(1, (dview.Page as DiagramPage).MeasurementUnits);
                    if (shape is Group)
                    {
                        foreach (INodeGroup n in (shape as Group).NodeChildren)
                        {
                            ProcessEvent(n, 4, dview);
                            if (n is Node)
                            {
                                (n as Node).PxOffsetX -= 1;// MeasureUnitsConverter.FromPixels(1, DiagramPage.Munits);
                                (n as Node).Focus();
                            }
                            //else
                            //{
                            //    if ((n as LineConnector).HeadNode == null && (n as LineConnector).TailNode == null)
                            //    {
                            //        (n as LineConnector).PxEndPointPosition = new Point((n as LineConnector).PxEndPointPosition.X - one, (n as LineConnector).PxEndPointPosition.Y);
                            //        (n as LineConnector).PxStartPointPosition = new Point((n as LineConnector).PxStartPointPosition.X - one, (n as LineConnector).PxStartPointPosition.Y);
                            //        (n as LineConnector).UpdateConnectorPathGeometry();
                            //        (n as LineConnector).Focus();
                            //    }
                            //    else
                            //        if ((n as LineConnector).HeadNode != null && (n as LineConnector).TailNode == null)
                            //        {
                            //            (n as LineConnector).PxEndPointPosition = new Point((n as LineConnector).PxEndPointPosition.X - one, (n as LineConnector).PxEndPointPosition.Y);
                            //            (n as LineConnector).UpdateConnectorPathGeometry();
                            //            (n as LineConnector).Focus();
                            //        }
                            //        else
                            //            if ((n as LineConnector).HeadNode == null && (n as LineConnector).TailNode != null)
                            //            {
                            //                (n as LineConnector).PxStartPointPosition = new Point((n as LineConnector).PxStartPointPosition.X - one, (n as LineConnector).PxStartPointPosition.Y);
                            //                (n as LineConnector).UpdateConnectorPathGeometry();
                            //                (n as LineConnector).Focus();
                            //            }
                            //}

                            else if (n is LineConnector)
                            {
                                (n as LineConnector).PxEndPointPosition = new Point((n as LineConnector).PxEndPointPosition.X - one, (n as LineConnector).PxEndPointPosition.Y);
                                (n as LineConnector).PxStartPointPosition = new Point((n as LineConnector).PxStartPointPosition.X - one, (n as LineConnector).PxStartPointPosition.Y);
                                if ((n as LineConnector).IntermediatePoints != null)
                                    for (int i = 0; i < (n as LineConnector).IntermediatePoints.Count; i++)
                                    {
                                        (n as LineConnector).IntermediatePoints[i] = new Point((n as LineConnector).IntermediatePoints[i].X - one, (n as LineConnector).IntermediatePoints[i].Y);
                                    }
                                (n as LineConnector).UpdateConnectorPathGeometry();
                                (n as LineConnector).Focus();
                            }
                            //(dview.Page as DiagramPage).Hor = -dview.Scrollviewer.HorizontalOffset / dview.CurrentZoom;
                            (dview.Page as DiagramPage).InvalidateMeasure();
                            (dview.Page as DiagramPage).InvalidateArrange();
                        }
                    }
                    else
                    {
                        ProcessEvent(shape, 4, dview);
                        if (shape is Node)
                        {
                            (shape as Node).PxOffsetX -= 1;// MeasureUnitsConverter.FromPixels(1, DiagramPage.Munits);
                            (shape as Node).Focus();
                        }
                        //else
                        //{
                        //    if ((shape as LineConnector).HeadNode == null && (shape as LineConnector).TailNode == null)
                        //    {
                        //        (shape as LineConnector).PxEndPointPosition = new Point((shape as LineConnector).PxEndPointPosition.X - one, (shape as LineConnector).PxEndPointPosition.Y);
                        //        (shape as LineConnector).PxStartPointPosition = new Point((shape as LineConnector).PxStartPointPosition.X - one, (shape as LineConnector).PxStartPointPosition.Y);
                        //        (shape as LineConnector).UpdateConnectorPathGeometry();
                        //        (shape as LineConnector).Focus();
                        //    }
                        //    else
                        //        if ((shape as LineConnector).HeadNode != null && (shape as LineConnector).TailNode == null)
                        //        {
                        //            LineConnector lc = (shape as LineConnector);
                        //            if (lc.IntermediatePoints != null && lc.IntermediatePoints.Count > 2)
                        //            {
                        //                if (
                        //                    lc.IntermediatePoints[0].Equals(lc.IntermediatePoints[1])
                        //                    )
                        //                {
                        //                    lc.IntermediatePoints[1] = new Point(lc.IntermediatePoints[1].X, lc.IntermediatePoints[1].Y + one);
                        //                    lc.IntermediatePoints[2] = new Point(lc.IntermediatePoints[2].X, lc.IntermediatePoints[2].Y + one);
                        //                }
                        //            }

                        //            (shape as LineConnector).PxEndPointPosition = new Point((shape as LineConnector).PxEndPointPosition.X - one, (shape as LineConnector).PxEndPointPosition.Y);
                        //            (shape as LineConnector).UpdateConnectorPathGeometry();
                        //            (shape as LineConnector).Focus();
                        //        }
                        //        else
                        //            if ((shape as LineConnector).HeadNode == null && (shape as LineConnector).TailNode != null)
                        //            {
                        //                LineConnector lc = (shape as LineConnector);
                        //                if (lc.IntermediatePoints != null && lc.IntermediatePoints.Count > 2)
                        //                {
                        //                    if (
                        //                        lc.IntermediatePoints[lc.IntermediatePoints.Count - 2].Equals(lc.IntermediatePoints[lc.IntermediatePoints.Count - 1])
                        //                        )
                        //                    {
                        //                        lc.IntermediatePoints[lc.IntermediatePoints.Count - 2] = new Point(lc.IntermediatePoints[lc.IntermediatePoints.Count - 2].X, lc.IntermediatePoints[lc.IntermediatePoints.Count - 2].Y + one);
                        //                        lc.IntermediatePoints[lc.IntermediatePoints.Count - 3] = new Point(lc.IntermediatePoints[lc.IntermediatePoints.Count - 3].X, lc.IntermediatePoints[lc.IntermediatePoints.Count - 3].Y + one);
                        //                    }
                        //                }

                        //                (shape as LineConnector).PxStartPointPosition = new Point((shape as LineConnector).PxStartPointPosition.X - one, (shape as LineConnector).PxStartPointPosition.Y);
                        //                (shape as LineConnector).UpdateConnectorPathGeometry();
                        //                (shape as LineConnector).Focus();
                        //            }
                        //            else
                        //                continue;
                        //    // return;
                        //}

                        else if (shape is LineConnector)
                        {
                            (shape as LineConnector).PxEndPointPosition = new Point((shape as LineConnector).PxEndPointPosition.X - one, (shape as LineConnector).PxEndPointPosition.Y);
                            (shape as LineConnector).PxStartPointPosition = new Point((shape as LineConnector).PxStartPointPosition.X - one, (shape as LineConnector).PxStartPointPosition.Y);
                            if ((shape as LineConnector).IntermediatePoints != null)
                                for (int i = 0; i < (shape as LineConnector).IntermediatePoints.Count; i++)
                                {
                                    (shape as LineConnector).IntermediatePoints[i] = new Point((shape as LineConnector).IntermediatePoints[i].X - one, (shape as LineConnector).IntermediatePoints[i].Y);
                                }
                            (shape as LineConnector).UpdateConnectorPathGeometry();
                            (shape as LineConnector).Focus();
                        }
                        //(dview.Page as DiagramPage).Hor = -dview.Scrollviewer.HorizontalOffset / dview.CurrentZoom;
                        (dview.Page as DiagramPage).InvalidateMeasure();
                        (dview.Page as DiagramPage).InvalidateArrange();
                    }
                }
            }
        }

        /// <summary>
        /// Invoked when MoveRight Command is Executed.
        /// </summary>
        /// <param name="dview">The DiagramView instance.</param>
        public static void MoveRight(DiagramView dview)
        {
            if (dview.IsPageEditable)
            {
                foreach (UIElement shape in dview.SelectionList)
                {
                    double one = 1;// MeasureUnitsConverter.FromPixels(1, (dview.Page as DiagramPage).MeasurementUnits);
                    if (shape is Group)
                    {
                        foreach (INodeGroup n in (shape as Group).NodeChildren)
                        {
                            ProcessEvent(n, 2, dview);
                            if (n is Node)
                            {
                                (n as Node).PxOffsetX += 1;// MeasureUnitsConverter.FromPixels(1, DiagramPage.Munits);
                                (n as Node).Focus();
                            }
                            //else
                            //{
                            //    if ((n as LineConnector).HeadNode == null && (n as LineConnector).TailNode == null)
                            //    {
                            //        (n as LineConnector).PxEndPointPosition = new Point((n as LineConnector).PxEndPointPosition.X + one, (n as LineConnector).PxEndPointPosition.Y);
                            //        (n as LineConnector).PxStartPointPosition = new Point((n as LineConnector).PxStartPointPosition.X + one, (n as LineConnector).PxStartPointPosition.Y);
                            //        (n as LineConnector).UpdateConnectorPathGeometry();
                            //        (n as LineConnector).Focus();
                            //    }
                            //    else
                            //        if ((n as LineConnector).HeadNode != null && (n as LineConnector).TailNode == null)
                            //        {
                            //            (n as LineConnector).PxEndPointPosition = new Point((n as LineConnector).PxEndPointPosition.X + one, (n as LineConnector).PxEndPointPosition.Y);
                            //            (n as LineConnector).UpdateConnectorPathGeometry();
                            //            (n as LineConnector).Focus();
                            //        }
                            //        else
                            //            if ((n as LineConnector).HeadNode == null && (n as LineConnector).TailNode != null)
                            //            {
                            //                (n as LineConnector).PxStartPointPosition = new Point((n as LineConnector).PxStartPointPosition.X + one, (n as LineConnector).PxStartPointPosition.Y);
                            //                (n as LineConnector).UpdateConnectorPathGeometry();
                            //                (n as LineConnector).Focus();
                            //            }
                            //}

                            else if (n is LineConnector)
                            {
                                (n as LineConnector).PxEndPointPosition = new Point((n as LineConnector).PxEndPointPosition.X + one, (n as LineConnector).PxEndPointPosition.Y);
                                (n as LineConnector).PxStartPointPosition = new Point((n as LineConnector).PxStartPointPosition.X + one, (n as LineConnector).PxStartPointPosition.Y);
                                if ((n as LineConnector).IntermediatePoints != null)
                                    for (int i = 0; i < (n as LineConnector).IntermediatePoints.Count; i++)
                                    {
                                        (n as LineConnector).IntermediatePoints[i] = new Point((n as LineConnector).IntermediatePoints[i].X + one, (n as LineConnector).IntermediatePoints[i].Y);
                                    }
                                (n as LineConnector).UpdateConnectorPathGeometry();
                                (n as LineConnector).Focus();
                            }
                            //(dview.Page as DiagramPage).Hor = -dview.Scrollviewer.HorizontalOffset / dview.CurrentZoom;
                            (dview.Page as DiagramPage).InvalidateMeasure();
                            (dview.Page as DiagramPage).InvalidateArrange();
                        }
                    }
                    else
                    {
                        ProcessEvent(shape, 2, dview);
                        if (shape is Node)
                        {
                            (shape as Node).PxOffsetX += 1;// MeasureUnitsConverter.FromPixels(1, DiagramPage.Munits);
                            (shape as Node).Focus();
                        }
                        //else
                        //{
                        //    if ((shape as LineConnector).HeadNode == null && (shape as LineConnector).TailNode == null)
                        //    {
                        //        (shape as LineConnector).PxEndPointPosition = new Point((shape as LineConnector).PxEndPointPosition.X + one, (shape as LineConnector).PxEndPointPosition.Y);
                        //        (shape as LineConnector).PxStartPointPosition = new Point((shape as LineConnector).PxStartPointPosition.X + one, (shape as LineConnector).PxStartPointPosition.Y);
                        //        (shape as LineConnector).UpdateConnectorPathGeometry();
                        //        (shape as LineConnector).Focus();
                        //    }
                        //    else
                        //        if ((shape as LineConnector).HeadNode != null && (shape as LineConnector).TailNode == null)
                        //        {
                        //            LineConnector lc = (shape as LineConnector);
                        //            if (lc.IntermediatePoints != null && lc.IntermediatePoints.Count > 2)
                        //            {
                        //                if (
                        //                    lc.IntermediatePoints[0].Equals(lc.IntermediatePoints[1])
                        //                    )
                        //                {
                        //                    lc.IntermediatePoints[1] = new Point(lc.IntermediatePoints[1].X, lc.IntermediatePoints[1].Y + one);
                        //                    lc.IntermediatePoints[2] = new Point(lc.IntermediatePoints[2].X, lc.IntermediatePoints[2].Y + one);
                        //                }
                        //            }

                        //            (shape as LineConnector).PxEndPointPosition = new Point((shape as LineConnector).PxEndPointPosition.X + one, (shape as LineConnector).PxEndPointPosition.Y);
                        //            (shape as LineConnector).UpdateConnectorPathGeometry();
                        //            (shape as LineConnector).Focus();
                        //        }
                        //        else
                        //            if ((shape as LineConnector).HeadNode == null && (shape as LineConnector).TailNode != null)
                        //            {
                        //                LineConnector lc = (shape as LineConnector);
                        //                if (lc.IntermediatePoints != null && lc.IntermediatePoints.Count > 2)
                        //                {
                        //                    if (
                        //                        lc.IntermediatePoints[lc.IntermediatePoints.Count - 2].Equals(lc.IntermediatePoints[lc.IntermediatePoints.Count - 1])
                        //                        )
                        //                    {
                        //                        lc.IntermediatePoints[lc.IntermediatePoints.Count - 2] = new Point(lc.IntermediatePoints[lc.IntermediatePoints.Count - 2].X, lc.IntermediatePoints[lc.IntermediatePoints.Count - 2].Y + one);
                        //                        lc.IntermediatePoints[lc.IntermediatePoints.Count - 3] = new Point(lc.IntermediatePoints[lc.IntermediatePoints.Count - 3].X, lc.IntermediatePoints[lc.IntermediatePoints.Count - 3].Y + one);
                        //                    }
                        //                }

                        //                (shape as LineConnector).PxStartPointPosition = new Point((shape as LineConnector).PxStartPointPosition.X + one, (shape as LineConnector).PxStartPointPosition.Y);
                        //                (shape as LineConnector).UpdateConnectorPathGeometry();
                        //                (shape as LineConnector).Focus();
                        //            }
                        //            else
                        //                continue;
                        //    // return;
                        //}

                        else if (shape is LineConnector)
                        {
                            (shape as LineConnector).PxEndPointPosition = new Point((shape as LineConnector).PxEndPointPosition.X + one, (shape as LineConnector).PxEndPointPosition.Y);
                            (shape as LineConnector).PxStartPointPosition = new Point((shape as LineConnector).PxStartPointPosition.X + one, (shape as LineConnector).PxStartPointPosition.Y);
                            if ((shape as LineConnector).IntermediatePoints != null)
                                for (int i = 0; i < (shape as LineConnector).IntermediatePoints.Count; i++)
                                {
                                    (shape as LineConnector).IntermediatePoints[i] = new Point((shape as LineConnector).IntermediatePoints[i].X + one, (shape as LineConnector).IntermediatePoints[i].Y);
                                }
                            (shape as LineConnector).UpdateConnectorPathGeometry();
                            (shape as LineConnector).Focus();
                        }
                        //(dview.Page as DiagramPage).Hor = -dview.Scrollviewer.HorizontalOffset / dview.CurrentZoom;
                        (dview.Page as DiagramPage).InvalidateMeasure();
                        (dview.Page as DiagramPage).InvalidateArrange();
                    }
                }
            }
        }

        /// <summary>
        /// Provides class handling for the PreviewMouseWheel routed event that occurs when the mouse
        /// wheel  is moved and the mouse pointer is over this control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseWheelEventArgs"/> instance containing the event data.</param>
        private void DiagramView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            DupDeleted = false;
            //IsMouseWheeled = true;
            //IsDupMouseWheeled = true;
            //IsDupMousePressed = false;

            //PanConstant = 0;
            if (IsPageEditable)
            {
                if (IsZoomEnabled)
                {
                    if (this.onReset)
                    {
                        this.onReset = false;
                    }

                    double oldzoom = CurrentZoom;
                    if (Keyboard.Modifiers == ModifierKeys.Control)
                    {
                        if (e.Delta > 0)
                        {
                            ZoomIn(this);
                        }
                        else
                        {
                            //CurrentZoom -= ZoomFactor;
                            ZoomOut(this);
                        }
                        /*
                        if (e.Delta > 0)
                        {
                            CurrentZoom += ZoomFactor;
                        }
                        else
                        {
                            CurrentZoom -= ZoomFactor;
                        }

                        if (CurrentZoom < .3)
                        {
                            CurrentZoom = .3;
                        }

                        if (this.CurrentZoom >= 30)
                        {
                            this.CurrentZoom = 30;
                        }

                        var physicalPoint = e.GetPosition(this);
                        this.zoomTransform = new ScaleTransform(CurrentZoom, CurrentZoom);
                        viewgrid.LayoutTransform = zoomTransform;
                        //if (this.Page != null)
                        //{
                        //    if (this.Page.Background != null)
                        //    {
                        //        this.Page.Background.Transform = zoomTransform;
                        //    }
                        //}

                        Scrollviewer.ScrollToHorizontalOffset(hf * CurrentZoom);
                        Scrollviewer.ScrollToVerticalOffset(vf * CurrentZoom);

                        if (e.Delta > 0)
                        {
                            if (X > 0)
                            {
                                X = -Scrollviewer.HorizontalOffset / CurrentZoom - (this.Page as DiagramPage).CurrentMinX;
                            }

                            ViewGridOrigin = new Point(X * CurrentZoom + xcoordinate, Y * CurrentZoom + ycoordinate - (this.Page as DiagramPage).CurrentMinY);
                        }
                        else
                        {
                            ////if (CurrentZoom <= 1.8 && CurrentZoom >=1)
                            ////{
                            ////    ViewGridOrigin = new Point(X * (CurrentZoom) + xcoordinate, Y * CurrentZoom + ycoordinate);
                            ////}
                            ////else
                            if (Scrollviewer.HorizontalOffset == Scrollviewer.ScrollableWidth)
                            {
                                X = (-Math.Abs((this.Page as DiagramPage).ActualWidth * CurrentZoom - Scrollviewer.ViewportWidth)) / CurrentZoom - (this.Page as DiagramPage).CurrentMinX;
                                ViewGridOrigin = new Point(-Math.Abs((this.Page as DiagramPage).ActualWidth * CurrentZoom - Scrollviewer.ViewportWidth) + xcoordinate, Y * CurrentZoom);
                            }
                            else
                            {
                                ViewGridOrigin = new Point(X * CurrentZoom + xcoordinate, Y * CurrentZoom + ycoordinate);
                            }

                            if (Scrollviewer.VerticalOffset == Scrollviewer.ScrollableHeight && Scrollviewer.VerticalOffset != 0)
                            {
                                Y = (-Math.Abs((this.Page as DiagramPage).ActualHeight * CurrentZoom - Scrollviewer.ViewportHeight)) / CurrentZoom - (this.Page as DiagramPage).CurrentMinY;
                                ViewGridOrigin = new Point(ViewGridOrigin.X, -Math.Abs((this.Page as DiagramPage).ActualHeight * CurrentZoom - Scrollviewer.ViewportHeight) + ycoordinate);
                            }
                            else
                            {
                                ViewGridOrigin = new Point(ViewGridOrigin.X, Y * CurrentZoom + ycoordinate);
                            }

                            if ((this.Page as DiagramPage).ActualWidth * CurrentZoom <= Scrollviewer.ViewportWidth)
                            {
                                X = -(this.Page as DiagramPage).CurrentMinX * CurrentZoom;
                                ViewGridOrigin = new Point(X + xcoordinate, ViewGridOrigin.Y);
                            }

                            if ((this.Page as DiagramPage).ActualHeight * CurrentZoom <= Scrollviewer.ViewportHeight)
                            {
                                Y = -(this.Page as DiagramPage).CurrentMinY * CurrentZoom;
                                ViewGridOrigin = new Point(ViewGridOrigin.X, Y + ycoordinate);
                            }
                        }
                        */
                        e.Handled = true;
                        return;
                    }
                }

                e.Handled = false;
            }

            e.Handled = false;
        }

        /// <summary>
        /// Provides class handling for the MouseUp routed event that occurs when the mouse
        /// button  is released and the mouse pointer is over this control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void DiagramView_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (IsPageEditable)
            {
                if (IsPanEnabled)
                {
                    if (this.IsMouseCaptured)
                    {
                        if (BrowserInteropHelper.IsBrowserHosted)
                        {
                            this.Cursor = System.Windows.Input.Cursors.Hand;
                        }
                        else
                        {
                            Assembly ass = Assembly.GetExecutingAssembly();
                            Stream stream = ass.GetManifestResourceStream("Syncfusion.Windows.Diagram.Icons.Released.cur");
                            m_cursor = new Cursor(stream);
                            this.Cursor = m_cursor;
                        }

                        this.ReleaseMouseCapture();
                        e.Handled = true;
                    }
                }
                else
                {
                    this.Cursor = Cursors.Arrow;
                }

                e.Handled = false;
            }
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.PreviewMouseUp"/> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs"/> that contains the event data. The event data reports that one or more mouse buttons were released.</param>
        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            // foreach (ICommon shape in oldselectionlist)
            /// {
            //  if (!SelectionList.Contains(shape as Node) && shape is Node)
            //   {
            //   NodeRoutedEventArgs newEventArgs = new NodeRoutedEventArgs(shape as Node);
            //   newEventArgs.RoutedEvent = DiagramView.NodeUnSelectedEvent;
            //  RaiseEvent(newEventArgs);
            // }

            //  else if (!SelectionList.Contains(shape as LineConnector) && shape is LineConnector)
            //  {
            //     ConnectorRoutedEventArgs newEventArgs1 = new ConnectorRoutedEventArgs(shape as LineConnector);
            //    newEventArgs1.RoutedEvent = DiagramView.ConnectorUnSelectedEvent;
            //  RaiseEvent(newEventArgs1);
            // }/
            //

            double aa = Scrollviewer.ScrollableWidth;
            //hf = Scrollviewer.HorizontalOffset / CurrentZoom;
            //vf = Scrollviewer.VerticalOffset / CurrentZoom;

            //ScrollViewer svr = new ScrollViewer();
            //ScrollChrome schrome = new ScrollChrome();
            //Rectangle rect = new Rectangle();
            Thumb tb = new Thumb();
            object o = e.OriginalSource;
            //RepeatButton rp = new RepeatButton();
            if (o is Thumb)
            {
                tb = o as Thumb;
            }

            //if (e.Source.GetType() == svr.GetType() && (e.OriginalSource.GetType() == tb.GetType() || e.OriginalSource.GetType() == rp.GetType()))
            if (e.Source is ScrollViewer && (e.OriginalSource is Thumb || e.OriginalSource is RepeatButton))
            {
                if (!tb.Name.Equals("HeadThumb") && !tb.Name.Equals("TailThumb"))
                {
                    if (HorizontalRuler != null)
                    {
                        //IsMouseScrolled = true;
                        //(this.Page as DiagramPage).HorValue = hf;
                        if (!DupDeleted)
                        {
                            UpdateViewGridOrigin();
                            //X += DupHorthumbdrag / CurrentZoom;
                            //ViewGridOrigin = new Point((X * CurrentZoom + xcoordinate), ViewGridOrigin.Y);
                            //DupHorthumbdrag = 0;

                            //double hrt = (Page as DiagramPage).CurrentMinY < (Page as DiagramPage).Dragtop ? (Page as DiagramPage).CurrentMinY : (Page as DiagramPage).Dragtop;
                            //hrt += Scrollviewer.VerticalOffset / CurrentZoom;
                            //double vrt = (Page as DiagramPage).CurrentMinX < (Page as DiagramPage).Dragleft ? (Page as DiagramPage).CurrentMinX : (Page as DiagramPage).Dragleft;
                            //vrt += Scrollviewer.HorizontalOffset / CurrentZoom;
                            //ViewGridOrigin = new Point(-vrt * CurrentZoom, -hrt * CurrentZoom);
                            ////DupVerthumbdrag = 0;
                        }
                    }

                    if (VerticalRuler != null)
                    {
                        //IsMouseScrolled = true;

                        //(this.Page as DiagramPage).VerValue = vf;
                        if (!DupDeleted)
                        {
                            UpdateViewGridOrigin();
                            //Y += DupVerthumbdrag / CurrentZoom;
                            //vdoff += DupVerthumbdrag;
                            //ViewGridOrigin = new Point(ViewGridOrigin.X, (Y * CurrentZoom + xcoordinate));

                            //double hrt = (Page as DiagramPage).CurrentMinY < (Page as DiagramPage).Dragtop ? (Page as DiagramPage).CurrentMinY : (Page as DiagramPage).Dragtop;
                            //hrt += Scrollviewer.VerticalOffset / CurrentZoom;
                            //double vrt = (Page as DiagramPage).CurrentMinX < (Page as DiagramPage).Dragleft ? (Page as DiagramPage).CurrentMinX : (Page as DiagramPage).Dragleft;
                            //vrt += Scrollviewer.HorizontalOffset / CurrentZoom;
                            //ViewGridOrigin = new Point(-vrt * CurrentZoom, -hrt * CurrentZoom);

                            ////DupHorthumbdrag = 0;
                            //DupVerthumbdrag = 0;
                        }
                    }
                }
            }

            foreach (ICommon shape in SelectionList)
            {
                //  if (shape is Node  && !oldselectionlist.Contains(shape as Node))
                //  {
                //     NodeRoutedEventArgs newEventArgs = new NodeRoutedEventArgs(shape as Node);
                //     newEventArgs.RoutedEvent = DiagramView.NodeSelectedEvent;
                //     RaiseEvent(newEventArgs);
                // }



                oldselectionlist.Clear();
                e.Handled = false;
                base.OnMouseLeftButtonUp(e);
            }
        }

        /// <summary>
        /// Provides class handling for the MouseUp routed event that occurs when the mouse
        /// button  is pressed and the mouse pointer is over this control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void DiagramView_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            foreach (ICommon item in this.SelectionList)
            {
                if (item is IShape)
                {
                    oldselectionlist.Add(item as IShape);
                }
                else if (item is LineConnector)
                {
                    oldselectionlist.Add(item as LineConnector);
                }
            }

            ////IsMouseScrolled = false;
            DupDeleted = false;
            //IsMouseUponly = true;
            ////hf = Scrollviewer.HorizontalOffset;
            ////vf = Scrollviewer.VerticalOffset;

            //if (OldHoroffset > Scrollviewer.HorizontalOffset)
            //{
            //    IsScrolledRight = false;
            //}
            //else
            //{
            //    IsScrolledRight = true;
            //}

            //OldHoroffset = Scrollviewer.HorizontalOffset;
            //if (OldVeroffset > Scrollviewer.VerticalOffset)
            //{
            //    IsScrolledBottom = false;
            //}
            //else
            //{
            //    IsScrolledBottom = true;
            //}

            //OldVeroffset = Scrollviewer.VerticalOffset;
            ScrollViewer svr = new ScrollViewer();
            ScrollChrome schrome = new ScrollChrome();
            Thumb tb = new Thumb();
            Rectangle rect = new Rectangle();
            Point p = e.GetPosition(this);
            if (e.Source.GetType() == svr.GetType() && e.OriginalSource.GetType() != rect.GetType())
            {
                //IsMouseScrolled = false;
                //if (!IsJustScrolled)
                //{
                //    //IsMouseUponly = true;
                //    //IsJustScrolled = true;
                //    //(dc.View.Page as DiagramPage).Dragleft += (dc.View.Page as DiagramPage).Minleft;
                //    //(dc.View.Page as DiagramPage).Dragtop += (dc.View.Page as DiagramPage).MinTop;
                //}

                ////(this.Page as DiagramPage).Hor = -this.Scrollviewer.HorizontalOffset;
                ////(this.Page as DiagramPage).InvalidateMeasure();
                ////(this.Page as DiagramPage).InvalidateArrange();
                //IsScrollCheck = true;
                //IsDupMousePressed = true;
            }

            if (IsPageEditable)
            {
                if (IsPanEnabled)
                {
                    ScrollViewer sv = new ScrollViewer();
                    ScrollChrome chrome = new ScrollChrome(); 
                    //if (!(e.Source is ScrollViewer))
                    {
                        this.screenStartPoint = e.GetPosition(Page);
                        this.CaptureMouse();
                        if (BrowserInteropHelper.IsBrowserHosted)
                        {
                            this.Cursor = Cursors.Hand;
                        }
                        else
                        {
                            Assembly ass = Assembly.GetExecutingAssembly();
                            Stream stream = ass.GetManifestResourceStream("Syncfusion.Windows.Diagram.Icons.Grabbed.cur");
                            m_cursor = new Cursor(stream);
                            this.Cursor = m_cursor;
                        }
                    }
                    //else
                    //{
                    //    IsScrollCheck = true;
                    //}

                    //IsScrollCheck = false;
                    e.Handled = true;
                }
                else
                {
                    this.Cursor = Cursors.Arrow;
                }

                e.Handled = false;
            }
        }

        /// <summary>
        /// Provides class handling for the MouseMove routed event that occurs when
        /// the mouse pointer is over this control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseEventArgs"/> instance containing the event data.</param>
        private void DiagramView_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsPageEditable)
            {
                if (IsPanEnabled)
                {
                    ////this.Cursor = Cursors.Hand;
                    if (this.IsMouseCaptured)
                    {
                        if (BrowserInteropHelper.IsBrowserHosted)
                        {
                            this.Cursor = System.Windows.Input.Cursors.Hand;
                        }
                        else
                        {
                            Assembly ass = Assembly.GetExecutingAssembly();
                            Stream stream = ass.GetManifestResourceStream("Syncfusion.Windows.Diagram.Icons.Grabbed.cur");
                            m_cursor = new Cursor(stream);
                            this.Cursor = m_cursor;
                        }

                        //translatepoint = viewgrid.TranslatePoint(new Point(0, 0), this);
                        //if (HorizontalRuler != null)
                        //{
                        //    PanConstant = -translatepoint.Y + HorizontalRuler.RulerThickness - 4.25;
                        //}
                        //else
                        //{
                        //    PanConstant = -translatepoint.Y;
                        //}

                        if (VerticalRuler != null && HorizontalRuler != null)
                        {
                            //xcoordinate = translatepoint.X - VerticalRuler.RulerThickness + 8.7 - (this.Page as DiagramPage).CurrentMinX;
                            //ycoordinate = translatepoint.Y - HorizontalRuler.RulerThickness + 4.25 - (this.Page as DiagramPage).CurrentMinY;
                            //ViewGridOrigin = new Point(xcoordinate, ycoordinate);
                            InvalidateViewGrid();
                            //UpdateViewGridOrigin();
                        }

                        var physicalPoint = e.GetPosition(Scrollviewer);
                        Pan(new Point(physicalPoint.X - this.screenStartPoint.X, physicalPoint.Y - this.screenStartPoint.Y));
                        e.Handled = true;
                    }
                    else
                    {
                        if (BrowserInteropHelper.IsBrowserHosted)
                        {
                            this.Cursor = System.Windows.Input.Cursors.Hand;
                        }
                        else
                        {
                            Assembly ass = Assembly.GetExecutingAssembly();
                            Stream stream = ass.GetManifestResourceStream("Syncfusion.Windows.Diagram.Icons.Released.cur");
                            m_cursor = new Cursor(stream);
                            this.Cursor = m_cursor;
                        }
                    }
                }
                else
                {
                    this.Cursor = Cursors.Arrow;
                }

                e.Handled = false;
            }
            else
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Handles the ScrollChanged event of the Scrollviewer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.ScrollChangedEventArgs"/> instance containing the event data.</param>
        private void Scrollviewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            //if (sender is ScrollViewer)
            //{
            //    Scrollviewer = sender as ScrollViewer;
            //}

            //if (Scrollviewer.ExtentWidth > Scrollviewer.ViewportWidth)
            //{
            //    GetHorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            //}
            //else
            //{
            //    GetHorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            //}

            //if (Scrollviewer.ExtentHeight > Scrollviewer.ViewportHeight)
            //{
            //    GetVerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            //}
            //else
            //{
            //    GetVerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            //}

            //if (HorizontalScrollBarVisibility == ScrollBarVisibility.Hidden)
            //{
            //    GetHorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            //}

            //if (VerticalScrollBarVisibility == ScrollBarVisibility.Hidden)
            //{
            //    GetVerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            //}

            //if (scrolledtonode)
            //{
            //    double hc = e.HorizontalChange;
            //    double vc = e.VerticalChange;
            //    X = X - hc / CurrentZoom;
            //    Y = Y - vc / CurrentZoom;
            //    ViewGridOrigin = new Point(X * CurrentZoom, Y * CurrentZoom);
            //    scrolledtonode = false;
            //}

            //HorThumbDragOffset = OldHoroffset - Scrollviewer.HorizontalOffset;
            //DupHorthumbdrag = HorThumbDragOffset;

            //VerThumbDragOffset = OldVeroffset - Scrollviewer.VerticalOffset;
            //DupVerthumbdrag = VerThumbDragOffset;

            //hf = Scrollviewer.HorizontalOffset / CurrentZoom;
            //vf = Scrollviewer.VerticalOffset / CurrentZoom;

            //newviewportwidth = Scrollviewer.ViewportWidth;
            //if (dc != null && dc.Model != null && dc.IsSymbolPaletteVisibilityChanged)
            //{
            //    if (newviewportwidth > oldviewportwidth)
            //    {
            //        X = -(this.Page as DiagramPage).CurrentMinX;
            //        ViewGridOrigin = new Point(X, ViewGridOrigin.Y);
            //        if (oldviewportwidth != 0)
            //        {
            //            exe = true;
            //        }
            //    }
            //    else
            //    {
            //        if (exe)
            //        {
            //            X += (this.Page as DiagramPage).CurrentMinX - (this.Page as DiagramPage).Dragleft;
            //            ViewGridOrigin = new Point(X, ViewGridOrigin.Y);
            //            exe = false;
            //        }
            //    }
            //}

            //if (dc != null)
            //{
            //    dc.IsSymbolPaletteVisibilityChanged = false;
            //}

            //oldviewportwidth = Scrollviewer.ViewportWidth;

            //if (hortickbar != null)
            //{
            //    hortickbar.InvalidateVisual();
            //}

            //if (vertickbar != null)
            //{
            //    vertickbar.InvalidateVisual();
            //}
        }

        #endregion

        #region class override

        /// <summary>
        /// Invoked whenever application code or internal processes call
        /// <see cref="System.Windows.FrameworkElement.ApplyTemplate"/> method.
        /// </summary>
        public override void OnApplyTemplate()
        {
            DiagramControl d = DiagramPage.GetDiagramControl(this);
            scrollview = GetTemplateChild("PART_ScrollViewer") as ScrollViewer;
            mViewGrid = GetTemplateChild("PART_Grid") as DiagramViewGrid;
            viewgrid = GetTemplateChild("viewgrid") as Grid;

            //Rectangle rect = GetTemplateChild("PART_PageBackground") as Rectangle;
            //rect.Loaded += new RoutedEventHandler(rect_Loaded);

            base.OnApplyTemplate();
            //this.translateTransform = new TranslateTransform();
            this.zoomTransform = new ScaleTransform();
            //this.transformGroup = new TransformGroup();
            this.MouseMove += new MouseEventHandler(DiagramView_MouseMove);
            this.PreviewMouseDown += new MouseButtonEventHandler(DiagramView_PreviewMouseDown);
            this.MouseUp += new MouseButtonEventHandler(DiagramView_MouseUp);
            this.PreviewMouseWheel += new MouseWheelEventHandler(DiagramView_PreviewMouseWheel);
            this.KeyDown += new KeyEventHandler(DiagramView_KeyDown);
            //Scrollviewer.ScrollChanged += new ScrollChangedEventHandler(Scrollviewer_ScrollChanged);
            Scrollviewer.SizeChanged += new SizeChangedEventHandler(Scrollviewer_SizeChanged);
            if (d != null && this.HorizontalRuler != null)
            {
                d.SetScope(this.HorizontalRuler);
            }

            if (d != null && this.VerticalRuler != null)
            {
                d.SetScope(this.VerticalRuler);
            }
        }

        void Scrollviewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (HorizontalRuler != null)
            {
                HorizontalRuler.InvalidateArrange();
            }
            if(VerticalRuler != null)
            {
                VerticalRuler.InvalidateArrange();
            }
        }

        void rect_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        /// <summary>
        /// Provides class handling for the PreviewMouseMove routed event that occurs when
        /// the mouse pointer is over this control.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            if (this.HorizontalRuler != null)
            {
                double vrulerthickness = 0;
                if (VerticalRuler != null)
                {
                    vrulerthickness = VerticalRuler.RulerThickness - 8.5;
                }

                HorizontalRuler.MarkerPosition = e.GetPosition(this).X - vrulerthickness;

                if (HorizontalRuler.MarkerPosition > HorizontalRuler.ActualWidth)
                {
                    HorizontalRuler.MarkerPosition = HorizontalRuler.ActualWidth;
                }
            }

            ////double k = 0;
            ////if ((max < s || s == 0) && max != 0)
            ////{
            ////    k = max;
            ////}
            ////else
            ////{
            ////    k = s;
            ////}

            ////double cm = (dview.Page as DiagramPage).CurrentMinY;
            ////double sum = 0;

            //// sum = -y + (dview.Page as DiagramPage).ConstMinY - (scrollcount * k);
            if (this.VerticalRuler != null)
            {
                double hrulerthickness = 0;
                if (HorizontalRuler != null)
                {
                    hrulerthickness = VerticalRuler.RulerThickness - 4.25;
                }

                VerticalRuler.MarkerPosition = e.GetPosition(this).Y - hrulerthickness;

                if (VerticalRuler.MarkerPosition > VerticalRuler.ActualHeight)
                {
                    VerticalRuler.MarkerPosition = VerticalRuler.ActualHeight;
                }
            }

            base.OnPreviewMouseMove(e);
            e.Handled = false;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.FrameworkElement.SizeChanged"/> event, using the specified information as part of the eventual event data.
        /// </summary>
        /// <param name="sizeInfo">Details of the old and new size involved in the change.</param>
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            UpdateRuler(this);
        }
        #endregion

        #region IView Members

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// Type: <see cref="IModel"/>
        /// IModel instance.
        /// </value>
        public IModel Model
        {
            get
            {
                return mModel;
            }

            set
            {
                mModel = value;
            }
        }

        /// <summary>
        /// Gets or sets the origin.
        /// </summary>
        /// <value>
        /// Type: <see cref="System.Drawing.Point"/>
        /// The pan.
        /// </value>
        public System.Drawing.Point Origin
        {
            get
            {
                return mOrigin;
            }

            set
            {
                mOrigin = value;
            }
        }

        /// <summary>
        /// Gets or sets the bounds.
        /// </summary>
        /// <value>
        /// Type: <see cref="System.Drawing.Rectangle"/>
        /// Bounds value.
        /// </value>
        public Thickness Bounds
        {
            get { return (Thickness)GetValue(BoundsProperty); }
            set { SetValue(BoundsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Bounds.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BoundsProperty =
            DependencyProperty.Register("Bounds", typeof(Thickness), typeof(DiagramView));
        
        //public Thickness Bounds
        //{
        //    get
        //    {
        //        return mBounds;
        //    }

        //    set
        //    {
        //        mBounds = value;
        //    }
        //}

        /// <summary>
        /// Gets or sets a value indicating whether [show rulers].
        /// </summary>
        /// <value>
        /// Type: <see cref="Boolean"/>
        /// True, if it is to displayed, false otherwise.
        /// </value>
        internal bool ShowRulers
        {
            get
            {
                return showRuler;
            }

            set
            {
                showRuler = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show page].
        /// </summary>
        /// <value>
        /// Type: <see cref="Boolean"/>
        /// True, if it is to displayed, false otherwise.
        /// </value>
        public bool ShowPage
        {
            get
            {
                return showPage;
            }

            set
            {
                showPage = value;
            }
        }

        #endregion

        //internal Thickness PxBounds
        //{
        //    get { return (Thickness)GetValue(PxBoundsProperty); }
        //    set { SetValue(PxBoundsProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for PxBounds.  This enables animation, styling, binding, etc...
        //internal static readonly DependencyProperty PxBoundsProperty =
        //    DependencyProperty.Register("PxBounds", typeof(Thickness), typeof(DiagramView));

        //internal double PxGridHorizontalOffset
        //{
        //    get { return (double)GetValue(PxGridHorizontalOffsetProperty); }
        //    set { SetValue(PxGridHorizontalOffsetProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for PxGridHorizontalOffset.  This enables animation, styling, binding, etc...
        //internal static readonly DependencyProperty PxGridHorizontalOffsetProperty =
        //    DependencyProperty.Register("PxGridHorizontalOffset", typeof(double), typeof(DiagramView));
        
        //internal double PxGridVerticalOffset
        //{
        //    get { return (double)GetValue(PxGridVerticalOffsetProperty); }
        //    set { SetValue(PxGridVerticalOffsetProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for PxGridVerticalOffset.  This enables animation, styling, binding, etc...
        //internal static readonly DependencyProperty PxGridVerticalOffsetProperty =
        //    DependencyProperty.Register("PxGridVerticalOffset", typeof(double), typeof(DiagramView));

        //internal double PxSnapOffsetX
        //{
        //    get { return (double)GetValue(PxSnapOffsetXProperty); }
        //    set { SetValue(PxSnapOffsetXProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for PxSnapOffsetX.  This enables animation, styling, binding, etc...
        //internal static readonly DependencyProperty PxSnapOffsetXProperty =
        //    DependencyProperty.Register("PxSnapOffsetX", typeof(double), typeof(DiagramView));

        //internal double PxSnapOffsetY
        //{
        //    get { return (double)GetValue(PxSnapOffsetYProperty); }
        //    set { SetValue(PxSnapOffsetYProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for PxSnapOffsetY.  This enables animation, styling, binding, etc...
        //internal static readonly DependencyProperty PxSnapOffsetYProperty =
        //    DependencyProperty.Register("PxSnapOffsetY", typeof(double), typeof(DiagramView));

        internal Thickness PxBounds
        {
            get
            {
                return MeasureUnitsConverter.ToPixels(Bounds, (Page as DiagramPage).MeasurementUnits);
            }

            set
            {
                Bounds = MeasureUnitsConverter.FromPixels(value, (Page as DiagramPage).MeasurementUnits);
            }
        }

        
        internal double PxSnapOffsetX
        {
            get
            {
                return MeasureUnitsConverter.ToPixels(SnapOffsetX, (Page as DiagramPage).MeasurementUnits);
            }

            set
            {
                SnapOffsetX = MeasureUnitsConverter.FromPixels(value, (Page as DiagramPage).MeasurementUnits);
            }
        }

        internal double PxSnapOffsetY
        {
            get
            {
                return MeasureUnitsConverter.ToPixels(SnapOffsetY, (Page as DiagramPage).MeasurementUnits);
            }

            set
            {
                SnapOffsetY = MeasureUnitsConverter.FromPixels(value, (Page as DiagramPage).MeasurementUnits);
            }
        }

        #region FromPage


        /// <summary>
        /// Used to store the start point.
        /// </summary>
        private Point? startPoint = null;

        internal DateTime timeRightClick;
        internal Polyline tempPolyLine;

        /// <summary>
        /// Used to refer to the child count value.
        /// </summary>
        private bool childcount = true;

        /// <summary>
        /// Used to refer to the children count.
        /// </summary>
        /// <remarks></remarks>
        private int no;

        /// <summary>
        /// Used to refer to the name count
        /// </summary>
        private int namecount = 0;
        /// <summary>
        /// Provides class handling for the OnDrop routed event that occurs when any item
        /// is dropped on this control..
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.DragEventArgs"/> that contains the event data.</param>
        protected override void OnDrop(DragEventArgs e)
        {
            //nodedropped = true;
            CollectionExt.Cleared = false;
            if (dc.View.IsPageEditable)
            {
                base.OnDrop(e);
                ////Hor = -dc.View.Scrollviewer.HorizontalOffset * dc.View.CurrentZoom;
                ////Ver = -dc.View.Scrollviewer.VerticalOffset * dc.View.CurrentZoom;
                DragObject shape = e.Data.GetData(typeof(DragObject)) as DragObject;
                if (shape != null && !String.IsNullOrEmpty(shape.SerializedItem))
                {
                    if (shape.SerializedItem.ToString().Contains("sync_L_o"))
                    {
                        DropLine(ConnectorType.Orthogonal, e.GetPosition(Page), dc);
                    }
                    else if (shape.SerializedItem.ToString().Contains("sync_L_s"))
                    {
                        DropLine(ConnectorType.Straight, e.GetPosition(Page), dc);
                    }
                    else
                        if (shape.SerializedItem.ToString().Contains("sync_L_b"))
                        {
                            DropLine(ConnectorType.Bezier, e.GetPosition(Page), dc);
                        }
                        else
                        {
                            foreach (object tshape in dc.View.SelectionList)
                            {
                                if (tshape is ICommon)
                                {
                                    if (tshape is Node)
                                    {

                                    }
                                    else if (tshape is LineConnector)
                                    {
                                        ConnectorRoutedEventArgs newEventArgs1 = new ConnectorRoutedEventArgs(tshape as LineConnector);
                                        newEventArgs1.RoutedEvent = DiagramView.ConnectorUnSelectedEvent;
                                        RaiseEvent(newEventArgs1);
                                    }
                                }
                            }
                            Node droppedNode = null;
                            PreviewNodeDropEventRoutedEventArgs nodedropnewEventArgs = new PreviewNodeDropEventRoutedEventArgs();
                            nodedropnewEventArgs.RoutedEvent = DiagramView.PreviewNodeDropEvent;
                            RaiseEvent(nodedropnewEventArgs);
                            if (nodedropnewEventArgs.Node != null)
                            {
                                droppedNode = nodedropnewEventArgs.Node as Node;
                            }
                            else
                            {
                                droppedNode = new Node();
                            }

                            string name = string.Empty;
                            object content = System.Windows.Markup.XamlReader.Load(System.Xml.XmlReader.Create(new StringReader(shape.SerializedItem)));
                            if (content != null)
                            {
                                name = (content as UIElement).Uid;
                                if (content is System.Windows.Shapes.Path)
                                {
                                    System.Windows.Shapes.Path path = content as System.Windows.Shapes.Path;
                                    path.IsHitTestVisible = false;
                                    (droppedNode as ContentControl).Content = path;
                                }
                                else
                                {
                                    DependencyObject d = new DependencyObject();

                                    if (content is UIElement)
                                    {
                                        (content as UIElement).IsHitTestVisible = false;
                                        if (content is Viewbox)
                                        {
                                            (content as Viewbox).Stretch = Stretch.Fill;
                                            (content as Viewbox).Width = (droppedNode as ContentControl).Width;
                                            (content as Viewbox).Height = (droppedNode as ContentControl).Height;
                                        }

                                        (droppedNode as ContentControl).Content = content;
                                    }
                                    else
                                    {
                                        (droppedNode as ContentControl).Content = content;
                                    }
                                }
                            }

                            Point position = e.GetPosition(Page);
                            droppedNode.MeasurementUnits = (Page as DiagramPage).MeasurementUnits;
                            droppedNode.PxWidth = 50;
                            droppedNode.PxHeight = 50;
                            if (childcount)
                            {
                                no = Page.Children.Count + 1;
                                childcount = false;
                            }

                            namecount = no++;
                            string str = "Node" + namecount;

                            try
                            {
                                if (string.IsNullOrEmpty(droppedNode.Name))
                                {
                                    if (dc.Model.Nodes.Count == 0)
                                    {
                                        droppedNode.Name = str;
                                    }

                                    foreach (IShape n in dc.Model.Nodes)
                                    {
                                        if (!(n is LineConnector))
                                        {
                                            if (n != null && (n as Node).Name != str)
                                            {
                                                droppedNode.Name = str;
                                            }
                                            else
                                            {
                                                droppedNode.Name = "new" + str;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            catch
                            {
                            }

                            dc.Model.Nodes.Add(droppedNode);

                            Point position2 = e.GetPosition(Page);
                            this.SelectionList.Clear();
                            this.SelectionList.Add(droppedNode);
                            (droppedNode as FrameworkElement).Focus();


                            NodeDroppedRoutedEventArgs newEventArgs = new NodeDroppedRoutedEventArgs(droppedNode, name);
                            newEventArgs.RoutedEvent = DiagramView.NodeDropEvent;
                            RaiseEvent(newEventArgs);
                            dc.View.undo = true;
                            //droppedNode.MeasurementUnits = (Page as DiagramPage).MeasurementUnits;
                            droppedNode.PxOffsetX = position2.X;// Math.Max(0, position2.X);//  MeasureUnitsConverter.FromPixels(Math.Max(0, position2.X), Page.MeasurementUnits);
                            droppedNode.PxOffsetY = position2.Y;// Math.Max(0, position2.Y);//  MeasureUnitsConverter.FromPixels(Math.Max(0, position2.Y), Page.MeasurementUnits);
                            if (!this.Page.Children.Contains(droppedNode as Node))
                            {
                                (this.Page as DiagramPage).AllChildren.Add(droppedNode as Node);
                                // View.Page.Children.Add(inode as Node);
                                if (this.EnableVirtualization)
                                {
                                    this.ScrollGrid.VirtualzingNode(droppedNode as Node);
                                }
                                else
                                {
                                    (this.Page as DiagramPage).AddingChildren(droppedNode as Node);
                                }
                                Panel.SetZIndex((droppedNode as FrameworkElement), Page.Children.Count);
                                ////if (inode is Group)
                                ////{
                                ////    foreach (INodeGroup child in (inode as Group).NodeChildren)
                                ////    {
                                ////        child.Groups.Add(inode); 
                                ////    }
                                ////}
                               // SetScope(inode as Node);
                            }
                            
                            dc.View.undo = false;
                            if (dc != null)
                            {
                                foreach (Layer l in dc.Model.Layers)
                                {
                                    if (l.Active)
                                    {
                                        l.Nodes.Add(droppedNode as Node);
                                    }
                                }
                            }
                            (droppedNode as Node).Loaded+=new RoutedEventHandler(DropNode_Loaded);
                        }
                }

                //e.Handled = true;
            }
        }

        private void DropNode_Loaded(object sender, RoutedEventArgs e)
        {
            dc.View.Page.InvalidateMeasure();
            dc.View.Page.InvalidateArrange();
            (sender as Node).Loaded -= new RoutedEventHandler(DropNode_Loaded);
        }
        private void DropLine_Loaded(object sender, RoutedEventArgs e)
        {
            dc.View.Page.InvalidateMeasure();
            dc.View.Page.InvalidateArrange();
            (sender as LineConnector).Loaded -= new RoutedEventHandler(DropLine_Loaded);
        }

        /// <summary>
        /// Provides class handling for the MouseLeftButtonUp routed event that occurs when the mouse
        /// button is released while the mouse pointer is over this control.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs"/> that contains the event data. The event data reports that the left mouse button was released.</param>
        /// 

      
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            CollectionExt.Cleared = false;
            if (dc.View.IsPageEditable)
            {
                if (BrowserInteropHelper.IsBrowserHosted)
                {
                    if (SymbolPaletteItem.Hasvalue)
                    {
                        SymbolPaletteItem.Hasvalue = false;

                        Node newItem = null;
                        object content = DiagramPage.o;

                        if (content != null)
                        {
                            if (content is System.Windows.Shapes.Path && (content as System.Windows.Shapes.Path).Uid == "sync_L_o")
                            {
                                DropLine(ConnectorType.Orthogonal, e.GetPosition(Page), dc);
                            }
                            else if (content is System.Windows.Shapes.Path && (content as System.Windows.Shapes.Path).Uid == "sync_L_s")
                            {
                                DropLine(ConnectorType.Straight, e.GetPosition(Page), dc);
                            }
                            else if (content is System.Windows.Shapes.Path && (content as System.Windows.Shapes.Path).Uid == "sync_L_b")
                            {
                                DropLine(ConnectorType.Bezier, e.GetPosition(Page), dc);
                            }
                            else
                            {
                                string name = (content as UIElement).Uid;
                                newItem = new Node();
                                newItem.Name = name;

                                if (content is System.Windows.Shapes.Path)
                                {
                                    System.Windows.Shapes.Path path = content as System.Windows.Shapes.Path;
                                    path.IsHitTestVisible = false;
                                    newItem.Content = path;
                                }
                                else
                                {
                                    DependencyObject d = new DependencyObject();

                                    if (content is UIElement)
                                    {
                                        (content as UIElement).IsHitTestVisible = false;
                                        newItem.Content = content;
                                    }
                                    else
                                    {
                                        newItem.Content = content;
                                    }
                                }

                                newItem.Isnodeexe = true;
                                Point position = e.GetPosition(Page);

                                newItem.PxOffsetX = (Math.Max(0, position.X));//MeasureUnitsConverter.FromPixels(Math.Max(0, position.X), Page.MeasurementUnits);
                                newItem.PxOffsetY = (Math.Max(0, position.Y));//MeasureUnitsConverter.FromPixels(Math.Max(0, position.Y), Page.MeasurementUnits);
                                newItem.Width = 50;
                                newItem.Height = 50;

                                if (childcount)
                                {
                                    no = Page.Children.Count + 1;
                                    childcount = false;
                                }

                                namecount = no++;
                                string str = "Node" + namecount;

                                try
                                {
                                    if (string.IsNullOrEmpty(newItem.Name))
                                    {
                                        if (dc.Model.Nodes.Count == 0)
                                        {
                                            newItem.Name = str;
                                        }

                                        foreach (IShape n in dc.Model.Nodes)
                                        {
                                            if (!(n is LineConnector))
                                            {
                                                if (n != null && (n as Node).Name != str)
                                                {
                                                    newItem.Name = str;
                                                }
                                                else
                                                {
                                                    newItem.Name = "new" + str;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                }

                                ////string str = "Node" + namecount;
                                ////try
                                ////{
                                ////    if (string.IsNullOrEmpty(newItem.Name))
                                ////    {
                                ////        if (diagctrl.Model.Nodes.Count == 0)
                                ////        {
                                ////            newItem.Name = str;
                                ////        }

                                ////        foreach (Node n in diagctrl.Model.Nodes)
                                ////        {
                                ////            if (n != null && n.Name != str)
                                ////            {
                                ////                newItem.Name = str;
                                ////                break;
                                ////            }
                                ////            else
                                ////            {
                                ////                newItem.Name = "new" + str;
                                ////            }
                                ////        }

                                ////        namecount++;
                                ////    }
                                ////}
                                ////catch
                                ////{
                                ////}
                                newItem.Page = Page;
                                dc.Model.Nodes.Add(newItem);
                                Panel.SetZIndex(newItem, Page.Children.Count);
                                SelectionList.Clear();
                                SelectionList.Add(newItem);
                                newItem.Focus();
                                NodeDroppedRoutedEventArgs newEventArgs = new NodeDroppedRoutedEventArgs(newItem, name);
                                newEventArgs.RoutedEvent = DiagramView.NodeDropEvent;
                                RaiseEvent(newEventArgs);
                            }

                            e.Handled = true;
                        }
                    }
                }
                if (temppath != null)
                {
                    if (DrawingTool == DrawingTools.Ellipse)
                    {

                        draw_ell = new System.Windows.Shapes.Path();
                        draw_ell.Data = new EllipseGeometry();
                        draw_ell.Data = temppath.Data as EllipseGeometry;
                        draw_ell.Stretch = Stretch.Fill;
                        if ((draw_ell.Data as EllipseGeometry).RadiusX != 0 && (draw_ell.Data as EllipseGeometry).RadiusY != 0)
                        {
                            Node n1 = new Node();
                            n1.OffsetX = (draw_ell.Data as EllipseGeometry).Bounds.Location.X;
                            n1.OffsetY = (draw_ell.Data as EllipseGeometry).Bounds.Location.Y;
                            n1.Width = ((draw_ell.Data as EllipseGeometry).RadiusX) * 2;
                            n1.Height = ((draw_ell.Data as EllipseGeometry).RadiusY) * 2;
                            n1.Loaded += new RoutedEventHandler(n1_Loaded);
                            n1.m_NodeDrawing = true;
                            dc.Model.Nodes.Add(n1);
                            SelectionList.Add(n1);
                            draw_ell.Focus();
                        }
                        Drawtools_Remove(temppath);
                        SelectionList.Clear();
                        temppath = null;
                        e.Handled = true;
                        //EnableDrawingTools = false;
                        timeRightClick = DateTime.Now;
                    }
                    else if (DrawingTool == DrawingTools.Rectangle)
                    {

                        draw_ell = new System.Windows.Shapes.Path();
                        draw_ell.Data = new RectangleGeometry();
                        draw_ell.Data = temppath.Data as RectangleGeometry;
                        draw_ell.Stretch = Stretch.Fill;
                        if ((temppath.Data as RectangleGeometry).Rect.Width != 0 && (temppath.Data as RectangleGeometry).Rect.Height != 0)
                        {
                            Node n1 = new Node();
                            n1.Loaded += new RoutedEventHandler(n1_Loaded);
                            n1.OffsetX = (temppath.Data as RectangleGeometry).Bounds.Location.X;
                            n1.OffsetY = (temppath.Data as RectangleGeometry).Bounds.Location.Y;
                            n1.Width = (temppath.Data as RectangleGeometry).Rect.Width;
                            n1.Height = (temppath.Data as RectangleGeometry).Rect.Height;
                            n1.m_NodeDrawing = true;
                            dc.Model.Nodes.Add(n1);
                            SelectionList.Add(n1);
                            draw_ell.Focus();
                        }
                        Drawtools_Remove(temppath);
                        SelectionList.Clear();
                        temppath = null;
                        e.Handled = true;
                        //EnableDrawingTools = false;
                        timeRightClick = DateTime.Now;

                    }
                    else if (DrawingTool == DrawingTools.RoundedRectangle)
                    {

                        draw_ell = new System.Windows.Shapes.Path();
                        draw_ell.Data = new RectangleGeometry();
                        draw_ell.Data = temppath.Data as RectangleGeometry;
                        draw_ell.Stretch = Stretch.Fill;
                        if ((temppath.Data as RectangleGeometry).Rect.Width != 0 && (temppath.Data as RectangleGeometry).Rect.Height != 0)
                        {
                            Node n1 = new Node();
                            n1.Loaded += new RoutedEventHandler(n1_Loaded);
                            n1.OffsetX = (temppath.Data as RectangleGeometry).Bounds.Location.X;
                            n1.OffsetY = (temppath.Data as RectangleGeometry).Bounds.Location.Y;
                            n1.Width = (temppath.Data as RectangleGeometry).Rect.Width;
                            n1.Height = (temppath.Data as RectangleGeometry).Rect.Height;
                            n1.m_NodeDrawing = true;
                            dc.Model.Nodes.Add(n1);
                            SelectionList.Add(n1);
                            draw_ell.Focus();
                        }
                        Drawtools_Remove(temppath);
                        temppath = null;
                        e.Handled = true;
                        SelectionList.Clear();
                        //EnableDrawingTools = false;
                        timeRightClick = DateTime.Now;

                    }
                    else if (DrawingTool == DrawingTools.StraightLine)
                    {
                        if ((temppath.Data as LineGeometry).StartPoint != (temppath.Data as LineGeometry).EndPoint)
                        {
                            ConnectorBase line = null;
                            line = new LineConnector();
                            line.ConnectorType = ConnectorType.Straight;
                            line.m_LineDrawing = true;
                            line.PxStartPointPosition = (temppath.Data as LineGeometry).StartPoint;
                            line.PxEndPointPosition = (temppath.Data as LineGeometry).EndPoint;
                            line.UpdateConnectorPathGeometry();
                            dc.Model.Connections.Add(line);
                            SelectionList.Add(line);
                            line.Focus();
                        }
                        Drawtools_Remove(temppath);
                        temppath = null;
                        ////update selection
                        SelectionList.Clear();
                        e.Handled = true;
                        EnableDrawingTools = false;
                        timeRightClick = DateTime.Now;
                    }
                    else if (DrawingTool == DrawingTools.BezierLine)
                    {

                        ConnectorBase line = null;
                        line = new LineConnector();
                        line.ConnectorType = ConnectorType.Bezier;
                        line.m_LineDrawing = true;
                        line.PxStartPointPosition = pf.StartPoint;
                        line.PxEndPointPosition = point3;
                        (temppath.Data as PathGeometry).Figures.Add(pf);
                        Drawtools_Remove(temppath);
                        temppath = null;
                        line.UpdateConnectorPathGeometry();
                        dc.Model.Connections.Add(line);
                        SelectionList.Clear();
                        SelectionList.Add(line);
                        e.Handled = true;
                        line.Focus();
                        EnableDrawingTools = false;
                        timeRightClick = DateTime.Now;
                    }

                    else if (DrawingTool == DrawingTools.OrthogonalLine)
                    {

                        ConnectorBase line = null;
                        line = new LineConnector();
                        line.ConnectorType = ConnectorType.Orthogonal;
                        line.m_LineDrawing = true;
                        line.PxStartPointPosition = pf.StartPoint;
                        line.PxEndPointPosition = temppoly.Points[temppoly.Points.Count - 1];
                        Drawtools_Remove(temppath);
                        temppath = null;
                        line.UpdateConnectorPathGeometry();
                        dc.Model.Connections.Add(line);
                        SelectionList.Clear();
                        SelectionList.Add(line);
                        e.Handled = true;
                        line.Focus();
                        EnableDrawingTools = false;
                        timeRightClick = DateTime.Now;
                    }
                }
            }
        }

        //Function for Path to Node Conversion
        void n1_Loaded(object sender, RoutedEventArgs e)
        {
            Node n1 = sender as Node;
            n1.Shape = Shapes.CustomPath;
            //p = n1.Template.FindName("PART_Shape", n1) as System.Windows.Shapes.Path;
            //p.Data = draw_ell.GetValue(System.Windows.Shapes.Path.DataProperty) as Geometry;

            Style CPS = new Style();
            CPS.BasedOn = n1.CustomPathStyle;
            CPS.TargetType = typeof(System.Windows.Shapes.Path);
            CPS.Setters.Add(new Setter(System.Windows.Shapes.Path.DataProperty, draw_ell.GetValue(System.Windows.Shapes.Path.DataProperty) as Geometry));
            n1.CustomPathStyle = CPS;

        }

        //Remove the Path from Page
        
        private void Drawtools_Remove(System.Windows.Shapes.Path temppath1)
        {
            (Page as DiagramPage).AllChildren.Remove(temppath1);
            (Page as DiagramPage).Children.Remove(temppath1);
        }

        internal void VerifyVirtualization()
        {
            if (this.EnableVirtualization)
            {
                this.ScrollGrid.callCalculate();
            }
            else
            {
                this.ScrollGrid.CallNormailzation();
            }
        }

        protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseRightButtonDown(e);

            if ((Page as DiagramPage).IsPolyLineEnabled == true)
            {

                if (tempPolyLine != null)
                {
                    ConnectorBase line = null;
                    line = new LineConnector();
                    line.ConnectorType = ConnectorType.Straight;
                    line.PxStartPointPosition = tempPolyLine.Points[0];
                    line.IntermediatePoints = new List<Point>();
                    for (int i = 0; i < tempPolyLine.Points.Count; i++)
                    {
                        if (i != 0 && i != tempPolyLine.Points.Count - 1)
                            line.IntermediatePoints.Add(tempPolyLine.Points[i]);
                    }
                    line.PxEndPointPosition = tempPolyLine.Points[tempPolyLine.Points.Count - 1];
                    (Page as DiagramPage).AllChildren.Remove(tempPolyLine);
                    (Page as DiagramPage).Children.Remove(tempPolyLine);
                    tempPolyLine = null;
                    line.UpdateConnectorPathGeometry();
                    dc.Model.Connections.Add(line);
                    ////update selection
                    SelectionList.Clear();
                    SelectionList.Add(line);
                    e.Handled = true;
                    line.Focus();
                    (Page as DiagramPage).IsPolyLineEnabled = false;
                    timeRightClick = DateTime.Now;
                }
            }

            if (DrawingTool == DrawingTools.PolyLine)
            {
                if (tempPolyLine != null)
                {
                    ConnectorBase line = null;
                    line = new LineConnector();
                    line.ConnectorType = ConnectorType.Straight;
                    line.m_LineDrawing = true;
                    line.PxStartPointPosition = tempPolyLine.Points[0];
                    line.IntermediatePoints = new List<Point>();
                    for (int i = 0; i < tempPolyLine.Points.Count; i++)
                    {
                        if (i != 0 && i != tempPolyLine.Points.Count - 1)
                            line.IntermediatePoints.Add(tempPolyLine.Points[i]);
                    }
                    line.PxEndPointPosition = tempPolyLine.Points[tempPolyLine.Points.Count - 1];
                    (Page as DiagramPage).AllChildren.Remove(tempPolyLine);
                    (Page as DiagramPage).Children.Remove (tempPolyLine);
                                       tempPolyLine = null;
                    line.UpdateConnectorPathGeometry();
                    dc.Model.Connections.Add(line);
                    ////update selection
                    SelectionList.Clear();
                    SelectionList.Add(line);
                    e.Handled = true;
                    line.Focus();
                    EnableDrawingTools = false;
                    timeRightClick = DateTime.Now;
                }
            }

            if (temppath != null)
            {
                if (DrawingTool == DrawingTools.Polygon)
                {
                    draw_ell = new System.Windows.Shapes.Path();
                    draw_ell.Data = new PathGeometry();
                    draw_ell.Data = (temppath.Data as PathGeometry);
                    draw_ell.Stretch = Stretch.Fill;
                    Node n2 = new Node();
                    n2.Loaded += new RoutedEventHandler(n2_Loaded);
                    n2.OffsetX = (temppath.Data as PathGeometry).Bounds.Location.X;
                    n2.OffsetY = (temppath.Data as PathGeometry).Bounds.Location.Y;
                    n2.Width = (temppath.Data as PathGeometry).Bounds.Width;
                    n2.Height = (temppath.Data as PathGeometry).Bounds.Height;
                    Drawtools_Remove(temppath);
                    temppath = null;
                    n2.m_NodeDrawing = true;
                    dc.Model.Nodes.Add(n2);
                    SelectionList.Clear();
                    SelectionList.Add(n2);
                    e.Handled = true;
                    draw_ell.Focus();
                    //EnableDrawingTools = false;
                    timeRightClick = DateTime.Now;

                }

            }
        }


        void n2_Loaded(object sender, RoutedEventArgs e)
        {
            Node n2 = sender as Node;
            n2.Shape = Shapes.CustomPath;
            //p = n2.Template.FindName("PART_Shape", n2) as System.Windows.Shapes.Path;
            //p.Data = Geometry.Parse((draw_ell.GetValue(System.Windows.Shapes.Path.DataProperty) as Geometry).ToString());
            Style CPS = new Style();
            CPS.BasedOn = n2.CustomPathStyle;
            CPS.TargetType = typeof(System.Windows.Shapes.Path);
            CPS.Setters.Add(new Setter(System.Windows.Shapes.Path.DataProperty, Geometry.Parse((draw_ell.GetValue(System.Windows.Shapes.Path.DataProperty) as Geometry).ToString())));
            n2.CustomPathStyle = CPS;

        }
        private bool WithinPageAndView(object p)
        {
            //if (Scrollviewer.Equals(p))
            if(p is ScrollableGrid)
            {
                return true;
            }
            else if (p is DiagramPage || p is Adorner)
            {
                return false;
            }
            else if (!(p is DependencyObject))
            {
                return false;
            }
            else
            {
                return WithinPageAndView(VisualTreeHelper.GetParent(p as DependencyObject));
            }
        }

        /// <summary>
        /// Provides class handling for the MouseDown routed event that occurs when the mouse
        /// button is pressed while the mouse pointer is over this control.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs"/> that contains the event data. This event data reports details about the mouse button that was pressed and the handled state.</param>
        /// 


        void DiagramView_MouseDown(object sender, MouseButtonEventArgs e)
        //protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            //foreach (UIElement element in Page.Children)
            //{
            //    if (element is Node)
            //    {
            //        (element as Node).CompleteEditing();
            //    }

            //    if (element is LineConnector)
            //    {
            //        if (e.Source != (element as LineConnector))
            //        {
            //            (element as LineConnector).CompleteConnEditing();
            //        }
            //    }
            //}
            dc = DiagramPage.GetDiagramControl(this);
            if (dc.View.IsPageEditable)
            {

                //base.OnMouseDown(e);
                if ((Page as DiagramPage).IsPolyLineEnabled == true)
                {
                    if (tempPolyLine == null)
                    {
                        tempPolyLine = new Polyline();
                        tempPolyLine.Points.Add(Mouse.GetPosition(Page));
                    }
                    tempPolyLine.Points.Add(Mouse.GetPosition(Page));
                    tempPolyLine.Stroke = new SolidColorBrush(Colors.Black);
                    tempPolyLine.StrokeThickness = 1;
                    if (Page.Children.IndexOf(tempPolyLine) < 0)
                        (Page as DiagramPage).Children.Add(tempPolyLine);
                    (Page as DiagramPage).AllChildren.Add(tempPolyLine);

                }

                    if (EnableDrawingTools==true)
                    {
                       
                        EnableConnection = false;
                        oldx = e.GetPosition(this.Page as DiagramPage).X;
                        oldy = e.GetPosition(this.Page as DiagramPage).Y;

                        // shapes
                        if (DrawingTool == DrawingTools.Ellipse)
                        {
                          
                            if (temppath == null)
                            {
                                temppath = new System.Windows.Shapes.Path
                                {
                                    Style = (Page as DiagramPage).CustomPathStyle,

                                    Data = new EllipseGeometry(new Point(oldx, oldy), 0, 0),
                                    Stretch = Stretch.None

                                };

                            }
                            Dimensions[0] = 0;
                            Dimensions[1] = 0;
                            temppath.Tag = Dimensions;

                            if (Page.Children.IndexOf(temppath) < 0)
                                Drawtools_Add(temppath);
                            
                        }

                        else if (DrawingTool == DrawingTools.Rectangle)
                        {
                           
                            if (temppath == null)
                            {
                                temppath = new System.Windows.Shapes.Path();
                                temppath.Style = (Page as DiagramPage).CustomPathStyle;
                                temppath.Stretch = Stretch.None;
                                RectangleGeometry rg = new RectangleGeometry();
                                rt.X = oldx;
                                rt.Y = oldy;
                                rt.Height = 0;
                                rt.Width = 0;
                                rg.Rect = rt;
                                temppath.Data = rg;
                            }
                            Dimensions[0] = 0;
                            Dimensions[0] = 0;
                            temppath.Tag = Dimensions;
                            if (Page.Children.IndexOf(temppath) < 0)
                                Drawtools_Add(temppath);
                        }
                        else if (DrawingTool == DrawingTools.RoundedRectangle)
                        {
                           
                            if (temppath == null)
                            {
                                temppath = new System.Windows.Shapes.Path();
                                temppath.Style = (Page as DiagramPage).CustomPathStyle;
                                temppath.Stretch = Stretch.None;
                                RectangleGeometry rgt = new RectangleGeometry();
                                rt.X = oldx;
                                rt.Y = oldy;
                                rt.Height = 0;
                                rt.Width = 0;
                                rgt.RadiusX = 0;
                                rgt.RadiusY = 0;
                                rgt.Rect = rt;
                                temppath.Data = rgt;
                            }

                            Dimensions[0] = 0;
                            Dimensions[0] = 0;
                            temppath.Tag = Dimensions;
                            if (Page.Children.IndexOf(temppath) < 0)

                                Drawtools_Add(temppath);
                        }
                        else if (DrawingTool == DrawingTools.Polygon)
                        {
                           
                            if (temppath == null)
                            {
                                temppath = new System.Windows.Shapes.Path();
                                tempbezier = new PathGeometry();
                                pf = new PathFigure();
                                pf.StartPoint = new Point(oldx, oldy);
                                temppoly = new PolyLineSegment();
                                PathSegmentCollection myPathSegmentCollection = new PathSegmentCollection();
                                pf.Segments = myPathSegmentCollection;
                                pf.Segments.Add(temppoly);
                                tempbezier.Figures.Add(pf);
                                temppath.Data = tempbezier;
                                temppath.Style = (Page as DiagramPage).CustomPathStyle;
                                temppath.Stretch = Stretch.None;
                            }
                            temppoly.Points.Add(Mouse.GetPosition(Page));
                            pf.IsClosed = true;
                            pf.IsFilled = true;
                            if (Page.Children.IndexOf(temppath) < 0)
                                Drawtools_Add(temppath);
                            
                        }

                       //connectors
                        else if (DrawingTool == DrawingTools.StraightLine)
                        {
                           
                            if (temppath == null)
                            {
                                temppath = new System.Windows.Shapes.Path
                                {

                                    Stroke = Brushes.Black,
                                    Data = new LineGeometry(new Point(oldx, oldy), new Point(oldx, oldy))
                                };
                               

                            }
                            if (Page.Children.IndexOf(temppath) < 0)
                                Drawtools_Add(temppath);
                        }

                        else if (DrawingTool == DrawingTools.BezierLine)
                        {
                           
                            if (temppath == null)
                            {
                                temppath = new System.Windows.Shapes.Path();
                                tempbezier = new PathGeometry();
                                pf = new PathFigure();
                                pf.StartPoint = new Point(oldx, oldy);
                                bs = new BezierSegment();
                                bs.Point1 = new Point(oldx, oldy);
                                bs.Point2 = new Point(oldx, oldy);
                                bs.Point3 = new Point(oldx, oldy);
                                point3 = bs.Point3;
                                pf.Segments.Add(bs);
                                tempbezier.Figures.Add(pf);
                                temppath.Stroke = Brushes.Black;
                                temppath.StrokeThickness = 1;
                                temppath.Data = tempbezier;
                            }
                            if (Page.Children.IndexOf(temppath) < 0)
                                Drawtools_Add(temppath);
                        }
                        else if (DrawingTool == DrawingTools.PolyLine)
                        {
                            if (tempPolyLine == null)
                            {
                                tempPolyLine = new Polyline();
                                tempPolyLine.Points.Add(Mouse.GetPosition(Page));
                            }
                            tempPolyLine.Points.Add(Mouse.GetPosition(Page));
                            tempPolyLine.Stroke = new SolidColorBrush(Colors.Black);
                            tempPolyLine.StrokeThickness = 1;
                            if (Page.Children.IndexOf(tempPolyLine) < 0)

                                (Page as DiagramPage).Children.Add (tempPolyLine);
                                (Page as DiagramPage).AllChildren.Add(tempPolyLine);
                        }
                       

                        else if (DrawingTool == DrawingTools.OrthogonalLine)
                        {
                           
                            if (temppath == null)
                            {
                                temppath = new System.Windows.Shapes.Path();
                                tempbezier = new PathGeometry();
                                pf = new PathFigure();
                                pf.StartPoint = new Point(oldx, oldy);
                                temppoly = new PolyLineSegment();
                                pf.Segments.Add(temppoly);
                                tempbezier.Figures.Add(pf);
                                temppath.Data = tempbezier;
                                temppath.Stroke = Brushes.Black;

                            }
                            if (temppoly.Points.Count <= 2)
                            {
                                temppoly.Points.Add(ortho1);
                                temppoly.Points.Add(ortho2);
                                temppoly.Points.Add(ortho3);
                            }
                            if (Page.Children.IndexOf(temppath) < 0)
                                Drawtools_Add(temppath);
                               
                       }

                    }
                //else if (e.Source == this)
                if (e.OriginalSource is DiagramPage || WithinPageAndView(e.OriginalSource))
                {
                    this.startPoint = new Point?(e.GetPosition(this));
                    SelectionList.Clear();
                    //Focus();
                    e.Handled = true;
                }
                else if (Scrollviewer.Equals(e.OriginalSource))
                {
                    this.startPoint = new Point?(e.GetPosition(this));
                    SelectionList.Clear();
                }
            }
        }


        private void Drawtools_Add(System.Windows.Shapes.Path temppath1)
        {
            if (Page.Children.IndexOf(temppath1) < 0)

                (Page as DiagramPage).Children.Add (temppath);
                (Page as DiagramPage).AllChildren.Add(temppath1);
          
        }

        //protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        //{
        //}

        /// <summary>
        /// Provides class handling for the MouseMove routed event that occurs when the mouse
        /// pointer  is over this control.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (IsPageEditable)
            {
                base.OnMouseMove(e);

                if (BrowserInteropHelper.IsBrowserHosted)
                {
                    if (SymbolPaletteItem.Hasvalue)
                    {
                        this.Cursor = Cursors.IBeam;
                    }
                    else
                    {
                        this.Cursor = Cursors.Arrow;
                    }
                }

                if (e.LeftButton != MouseButtonState.Pressed)
                {
                    this.startPoint = null;
                }

                if (this.startPoint.HasValue)
                {
                    if (EnableDrawingTools == false)
                    {
                        AdornerLayer adorner = AdornerLayer.GetAdornerLayer(this);
                        if (adorner != null)
                        {
                            NodeSelectionAdorner nodeadorner = new NodeSelectionAdorner(this, startPoint);
                            if (adorner != null)
                            {
                                adorner.Add(nodeadorner);
                            }
                        }
                    }
                }

                if ((Page as DiagramPage).IsPolyLineEnabled == true)
                {
                    if (tempPolyLine != null)
                    {
                        tempPolyLine.Points[tempPolyLine.Points.Count - 1] = Mouse.GetPosition(Page);
                    }
                }
                       
                
                    if (EnableDrawingTools == true)
                    {
                        if (DrawingTool == DrawingTools.Polygon)
                        {
                           
                            double newx = Mouse.GetPosition(Page).X;
                            double newy = Mouse.GetPosition(Page).Y;
                            if (temppath != null)
                            {
                                temppath.Cursor = Cursors.Cross;
                                temppath.CaptureMouse();
                                pf.IsClosed = true;
                                pf.IsFilled = true;
                                temppoly.Points[temppoly.Points.Count - 1] = e.GetPosition(Page);
                            }
                        }

                        else if (DrawingTool == DrawingTools.BezierLine)
                        {

                            if (temppath != null)
                            {
                                temppath.Cursor = Cursors.Cross;
                                temppath.CaptureMouse();
                                double newx = Mouse.GetPosition(this.Page as DiagramPage).X;
                                double newy = Mouse.GetPosition(this.Page as DiagramPage).Y;
                                double diffx = Math.Abs(newx - oldx);
                                double x = new double();
                                double y = new double();
                                double dis = new double();
                                //double x1 = new double();
                                //double y1 = new double();
                                //double dis1 = new double();
                                double newnewx = new double();

                                Point b1 = new Point();
                                Point b2 = new Point();
                                Point b3 = new Point();
                               
                                 
                                if (oldx > 0)
                                {
                                    x = Math.Pow((oldx-newx), 2);
                                    y = Math.Pow((oldy - newy), 2);
                                    dis = Math.Sqrt((x + y) / 4);
                                    //y1 = Math.Sqrt(y);
                                    //x1 = Math.Sqrt(x);
                                    //dis1 = Math.Sqrt(dis * 6);

                                    if (newx <= oldx)
                                    {
                                        bs.Point1 = new Point(Math.Abs(oldx - dis), oldy);
                                        bs.Point2 = new Point(Math.Abs(newx + dis), newy);
                                        bs.Point3 = new Point(newx, newy);
                                        point3 = bs.Point3;
                                    }
                                    else
                                    {
                                        bs.Point1 = new Point(Math.Abs(oldx - dis), oldy);
                                        bs.Point2 = new Point(Math.Abs(newx + dis), newy);
                                        bs.Point3 = new Point(newx, newy);
                                        point3 = bs.Point3;
                                    }
                                    if (newx <= 0)
                                    {
                                        //newnewx = -newx;
                                        b1 = new Point((Math.Abs(oldx - dis)), oldy);
                                        b2 = new Point((Math.Abs(newx + dis)), newy);
                                        b3 = new Point(newx, newy);
                                        bs.Point1 = new Point((b1.X), oldy);
                                        bs.Point2 = new Point(-(b2.X), newy);
                                        bs.Point3 = new Point(newx, newy);
                                        point3 = bs.Point3;
                                    }
                                }

                                else if (oldx < 0)
                                {
                                    x = Math.Pow((oldx - newx), 2);
                                    y = Math.Pow((oldy - newy), 2);
                                    dis = Math.Sqrt((x + y) / 4);
                                    //y1 = Math.Sqrt(y);
                                    //x1 = Math.Sqrt(x);
                                    //dis1 = Math.Sqrt(dis * 6);
                                   
                                    b1 = new Point(Math.Abs((-oldx) + dis), oldy);
                                    b2 = new Point(Math.Abs((-newx) - dis), newy);
                                    b3 = new Point(newx, newy);
                                    if (newx >= oldx)
                                    {
                                        b1 = new Point(Math.Abs(oldx - dis), oldy);
                                        b2 = new Point(Math.Abs(newx + dis), newy);
                                        b3 = new Point(newx, newy);
                                        bs.Point1 = new Point(-(b1.X), oldy);
                                        bs.Point2 = new Point(-(b2.X), newy);
                                        bs.Point3 = new Point(newx, newy);
                                        point3 = bs.Point3;
                                    }
                                    else
                                    {
                                        b1 = new Point(Math.Abs(oldx - dis), oldy);
                                        b2 = new Point(Math.Abs(newx + dis), newy);
                                        b3 = new Point(newx, newy);
                                        bs.Point1 = new Point(-(b1.X), oldy);
                                        bs.Point2 = new Point(-(b2.X), newy);
                                        bs.Point3 = new Point(newx, newy);
                                        point3 = bs.Point3;

                                    }
                                    if (newx >= 0)
                                    {

                                        if (newx > 0)
                                        {
                                            b1 = new Point(Math.Abs((oldx) - dis), oldy);
                                            b2 = new Point(Math.Abs((newx) + dis), newy);
                                            b3 = new Point(newx, newy);
                                            bs.Point1 = new Point(-(b1.X), oldy);
                                            bs.Point2 = new Point((b2.X), newy);
                                            bs.Point3 = new Point(newx, newy);
                                            point3 = bs.Point3;
                                        }

                                    }
                                  
                                }


                            }

                        }
                        else if (DrawingTool == DrawingTools.PolyLine)
                        {
                            if (tempPolyLine != null)
                            {
                                tempPolyLine.Points[tempPolyLine.Points.Count - 1] = Mouse.GetPosition(Page);
                            }
                        }

                       
                        else if (DrawingTool == DrawingTools.OrthogonalLine)
                        {
                            ortho1 = e.GetPosition(Page);
                            ortho2 = e.GetPosition(Page);
                            ortho3 = e.GetPosition(Page);


                            if (temppath != null)
                            {

                                temppath.Cursor = Cursors.Cross;
                                temppath.CaptureMouse();
                                double newx = Mouse.GetPosition(this.Page as DiagramPage).X;
                                double newy = Mouse.GetPosition(this.Page as DiagramPage).Y;
                                double newy1 = new double();
                                double newx1 = new double();
                                if (temppoly.Points.Count <= 3)
                                {
                                   
                                    if ((oldx >= 0))
                                    {
                                        double diffy1 = 25;
                                        if (newy > oldy)
                                        {
                                            newy1 = diffy1 + oldy;
                                        }
                                        else
                                        {
                                            newy1 = Math.Abs(diffy1 - oldy);
                                        }

                                        ortho1 = new Point(oldx, newy1);
                                        temppoly.Points[0] = ortho1;
                                        double diffx1 = (( newx-oldx));
                                        if (newx > oldx)
                                        {
                                            newx1 = diffx1 + oldx;
                                        }
                                        else
                                        {
                                            if (diffx1 < 0)
                                            {
                                                newx1 = (diffx1 + oldx);
                                            }
                                            else
                                            {
                                                newx1 = (diffx1 + oldx);
                                            }
                                        }
                                        ortho2 = new Point(newx1, newy1);
                                        temppoly.Points[1] = ortho2;
                                        ortho3 = new Point(newx1, newy);
                                        temppoly.Points[2] = ortho3;
                                    }
                                    else
                                    {
                                        double diffy1 = 25;
                                        if (newy > oldy)
                                        {
                                            newy1 = diffy1 + oldy;
                                        }
                                        else
                                        {
                                            newy1 = Math.Abs(oldy-diffy1);
                                        }

                                        ortho1 = new Point(oldx, newy1);
                                        temppoly.Points[0] = ortho1;
                                            if (newx > oldx)
                                            {
                                                double diffx1 = ((newx - oldx));
                                                if (diffx1 < 0)
                                                {
                                                    newx1 = diffx1 - oldx;
                                                }
                                                else 
                                                {
                                                    newx1 = diffx1 + oldx;
                                                }

                                            }
                                            else
                                            {
                                                double diffx1 = ((oldx + newx));
                                                if (diffx1 < 0)
                                                {
                                                    newx1 = diffx1 - oldx;
                                                }
                                                else
                                                {
                                                    newx1 = diffx1 + oldx;
                                                }

                                            }
                                        ortho2 = new Point(newx1, newy1);
                                        temppoly.Points[1] = ortho2;
                                        ortho3 = new Point(newx1, newy);
                                        temppoly.Points[2] = ortho3;
                                    }

                                }

                            }
                        }

                    }

                if (EnableDrawingTools && Mouse.LeftButton == MouseButtonState.Pressed)
                {

                    if (DrawingTool == DrawingTools.Ellipse)
                    {

                        if (temppath != null)
                        {
                            temppath.Cursor = Cursors.Cross;
                            double newx = Mouse.GetPosition(this.Page as DiagramPage).X;
                            double newy = Mouse.GetPosition(this.Page as DiagramPage).Y;
                            (temppath.Data as EllipseGeometry).Center = new Point(((newx + oldx) / 2), ((newy + oldy) / 2));
                            (temppath.Data as EllipseGeometry).RadiusX = Math.Abs((newx - oldx) / 2);
                            (temppath.Data as EllipseGeometry).RadiusY = Math.Abs((newy - oldy) / 2);
                            temppath.CaptureMouse();
                            double[] Dimensions = new double[2] { (temppath.Data as EllipseGeometry).RadiusX, (temppath.Data as EllipseGeometry).RadiusY };
                        }
                    }
                    else if (DrawingTool == DrawingTools.Rectangle)
                    {
                        if (temppath != null)
                        {
                            temppath.Cursor = Cursors.Cross;
                            double newx = Mouse.GetPosition(this.Page as DiagramPage).X;
                            double newy = Mouse.GetPosition(this.Page as DiagramPage).Y;
                            Point startpoint = new Point(oldx, oldy);
                            temppath.CaptureMouse();
                            Point startpoint_rect = new Point(oldx, oldy);
                            if ((e.GetPosition(Page).Y < oldy))
                            {
                                startpoint_rect.X = oldx;
                                startpoint_rect.Y = e.GetPosition(Page).Y;
                            }
                            if (e.GetPosition(Page).X < oldx)
                            {
                                startpoint_rect.Y = oldy;
                                startpoint_rect.X = e.GetPosition(Page).X;
                            }
                            if ((e.GetPosition(Page).X < oldx) && (e.GetPosition(Page).Y < oldy))
                            {
                                startpoint_rect.X = e.GetPosition(Page).X;
                                startpoint_rect.Y = e.GetPosition(Page).Y;
                            }

                            (temppath.Data as RectangleGeometry).Rect = new Rect(new Point(startpoint_rect.X, startpoint_rect.Y), new Size(Math.Abs(oldx - newx), Math.Abs(oldy - newy)));
                            double[] Dimensions = new double[2] { (temppath.Data as RectangleGeometry).RadiusX, (temppath.Data as RectangleGeometry).RadiusY };
                        }
                    }

                    else if (DrawingTool == DrawingTools.RoundedRectangle)
                    {
                        double newx = Mouse.GetPosition(Page).X;
                        double newy = Mouse.GetPosition(Page).Y;
                        if (temppath != null)
                        {
                            temppath.Cursor = Cursors.Cross;
                            temppath.CaptureMouse();
                            Point startpoint_rect = new Point(oldx, oldy);
                            if ((e.GetPosition(Page).Y < oldy))
                            {
                                startpoint_rect.X = oldx;
                                startpoint_rect.Y = e.GetPosition(Page).Y;
                            }
                            if (e.GetPosition(Page).X < oldx)
                            {
                                startpoint_rect.Y = oldy;
                                startpoint_rect.X = e.GetPosition(Page).X;
                            }
                            if ((e.GetPosition(Page).X < oldx) && (e.GetPosition(Page).Y < oldy))
                            {
                                startpoint_rect.X = e.GetPosition(Page).X;
                                startpoint_rect.Y = e.GetPosition(Page).Y;
                            }
                            (temppath.Data as RectangleGeometry).RadiusX = Math.Abs(newx - oldx) / 16;
                            (temppath.Data as RectangleGeometry).RadiusY = Math.Abs(newy - oldy) / 16;
                            (temppath.Data as RectangleGeometry).Rect = new Rect(new Point(startpoint_rect.X, startpoint_rect.Y), new Size(Math.Abs(newx - oldx), Math.Abs(newy - oldy)));
                        }

                    }

                    else if (DrawingTool == DrawingTools.StraightLine)
                    {
                        if (temppath != null)
                        {
                            temppath.Cursor = Cursors.Cross;
                            double newx = Mouse.GetPosition(this.Page as DiagramPage).X;
                            double newy = Mouse.GetPosition(this.Page as DiagramPage).Y;
                            temppath.CaptureMouse();
                            (temppath.Data as LineGeometry).StartPoint = new Point(oldx, oldy);
                            (temppath.Data as LineGeometry).EndPoint = new Point(newx, newy);
                        }

                    }
                  
                }
                //e.Handled = true;
            }
        }


        

       

        /// <summary>
        /// Drops the line.
        /// </summary>
        /// <param name="connectortype">The connector type.</param>
        /// <param name="position">The position.</param>
        /// <param name="diagctrl">The diagram control object.</param>
        private void DropLine(ConnectorType connectortype, Point position, DiagramControl diagctrl)
        {
            PreviewConnectorDropEventRoutedEventArgs linedropnewEventArgs = new PreviewConnectorDropEventRoutedEventArgs();
            linedropnewEventArgs.RoutedEvent = DiagramView.PreviewConnectorDropEvent;
            RaiseEvent(linedropnewEventArgs);
            ConnectorBase line = null;
            //IsConnectorDropped = true;
            if (linedropnewEventArgs.Connector != null)
            {
                line = linedropnewEventArgs.Connector as ConnectorBase;
            }
            else
            {
                line = new LineConnector();
            }

            line.ConnectorType = connectortype;
            line.DropPoint = position;
            line.MeasurementUnit = (Page as DiagramPage).MeasurementUnits;
            line.PxStartPointPosition = new Point(line.DropPoint.X - 25, line.DropPoint.Y - 25);
            line.PxEndPointPosition = new Point(line.DropPoint.X + 25, line.DropPoint.Y + 25);
            if (!this.Page.Children.Contains(line as Control))
            {
                
                (this.Page as DiagramPage).AllChildren.Add(line as Control);
                if (this.EnableVirtualization)
                {
                    this.ScrollGrid.VirtualizingLine(line as LineConnector);
                }
                else
                {
                    (this.Page as DiagramPage).AddingChildren(line as LineConnector);
                }
                Panel.SetZIndex(line as Control, this.Page.Children.Count);
                //(View.Page as DiagramPage).AddingChildren(iconn as LineConnector);
                //View.Page.Children.Add(iconn as Control);
              //  SetScope(iconn as Control);
            }
            line.UpdateConnectorPathGeometry();
            diagctrl.Model.Connections.Add(line);
            line.Loaded += new RoutedEventHandler(DropLine_Loaded);
            ////update selection
            this.SelectionList.Clear();
            this.SelectionList.Add(line);
            line.Focus();
            ConnectorDroppedRoutedEventArgs newEventArgs = new ConnectorDroppedRoutedEventArgs(line as ConnectorBase);
            newEventArgs.RoutedEvent = DiagramView.ConnectorDropEvent;
            RaiseEvent(newEventArgs);
            if (dc != null)
            {
                foreach (Layer l in dc.Model.Layers)
                {
                    if (l.Active)
                    {
                        l.Lines.Add(line as LineConnector);
                    }
                }
            }

            
        }

        #endregion

    }
}
