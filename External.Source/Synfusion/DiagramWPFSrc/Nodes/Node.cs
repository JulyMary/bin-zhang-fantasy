// <copyright file="Node.cs" company="Syncfusion">
// Copyright Syncfusion Inc. 2001 - 2011. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Data;

namespace Syncfusion.Windows.Diagram
{

    /// <summary>
    /// Represents the node class.
    /// </summary>
    /// <remarks>
    /// Nodes are graphical objects that can be drawn on the page by selecting them from the Symbol Palette and dropping them on the page, or they can be added through code behind.
    /// </remarks>
    /// <example>
    /// <para/>The following example shows how to create a <see cref="DiagramModel"/> in C# and add nodes to it.
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
    ///       Model = new DiagramModel ();
    ///       View = new DiagramView ();
    ///       Control.View = View;
    ///       Control.Model = Model;
    ///       View.Bounds = new Thickness(0, 0, 1000, 1000);
    ///       Node n = new Node(Guid.NewGuid(), "Start");
    ///        n.Shape = Shapes.FlowChart_Start;
    ///        n.IsLabelEditable = true;
    ///        n.Label = "Start";
    ///        n.Level = 1;
    ///        n.OffsetX = 150;
    ///        n.OffsetY = 25;
    ///        n.Width = 150;
    ///        n.Height = 75;
    ///        n.LabelVerticalAlignment = VerticalAlignment.Center;
    ///        n.LabelHorizontalAlignment = HorizontalAlignment.Center;
    ///        n.ToolTip="Start Node";
    ///        Model.Nodes.Add(n);
    ///    }
    ///    }
    ///    }
    /// </code>
    /// </example>
    /// <seealso cref="DiagramModel"/>
    /// <seealso cref="LineConnector"/>
#if SyncfusionFramework4_0
    [DesignTimeVisible(false)]
#endif
    public partial class Node : ContentControl, IShape, INodeGroup, INotifyPropertyChanged, ICommon
    {

        internal bool m_LayoutDisconnected;

        #region Class variables

        internal Size m_TempSize;
        internal bool m_MouseMoving = false;
        internal bool m_MouseResizing = false;
        internal Point m_TempPosition;

        internal bool subTreeReVal = true;

        internal double segmentoffset;

        internal double tempx;

        internal double tempy;

        internal double RSize { get { return Math.Max(Width, Height); } }

        internal ObservableCollection<LineConnector> In = new ObservableCollection<LineConnector>();

        internal ObservableCollection<LineConnector> Out = new ObservableCollection<LineConnector>();

        internal ObservableCollection<Node> RParents = new ObservableCollection<Node>();

        internal ObservableCollection<Node> RChildrens = new ObservableCollection<Node>();

        internal Node RParent;

        internal int Stage;

        internal bool isfired;

        internal bool currentdragging=false;

        //internal structSegment Segment = new structSegment();

        internal bool Visited;

        internal bool onceResized;
        internal int tempcount;
        private bool IsFirstLoaded=false;

        //internal struct structSegment
        //{
        //    internal double Width;
        //    internal double Height;
        //}

        /// <summary>
        /// Used to store Diagram Control object
        /// </summary>
        private DiagramControl dc;

        /// <summary>
        /// Used to check whether the first node is selected.
        /// </summary>
        //private bool selectfirst = false;

        /// <summary>
        /// used to store the groups.
        /// </summary>
        private CollectionExt m_groups = new CollectionExt();

        /// <summary>
        /// Used to store the rank
        /// </summary>
        private int m_rank = -1;

        /// <summary>
        /// Used to store the count of nodes.
        /// </summary>
        private int no = -1;

        /// <summary>
        /// Used to store the start point.
        /// </summary>
        private System.Windows.Point? startPoint = null;

        /// <summary>
        /// Used to store the connections list
        /// </summary>
        private List<IEdge> connections;

        ///// <summary>
        ///// Used to store the node position.
        ///// </summary>
        //private Point m_nodePosition;

        /// <summary>
        /// Used to store the Guid.
        /// </summary>
        private Guid m_id;

        /// <summary>
        /// Used to store the full name of the node.
        /// </summary>
        private string m_fullName;

        /// <summary>
        /// Used to store the in edges
        /// </summary>
        private CollectionExt m_inEdges = new CollectionExt();

        /// <summary>
        /// Used to store the out edges.
        /// </summary>
        private CollectionExt m_outEdges = new CollectionExt();

        /// <summary>
        /// Used to store the edges
        /// </summary>
        private CollectionExt m_edges = new CollectionExt();

        /// <summary>
        /// Used to store the connectors
        /// </summary>
        private CollectionExt m_connectors = new CollectionExt();

        /// <summary>
        /// Used to store the parents
        /// </summary>
        private CollectionExt mParents = new CollectionExt();

        /// <summary>
        /// Used to store the children
        /// </summary>
        private CollectionExt mChild = new CollectionExt();

        /// <summary>
        /// Used to store the ports.
        /// </summary>
        private ObservableCollection<ConnectionPort> m_ports = new ObservableCollection<ConnectionPort>();

        /// <summary>
        /// Used to store the parent node
        /// </summary>
        private IShape mParentNode = null;

        /// <summary>
        /// Used to store the parent edge.
        /// </summary>
        private IEdge mParentEdge = null;

        /// <summary>
        /// Used to store the tree children
        /// </summary>
        private CollectionExt treeChildren = null;

        /// <summary>
        /// Used to store the depth.
        /// </summary>
        private int mDepth = -1;

        /// <summary>
        /// Used to store the model.
        /// </summary>
        private DiagramModel model;

        /// <summary>
        /// Used to store the bounding rectangle.
        /// </summary>
        private System.Drawing.Rectangle mRectangle = new System.Drawing.Rectangle(0, 0, 100, 70);

        /// <summary>
        /// Used to store the IsFixed property value.
        /// </summary>
        private bool mIsFixed = false;

        /// <summary>
        /// Used to store the IsExpanded property value.
        /// </summary>
        private bool mIsExpanded = true;

        /// <summary>
        /// Used to store the page.
        /// </summary>
        private Panel mPage;

        private DiagramControl MdiagramControl;

        /// <summary>
        /// Used to store the View.
        /// </summary>
        private DiagramView dview;

        /// <summary>
        /// Used to store the execution check.
        /// </summary>
        private bool ex = false;

        /// <summary>
        /// Used to store the mouse up state
        /// </summary>
        private static bool mouseup = true;

        /// <summary>
        /// Used to store the last node click instance
        /// </summary>
        private DateTime lastNodeClick;

        /// <summary>
        /// Used to store the last node click point
        /// </summary>
        private Point lastNodePoint;

        /// <summary>
        /// Used to store the editor
        /// </summary>
        private LabelEditor editor;

        /// <summary>
        /// Used to store the resize node property setting.
        /// </summary>
        private bool resizenode = false;

        /// <summary>
        /// Used to store the rotate thumb
        /// </summary>
        private Control rotatethumb;

        /// <summary>
        /// Used to store the Group's rotate thumb
        /// </summary>
        private Control grouprotatethumb;
        
        /// <summary>
        /// Used to store the source port.
        /// </summary>
        private ConnectionPort sourceHitPort;

        /// <summary>
        /// Used to store the port visibility
        /// </summary>
        private bool portvisibilitycheck = false;

        /// <summary>
        /// Used to store the port no.
        /// </summary>
        private int pno = 1;

        /// <summary>
        /// Used to store the visible property value
        /// </summary>
        private bool m_wasvisible = false;

        /// <summary>
        /// Used to store the old ZIndex
        /// </summary>
        private int m_oldindex = 0;

        /// <summary>
        /// Used to store the new ZIndex
        /// </summary>
        private int m_newindex = 0;
        
        /// <summary>
        /// Used to store the old offset position.
        /// </summary>
        private Point oldoff = new Point();

        /// <summary>
        /// Used to store node offset assignment check value.
        /// </summary>
        private bool m_isexe = false;

        /// <summary>
        /// Used to store the state of the node in case of cycle detection.
        /// </summary>
        private int state = 0;

        /// <summary>
        /// Used to check if this node is to be connected o its parent or not.
        /// </summary>
        private bool canconn = true;

        private bool NodeVirtual = true;

        internal bool m_NodeDrawing = false;
        /// <summary>
        /// Used to check if mouse is double clicked.
        /// </summary>
        private bool isdoubleclicked = false;

        /// <summary>
        /// Used to store the old size of node.
        /// </summary>
        private Size osize = new Size();

        /// <summary>
        /// Used to check if HitTestVisibility is true or not.
        /// </summary>
        private bool hittest = false;

        /// <summary>
        /// Used to store the row count.
        /// </summary>
        private int row = 0;

        /// <summary>
        /// Used to store the column count.
        /// </summary>
        private int col = 0;

        /// <summary>
        /// Used to store the node width in pixels.
        /// </summary>
        private double pwidth = 0;

        /// <summary>
        /// Used to store the node height in pixels.
        /// </summary>
        private double pheight = 0;


        /// <summary>
        /// Used to Store the Whether the Node is Resized or not.
        /// </summary>
        internal bool m_IsResizing = false;

        /// <summary>
        /// Used to store the old offsetx while undo/redo.
        /// </summary>
        private double oldx = 0;

        /// <summary>
        /// Used to store the old offsety while undo/redo.
        /// </summary>
        private double oldy = 0;

        /// <summary>
        /// Used to check if oldx and oldy are set once.
        /// </summary>
        private bool exeonce = false;

        /// <summary>
        /// Used to check if the default value is assigned.
        /// </summary>
        //private bool isdefaulted = false;

        /// <summary>
        /// Used to check if the node was resized.
        /// </summary>
        private bool resize = false;




        #endregion

        #region Initialization

        /// <summary>
        /// Initializes static members of the <see cref="Node"/> class.
        /// </summary>
        static Node()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Node), new FrameworkPropertyMetadata(typeof(Node)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        /// <param name="id">The Guid id.</param>
        /// <param name="name">The node name.</param>
        public Node(Guid id, string name)
        {
            this.m_id = id;
            this.Name = name;
            if (this.Name == null || this.Name == string.Empty)
            {
                if (id != null)
                {
                    this.Name = "Node" + id.ToString("N");
                }
                else
                {
                    this.Name = "Node" + Guid.NewGuid().ToString("N");
                }
            }
            this.LayoutUpdated += new EventHandler(Node_LayoutUpdated);
            this.Loaded += new RoutedEventHandler(Node_Loaded);
            this.AddHandler(Control.MouseLeftButtonUpEvent, new MouseButtonEventHandler(Node_MouseLeftButtonUp), true);
            this.Unloaded += new RoutedEventHandler(Node_Unloaded);
            this.SizeChanged += new SizeChangedEventHandler(Node_SizeChanged);
            Boundaries = new Path();
            this.Loaded += new RoutedEventHandler(First_Load);
        }

        private bool _FirstLoaded = false;

        void First_Load(object sender, RoutedEventArgs e)
        {
            _FirstLoaded = true;
            this.Width = MeasureUnitsConverter.ToPixels(this.Width, this.MeasurementUnits);
            this.Height = MeasureUnitsConverter.ToPixels(this.Height, this.MeasurementUnits);
            if (centerport != null)
            {
                centerport.Left = this.Width / 2;
                centerport.Top = this.Height / 2;
            }
        }

        //internal static void SetUnitBinding(string PropName, string UnitPropName, DependencyProperty dependencyProperty, FrameworkElement src)
        //{
        //    MultiBinding UnitPixelBind = new MultiBinding();
        //    UnitPixelBind.Mode = BindingMode.TwoWay;
        //    UnitPixelBind.Converter = new PixelUnitConverter();
        //    Binding UnitValue = new Binding(PropName);
        //    UnitValue.Mode = BindingMode.TwoWay;
        //    UnitValue.Source = src;
        //    UnitPixelBind.Bindings.Add(UnitValue);
        //    Binding MeasureValue = new Binding(UnitPropName);
        //    MeasureValue.Mode = BindingMode.TwoWay;
        //    MeasureValue.Source = src;
        //    UnitPixelBind.Bindings.Add(MeasureValue);
        //    src.SetBinding(dependencyProperty, UnitPixelBind);
        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        /// <param name="id">The Guid id.</param>
        public Node(Guid id)
            : this(id, null)
        {
            //this.m_id = id;
            //this.LayoutUpdated += new EventHandler(Node_LayoutUpdated);
            //this.Loaded += new RoutedEventHandler(Node_Loaded);
            //this.AddHandler(Control.MouseLeftButtonUpEvent, new MouseButtonEventHandler(Node_MouseLeftButtonUp), true);
            //this.RemoveHandler(Control.MouseLeftButtonUpEvent, new MouseButtonEventHandler(Node_MouseLeftButtonUp));
            //this.Unloaded += new RoutedEventHandler(Node_Unloaded);
            //this.SizeChanged += new SizeChangedEventHandler(Node_SizeChanged);
            //Boundaries = new Path();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        public Node()
            : this(Guid.NewGuid())
        {
            //this.LayoutUpdated += new EventHandler(Node_LayoutUpdated);
            //this.Loaded += new RoutedEventHandler(Node_Loaded);
            //this.AddHandler(Control.MouseLeftButtonUpEvent, new MouseButtonEventHandler(Node_MouseLeftButtonUp), true);
            //this.RemoveHandler(Control.MouseLeftButtonUpEvent, new MouseButtonEventHandler(Node_MouseLeftButtonUp));
            //this.Unloaded += new RoutedEventHandler(Node_Unloaded);
            //this.SizeChanged += new SizeChangedEventHandler(Node_SizeChanged);
            //Boundaries = new Path();
        }

        /// <summary>
        /// Handles the SizeChanged event of the Node control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.SizeChangedEventArgs"/> instance containing the event data.</param>
        private void Node_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (dc != null && !(sender is Layer)&&this.IsFirstLoaded)
            {
                //if (!(dc.View.undo || dc.View.redo))
                //{
                //    dview.RedoStack.Clear();
                //}
                if (!dc.View.IsResized && (!dc.View.IsResizedUndone || !this.onceResized) && !Resized)
                {
                    foreach (ConnectionPort port in this.Ports)
                    {
                        if (!dc.View.IsResizedRedone)
                        {
                            port.PreviousPortPoint = new Point(port.Left, port.Top);
                        }
                        if (dview != null && dview.UndoRedoEnabled && !(this is Group))
                        {
                            dview.UndoStack.Push(port);
                            dview.UndoStack.Push(port.PreviousPortPoint);
                        }
                    }
                    if (dview != null && dview.UndoRedoEnabled && !(this is Group))
                    {
                        dview.UndoStack.Push(dview.DragDelta.ToString());
                        dview.UndoStack.Push(this);
                        dc.View.UndoStack.Push(e.PreviousSize);
                        dview.UndoStack.Push(new Point(oldx, oldy));
                        dview.UndoStack.Push(dview.SelectionList.Count);
                        dview.UndoStack.Push("Resized");
                        dview.DragDelta = "No";
                    }
                    //Resized = true;
                    dc.View.IsResizedRedone = false;

                }
                else
                {

                    if (this.tempcount <= 1)
                    {
                        dc.View.IsResizedUndone = false;
                    }

                }

            }
        }

        internal ConnectionPort centerport;

        private DiagramPage GetParent(DependencyObject dep)
        {
            DependencyObject obj = VisualTreeHelper.GetParent(dep);
            if (obj == null)
            {
                return null;
            }
            else if (obj is DiagramPage)
            {
                return obj as DiagramPage;
            }
            else
            {
                return GetParent(obj);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.FrameworkElement.Initialized"/> event. This method is invoked whenever <see cref="P:System.Windows.FrameworkElement.IsInitialized"/> is set to true internally.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.RoutedEventArgs"/> that contains the event data.</param>
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            //if (Page == null)
            //{
            //    Page = GetParent(this);
            //}
            //if (Page != null)
            //{
            //    PxOffsetX = MeasureUnitsConverter.ToPixels(OffsetX, (Page as DiagramPage).MeasurementUnits);
            //    PxOffsetY = MeasureUnitsConverter.ToPixels(OffsetY, (Page as DiagramPage).MeasurementUnits);
            //    PxPosition = MeasureUnitsConverter.ToPixels(Position, (Page as DiagramPage).MeasurementUnits);

            //    //PxOffsetX = MeasureUnitsConverter.ToPixels(OffsetX, this.MeasurementUnits);
            //    //PxOffsetY = MeasureUnitsConverter.ToPixels(OffsetY, this.MeasurementUnits);
            //    //PxPosition = MeasureUnitsConverter.ToPixels(Position, this.MeasurementUnits);
            //}
            //else
            //{
            //    PxOffsetX = OffsetX;
            //    PxOffsetY = OffsetY;
            //    PxPosition = Position;
            //}

            if (this.Content is FrameworkElement)
            {
                //(this.Content as FrameworkElement).IsHitTestVisible = false;
            }
            if (!DiagramControl.IsPageLoaded)
            {
                centerport = new ConnectionPort();
                centerport.Name = "PART_Sync_CenterPort";
                centerport.Node = this;
                centerport.PortShape = PortShapes.Circle;
                if (!double.IsNaN(this.Width) || !double.IsNaN(this.Height))
                {
                    if (double.IsNaN(centerport.Width) || double.IsNaN(centerport.Height))
                    {
                        //double four = 4;// MeasureUnitsConverter.FromPixels(4, DiagramPage.Munits);
                        centerport.Left = this.Width / 2;// -four;
                        centerport.Top = this.Height / 2;// -four;
                    }
                    else
                    {
                        centerport.Left = this.Width / 2;// -centerport.Width / 2;
                        centerport.Top = this.Height / 2;// -centerport.Height / 2;
                    }
                }
                else
                {
                    double two0ne = 25;// MeasureUnitsConverter.FromPixels(25, this.MeasurementUnits);
                    centerport.Left = two0ne;
                    centerport.Top = two0ne;
                }

                if (this.Ports != null)
                {
                    this.Ports.Add(centerport);
                }

                centerport.CenterPortReferenceNo = 0;
            }
        }

        /// <summary>
        /// Calls Node_Loaded method of the instance, notifies of the dependency property value changes.
        /// </summary>
        /// <param name="sender"> object, the change occurs on.</param>
        /// <param name="e">Property change details, such as old value and new value.</param>
        private void Node_Loaded(object sender, RoutedEventArgs e)
        {
            if (Page == null)
            {
                Page = GetParent(this);
            }

            if (Nodediagramcontrol == null)
            {
                Nodediagramcontrol=DiagramPage.GetDiagramControl((sender as Node));

            }
            //if (Page != null)
            //{
            //    PxOffsetX = MeasureUnitsConverter.ToPixels(OffsetX, (Page as DiagramPage).MeasurementUnits);
            //    PxOffsetY = MeasureUnitsConverter.ToPixels(OffsetY, (Page as DiagramPage).MeasurementUnits);
            //    PxPosition = MeasureUnitsConverter.ToPixels(Position, (Page as DiagramPage).MeasurementUnits);

            //    //PxOffsetX = MeasureUnitsConverter.ToPixels(OffsetX, this.MeasurementUnits);
            //    //PxOffsetY = MeasureUnitsConverter.ToPixels(OffsetY, this.MeasurementUnits);
            //    //PxPosition = MeasureUnitsConverter.ToPixels(Position, this.MeasurementUnits);
            //}
            //else
            //{
            //    PxOffsetX = OffsetX;
            //    PxOffsetY = OffsetY;
            //    PxPosition = Position;
            //}
            //SetUnitBinding("PxOffsetX", "MeasurementUnits", Node.OffsetXProperty, this);
            //SetUnitBinding("PxOffsetY", "MeasurementUnits", Node.OffsetYProperty, this);
            //SetUnitBinding("PxPosition", "MeasurementUnits", Node.PositionProperty, this);

            this.IsFirstLoaded = true;
            if (this.IsSelected)
            {
                if (dview != null)
                {
                    dview.SelectionList.Add(this);
                }
            }
            try
            {
                this.MouseLeftButtonUp += new MouseButtonEventHandler(Node_MouseLeftButtonUp);
                dview = GetDiagramView(this);
                dc = DiagramPage.GetDiagramControl(this);
                if (!exeonce)
                {
                    oldx = (double)this.PxOffsetX;//MeasureUnitsConverter.ToPixels((double)this.LogicalOffsetX, (dview.Page as DiagramPage).MeasurementUnits);
                    oldy = (double)this.PxOffsetY;//MeasureUnitsConverter.ToPixels((double)this.LogicalOffsetY, (dview.Page as DiagramPage).MeasurementUnits);
                    exeonce = true;
                }

                if (!dview.IsPageEditable)
                {
                    DiagramView.PageEdit = false;
                    IsLabelEditable = false;
                }

                if (this.Content is Viewbox)
                {
                    (this.Content as Viewbox).Stretch = Stretch.Fill;
                    (this.Content as Viewbox).Width = this.Width;
                    (this.Content as Viewbox).Height = this.Height;
                }
                foreach (ConnectionPort cport in this.Ports)
                {
                    if (cport.PortReferenceNo < 1)
                    {
                        cport.PortReferenceNo = this.countpno + pno;
                        pno++;
                    }
                }

                //foreach (ConnectionPort cport in this.Ports)
                //{
                //    if (cport.PortReferenceNo < 1)
                //    {
                //        cport.PortReferenceNo = (cport.Node.ReferenceNo * 10) + pno;
                //        pno++;
                //    }
                //}
            }
            catch
            {
            }
            if (this.m_NodeDrawing)
            {
                this.m_NodeDrawing = false;
                dc.View.Page.InvalidateMeasure();
            }
        }

        /// <summary>
        /// Handles the Unloaded event of the Node control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Node_Unloaded(object sender, RoutedEventArgs e)
        {
            if (dview != null && dview.Page != null)
            {
                //if (dview.Scrollviewer != null && !(dview.Page as DiagramPage).IsDiagrampageLoaded)
                //{
                //    if (dview.IsMouseScrolled)
                //    {
                //        //if ((dview.Page as DiagramPage).Minleft >= 0)
                //        //{
                //        //    dview.Scrollviewer.ScrollToHorizontalOffset(Math.Abs((dview.Page as DiagramPage).HorValue));
                //        //}
                //        //else
                //        //{
                //        //    dview.Scrollviewer.ScrollToHorizontalOffset(Math.Abs((dview.Page as DiagramPage).HorValue * dview.CurrentZoom));
                //        //}

                //        //if ((dview.Page as DiagramPage).MinTop >= 0)
                //        //{
                //        //    dview.Scrollviewer.ScrollToVerticalOffset(Math.Abs((dview.Page as DiagramPage).VerValue));
                //        //}
                //        //else
                //        //{
                //        //    dview.Scrollviewer.ScrollToVerticalOffset(Math.Abs((dview.Page as DiagramPage).VerValue * dview.CurrentZoom));
                //        //}
                //    }
                //    else
                //    {
                //        //dview.Scrollviewer.ScrollToHorizontalOffset(Math.Abs((dview.Page as DiagramPage).HorValue * dview.CurrentZoom));
                //        //dview.Scrollviewer.ScrollToVerticalOffset(Math.Abs((dview.Page as DiagramPage).VerValue * dview.CurrentZoom));
                //    }
                //}
            }
        }

        #endregion

        #region Properties

        public TextDecorationCollection LabelTextDecorations
        {
            get { return (TextDecorationCollection)GetValue(LabelTextDecorationsProperty); }
            set { SetValue(LabelTextDecorationsProperty, value); }
        }

        internal double Left
        {
            get { return this.PxOffsetX; }// MeasureUnitsConverter.ToPixels(this.LogicalOffsetX, this.MeasurementUnits); }
        }

        internal double Top
        {
            get { return this.PxOffsetY; }// MeasureUnitsConverter.ToPixels(this.LogicalOffsetY, this.MeasurementUnits); }
        }

        internal double Right
        {
            get { return this.Left + this.ActualWidth; }
        }

        internal double Bottom
        {
            get { return this.Top + this.ActualHeight; }
        }

        /// <summary>
        /// Gets or sets the boundaries.
        /// </summary>
        /// <value>The boundaries.</value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]                    
        public Shape Boundaries
        {
            get
            {
                return (Shape)GetValue(BoundariesProperty);
            }
            set
            {
                SetValue(BoundariesProperty, value);
            }
        }

        public IntersectionMode IntersectionMode
        {
            get
            {
                return (IntersectionMode)GetValue(IntersectionModeProperty);
            }
            set
            {
                SetValue(IntersectionModeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the rotate angle.
        /// </summary>
        /// <value>The rotate angle.</value>
        internal double RotateAngle
        {
            get
            {
                return (double)GetValue(RotateAngleProperty);
            }

            set
            {
                SetValue(RotateAngleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether content is hit test visible. Used for serialization purposes internally.
        /// </summary>
        /// <value>
        /// <c>true</c> if [content hit test visible]; otherwise, <c>false</c>.
        /// </value>
        public bool ContentHitTestVisible
        {
            get { return hittest; }
            set { hittest = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Node"/> is resized.
        /// </summary>
        /// <value><c>true</c> if resized; otherwise, <c>false</c>.</value>
        internal bool Resized
        {
            get { return resize; }
            set { resize = value; }
        }

        /// <summary>
        /// Gets or sets the old size.
        /// </summary>
        /// <value>The old size.</value>
        internal Size Oldsize
        {
            get { return osize; }
            set { osize = value; }
        }

        /// <summary>
        /// Gets the groups to which the INodeGroup objects belong.
        /// </summary>
        /// <value>The groups.</value>
        public CollectionExt Groups
        {
            get
            {
                return m_groups;
            }
        }

        /// <summary>
        /// Gets or sets the gripper visibility.
        /// </summary>
        /// <value>
        /// Type: <see cref="Visibility"/>
        /// Default value is Collapsed.
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
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.IsLabelEditable = true;
        /// n.Label = "Start";
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// n.ToolTip="Start Node";
        /// n.GripperVisibility=Visibility.Visible;
        /// Model.Nodes.Add(n);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="Gripper"/>
        public Visibility GripperVisibility
        {
            get
            {
                return (Visibility)GetValue(GripperVisibilityProperty);
            }

            set
            {
                SetValue(GripperVisibilityProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the gripper style.  
        /// </summary>
        /// <remarks>
        /// When the gripper style is set, it is necessary to specify the Width, Height, HorizontalAlignment, VerticalAlignment and the Margin properties because GripperStyle property overrides the default settings.
        /// </remarks>
        /// <example>
        /// <para/>The following example shows how to write a style for the Gripper in Window.Resources.
        /// <code language="XAML">
        ///  &lt;Style x:Key="GripperStyle"  TargetType="{x:Type syncfusion:Gripper}"&gt;
        ///        &lt;Setter Property="Width" Value="30"/&gt;
        ///        &lt;Setter Property="Height" Value="30"/&gt;
        ///        &lt;Setter Property="HorizontalAlignment" Value="Left"/&gt;
        ///        &lt;Setter Property="VerticalAlignment" Value="Top"/&gt; 
        ///        &lt;Setter Property="Margin" Value="10,-15,0,0"/&gt;
        ///        &lt;Setter Property="Template"&gt;
        ///            &lt;Setter.Value&gt;
        ///                &lt;ControlTemplate TargetType="{x:Type syncfusion:Gripper}"&gt;
        ///                    &lt;Border Background="Blue" CornerRadius="10"   /&gt;
        ///                &lt;/ControlTemplate&gt;
        ///           &lt;/Setter.Value&gt;
        ///        &lt;/Setter&gt;
        ///    &lt;/Style&gt;
        /// </code>
        /// <para/>The following code shows how to assign the style created to the Gripper of the <see cref="Node"/>.
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
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.IsLabelEditable = true;
        /// n.Label = "Start";
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// n.ToolTip="Start Node";
        /// n.GripperStyle=this.Resources["GripperStyle"] as Style.
        /// Model.Nodes.Add(n);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="Gripper"/>
        public Style GripperStyle
        {
            get
            {
                return (Style)GetValue(GripperStyleProperty);
            }

            set
            {
                SetValue(GripperStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Node"/> rotation is allowed.
        /// </summary>
        /// <value>
        /// Type: <see cref="Boolean"/>
        /// True, if rotation is enabled, false otherwise.
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
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.IsLabelEditable = true;
        /// n.Label = "Start";
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// n.ToolTip="Start Node";
        /// n.AllowRotate=true;
        /// Model.Nodes.Add(n);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        public bool AllowRotate
        {
            get
            {
                return (bool)GetValue(AllowRotateProperty);
            }

            set
            {
                SetValue(AllowRotateProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Node"/> resize is allowed.
        /// </summary>
        /// <value>
        /// Type: <see cref="Boolean"/>
        /// True, if resizing is enabled, false otherwise.
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
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.IsLabelEditable = true;
        /// n.Label = "Start";
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// n.ToolTip="Start Node";
        /// n.AllowResize=true;
        /// Model.Nodes.Add(n);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        public bool AllowResize
        {
            get
            {
                return (bool)GetValue(AllowResizeProperty);
            }

            set
            {
                SetValue(AllowResizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Node"/> can be moved.
        /// </summary>
        /// <value>
        /// Type: <see cref="Boolean"/>
        /// True, if drag is enabled, false otherwise.
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
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.IsLabelEditable = true;
        /// n.Label = "Start";
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// n.ToolTip="Start Node";
        /// n.AllowMove=true;
        /// Model.Nodes.Add(n);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        public bool AllowMove
        {
            get
            {
                return (bool)GetValue(AllowMoveProperty);
            }

            set
            {
                SetValue(AllowMoveProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Node"/> can be selected.
        /// </summary>
        /// <value>
        /// Type: <see cref="Boolean"/>
        /// True, if selection is enabled, false otherwise.
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
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.IsLabelEditable = true;
        /// n.Label = "Start";
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// n.ToolTip="Start Node";
        /// n.AllowSelect=true;
        /// Model.Nodes.Add(n);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        public bool AllowSelect
        {
            get
            {
                return (bool)GetValue(AllowSelectProperty);
            }

            set
            {
                SetValue(AllowSelectProperty, value);
            }
        }


        internal Rect RectBounds
        {
            get
            {
                return (Rect)GetValue(RectBoundsProperty);
            }

            set
            {
                SetValue(RectBoundsProperty, value);
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [allow delete].
        /// </summary>
        /// <value><c>true</c> if [allow delete]; otherwise, <c>false</c>.</value>
        public bool AllowDelete
        {
            get
            {
                return (bool)GetValue(AllowDeleteProperty);
            }
            set
            {
                SetValue(AllowDeleteProperty, value);
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether to resize this node.
        /// </summary>
        /// <value><c>true</c> if [resize this node]; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// Used for serialization purpose.
        /// </remarks>
        public bool ResizeThisNode
        {
            get { return resizenode; }
            set { resizenode = value; }
        }

        /// <summary>
        /// Gets or sets the reference no.
        /// </summary>
        /// <value>The reference no.</value>
        /// <remarks>
        /// Used for serialization purpose.
        /// </remarks>
        public int ReferenceNo
        {
            get { return no; }
            set { no = value; }
        }

        /// <summary>
        /// Gets or sets the page.
        /// </summary>
        /// <value>
        /// Type: <see cref="Panel"/>
        /// Panel instance.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Panel Page
        {
            get { return mPage; }
            set { mPage = value; }
        }

        internal DiagramControl Nodediagramcontrol
        {
            get { return MdiagramControl; }
            set { MdiagramControl = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is label editable.
        /// </summary>
        /// <value>
        /// Type: <see cref="bool"/>
        /// True, if it can be edited, false otherwise.
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
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.IsLabelEditable = true;
        /// n.Label = "Start";
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// n.ToolTip="Start Node";
        /// node.IsLabelEditable=true;
        /// Model.Nodes.Add(n);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        /// <remarks>
        /// Default Value is false.When this is false, HitTest is also set to false.
        /// When set to true, clicking on the label, will make the editable textbox visible.
        /// Enter the new label and press ENTER to apply the changed label,
        /// or press ESC to ignore the new label and revert back to the old one.
        /// </remarks>
        public bool IsLabelEditable
        {
            get { return (bool)GetValue(IsLabelEditableProperty); }
            set { SetValue(IsLabelEditableProperty, value); }
        }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// Type: <see cref="string"/>
        /// String value.
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
        ///       Model = new DiagramModel ();
        ///       View = new DiagramView ();
        ///       Control.View = View;
        ///       Control.Model = Model;
        ///       View.Bounds = new Thickness(0, 0, 1000, 1000);
        ///       Node n = new Node(Guid.NewGuid(), "Start");
        ///        n.Shape = Shapes.FlowChart_Start;
        ///        n.IsLabelEditable = true;
        ///        n.Label = "Start";
        ///        n.OffsetX = 150;
        ///        n.OffsetY = 25;
        ///        n.Width = 150;
        ///        n.Height = 75;
        ///        n.ToolTip="Start Node";
        ///        node.Label="SyncNode";
        ///        Model.Nodes.Add(n);
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        /// <remarks>
        /// Default value is an empty string.
        /// </remarks>
        public string Label
        {
            get
            {
                return (string)GetValue(LabelProperty);
            }

            set
            {
                SetValue(LabelProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label visibility.
        /// </summary>
        /// <value>
        /// Type: <see cref="Visibility"/>
        /// Enum specifying the visibility.
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
        ///       Model = new DiagramModel ();
        ///       View = new DiagramView ();
        ///       Control.View = View;
        ///       Control.Model = Model;
        ///       View.Bounds = new Thickness(0, 0, 1000, 1000);
        ///       Node n = new Node(Guid.NewGuid(), "Start");
        ///        n.Shape = Shapes.FlowChart_Start;
        ///        n.IsLabelEditable = true;
        ///        n.Label = "Start";
        ///        n.OffsetX = 150;
        ///        n.OffsetY = 25;
        ///        n.Width = 150;
        ///        n.Height = 75;
        ///        n.ToolTip="Start Node";
        ///        node.LabelVisibility=Visibility.Visible;
        ///        Model.Nodes.Add(n);
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        /// <remarks>
        /// Default value is visible.
        /// </remarks>
        public Visibility LabelVisibility
        {
            get
            {
                return (Visibility)GetValue(LabelVisibilityProperty);
            }

            set
            {
                SetValue(LabelVisibilityProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the HorizontalAlignment of the Label. This will take effect only if the LabelWidth is set.
        /// </summary>
        /// <value>
        /// Type: <see cref="HorizontalAlignment"/>
        /// Enum specifying the alignment position.</value>
        /// <remarks>Default HorizontalAlignment is at the Center.</remarks>
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
        ///       Model = new DiagramModel ();
        ///       View = new DiagramView ();
        ///       Control.View = View;
        ///       Control.Model = Model;
        ///       View.Bounds = new Thickness(0, 0, 1000, 1000);
        ///       Node n = new Node(Guid.NewGuid(), "Start");
        ///        n.Shape = Shapes.FlowChart_Start;
        ///        n.IsLabelEditable = true;
        ///        n.Label = "Start";
        ///        n.OffsetX = 150;
        ///        n.OffsetY = 25;
        ///        n.Width = 150;
        ///        n.Height = 75;
        ///        n.ToolTip="Start Node";
        ///        node.LabelHorizontalAlignment=HorizontalAlignment.Left;
        ///        Model.Nodes.Add(n);
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        public HorizontalAlignment LabelHorizontalAlignment
        {
            get
            {
                return (HorizontalAlignment)GetValue(LabelHorizontalAlignmentProperty);
            }

            set
            {
                SetValue(LabelHorizontalAlignmentProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the VerticalAlignment of the Label.
        /// </summary>
        /// <value>
        /// Type: <see cref="VerticalAlignment"/>
        /// Enum specifying the alignment position.</value>
        /// <remarks>Default VerticalAlignment is at the Top.</remarks>
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
        ///       Model = new DiagramModel ();
        ///       View = new DiagramView ();
        ///       Control.View = View;
        ///       Control.Model = Model;
        ///       View.Bounds = new Thickness(0, 0, 1000, 1000);
        ///       Node n = new Node(Guid.NewGuid(), "Start");
        ///        n.Shape = Shapes.FlowChart_Start;
        ///        n.IsLabelEditable = true;
        ///        n.Label = "Start";
        ///        n.OffsetX = 150;
        ///        n.OffsetY = 25;
        ///        n.Width = 150;
        ///        n.Height = 75;
        ///        n.ToolTip="Start Node";
        ///        node.LabelVerticalAlignment = VerticalAlignment.Left;
        ///        Model.Nodes.Add(n);
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        public VerticalAlignment LabelVerticalAlignment
        {
            get
            {
                return (VerticalAlignment)GetValue(LabelVerticalAlignmentProperty);
            }

            set
            {
                SetValue(LabelVerticalAlignmentProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label angle.
        /// </summary>
        /// <value>
        /// Type: <see cref="double"/>
        /// Angle value in pixels.
        /// </value>
        /// <remarks>Default Angle is 0d.</remarks>
        public double LabelAngle
        {
            get { return (double)GetValue(LabelAngleProperty); }
            set { SetValue(LabelAngleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the connection drag is over.
        /// </summary>
        /// <value>
        /// <c>true</c> if connection drag is completed; otherwise, <c>false</c>.
        /// </value>
        public bool IsDragConnectionOver
        {
            get { return (bool)GetValue(IsDragConnectionOverProperty); }
            set { SetValue(IsDragConnectionOverProperty, value); }
        }

        /// <summary>
        /// Gets or sets the shape.
        /// </summary>
        /// <value>
        /// Type: <see cref="double"/>
        /// Enum specifying the Shapes .
        /// </value>
        /// <remarks>
        /// Several built-in shapes are provided. The user can select from any of the built-in shapes or specify their own custom shape using the <see cref="CustomPathStyle"/> property.
        /// </remarks>
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
        ///       Model = new DiagramModel ();
        ///       View = new DiagramView ();
        ///       Control.View = View;
        ///       Control.Model = Model;
        ///       View.Bounds = new Thickness(0, 0, 1000, 1000);
        ///       Node n = new Node(Guid.NewGuid(), "Start");
        ///        n.Shape = Shapes.FlowChart_Start;
        ///        n.IsLabelEditable = true;
        ///        n.Label = "Start";
        ///        n.OffsetX = 150;
        ///        n.OffsetY = 25;
        ///        n.Width = 150;
        ///        n.Height = 75;
        ///        n.ToolTip="Start Node";
        ///        node.LabelVerticalAlignment = VerticalAlignment.Left;
        ///        Model.Nodes.Add(n);
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        public Shapes Shape
        {
            get { return (Shapes)GetValue(ShapeProperty); }
            set { SetValue(ShapeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the PathStyle of the Node.
        /// </summary>
        /// <value>
        /// Type: <see cref="Style"/>
        /// </value>
        /// <remarks>
        /// While setting the custom path, the shape of the node can be set to Custom.
        /// </remarks>
        /// <example>
        /// <para/>This example shows how to set PathStyle of a node .
        /// Specify a resource in XAML .
        /// <code language="XAML">
        /// &lt;Style TargetType="{x:Type Path}" x:Key="myNode"&gt;
        ///     &lt;Setter Property="Data" Value="M200,239L200,200 240,239 280,202 320,238 281,279 240,244 198,279z"&gt;&lt;/Setter&gt;
        ///     &lt;Setter Property="Fill" Value="MidnightBlue" /&gt;
        /// &lt;/Style&gt;
        ///  </code>
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
        ///       Model = new DiagramModel ();
        ///       View = new DiagramView ();
        ///       Control.View = View;
        ///       Control.Model = Model;
        ///       View.Bounds = new Thickness(0, 0, 1000, 1000);
        ///       Node n = new Node(Guid.NewGuid(), "Start");
        ///       Style customstyle = (Style)this.Resources["myNode"];
        ///        n.CustomPathStyle=customstyle;
        ///        n.Shape = Shapes.Custom;
        ///        n.IsLabelEditable = true;
        ///        n.Label = "Start";
        ///        n.OffsetX = 150;
        ///        n.OffsetY = 25;
        ///        n.Width = 150;
        ///        n.Height = 75;
        ///        Model.Nodes.Add(n);
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        public Style CustomPathStyle
        {
            get { return (Style)GetValue(CustomPathStyleProperty); }
            set { SetValue(CustomPathStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>The level.</value>
        /// <remarks>
        /// Based on the level property , the nodes belonging to the same level can be customized to have the same look and feel.
        /// </remarks>
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
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.IsLabelEditable = true;
        /// n.Level = 2;
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// node.LabelVerticalAlignment = VerticalAlignment.Left;
        /// Model.Nodes.Add(n);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        public int Level
        {
            get { return (int)GetValue(LevelProperty); }
            set { SetValue(LevelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Center Position  of the Node.
        /// </summary>
        /// <value>
        /// Type: <see cref="Point"/>
        /// Center Point.</value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Point Position
        {
            get { return (Point)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Position.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(Point), typeof(Node));
        
        //public Point Position
        //{
        //    get
        //    {
        //        return m_nodePosition;
        //    }

        //    set
        //    {
        //        if (m_nodePosition != value)
        //        {
        //            m_nodePosition = value;
        //            OnPropertyChanged("Position");
        //        }
        //    }
        //}

        /// <summary>
        /// Gets or sets a value indicating whether this instance is center port enabled.
        /// </summary>
        /// <value>
        /// Type: <see cref="bool"/>
        /// True, if it is enabled, false otherwise.
        /// </value>
        /// <remarks>Default value is true.</remarks>
        public bool IsPortEnabled
        {
            get
            {
                return (bool)GetValue(IsPortEnabledProperty);
            }

            set
            {
                SetValue(IsPortEnabledProperty, value);
            }
        }

        /// <summary>
        /// Gets the information about the node.
        /// </summary>
        /// <returns>Node info value</returns>
        /// <value>
        /// Type: <see cref="NodeInfo"/>
        /// </value>
        internal NodeInfo GetInfo()
        {
            NodeInfo info = new NodeInfo();
            info.Left = this.PxOffsetX;
            info.Top = this.PxOffsetY;
            double aw, ah;
            if (DiagramControl.IsPageLoaded)
            {
                double paw = this.Width;// MeasureUnitsConverter.ToPixels(this.Width, this.MeasurementUnits);
                double pah = this.Height;// MeasureUnitsConverter.ToPixels(this.Height, this.MeasurementUnits);
                aw = paw;// MeasureUnitsConverter.FromPixels(paw, this.MeasurementUnits);
                ah = pah;// MeasureUnitsConverter.FromPixels(pah, this.MeasurementUnits);
            }
            else
            {
                aw = this.ActualWidth;// MeasureUnitsConverter.FromPixels(this.ActualWidth, this.MeasurementUnits);
                ah = this.ActualHeight;// MeasureUnitsConverter.FromPixels(this.ActualHeight, this.MeasurementUnits);
            }

            info.Size = new Size(aw, ah);
            Point pos = this.PxPosition;// MeasureUnitsConverter.FromPixels(this.Position, this.MeasurementUnits);
            info.Position = pos;
            info.MeasurementUnit = this.MeasurementUnits;
            return info;
        }

        /// <summary>
        /// Gets a Collection of IEdge connections.
        /// </summary>
        /// <value>
        /// Type: <see cref="List"/>
        /// </value>
        public List<IEdge> Connections
        {
            get
            {
                if (connections == null)
                {
                    connections = new List<IEdge>();
                }

                return connections;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Node"/> is double clicked.
        /// </summary>
        /// <value>
        /// <c>true</c> if this <see cref="Node"/> is double clicked; otherwise, <c>false</c>.
        /// </value>
        internal bool IsDoubleClicked
        {
            get
            {
                return (bool)GetValue(IsDoubleClickedProperty);
            }

            set
            {
                SetValue(IsDoubleClickedProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the ports.
        /// </summary>
        /// <value>The ports.</value>
        /// <example>
        /// C#:
        /// <para/>
        /// The following example shows how to create a <see cref="ConnectionPort"/> in C#.
        /// <code language="C#">
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
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// //Creates a node
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.IsLabelEditable = true;
        /// n.Label = "Start";
        /// n.Level = 1;
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// n.ToolTip="Start Node";
        /// Model.Nodes.Add(n);
        /// //Define a Custom port for the node.
        /// ConnectionPort port = new ConnectionPort();
        /// port.Node=n;
        /// port.Left=75;
        /// port.Top=10;
        /// port.PortShape = PortShapes.Arrow;
        /// port.PortStyle.Fill = Brushes.Transparent;
        /// port.Height = 11;
        /// port.Width = 11;
        /// n.Ports.Add(port);
        /// Node n1 = new Node(Guid.NewGuid(), "Decision1");
        /// n1.Shape = Shapes.FlowChart_Process;
        /// n1.IsLabelEditable = true;
        /// n1.Label = "Alarm Rings";
        /// n1.Level = 2;
        /// n1.OffsetX = 150;
        /// n1.OffsetY = 125;
        /// n1.Width = 150;
        /// n1.Height = 75;
        /// Model.Nodes.Add(n1);
        /// ConnectionPort port1 = new ConnectionPort();
        /// port1.Node=n;
        /// port1.Left=75;
        /// port1.Top=50;
        /// port1.PortShape = PortShapes.Arrow;
        /// port1.PortStyle.Fill = Brushes.Transparent;
        /// port1.Height = 11;
        /// port1.Width = 11;
        /// n1.Ports.Add(port1);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="ConnectionPort"/>
        public ObservableCollection<ConnectionPort> Ports
        {
            get { return m_ports; }
            set { m_ports = value; }
        }

        /// <summary>
        /// Gets or sets the port visibility.
        /// </summary>
        /// <value>The port visibility.</value>
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
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.IsLabelEditable = true;
        /// n.Label = "Start";
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// n.ToolTip="Start Node";
        /// n.PortVisibility = Visibility.Visible;
        /// Model.Nodes.Add(n);
        /// }
        /// }
        /// }
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
        /// Gets or sets a value indicating whether port can be moved or not.
        /// </summary>
        /// <value>
        /// Type: <see cref="Boolean"/>
        /// True, if drag is enabled, false otherwise.
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
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.IsLabelEditable = true;
        /// n.Label = "Start";
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// n.ToolTip="Start Node";
        /// n.AllowPortDrag = true;
        /// Model.Nodes.Add(n);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        public bool AllowPortDrag
        {
            get
            {
                return (bool)GetValue(AllowPortDragProperty);
            }

            set
            {
                SetValue(AllowPortDragProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the width of the label.
        /// </summary>
        /// <value>
        /// Type: <see cref="double"/>
        /// By default the label width equals the node width.
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
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.IsLabelEditable = true;
        /// n.Label = "Start";
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// n.ToolTip="Start Node";
        /// n.LabelWidth = 50;
        /// Model.Nodes.Add(n);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        public double LabelWidth
        {
            get
            {
                return (double)GetValue(LabelWidthProperty);
            }

            set
            {
                SetValue(LabelWidthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the height of the label.
        /// </summary>
        /// <value>
        /// Type: <see cref="double"/>
        /// By default the label height equals 20.
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
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.IsLabelEditable = true;
        /// n.Label = "Start";
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// n.ToolTip="Start Node";
        /// n.LabelHeight = 50;
        /// Model.Nodes.Add(n);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        internal double LabelHeight
        {
            get
            {
                return (double)GetValue(LabelHeightProperty);
            }

            set
            {
                SetValue(LabelHeightProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label text wrapping.
        /// </summary>
        /// <value>
        /// Type: <see cref="TextWrapping"/>
        /// By default it is set to NoWrap.
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
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.IsLabelEditable = true;
        /// n.Label = "Start";
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// n.ToolTip="Start Node";
        /// n.LabelTextWrapping = TextWrapping.Wrap;
        /// Model.Nodes.Add(n);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        public TextWrapping LabelTextWrapping
        {
            get
            {
                return (TextWrapping)GetValue(LabelTextWrappingProperty);
            }

            set
            {
                SetValue(LabelTextWrappingProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the size of the label font.
        /// </summary>
        /// <value>
        /// Type: <see cref="double"/>
        /// By default it is set to 11d.
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
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.IsLabelEditable = true;
        /// n.Label = "Start";
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// n.ToolTip="Start Node";
        /// n.LabelFontSize = 14;
        /// Model.Nodes.Add(n);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        public double LabelFontSize
        {
            get
            {
                return (double)GetValue(LabelFontSizeProperty);
            }

            set
            {
                SetValue(LabelFontSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label font family.
        /// </summary>
        /// <value>
        /// Type: <see cref="FontFamily"/>
        /// By default it is set to Arial.
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
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.IsLabelEditable = true;
        /// n.Label = "Start";
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// n.ToolTip="Start Node";
        /// n.LabelFontFamily = new FontFamily("Verdana");
        /// Model.Nodes.Add(n);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        public FontFamily LabelFontFamily
        {
            get
            {
                return (FontFamily)GetValue(LabelFontFamilyProperty);
            }

            set
            {
                SetValue(LabelFontFamilyProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label font weight.
        /// </summary>
        /// <value>
        /// Type: <see cref="FontWeight"/>
        /// By default it is set to SemiBold.
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
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.IsLabelEditable = true;
        /// n.Label = "Start";
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// n.ToolTip="Start Node";
        /// n.LabelFontWeight = FontWeights.Bold;
        /// Model.Nodes.Add(n);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        public FontWeight LabelFontWeight
        {
            get
            {
                return (FontWeight)GetValue(LabelFontWeightProperty);
            }

            set
            {
                SetValue(LabelFontWeightProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label font style.
        /// </summary>
        /// <value>
        /// Type: <see cref="FontStyle"/>
        /// By default it is set to Normal.
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
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.IsLabelEditable = true;
        /// n.Label = "Start";
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// n.ToolTip="Start Node";
        /// n.LabelFontStyle = FontStyles.Italic;
        /// Model.Nodes.Add(n);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        public FontStyle LabelFontStyle
        {
            get
            {
                return (FontStyle)GetValue(LabelFontStyleProperty);
            }

            set
            {
                SetValue(LabelFontStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label text trimming.
        /// </summary>
        /// <value>
        /// Type: <see cref="TextTrimming"/>
        /// By default it is set to CharacterEllipsis.
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
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.IsLabelEditable = true;
        /// n.Label = "Start";
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// n.ToolTip="Start Node";
        /// n.LabelTextTrimming = TextTrimming.None;
        /// Model.Nodes.Add(n);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        public TextTrimming LabelTextTrimming
        {
            get
            {
                return (TextTrimming)GetValue(LabelTextTrimmingProperty);
            }

            set
            {
                SetValue(LabelTextTrimmingProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label background.
        /// </summary>
        /// <value>
        /// Type: <see cref="Brush"/>
        /// By default it is set to White.
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
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.IsLabelEditable = true;
        /// n.Label = "Start";
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// n.ToolTip="Start Node";
        /// n.LabelBackground = Brushes.Red;
        /// Model.Nodes.Add(n);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        public Brush LabelBackground
        {
            get
            {
                return (Brush)GetValue(LabelBackgroundProperty);
            }

            set
            {
                SetValue(LabelBackgroundProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label foreground.
        /// </summary>
        /// <value>
        /// Type: <see cref="Brush"/>
        /// By default it is set to Black.
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
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.IsLabelEditable = true;
        /// n.Label = "Start";
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// n.ToolTip="Start Node";
        /// n.LabelForeground = Brushes.Blue;
        /// Model.Nodes.Add(n);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        public Brush LabelForeground
        {
            get
            {
                return (Brush)GetValue(LabelForegroundProperty);
            }

            set
            {
                SetValue(LabelForegroundProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label text alignment.
        /// </summary>
        /// <value>
        /// Type: <see cref="TextAlignment"/>
        /// By default it is set to Center.
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
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.IsLabelEditable = true;
        /// n.Label = "Start";
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// n.ToolTip="Start Node";
        /// n.TextAlignment = TextAlignment.Left;
        /// Model.Nodes.Add(n);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        public TextAlignment LabelTextAlignment
        {
            get
            {
                return (TextAlignment)GetValue(LabelTextAlignmentProperty);
            }

            set
            {
                SetValue(LabelTextAlignmentProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable multiline label].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [enable multiline label]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableMultilineLabel
        {
            get
            {
                return (bool)GetValue(EnableMultilineLabelProperty);
            }
            set
            {
                SetValue(EnableMultilineLabelProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [was port visible].
        /// </summary>
        /// <value>
        /// Type: <see cref="Boolean"/>
        /// True, if it was visible, false otherwise.
        /// </value>
        internal bool WasPortVisible
        {
            get { return m_wasvisible; }
            set { m_wasvisible = value; }
        }
        internal bool AlwaysPortVisible;


        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Node"/> offset properties are set.
        /// </summary>
        /// <value><c>true</c> if isnodeexe; otherwise, <c>false</c>.</value>
        internal bool Isnodeexe
        {
            get { return m_isexe; }
            set { m_isexe = value; }
        }

        /// <summary>
        /// Gets or sets the state of the node in case of cycle detection.
        /// </summary>
        /// <value>The state of the node. (0-->non-visited; 1-->Visited and InProgress; 2-->Done)</value>
        internal int State
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this node can be connected to the other specified node in case of hierarchical-tree layout.
        /// </summary>
        /// <value>
        /// <c>true</c> if can connect; otherwise, <c>false</c>.
        /// </value>
        internal bool CanConnect
        {
            get { return canconn; }
            set { canconn = value; }
        }

        public bool AllowVirtualization
        {
            get { return NodeVirtual; }
            set { NodeVirtual = value; }

        }

        /// <summary>
        /// Gets or sets the width of the node in pixels.
        /// </summary>
        /// <value>The width of the node.</value>
        internal double PixelWidth
        {
            get { return pwidth; }
            set { pwidth = value; }
        }

        /// <summary>
        /// Gets or sets the height of the node in pixels.
        /// </summary>
        /// <value>The height of the node.</value>
        internal double PixelHeight
        {
            get { return pheight; }
            set { pheight = value; }
        }

        /// <summary>
        /// Gets or sets the Measurement unit property.
        /// <value>
        /// Type: <see cref="MeasureUnits"/>
        /// Enum specifying the unit to be used.
        /// </value>
        /// </summary>
        public MeasureUnits MeasurementUnits
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



        public Style LeftResizer
        {
            get { return (Style)GetValue(LeftResizerProperty); }
            set { SetValue(LeftResizerProperty, value); }
        }

        public Style RightResizer
        {
            get { return (Style)GetValue(RightResizerProperty); }
            set { SetValue(RightResizerProperty, value); }
        }
        public Style BottomResizer
        {
            get { return (Style)GetValue(BottomResizerProperty); }
            set { SetValue(BottomResizerProperty, value); }
        }

        public Style TopResizer
        {
            get { return (Style)GetValue(TopResizerProperty); }
            set { SetValue(TopResizerProperty, value); }
        }

        public Style TopLeftCornerResizer
        {
            get { return (Style)GetValue(TopLeftCornerResizerProperty); }
            set { SetValue(TopLeftCornerResizerProperty, value); }
        }

        public Style TopRightCornerResizer
        {
            get { return (Style)GetValue(TopRightCornerResizerProperty); }
            set { SetValue(TopRightCornerResizerProperty, value); }
        }

        public Style BottomLeftCornerResizer
        {
            get { return (Style)GetValue(BottomLeftCornerResizerProperty); }
            set { SetValue(BottomLeftCornerResizerProperty, value); }
        }

        public Style BottomRightCornerResizer
        {
            get { return (Style)GetValue(BottomRightCornerResizerProperty); }
            set { SetValue(BottomRightCornerResizerProperty, value); }
        }
  
        #endregion

        #region Dependency Properties
        
        public static readonly DependencyProperty LabelTextDecorationsProperty = DependencyProperty.Register("LabelTextDecorations", typeof(TextDecorationCollection), typeof(Node));
        public static readonly DependencyProperty BoundariesProperty = DependencyProperty.Register("Boundaries", typeof(Shape), typeof(Node));
        public static readonly DependencyProperty IntersectionModeProperty = DependencyProperty.Register("IntersectionMode", typeof(IntersectionMode), typeof(Node));
        /// <summary>
        /// Identifies the LabelAngle dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelAngleProperty = DependencyProperty.Register("LabelAngle", typeof(double), typeof(Node), new FrameworkPropertyMetadata(0d));

        /// <summary>
        /// Identifies the LogicalOffsetY dependency property.
        /// </summary>
        [Obsolete("Use OffsetYProperty")]
        public static readonly DependencyProperty LogicalOffsetYProperty = DependencyProperty.Register("LogicalOffsetY", typeof(double), typeof(Node), new FrameworkPropertyMetadata(0d, new PropertyChangedCallback(OnLogicalOffsetYChanged)));

        /// <summary>
        /// Identifies the LogicalOffsetX dependency property.
        /// </summary>
        [Obsolete("Use OffsetXProperty")]
        public static readonly DependencyProperty LogicalOffsetXProperty = DependencyProperty.Register("LogicalOffsetX", typeof(double), typeof(Node), new FrameworkPropertyMetadata(0d, new PropertyChangedCallback(OnLogicalOffsetXChanged)));

        /// <summary>
        /// Identifies the RotateAngle dependency property.
        /// </summary>
        public static readonly DependencyProperty RotateAngleProperty = DependencyProperty.Register("RotateAngle", typeof(double), typeof(Node), new FrameworkPropertyMetadata(0d, new PropertyChangedCallback(OnRotateAngleChanged)));

        /// <summary>
        /// Identifies the LabelWidth dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelWidthProperty = DependencyProperty.Register("LabelWidth", typeof(double), typeof(Node), new FrameworkPropertyMetadata(double.NaN));

        /// <summary>
        /// Identifies the LabelHeight dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelHeightProperty = DependencyProperty.Register("LabelHeight", typeof(double), typeof(Node));

        /// <summary>
        /// Identifies the LabelTextWrapping dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelTextWrappingProperty = DependencyProperty.Register("LabelTextWrapping", typeof(TextWrapping), typeof(Node), new FrameworkPropertyMetadata(TextWrapping.NoWrap));

        /// <summary>
        /// Identifies the LabelFontSize dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelFontSizeProperty = DependencyProperty.Register("LabelFontSize", typeof(double), typeof(Node), new FrameworkPropertyMetadata(11d));

        /// <summary>
        /// Identifies the LabelFontFamily dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelFontFamilyProperty = DependencyProperty.Register("LabelFontFamily", typeof(FontFamily), typeof(Node), new FrameworkPropertyMetadata(new FontFamily("Arial")));

        /// <summary>
        /// Identifies the LabelFontWeight dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelFontWeightProperty = DependencyProperty.Register("LabelFontWeight", typeof(FontWeight), typeof(Node), new FrameworkPropertyMetadata(FontWeights.SemiBold));

        /// <summary>
        /// Identifies the LabelFontStyle dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelFontStyleProperty = DependencyProperty.Register("LabelFontStyle", typeof(FontStyle), typeof(Node), new FrameworkPropertyMetadata(FontStyles.Normal));

        /// <summary>
        /// Identifies the LabelTextTrimming dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelTextTrimmingProperty = DependencyProperty.Register("LabelTextTrimming", typeof(TextTrimming), typeof(Node), new FrameworkPropertyMetadata(TextTrimming.CharacterEllipsis));

        /// <summary>
        /// Identifies the LabelTextAlignment dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelTextAlignmentProperty = DependencyProperty.Register("LabelTextAlignment", typeof(TextAlignment), typeof(Node), new FrameworkPropertyMetadata(TextAlignment.Center));

        /// <summary>
        /// Identifies the GripperVisibility dependency property.
        /// </summary>
        public static readonly DependencyProperty GripperVisibilityProperty = DependencyProperty.Register("GripperVisibility", typeof(Visibility), typeof(Node), new UIPropertyMetadata(Visibility.Hidden));

        /// <summary>
        /// Identifies the GripperStyle dependency property.
        /// </summary>
        public static readonly DependencyProperty GripperStyleProperty = DependencyProperty.Register("GripperStyle", typeof(Style), typeof(Node));

        /// <summary>
        ///  Identifies the LabelBackground dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelBackgroundProperty = DependencyProperty.Register("LabelBackground", typeof(Brush), typeof(Node), new FrameworkPropertyMetadata(Brushes.Transparent));

        /// <summary>
        ///  Identifies the LabelForeground dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelForegroundProperty = DependencyProperty.Register("LabelForeground", typeof(Brush), typeof(Node), new FrameworkPropertyMetadata(Brushes.Black));

        /// <summary>
        /// Identifies the IsLabelEditable dependency property.
        /// </summary>
        public static readonly DependencyProperty IsLabelEditableProperty = DependencyProperty.Register("IsLabelEditable", typeof(bool), typeof(Node), new UIPropertyMetadata(true));

        /// <summary>
        /// Identifies the AllowSelect dependency property.
        /// </summary>
        public static readonly DependencyProperty AllowSelectProperty = DependencyProperty.Register("AllowSelect", typeof(bool), typeof(Node), new UIPropertyMetadata(true));


        internal static readonly DependencyProperty RectBoundsProperty = DependencyProperty.Register("Rectbounds", typeof(Rect), typeof(Node), new PropertyMetadata(Rect.Empty));
        /// <summary>
        /// Identifies the AllowMove dependency property.
        /// </summary>
        public static readonly DependencyProperty AllowMoveProperty = DependencyProperty.Register("AllowMove", typeof(bool), typeof(Node), new UIPropertyMetadata(true));

        /// <summary>
        /// Identifies the AllowRotate dependency property.
        /// </summary>
        public static readonly DependencyProperty AllowRotateProperty = DependencyProperty.Register("AllowRotate", typeof(bool), typeof(Node), new UIPropertyMetadata(true, new PropertyChangedCallback(OnAllowRotatePropertyChanged)));

        /// <summary>
        /// Identifies the AllowResize dependency property.
        /// </summary>
        public static readonly DependencyProperty AllowResizeProperty = DependencyProperty.Register("AllowResize", typeof(bool), typeof(Node), new UIPropertyMetadata(true));

        /// <summary>
        /// Identifies the AllowPortDrag dependency property.
        /// </summary>
        public static readonly DependencyProperty AllowPortDragProperty = DependencyProperty.Register("AllowPortDrag", typeof(bool), typeof(Node), new UIPropertyMetadata(true));

        /// <summary>
        /// Identifies the AllowDelete dependency property.
        /// </summary>
        public static readonly DependencyProperty AllowDeleteProperty = DependencyProperty.Register("AllowDelete", typeof(bool), typeof(Node), new UIPropertyMetadata(true, new PropertyChangedCallback(OnAllowDeletePropertyChanged)));

        /// <summary>
        /// Identifies the MeasurementUnits property.
        /// </summary>
        public static readonly DependencyProperty MeasurementUnitsProperty = DependencyProperty.Register("MeasurementUnits", typeof(MeasureUnits), typeof(Node), new PropertyMetadata(MeasureUnits.Pixel, new PropertyChangedCallback(OnUnitsChanged)));

        /// <summary>
        /// Identifies the ParentId dependency property.
        /// </summary>
        public static readonly DependencyProperty ParentIDProperty = DependencyProperty.Register("ParentID", typeof(Guid), typeof(Node));

        /// <summary>
        /// Identifies the IsGroup dependency property.
        /// </summary>
        public static readonly DependencyProperty IsGroupedProperty = DependencyProperty.Register("IsGrouped", typeof(bool), typeof(Node));

        /// <summary>
        /// Identifies the IsSelected dependency property.
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(Node), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnIsSelectedChanged)));

        /// <summary>
        /// Identifies the DragProviderTemplate dependency property.
        /// </summary>
        public static readonly DependencyProperty DragProviderTemplateProperty = DependencyProperty.RegisterAttached("DragProviderTemplate", typeof(ControlTemplate), typeof(Node));

        /// <summary>
        /// Identifies the IsDragConnectionOver dependency property.
        /// </summary>
        public static readonly DependencyProperty IsDragConnectionOverProperty = DependencyProperty.Register("IsDragConnectionOver", typeof(bool), typeof(Node), new FrameworkPropertyMetadata(false));

        /// <summary>
        /// Identifies the Shape dependency property.
        /// </summary>
        public static readonly DependencyProperty ShapeProperty = DependencyProperty.Register("Shape", typeof(Shapes), typeof(Node), new FrameworkPropertyMetadata(Shapes.Default));

        /// <summary>
        /// Identifies the CustomPathStyle dependency property.
        /// </summary>
        public static readonly DependencyProperty CustomPathStyleProperty = DependencyProperty.Register("CustomPathStyle", typeof(Style), typeof(Node), new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Identifies the Level dependency property.
        /// </summary>
        public static readonly DependencyProperty LevelProperty = DependencyProperty.Register("Level", typeof(int), typeof(Node), new FrameworkPropertyMetadata(0));

        /// <summary>
        /// Identifies the LabelVerticalAlignment dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelVerticalAlignmentProperty = DependencyProperty.Register("LabelVerticalAlignment", typeof(VerticalAlignment), typeof(Node), new FrameworkPropertyMetadata(VerticalAlignment.Top));

        /// <summary>
        /// Identifies the LabelHorizontalAlignment dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelHorizontalAlignmentProperty = DependencyProperty.Register("LabelHorizontalAlignment", typeof(HorizontalAlignment), typeof(Node), new FrameworkPropertyMetadata(HorizontalAlignment.Center));

        /// <summary>
        /// Identifies the Label dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(Node), new UIPropertyMetadata(string.Empty, new PropertyChangedCallback(OnLabelChanged)));

        /// <summary>
        /// Identifies the LabelVisibility dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelVisibilityProperty = DependencyProperty.Register("LabelVisibility", typeof(Visibility), typeof(Node), new UIPropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Identifies the PortVisibility dependency property.
        /// </summary>
        public static readonly DependencyProperty PortVisibilityProperty = DependencyProperty.Register("PortVisibility", typeof(Visibility), typeof(Node), new UIPropertyMetadata(Visibility.Collapsed));

        /// <summary>
        /// Identifies the IsCenterPortEnabled dependency property.
        /// </summary>
        public static readonly DependencyProperty IsPortEnabledProperty = DependencyProperty.Register("IsPortEnabled", typeof(bool), typeof(Node), new UIPropertyMetadata(true));

        /// <summary>
        /// Identifies the IsDoubleClicked dependency property.
        /// </summary>
        public static readonly DependencyProperty IsDoubleClickedProperty = DependencyProperty.Register("IsDoubleClicked", typeof(bool), typeof(Node), new UIPropertyMetadata(true));
        /// <summary>
        /// Identifies the EnableMultilineLabel dependency Property.
        /// </summary>
        public static readonly DependencyProperty EnableMultilineLabelProperty = DependencyProperty.Register("EnableMultilineLabel", typeof(bool), typeof(Node), new PropertyMetadata(false));

        public static readonly DependencyProperty LeftResizerProperty = DependencyProperty.Register("LeftResizer", typeof(Style), typeof(Node), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty RightResizerProperty = DependencyProperty.Register("RightResizer", typeof(Style), typeof(Node), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty BottomResizerProperty = DependencyProperty.Register("BottomResizer", typeof(Style), typeof(Node), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty TopResizerProperty = DependencyProperty.Register("TopResizer", typeof(Style), typeof(Node), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty TopLeftCornerResizerProperty = DependencyProperty.Register("TopLeftCornerResizer", typeof(Style), typeof(Node), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty TopRightCornerResizerProperty = DependencyProperty.Register("TopRightCornerResizer", typeof(Style), typeof(Node), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty BottomLeftCornerResizerProperty = DependencyProperty.Register("BottomLeftCornerResizer", typeof(Style), typeof(Node), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty BottomRightCornerResizerProperty = DependencyProperty.Register("BottomRightCornerResizer", typeof(Style), typeof(Node), new FrameworkPropertyMetadata(null));
  
        #endregion

        #region Events

        /// <summary>
        /// Raises the click event.
        /// </summary>
        public void RaiseClickEvent()
        {
            NodeRoutedEventArgs newEventArgs = new NodeRoutedEventArgs(this);
            newEventArgs.RoutedEvent = DiagramView.NodeClickEvent;
            RaiseEvent(newEventArgs);
        }

        /// <summary>
        /// Raises the drag start event.
        /// </summary>
        public void RaiseNodeDragStartEvent()
        {
            if (Node.mouseup && dview.IsPageEditable)
            {
                NodeRoutedEventArgs newEventArgs = new NodeRoutedEventArgs(this);
                newEventArgs.RoutedEvent = DiagramView.NodeDragStartEvent;
                RaiseEvent(newEventArgs);
                Node.mouseup = false;
            }
        }

        /// <summary>
        /// Raises the drag end event.
        /// </summary>
        public void RaiseNodeDragEndEvent()
        {
            if (dview.IsPageEditable)
            {
                NodeRoutedEventArgs newEventArgs = new NodeRoutedEventArgs(this);
                newEventArgs.RoutedEvent = DiagramView.NodeDragEndEvent;
                RaiseEvent(newEventArgs);
            }
        }

        /// <summary>
        /// Raises the double click event.
        /// </summary>
        public void RaiseDoubleClickEvent()
        {
            NodeRoutedEventArgs newEventArgs = new NodeRoutedEventArgs(this);
            newEventArgs.RoutedEvent = DiagramView.NodeDoubleClickEvent;
            RaiseEvent(newEventArgs);
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
        /// <param name="name">The property name </param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                if (dview != null)
                {
                    if (dview.IsPageSaved)
                    {
                        foreach (DiagramProperty d in dview.DiagramProperties.Where(item => item.ObjectType.Equals(typeof(Node))))
                        {
                            if (d.PropertyName.Equals(name))
                            {
                                dview.IsPageSaved = false;
                            }
                        }
                    }
                }
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        /// <summary>
        /// Invoked whenever the effective value of any dependency property on this <see cref="T:System.Windows.FrameworkElement"/> has been updated. The specific dependency property that changed is reported in the arguments parameter. Overrides <see cref="M:System.Windows.DependencyObject.OnPropertyChanged(System.Windows.DependencyPropertyChangedEventArgs)"/>.
        /// </summary>
        /// <param name="e">The event data that describes the property that changed, as well as old and new values.</param>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                if (dview != null)
                {
                    if (dview.IsPageSaved)
                    {
                        foreach (DiagramProperty d in dview.DiagramProperties.Where(item => item.ObjectType.Equals(typeof(Node))))
                        {
                            if (d.PropertyName.Equals(e.Property.Name))
                            {
                                dview.IsPageSaved = false;
                            }
                        }
                    }
                }
                handler(this, new PropertyChangedEventArgs(e.Property.ToString()));
            }
        }

        #endregion

        #region class override

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate"/>.
        /// </summary>
        public override void OnApplyTemplate()
        {
            editor = GetTemplateChild("PART_LabelEditor") as LabelEditor;
            rotatethumb = GetTemplateChild("PART_Rotator") as Control;
            grouprotatethumb = GetTemplateChild("PART_Rotator1") as Control;
            dview = GetDiagramView(this);

            if (dview != null)
            {
                //this.MeasurementUnits = (dview.Page as DiagramPage).MeasurementUnits;
                if (!dview.SelectionList.Contains(this) && this.IsSelected)
                {
                    this.NodeSelection();
                }
                //if(this.IsSelected)
                //(dview as DiagramView).SelectionList.Add(this);
            }

            //this.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            //if (double.IsNaN(this.Width))
            //{
            //    this.Width = DesiredSize.Width;
            //}
            //if (double.IsNaN(this.Height))
            //{
            //    this.Height = DesiredSize.Height;
            //}

            if (!DiagramControl.IsPageLoaded)
            {
                ConnectionPort centerport = new ConnectionPort();
                centerport.Name = "PART_Sync_CenterPort";
                centerport.Node = this;
                centerport.PortShape = PortShapes.Circle;
                if (!double.IsNaN(this.Width) || !double.IsNaN(this.Height))
                {
                    if (double.IsNaN(centerport.Width) || double.IsNaN(centerport.Height))
                    {
                        centerport.Left = this.Width / 2 ;
                        centerport.Top = this.Height / 2 ;
                    }
                    else
                    {
                        centerport.Left = this.Width / 2 - centerport.Width / 2;
                        centerport.Top = this.Height / 2 - centerport.Height / 2;
                    }
                }
                else
                {
                    double two0ne = 21;// MeasureUnitsConverter.FromPixels(21, this.MeasurementUnits);
                    centerport.Left = two0ne;
                    centerport.Top = two0ne;
                }

                if (this.Ports != null)
                {
                    //this.Ports.Add(centerport);
                }

                centerport.CenterPortReferenceNo = 0;
            }
            dc = DiagramPage.GetDiagramControl(this);
            if (dc != null)
            {
                ContextMenu_Delete = dc.m_ResourceWrapper.ContextMenu_Delete;
                ContextMenu_Grouping = dc.m_ResourceWrapper.ContextMenu_Grouping;
                ContextMenu_Grouping_Group = dc.m_ResourceWrapper.ContextMenu_Grouping_Group;
                ContextMenu_Grouping_Ungroup = dc.m_ResourceWrapper.ContextMenu_Grouping_Ungroup;
                ContextMenu_Order = dc.m_ResourceWrapper.ContextMenu_Order;
                ContextMenu_Order_BringForward = dc.m_ResourceWrapper.ContextMenu_Order_BringForward;
                ContextMenu_Order_BringToFront = dc.m_ResourceWrapper.ContextMenu_Order_BringToFront;
                ContextMenu_Order_SendBackward = dc.m_ResourceWrapper.ContextMenu_Order_SendBackward;
                ContextMenu_Order_SendToBack = dc.m_ResourceWrapper.ContextMenu_Order_SendToBack;
            }
        }


        private void NodeSelection()
        {
            
            dview = GetDiagramView(this);
            //if(this.IsSelected)
            //(dview as DiagramView).SelectionList.Add(this);
            Node n = this as Node;
            if (n.IsSelected && n.Page != null && (n.Page as DiagramPage).SelectionList != null)
            {
                (n.Page as DiagramPage).SelectionList.Add(n);
            }
            else if (!n.IsSelected && n.Page != null && (n.Page as DiagramPage).SelectionList != null && !(n.Page as DiagramPage).SelectionList.m_collClear)
            {
                (n.Page as DiagramPage).SelectionList.Remove(n);
            }
            if ((n.GetTemplateChild("PART_Rotator") as Rotator) != null)
            {
                if (n.AllowRotate && n.IsSelected)
                {
                    (n.GetTemplateChild("PART_Rotator") as Rotator).Visibility = Visibility.Visible;
                }
                else
                {
                    (n.GetTemplateChild("PART_Rotator") as Rotator).Visibility = Visibility.Collapsed;
                }
            }
            if (!n.IsSelected)
            {
                NodeRoutedEventArgs newEventArgs = new NodeRoutedEventArgs(n as Node);
                newEventArgs.RoutedEvent = DiagramView.NodeUnSelectedEvent;
                n.RaiseEvent(newEventArgs);
                //  RaiseEvent(newEventArgs);
            }
            else
            {
                NodeRoutedEventArgs newEventArgs = new NodeRoutedEventArgs(n as Node);
                newEventArgs.RoutedEvent = DiagramView.NodeSelectedEvent;
                n.RaiseEvent(newEventArgs);
            }

        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Controls.Control.MouseDoubleClick"/> routed event.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            isdoubleclicked = true;
            if (dview.IsPageEditable)
            {
                RaiseDoubleClickEvent();
                NodeLabeledit();
            }
        }

        /// <summary>
        /// Provides class handling for the MouseDown routed event that occurs when the mouse 
        /// button is pressed while the mouse pointer is over this control.
        /// </summary>
        /// <param name="e">The MouseButtonEventArgs</param>
        //protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
        //{
        //    base.OnPreviewMouseRightButtonDown(e);
        //    if (dc.View.IsPageEditable && !dview.IsPanEnabled && this.AllowSelect)
        //    {
        //        ////base.OnPreviewMouseLeftButtonDown(e);

        //        IDiagramPage diagramPanel = VisualTreeHelper.GetParent(this) as IDiagramPage;
        //        if (diagramPanel != null)
        //        {
        //            if (!this.IsGrouped)
        //            {
        //                ////diagramPanel.SelectionList.Clear();
        //                if (!this.IsSelected)
        //                {
        //                    diagramPanel.SelectionList.Clear();
        //                    diagramPanel.SelectionList.Add(this);
        //                }
        //            }
        //            else
        //            {
        //                bool anyOneSelected = this.IsSelected;
        //                if (!anyOneSelected)
        //                {
        //                    foreach (Group g in this.Groups)
        //                    {
        //                        if (g.IsSelected == true)
        //                        {
        //                            anyOneSelected = true;
        //                        }
        //                    }
        //                }
        //                if (!anyOneSelected)
        //                {
        //                    diagramPanel.SelectionList.Clear();
        //                    diagramPanel.SelectionList.Add(this);
        //                }
        //            }
        //        }
        //    }
        //}

        internal string ContextMenu_Delete;
        internal string ContextMenu_Grouping;
        internal string ContextMenu_Grouping_Group;
        internal string ContextMenu_Grouping_Ungroup;
        internal string ContextMenu_Order;
        internal string ContextMenu_Order_BringForward;
        internal string ContextMenu_Order_BringToFront;
        internal string ContextMenu_Order_SendBackward;
        internal string ContextMenu_Order_SendToBack;

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.UIElement.MouseRightButtonUp"/> routed event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs"/> that contains the event data. The event data reports that the right mouse button was released.</param>
        protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        {
            if (dview != null)
            {

                if (dc.View.Page != null && (dc.View.Page is DiagramPage))
                {
                    TimeSpan diff = DateTime.Now.Subtract(dc.View.timeRightClick);
                    int d = diff.CompareTo(new TimeSpan(0, 0, 0, 0, 500));
                    if (d == 1)
                    {


                        if (dview.IsPageEditable && dview.NodeContextMenu == null && this.ContextMenu == null)
                        {
                            MenuItem Order = new MenuItem();
                            Order.Header = ContextMenu_Order;// "Order";
                            MenuItem Front = new MenuItem();
                            Front.Header = ContextMenu_Order_BringToFront;// "Bring To Front";
                            Front.Click += new RoutedEventHandler(Front_Click);
                            Order.Items.Add(Front);
                            MenuItem Forward = new MenuItem();
                            Forward.Header = ContextMenu_Order_BringForward;// "Bring Forward";
                            Forward.Click += new RoutedEventHandler(Forward_Click);
                            Order.Items.Add(Forward);
                            MenuItem Backward = new MenuItem();
                            Backward.Header = ContextMenu_Order_SendBackward;// "Send Backward";
                            Backward.Click += new RoutedEventHandler(Backward_Click);
                            Order.Items.Add(Backward);
                            MenuItem Back = new MenuItem();
                            Back.Header = ContextMenu_Order_SendToBack;// "Send To Back";
                            Back.Click += new RoutedEventHandler(Back_Click);
                            Order.Items.Add(Back);

                            MenuItem Grouping = new MenuItem();
                            Grouping.Header = ContextMenu_Grouping;// "Grouping";
                            MenuItem Group = new MenuItem();
                            Group.Header = ContextMenu_Grouping_Group;// "Group";
                            Group.Click += new RoutedEventHandler(Group_Click);
                            Grouping.Items.Add(Group);
                            MenuItem Ungroup = new MenuItem();
                            Ungroup.Header = ContextMenu_Grouping_Ungroup;// "Ungroup";
                            Ungroup.Click += new RoutedEventHandler(Ungroup_Click);
                            Grouping.Items.Add(Ungroup);

                            MenuItem Delete = new MenuItem();
                            Delete.Header = ContextMenu_Delete;// "Delete";
                            Delete.Click += new RoutedEventHandler(Del_Click);
                            ContextMenu nodecontextmenu = new ContextMenu();
                            nodecontextmenu.Items.Add(Order);
                            nodecontextmenu.Items.Add(Grouping);
                            nodecontextmenu.Items.Add(Delete);



                            setDisableOrEnable(Front, Forward, Backward, Back, Delete, Group);

                            if (this.ContextMenu == null)
                            {
                                this.ContextMenu = nodecontextmenu;
                            }
                        }
                        else if (this.ContextMenu == null)
                        {
                            {
                                this.ContextMenu = dview.NodeContextMenu;
                            }
                        }
                        else
                        {
                            //Refresh reference  context menu.
                            if (this.ContextMenu != null)
                            {
                                if (!isCustomContextMenu())
                                {
                                    ContextMenu nodecontextmenu = this.ContextMenu;
                                    MenuItem Order = nodecontextmenu.Items[0] as MenuItem;
                                    MenuItem Grouping = nodecontextmenu.Items[1] as MenuItem;
                                    MenuItem Delete = nodecontextmenu.Items[2] as MenuItem;
                                    Delete.Click -= new RoutedEventHandler(Del_Click);
                                    Delete.Click += new RoutedEventHandler(Del_Click);

                                    MenuItem Front = Order.Items[0] as MenuItem;
                                    Front.Click -= new RoutedEventHandler(Front_Click);
                                    Front.Click += new RoutedEventHandler(Front_Click);
                                    MenuItem Forward = Order.Items[1] as MenuItem;
                                    Forward.Click -= new RoutedEventHandler(Forward_Click);
                                    Forward.Click += new RoutedEventHandler(Forward_Click);
                                    MenuItem Backward = Order.Items[2] as MenuItem;
                                    Backward.Click -= new RoutedEventHandler(Backward_Click);
                                    Backward.Click += new RoutedEventHandler(Backward_Click);
                                    MenuItem Back = Order.Items[3] as MenuItem;
                                    Back.Click -= new RoutedEventHandler(Back_Click);
                                    Back.Click += new RoutedEventHandler(Back_Click);
                                    MenuItem Group = Grouping.Items[0] as MenuItem;
                                    Group.Click -= new RoutedEventHandler(Group_Click);
                                    Group.Click += new RoutedEventHandler(Group_Click);
                                    MenuItem Ungroup = Grouping.Items[1] as MenuItem;
                                    Ungroup.Click -= new RoutedEventHandler(Ungroup_Click);
                                    Ungroup.Click += new RoutedEventHandler(Ungroup_Click);

                                    setDisableOrEnable(Front, Forward, Backward, Back, Delete, Group);
                                }
                            }
                        }
                    }
                }
            }
            base.OnMouseRightButtonUp(e);
        }

        private bool isCustomContextMenu()
        {
            if (this.ContextMenu != null)
            {

                ContextMenu nodecontextmenu = this.ContextMenu;
                if (nodecontextmenu.Items.Count == 3 &&
                    nodecontextmenu.Items[0] is MenuItem &&
                        nodecontextmenu.Items[1] is MenuItem &&
                        nodecontextmenu.Items[2] is MenuItem)
                {

                    MenuItem Order = nodecontextmenu.Items[0] as MenuItem;
                    MenuItem Grouping = nodecontextmenu.Items[1] as MenuItem;
                    MenuItem Delete = nodecontextmenu.Items[2] as MenuItem;

                    if (Order.Header.Equals(ContextMenu_Order) &&
                        Grouping.Header.Equals(ContextMenu_Grouping) &&
                        Delete.Header.Equals(ContextMenu_Delete)
                        )
                    {
                    }
                    else
                    {
                        return true;
                    }

                    if (Order.Items.Count == 4 &&
                        Order.Items[0] is MenuItem &&
                            Order.Items[1] is MenuItem &&
                            Order.Items[2] is MenuItem &&
                            Order.Items[3] is MenuItem)
                    {

                        MenuItem Front = Order.Items[0] as MenuItem;
                        MenuItem Forward = Order.Items[1] as MenuItem;
                        MenuItem Backward = Order.Items[2] as MenuItem;
                        MenuItem Back = Order.Items[3] as MenuItem;
                        if (Front.Header.Equals(ContextMenu_Order_BringToFront) &&
                        Forward.Header.Equals(ContextMenu_Order_BringForward) &&
                        Backward.Header.Equals(ContextMenu_Order_SendBackward) &&
                            Back.Header.Equals(ContextMenu_Order_SendToBack)
                        )
                        {
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                    if (Grouping.Items.Count == 2 &&
                        Grouping.Items[0] is MenuItem &&
                            Grouping.Items[1] is MenuItem)
                    {

                        MenuItem Group = Grouping.Items[0] as MenuItem;
                        MenuItem Ungroup = Grouping.Items[1] as MenuItem;
                        if (Group.Header.Equals(ContextMenu_Grouping_Group) &&
                        Ungroup.Header.Equals(ContextMenu_Grouping_Ungroup)
                        )
                        {
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }

            }
            return false;
        }

        private void setDisableOrEnable(MenuItem Front, MenuItem Forward, MenuItem Backward, MenuItem Back, MenuItem Delete, MenuItem Group)
        {

            if (this.IsSelected)
            {
                Front.IsEnabled = true;
                Forward.IsEnabled = true;
                Backward.IsEnabled = true;
                Back.IsEnabled = true;
                Delete.IsEnabled = true;
            }
            else
            {
                Front.IsEnabled = false;
                Forward.IsEnabled = false;
                Backward.IsEnabled = false;
                Back.IsEnabled = false;
                Delete.IsEnabled = false;
            }

            if (this.IsGrouped)
            {
                foreach (Group g in this.Groups)
                {
                    if (g.IsSelected)
                    {
                        Delete.IsEnabled = true;
                    }
                }
            }

            if (dview != null && dview.SelectionList.Count > 1 && !this.IsGrouped)
            {
                Group.IsEnabled = true;
            }
            else
            {
                foreach (INodeGroup item in dview.SelectionList)
                {
                    if (item is Group)
                    {
                        foreach (INodeGroup n in (item as Group).NodeChildren)
                        {
                            foreach (INodeGroup node in dview.SelectionList)
                            {
                                if (!(item as Group).NodeChildren.Contains(node) && !(node is Group))
                                {
                                    Group.IsEnabled = true;
                                    break;
                                }
                                else
                                {
                                    Group.IsEnabled = false;
                                }
                            }
                        }
                    }
                }
            }

            if (dview.SelectionList.Count <= 1)
            {
                Group.IsEnabled = false;
            }
        }

        /// <summary>
        /// Handles the Click event of the delete menu item.
        /// </summary>
        /// <param name="sender">The diagram view.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Del_Click(object sender, RoutedEventArgs e)
        {
            DiagramCommandManager.Delete.Execute(dview.Page, dview);
        }

        /// <summary>
        /// Handles the Click event of the bring to front menu item.
        /// </summary>
        /// <param name="sender">The diagram view.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Front_Click(object sender, RoutedEventArgs e)
        {
            DiagramCommandManager.BringToFront.Execute(dview.Page, dview);
        }

        /// <summary>
        /// Handles the Click event of the bring forward menu item.
        /// </summary>
        /// <param name="sender">The diagram view.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            DiagramCommandManager.MoveForward.Execute(dview.Page, dview);
        }

        /// <summary>
        /// Handles the Click event of the send backward menu item.
        /// </summary>
        /// <param name="sender">The diagram view.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Backward_Click(object sender, RoutedEventArgs e)
        {
            DiagramCommandManager.SendBackward.Execute(dview.Page, dview);
        }

        /// <summary>
        /// Handles the Click event of the send to back menu item.
        /// </summary>
        /// <param name="sender">The diagram view.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            DiagramCommandManager.SendToBack.Execute(dview.Page, dview);
        }

        /// <summary>
        /// Handles the Click event of the group menu item.
        /// </summary>
        /// <param name="sender">The diagram view.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Group_Click(object sender, RoutedEventArgs e)
        {
            DiagramCommandManager.Group.Execute(dview.Page, dview);
        }

        /// <summary>
        /// Handles the Click event of the ungroup menu item.
        /// </summary>
        /// <param name="sender">The diagram view.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Ungroup_Click(object sender, RoutedEventArgs e)
        {
            DiagramCommandManager.Ungroup.Execute(dview.Page, dview);
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.UIElement.PreviewMouseLeftButtonDown"/> routed event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs"/> that contains the event data. The event data reports that the left mouse button was pressed.</param>
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            if (!Node.IsInSelectionScope(this, (DependencyObject)e.OriginalSource) )
            {
                ex = false;
                if (dview.IsPageEditable && !this.IsGrouped)
                {
                    if (!dview.IsPanEnabled)
                    {
                        base.OnPreviewMouseLeftButtonDown(e);
                        IDiagramPage m_diagramPage = VisualTreeHelper.GetParent(this) as IDiagramPage;

                        //// update selection
                        if (m_diagramPage != null)
                        {
                            if (dview.EnableConnection)
                            {
                                this.startPoint = e.GetPosition(m_diagramPage as Panel);
                                e.Handled = true;
                            }

                            if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) != ModifierKeys.None)
                            {
                                if (this.IsSelected)
                                {
                                    (dview as DiagramView).SelectionList.Remove(this);
                                }
                                else
                                {
                                    if (this.AllowSelect)
                                    {
                                        (dview as DiagramView).SelectionList.Add(this);
                                    }
                                }
                            }
                            else if (!this.IsSelected)
                            {
                                if (m_diagramPage != null)
                                {
                                    if (dview.EnableConnection)
                                    {
                                        this.startPoint = e.GetPosition(m_diagramPage as Panel);
                                        e.Handled = true;
                                    }
                                }

                                if (this.AllowSelect)
                                {
                                    m_diagramPage.SelectionList.Select(this);
                                }
                            }

                            this.Focus();
                        }

                        lastNodeClick = DateTime.Now;
                        lastNodePoint = e.GetPosition(m_diagramPage as Panel);
                    }
                }
                else
                    if (dview.IsPageEditable && this.IsGrouped)
                    {
                        if (!dview.IsPanEnabled)
                        {
                            base.OnPreviewMouseLeftButtonDown(e);
                            IDiagramPage m_diagramPage = VisualTreeHelper.GetParent(this) as IDiagramPage;

                            //// update selection
                            if (m_diagramPage != null)
                            {
                                if (dview.EnableConnection)
                                {
                                    this.startPoint = e.GetPosition(m_diagramPage as Panel);
                                    e.Handled = true;
                                }
                            }
                        }
                    }
            }
            else
            {
            }
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            foreach (Node allnode in dc.Model.Nodes)
            {
                if (allnode.PortVisibility == Visibility.Visible)
                {
                    allnode.AlwaysPortVisible = true;
                }
                else
                {
                    allnode.AlwaysPortVisible = false;
                }
            }

            base.OnMouseEnter(e);
        }
        /// <summary>
        /// Provides class handling for the MouseMove routed event that occurs when the mouse 
        /// pointer  is over this control.
        /// </summary>
        /// <param name="e">The MouseEventArgs</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (dview.IsPageEditable)
            {
                foreach (Node n in dc.Model.Nodes)
                {

                    if (n.AlwaysPortVisible)
                    {
                        n.PortVisibility = Visibility.Visible;
                    }
                    else
                    {
                        n.PortVisibility = Visibility.Collapsed;
                    }
                }
                if (IsPortEnabled)
                {

                    if (this.PortVisibility != Visibility.Visible)
                    {
                        portvisibilitycheck = true;
                        this.PortVisibility = Visibility.Visible;
                    }
                }

                if (!dview.IsPageEditable || !this.AllowMove || !this.AllowSelect)
                {
                    this.Cursor = System.Windows.Input.Cursors.Arrow;
                }

                base.OnMouseMove(e);

                if (e.LeftButton != MouseButtonState.Pressed)
                {
                    this.startPoint = null;
                }

                if (this.startPoint.HasValue)
                {
                    IDiagramPage m_diagramPanel = GetPanel(this);

                    if (m_diagramPanel != null)
                    {
                        if (!IsDoubleClick(e.GetPosition(m_diagramPanel as DiagramPage)))
                        {
                            AdornerLayer adorner = AdornerLayer.GetAdornerLayer(m_diagramPanel as Panel);
                            if (adorner != null)
                            {
                                foreach (ConnectionPort port in this.Ports)
                                {
                                    if (port.Ismouseover)
                                    {
                                        sourceHitPort = port;
                                    }
                                }

                                NodeConnectorAdorner nodeadorner = new NodeConnectorAdorner(m_diagramPanel, sourceHitPort, this, GetDiagramView(this));
                                dview.EnableConnection = false;
                                if (nodeadorner != null)
                                {
                                    adorner.Add(nodeadorner);
                                    e.Handled = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseLeave"/> attached event is raised on this element. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (IsPortEnabled && portvisibilitycheck)
            {
                foreach (Node n in dc.Model.Nodes)
                {
                    if (n.AlwaysPortVisible)
                    {
                        n.PortVisibility = Visibility.Visible;

                    }
                    else
                    {
                        n.PortVisibility = Visibility.Collapsed;
                    }
                }
                //this.PortVisibility = Visibility.Collapsed;
                portvisibilitycheck = false;
            }
        }

        #endregion

        #region Implementation

        internal static double Round(double value, double cutoff)
        {
            return (Math.Floor(value / cutoff) + (value % cutoff > cutoff / 2 ? 1 : 0)) * cutoff;
        }

        internal static void setINodeBinding(Node n, INode i)
        {
            if (n != null && i != null)
            {
                Binding X = new Binding("OffsetX");
                X.Source = i;
                X.Mode = BindingMode.TwoWay;
                n.SetBinding(Node.OffsetXProperty, X);

                Binding Y = new Binding("OffsetY");
                Y.Source = i;
                Y.Mode = BindingMode.TwoWay;
                n.SetBinding(Node.OffsetYProperty, Y);

                //Binding height = new Binding("Height");
                //height.Source = i;
                //height.Mode = BindingMode.TwoWay;
                //n.SetBinding(Node.HeightProperty, height);

                //Binding width = new Binding("Width");
                //width.Source = i;
                //width.Mode = BindingMode.TwoWay;
                //n.SetBinding(Node.WidthProperty, width);

                //Binding shape = new Binding("Shape");
                //shape.Source = i;
                //shape.Mode = BindingMode.TwoWay;
                //n.SetBinding(Node.ShapeProperty, shape);

                //Binding zorder = new Binding("Zorder");
                //zorder.Source = i;
                //zorder.Mode = BindingMode.TwoWay;
                ////n.SetBinding(Node., zorder);
            }
        }

        /// <summary>
        /// Invoked when Label editing is started.
        /// </summary>
        public void NodeLabeledit()
        {
            if (IsLabelEditable)
            {
                if (editor != null)
                {
                    editor.LabelEditStartInternal(editor);
                }
            }
        }

        /// <summary>
        /// Invoked when label editing is complete.
        /// </summary>
        internal void CompleteEditing()
        {
            if (editor != null)
            {
                editor.CompleteHeaderEditInternal(editor, true);
            }
        }

        /// <summary>
        /// Called when the mouse button is clicked twice.
        /// </summary>
        /// <param name="position">Mouse Position</param>
        /// <returns>true if double clicked, false otherwise</returns>
        public bool IsDoubleClick(Point position)
        {
            if (((DateTime.Now.Subtract(lastNodeClick).TotalMilliseconds < 500) && (Math.Abs((double)(lastNodePoint.X - position.X)) <= 2)) && (Math.Abs((double)(lastNodePoint.Y - position.Y)) <= 2))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Handles the MouseLeftButtonUp event of the Node control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Node_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            bool notselected = true;
            bool selected = true;
            if (!dview.Ispositionchanged && this.IsGrouped)
            {
                if (dview.IsPageEditable)
                {
                    if (!dview.IsPanEnabled && !isdoubleclicked)
                    {
                        //// base.OnPreviewMouseLeftButtonDown(e);
                        IDiagramPage m_diagramPage = VisualTreeHelper.GetParent(this) as IDiagramPage;

                        //// update selection
                        if (m_diagramPage != null)
                        {
                            if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) != ModifierKeys.None)
                            {
                                if (this.IsSelected)
                                {
                                    m_diagramPage.SelectionList.Remove(this);

                                    foreach (Group group in this.Groups)
                                    {
                                        foreach (ICommon Innode in group.NodeChildren)
                                        {
                                            if (this != Innode)
                                            {
                                                if (Innode.IsSelected)
                                                {
                                                    selected = false;

                                                }
                                            }
                                        }
                                    }
                                    foreach (Group groups in this.Groups)
                                    {
                                        if (selected)
                                        {
                                            if (!groups.IsSelected)
                                            {

                                            }
                                            else
                                            {
                                                groups.IsSelected = false;
                                                this.IsSelected = false;
                                            }
                                        }

                                    }
                                    //if (this.IsGrouped)
                                    //{
                                    //    foreach (Group gnode in this.Groups)
                                    //    {
                                    //        if ((gnode as Group).IsSelected)
                                    //        {
                                    //            gnode.IsSelected = false;
                                    //        }
                                    //        else
                                    //        {
                                    //            gnode.IsSelected = true;
                                    //        }

                                    //    }
                                    //}
                                }
                                else
                                {
                                    if (!this.IsSelected)
                                    {

                                        foreach (Group group in this.Groups)
                                        {
                                            foreach (ICommon Innode in group.NodeChildren)
                                            {
                                                if (this != Innode)
                                                {
                                                    if (Innode.IsSelected)
                                                    {
                                                        selected = false;

                                                    }
                                                }
                                            }


                                        }
                                        bool checking = true;
                                        //bool groupchecking = true;
                                        for (int i = 0; i < this.Groups.Count; i++)
                                        {

                                            if (selected)
                                            {

                                                //if (!(this.Groups[i] as Group).IsSelected)
                                                //{
                                                //   (this.Groups[i] as Group).IsSelected = true;
                                                //}
                                                //else
                                                //{
                                                //  (this.Groups[i] as Group).IsSelected = false;
                                                //    this.IsSelected = true;
                                                //}

                                                //else
                                                //{
                                                if (checking)
                                                {
                                                    if ((this.Groups[i] as Group).IsSelected)
                                                    {
                                                        (this.Groups[i] as Group).IsSelected = false;
                                                        if (i == this.Groups.Count - 1)
                                                        {
                                                            this.IsSelected = true;
                                                            checking = false;
                                                            break;
                                                        }


                                                        checking = false;
                                                    }

                                                    if (i == this.Groups.Count - 1)
                                                    {
                                                        (this.Groups[0] as Group).IsSelected = true;
                                                        checking = false;
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    (this.Groups[i] as Group).IsSelected = true;
                                                    break;
                                                }



                                                //if (checking)
                                                //{
                                                //    if ((this.Groups[this.Groups.Count - 1] as Group).IsSelected)
                                                //    {
                                                //        (this.Groups[this.Groups.Count - 1] as Group).IsSelected = false;
                                                //        this.IsSelected = true;
                                                //        break;
                                                //    }

                                                //    if (!(this.Groups[i] as Group).IsSelected)
                                                //    {
                                                //        (this.Groups[i] as Group).IsSelected = true;
                                                //        checking = false;
                                                //    }
                                                //    else
                                                //    {
                                                //        if (i < this.Groups.Count - 1)
                                                //        {
                                                //            (this.Groups[i] as Group).IsSelected = false;
                                                //            (this.Groups[i + 1] as Group).IsSelected = true;
                                                //            checking = false;
                                                //            groupchecking = false;
                                                //        }
                                                //        else
                                                //        {
                                                //            (this.Groups[i] as Group).IsSelected = false;
                                                //            this.IsSelected = true;

                                                //        }
                                                //    }

                                                //}
                                                //else if (groupchecking)
                                                //{
                                                //    (this.Groups[i] as Group).IsSelected = false;
                                                //    groupchecking = true;
                                                //}
                                            }
                                            else
                                            {
                                                this.IsSelected = true;
                                                for (int j = 0; j < this.Groups.Count; j++)
                                                {
                                                    (this.Groups[j] as Group).IsSelected = false;
                                                }
                                                break;
                                            }
                                            //}
                                        }

                                        //if (m_diagramPage.SelectionList.Count <2)
                                        //{
                                        //    if (this.AllowSelect && notselected)
                                        //    {
                                        //        if (!this.IsGrouped)
                                        //        {
                                        //            m_diagramPage.SelectionList.Add(this);
                                        //        }
                                        //        else
                                        //        {
                                        //            m_diagramPage.SelectionList.Add(this.Groups[this.Groups.Count - 1]);
                                        //        }
                                        //    }
                                        //}
                                        //else
                                        //{
                                        //    if (!selectfirst)
                                        //    {
                                        //        m_diagramPage.SelectionList.Select(m_diagramPage.SelectionList[0]);
                                        //        selectfirst = true;
                                        //    }
                                        //    else
                                        //    {
                                        //        selectfirst = false;
                                        //        m_diagramPage.SelectionList.Select(m_diagramPage.SelectionList[0]);
                                        //    }
                                        //}
                                    }
                                    //else if (this.AllowSelect && this.IsGrouped)
                                    //{
                                    //    m_diagramPage.SelectionList.Add(this.Groups[this.Groups.Count - 1]);
                                    //}
                                }
                            }
                            else
                            {
                                if (!this.IsSelected)
                                {
                                    foreach (Node gnode in this.Groups)
                                    {
                                        if (!gnode.IsSelected)
                                        {
                                            notselected = true;
                                        }
                                        else
                                        {
                                            notselected = false;
                                            break;
                                        }
                                    }

                                    if (this.AllowSelect && notselected)
                                    {
                                        if (!this.IsGrouped)
                                        {
                                            m_diagramPage.SelectionList.Select(this);
                                        }
                                        else
                                        {
                                            if ((this.Groups[this.Groups.Count - 1] as Group).AllowSelect)
                                            {
                                                m_diagramPage.SelectionList.Select(this.Groups[this.Groups.Count - 1]);
                                            }
                                        }
                                    }
                                    else if (this.AllowSelect && this.IsGrouped)
                                    {
                                        CollectionExt groupednodes = new CollectionExt();
                                        foreach (Group g in this.Groups)
                                        {
                                            groupednodes.Add(g);
                                        }

                                        groupednodes.Insert(0, this);
                                        foreach (Node gnode in groupednodes)
                                        {
                                            if (gnode.IsSelected)
                                            {
                                                if (groupednodes.Count > 1)
                                                {
                                                    int index = groupednodes.IndexOf(gnode);
                                                    if (index == 0)
                                                    {
                                                        m_diagramPage.SelectionList.Select(groupednodes[groupednodes.Count - 1]);
                                                    }
                                                    else
                                                    {
                                                        m_diagramPage.SelectionList.Select(groupednodes[index - 1]);
                                                    }

                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (this.AllowSelect && this.IsGrouped)
                                {
                                    m_diagramPage.SelectionList.Select(this.Groups[this.Groups.Count - 1]);
                                }
                            }

                            this.Focus();
                        }
                    }
                }
            }

            dview.Ispositionchanged = false;
            isdoubleclicked = false;

            this.ResizeThisNode = false;
            if (!ex)
            {
                if (DragProvider.Isdragging)
                {
                   // RaiseNodeDragEndEvent();
                    DiagramView.IsOtherEvent = true;
                }

                if (!DiagramView.IsOtherEvent)
                {
                    RaiseClickEvent();
                }
            }

            DiagramView.IsOtherEvent = false;
            ex = true;
            Node.mouseup = true;
            DragProvider.Isdragging = false;
        }
        private static void OnAllowRotatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Node node = d as Node;
            if ((node.GetTemplateChild("PART_Rotator") as Rotator) != null)
            {
                if (node.IsSelected && node.AllowRotate)
                {
                    (node.GetTemplateChild("PART_Rotator") as Rotator).Visibility = Visibility.Visible;
                }
                else
                {
                    (node.GetTemplateChild("PART_Rotator") as Rotator).Visibility = Visibility.Collapsed;
                }
            }
        }


        /// <summary>
        /// Called when [units changed].
        /// </summary>
        /// <param name="d">The DependencyObject.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnUnitsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Node n = d as Node;
            if (n._FirstLoaded)
            {
                n.OffsetX = MeasureUnitsConverter.Convert(n.OffsetX, (MeasureUnits)e.OldValue, (MeasureUnits)e.NewValue);
                n.OffsetY = MeasureUnitsConverter.Convert(n.OffsetY, (MeasureUnits)e.OldValue, (MeasureUnits)e.NewValue);
                n.Position = MeasureUnitsConverter.Convert(n.Position, (MeasureUnits)e.OldValue, (MeasureUnits)e.NewValue);
            }
        }

        private static void OnAllowDeletePropertyChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            (s as Node).Focus();
        }

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Node n = d as Node;
               n.isfired = true;
              n.NodeSelection();
           // if (n.IsSelected && n.Page != null && (n.Page as DiagramPage).SelectionList != null)
           // {
             //   (n.Page as DiagramPage).SelectionList.Add(n);
           // }
           // else if (!n.IsSelected && n.Page != null && (n.Page as DiagramPage).SelectionList != null && !(n.Page as DiagramPage).SelectionList.m_collClear)
           // /{
           //     (n.Page as DiagramPage).SelectionList.Remove(n);
          //  }
           // if ((n.GetTemplateChild("PART_Rotator") as Rotator) != null)
           // {
           //     if (n.AllowRotate && n.IsSelected)
           //     {
             //       (n.GetTemplateChild("PART_Rotator") as Rotator).Visibility = Visibility.Visible;
             //   }
             //   else
             //   {
              //      (n.GetTemplateChild("PART_Rotator") as Rotator).Visibility = Visibility.Collapsed;
             //   }
           // }
        }

        /// <summary>
        /// Called when rotate angle is changed.
        /// </summary>
        /// <param name="d">The dependencyObject.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnRotateAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramControl dc = DiagramPage.GetDiagramControl(d as Node);
            if (dc != null && !dc.View.Undone && !dc.View.IsLayout)
            {
                if (!(dc.View.undo || dc.View.redo))
                {
                    dc.View.RedoStack.Clear();
                }
                if (dc.View.UndoRedoEnabled && ! (d as Node).m_IsResizing)
                {
                    dc.View.UndoStack.Push(d as Node);
                    dc.View.UndoStack.Push(e.OldValue);
                    dc.View.UndoStack.Push(dc.View.NodeRotateCount);
                    dc.View.UndoStack.Push("Rotated");
                }
            }
        }

        /// <summary>
        /// Called when [logical offset X changed].
        /// </summary>
        /// <param name="d">The DependencyObject.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnLogicalOffsetXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            DiagramControl dc = DiagramPage.GetDiagramControl(d as Node);
            if (dc != null && !(d is Layer))
            {
                if (!(dc.View.undo || dc.View.redo || /*(dc.View.Page as DiagramPage).IsUnitChanged || */dc.View.IsMeasureCalled))
                {
                    dc.View.RedoStack.Clear();
                }
                if (/*!(dc.View.Page as DiagramPage).IsUnitChanged && */!dc.View.IsResizedRedone)
                {
                    (d as Node).oldx = (double)e.OldValue;// MeasureUnitsConverter.ToPixels((double)e.OldValue, (dc.View.Page as DiagramPage).MeasurementUnits);
                }

                if (!dc.View.Redone && !dc.View.IsDragged && !dc.View.Undone && !dc.View.IsResized && !(d is Group) && !dc.View.IsMeasureCalled && !dc.View.IsLayout && !dc.View.IsMeasureCalled && !dc.View.IsLayout)// && !(dc.View.Page as DiagramPage).IsUnitChanged)
                {
                    if (dc.View.UndoRedoEnabled)
                    {
                        dc.View.UndoStack.Push((double)e.OldValue);//MeasureUnitsConverter.ToPixels((double)e.OldValue, (dc.View.Page as DiagramPage).MeasurementUnits));
                        dc.View.UndoStack.Push("offsetx");
                        dc.View.UndoStack.Push(d as Node);
                        dc.View.UndoStack.Push(dc.View.NodeDragCount);
                        dc.View.UndoStack.Push("Dragged");
                    }
                }
            }
        }

        /// <summary>
        /// Called when [logical offset Y changed].
        /// </summary>
        /// <param name="d">The DependencyObject .</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnLogicalOffsetYChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramControl dc = DiagramPage.GetDiagramControl(d as Node);
            if (dc != null && !(d is Layer))
            {
                if (!(dc.View.undo || dc.View.redo || /*(dc.View.Page as DiagramPage).IsUnitChanged || */dc.View.IsMeasureCalled))
                {
                    dc.View.RedoStack.Clear();
                }
                if (/*!(dc.View.Page as DiagramPage).IsUnitChanged &&*/ !dc.View.IsResizedRedone)
                {
                    (d as Node).oldy = (double)e.OldValue;// MeasureUnitsConverter.ToPixels((double)e.OldValue, (dc.View.Page as DiagramPage).MeasurementUnits);
                }

                if (!dc.View.Redone && !dc.View.IsDragged && !dc.View.Undone && !dc.View.IsResized && !(d is Group) && !dc.View.IsMeasureCalled && !dc.View.IsLayout )// && !(dc.View.Page as DiagramPage).IsUnitChanged)
                {
                    if (dc.View.UndoRedoEnabled)
                    {
                        dc.View.UndoStack.Push((double)e.OldValue);//(MeasureUnitsConverter.ToPixels((double)e.OldValue, (dc.View.Page as DiagramPage).MeasurementUnits)));
                        dc.View.UndoStack.Push("offsety");
                        dc.View.UndoStack.Push(d as Node);
                        dc.View.UndoStack.Push(dc.View.NodeDragCount);
                        dc.View.UndoStack.Push("Dragged");
                    }
                }
            }
        }

        /// <summary>
        /// Called when [label changed].
        /// </summary>
        /// <param name="d">The DependencyObject.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnLabelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            string oldvalue = (string)e.OldValue;
            string newvalue = (string)e.NewValue;
            Node node = d as Node;
            LabelRoutedEventArgs newEventArgs = new LabelRoutedEventArgs((string)e.OldValue, (string)e.NewValue, node);
            newEventArgs.RoutedEvent = DiagramView.NodeLabelChangedEvent;
            node.RaiseEvent(newEventArgs);
#if SyncfusionFramework3_5
            if (node.editor != null)
            {
                BindingExpression expression = node.editor.GetBindingExpression(LabelEditor.LabelProperty);
                if (expression != null)
                {
                    expression.UpdateTarget();
                }
            }
#endif
        }

        /// <summary>
        /// Calls Node_LayoutUpdated method of the instance, notifies of the sender value changes.
        /// </summary>
        /// <param name="sender">object, the change occurs on.</param>
        /// <param name="e">Property change details, such as old value and new value.</param>
        public void Node_LayoutUpdated(object sender, EventArgs e)
        {
            if (this.Page != null && this.Page.Children.Contains(this))
            {
                this.PxPosition = this.TransformToAncestor(this.Page as Panel).Transform(new Point(this.Width / 2, this.Height / 2));               
            }

            //if (this.LabelWidth == 0 && editor != null)
            //{
            //    this.LabelWidth = editor.TextWidth;
            //    isdefaulted = true;
            //}

            //if (isdefaulted)
            //{
            //    this.LabelWidth = this.Width;
            //}

            if (IsSelected && AllowRotate)
            {
                if (!(this is Group) && rotatethumb != null)
                {
                    //rotatethumb.Visibility = Visibility.Visible;
                }
                else if (grouprotatethumb != null)
                {
                    grouprotatethumb.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (!(this is Group))
                {
                    if (rotatethumb != null)
                    {
                        //rotatethumb.Visibility = Visibility.Collapsed;
                    }
                }
                else if (grouprotatethumb != null)
                {
                    if (grouprotatethumb != null)
                    {
                        grouprotatethumb.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the diagram view.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The DiagramView instance.</returns>
        internal static DiagramView GetDiagramView(DependencyObject element)
        {
            while (element != null && !(element is DiagramView))
            {
                element = VisualTreeHelper.GetParent(element);
            }

            return element as DiagramView;
        }

        /// <summary>
        /// Gets the panel.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The panel instance</returns>
        private IDiagramPage GetPanel(DependencyObject element)
        {
            while (element != null && !(element is IDiagramPage))
            {
                element = VisualTreeHelper.GetParent(element);
            }

            return element as IDiagramPage;
        }

        internal void refreshBoundaries()
        {
            Shape temp = new Path();
            bool isContent = false;
            //IntersectionMode = IntersectionMode.OnBorder;
            if (IntersectionMode == IntersectionMode.OnBorder)
            {
                SetOnBorder(ref temp);
                //isContent = true;
            }
            else if (IntersectionMode == IntersectionMode.OnContent)
            {
                if (Content != null)
                {
                    if (Content is Shape)
                    {
                        temp = (Content as Shape);
                    }
                    else
                    {
                        temp = null;
                    }
                }
                if (temp == null && Template != null)
                {
                    try
                    {
                        temp = Template.FindName("PART_Shape", this) as Shape;
                    }
                    catch (Exception e)
                    {
                        temp = null;
                        e.ToString();
                    }
                }
                if (temp == null || temp.RenderedGeometry.Bounds == Rect.Empty)
                {
                    isContent = SetOnBorder(ref temp);
                }
            }
            if (Boundaries != null && temp != null && !isContent)
            {
                Transform t;
                Path path2 = new Path();
                t = new TranslateTransform(-temp.RenderedGeometry.Bounds.Left, -temp.RenderedGeometry.Bounds.Top);
                if (path2.RenderedGeometry != null)
                {
                    PathGeometry pg = Geometry.Combine(temp.RenderedGeometry, temp.RenderedGeometry, GeometryCombineMode.Intersect, t);
                    path2 = new Path();
                    path2.Data = pg;
                }
                if (path2.Data != null && (temp as Path) != null && (temp as Path).Data != null && (temp as Path).Data.Bounds != Rect.Empty)
                {
                    t = new TranslateTransform(-(temp as Path).Data.Bounds.Left, -(temp as Path).Data.Bounds.Top);
                    PathGeometry pg = Geometry.Combine((temp as Path).Data, (temp as Path).Data, GeometryCombineMode.Intersect, t);
                    path2 = new Path();
                    path2.Data = pg;
                }
                t = new ScaleTransform((Width - Margin.Left - Margin.Right - temp.Margin.Left - temp.Margin.Right) / path2.Data.Bounds.Size.Width,
                                    (Height - Margin.Top - Margin.Bottom - temp.Margin.Top - temp.Margin.Bottom) / path2.Data.Bounds.Size.Height);

                if (path2.Data != null)
                {
                    PathGeometry pg = Geometry.Combine(path2.Data, path2.Data, GeometryCombineMode.Intersect, t);
                    path2 = new Path();
                    path2.Data = pg;
                }

                t = new RotateTransform(RotateAngle,
                                        path2.Data.Bounds.Size.Width / 2,
                                        path2.Data.Bounds.Size.Height / 2);
                if (path2.Data != null)
                {
                    PathGeometry pg = Geometry.Combine(path2.Data, path2.Data, GeometryCombineMode.Intersect, t);
                    path2 = new Path();
                    path2.Data = pg;
                }
                if (dview != null && dview.Page != null)
                {
                    Point ofst = new Point(PxOffsetX, PxOffsetY);// MeasureUnitsConverter.ToPixels(new Point(LogicalOffsetX, LogicalOffsetY), (dview.Page as DiagramPage).MeasurementUnits);
                    t = new TranslateTransform(ofst.X + Margin.Left + temp.Margin.Left, ofst.Y + Margin.Top + temp.Margin.Top);
                }
                else
                {
                    t = new TranslateTransform(PxOffsetX + Margin.Left + temp.Margin.Left, PxOffsetY + Margin.Top + temp.Margin.Top);
                }
                if (path2.Data != null)
                {
                    PathGeometry pg = Geometry.Combine(path2.Data, path2.Data, GeometryCombineMode.Intersect, t);
                    path2 = new Path();
                    path2.Data = pg;
                }

                t = new ScaleTransform((path2.Data.Bounds.Width) / path2.Data.Bounds.Width,
                                    (path2.Data.Bounds.Height) / path2.Data.Bounds.Height, path2.Data.Bounds.Left + path2.Data.Bounds.Width / 2, path2.Data.Bounds.Top + path2.Data.Bounds.Height / 2);
                if (path2.Data != null)
                {
                    PathGeometry pg = Geometry.Combine(path2.Data, path2.Data, GeometryCombineMode.Intersect, t);
                    path2 = new Path();
                    path2.Data = pg;
                }
                Boundaries = path2;
            }
        }

        private bool SetOnBorder(ref Shape temp)
        {
            Rectangle r = new Rectangle();
            FrameworkElement element;
            if (this.Content != null && this.Content is FrameworkElement)
            {
                element = this.Content as FrameworkElement;
                if (element.RenderSize.Width == 0 && element.RenderSize.Height == 0)
                {
                    element = this as FrameworkElement;
                }
            }
            else
            {
                element = this as FrameworkElement;
            }

            if (element != null)
            {
                if (element.RenderSize.Width > 0.0)
                {
                    r.Width = element.RenderSize.Width - element.Margin.Left - element.Margin.Right;
                    r.Height = element.RenderSize.Height - element.Margin.Bottom - element.Margin.Top;
                }
                Point pos = new Point(Canvas.GetLeft(element), Canvas.GetTop(element));
                if (dview == null)
                {
                    dview = GetDiagramView(this);
                }
                if (dview != null)
                {
                    pos = element.TranslatePoint(new Point(0, 0), dview.Page);
                }
                temp = new Path();
                Geometry g = new RectangleGeometry(new Rect(new Size(r.Width, r.Height)));
                if (RotateAngle == 0)
                {
                    (temp as Path).Data = Geometry.Combine(g, g, GeometryCombineMode.Intersect, new TranslateTransform(pos.X, pos.Y) as Transform);
                    Boundaries = temp;
                    return true;
                }
                else
                {
                    g = new RectangleGeometry(new Rect(new Size(this.ActualWidth, ActualHeight)));
                    (temp as Path).Data = Geometry.Combine(g, g, GeometryCombineMode.Intersect, new TranslateTransform(0, 0) as Transform);
                }
                Boundaries = temp;
            }
            return false;
        }

        #endregion

        #region INodeGroup

        /// <summary>
        /// Gets or sets a value indicating whether this instance is grouped.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is grouped; otherwise, <c>false</c>.
        /// </value>
        public bool IsGrouped
        {
            get { return (bool)GetValue(IsGroupedProperty); }
            set { SetValue(IsGroupedProperty, value); }
        }

        #endregion

        #region ICommon Members

        /// <summary>
        /// Gets or sets a value indicating whether this instance is selected.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is selected; otherwise, <c>false</c>.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        /// <summary>
        /// Gets or sets the old ZIndex value.
        /// </summary>
        /// <value>The old ZIndex value</value>
        public int OldZIndex
        {
            get { return m_oldindex; }
            set { m_oldindex = value; }
        }

        internal double _Width
        {
            get 
            {
                if (this.IsLoaded)
                {
                    return this.ActualWidth;
                }
                else
                {
                    if (double.IsNaN(this.Width))
                    {
                        return 0;
                    }
                    else
                    {
                        return this.Width;
                    }
                }
            }
        }

        internal double _Height
        {
            get
            {
                if (this.IsLoaded)
                {
                    return this.ActualHeight;
                }
                else
                {
                    if (double.IsNaN(this.Height))
                    {
                        return 0;
                    }
                    else
                    {
                        return this.Height;
                    }
                }
            }
        }
        /// <summary>
        /// Gets or sets the new ZIndex value.
        /// </summary>
        /// <value>The new ZIndex value</value>
        public int NewZIndex
        {
            get { return m_newindex; }
            set { m_newindex = value; }
        }

        #endregion

        #region IShape Members

        /// <summary>
        /// Gets or sets the Rank to which the node belongs to.
        /// </summary>
        internal int Rank
        {
            get { return m_rank; }
            set { m_rank = value; }
        }
        internal int countpno = 0;

        /// <summary>
        /// Gets or sets the parent nodes  based on the connections in a hierarchical layout.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CollectionExt Parents
        {
            get
            {
                return mParents;
            }

            set
            {
                mParents = value;
            }
        }

        /// <summary>
        /// Gets or sets the child nodes  based on the connections in a hierarchical layout.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CollectionExt HChildren
        {
            get
            {
                return mChild;
            }

            set
            {
                mChild = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is fixed.
        /// </summary>
        /// <value><c>true</c> if this instance is fixed; otherwise, <c>false</c>.</value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsFixed
        {
            get { return mIsFixed; }
            set { mIsFixed = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is expanded.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is expanded; otherwise, <c>false</c>.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsExpanded
        {
            get { return mIsExpanded; }
            set { mIsExpanded = value; }
        }

        /// <summary>
        /// Gets the in-degree of the node, the number of edges for which this node
        /// is the target.
        /// </summary>
        /// <value>The in degree</value>
        public int InDegree
        {
            get { return this.InEdges.Count; }
        }

        /// <summary>
        /// Gets the out-degree of the node, the number of edges for which this node
        /// is the source.
        /// </summary>
        /// <value>The out degree.</value>
        public int OutDegree
        {
            get { return this.OutEdges.Count; }
        }

        /// <summary>
        /// Gets the degree of the node, the number of edges for which this node
        /// is either the source or the target.
        /// </summary>
        /// <value>The degree.</value>
        public int Degree
        {
            get { return this.Edges.Count; }
        }

        /// <summary>
        /// Gets the collection of all incoming edges, those for which this node
        /// is the target.
        /// </summary>
        /// <value>The In Edges .</value>
        public CollectionExt InEdges
        {
            get { return m_inEdges; }
        }

        /// <summary>
        /// Gets the collection of all outgoing edges, those for which this node
        /// is the source.
        /// </summary>
        /// <value>The OutEdges</value>
        public CollectionExt OutEdges
        {
            get { return m_outEdges; }
        }

        /// <summary>
        /// Gets the collection of all incident edges, those for which this node
        /// is either the source or the target.
        /// </summary>
        /// <value>The Collection of Edges</value>
        public CollectionExt Edges
        {
            get
            {
                return m_edges;
            }
        }

        /// <summary>
        /// Gets the collection of all adjacent nodes connected to this node by an
        /// incoming edge (i.e., all nodes that "point" at this one).
        /// </summary>
        /// <value>Collection of inedges.</value>
        public CollectionExt InNeighbors
        {
            get
            {
                CollectionExt edges = (this as IShape).InEdges;
                CollectionExt ret = new CollectionExt();
                foreach (IEdge edge in edges)
                {
                    ret.Add(edge.HeadNode);
                }

                return ret;
            }
        }

        /// <summary>
        /// Gets the collection of adjacent nodes connected to this node by an
        /// outgoing edge (i.e., all nodes "pointed" to by this one).
        /// </summary>
        /// <value>Collection of out edges</value>
        public CollectionExt OutNeighbors
        {
            get
            {
                CollectionExt edges = (this as IShape).OutEdges;
                CollectionExt ret = new CollectionExt();
                foreach (IEdge edge in edges)
                {
                    ret.Add(edge.TailNode);
                }

                return ret;
            }
        }

        /// <summary>
        /// Gets an iterator over all nodes connected to this node.
        /// </summary>
        /// <value></value>
        public CollectionExt Neighbors
        {
            get
            {
                CollectionExt edges = (this as IShape).Edges;
                CollectionExt ret = new CollectionExt();
                foreach (IEdge edge in edges)
                {
                    if (edge.HeadNode == this)
                    {
                        ret.Add(edge.TailNode);
                    }
                    else
                    {
                        ret.Add(edge.HeadNode);
                    }
                }

                return ret;
            }
        }

        /// <summary>
        /// Gets or sets the parent of the entity
        /// </summary>
        /// <value></value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IShape ParentNode
        {
            get
            {
                return mParentNode;
            }

            set
            {
                mParentNode = value;
            }
        }

        /// <summary>
        /// Gets or sets the edge between this node and its parent node in a tree
        /// structure.
        /// </summary>
        /// <value>The Parent edge</value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEdge ParentEdge
        {
            get
            {
                return mParentEdge;
            }

            set
            {
                mParentEdge = value;
            }
        }

        /// <summary>
        /// Gets or sets the tree depth of this node.
        /// <remarks>The root's tree depth is
        /// zero, and each level of the tree is one depth level greater.
        /// </remarks>
        /// </summary>
        /// <value>The depth value</value>
        /// <remarks>The root's tree depth is
        /// zero, and each level of the tree is one depth level greater.
        /// </remarks>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Depth
        {
            get { return mDepth; }
            set { mDepth = value; }
        }

        /// <summary>
        /// Gets the number of tree children of this node.
        /// </summary>
        /// <value>The child count</value>
        public int ChildCount
        {
            get
            {
                if (this.treeChildren == null)
                {
                    return 0;
                }
                else
                {
                    return this.treeChildren.Count;
                }
            }
        }

        /// <summary>
        /// Gets this node's first tree child.
        /// </summary>
        /// <value>The first child</value>
        public IShape FirstChild
        {
            get
            {
                if (this.treeChildren == null || this.treeChildren.Count == 0)
                {
                    return null;
                }
                else
                {
                    return (IShape)treeChildren[0];
                }
            }
        }

        /// <summary>
        /// Gets this node's last tree child.
        /// </summary>
        /// <value></value>
        public IShape LastChild
        {
            get
            {
                if (this.treeChildren == null || this.treeChildren.Count == 0)
                {
                    return null;
                }
                else
                {
                    return (IShape)treeChildren[this.treeChildren.Count - 1];
                }
            }
        }

        /// <summary>
        /// Gets this node's previous tree sibling.
        /// </summary>
        /// <value></value>
        public IShape PreviousSibling
        {
            get
            {
                if ((this as IShape).ParentNode == null)
                {
                    return null;
                }
                else
                {
                    CollectionExt ch = (this as IShape).ParentNode.Children;
                    int chi = ch.IndexOf(this as IShape);
                    if (chi < 0 || chi > ch.Count - 1)
                    {
                        throw new IndexOutOfRangeException();
                    }

                    if (chi == 0)
                    {
                        return null;
                    }
                    else
                    {
                        return (IShape)ch[chi - 1];
                    }
                }
            }
        }

        /// <summary>
        /// Gets this node's next tree sibling.
        /// </summary>
        /// <value></value>
        public IShape NextSibling
        {
            get
            {
                if ((this as IShape).ParentNode == null)
                {
                    return null;
                }
                else
                {
                    CollectionExt ch = (this as IShape).ParentNode.Children;
                    int chi = ch.IndexOf(this as IShape);
                    if (chi < 0 || chi > ch.Count - 1)
                    {
                        throw new IndexOutOfRangeException();
                    }

                    if (chi == ch.Count - 1)
                    {
                        return null;
                    }
                    else
                    {
                        return (IShape)ch[chi + 1];
                    }
                }
            }
        }

        /// <summary>
        /// Gets the previous shape.
        /// </summary>
        /// <value>The previous shape.</value>
        public IShape PreviousShape
        {
            get
            {
                if (Model != null && Model.Nodes != null)
                {
                    int ind = Model.Nodes.IndexOf(this);

                    if (ind <= 0)
                    {
                        return null;
                    }
                    else
                    {
                        return (IShape)Model.Nodes[ind - 1];
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the row.
        /// </summary>
        /// <value>The row count.</value>
        public int Row
        {
            get { return row; }
            set { row = value; }
        }

        /// <summary>
        /// Gets or sets the column.
        /// </summary>
        /// <value>The column count.</value>
        public int Column
        {
            get { return col; }
            set { col = value; }
        }

        /// <summary>
        /// Gets or sets an iterator over this node's tree children.
        /// </summary>
        /// <value></value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CollectionExt Children
        {
            get { return treeChildren; }
            set { treeChildren = value; }
        }

        /// <summary>
        /// Gets the System.Drawing.Rectangle.
        /// </summary>
        public System.Drawing.Rectangle Rectangle
        {
            get { return mRectangle; }
        }

        /// <summary>
        /// Gets the collection of this node's tree children's edges.
        /// </summary>
        /// <value></value>
        public CollectionExt ChildEdges
        {
            get
            {
                if (this.model != null && this.Model is IGraph && (this.Model as IGraph).Tree != null)
                {
                    return (this.Model as IGraph).Tree.ChildEdges(this);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Moves the node, the argument being the motion vector.
        /// </summary>
        /// <param name="p">The point p.</param>
        public void Move(System.Drawing.Point p)
        {
        }

        /// <summary>
        /// Gets the unique identifier of this node.
        /// </summary>
        /// <value>The unique identifier value.</value>
        public Guid ID
        {
            get { return m_id; }
        }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>The model.</value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DiagramModel Model
        {
            get { return model; }
            set { model = value; }
        }

        /// <summary>
        /// Gets the collection of connectors.
        /// </summary>
        /// <value></value>
        public CollectionExt Connectors
        {
            get { return m_connectors; }
        }
        bool load=false;
        internal bool IsInternallyLoaded
        {
            get { return load; }
            set { load = value; }
        }

        /// <summary>
        /// Gets the Node ID
        /// </summary>
        internal Node NodeID
        {
            get { return this; }
        }

        /// <summary>
        /// Gets or sets the parent ID.
        /// </summary>
        /// <value>The parent ID.</value>
        public Guid ParentID
        {
            get { return (Guid)GetValue(ParentIDProperty); }
            set { SetValue(ParentIDProperty, value); }
        }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>The full name.</value>
        /// <remarks>
        /// The full name is the name of the node concatenated with the names
        /// of all parent nodes.
        /// </remarks>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public string FullName
        {
            get { return m_fullName; }
            set { m_fullName = value; }
        }

        /// <summary>
        /// Gets or sets the logical offset X. Used for internal calculation.
        /// </summary>
        /// <value>The logical offset X.</value>
        [Obsolete("Use OffsetX")]
        public double LogicalOffsetX
        {
            get
            {
                return (double)GetValue(LogicalOffsetXProperty);
            }

            set
            {
                SetValue(LogicalOffsetXProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the logical offset Y. Used for internal calculation.
        /// </summary>
        /// <value>The logical offset Y.</value>
        [Obsolete("Use OffsetY")]
        public double LogicalOffsetY
        {
            get
            {
                return (double)GetValue(LogicalOffsetYProperty);
            }

            set
            {
                SetValue(LogicalOffsetYProperty, value);
            }
        }
        
        /// <summary>
        /// Gets or sets the old offset position.
        /// </summary>
        /// <value>The old offset point.</value>
        internal Point OldOffset
        {
            get
            {
                return oldoff;
            }

            set
            {
                oldoff = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the offset X.
        /// </summary>
        /// <value>The offset X.</value>
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
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.IsLabelEditable = true;
        /// n.Label = "Start";
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// n.ToolTip="Start Node";
        /// node.AllowRotate=true;
        /// Model.Nodes.Add(n);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        public double OffsetX
        {
            get { return (double)GetValue(OffsetXProperty); }
            set { SetValue(OffsetXProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OffsetX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OffsetXProperty =
            DependencyProperty.Register("OffsetX", typeof(double), typeof(Node), new FrameworkPropertyMetadata(0d, new PropertyChangedCallback(OnLogicalOffsetXChanged)));


        /// <summary>
        /// Gets or sets the offset Y.
        /// </summary>
        /// <value>The offset Y.</value>
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
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.IsLabelEditable = true;
        /// n.Label = "Start";
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// n.ToolTip="Start Node";
        /// node.AllowRotate=true;
        /// Model.Nodes.Add(n);
        /// }
        /// }
        /// }
        /// </code>
        // </example>
        public double OffsetY
        {
            get { return (double)GetValue(OffsetYProperty); }
            set { SetValue(OffsetYProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OffsetY.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OffsetYProperty =
            DependencyProperty.Register("OffsetY", typeof(double), typeof(Node), new FrameworkPropertyMetadata(0d, new PropertyChangedCallback(OnLogicalOffsetYChanged)));


        #endregion

        internal void SnapWidth(bool IsLeft)
        {
            if (this.dc != null && this.dc.View != null && this.dc.View.SnapToVerticalGrid && this.m_MouseResizing)
            {
                if (IsLeft)
                {
                    //double del = MeasureUnitsConverter.ToPixels(this.LogicalOffsetX, this.MeasurementUnits);
                    //this.LogicalOffsetX = MeasureUnitsConverter.FromPixels(Node.Round(this.m_TempPosition.X, dc.View.PxSnapOffsetX), this.MeasurementUnits);
                    //del -= MeasureUnitsConverter.ToPixels(this.LogicalOffsetX, this.MeasurementUnits);
                    double del = this.PxOffsetX;
                    this.PxOffsetX = Node.Round(this.m_TempPosition.X, dc.View.PxSnapOffsetX);
                    del -= this.PxOffsetX;
                    this.Width += del;
                }
                else
                {
                    //double x = MeasureUnitsConverter.ToPixels(this.LogicalOffsetX, this.MeasurementUnits);
                    double x = this.PxOffsetX;
                    this.Width = Node.Round(x + this.m_TempSize.Width, dc.View.PxSnapOffsetX) - x;
                }
            }
            else
            {
                if (IsLeft)
                {
                    //this.LogicalOffsetX = MeasureUnitsConverter.FromPixels(this.m_TempPosition.X, this.MeasurementUnits);
                    this.PxOffsetX = this.m_TempPosition.X;
                }
                this.Width = this.m_TempSize.Width;
            }
        }

        internal void SnapHeight(bool IsTop)
        {
            if (this.dc != null && this.dc.View != null && this.dc.View.SnapToHorizontalGrid && this.m_MouseResizing)
            {
                if (IsTop)
                {
                    //double del = MeasureUnitsConverter.ToPixels(this.LogicalOffsetY, this.MeasurementUnits);
                    double del = this.PxOffsetY;
                    //this.LogicalOffsetY = MeasureUnitsConverter.FromPixels(Node.Round(this.m_TempPosition.Y, dc.View.PxSnapOffsetY), this.MeasurementUnits);
                    this.PxOffsetY = Node.Round(this.m_TempPosition.Y, dc.View.PxSnapOffsetY);
                    del -= this.PxOffsetY;// MeasureUnitsConverter.ToPixels(this.LogicalOffsetY, this.MeasurementUnits);
                    this.Height += del;
                    //this.PxLogicalOffsetY = Node.Round(this.m_TempPosition.Y, (dc.View.Page as DiagramPage).PxGridHorizontalOffset);
                    //del -= this.PxLogicalOffsetY;
                    //this.Height += del;
                }
                else
                {
                    double y = this.PxOffsetY;// MeasureUnitsConverter.ToPixels(this.LogicalOffsetY, this.MeasurementUnits);
                    this.Height = Node.Round(y + this.m_TempSize.Height, dc.View.PxSnapOffsetY) - y;
                }
            }
            else
            {
                if (IsTop)
                {
                    this.PxOffsetY = this.m_TempPosition.Y;// MeasureUnitsConverter.FromPixels(this.m_TempPosition.Y, this.MeasurementUnits);
                }
                //this.Height = Node.Round(this.m_TempSize.Height, (dc.View.Page as DiagramPage).PxGridHorizontalOffset);
                this.Height = this.m_TempSize.Height;
                //Debug.WriteLine(this.Height);
            }
        }

        //internal double PxOffsetY
        //{
        //    get { return (double)GetValue(PxOffsetYProperty); }
        //    set { SetValue(PxOffsetYProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for PxLogicalOffsetY.  This enables animation, styling, binding, etc...
        //internal static readonly DependencyProperty PxOffsetYProperty =
        //    DependencyProperty.Register("PxOffsetY", typeof(double), typeof(Node));

        //internal Point PxPosition
        //{
        //    get { return (Point)GetValue(PxPositionProperty); }
        //    set { SetValue(PxPositionProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for PxPosition.  This enables animation, styling, binding, etc...
        //internal static readonly DependencyProperty PxPositionProperty =
        //    DependencyProperty.Register("PxPosition", typeof(Point), typeof(Node));
        
        //internal double PxOffsetX
        //{
        //    get { return (double)GetValue(PxOffsetXProperty); }
        //    set { SetValue(PxOffsetXProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for PxLogicalOffsetX.  This enables animation, styling, binding, etc...
        //internal static readonly DependencyProperty PxOffsetXProperty =
        //    DependencyProperty.Register("PxOffsetX", typeof(double), typeof(Node));

        internal double PxOffsetX
        {
            get
            {
                return MeasureUnitsConverter.ToPixels(OffsetX, this.MeasurementUnits);
            }

            set
            {
                OffsetX = MeasureUnitsConverter.FromPixels(value, this.MeasurementUnits);
            }
        }

        internal double PxOffsetY
        {
            get
            {
                return MeasureUnitsConverter.ToPixels(OffsetY, this.MeasurementUnits);
            }

            set
            {
                OffsetY = MeasureUnitsConverter.FromPixels(value, this.MeasurementUnits);
            }
        }

        internal Point PxPosition
        {
            get
            {
                return MeasureUnitsConverter.ToPixels(Position, this.MeasurementUnits);
            }

            set
            {
                Position = MeasureUnitsConverter.FromPixels(value, this.MeasurementUnits);
            }
        }

        internal double PxWidth
        {
            get
            {
                return MeasureUnitsConverter.ToPixels(this.Width, this.MeasurementUnits);
            }
            set
            {
                Width = MeasureUnitsConverter.FromPixels(value, this.MeasurementUnits);
            }
        }

        internal double PxHeight
        {
            get
            {
                return MeasureUnitsConverter.ToPixels(this.Height, this.MeasurementUnits);
            }
            set
            {
                Height = MeasureUnitsConverter.FromPixels(value, this.MeasurementUnits);
            }
        }
    }

    #region struct
    /// <summary>
    /// Gives information about the node.
    /// </summary>
    public struct NodeInfo
    {
        /// <summary>
        /// Gets or sets the node left position.
        /// </summary>
        public double Left { get; set; }

        /// <summary>
        /// Gets or sets the node top position.
        /// </summary>
        public double Top { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        public Size Size { get; set; }

        /// <summary>
        /// Gets or sets the node centre position.
        /// </summary>
        public Point Position { get; set; }

        /// <summary>
        /// Gets or sets the units.
        /// </summary>
        public MeasureUnits MeasurementUnit { get; set; }

        internal Point TopPoint
        {
            get
            {
                return new Point(Left + Size.Width / 2, Top);
            }
        }

        internal Point LeftPoint
        {
            get
            {
                return new Point(Left, Top + Size.Height / 2);
            }
        }

        internal Point RightPoint
        {
            get
            {
                return new Point(Left + Size.Width, Top + Size.Height / 2);
            }
        }

        internal Point BottomPoint
        {
            get
            {
                return new Point(Left + Size.Width / 2, Top + Size.Height);
            }
        }
    }
    #endregion
}
