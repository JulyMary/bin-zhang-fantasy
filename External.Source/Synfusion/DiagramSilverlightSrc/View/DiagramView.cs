﻿#region Copyright
// Copyright Syncfusion Inc. 2001 - 2011. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion

namespace Syncfusion.Windows.Diagram
{
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
    using System.Windows.Printing;
    using System.Windows.Media.Imaging;

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
    ///namespace SilverlightApplication1
    /// {
    /// public partial class MainPage : UserControl
    /// {
    ///    public DiagramControl Control;
    ///    public DiagramModel Model;
    ///    public DiagramView View;
    ///    public MainPage()
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
    public partial class DiagramView : ContentControl, IView
    {        
        public bool CustomCursorEnabled
        {
            get { return (bool)GetValue(CustomCursorEnabledProperty); }
            set { SetValue(CustomCursorEnabledProperty, value); }
        }

        public static readonly DependencyProperty CustomCursorEnabledProperty = DependencyProperty.Register("CustomCursorEnabled", typeof(bool), typeof(DiagramView), new PropertyMetadata(true));
        public static readonly DependencyProperty SnapToHorizontalGridProperty = DependencyProperty.Register("SnapToHorizontalGrid", typeof(bool), typeof(DiagramView), new PropertyMetadata(false));
        public static readonly DependencyProperty SnapToVerticalGridProperty = DependencyProperty.Register("SnapToVerticalGrid", typeof(bool), typeof(DiagramView), new PropertyMetadata(false));
        public static readonly DependencyProperty SnapOffsetXProperty = DependencyProperty.Register("SnapOffsetX", typeof(double), typeof(DiagramView), new PropertyMetadata(25d));
        public static readonly DependencyProperty SnapOffsetYProperty = DependencyProperty.Register("SnapOffsetY", typeof(double), typeof(DiagramView), new PropertyMetadata(25d));

        public static readonly DependencyProperty UndoRedoEnabledProperty = DependencyProperty.Register("UndoRedoEnabled", typeof(bool), typeof(DiagramView), new PropertyMetadata(true,new PropertyChangedCallback(OnUndoRedoEnabledChanged)));

        public static readonly DependencyProperty HorizontalRulerProperty = DependencyProperty.Register("HorizontalRuler", typeof(HorizontalRuler), typeof(DiagramView), new PropertyMetadata(null, new PropertyChangedCallback(OnHorizontalRulerChanged)));

        public static readonly DependencyProperty VerticalRulerProperty = DependencyProperty.Register("VerticalRuler", typeof(VerticalRuler), typeof(DiagramView), new PropertyMetadata(null, new PropertyChangedCallback(OnVerticalRulerChanged)));

        /// <summary>
        /// Identifies the CurrentZoom property.This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty CurrentZoomProperty = DependencyProperty.Register("CurrentZoom", typeof(double), typeof(DiagramView), new PropertyMetadata(1d, new PropertyChangedCallback(OnCurrentZoomChanged)));

        /// <summary>
        /// Identifies the EnableConnection dependency property.
        /// </summary>
        public static readonly DependencyProperty EnableConnectionProperty = DependencyProperty.Register("EnableConnection", typeof(bool), typeof(DiagramView), new PropertyMetadata(false));

        /// <summary>
        /// Identifies the Selected item.This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty IsPageEditableProperty = DependencyProperty.Register("IsPageEditable", typeof(bool), typeof(DiagramView), new PropertyMetadata(true, new PropertyChangedCallback(OnIsPageEditableChanged)));

        /// <summary>
        /// Identifies the Selected item.This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(DiagramView), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the Page property.This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty PageProperty = DependencyProperty.Register("Page", typeof(IDiagramPage), typeof(DiagramView), new PropertyMetadata(new PropertyChangedCallback(OnPageChanged)));

        /// <summary>
        /// Identifies the ViewGridOrigin property.This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewGridOriginProperty = DependencyProperty.Register("ViewGridOrigin", typeof(Point), typeof(DiagramView), new PropertyMetadata(new Point(0, 0), new PropertyChangedCallback(OnViewGridOriginChanged)));

        /// <summary>
        /// Identifies the NodeContextMenuProperty property.This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty NodeContextMenuProperty = DependencyProperty.Register("NodeContextMenu", typeof(ContextMenuControl), typeof(DiagramView), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the LineContextMenuProperty property.This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty LineConnectorContextMenuProperty = DependencyProperty.Register("LineConnectorContextMenu", typeof(ContextMenuControl), typeof(DiagramView), new PropertyMetadata(null));


        /// <summary>
        /// Identifies the ContextMenu property.This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty ContextMenuProperty = DependencyProperty.Register("ContextMenu", typeof(ContextMenuControl), typeof(DiagramView), new PropertyMetadata(null));


        /// <summary>
        /// Identifies the ShowVerticalGridLine property.This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowVerticalGridLineProperty = DependencyProperty.Register("ShowVerticalGridLine", typeof(bool), typeof(DiagramView), new PropertyMetadata(false, new PropertyChangedCallback(OnShowVLineChanged)));

        /// <summary>
        /// Identifies the ShowVerticalRuler property.This is a dependency property.
        /// </summary>
        internal static readonly DependencyProperty ShowVerticalRulerProperty = DependencyProperty.Register("ShowVerticalRulers", typeof(bool), typeof(DiagramView), new PropertyMetadata(true, new PropertyChangedCallback(OnShowVRulerChanged)));

        /// <summary>
        /// Identifies the ZoomFactor property.This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty ZoomFactorProperty = DependencyProperty.Register("ZoomFactor", typeof(double), typeof(DiagramView), new PropertyMetadata(.2d, new PropertyChangedCallback(OnZoomFactorChanged)));

        /// <summary>
        /// Used to store the values in the selection list.
        /// </summary>
        internal ObservableCollection<ICommon> oldselectionlist = new ObservableCollection<ICommon>();

        /// <summary>
        /// Identifies the PortVisibility dependency property.
        /// </summary>
        internal static readonly DependencyProperty PortVisibilityProperty = DependencyProperty.Register("PortVisibility", typeof(Visibility), typeof(DiagramView), new PropertyMetadata(Visibility.Collapsed));

        /// <summary>
        /// Identifies the Show Grid property.This is a dependency property.
        /// </summary>
        internal static readonly DependencyProperty ShowGridProperty = DependencyProperty.Register("ShowGrid", typeof(bool), typeof(DiagramView), new PropertyMetadata(true, new PropertyChangedCallback(OnShowGridChanged)));

        /// <summary>
        /// Identifies the ShowHorizontalGridLine property.This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowHorizontalGridLineProperty = DependencyProperty.Register("ShowHorizontalGridLine", typeof(bool), typeof(DiagramView), new PropertyMetadata(false, new PropertyChangedCallback(OnShowHLineChanged)));

        /// <summary>
        /// Identifies the ShowHorizontalRuler property.This is a dependency property.
        /// </summary>
        internal static readonly DependencyProperty ShowHorizontalRulerProperty = DependencyProperty.Register("ShowHorizontalRulers", typeof(bool), typeof(DiagramView), new PropertyMetadata(true, new PropertyChangedCallback(OnShowHRulerChanged)));

        /// <summary>
        /// Identifies the IsPanEnabled dependency property.
        /// </summary>
        internal static readonly DependencyProperty IsPanEnabledProperty = DependencyProperty.Register("IsPanEnabled", typeof(bool), typeof(DiagramView), new PropertyMetadata(false));

        /// <summary>
        /// Identifies the IsZoomEnabled dependency property.
        /// </summary>
        public static readonly DependencyProperty IsZoomEnabledProperty = DependencyProperty.Register("IsZoomEnabled", typeof(bool), typeof(DiagramView), new PropertyMetadata(true));

        /// <summary>
        /// Identifies the HorizontalScrollBarVisibility dependency property.
        /// </summary>
        internal static readonly DependencyProperty GetHorizontalScrollBarVisibilityProperty = DependencyProperty.Register("GetHorizontalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(DiagramView), new PropertyMetadata(ScrollBarVisibility.Auto, new PropertyChangedCallback(OnHorizontalScrollBarVisibilityChanged)));

        /// <summary>
        /// Identifies the VerticalScrollBarVisibility dependency property.
        /// </summary>
        internal static readonly DependencyProperty GetVerticalScrollBarVisibilityProperty = DependencyProperty.Register("GetVerticalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(DiagramView), new PropertyMetadata(ScrollBarVisibility.Auto, new PropertyChangedCallback(OnVerticalScrollBarVisibilityChanged)));

        internal bool m_IsCommandInProgress = false;

        /// <summary>
        ///  Used to store diagram Control instance.
        /// </summary>
        private DiagramControl dc;

        /// <summary>
        /// Used to store the delete count.
        /// </summary>
        private int delcount = 0;

        /// <summary>
        /// Checks if Delete command was executed.
        /// </summary>
        private bool deletecommandexe = false;

        /// <summary>
        /// Checks if <see cref="Node"/> was dragged.
        /// </summary>
        private bool dragged = false;

        /// <summary>
        /// Used to store the view.
        /// </summary>
        private DiagramView dview;

        /// <summary>
        ///  Used to store boolean value on executing on x axis once.
        /// </summary>
        private bool exeonce = false;

        /// <summary>
        /// Used to store boolean value on executing on y axis once.
        /// </summary>
        private bool exeoncey = false;

        /// <summary>
        /// Used to store horizontal offset
        /// </summary>
        //private double hf = 0;

        /// <summary>
        /// Used to store horizontal thumb instance
        /// </summary>
        private double horthumb = 0;

        /// <summary>
        /// Used to store internal edges collection.
        /// </summary>
        private CollectionExt intedges = new CollectionExt();

        /// <summary>
        /// Used to check if node is deleted
        /// </summary>
        private bool isdel = false;

        /// <summary>
        /// Used to store the node drag information.
        /// </summary>
        private bool isdrag = false;

        /// <summary>
        /// Used to check if node is dragged.
        /// </summary>
        private bool isdragged = false;

        /// <summary>
        /// Checks if deletion was done.
        /// </summary>
        private bool isdupdel;

        /// <summary>
        /// Used to check line drag in x coordinate.
        /// </summary>
        private bool islinedragged = false;

        /// <summary>
        /// Used to check line drag in y coordinate.
        /// </summary>
        private bool islinedraggedy = false;

        private bool islineloaded = false;

        /// <summary>
        /// Used to check is mouse is wheeled
        /// </summary>
        private bool ismousewheel = false;

        /// <summary>
        /// Used to store the IsPageEditable property
        /// </summary>
        private static bool ispagedit = true;

        private bool isrotating = false;

        /// <summary>
        /// Used to store the View grid state
        /// </summary>
        private static bool isvieworiginchanged = false;

        /// <summary>
        /// Used to check if mouse is scrolled
        /// </summary>
        private bool justscrolled = false;

        /// <summary>
        /// Used to check if mouse is just wheeled
        /// </summary>
        private bool justwheeled = true;

        /// <summary>
        /// Checks if automatic layout is used.
        /// </summary>
        private bool layout = false;

        /// <summary>
        /// Checks if line was deleted.
        /// </summary>
        private bool linedel = false;

        /// <summary>
        /// Used to store the line drag in x coordinate.
        /// </summary>
        private double linedragx = 0;

        /// <summary>
        /// Used to store the line drag in y coordinate.
        /// </summary>
        private double linedragy = 0;

        /// <summary>
        /// Used to store the groups in the page.
        /// </summary>
        private CollectionExt mgroups = new CollectionExt();

        private Canvas maingrid;

        /// <summary>
        /// Used to store the bounds
        /// </summary>
        private Thickness mbounds;

        /// <summary>
        /// Checks if MeasureOveride  was called.
        /// </summary>
        private bool measured = false;

        /// <summary>
        /// Used to store the model
        /// </summary>
        private IModel mmodel;

        /// <summary>
        /// Used to store the origin point
        /// </summary>
        private Point mOrigin;

        /// <summary>
        /// Used to check if mouse is pressed
        /// </summary>
        private bool mousep = false;

        /// <summary>
        /// Used to check if mouse is wheeled
        /// </summary>
        private bool mousew = false;

        /// <summary>
        /// Used to check if scrollbars were moved.
        /// </summary>
        private bool mscrolled = false;

        /// <summary>
        /// Used to check if mouse is up.
        /// </summary>
        private bool muponly = false;

        /// <summary>
        /// Checks if node was deleted.
        /// </summary>
        private bool nodedel = false;

        /// <summary>
        /// Used to store the node drag count.
        /// </summary>
        private int nodedragcount = 1;

        /// <summary>
        /// Used to check if offsetx is greater than zero
        /// </summary>
        private bool offgtz = false;

        /// <summary>
        /// Used to check if offsety is greater than zero
        /// </summary>
        private bool offygtz = false;

        /// <summary>
        /// Used to store old horizontal offset
        /// </summary>
        private double oldhoffset = 0;

        /// <summary>
        ///  Used to store old vertical offset
        /// </summary>
        private double oldvoffset = 0;

        /// <summary>
        /// Used to store the x value of the page origin with respect to the view.
        /// </summary>
        private double oldxviewgrid = 0;

        /// <summary>
        /// Used to store the y value of the page origin with respect to the view.
        /// </summary>
        private double oldyviewgrid = 0;

        /// <summary>
        /// Used to store the OnReset command value.
        /// </summary>
        private bool onReset = false;

        /// <summary>
        ///  Used to store the other event's state
        /// </summary>
        private static bool otherevents = false;

        /// <summary>
        /// Used to store pan constant
        /// </summary>
        private double panconst = 0;

        /// <summary>
        /// Used to store pixel interval
        /// </summary>
        private double pixelvalue = 50;

        /// <summary>
        /// Checks if Redo command was executed.
        /// </summary>
        private bool redo = false;

        /// <summary>
        /// Refers to the redo stack.
        /// </summary>
        private Stack<object> redocommandstack = new Stack<object>();

        /// <summary>
        /// Checks if <see cref="Node"/> was resized as a resule of redo operation.
        /// </summary>
        private bool redoresize = false;

        /// <summary>
        /// Used to store the count of the <see cref="Node"/> been resized.
        /// </summary>
        private int resizecount = 0;

        /// <summary>
        /// Checks if <see cref="Node"/> was resized.
        /// </summary>
        private bool resized = true;

        /// <summary>
        /// Used to store the count of the <see cref="Node"/> been rotated.
        /// </summary>
        private int rotatecount = 1;

        /// <summary>
        /// Used to store the screen start point
        /// </summary>
        private Point screenStartPoint = new Point(0, 0);

        /// <summary>
        /// Used to check if scrolled 
        /// </summary>
        private bool scrollchk = false;

        /// <summary>
        /// Used to check if mouse is scrolled 
        /// </summary>
        private bool scrolled = false;

        /// <summary>
        /// Used to check if scrolled bottom
        /// </summary>
        private bool scrolledbottom = false;

        /// <summary>
        /// Used to check if scrolled right
        /// </summary>
        private bool scrolledright = false;

        /// <summary>
        /// Used to store scroll thumb instance
        /// </summary>
        private double scrollhorthumb = 0;

        /// <summary>
        /// Used to store scroll thumb click boolean value.
        /// </summary>
        private bool scrollthumb = false;

        /// <summary>
        ///  Used to store scroll vertical thumb offset
        /// </summary>
        private double scrollverthumb = 0;

        /// <summary>
        /// Used to store the scrollviewer instance
        /// </summary>
        private ScrollViewer scrollview;

        /// <summary>
        /// Used to store ShowPage property
        /// </summary>
        private bool showPage;

        /// <summary>
        /// Used to store ShowRulers property
        /// </summary>
        private bool showRuler;

        /// <summary>
        /// Used to store the transform group
        /// </summary>
        private TransformGroup transformGroup;

        /// <summary>
        /// Used to store the translate transform
        /// </summary>
        private TranslateTransform translateTransform;

        /// <summary>
        /// Checks if Undo command was executed.
        /// </summary>
        private bool undo = false;



        /// <summary>
        /// Refers to the undo stack.
        /// </summary>
        private Stack<object> undocommandstack = new Stack<object>();

        /// <summary>
        /// Checks if <see cref="Node"/> was resized as a resule of undo operation.
        /// </summary>
        private bool undoresize = false;

        /// <summary>
        /// Used to store vertical thumb instance
        /// </summary>
        private double verthumb = 0;

        /// <summary>
        /// Used to store vertical offset
        /// </summary>
        //private double vf = 0;

        /// <summary>
        ///  Used to store the view grid
        /// </summary>
        internal Grid viewgrid;

        /// <summary>
        ///  Used to store the view grid origin
        /// </summary>
        private Point viewgridorigin = new Point(0, 0);

        /// <summary>
        ///  Used to store the origin point
        /// </summary>
        private Point vieworigin = new Point(0, 0);

        /// <summary>
        /// Used to check view grid origin position
        /// </summary>
        private bool vieworiginchanged2 = false;

        /// <summary>
        /// Used to store horizontal thumb offset value.
        /// </summary>
        private double x = 0;

        /// <summary>
        /// Used to store x coordinate value.
        /// </summary>
        //private double xcoordinate = 0;

        /// <summary>
        /// Used to store vertical thumb offset value.
        /// </summary>
        private double y = 0;

        /// <summary>
        /// Used to store y coordinate value.
        /// </summary>
        //private double ycoordinate = 0;

        /// <summary>
        /// Used to store  the scale transform
        /// </summary>
        internal ScaleTransform zoomTransform;

        /// <summary>
        /// Used to store whether the View Loaded or not.
        /// </summary>
        internal bool IsLoaded;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagramView"/> class.
        /// </summary>
        public DiagramView()
        {
            this.DefaultStyleKey = typeof(DiagramView);
            this.Loaded += new RoutedEventHandler(DiagramView_Loaded);
            this.Page = new DiagramPage();
            this.tUndoStack = new StackExt<object>();
            this.tRedoStack = new StackExt<object>();
            this.tUndoStack.PushedEvent += new PushedEventHandler(tUndoStack_PushedEvent);
            this.Loaded += new RoutedEventHandler(LoadedForUnitChange);
            //this.Unloaded += new RoutedEventHandler(DiagramView_Unloaded);
            //this.HorizontalGridLineStyle = new GridLineStyle();
            //HorizontalGridLineStyle.Stroke = new SolidColorBrush(Colors.DarkGray);
            //HorizontalGridLineStyle.StrokeThickness = 0.3d;

            //this.VerticalGridLineStyle = new GridLineStyle();
            //VerticalGridLineStyle.Stroke = new SolidColorBrush(Colors.DarkGray);
            //VerticalGridLineStyle.StrokeThickness = 0.3d;
            p._Visual = Page;
            p.SetVisual();
        }
        DiagramPrintDialog p = new DiagramPrintDialog();
        public DiagramPrintDialog DiagramPrintDialog
        {
            get { return this.p; }
            set{this.p=value;}
        }
        internal double PxSnapOffsetX
        {
            get
            {
                return MeasureUnitsConverter.ToPixels(SnapOffsetX, (this.Page as DiagramPage).MeasurementUnits);
            }
            set
            {
                SnapOffsetX = MeasureUnitsConverter.FromPixels(SnapOffsetX, (this.Page as DiagramPage).MeasurementUnits);
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

        internal double PxSnapOffsetY
        {
            get
            {
                return MeasureUnitsConverter.ToPixels(SnapOffsetY, (this.Page as DiagramPage).MeasurementUnits);
            }
            set
            {
                SnapOffsetY = MeasureUnitsConverter.FromPixels(SnapOffsetY, (this.Page as DiagramPage).MeasurementUnits);
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

        public bool UndoRedoEnabled
        {
            get
            {
                return (bool)GetValue(UndoRedoEnabledProperty);
            }

            set
            {
                SetValue(UndoRedoEnabledProperty, value);
            }
        }
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
        /// Calls property changed event handler.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public GridLineStyle HorizontalGridLineStyle
        {
            get
            {
                return (GridLineStyle)GetValue(HorizontalGridLineStyleProperty);
            }

            set
            {
                SetValue(HorizontalGridLineStyleProperty, value);
            }
        }

        public static readonly DependencyProperty HorizontalGridLineStyleProperty = DependencyProperty.Register("HorizontalGridLineStyle", typeof(GridLineStyle), typeof(DiagramView), new PropertyMetadata(new GridLineStyle() { Brush = new SolidColorBrush(Colors.DarkGray), StrokeThickness = 0.3d }));

        public GridLineStyle VerticalGridLineStyle
        {
            get
            {
                return (GridLineStyle)GetValue(VerticalGridLineStyleProperty);
            }

            set
            {
                SetValue(VerticalGridLineStyleProperty, value);
            }
        }

        public static readonly DependencyProperty VerticalGridLineStyleProperty = DependencyProperty.Register("VerticalGridLineStyle", typeof(GridLineStyle), typeof(DiagramView), new PropertyMetadata(new GridLineStyle() { Brush = new SolidColorBrush(Colors.DarkGray), StrokeThickness = 0.3d }));


        /// <summary>
        /// Event  when Series Visibility changed by the chart legend
        /// </summary>
        //public event NodeDroppedEventHandler NodeDropped;



        #region Event Handlers


        /// <summary>
        /// Occurs when [line moved].
        /// </summary>
        public event LineNudgeEventHandler LineMoved;

        /// <summary>
        /// Calls when Series Visiblity Change by the Legend Checkbox
        /// </summary>
        /// <param name="obj">Sender Object.  It may be either ChartSeries or Segment</param>
        /// <param name="e">Event Argument</param>
        internal virtual void OnLineMoved(object obj, LineNudgeEventArgs e)
        {
            if (this.LineMoved != null)
            {
                LineMoved(obj, e);
            }
        }

        /// <summary>
        /// Occurs when [node moved].
        /// </summary>
        public event NodeNudgeEventHandler NodeMoved;

        /// <summary>
        /// Calls when Series Visiblity Change by the Legend Checkbox
        /// </summary>
        /// <param name="obj">Sender Object.  It may be either ChartSeries or Segment</param>
        /// <param name="e">Event Argument</param>
        internal virtual void OnNodeMoved(object obj, NodeNudgeEventArgs e)
        {
            if (this.NodeMoved != null)
            {
                NodeMoved(obj, e);
            }
        }

        /// <summary>
        /// NodeSelected Event Handler
        /// </summary>
        /// Type: <see cref="NodeEventHandler"/>
        public event NodeEventHandler NodeSelected;

        /// <summary>
        /// Calls when Series Visiblity Change by the Legend Checkbox
        /// </summary>
        /// <param name="obj">Sender Object.  It may be either ChartSeries or Segment</param>
        /// <param name="e">Event Argument</param>
        internal virtual void OnNodeSelected(object obj, NodeRoutedEventArgs e)
        {
            if (this.NodeSelected != null)
            {
                NodeSelected(obj, e);
            }
        }

        /// <summary>
        /// NodeUnSelected Event Handler
        /// </summary>
        /// Type: <see cref="NodeEventHandler"/>
        public event NodeEventHandler NodeUnSelected;

        /// <summary>
        /// Calls when Series Visiblity Change by the Legend Checkbox
        /// </summary>
        /// <param name="obj">Sender Object.  It may be either ChartSeries or Segment</param>
        /// <param name="e">Event Argument</param>
        internal virtual void OnNodeUnSelected(object obj, NodeRoutedEventArgs e)
        {
            if (this.NodeUnSelected != null)
            {
                NodeUnSelected(obj, e);
            }
        }

        /// <summary>
        /// NodeDeleted Event Handler
        /// </summary>
        /// Type: <see cref="NodeDeleteEventHandler"/>
        public event NodeDeleteEventHandler NodeDeleted;

        /// <summary>
        /// Calls when Series Visiblity Change by the Legend Checkbox
        /// </summary>
        /// <param name="obj">Sender Object.  It may be either ChartSeries or Segment</param>
        /// <param name="e">Event Argument</param>
        internal virtual void OnNodeDeleted(object obj, NodeDeleteRoutedEventArgs e)
        {
            if (this.NodeDeleted != null)
            {

                NodeDeleted(obj, e);
            }
        }


        /// <summary>
        /// NodeDeleted Event Handler
        /// </summary>
        /// Type: <see cref="NodeDeleteEventHandler"/>
        public event NodeDeleteEventHandler NodeDeleting;

        /// <summary>
        /// Calls when Series Visiblity Change by the Legend Checkbox
        /// </summary>
        /// <param name="obj">Sender Object.  It may be either ChartSeries or Segment</param>
        /// <param name="e">Event Argument</param>
        internal virtual void OnNodeDeleting(object obj, NodeDeleteRoutedEventArgs e)
        {
            if (this.NodeDeleting != null)
            {
                NodeDeleting(obj, e);
            }
        }

        /// <summary>
        /// ConnectorDeleted Event Handler
        /// </summary>
        /// Type: <see cref="ConnectionDeleteEventHandler"/>
        public event ConnectionDeleteEventHandler ConnectorDeleted;

        /// <summary>
        /// Calls when Series Visiblity Change by the Legend Checkbox
        /// </summary>
        /// <param name="obj">Sender Object.  It may be either ChartSeries or Segment</param>
        /// <param name="e">Event Argument</param>
        internal virtual void OnConnectionDeleted(object obj, ConnectionDeleteRoutedEventArgs e)
        {
            if (this.ConnectorDeleted != null)
            {
                ConnectorDeleted(obj, e);
            }
        }

        /// <summary>
        /// ConnectorDeleted Event Handler
        /// </summary>
        /// Type: <see cref="ConnectionDeleteEventHandler"/>
        public event ConnectionDeleteEventHandler ConnectorDeleting;

        /// <summary>
        /// Calls when Series Visiblity Change by the Legend Checkbox
        /// </summary>
        /// <param name="obj">Sender Object.  It may be either ChartSeries or Segment</param>
        /// <param name="e">Event Argument</param>
        internal virtual void OnConnectionDeleting(object obj, ConnectionDeleteRoutedEventArgs e)
        {
            if (this.ConnectorDeleting != null)
            {
                ConnectorDeleting(obj, e);
            }
        }

        /// <summary>
        /// PreviewNodeDrop Event Handler
        /// </summary>
        /// Type: <see cref="PreviewNodeDropEventHandler"/>
        public event PreviewNodeDropEventHandler PreviewNodeDrop;

        /// <summary>
        /// Calls when Series Visiblity Change by the Legend Checkbox
        /// </summary>
        /// <param name="obj">Sender Object.  It may be either ChartSeries or Segment</param>
        /// <param name="e">Event Argument</param>
        internal virtual void OnPreviewNodeDrop(object obj, PreviewNodeDropEventRoutedEventArgs e)
        {
            if (this.PreviewNodeDrop != null)
            {
                PreviewNodeDrop(obj, e);
            }
        }

        /// <summary>
        /// PreviewConnectorDrop Event Handler
        /// </summary>
        /// Type: <see cref="PreviewConnectorDropEventHandler"/>
        public event PreviewConnectorDropEventHandler PreviewConnectorDrop;

        /// <summary>
        /// Calls when Series Visiblity Change by the Legend Checkbox
        /// </summary>
        /// <param name="obj">Sender Object.  It may be either ChartSeries or Segment</param>
        /// <param name="e">Event Argument</param>
        internal virtual void OnPreviewConnectorDrop(object obj, PreviewConnectorDropEventRoutedEventArgs e)
        {
            if (this.PreviewConnectorDrop != null)
            {
                PreviewConnectorDrop(obj, e);
            }
        }

        /// <summary>
        ///  NodeDragStart Event Handler .
        /// </summary>
        /// Type: <see cref="NodeEventHandler"/>
        public event NodeEventHandler NodeDragStart;

        /// <summary>
        /// Calls when Series Visiblity Change by the Legend Checkbox
        /// </summary>
        /// <param name="obj">Sender Object.  It may be either ChartSeries or Segment</param>
        /// <param name="e">Event Argument</param>
        internal virtual void OnNodeDragStart(object obj, NodeRoutedEventArgs e)
        {
            if (this.NodeDragStart != null)
            {
                NodeDragStart(obj, e);
            }
        }

        /// <summary>
        ///  NodeDragStart Event Handler .
        /// </summary>
        /// Type: <see cref="NodeEventHandler"/>
        public event NodeEventHandler NodeDragEnd;

        /// <summary>
        /// Calls when Series Visiblity Change by the Legend Checkbox
        /// </summary>
        /// <param name="obj">Sender Object.  It may be either ChartSeries or Segment</param>
        /// <param name="e">Event Argument</param>
        internal virtual void OnNodeDragEnd(object obj, NodeRoutedEventArgs e)
        {
            if (this.NodeDragEnd != null)
            {
                NodeDragEnd(obj, e);
            }
        }

        /// <summary>
        ///  NodeClick Event Handler .
        /// </summary>
        /// Type: <see cref="NodeEventHandler"/>
        public event NodeEventHandler NodeClick;

        public event ConnectorRoutedEventHandler ConnectorClick;


        internal virtual void OnConnectorClick(object obj, ConnectorRoutedEventArgs e)
        {
            if (this.ConnectorClick != null)
            {
                ConnectorClick(obj, e);   
            }
        }
        /// <summary>
        /// Calls when Series Visiblity Change by the Legend Checkbox
        /// </summary>
        /// <param name="obj">Sender Object.  It may be either ChartSeries or Segment</param>
        /// <param name="e">Event Argument</param>
        internal virtual void OnNodeClick(object obj, NodeRoutedEventArgs e)
        {
            if (this.NodeClick != null)
            {
                NodeClick(obj, e);
            }
        }

        /// <summary>
        ///  NodeDoubleClick Event Handler .
        /// </summary>
        /// Type: <see cref="NodeEventHandler"/>
        public event NodeEventHandler NodeDoubleClick;

        /// <summary>
        /// Calls when Series Visiblity Change by the Legend Checkbox
        /// </summary>
        /// <param name="obj">Sender Object.  It may be either ChartSeries or Segment</param>
        /// <param name="e">Event Argument</param>
        internal virtual void OnNodeDoubleClick(object obj, NodeRoutedEventArgs e)
        {
            if (this.NodeDoubleClick != null)
            {
                NodeDoubleClick(obj, e);
            }
        }

        /// <summary>
        ///  ConnectorDoubleClick Event Handler .
        /// </summary>
        /// Type: <see cref="ConnChangedEventHandler"/>
        public event ConnChangedEventHandler ConnectorDoubleClick;

        /// <summary>
        /// Calls when Series Visiblity Change by the Legend Checkbox
        /// </summary>
        /// <param name="obj">Sender Object.  It may be either ChartSeries or Segment</param>
        /// <param name="e">Event Argument</param>
        internal virtual void OnConnectorDoubleClick(object obj, ConnRoutedEventArgs e)
        {
            if (this.ConnectorDoubleClick != null)
            {
                ConnectorDoubleClick(obj, e);
            }
        }

        /// <summary>
        ///  NodeDropped Event Handler .
        /// </summary>
        /// Type: <see cref="NodeDroppedEventHandler"/>
        public event NodeDroppedEventHandler NodeDrop;

        /// <summary>
        /// Calls when Series Visiblity Change by the Legend Checkbox
        /// </summary>
        /// <param name="obj">Sender Object.  It may be either ChartSeries or Segment</param>
        /// <param name="e">Event Argument</param>
        internal virtual void OnNodeDropped(object obj, NodeDroppedRoutedEventArgs e)
        {
            if (this.NodeDrop != null)
            {
                NodeDrop(obj, e);
            }
        }

        /// <summary>
        ///  ConnectorDropped Event Handler .
        /// </summary>
        /// Type: <see cref="ConnectorDroppedEventHandler"/>
        public event ConnectorDroppedEventHandler ConnectorDrop;

        /// <summary>
        /// Calls when Series Visiblity Change by the Legend Checkbox
        /// </summary>
        /// <param name="obj">Sender Object.  It may be either ChartSeries or Segment</param>
        /// <param name="e">Event Argument</param>
        internal virtual void OnConnectorDrop(object obj, ConnectorDroppedRoutedEventArgs e)
        {
            if (this.ConnectorDrop != null)
            {
                ConnectorDrop(obj, e);
            }
        }

        /// <summary>
        ///  NodeResized Event Handler .
        /// </summary>
        /// Type: <see cref="NodeEventHandler"/>
        public event NodeEventHandler NodeResized;

        /// <summary>
        /// Calls when Series Visiblity Change by the Legend Checkbox
        /// </summary>
        /// <param name="obj">Sender Object.  It may be either ChartSeries or Segment</param>
        /// <param name="e">Event Argument</param>
        internal virtual void OnNodeResized(object obj, NodeRoutedEventArgs e)
        {
            if (this.NodeResized != null)
            {
                NodeResized(obj, e);
            }
        }

        /// <summary>
        ///  NodeResizing Event Handler .
        /// </summary>
        /// Type: <see cref="NodeEventHandler"/>
        public event NodeEventHandler NodeResizing;

        /// <summary>
        /// Calls when Series Visiblity Change by the Legend Checkbox
        /// </summary>
        /// <param name="obj">Sender Object.  It may be either ChartSeries or Segment</param>
        /// <param name="e">Event Argument</param>
        internal virtual void OnNodeResizing(object obj, NodeRoutedEventArgs e)
        {
            if (this.NodeResizing != null)
            {
                NodeResizing(obj, e);
            }
        }

        /// <summary>
        ///  NodeRotationChanged Event Handler .
        /// </summary>
        /// Type: <see cref="NodeEventHandler"/>
        public event NodeEventHandler NodeRotationChanged;

        /// <summary>
        /// Calls when Series Visiblity Change by the Legend Checkbox
        /// </summary>
        /// <param name="obj">Sender Object.  It may be either ChartSeries or Segment</param>
        /// <param name="e">Event Argument</param>
        internal virtual void OnNodeRotationChanged(object obj, NodeRoutedEventArgs e)
        {
            if (this.NodeRotationChanged != null)
            {
                NodeRotationChanged(obj, e);
            }
        }

        /// <summary>
        ///  NodeRotationChanging Event Handler .
        /// </summary>
        /// Type: <see cref="NodeEventHandler"/>
        public event NodeEventHandler NodeRotationChanging;

        /// <summary>
        /// Calls when Series Visiblity Change by the Legend Checkbox
        /// </summary>
        /// <param name="obj">Sender Object.  It may be either ChartSeries or Segment</param>
        /// <param name="e">Event Argument</param>
        internal virtual void OnNodeRotationChanging(object obj, NodeRoutedEventArgs e)
        {
            if (this.NodeRotationChanging != null)
            {
                NodeRotationChanging(obj, e);
            }
        }



        /// <summary>
        ///  Grouped Event Handler .
        /// </summary>
        /// Type: <see cref="GroupedEventHandler"/>
        public event GroupEventHandler Grouping;
        internal virtual void OnGrouping(object obj, GroupEventArgs e)
        {
            if (this.Grouping != null)
            {
                Grouping(obj, e);
            }
        }


        public event GroupEventHandler Grouped;
        internal virtual void OnGrouped(object obj, GroupEventArgs e)
        {
            if (this.Grouped != null)
            {
                Grouped(obj, e);
            }
        }

        /// <summary>
        ///  UnGrouped Event Handler .
        /// </summary>
        /// Type: <see cref="UnGroupedEventHandler"/>
        public event UnGroupEventHandler UnGrouped;
        internal virtual void OnunGrouped(object obj, UnGroupEventArgs e)
        {
            if (this.UnGrouped != null)
            {
                UnGrouped(obj, e);
            }
        }

        public event UnGroupEventHandler UnGrouping;
        internal virtual void OnUnGrouping(object obj, UnGroupEventArgs e)
        {
            if (this.UnGrouping != null)
            {
                UnGrouping(obj, e);
            }
        }

        /// <summary>
        ///  ConnectorDragStart Event Handler .
        /// </summary>
        /// Type: <see cref="ConnDragChangedEventHandler"/>
        public event ConnDragChangedEventHandler ConnectorDragStart;

        /// <summary>
        /// Calls when Series Visiblity Change by the Legend Checkbox
        /// </summary>
        /// <param name="obj">Sender Object.  It may be either ChartSeries or Segment</param>
        /// <param name="e">Event Argument</param>
        internal virtual void OnConnectorDragStart(object obj, ConnDragRoutedEventArgs e)
        {
            if (this.ConnectorDragStart != null)
            {
                ConnectorDragStart(obj, e);
            }
        }

        /// <summary>
        ///  ConnectorDragEnd Event Handler .
        /// </summary>
        /// Type: <see cref="ConnDragEndChangedEventHandler"/>
        public event ConnDragEndChangedEventHandler ConnectorDragEnd;

        /// <summary>
        /// Calls when Series Visiblity Change by the Legend Checkbox
        /// </summary>
        /// <param name="obj">Sender Object.  It may be either ChartSeries or Segment</param>
        /// <param name="e">Event Argument</param>
        internal virtual void OnConnectorDragEnd(object obj, ConnDragEndRoutedEventArgs e)
        {
            if (this.ConnectorDragEnd != null)
            {
                ConnectorDragEnd(obj, e);
            }
        }


        /// <summary>
        ///  HeadNodeChanged Event Handler .
        /// </summary>
        /// Type: <see cref="NodeChangedEventHandler"/>
        public event NodeChangedEventHandler HeadNodeChanged;

        /// <summary>
        /// Calls when Series Visiblity Change by the Legend Checkbox
        /// </summary>
        /// <param name="obj">Sender Object.  It may be either ChartSeries or Segment</param>
        /// <param name="e">Event Argument</param>
        internal virtual void OnHeadNodeChanged(object obj, NodeChangedRoutedEventArgs e)
        {
            if (this.HeadNodeChanged != null)
            {
                HeadNodeChanged(obj, e);
            }
        }

        /// <summary>
        ///  TailNodeChanged Event Handler .
        /// </summary>
        /// Type: <see cref="NodeChangedEventHandler"/>
        public event NodeChangedEventHandler TailNodeChanged;

        /// <summary>
        /// Calls when Series Visiblity Change by the Legend Checkbox
        /// </summary>
        /// <param name="obj">Sender Object.  It may be either ChartSeries or Segment</param>
        /// <param name="e">Event Argument</param>
        internal virtual void OnTailNodeChanged(object obj, NodeChangedRoutedEventArgs e)
        {
            if (this.TailNodeChanged != null)
            {
                TailNodeChanged(obj, e);
            }
        }

        /// <summary>
        ///  NodeLabelChanged Event Handler .
        /// </summary>
        /// Type: <see cref="LabelChangedEventHandler"/>
        public event LabelChangedEventHandler NodeLabelChanged;

        /// <summary>
        /// Calls when Series Visiblity Change by the Legend Checkbox
        /// </summary>
        /// <param name="obj">Sender Object.  It may be either ChartSeries or Segment</param>
        /// <param name="e">Event Argument</param>
        internal virtual void OnNodeLabelChanged(object obj, LabelRoutedEventArgs e)
        {
            if (this.NodeLabelChanged != null)
            {
                NodeLabelChanged(obj, e);
            }
        }

        /// <summary>
        ///  LabelChangedEventHandler Event Handler .
        /// </summary>
        /// Type: <see cref="LabelChangedEventHandler"/>
        public event LabelChangedEventHandler NodeStartLabelEdit;

        /// <summary>
        /// Calls when Series Visiblity Change by the Legend Checkbox
        /// </summary>
        /// <param name="obj">Sender Object.  It may be either ChartSeries or Segment</param>
        /// <param name="e">Event Argument</param>
        internal virtual void OnNodeStartLabelEdit(object obj, LabelRoutedEventArgs e)
        {
            if (this.NodeStartLabelEdit != null)
            {
                NodeStartLabelEdit(obj, e);
            }
        }

        /// <summary>
        ///  LabelChangedEventHandler Event Handler .
        /// </summary>
        /// Type: <see cref="LabelConnChangedEventHandler"/>
        public event LabelConnChangedEventHandler ConnectorLabelChanged;

        /// <summary>
        /// Calls when Series Visiblity Change by the Legend Checkbox
        /// </summary>
        /// <param name="obj">Sender Object.  It may be either ChartSeries or Segment</param>
        /// <param name="e">Event Argument</param>
        internal virtual void OnConnectorLabelChanged(object obj, LabelConnRoutedEventArgs e)
        {
            if (this.ConnectorLabelChanged != null)
            {
                ConnectorLabelChanged(obj, e);
            }
        }

        /// <summary>
        ///  ConnectorStartLabelEdit Event Handler .
        /// </summary>
        /// Type: <see cref="LabelEditConnChangedEventHandler"/>
        public event LabelEditConnChangedEventHandler ConnectorStartLabelEdit;

        /// <summary>
        /// Calls when Series Visiblity Change by the Legend Checkbox
        /// </summary>
        /// <param name="obj">Sender Object.  It may be either ChartSeries or Segment</param>
        /// <param name="e">Event Argument</param>
        internal virtual void OnConnectorStartLabelEdit(object obj, LabelEditConnRoutedEventArgs e)
        {
            if (this.ConnectorStartLabelEdit != null)
            {
                ConnectorStartLabelEdit(obj, e);
            }
        }


        /// <summary>
        ///  BeforeConnectionCreate Event Handler .
        /// </summary>
        /// Type: <see cref="BeforeCreateConnectionEventHandler"/>
        public event BeforeCreateConnectionEventHandler BeforeConnectionCreate;

        /// <summary>
        /// Calls when Series Visiblity Change by the Legend Checkbox
        /// </summary>
        /// <param name="obj">Sender Object.  It may be either ChartSeries or Segment</param>
        /// <param name="e">Event Argument</param>
        internal virtual void OnBeforeConnectionCreate(object obj, BeforeCreateConnectionRoutedEventArgs e)
        {
            if (this.BeforeConnectionCreate != null)
            {
                BeforeConnectionCreate(obj, e);
            }
        }


        /// <summary>
        ///  AfterConnectionCreate Event Handler .
        /// </summary>
        /// Type: <see cref="BeforeCreateConnectionEventHandler"/>
        public event ConnDragEndChangedEventHandler AfterConnectionCreate;

        /// <summary>
        /// Calls when Series Visiblity Change by the Legend Checkbox
        /// </summary>
        /// <param name="obj">Sender Object.  It may be either ChartSeries or Segment</param>
        /// <param name="e">Event Argument</param>
        internal virtual void OnAfterConnectionCreate(object obj, ConnDragEndRoutedEventArgs e)
        {
            if (this.AfterConnectionCreate != null)
            {
                AfterConnectionCreate(obj, e);
            }
        }


        public event ConnectorSelectedEventHandler ConnectorSelected;
        internal virtual void OnConnectorSelected(object obj, ConnectorRoutedEventArgs e)
        {
            if (this.ConnectorSelected != null)
            {
                ConnectorSelected(obj, e);
            }
        }

        public event ConnectorSelectedEventHandler ConnectorUnSelected;
        internal virtual void OnConnectorUnselected(object obj, ConnectorRoutedEventArgs e)
        {
            if (this.ConnectorUnSelected != null)
            {
                ConnectorUnSelected(obj, e);
            }
        }

        #endregion


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

        internal Thickness PxBounds
        {
            get
            {
                return MeasureUnitsConverter.ToPixels(Bounds, (this.Page as DiagramPage).MeasurementUnits);
            }
            set
            {
                Bounds = MeasureUnitsConverter.FromPixels(value, (this.Page as DiagramPage).MeasurementUnits);
            }
        }


        /// <summary>
        /// Gets or sets the node context menu.
        /// </summary>
        /// <value>The node context menu.</value>
        public ContextMenuControl NodeContextMenu
        {
            get
            {
                return (ContextMenuControl)GetValue(NodeContextMenuProperty);
            }

            set
            {
                SetValue(NodeContextMenuProperty, value);
            }
        }


        /// <summary>
        /// Gets or sets the line context menu.
        /// </summary>
        /// <value>The line context menu.</value>
        public ContextMenuControl LineConnectorContextMenu
        {
            get
            {
                return (ContextMenuControl)GetValue(LineConnectorContextMenuProperty);
            }
            set
            {
                SetValue(LineConnectorContextMenuProperty, value);
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
            get
            {
                return this.mbounds;
            }

            set
            {
                this.mbounds = value;
            }
        }


        public ContextMenuControl ContextMenu
        {
            get
            {
                return (ContextMenuControl)GetValue(ContextMenuProperty);
            }
            set
            {
                SetValue(ContextMenuProperty, value);
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
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        ///    public DiagramControl Control;
        ///    public DiagramModel Model;
        ///    public DiagramView View;
        ///    public MainPage()
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
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public MainPage()
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
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public MainPage()
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
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        ///    public DiagramControl Control;
        ///    public DiagramModel Model;
        ///    public DiagramView View;
        ///    public MainPage()
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
                return this.mmodel;
            }

            set
            {
                this.mmodel = value;
            }
        }

        /// <summary>
        /// Gets or sets the origin.
        /// </summary>
        /// <value>
        /// Type: <see cref="System.Drawing.Point"/>
        /// The pan.
        /// </value>
        public Point Origin
        {
            get
            {
                return this.mOrigin;
            }

            set
            {
                this.mOrigin = value;
            }
        }

        /// <summary>
        /// Gets or sets the page.
        /// </summary>
        /// <value>
        /// Type: <see cref="Panel"/>
        /// Panel instance.
        /// </value>
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
        /// Gets or sets the delete count.
        /// </summary>
        /// <value>The delete count.</value>
        internal int DeleteCount
        {
            get { return this.delcount; }
            set { this.delcount = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Node"/> or <see cref="LineConnector"/> is deleted.
        /// </summary>
        /// <value><c>true</c> if deleted; otherwise, <c>false</c>.</value>
        internal bool Deleted
        {
            get { return this.isdel; }
            set { this.isdel = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether delete is done.
        /// </summary>
        /// <value><c>true</c> if [deleted]; otherwise, <c>false</c>.</value>
        internal bool DupDeleted
        {
            get { return this.isdupdel; }
            set { this.isdupdel = value; }
        }

        /// <summary>
        /// Gets or sets the duplicate horizontal thumb drag.
        /// </summary>
        /// <value>The duplicate horizontal thumb drag.</value>
        internal double DupHorthumbdrag
        {
            get { return this.horthumb; }
            set { this.horthumb = value; }
        }

        /// <summary>
        /// Gets or sets the duplicate vertical thumb drag.
        /// </summary>
        /// <value>The duplicate vertical thumb drag..</value>
        internal double DupVerthumbdrag
        {
            get { return this.verthumb; }
            set { this.verthumb = value; }
        }

        /// <summary>
        /// Gets or sets the horizontal thumb drag offset.
        /// </summary>
        /// <value>The horizontal thumb drag offset.</value>
        internal double HorThumbDragOffset
        {
            get { return scrollhorthumb; }
            set { scrollhorthumb = value; }
        }

        /// <summary>
        /// Gets the internal edges.
        /// </summary>
        /// <value>The internal edges.</value>
        internal CollectionExt InternalEdges
        {
            get { return this.intedges; }
        }

        /// <summary>
        /// Gets the internal groups collection.
        /// </summary>
        /// <value>The internal groups collection.</value>
        internal CollectionExt InternalGroups
        {
            get { return this.mgroups; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the delete command is executed.
        /// </summary>
        /// <value>
        /// <c>true</c> if delete command is executed; otherwise, <c>false</c>.
        /// </value>
        internal bool IsDeleteCommandExecuted
        {
            get { return this.deletecommandexe; }
            set { this.deletecommandexe = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Node"/> is dragged.
        /// </summary>
        /// <value>
        /// <c>true</c> if <see cref="Node"/>  is dragged; otherwise, <c>false</c>.
        /// </value>
        internal bool IsDragged
        {
            get { return this.dragged; }
            set { this.dragged = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether node is dragged.
        /// </summary>
        /// <value><c>true</c> if is dragged; otherwise, <c>false</c>.</value>
        internal bool IsDragging
        {
            get { return this.isdragged; }
            set { this.isdragged = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is dup mouse pressed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dup mouse pressed; otherwise, <c>false</c>.
        /// </value>
        internal bool IsDupMousePressed
        {
            get { return this.mousep; }
            set { this.mousep = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is dup mouse wheeled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dup mouse wheeled; otherwise, <c>false</c>.
        /// </value>
        internal bool IsDupMouseWheeled
        {
            get { return this.mousew; }
            set { this.mousew = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is exe once.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is exe once; otherwise, <c>false</c>.
        /// </value>
        internal bool IsExeOnce
        {
            get { return this.exeonce; }
            set { this.exeonce = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is exe once Y.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is exe once Y; otherwise, <c>false</c>.
        /// </value>
        internal bool IsExeOnceY
        {
            get { return this.exeoncey; }
            set { this.exeoncey = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is just scrolled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is just scrolled; otherwise, <c>false</c>.
        /// </value>
        internal bool IsJustScrolled
        {
            get { return this.justscrolled; }
            set { this.justscrolled = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is just wheeled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is just wheeled; otherwise, <c>false</c>.
        /// </value>
        internal bool IsJustWheeled
        {
            get { return this.justwheeled; }
            set { this.justwheeled = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether automatic layout is used.
        /// </summary>
        /// <value><c>true</c> if automatic layout is used; otherwise, <c>false</c>.</value>
        internal bool IsLayout
        {
            get { return this.layout; }
            set { this.layout = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="LineConnector"/> is deleted.
        /// </summary>
        /// <value><c>true</c> if is line deleted; otherwise, <c>false</c>.</value>
        internal bool Islinedeleted
        {
            get { return this.linedel; }
            set { this.linedel = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether connector is dragged.
        /// </summary>
        /// <value><c>true</c> if line is dragged; otherwise, <c>false</c>.</value>
        internal bool IsLinedragdelta
        {
            get { return this.islinedragged; }
            set { this.islinedragged = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether connector is dragged.
        /// </summary>
        /// <value><c>true</c> if line is dragged; otherwise, <c>false</c>.</value>
        internal bool IsLinedragdeltay
        {
            get { return this.islinedraggedy; }
            set { this.islinedraggedy = value; }
        }

        internal bool Islineloaded
        {
            get { return this.islineloaded; }
            set { this.islineloaded = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the MeasureOverride of <see cref="DiagramPage"/> is called.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is measure called; otherwise, <c>false</c>.
        /// </value>
        internal bool IsMeasureCalled
        {
            get { return this.measured; }
            set { this.measured = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the scrollbar were moved.
        /// </summary>
        /// <value>
        /// <c>true</c> if mouse scrolled; otherwise, <c>false</c>.
        /// </value>
        internal bool IsMouseScrolled
        {
            get { return this.mscrolled; }
            set { this.mscrolled = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether mouse up is fired.
        /// </summary>
        /// <value>
        /// <c>true</c> if mouse up, otherwise, <c>false</c>.
        /// </value>
        internal bool IsMouseUponly
        {
            get { return this.muponly; }
            set { this.muponly = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is mouse wheeled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is mouse wheeled; otherwise, <c>false</c>.
        /// </value>
        internal bool IsMouseWheeled
        {
            get { return this.ismousewheel; }
            set { this.ismousewheel = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a <see cref="Node"/> is deleted.
        /// </summary>
        /// <value><c>true</c> if node is deleted; otherwise, <c>false</c>.</value>
        internal bool Isnodedeleted
        {
            get { return this.nodedel; }
            set { this.nodedel = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the offsetx is positive.
        /// </summary>
        /// <value>
        /// <c>true</c> if offsetx is positive; otherwise, <c>false</c>.
        /// </value>
        internal bool IsOffsetxpositive
        {
            get { return this.offgtz; }
            set { this.offgtz = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the offsety is positive.
        /// </summary>
        /// <value>
        /// <c>true</c> if offsety is positive; otherwise, <c>false</c>.
        /// </value>
        internal bool IsOffsetypositive
        {
            get { return this.offygtz; }
            set { this.offygtz = value; }
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
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        ///    public DiagramControl Control;
        ///    public DiagramModel Model;
        ///    public DiagramView View;
        ///    public MainPage()
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
        internal bool IsPanEnabled
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
        /// Gets or sets a value indicating whether the <see cref="Node"/> position is changed.
        /// </summary>
        /// <value><c>true</c> if position is changed; otherwise, <c>false</c>.</value>
        internal bool Ispositionchanged
        {
            get { return this.isdrag; }
            set { this.isdrag = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Node"/> is resized.
        /// </summary>
        /// <value>
        /// <c>true</c> if <see cref="Node"/> is resized; otherwise, <c>false</c>.
        /// </value>
        internal bool IsResized
        {
            get { return this.resized; }
            set { this.resized = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Node"/> is resized as a result of redo operation.
        /// </summary>
        /// <value>
        /// <c>true</c> if <see cref="Node"/> is resized redone; otherwise, <c>false</c>.
        /// </value>
        internal bool IsResizedRedone
        {
            get { return this.redoresize; }
            set { this.redoresize = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Node"/> is resized as a result of undo operation.
        /// </summary>
        /// <value>
        /// <c>true</c> if <see cref="Node"/> is resized undone; otherwise, <c>false</c>.
        /// </value>
        internal bool IsResizedUndone
        {
            get { return this.undoresize; }
            set { this.undoresize = value; }
        }

        internal bool IsRotating
        {
            get { return this.isrotating; }
            set { this.isrotating = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is scroll check.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is scroll check; otherwise, <c>false</c>.
        /// </value>
        internal bool IsScrollCheck
        {
            get { return this.scrollchk; }
            set { this.scrollchk = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is scrolled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is scrolled; otherwise, <c>false</c>.
        /// </value>
        internal bool IsScrolled
        {
            get { return this.scrolled; }
            set { this.scrolled = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is scrolled bottom.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is scrolled bottom; otherwise, <c>false</c>.
        /// </value>
        internal bool IsScrolledBottom
        {
            get { return this.scrolledbottom; }
            set { this.scrolledbottom = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is scrolled right.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is scrolled right; otherwise, <c>false</c>.
        /// </value>
        internal bool IsScrolledRight
        {
            get { return this.scrolledright; }
            set { this.scrolledright = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is scroll thumb.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is scroll thumb; otherwise, <c>false</c>.
        /// </value>
        internal bool IsScrollThumb
        {
            get { return this.scrollthumb; }
            set { this.scrollthumb = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="DiagramView"/> is isvieworiginchanged2.
        /// </summary>
        /// <value><c>true</c> if isvieworiginchanged2; otherwise, <c>false</c>.</value>
        internal bool Isvieworiginchanged2
        {
            get { return this.vieworiginchanged2; }
            set { this.vieworiginchanged2 = value; }
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
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        ///    public DiagramControl Control;
        ///    public DiagramModel Model;
        ///    public DiagramView View;
        ///    public MainPage()
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
        //internal bool IsZoomEnabled
        //{
        //    get
        //    {
        //        return (bool)GetValue(IsZoomEnabledProperty);
        //    }

        //    set
        //    {
        //        SetValue(IsZoomEnabledProperty, value);
        //    }
        //}

        /// <summary>
        /// Gets or sets the line horizontal drag offset.
        /// </summary>
        /// <value>The line horizontal offset.</value>
        internal double Linehoffset
        {
            get { return this.linedragx; }
            set { this.linedragx = value; }
        }

        /// <summary>
        /// Gets or sets the line vertical drag offset.
        /// </summary>
        /// <value>The line vertical offset.</value>
        internal double Linevoffset
        {
            get { return this.linedragy; }
            set { this.linedragy = value; }
        }

        /// <summary>
        /// Gets or sets the node drag count.
        /// </summary>
        /// <value>The node drag count.</value>
        internal int NodeDragCount
        {
            get { return this.nodedragcount; }
            set { this.nodedragcount = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Node"/> resized count.
        /// </summary>
        /// <value>The node resized count.</value>
        internal int NodeResizedCount
        {
            get { return this.resizecount; }
            set { this.resizecount = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Node"/> rotate count.
        /// </summary>
        /// <value>The node rotate count.</value>
        internal int NodeRotateCount
        {
            get { return this.rotatecount; }
            set { this.rotatecount = value; }
        }

        /// <summary>
        /// Gets or sets the old horizontal offset.
        /// </summary>
        /// <value>The old old horizontal offset.</value>
        internal double OldHoroffset
        {
            get { return this.oldhoffset; }
            set { this.oldhoffset = value; }
        }

        /// <summary>
        /// Gets or sets the old vertical offset.
        /// </summary>
        /// <value>The old vertical offset.</value>
        internal double OldVeroffset
        {
            get { return this.oldvoffset; }
            set { this.oldvoffset = value; }
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
        /// Gets or sets the panned value.
        /// </summary>
        /// <value>The value by which the page is panned.</value>
        internal double PanConstant
        {
            get { return this.panconst; }
            set { this.panconst = value; }
        }

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
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        ///    public DiagramControl Control;
        ///    public DiagramModel Model;
        ///    public DiagramView View;
        ///    public MainPage()
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
        internal Visibility PortVisibility
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
        /// Gets or sets a value indicating whether the Redo command is executed.
        /// </summary>
        /// <value><c>true</c> if redone; otherwise, <c>false</c>.</value>
        internal bool Redone
        {
            get { return this.redo; }
            set { this.redo = value; }
        }

        /// <summary>
        /// Gets or sets the redo stack.
        /// </summary>
        /// <value>The redo stack.</value>
        internal Stack<object> RedoStack
        {
            get { return this.redocommandstack; }
            set { this.redocommandstack = value; }
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


        internal Canvas SelectionCanvas
        {
            get { return this.maingrid; }
            set { this.maingrid = value; }
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
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        ///    public DiagramControl Control;
        ///    public DiagramModel Model;
        ///    public DiagramView View;
        ///    public MainPage()
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
                return (Page as DiagramPage).SelectionList;
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
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        ///    public DiagramControl Control;
        ///    public DiagramModel Model;
        ///    public DiagramView View;
        ///    public MainPage()
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
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        ///    public DiagramControl Control;
        ///    public DiagramModel Model;
        ///    public DiagramView View;
        ///    public MainPage()
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
        internal bool ShowHorizontalRulers
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
                return this.showRuler;
            }

            set
            {
                this.showRuler = value;
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
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        ///    public DiagramControl Control;
        ///    public DiagramModel Model;
        ///    public DiagramView View;
        ///    public MainPage()
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
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        ///    public DiagramControl Control;
        ///    public DiagramModel Model;
        ///    public DiagramView View;
        ///    public MainPage()
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
        internal bool ShowVerticalRulers
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
        /// Gets or sets a value indicating whether the Undo command is executed.
        /// </summary>
        /// <value><c>true</c> if undone; otherwise, <c>false</c>.</value>
        internal bool Undone
        {
            get { return this.undo; }
            set { this.undo = value; }
        }

        /// <summary>
        /// Gets or sets the undo stack.
        /// </summary>
        /// <value>The undo stack.</value>
        internal Stack<object> UndoStack
        {
            get { return this.undocommandstack; }
            set { this.undocommandstack = value; }
        }

        /// <summary>
        /// Gets or sets the vertical thumb drag offset.
        /// </summary>
        /// <value>The vertical thumb drag offset.</value>
        internal double VerThumbDragOffset
        {
            get { return this.scrollverthumb; }
            set { this.scrollverthumb = value; }
        }

        /// <summary>
        /// Gets or sets the view grid which contains the page.
        /// </summary>
        /// <value>The view grid.</value>
        internal Grid ViewGrid
        {
            get { return this.viewgrid; }
            set { this.viewgrid = value; }
        }

        /// <summary>
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
        /// Gets or sets the X coordinate.
        /// </summary>
        /// <value>The x value</value>
        internal double X
        {
            get { return this.x; }
            set { this.x = value; }
        }

        /// <summary>
        /// Gets or sets the view origin Y coordinate.
        /// </summary>
        /// <value>The Y value.</value>
        internal double Y
        {
            get { return this.y; }
            set { this.y = value; }
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
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        ///    public DiagramControl Control;
        ///    public DiagramModel Model;
        ///    public DiagramView View;
        ///    public MainPage()
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
        /// Converts the value from pixels to the current measurement unit.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The Converted value</returns>
        internal double ConvertValue(double value)
        {
            double b = MeasureUnitsConverter.FromPixels(value, (this.Page as DiagramPage).MeasurementUnits);
            return b;
        }

        internal void DeleteObjects(DiagramView mDiagramView)
        {
            mDiagramView.DeleteCount = 0;
            mDiagramView.IsDeleteCommandExecuted = true;
            CollectionExt.Cleared = false;
            DiagramControl dc = DiagramPage.GetDiagramControl(mDiagramView) as DiagramControl;
            mDiagramView.Deleted = true;
            mDiagramView.DupDeleted = true;
            this.dc = DiagramPage.GetDiagramControl(mDiagramView) as DiagramControl;

            tUndoStack.Push("Stop");
            foreach (UIElement element in mDiagramView.SelectionList)
            {
                if (element is LineConnector)
                {
                    ConnectionDeleteRoutedEventArgs newEventArgs = new ConnectionDeleteRoutedEventArgs(element as LineConnector);
                    dc.View.OnConnectionDeleting(element as LineConnector, newEventArgs);
                    this.dc.View.Islinedeleted = true;
                    this.dc.Model.Connections.Remove(element as LineConnector);
                    this.dc.View.Islinedeleted = false;
                    ConnectionDeleteRoutedEventArgs newEventArgs1 = new ConnectionDeleteRoutedEventArgs(element as LineConnector);
                    dc.View.OnConnectionDeleted(element as LineConnector, newEventArgs1);
                    this.oldselectionlist.Remove(element as LineConnector);

                }

                if (element is Node)
                {

                    NodeDeleteRoutedEventArgs newEventArgs2 = new NodeDeleteRoutedEventArgs(element as Node);
                    dview.OnNodeDeleting(element as Node, newEventArgs2);
                    this.dc.View.Isnodedeleted = true;
                    this.dc.Model.Nodes.Remove(element as Control);
                    this.dc.View.Isnodedeleted = false;
                    NodeDeleteRoutedEventArgs newEventArgs = new NodeDeleteRoutedEventArgs(element as Node);
                    dc.View.OnNodeDeleted(element as Node, newEventArgs);
                    this.oldselectionlist.Remove(element as Node);
                }
            }
            tUndoStack.Push("Start");

            mDiagramView.SelectionList.Clear();
        }

        private void LoadedForUnitChange(object sender, RoutedEventArgs e)
        {
            this.Loaded -= LoadedForUnitChange;
            if ((this.Page as DiagramPage).MeasurementUnits != MeasureUnits.Pixel)
            {
                SnapOffsetX = MeasureUnitsConverter.Convert(SnapOffsetX, MeasureUnits.Pixel, (this.Page as DiagramPage).MeasurementUnits);
                SnapOffsetY = MeasureUnitsConverter.Convert(SnapOffsetY, MeasureUnits.Pixel, (this.Page as DiagramPage).MeasurementUnits);
            }
        }

        /// <summary>
        /// Invoked when the Diagram View is loaded.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void DiagramView_Loaded(object sender, RoutedEventArgs e)
        {
            this.dc = DiagramPage.GetDiagramControl(this);
            dview = this.dc.View;
            if (this.dc.IsUnloaded)
            {
                Scrollviewer = GetTemplateChild("PART_ScrollViewer") as ScrollViewer;
                (this.Page as DiagramPage).HorValue = Scrollviewer.HorizontalOffset / this.CurrentZoom;
                (this.Page as DiagramPage).VerValue = Scrollviewer.VerticalOffset / this.CurrentZoom;
            }

        }

        /// <summary>
        /// Provides class handling for the MouseUp routed event that occurs when the mouse
        /// button  is released and the mouse pointer is over this control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void DiagramView_MouseUp(object sender, MouseButtonEventArgs e)
        {
            foreach (ICommon shape in SelectionList)
            {
                if (shape is Node && !oldselectionlist.Contains(shape as ICommon) && dview.IsPageEditable)
                {
                    NodeRoutedEventArgs newEventArgs = new NodeRoutedEventArgs(shape as Node);
                    dview.OnNodeSelected(shape as Node, newEventArgs);
                }
                if (shape is LineConnector && !oldselectionlist.Contains(shape as ICommon) && dview.IsPageEditable)
                {
                    ConnectorRoutedEventArgs newEventArgs1 = new ConnectorRoutedEventArgs(shape as LineConnector);
                    dview.OnConnectorSelected(shape as LineConnector, newEventArgs1);
                }

            }
            foreach (ICommon shape in oldselectionlist)
            {
                if (!SelectionList.Contains(shape as Node) && dview.IsPageEditable && shape is Node)
                {
                    NodeRoutedEventArgs newEventArgs = new NodeRoutedEventArgs(shape as Node);
                    dview.OnNodeUnSelected(shape as Node, newEventArgs);
                }
                if (!SelectionList.Contains(shape as LineConnector) && dview.IsPageEditable && shape is LineConnector)
                {
                    ConnectorRoutedEventArgs newEventArgs1 = new ConnectorRoutedEventArgs(shape as LineConnector);
                    dview.OnConnectorUnselected(shape as LineConnector, newEventArgs1);
                }
            }
            dview.oldselectionlist.Clear();
            foreach (ICommon item in dview.SelectionList)
            {
                if (item is ICommon)
                {
                    dview.oldselectionlist.Add(item as ICommon);
                }
            }
            this.ReleaseMouseCapture();
            this.IsDragging = false;
            e.Handled = false;
        }


        protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        {

            if (this != null && !Node.flagpopup && !LineConnector.linepopup)
            {
                if (this.IsPageEditable)
                {
                    if (this.ContextMenu != null)
                    {
                        ContextMenu.OpenPopup(e.GetPosition(null));
                    }
                }
            }
            LineConnector.linepopup = false;
            Node.flagpopup = false;
            base.OnMouseRightButtonUp(e);
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
                this.oldselectionlist.Add(item as ICommon);
            }

            this.DupDeleted = false;

            if (this.OldHoroffset > Scrollviewer.HorizontalOffset)
            {
                this.IsScrolledRight = false;
            }
            else
            {
                this.IsScrolledRight = true;
            }

            this.OldHoroffset = Scrollviewer.HorizontalOffset;
            if (this.OldVeroffset > Scrollviewer.VerticalOffset)
            {
                this.IsScrolledBottom = false;
            }
            else
            {
                this.IsScrolledBottom = true;
            }

            this.OldVeroffset = Scrollviewer.VerticalOffset;
            ScrollViewer svr = new ScrollViewer();
            Thumb tb = new Thumb();
            Rectangle rect = new Rectangle();
            Point p = e.GetPosition(this);
            if (e.OriginalSource.GetType() == svr.GetType() && e.OriginalSource.GetType() != rect.GetType())
            {
                this.IsMouseScrolled = false;
                if (!this.IsJustScrolled)
                {
                    this.IsMouseUponly = true;
                    this.IsJustScrolled = true;
                    (this.dc.View.Page as DiagramPage).Dragleft += (this.dc.View.Page as DiagramPage).Minleft;
                    (this.dc.View.Page as DiagramPage).Dragtop += (this.dc.View.Page as DiagramPage).MinTop;
                }

                this.IsScrollCheck = true;
                this.IsDupMousePressed = true;
            }

            this.SelectionList.Clear();
        }

        /// <summary>
        /// Provides class handling for the PreviewMouseWheel routed event that occurs when the mouse
        /// wheel  is moved and the mouse pointer is over this control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseWheelEventArgs"/> instance containing the event data.</param>
        /// 
        private void DiagraView_Mouse(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;

        }
        //private void DiagramView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        //{
        //    this.DupDeleted = false;
        //    this.IsMouseWheeled = true;
        //    this.IsDupMouseWheeled = true;
        //    this.IsDupMousePressed = false;

        //    this.PanConstant = 0;
        //    if (this.IsPageEditable)
        //    {
        //        if (this.ZoomEnabled)
        //        {
        //            if (dview.onReset)
        //            {
        //                dview.onReset = false;
        //            }

        //            double oldzoom = this.CurrentZoom;
        //            if (Keyboard.Modifiers == ModifierKeys.Alt)
        //            {
        //                if (e.Delta > 0)
        //                {
        //                    this.CurrentZoom += this.ZoomFactor;
        //                }
        //                else
        //                {
        //                    this.CurrentZoom -= this.ZoomFactor;
        //                }

        //                if (this.CurrentZoom < .3)
        //                {
        //                    this.CurrentZoom = .3;
        //                }

        //                if (dview.CurrentZoom >= 30)
        //                {
        //                    dview.CurrentZoom = 30;
        //                }

        //                var physicalPoint = e.GetPosition(this);
        //                this.Scrollviewer.ScrollToHorizontalOffset(hf * this.CurrentZoom);
        //                this.Scrollviewer.ScrollToVerticalOffset(vf * this.CurrentZoom);

        //                if (e.Delta > 0)
        //                {
        //                    if (this.X > 0)
        //                    {
        //                        this.X = -Scrollviewer.HorizontalOffset / this.CurrentZoom - (dview.Page as DiagramPage).CurrentMinX;
        //                    }

        //                    this.ViewGridOrigin = new Point(this.X * this.CurrentZoom + xcoordinate, this.Y * this.CurrentZoom + ycoordinate - (dview.Page as DiagramPage).CurrentMinY);
        //                }
        //                else
        //                {
        //                    if (this.Scrollviewer.HorizontalOffset == this.Scrollviewer.ScrollableWidth)
        //                    {
        //                        this.X = (-Math.Abs((this.Page as DiagramPage).ActualWidth * this.CurrentZoom - Scrollviewer.ViewportWidth)) / this.CurrentZoom - (dview.Page as DiagramPage).CurrentMinX;
        //                        this.ViewGridOrigin = new Point(-Math.Abs((this.Page as DiagramPage).ActualWidth * this.CurrentZoom - Scrollviewer.ViewportWidth) + xcoordinate, this.Y * this.CurrentZoom);
        //                    }
        //                    else
        //                    {
        //                        this.ViewGridOrigin = new Point(this.X * this.CurrentZoom + xcoordinate, this.Y * this.CurrentZoom + ycoordinate);
        //                    }

        //                    if (Scrollviewer.VerticalOffset == Scrollviewer.ScrollableHeight && Scrollviewer.VerticalOffset != 0)
        //                    {
        //                        this.Y = (-Math.Abs((this.Page as DiagramPage).ActualHeight * CurrentZoom - Scrollviewer.ViewportHeight)) / CurrentZoom - (dview.Page as DiagramPage).CurrentMinY;
        //                        this.ViewGridOrigin = new Point(ViewGridOrigin.X, -Math.Abs((this.Page as DiagramPage).ActualHeight * CurrentZoom - Scrollviewer.ViewportHeight) + ycoordinate);
        //                    }
        //                    else
        //                    {
        //                        this.ViewGridOrigin = new Point(this.ViewGridOrigin.X, this.Y * this.CurrentZoom + ycoordinate);
        //                    }

        //                    if ((this.Page as DiagramPage).ActualWidth * this.CurrentZoom <= this.Scrollviewer.ViewportWidth)
        //                    {
        //                        this.X = -(dview.Page as DiagramPage).CurrentMinX * this.CurrentZoom;
        //                        this.ViewGridOrigin = new Point(this.X + xcoordinate, this.ViewGridOrigin.Y);
        //                    }

        //                    if ((this.Page as DiagramPage).ActualHeight * this.CurrentZoom <= this.Scrollviewer.ViewportHeight)
        //                    {
        //                        this.Y = -(dview.Page as DiagramPage).CurrentMinY * this.CurrentZoom;
        //                        this.ViewGridOrigin = new Point(this.ViewGridOrigin.X, this.Y + ycoordinate);
        //                    }
        //                }

        //                e.Handled = true;
        //            }
        //        }

        //        e.Handled = false;
        //    }

        //    e.Handled = false;
        //}



        /// <summary>
        /// Handles the Unloaded event of the DiagramView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void DiagramView_Unloaded(object sender, RoutedEventArgs e)
        {
            if (this.Page != null)
            {
                (this.Page as DiagramPage).HorValue = this.Scrollviewer.HorizontalOffset / CurrentZoom;
                (this.Page as DiagramPage).VerValue = this.Scrollviewer.VerticalOffset / CurrentZoom;                
            }
        }

        /// <summary>
        /// Gets the position when the interval is default.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <returns>The position</returns>
        private double GetDefaultPosition(double x)
        {
            double c = 50;// MeasureUnitsConverter.FromPixels(50, (dview.Page as DiagramPage).MeasurementUnits);

            double mul = (50 * x) / c;
            switch ((dview.Page as DiagramPage).MeasurementUnits)
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
            dview = DiagramPage.GetDiagramControl(this).View;
            double mul = 0;
            double c = 50;// MeasureUnitsConverter.FromPixels(50, (dview.Page as DiagramPage).MeasurementUnits);

            mul = dview.pixelvalue / c;
            switch ((dview.Page as DiagramPage).MeasurementUnits)
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
        /// Gets the rounding value for the measurement units.
        /// </summary>
        /// <returns>The rounding value</returns>
        private double GetRounding()
        {
            //double c = 50;// MeasureUnitsConverter.FromPixels(50, (dview.Page as DiagramPage).MeasurementUnits);
            switch ((dview.Page as DiagramPage).MeasurementUnits)
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
        /// Gets the vertical ruler position
        /// </summary>
        /// <returns>The vertical ruler position</returns>
        private double GetVerticalPosition()
        {
            dview = DiagramPage.GetDiagramControl(this).View;
            double mul = 0;
            double c = 50;// MeasureUnitsConverter.FromPixels(50, (dview.Page as DiagramPage).MeasurementUnits);

            mul = dview.pixelvalue / c;
            switch ((dview.Page as DiagramPage).MeasurementUnits)
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
        /// Invoked when MoveUp Command is Executed.
        /// </summary>
        /// <param name="dview">The DiagramView instance.</param>
        public static void MoveDown(DiagramView dview)
        {
            double NudgeY = 1;
            if (dview != null)
            {
                dview.m_IsCommandInProgress = true;
                dview.tUndoStack.Push("Stop");
            }
            if (dview.IsPageEditable)
            {
                foreach (UIElement shape in dview.SelectionList)
                {
                    if (shape is Group)
                    {
                        foreach (INodeGroup n in (shape as Group).NodeChildren)
                        {
                            ProcessEvent(n, 3, dview);
                            if (n is Node)
                            {
                                (n as Node).PxLogicalOffsetY += NudgeY;// MeasureUnitsConverter.FromPixels(1, DiagramPage.Munits);
                                (n as Node).Focus();
                            }
                            else
                            {
                                if ((n as LineConnector).HeadNode == null && (n as LineConnector).TailNode == null)
                                {
                                    (n as LineConnector).PxEndPointPosition = new Point((n as LineConnector).PxEndPointPosition.X, (n as LineConnector).PxEndPointPosition.Y + NudgeY);
                                    (n as LineConnector).PxStartPointPosition = new Point((n as LineConnector).PxStartPointPosition.X, (n as LineConnector).PxStartPointPosition.Y + NudgeY);
                                    (n as LineConnector).UpdateConnectorPathGeometry();
                                    (n as LineConnector).Focus();
                                }
                                else
                                    if ((n as LineConnector).HeadNode != null && (n as LineConnector).TailNode == null)
                                    {
                                        (n as LineConnector).PxEndPointPosition = new Point((n as LineConnector).PxEndPointPosition.X, (n as LineConnector).PxEndPointPosition.Y + NudgeY);
                                        (n as LineConnector).UpdateConnectorPathGeometry();
                                        (n as LineConnector).Focus();
                                    }
                                    else
                                        if ((n as LineConnector).HeadNode == null && (n as LineConnector).TailNode != null)
                                        {
                                            (n as LineConnector).PxStartPointPosition = new Point((n as LineConnector).PxStartPointPosition.X, (n as LineConnector).PxStartPointPosition.Y + NudgeY);
                                            (n as LineConnector).UpdateConnectorPathGeometry();
                                            (n as LineConnector).Focus();
                                        }
                            }

                            (dview.Page as DiagramPage).Ver = -dview.Scrollviewer.VerticalOffset / dview.CurrentZoom;
                            (dview.Page as DiagramPage).InvalidateMeasure();
                            (dview.Page as DiagramPage).InvalidateArrange();
                        }
                    }
                    else
                    {
                        ProcessEvent(shape, 3, dview);
                        if (shape is Node)
                        {
                            (shape as Node).PxLogicalOffsetY += NudgeY;// MeasureUnitsConverter.FromPixels(1, DiagramPage.Munits);
                            (shape as Node).Focus();
                        }
                        else
                        {
                            if ((shape as LineConnector).HeadNode == null && (shape as LineConnector).TailNode == null)
                            {
                                (shape as LineConnector).PxEndPointPosition = new Point((shape as LineConnector).PxEndPointPosition.X, (shape as LineConnector).PxEndPointPosition.Y + NudgeY);
                                (shape as LineConnector).PxStartPointPosition = new Point((shape as LineConnector).PxStartPointPosition.X, (shape as LineConnector).PxStartPointPosition.Y + NudgeY);
                                (shape as LineConnector).UpdateConnectorPathGeometry();
                                (shape as LineConnector).Focus();
                            }
                            else
                                if ((shape as LineConnector).HeadNode != null && (shape as LineConnector).TailNode == null)
                                {
                                    (shape as LineConnector).PxEndPointPosition = new Point((shape as LineConnector).PxEndPointPosition.X, (shape as LineConnector).PxEndPointPosition.Y + NudgeY);
                                    (shape as LineConnector).UpdateConnectorPathGeometry();
                                    (shape as LineConnector).Focus();
                                }
                                else
                                    if ((shape as LineConnector).HeadNode == null && (shape as LineConnector).TailNode != null)
                                    {
                                        (shape as LineConnector).PxStartPointPosition = new Point((shape as LineConnector).PxStartPointPosition.X, (shape as LineConnector).PxStartPointPosition.Y + NudgeY);
                                        (shape as LineConnector).UpdateConnectorPathGeometry();
                                        (shape as LineConnector).Focus();
                                    }
                        }

                        (dview.Page as DiagramPage).Ver = -dview.Scrollviewer.VerticalOffset / dview.CurrentZoom;
                        (dview.Page as DiagramPage).InvalidateMeasure();
                        (dview.Page as DiagramPage).InvalidateArrange();
                    }
                }
            }
            if (dview != null)
            {
                (dview.Page as DiagramPage).UpdateLayout();
                dview.m_IsCommandInProgress = false;
                dview.tUndoStack.Push("Start");
            }
        }

        /// <summary>
        /// Invoked when MoveUp Command is Executed.
        /// </summary>
        /// <param name="dview">The DiagramView instance.</param>
        public static void MoveLeft(DiagramView dview)
        {
            double NudgeX = 1;
            if (dview != null)
            {
                dview.m_IsCommandInProgress = true;
                dview.tUndoStack.Push("Stop");
            }
            if (dview.IsPageEditable)
            {
                foreach (UIElement shape in dview.SelectionList)
                {
                    if (shape is Group)
                    {
                        foreach (INodeGroup n in (shape as Group).NodeChildren)
                        {
                            ProcessEvent(n, 4, dview);
                            if (n is Node)
                            {
                                (n as Node).PxLogicalOffsetX -= NudgeX;// MeasureUnitsConverter.FromPixels(1, DiagramPage.Munits);
                                (n as Node).Focus();
                            }
                            else
                            {
                                if ((n as LineConnector).HeadNode == null && (n as LineConnector).TailNode == null)
                                {
                                    (n as LineConnector).PxEndPointPosition = new Point((n as LineConnector).PxEndPointPosition.X - NudgeX, (n as LineConnector).PxEndPointPosition.Y);
                                    (n as LineConnector).PxStartPointPosition = new Point((n as LineConnector).PxStartPointPosition.X - NudgeX, (n as LineConnector).PxStartPointPosition.Y);
                                    (n as LineConnector).UpdateConnectorPathGeometry();
                                    (n as LineConnector).Focus();
                                }
                                else
                                    if ((n as LineConnector).HeadNode != null && (n as LineConnector).TailNode == null)
                                    {
                                        (n as LineConnector).PxEndPointPosition = new Point((n as LineConnector).PxEndPointPosition.X - NudgeX, (n as LineConnector).PxEndPointPosition.Y);
                                        (n as LineConnector).UpdateConnectorPathGeometry();
                                        (n as LineConnector).Focus();
                                    }
                                    else
                                        if ((n as LineConnector).HeadNode == null && (n as LineConnector).TailNode != null)
                                        {
                                            (n as LineConnector).PxStartPointPosition = new Point((n as LineConnector).PxStartPointPosition.X - NudgeX, (n as LineConnector).PxStartPointPosition.Y);
                                            (n as LineConnector).UpdateConnectorPathGeometry();
                                            (n as LineConnector).Focus();
                                        }
                            }

                            (dview.Page as DiagramPage).Hor = -dview.Scrollviewer.HorizontalOffset / dview.CurrentZoom;
                            (dview.Page as DiagramPage).InvalidateMeasure();
                            (dview.Page as DiagramPage).InvalidateArrange();
                        }
                    }
                    else
                    {
                        ProcessEvent(shape, 4, dview);
                        if (shape is Node)
                        {
                            (shape as Node).PxLogicalOffsetX -= NudgeX;// MeasureUnitsConverter.FromPixels(1, DiagramPage.Munits);
                            (shape as Node).Focus();
                        }
                        else
                        {
                            if ((shape as LineConnector).HeadNode == null && (shape as LineConnector).TailNode == null)
                            {
                                (shape as LineConnector).PxEndPointPosition = new Point((shape as LineConnector).PxEndPointPosition.X - NudgeX, (shape as LineConnector).PxEndPointPosition.Y);
                                (shape as LineConnector).PxStartPointPosition = new Point((shape as LineConnector).PxStartPointPosition.X - NudgeX, (shape as LineConnector).PxStartPointPosition.Y);
                                (shape as LineConnector).UpdateConnectorPathGeometry();
                                (shape as LineConnector).Focus();
                            }
                            else
                                if ((shape as LineConnector).HeadNode != null && (shape as LineConnector).TailNode == null)
                                {
                                    (shape as LineConnector).PxEndPointPosition = new Point((shape as LineConnector).PxEndPointPosition.X - NudgeX, (shape as LineConnector).PxEndPointPosition.Y);
                                    (shape as LineConnector).UpdateConnectorPathGeometry();
                                    (shape as LineConnector).Focus();
                                }
                                else
                                    if ((shape as LineConnector).HeadNode == null && (shape as LineConnector).TailNode != null)
                                    {
                                        (shape as LineConnector).PxStartPointPosition = new Point((shape as LineConnector).PxStartPointPosition.X - NudgeX, (shape as LineConnector).PxStartPointPosition.Y);
                                        (shape as LineConnector).UpdateConnectorPathGeometry();
                                        (shape as LineConnector).Focus();
                                    }
                        }

                        (dview.Page as DiagramPage).Hor = -dview.Scrollviewer.HorizontalOffset / dview.CurrentZoom;
                        (dview.Page as DiagramPage).InvalidateMeasure();
                        (dview.Page as DiagramPage).InvalidateArrange();
                    }
                }
            }
            if (dview != null)
            {
                dview.m_IsCommandInProgress = false;
                dview.tUndoStack.Push("Start");
            }
        }

        /// <summary>
        /// Invoked when MoveRight Command is Executed.
        /// </summary>
        /// <param name="dview">The DiagramView instance.</param>
        public static void MoveRight(DiagramView dview)
        {
            double NudgeX = 1;
            if (dview != null)
            {
                dview.m_IsCommandInProgress = true;
                dview.tUndoStack.Push("Stop");
            }
            if (dview.IsPageEditable)
            {
                foreach (UIElement shape in dview.SelectionList)
                {
                    if (shape is Group)
                    {
                        foreach (INodeGroup n in (shape as Group).NodeChildren)
                        {
                            ProcessEvent(n, 2, dview);
                            if (n is Node)
                            {
                                (n as Node).PxLogicalOffsetX += NudgeX;// MeasureUnitsConverter.FromPixels(1, DiagramPage.Munits);
                                (n as Node).Focus();
                            }
                            else
                            {
                                if ((n as LineConnector).HeadNode == null && (n as LineConnector).TailNode == null)
                                {
                                    (n as LineConnector).PxEndPointPosition = new Point((n as LineConnector).PxEndPointPosition.X + NudgeX, (n as LineConnector).PxEndPointPosition.Y);
                                    (n as LineConnector).PxStartPointPosition = new Point((n as LineConnector).PxStartPointPosition.X + NudgeX, (n as LineConnector).PxStartPointPosition.Y);
                                    (n as LineConnector).UpdateConnectorPathGeometry();
                                    (n as LineConnector).Focus();
                                }
                                else
                                    if ((n as LineConnector).HeadNode != null && (n as LineConnector).TailNode == null)
                                    {
                                        (n as LineConnector).PxEndPointPosition = new Point((n as LineConnector).PxEndPointPosition.X + NudgeX, (n as LineConnector).PxEndPointPosition.Y);
                                        (n as LineConnector).UpdateConnectorPathGeometry();
                                        (n as LineConnector).Focus();
                                    }
                                    else
                                        if ((n as LineConnector).HeadNode == null && (n as LineConnector).TailNode != null)
                                        {
                                            (n as LineConnector).PxStartPointPosition = new Point((n as LineConnector).PxStartPointPosition.X + NudgeX, (n as LineConnector).PxStartPointPosition.Y);
                                            (n as LineConnector).UpdateConnectorPathGeometry();
                                            (n as LineConnector).Focus();
                                        }
                            }

                            (dview.Page as DiagramPage).Hor = -dview.Scrollviewer.HorizontalOffset / dview.CurrentZoom;
                            (dview.Page as DiagramPage).InvalidateMeasure();
                            (dview.Page as DiagramPage).InvalidateArrange();
                        }
                    }
                    else
                    {
                        ProcessEvent(shape, 2, dview);
                        if (shape is Node)
                        {
                            (shape as Node).PxLogicalOffsetX += NudgeX;// MeasureUnitsConverter.FromPixels(1, DiagramPage.Munits);
                            (shape as Node).Focus();
                        }
                        else
                        {
                            if ((shape as LineConnector).HeadNode == null && (shape as LineConnector).TailNode == null)
                            {
                                (shape as LineConnector).PxEndPointPosition = new Point((shape as LineConnector).PxEndPointPosition.X + NudgeX, (shape as LineConnector).PxEndPointPosition.Y);
                                (shape as LineConnector).PxStartPointPosition = new Point((shape as LineConnector).PxStartPointPosition.X + NudgeX, (shape as LineConnector).PxStartPointPosition.Y);
                                (shape as LineConnector).UpdateConnectorPathGeometry();
                                (shape as LineConnector).Focus();
                            }
                            else
                                if ((shape as LineConnector).HeadNode != null && (shape as LineConnector).TailNode == null)
                                {
                                    (shape as LineConnector).PxEndPointPosition = new Point((shape as LineConnector).PxEndPointPosition.X + NudgeX, (shape as LineConnector).PxEndPointPosition.Y);
                                    (shape as LineConnector).UpdateConnectorPathGeometry();
                                    (shape as LineConnector).Focus();
                                }
                                else
                                    if ((shape as LineConnector).HeadNode == null && (shape as LineConnector).TailNode != null)
                                    {
                                        (shape as LineConnector).PxStartPointPosition = new Point((shape as LineConnector).PxStartPointPosition.X + NudgeX, (shape as LineConnector).PxStartPointPosition.Y);
                                        (shape as LineConnector).UpdateConnectorPathGeometry();
                                        (shape as LineConnector).Focus();
                                    }
                        }

                        (dview.Page as DiagramPage).Hor = -dview.Scrollviewer.HorizontalOffset / dview.CurrentZoom;
                        (dview.Page as DiagramPage).InvalidateMeasure();
                        (dview.Page as DiagramPage).InvalidateArrange();
                    }
                }
            }
            if (dview != null)
            {
                dview.m_IsCommandInProgress = false;
                dview.tUndoStack.Push("Start");
            }
        }

        /// <summary>
        /// Invoked when MoveUp Command is Executed.
        /// </summary>
        /// <param name="dview">The DiagramView instance.</param>
        public static void MoveUp(DiagramView dview)
        {
            double NudgeY = 1;
            if (dview != null)
            {
                dview.m_IsCommandInProgress = true;
                dview.tUndoStack.Push("Stop");
            }
            if (dview.IsPageEditable)
            {

                foreach (UIElement shape in dview.SelectionList)
                {
                    if (shape is Group)
                    {
                        foreach (INodeGroup n in (shape as Group).NodeChildren)
                        {
                            ProcessEvent(n, 1, dview);
                            if (n is Node)
                            {
                                (n as Node).PxLogicalOffsetY -= NudgeY;// MeasureUnitsConverter.FromPixels(1, DiagramPage.Munits);
                                (n as Node).Focus();
                            }
                            else
                            {
                                if ((n as LineConnector).HeadNode == null && (n as LineConnector).TailNode == null)
                                {
                                    (n as LineConnector).PxEndPointPosition = new Point((n as LineConnector).PxEndPointPosition.X, (n as LineConnector).PxEndPointPosition.Y - NudgeY);
                                    (n as LineConnector).PxStartPointPosition = new Point((n as LineConnector).PxStartPointPosition.X, (n as LineConnector).PxStartPointPosition.Y - NudgeY);
                                    (n as LineConnector).UpdateConnectorPathGeometry();
                                    (n as LineConnector).Focus();
                                }
                                else if ((n as LineConnector).HeadNode != null && (n as LineConnector).TailNode == null)
                                {
                                    (n as LineConnector).PxEndPointPosition = new Point((n as LineConnector).PxEndPointPosition.X, (n as LineConnector).PxEndPointPosition.Y - NudgeY);
                                    (n as LineConnector).UpdateConnectorPathGeometry();
                                    (n as LineConnector).Focus();
                                }
                                else if ((n as LineConnector).HeadNode == null && (n as LineConnector).TailNode != null)
                                {
                                    (n as LineConnector).PxStartPointPosition = new Point((n as LineConnector).PxStartPointPosition.X, (n as LineConnector).PxStartPointPosition.Y - NudgeY);
                                    (n as LineConnector).UpdateConnectorPathGeometry();
                                    (n as LineConnector).Focus();
                                }
                            }

                            (dview.Page as DiagramPage).Ver = -dview.Scrollviewer.VerticalOffset / dview.CurrentZoom;
                            (dview.Page as DiagramPage).InvalidateMeasure();
                            (dview.Page as DiagramPage).InvalidateArrange();
                        }
                    }
                    else
                    {
                        ProcessEvent(shape, 1, dview);
                        if (shape is Node)
                        {
                            (shape as Node).PxLogicalOffsetY -= NudgeY;// MeasureUnitsConverter.FromPixels(1, DiagramPage.Munits);
                            (shape as Node).Focus();
                        }
                        else
                        {
                            if ((shape as LineConnector).HeadNode == null && (shape as LineConnector).TailNode == null)
                            {
                                (shape as LineConnector).PxEndPointPosition = new Point((shape as LineConnector).PxEndPointPosition.X, (shape as LineConnector).PxEndPointPosition.Y - NudgeY);
                                (shape as LineConnector).PxStartPointPosition = new Point((shape as LineConnector).PxStartPointPosition.X, (shape as LineConnector).PxStartPointPosition.Y - NudgeY);
                                (shape as LineConnector).UpdateConnectorPathGeometry();
                                (shape as LineConnector).Focus();
                            }
                            else if ((shape as LineConnector).HeadNode != null && (shape as LineConnector).TailNode == null)
                            {
                                (shape as LineConnector).PxEndPointPosition = new Point((shape as LineConnector).PxEndPointPosition.X, (shape as LineConnector).PxEndPointPosition.Y - NudgeY);
                                (shape as LineConnector).UpdateConnectorPathGeometry();
                                (shape as LineConnector).Focus();
                            }
                            else if ((shape as LineConnector).HeadNode == null && (shape as LineConnector).TailNode != null)
                            {
                                (shape as LineConnector).PxStartPointPosition = new Point((shape as LineConnector).PxStartPointPosition.X, (shape as LineConnector).PxStartPointPosition.Y - NudgeY);
                                (shape as LineConnector).UpdateConnectorPathGeometry();
                                (shape as LineConnector).Focus();
                            }
                        }

                        (dview.Page as DiagramPage).Ver = -dview.Scrollviewer.VerticalOffset / dview.CurrentZoom;
                        (dview.Page as DiagramPage).InvalidateMeasure();
                        (dview.Page as DiagramPage).InvalidateArrange();
                    }
                }
            }
            if (dview != null)
            {
                dview.m_IsCommandInProgress = false;
                dview.tUndoStack.Push("Start");
            }
        }

        public static void SelectAll(DiagramView mDiagramView)
        {

            if (mDiagramView != null)
            {
                DiagramControl dc = DiagramPage.GetDiagramControl(mDiagramView) as DiagramControl;
                if (dc.View.IsPageEditable)
                {
                    foreach (Node node in dc.Model.Nodes)
                    {
                        if (node.AllowSelect)
                        {
                           // node.IsSelected = true;
                            mDiagramView.SelectionList.Add(node);
                            mDiagramView.oldselectionlist.Add(node);
                        }
                    }
                    foreach (LineConnector line in dc.Model.Connections)
                    {
                        //line.IsSelected = true;
                        mDiagramView.SelectionList.Add(line);
                        mDiagramView.oldselectionlist.Add(line);

                    }
                }
            }
        }

        internal MeasureUnits MeasurementUnits
        {
            get
            {
                return (MeasureUnits)GetValue(MeasurementUnitsProperty);
            }

            set
            {
                SetValue(MeasurementUnitsProperty, value);
            }
        }

        public static readonly DependencyProperty MeasurementUnitsProperty = DependencyProperty.Register("MeasurementUnits", typeof(MeasureUnits), typeof(DiagramView), new PropertyMetadata(MeasureUnits.Pixel, new PropertyChangedCallback(OnUnitsChanged)));

        private static void OnUnitsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramView view = d as DiagramView;
            if (view != null && view.m_IsPixelDefultUnit)
            {
                view.Bounds = MeasureUnitsConverter.Convert(view.Bounds, (MeasureUnits)e.OldValue, (MeasureUnits)e.NewValue);
            }
            view.m_IsPixelDefultUnit = true;
        }

        private bool m_IsPixelDefultUnit = true;

        /// <summary>
        /// Invoked whenever application code or internal processes call
        /// <see cref="System.Windows.FrameworkElement.ApplyTemplate"/> method.
        /// </summary>
        public override void OnApplyTemplate()
        {
            DiagramControl d = DiagramPage.GetDiagramControl(this);
            scrollview = GetTemplateChild("PART_ScrollViewer") as ScrollViewer;
            viewgrid = GetTemplateChild("viewgrid") as Grid;
            SelectionCanvas = GetTemplateChild("PART_Adorner") as Canvas;

            if (this.Page != null)
            {
                if (this.MeasurementUnits != (Page as DiagramPage).MeasurementUnits)
                {
                    m_IsPixelDefultUnit = false;
                }
                System.Windows.Data.Binding measure = new System.Windows.Data.Binding("MeasurementUnits");
                measure.Source = Page;
                this.SetBinding(DiagramView.MeasurementUnitsProperty, measure);
            }

            DiagramView dv = this;
            Border bor = dv.GetTemplateChild("PART_HorizontalRuler") as Border;
            Ruler rul = this.HorizontalRuler as Ruler;
            ContentPresenter cp = bor.Child as ContentPresenter;
            if (bor != null)
            {
                if (rul != null)
                {
                    if (scrollview != null)
                    {
                        rul.sv = scrollview;
                    }
                    if (string.IsNullOrEmpty(rul.Name))
                    {
                        rul.Name = "HRuler";
                    }
                    if (rul.Height.Equals(double.NaN))
                    {
                        rul.Height = 25;
                    }
                    cp.Content = rul;
                }
            }

            rul = this.VerticalRuler as Ruler;
            bor = dv.GetTemplateChild("PART_VerticalRuler") as Border;
            if (bor != null)
            {
                cp = bor.Child as ContentPresenter;
                LayoutTransforUtil lt = new LayoutTransforUtil();
                if (rul != null)
                {
                    if (scrollview != null)
                    {
                        rul.sv = scrollview;
                    }
                    if (string.IsNullOrEmpty(rul.Name))
                    {
                        rul.Name = "VRuler";
                    }
                    if (rul.Height.Equals(double.NaN))
                    {
                        rul.Height = 25;
                    }
                    lt.Content = rul;
                    cp.Content = lt;
                }
            }

            //Border bor = GetTemplateChild("PART_HorizontalRuler") as Border;
            //ContentPresenter cp = bor.Child as ContentPresenter;
            //LayoutTransforUtil test = new LayoutTransforUtil();
            //test.angle = 0;
            //Ruler rul = new HorizontalRuler("HRuler") { Height = 25 };
            //test.Content = rul;
            ////LayoutTransforUtil hr = new LayoutTransforUtil();
            ////hr.Children.Add(new HorizontalRuler("VRuler") { Height = 50 });
            ////VerticalRuler hr = new VerticalRuler(); hr.Height = 50; //hr.Orientation = Orientation.Vertical;
            //cp.Content = rul;

            //bor = GetTemplateChild("PART_VerticalRuler") as Border;
            //cp = bor.Child as ContentPresenter;
            //LayoutTransforUtil lt = new LayoutTransforUtil();
            //lt.Content = (new VerticalRuler("VRuler") { Height = 25 });
            ////VerticalRuler hr = new VerticalRuler(); hr.Height = 50; //hr.Orientation = Orientation.Vertical;
            //cp.Content = lt;

            base.OnApplyTemplate();
            this.translateTransform = new TranslateTransform();
            this.zoomTransform = new ScaleTransform();
            this.transformGroup = new TransformGroup();
            this.MouseLeftButtonUp += new MouseButtonEventHandler(DiagramView_MouseUp);
            this.MouseRightButtonDown += new MouseButtonEventHandler(DiagramView_MouseRightButtonDown);
            this.MouseWheel += new MouseWheelEventHandler(DiagraView_Mouse);
        }

        public void PrintPreview()
        {
            p.Show();
            p.UpdatePageCount();
            p.SetPage();
            
        }
        public void PrintDialog()
        {
            DiagramPrintDialog._printDocument.Print(DiagramPrintDialog.DocumentName);
           // this.DiagramPrintDialog._printDocument.Print("Silverlight Print Page");
        }
        void DiagramView_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            foreach (ICommon shape in SelectionList)
            {
                if (shape is Node && !oldselectionlist.Contains(shape as IShape))
                {
                    NodeRoutedEventArgs newEventArgs = new NodeRoutedEventArgs(shape as Node);
                    dview.OnNodeSelected(shape as Node, newEventArgs);
                }
                if (shape is LineConnector && !oldselectionlist.Contains(shape as ICommon))
                {
                    ConnectorRoutedEventArgs newEventArgs1 = new ConnectorRoutedEventArgs(shape as LineConnector);
                    dview.OnConnectorSelected(shape as LineConnector, newEventArgs1);
                }

            }
            foreach (ICommon shape in oldselectionlist)
            {
                if (shape is Node && !SelectionList.Contains(shape as Node))
                {
                    NodeRoutedEventArgs newEventArgs = new NodeRoutedEventArgs(shape as Node);
                    dview.OnNodeUnSelected(shape as Node, newEventArgs);
                }
                if (!SelectionList.Contains(shape as LineConnector) && dview.IsPageEditable && shape is LineConnector)
                {
                    ConnectorRoutedEventArgs newEventArgs1 = new ConnectorRoutedEventArgs(shape as LineConnector);
                    dview.OnConnectorUnselected(shape as LineConnector, newEventArgs1);
                }
            }

            dview.oldselectionlist.Clear();
            foreach (ICommon item in dview.SelectionList)
            {
                if (item is ICommon)
                {
                    dview.oldselectionlist.Add(item as ICommon);
                }
            }
            this.ReleaseMouseCapture();
            this.IsDragging = false;
            e.Handled = true;
        }




        /// <summary>
        /// Raises the <see cref="E:System.Windows.UIElement.Drop"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.DragEventArgs"/> that contains the event data.</param>
        protected override void OnDrop(DragEventArgs e)
        {


        }


        private static void OnVerticalRulerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramView dv = d as DiagramView;
            Border bor;
            ContentPresenter cp;
            Ruler rul = e.NewValue as Ruler;
            bor = dv.GetTemplateChild("PART_VerticalRuler") as Border;
            if (bor != null)
            {
                cp = bor.Child as ContentPresenter;
                if (rul != null)
                {
                    if (dv.Scrollviewer != null)
                    {
                        rul.sv = dv.Scrollviewer;
                    }
                    if (string.IsNullOrEmpty(rul.Name))
                    {
                        rul.Name = "VRuler";
                    }
                    if (rul.Height.Equals(double.NaN))
                    {
                        rul.Height = 25;
                    }
                    LayoutTransforUtil lt = new LayoutTransforUtil();
                    lt.Content = rul;
                    cp.Content = lt;
                }
                else
                {
                    cp.Content = null;
                }
            }
        }

        private static void OnHorizontalRulerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramView dv = d as DiagramView;
            Ruler rul = e.NewValue as Ruler;
            Border bor = dv.GetTemplateChild("PART_HorizontalRuler") as Border;
            if (bor != null)
            {
                ContentPresenter cp = bor.Child as ContentPresenter;
                if (rul != null)
                {
                    if (dv.Scrollviewer != null)
                    {
                        rul.sv = dv.Scrollviewer;
                    }
                    if (string.IsNullOrEmpty(rul.Name))
                    {
                        rul.Name = "HRuler";
                    }
                    if (rul.Height.Equals(double.NaN))
                    {
                        rul.Height = 25;
                    }
                    cp.Content = rul;
                }
                else
                {
                    cp.Content = null;
                }
            }
        }
        /// <summary>
        /// Called when UndoRedoEnabled Property changed
        /// </summary>
        /// <param name="d">Dependency object, the change occurs on.</param>
        /// <param name="e">Property change details, such as old value and new value. <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnUndoRedoEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool result = (bool)e.NewValue;
            StackExt<object> obj = (d as DiagramView).tUndoStack;
            if (obj != null)
            {
                if (result)
                {
                    obj.m_CanPush = true;
                }
                else
                {
                    obj.m_CanPush = false;
                }
            }
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

        /// <summary>
        /// Calls OnGridHOffsetChanged method of the instance, notifies of the dependency property value changes.
        /// </summary>
        /// <param name="d">Dependency object, the change occurs on.</param>
        /// <param name="e">Property change details, such as old value and new value.</param>
        private static void OnGridHOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramView view = (DiagramView)d;

        }

        /// <summary>
        /// Calls OnGridVOffsetChanged method of the instance, notifies of the dependency property value changes.
        /// </summary>
        /// <param name="d">Dependency object, the change occurs on.</param>
        /// <param name="e">Property change details, such as old value and new value.</param>
        private static void OnGridVOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramView view = (DiagramView)d;

        }

        /// <summary>
        /// Calls OnHorizontalLineStyleChanged method of the instance, notifies of the dependency property value changes.
        /// </summary>
        /// <param name="d">Dependency object, the change occurs on.</param>
        /// <param name="e">Property change details, such as old value and new value.</param>
        private static void OnHorizontalLineStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramView view = (DiagramView)d;

        }

        /// <summary>
        /// Called when [horizontal scroll bar visibility changed].
        /// </summary>
        /// <param name="d">The DependencyObject.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnHorizontalScrollBarVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
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
        }

        private static void OnPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramView view = (DiagramView)d;
            view.DiagramPrintDialog._Visual =(DiagramPage)e.NewValue;
        }

        /// <summary>
        /// Raised when the appropriate property changes.
        /// </summary>
        /// <param name="name">The property name</param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
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
        }

        /// <summary>
        /// Calls OnShowHLineChanged method of the instance, notifies of the dependency property value changes.
        /// </summary>
        /// <param name="d">Dependency object, the change occurs on.</param>
        /// <param name="e">Property change details, such as old value and new value.</param>
        private static void OnShowHLineChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramView view = (DiagramView)d;
            if (view.IsPageEditable)
            {
            }
        }

        /// <summary>
        /// Calls OnShowHRulerChanged method of the instance, notifies of the dependency property value changes.
        /// </summary>
        /// <param name="d">Dependency object, the change occurs on.</param>
        /// <param name="e">Property change details, such as old value and new value.</param>
        private static void OnShowHRulerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        /// <summary>
        /// Calls OnShowVLineChanged method of the instance, notifies of the dependency property value changes.
        /// </summary>
        /// <param name="d">Dependency object, the change occurs on.</param>
        /// <param name="e">Property change details, such as old value and new value.</param>
        private static void OnShowVLineChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramView view = (DiagramView)d;
            if (view.IsPageEditable)
            {
            }
        }

        /// <summary>
        /// Calls OnShowVRulerChanged method of the instance, notifies of the dependency property value changes.
        /// </summary>
        /// <param name="d">Dependency object, the change occurs on.</param>
        /// <param name="e">Property change details, such as old value and new value.</param>
        private static void OnShowVRulerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        /// <summary>
        /// Calls OnVerticalLineStyleChanged method of the instance, notifies of the dependency property value changes.
        /// </summary>
        /// <param name="d">Dependency object, the change occurs on.</param>
        /// <param name="e">Property change details, such as old value and new value.</param>
        private static void OnVerticalLineStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramView view = (DiagramView)d;
        }

        /// <summary>
        /// Called when [vertical scroll bar visibility changed].
        /// </summary>
        /// <param name="d">The DependencyObject.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnVerticalScrollBarVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
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
        }

        /// <summary>
        /// Calls OnZoomFactorChanged method of the instance, notifies of the dependency property value changes.
        /// </summary>
        /// <param name="d">Dependency object, the change occurs on.</param>
        /// <param name="e">Property change details, such as old value and new value.</param>
        private static void OnZoomFactorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramView view = (DiagramView)d;
        }

        private static void ProcessEvent(object n, int p, DiagramView dview)
        {
            if (n is Node)
            {
                NodeNudgeEventArgs newEventArgs = new NodeNudgeEventArgs(n as Node, p);
                dview.OnNodeMoved(n as Node, newEventArgs);
            }
            else if (n is LineConnector)
            {
                if ((n as LineConnector).HeadNode == null || (n as LineConnector).TailNode == null)
                {
                    LineNudgeEventArgs newEventArgs = new LineNudgeEventArgs(n as LineConnector);
                    dview.OnLineMoved(n as LineConnector, newEventArgs);
                }
            }
        }

        /// <summary>
        /// Invoked when Reset Command is Executed.
        /// </summary>
        /// <param name="dview">The diagramview instance.</param>
        public static void Reset(DiagramView dview)
        {
            dview.onReset = true;
            if (dview.IsPageEditable)
            {
                dview.Scrollviewer.ScrollToHorizontalOffset(0);
                dview.Scrollviewer.ScrollToVerticalOffset(0);
                dview.CurrentZoom = 1;


                dview.zoomTransform.ScaleX = dview.CurrentZoom;
                dview.zoomTransform.ScaleY = dview.CurrentZoom;

                dview.Page.RenderTransform = dview.zoomTransform;
                dview.Page.InvalidateArrange();
                dview.Page.InvalidateMeasure();


            }
        }

        /// <summary>
        /// Scrolls to horizontal offset.
        /// </summary>
        /// <param name="value">The value to scroll to.</param>
        internal void ScrollToHorizontalOffset(double value)
        {
            double h1 = this.Scrollviewer.HorizontalOffset / this.CurrentZoom + this.Scrollviewer.ViewportWidth / this.CurrentZoom - value;
            double h2 = this.Scrollviewer.HorizontalOffset / this.CurrentZoom + Math.Abs(h1);
            double vv = -this.ViewGridOrigin.X / this.CurrentZoom + this.Scrollviewer.ViewportWidth / this.CurrentZoom;
            if (this.ViewGridOrigin.X > 0)
            {
                h2 = value - vv + this.Scrollviewer.HorizontalOffset / this.CurrentZoom;
            }

            double vvend = -this.ViewGridOrigin.X / this.CurrentZoom + this.Scrollviewer.ViewportWidth / this.CurrentZoom;

            if (this.ViewGridOrigin.X > 0)
            {
                h2 = value - vv + this.Scrollviewer.HorizontalOffset / this.CurrentZoom;
            }

            double ccc = this.Scrollviewer.ScrollableWidth;
            double cc = this.oldxviewgrid - this.ViewGridOrigin.X;
            double a = cc + this.Scrollviewer.ViewportWidth / this.CurrentZoom;
            double sw = this.Scrollviewer.ExtentWidth;
            double vvcheck = -this.ViewGridOrigin.X / this.CurrentZoom + this.Scrollviewer.ViewportWidth;

        }

        /// <summary>
        /// Scrolls to the specified node.
        /// </summary>
        /// <param name="node">The node object.</param>
        public void ScrollToNode(Node node)
        {
        }

        /// <summary>
        /// Scrolls to vertical offset.
        /// </summary>
        /// <param name="value">The value to scroll to.</param>
        internal void ScrollToVerticalOffset(double value)
        {
            double h1 = this.Scrollviewer.VerticalOffset / this.CurrentZoom + this.Scrollviewer.ViewportHeight / this.CurrentZoom - value;
            double h2 = this.Scrollviewer.VerticalOffset / this.CurrentZoom + Math.Abs(h1);
            double vv = -this.ViewGridOrigin.Y / this.CurrentZoom + this.Scrollviewer.ViewportHeight / this.CurrentZoom;
            double vvcheck = -this.ViewGridOrigin.Y / this.CurrentZoom + this.Scrollviewer.ViewportHeight;
            if (this.ViewGridOrigin.Y > 0)
            {
                h2 = value - vv + this.Scrollviewer.VerticalOffset / this.CurrentZoom;
            }

            double cccc = this.Scrollviewer.ScrollableHeight;
            double cc = this.oldyviewgrid + Y;
            double a = cc + this.Scrollviewer.ViewportHeight / this.CurrentZoom;
            double vvend = -this.ViewGridOrigin.Y / this.CurrentZoom + this.Scrollviewer.ViewportHeight / this.CurrentZoom;

        }

        /// <summary>
        /// Updates the rulers.
        /// </summary>
        /// <param name="view">DiagramView instance</param>
        public void UpdateRuler(DiagramView view)
        {
        }

        /// <summary>
        /// Invoked when ZoomIn Command is Executed.
        /// </summary>
        /// <param name="dview">The diagramview instance.</param>




        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (IsZoomEnabled)
            {
                if (Keyboard.Modifiers == ModifierKeys.Control)
                {
                    if (e.Delta > 0)
                    {
                        dc.ZoomIn.Execute(dview); e.Handled = true;
                    }
                    else
                    {
                        dc.ZoomOut.Execute(dview); e.Handled = true;
                    }
                }
            }
            //this.DupDeleted = false;
            //this.IsMouseWheeled = true;
            //this.IsDupMouseWheeled = true;
            //this.IsDupMousePressed = false;

            //this.PanConstant = 0;
            //if (Keyboard.Modifiers == ModifierKeys.Control)
            //{
            //    if (e.Delta > 0)
            //    {
            //        if (this.IsPageEditable)
            //        {
            //            if (this.ZoomEnabled)
            //            {
            //                if (dview.onReset)
            //                {
            //                    dview.onReset = false;
            //                }
            //            }

            //            if (dview.onReset)
            //            {
            //                dview.CurrentZoom = 1;
            //                dview.onReset = false;
            //            }

            //            dview.CurrentZoom += dview.ZoomFactor;
            //            if (dview.CurrentZoom >= 20)
            //            {
            //                dview.CurrentZoom = 20;
            //            }

            //            dview.zoomTransform.ScaleX = dview.CurrentZoom;
            //            dview.zoomTransform.ScaleY = dview.CurrentZoom;
            //            dview.Page.RenderTransform = dview.zoomTransform;
            //            dview.Page.InvalidateArrange();
            //            dview.Page.InvalidateMeasure();

            //            //dview.ViewGrid.RenderTransform = dview.zoomTransform;



            //        }
            //    }
            //    else
            //    {
            //        if (dview.IsPageEditable)
            //        {

            //            if (dview.onReset)
            //            {
            //                dview.CurrentZoom = 1;
            //                dview.onReset = false;
            //            }

            //            dview.CurrentZoom -= dview.ZoomFactor;
            //            if (dview.CurrentZoom <= 0.3)
            //            {
            //                dview.CurrentZoom = 0.3;
            //            }

            //            dview.zoomTransform.ScaleX = dview.CurrentZoom;
            //            dview.zoomTransform.ScaleY = dview.CurrentZoom;
            //            //dview.ViewGrid.RenderTransform = dview.zoomTransform;
            //            dview.Page.RenderTransform = dview.zoomTransform;
            //            dview.Page.InvalidateArrange();
            //            dview.Page.InvalidateMeasure();



            //        }

            //    }


            //}




            e.Handled = true;
            base.OnMouseWheel(e);




        }

        public static void ZoomIn(DiagramView dview)
        {
            if (dview.IsPageEditable)
            {

                if (dview.onReset)
                {
                    dview.CurrentZoom = 1;
                    dview.onReset = false;
                }

                dview.CurrentZoom += dview.ZoomFactor;
                if (dview.CurrentZoom >= 6)
                {
                    dview.CurrentZoom = 6;
                }

                dview.zoomTransform.ScaleX = dview.CurrentZoom;
                dview.zoomTransform.ScaleY = dview.CurrentZoom;
                // dview.viewgrid.RenderTransform = dview.zoomTransform;
                dview.Page.RenderTransform = dview.zoomTransform;
                dview.Page.InvalidateArrange();
                dview.Page.InvalidateMeasure();


            }
        }

        /// <summary>
        /// Invoked when ZoomOut Command is Executed.
        /// </summary>
        /// <param name="dview">The diagramview instance.</param>
        public static void ZoomOut(DiagramView dview)
        {
            if (dview.IsPageEditable)
            {

                if (dview.onReset)
                {
                    dview.CurrentZoom = 1;
                    dview.onReset = false;
                }

                dview.CurrentZoom -= dview.ZoomFactor;
                if (dview.CurrentZoom < .3)
                {
                    dview.CurrentZoom = .3;
                }

                dview.zoomTransform.ScaleX = dview.CurrentZoom;
                dview.zoomTransform.ScaleY = dview.CurrentZoom;
                //dview.viewgrid.RenderTransform = dview.zoomTransform;
                dview.Page.RenderTransform = dview.zoomTransform;
                dview.Page.InvalidateArrange();
                dview.Page.InvalidateMeasure();



            }
        }
        #region Class fields

        #endregion

        #region Initialization

        #endregion

        #region Properties

        #endregion

        #region Internal Properties

        #endregion

        #region Dependency Property

        #endregion

        #region INotifyPropertyChanged Members

        #endregion

        #region Implementation

        #endregion

        #region class override

        #endregion

        #region IView Members

        #endregion


    }
}
