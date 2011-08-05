#region Copyright
// Copyright Syncfusion Inc. 2001 - 2011. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion

namespace Syncfusion.Windows.Diagram
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    //using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Interop;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Xml;
    using System.Windows.Shapes;

    /// <summary>
    /// Represents the diagram page .
    /// <para> The DiagramPage is just a container to hold the objects(nodes and connectors) added through model.
    /// The DiagramView uses the page to display the diagram objects.
    /// </para>
    /// </summary>
    public class DiagramPage : Panel, IDiagramPage
    {
        MultiScaleImage image = new MultiScaleImage();

        /// <summary>
        /// Identifies the AllowSelect dependency property.
        /// </summary>
        public static readonly DependencyProperty EnableResizingCurrentNodeOnMultipleSelectionProperty = DependencyProperty.Register("EnableResizingCurrentNodeOnMultipleSelection", typeof(bool), typeof(DiagramPage), new PropertyMetadata(false));

        /// <summary>
        /// Defines the LayoutType property.  This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty LayoutTypeProperty = DependencyProperty.Register("LayoutType", typeof(LayoutType), typeof(DiagramPage), new PropertyMetadata(LayoutType.None));

        /// <summary>
        /// Defines the MeasurementUnits property.  This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty MeasurementUnitsProperty = DependencyProperty.Register("MeasurementUnits", typeof(MeasureUnits), typeof(DiagramPage), new PropertyMetadata(MeasureUnits.Pixel, new PropertyChangedCallback(OnUnitsChanged)));

        internal static string Pathstring = string.Empty;

        /// <summary>
        /// Used to store the measure units.
        /// </summary>
        private static MeasureUnits munits;

        /// <summary>
        /// Used to store selection list
        /// </summary>
        private NodeCollection mselectionList;

        /// <summary>
        /// Used to store a static object.
        /// </summary>
        private static object o;

        /// <summary>
        /// Used to store the page.
        /// </summary>
        private DiagramPage page;

        /// <summary>
        /// Used to refer to the child count value.
        /// </summary>
        private bool childcount = true;

        /// <summary>
        ///  Used to store the minx.
        /// </summary>
        private double cminx = 0;

        /// <summary>
        ///  Used to store the miny
        /// </summary>
        private double cminy = 0;

        /// <summary>
        /// Used to store the constant minx
        /// </summary>
        private double cmx = 0;

        /// <summary>
        ///  Used to store the constant miny
        /// </summary>
        private double cmy = 0;

        /// <summary>
        ///  Used to store the  current minx
        /// </summary>
        private double curminx = 0;

        /// <summary>
        ///  Used to store the current miny
        /// </summary>
        private double curminy = 0;

        /// <summary>
        /// Used to store the diagram control.
        /// </summary>
        private DiagramControl dc;

        /// <summary>
        /// Used to refer to the diagram control instance.
        /// </summary>
        private DiagramControl diagctrl;

        /// <summary>
        /// Used to store the drag left value.
        /// </summary>
        private double dleft = 0;

        /// <summary>
        /// Used to store the drag top value.
        /// </summary>
        private double dtop = 0;

        /// <summary>
        /// Used to store the View instance.
        /// </summary>
        private DiagramView dview;

        private Point endPoint = new Point(0, 0);

        /// <summary>
        /// Used to refer to the execution instance.
        /// </summary>
        private bool exeonce = false;

        /// <summary>
        ///  Used to store the GreaterThanZero value.
        /// </summary>
        private bool gtz = false;

        /// <summary>
        ///  Used to store the GreaterThanZeroY bool value.
        /// </summary>
        private bool gtzy = false;

        /// <summary>
        ///  Used to store the horizontal offset.
        /// </summary>
        private double horizontaloffset = 0;

        /// <summary>
        /// Used to refer to the horizontal offset
        /// </summary>
        private double horoffset = 25d;

        /// <summary>
        /// Used to store the horizontal spacing reference.
        /// </summary>
        private double href;

        /// <summary>
        /// Used to store the horizontal offset on adding connectors.
        /// </summary>
        private double hval = 0;

        /// <summary>
        /// Used to store the int value.
        /// </summary>
        private int i = 0;

        /// <summary>
        /// Used to check if executed.
        /// </summary>
        private bool isexe = false;

        /// <summary>
        /// Used to refer to the not pixel offset.
        /// </summary>
        private bool isnotpixeloffset = false;

        /// <summary>
        /// Used to store the unit details 
        /// </summary>
        private bool isnotpixelwh = false;

        /// <summary>
        /// Used to store the changed unit.
        /// </summary>
        private bool isunitchanged = false;

        /// <summary>
        /// Used to store the least negative offsetx.
        /// </summary>
        private double leastx = 0;

        /// <summary>
        /// Used to store the least negative offsety.
        /// </summary>
        private double leasty = 0;

        /// <summary>
        /// Used to check if connector is dropped.
        /// </summary>
        private bool linedrop = false;

        /// <summary>
        /// Used to check if the page is loaded.
        /// </summary>
        private bool loaded = false;

        /// <summary>
        /// Used to store the line style reference value.
        /// </summary>
        private Style lstyleref;

        /// <summary>
        /// Used to store the layout type.
        /// </summary>
        private LayoutType ltref;

        /// <summary>
        /// Used to store the connector type.
        /// </summary>
        private ConnectorType mconnectionType = ConnectorType.Orthogonal;

        /// <summary>
        /// Used to refer to the unit changed event.
        /// </summary>
        private bool munitchanged = false;

        /// <summary>
        /// Used to store max value.
        /// </summary>
        private double max = 0;

        private bool mcall = false;

        /// <summary>
        /// Used to store the minimumx.
        /// </summary>
        private double minimumX = 0;

        /// <summary>
        /// Used to store the minimumy.
        /// </summary>
        private double minimumY = 0;

        /// <summary>
        /// Used to store the minleftx value.
        /// </summary>
        private double minleftX = 0;

        /// <summary>
        /// Used to store the this.mintopY value.
        /// </summary>
        private double mintopY = 0;

        /// <summary>
        /// Used to refer to the name count
        /// </summary>
        private int namecount = 0;

        /// <summary>
        /// Used to refer to the children count.
        /// </summary>
        /// <remarks></remarks>
        private int no;

        /// <summary>
        /// Used to refer to the not first exe value.
        /// </summary>
        private bool notfirstexe = false;

        /// <summary>
        /// Used to store the units
        /// </summary>
        private MeasureUnits ounit;

        /// <summary>
        /// Used to store boolean value on executing once.
        /// </summary>
        private bool once = false;

        /// <summary>
        /// Used to store boolean value on reaching zero once.
        /// </summary>
        private bool oncezero = false;

        /// <summary>
        /// Used to store the  tree orientation
        /// </summary>
        private TreeOrientation oref;

        /// <summary>
        /// Used to store the IsPositive value.
        /// </summary>
        private bool pos = false;

        /// <summary>
        ///  Used to store the IsPositiveY bool value.
        /// </summary>
        private bool posy = false;

        /// <summary>
        /// Used to store double value.
        /// </summary>
        private double s = 0;

        /// <summary>
        /// Used to store mouse scroll count
        /// </summary>
        private int scrollcount = 0;

        /// <summary>
        /// Used to store the space between the sub-trees.
        /// </summary>
        private double sref;

        /// <summary>
        /// Used to store the start point.
        /// </summary>
        private Point? startPoint = null;

        /// <summary>
        /// Used to store the resize bool value.
        /// </summary>
        private Style styleref;

        /// <summary>
        /// Used to store the horizontal scollbar width.
        /// </summary>
        private double sx = 0;

        /// <summary>
        /// Used to store the vertical scollbar width.
        /// </summary>
        private double sy = 0;

        private ResourceDictionary symbols = new ResourceDictionary();

        /// <summary>
        /// Used to store the units temporarily.
        /// </summary>
        private MeasureUnits temp;

        /// <summary>
        /// Used to check if the node transformation is done.
        /// </summary>
        private bool transformed = false;

        /// <summary>
        /// Checks if the unit got changed.
        /// </summary>
        private bool unitchan = false;

        /// <summary>
        ///  Used to store the vertical offset.
        /// </summary>
        private double verticaloffset = 0;

        /// <summary>
        /// Used to refer to the vertical offset. Default value is 25d.
        /// </summary>
        private double vertoffset = 25d;

        /// <summary>
        /// Used to store the vertical spacing reference.
        /// </summary>
        private double vref;

        /// <summary>
        /// Used to store the vertical offset on adding connectors.
        /// </summary>
        private double vval = 0;


        /// <summary>
        /// Used to Start Point of the Page.
        /// </summary>
        private Point spoint;

        internal enum GridOffsets
        {
            Horizontal,
            Vertical
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DiagramPage"/> class.
        /// </summary>
        public DiagramPage()
        {
            this.Loaded += new RoutedEventHandler(this.DiagramPage_Loaded);
            if (!this.CaptureMouse())
            {
                this.Background = new SolidColorBrush(Colors.Transparent);
            }

            this.Children.Add(new DiagramViewGrid());

            this.MouseRightButtonDown += new MouseButtonEventHandler(DiagramPage_MouseRightButtonDown);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(this.DiagramPage_MouseLeftButtonDown);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(this.DiagramPage_MouseLeftButtonUp);
            this.MouseMove += new MouseEventHandler(this.DiagramPage_MouseMove);
            this.KeyDown += new KeyEventHandler(this.DiagramPage_KeyDown);
            this.SizeChanged += new SizeChangedEventHandler(DiagramPage_SizeChanged);
            this.MouseWheel += new MouseWheelEventHandler(DiagramPage_MouseWheel);
        }


        void DiagramPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.Children.Count > 0 && this.Children[0] is DiagramViewGrid)
            {
                DiagramView view = (this.Children[0] as DiagramViewGrid).m_View;
                if (view != null)
                {
                    (this.Children[0] as DiagramViewGrid).Width = Math.Max(view.Scrollviewer.ActualWidth, view.Scrollviewer.ExtentWidth);
                    (this.Children[0] as DiagramViewGrid).Height = Math.Max(view.Scrollviewer.ActualHeight, view.Scrollviewer.ExtentHeight);
                    (this.Children[0] as DiagramViewGrid).UpdateLayout();
                }
            }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the Actual Height of the DiagramPage.
        /// </summary>
        /// <value>
        /// Type: <see cref="double"/>
        /// Actual Height of the DiagramPage in pixels.
        /// </value>
        public new double ActualHeight
        {
            get
            {
                return base.ActualHeight;
            }
        }

        /// <summary>
        /// Gets the Actual Width of the DiagramPage.
        /// </summary>
        /// <value>
        /// Type: <see cref="double"/>
        /// Actual Width of the DiagramPage in pixels.
        /// </value>
        public new double ActualWidth
        {
            get
            {
                return base.ActualWidth;
            }
        }

        /// <summary>
        /// Gets or sets the the type of connection to be used.
        /// </summary>
        /// <value>
        /// Type: <see cref="ConnectorType"/>
        /// Enum specifying the type of the connector to be used.
        /// </value>
        /// <example>
        /// <para/>This example shows how to set ConnectorType in C#.
        /// <code language="C#">
        /// connObject.ConnectorType = ConnectorType.Orthogonal;
        /// </code>
        /// </example>
        public ConnectorType ConnectorType
        {
            get
            {
                return this.mconnectionType;
            }

            set
            {
                if (value != this.mconnectionType)
                {
                    this.mconnectionType = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable resizing current node on multiple selection].
        /// </summary>
        /// <value>
        /// <c>true</c> if [enable resizing current node on multiple selection]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableResizingCurrentNodeOnMultipleSelection
        {
            get
            {
                return (bool)GetValue(EnableResizingCurrentNodeOnMultipleSelectionProperty);
            }

            set
            {
                SetValue(EnableResizingCurrentNodeOnMultipleSelectionProperty, value);
            }
        }

        internal double PxGridHorizontalOffset
        {
            get
            {
                return MeasureUnitsConverter.ToPixels(GridHorizontalOffset, this.MeasurementUnits);
            }
            set
            {
                GridHorizontalOffset = MeasureUnitsConverter.FromPixels(value, this.MeasurementUnits);
            }
        }

        internal double PxGridVerticalOffset
        {
            get
            {
                return MeasureUnitsConverter.ToPixels(GridVerticalOffset, this.MeasurementUnits);
            }
            set
            {
                GridVerticalOffset = MeasureUnitsConverter.FromPixels(value, this.MeasurementUnits);
            }
        }

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
        ///       (View.Page as DiagramPage).GridHorizontalOffset=100;
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        public double GridHorizontalOffset
        {
            get
            {
                return this.horoffset;
            }
            set
            {
                setGridOffsets(value, GridOffsets.Horizontal);
                updateGridLines(GridOffsets.Horizontal);
            }
        }

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
        ///       (View.Page as DiagramPage).GridVerticalOffset=100;
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        public double GridVerticalOffset
        {
            get
            {
                return this.vertoffset;
            }

            set
            {
                setGridOffsets(value, GridOffsets.Vertical);
                updateGridLines(GridOffsets.Vertical);
            }
        }

        /// <summary>
        /// Gets or sets the Horizontal spacing reference.Used for Serialization purpose.
        /// </summary>
        /// <value>The horizontal spacing reference.</value>
        public double HorizontalSpacingref
        {
            get { return this.href; }
            set { this.href = value; }
        }

        /// <summary>
        /// Gets or sets the Measurement unit property.
        /// </summary>
        /// <value>
        /// Type: <see cref="MeasureUnits"/>
        /// Enum specifying the unit to be used.
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
        ///       (View.Page as DiagramPage).MeasurementUnits = MeasureUnits.Inch;
        ///       Node n = new Node(Guid.NewGuid(), "Start");
        ///        n.Shape = Shapes.FlowChart_Start;
        ///        n.IsLabelEditable = true;
        ///        n.Label = "Start";
        ///        n.OffsetX = 1.5;
        ///        n.OffsetY = 2.5;
        ///        n.Width = 1.5;
        ///        n.Height = 0.75;
        ///        n.ToolTip="Start Node";
        ///        Model.Nodes.Add(n);
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
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

        /// <summary>
        ///  Gets or sets the SpaceBetweenSubTreeSpacing reference .Used for Serialization purpose.
        /// </summary>
        /// <value>The sub tree spacing reference.</value>
        public LayoutType LayoutTyperef
        {
            get { return this.ltref; }
            set { this.ltref = value; }
        }

        /// <summary>
        /// Gets or sets the LineStyleRef reference. Used for Serialization purpose.
        /// </summary>
        /// <value>The line style ref.</value>
        public Style LineStyleRef
        {
            get { return this.lstyleref; }
            set { this.lstyleref = value; }
        }

        /// <summary>
        /// Gets or sets the orientation reference. Used for Serialization purpose.
        /// </summary>
        /// <value>The orientation ref.</value>
        public TreeOrientation OrientationRef
        {
            get { return this.oref; }
            set { this.oref = value; }
        }

        /// <summary>
        /// Gets or sets the reference count. Used for serialization purposes
        /// </summary>
        /// <value>The reference count.</value>
        public int ReferenceCount
        {
            get { return this.i; }
            set { this.i = value; }
        }

        /// <summary>
        /// Gets the Selection List of the items.
        /// </summary>
        /// <value>
        /// Type: <see cref="NodeCollection"/>
        /// The list containing the selected items.
        /// </value>
        public NodeCollection SelectionList
        {
            get
            {
                if (mselectionList == null)
                {
                    mselectionList = new NodeCollection(this);
                }

                return mselectionList;
            }
        }

        /// <summary>
        /// Gets or sets the style reference. Used for Serialization purpose.
        /// </summary>
        /// <value>The style ref.</value>
        public Style StyleRef
        {
            get { return this.styleref; }
            set { this.styleref = value; }
        }

        /// <summary>
        ///  Gets or sets the SpaceBetweenSubTreeSpacing reference .Used for Serialization purpose.
        /// </summary>
        /// <value>The sub tree spacing reference.</value>
        public double SubTreeSpacingref
        {
            get { return this.sref; }
            set { this.sref = value; }
        }

        /// <summary>
        /// Gets or sets the vertical spacing reference .Used for Serialization purpose.
        /// </summary>
        /// <value>The vertical spacing reference.</value>
        public double VerticalSpacingref
        {
            get { return this.vref; }
            set { this.vref = value; }
        }

        /// <summary>
        /// Gets or sets the measure units.
        /// </summary>
        /// <value>The measure unit.</value>
        internal static MeasureUnits Munits
        {
            get { return munits; }
            set { munits = value; }
        }

        /// <summary>
        /// Gets or sets the const min X.
        /// </summary>
        /// <value>The const min X.</value>
        internal double ConstMinX
        {
            get { return this.cminx; }
            set { this.cminx = value; }
        }

        /// <summary>
        /// Gets or sets the const min Y.
        /// </summary>
        /// <value>The const min Y.</value>
        internal double ConstMinY
        {
            get { return this.cminy; }
            set { this.cminy = value; }
        }

        /// <summary>
        /// Gets or sets the current min X.
        /// </summary>
        /// <value>The current min X.</value>
        internal double CurrentMinX
        {
            get { return this.cmx; }
            set { this.cmx = value; }
        }

        /// <summary>
        /// Gets or sets the current min Y.
        /// </summary>
        /// <value>The current min Y.</value>
        internal double CurrentMinY
        {
            get { return this.cmy; }
            set { this.cmy = value; }
        }

        /// <summary>
        /// Gets or sets the Dragleft. Used for serialization purpose
        /// </summary>
        /// <value>The Dragged value.</value>
        internal double Dragleft
        {
            get { return this.dleft; }
            set { this.dleft = value; }
        }

        /// <summary>
        /// Gets or sets the Dragtop. Used for serialization purpose
        /// </summary>
        /// <value>The dragged value.</value>
        internal double Dragtop
        {
            get { return this.dtop; }
            set { this.dtop = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [greater than zero].
        /// </summary>
        /// <value><c>true</c> if [greater than zero]; otherwise, <c>false</c>.</value>
        internal bool GreaterThanZero
        {
            get { return this.gtz; }
            set { this.gtz = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [greater than zero Y].
        /// </summary>
        /// <value><c>true</c> if [greater than zero Y]; otherwise, <c>false</c>.</value>
        internal bool GreaterThanZeroY
        {
            get { return this.gtzy; }
            set { this.gtzy = value; }
        }

        /// <summary>
        /// Gets or sets the horizontal offset.
        /// </summary>
        /// <value>The horizontal offset.</value>
        internal double Hor
        {
            get { return this.horizontaloffset; }
            set { this.horizontaloffset = value; }
        }

        /// <summary>
        /// Gets or sets the horizontal offset value.
        /// </summary>
        /// <value>The horizontal offset value.</value>
        internal double HorValue
        {
            get { return this.hval; }
            set { this.hval = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether connector is dropped.
        /// </summary>
        /// <value>
        /// <c>true</c> if connector is dropped; otherwise, <c>false</c>.
        /// </value>
        internal bool IsConnectorDropped
        {
            get { return this.linedrop; }
            set { this.linedrop = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether diagram page is loaded.
        /// </summary>
        /// <value>
        /// <c>true</c> if diagram page is loaded; otherwise, <c>false</c>.
        /// </value>
        internal bool IsDiagrampageLoaded
        {
            get { return this.loaded; }
            set { this.loaded = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is positive.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is positive; otherwise, <c>false</c>.
        /// </value>
        internal bool IsPositive
        {
            get { return this.pos; }
            set { this.pos = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is positive Y.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is positive Y; otherwise, <c>false</c>.
        /// </value>
        internal bool IsPositiveY
        {
            get { return this.posy; }
            set { this.posy = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether  <see cref="Node"/> is rotated or resized.
        /// </summary>
        /// <value><c>true</c> if transformed; otherwise, <c>false</c>.</value>
        internal bool Istransformed
        {
            get { return this.transformed; }
            set { this.transformed = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the unit is changed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is unit changed; otherwise, <c>false</c>.
        /// </value>
        internal bool IsUnitChanged
        {
            get
            {
                return this.isunitchanged;
            }

            set
            {
                this.isunitchanged = value;
            }
        }

        /// <summary>
        /// Gets or sets the least X.
        /// </summary>
        /// <value>The least X.</value>
        internal double LeastX
        {
            get { return this.leastx; }
            set { this.leastx = value; }
        }

        /// <summary>
        /// Gets or sets the least Y.
        /// </summary>
        /// <value>The least Y.</value>
        internal double LeastY
        {
            get { return this.leasty; }
            set { this.leasty = value; }
        }

        internal bool ManualMeasureCall
        {
            get { return this.mcall; }
            set { this.mcall = value; }
        }

        /// <summary>
        /// Gets or sets the minleft.
        /// </summary>
        /// <value>The minleft.</value>
        internal double Minleft
        {
            get { return this.minleftX; }
            set { this.minleftX = value; }
        }

        /// <summary>
        /// Gets or sets the min top.
        /// </summary>
        /// <value>The min top.</value>
        internal double MinTop
        {
            get { return this.mintopY; }
            set { this.mintopY = value; }
        }

        /// <summary>
        /// Gets or sets the min X.
        /// </summary>
        /// <value>The min X.</value>
        internal double MinX
        {
            get { return this.minimumX; }
            set { this.minimumX = value; }
        }

        /// <summary>
        /// Gets or sets the min Y.
        /// </summary>
        /// <value>The min Y.</value>
        internal double MinY
        {
            get { return this.minimumY; }
            set { this.minimumY = value; }
        }

        /// <summary>
        /// Gets or sets the old min X.
        /// </summary>
        /// <value>The old min X.</value>
        internal double OldMinX
        {
            get { return this.curminx; }
            set { this.curminx = value; }
        }

        /// <summary>
        /// Gets or sets the old min Y.
        /// </summary>
        /// <value>The old min Y.</value>
        internal double OldMinY
        {
            get { return this.curminy; }
            set { this.curminy = value; }
        }

        /// <summary>
        /// Gets or sets the old unit.
        /// </summary>
        /// <value>The old unit.</value>
        internal MeasureUnits OldUnit
        {
            get
            {
                return this.ounit;
            }

            set
            {
                this.ounit = value;
            }
        }

        /// <summary>
        /// Gets or sets the scroll X.
        /// </summary>
        /// <value>The scroll X.</value>
        internal double ScrollX
        {
            get
            {
                return this.sx;
            }

            set
            {
                this.sx = value;
            }
        }

        /// <summary>
        /// Gets or sets the scroll Y.
        /// </summary>
        /// <value>The scroll Y.</value>
        internal double ScrollY
        {
            get
            {
                return this.sy;
            }

            set
            {
                this.sy = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the unit changed.
        /// </summary>
        /// <value><c>true</c> if unit converted; otherwise, <c>false</c>.</value>
        internal bool Unitconverted
        {
            get { return this.unitchan; }
            set { this.unitchan = value; }
        }

        /// <summary>
        /// Gets or sets the vertical offset.
        /// </summary>
        /// <value>The vertical offset.</value>
        internal double Ver
        {
            get { return this.verticaloffset; }
            set { this.verticaloffset = value; }
        }

        /// <summary>
        /// Gets or sets the vertical offset value.
        /// </summary>
        /// <value>The vertical offset value.</value>
        internal double VerValue
        {
            get { return this.vval; }
            set { this.vval = value; }
        }

        /// <summary>
        /// Creates a clone of the object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public static void Copyitem(object obj)
        {
            o = obj;
        }

        /// <summary>
        /// Invalidates the measures.
        /// </summary>
        public new void InvalidateMeasure()
        {
            base.InvalidateMeasure();
        }

        public override void OnApplyTemplate()
        {
            this.dview = Node.GetDiagramView(this);
            this.dc = GetDiagramControl(this);

            if (dc != null && dc.Model != null)
            {
                if (dc.Model.MeasurementUnits != MeasurementUnits)
                {
                    dc.Model.m_IsPixelDefultUnit = false;
                }
            }
        }

        /// <summary>
        /// Gets the Diagram Control object.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The Diagram Control object</returns>
        internal static DiagramControl GetDiagramControl(FrameworkElement element)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(element);
            while (parent != null)
            {
                if (parent is DiagramControl)
                {
                    return parent as DiagramControl;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }

        /// <summary>
        /// Measures elements.
        /// </summary>
        /// <param name="availableSize">The available size</param>
        /// <returns>The available size.</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            this.dc = GetDiagramControl(this);
            Size size = new Size();
            foreach (UIElement element in this.Children)
            {
                if (element is INodeGroup)
                {
                    if ((element as INodeGroup).ReferenceNo < 0)
                    {
                        (element as INodeGroup).ReferenceNo = ReferenceCount++;
                    }
                }

                //if (element is IShape)
                //{
                //    (element as IShape).LogicalOffsetX = (element as IShape).LogicalOffsetX;// MeasureUnitsConverter.FromPixels((element as IShape).LogicalOffsetX, this.MeasurementUnits);
                //    (element as IShape).LogicalOffsetY = (element as IShape).LogicalOffsetY;// MeasureUnitsConverter.FromPixels((element as IShape).LogicalOffsetY, this.MeasurementUnits);
                //}

                double offx;
                double offy;
                if (element is IShape)
                {
                    offx = (element as Node).PxLogicalOffsetX;// MeasureUnitsConverter.ToPixels((element as IShape).LogicalOffsetX, this.MeasurementUnits);
                    offy = (element as Node).PxLogicalOffsetY;// MeasureUnitsConverter.ToPixels((element as IShape).LogicalOffsetY, this.MeasurementUnits);
                }
                else if (element is IEdge)
                {
                    offx = Math.Max((element as LineConnector).PxStartPointPosition.X, (element as LineConnector).PxEndPointPosition.X);
                    offy = Math.Max((element as LineConnector).PxStartPointPosition.Y, (element as LineConnector).PxEndPointPosition.Y); ;
                }
                else
                {
                    offx = 0;
                    offy = 0;
                }

                if (double.IsNaN(offx))
                {
                    offx = 0;
                }
                if (double.IsNaN(offy))
                {
                    offy = 0;
                }
                try
                {
                    element.Measure(availableSize);
                    Size dsize = element.DesiredSize;
                    if (!(element is DiagramViewGrid) && !double.IsNaN(dsize.Width) && !double.IsNaN(dsize.Height))
                    {
                        size.Width = Math.Max(size.Width, offx + dsize.Width);
                        size.Height = Math.Max(size.Height, offy + dsize.Height);
                    }
                }
                catch
                {

                }


            }
            if (dview != null)
            {
                size.Width *= dview.CurrentZoom;
                size.Height *= dview.CurrentZoom;
            }
            return size;
        }

        /// <summary>
        /// Raised when the appropriate property changes.
        /// </summary>
        /// <param name="name">The property name.</param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        /// <summary>
        /// Positions child elements and determines a size for the control.
        /// </summary>
        /// <param name="finalSize">The final area within the parent
        /// that this element should use to arrange itself and its children.</param>
        /// <returns>The actual size used.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (UIElement element in this.Children)
            {
                double offsetX = 0;
                double offsetY = 0;
                if (element is IShape)
                {
                    offsetX = (element as Node).PxLogicalOffsetX;// MeasureUnitsConverter.ToPixels((element as IShape).LogicalOffsetX, this.MeasurementUnits);
                    offsetY = (element as Node).PxLogicalOffsetY;// MeasureUnitsConverter.ToPixels((element as IShape).LogicalOffsetY, this.MeasurementUnits);
                }
                if (double.IsNaN(offsetX))
                {
                    offsetX = 0;
                }
                if (double.IsNaN(offsetY))
                {
                    offsetY = 0;
                }
                Size dsize = element.DesiredSize;
                //if (!(element is DiagramViewGrid))
                try
                {
                    element.Arrange(new Rect(offsetX, offsetY, dsize.Width, dsize.Height));
                }
                catch
                { }
            }

            DiagramControl.IsPageLoaded = false;
            this.dc.View.Deleted = false;
            return finalSize;
        }

        /// <summary>
        /// Called when [units changed].
        /// </summary>
        /// <param name="d">The DependencyObject.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnUnitsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramPage page = (DiagramPage)d;

            if (page.dc != null && page.dc.Model != null)
            {
                DiagramModel model = page.dc.Model;
                model.MeasurementUnits = (MeasureUnits)e.NewValue;
                if (model != null && model.m_IsPixelDefultUnit)
                {
                    model.HorizontalSpacing = MeasureUnitsConverter.Convert(model.HorizontalSpacing, (MeasureUnits)e.OldValue, (MeasureUnits)e.NewValue);
                    model.VerticalSpacing = MeasureUnitsConverter.Convert(model.VerticalSpacing, (MeasureUnits)e.OldValue, (MeasureUnits)e.NewValue);
                    model.SpaceBetweenSubTrees = MeasureUnitsConverter.Convert(model.SpaceBetweenSubTrees, (MeasureUnits)e.OldValue, (MeasureUnits)e.NewValue);
                    page.dc.View.SnapOffsetX = MeasureUnitsConverter.Convert(page.dc.View.SnapOffsetX, (MeasureUnits)e.OldValue, (MeasureUnits)e.NewValue);
                    page.dc.View.SnapOffsetY = MeasureUnitsConverter.Convert(page.dc.View.SnapOffsetY, (MeasureUnits)e.OldValue, (MeasureUnits)e.NewValue);
                    page.setGridOffsets(MeasureUnitsConverter.Convert(page.GridHorizontalOffset, (MeasureUnits)e.OldValue, (MeasureUnits)e.NewValue), GridOffsets.Horizontal);
                    page.setGridOffsets(MeasureUnitsConverter.Convert(page.GridVerticalOffset, (MeasureUnits)e.OldValue, (MeasureUnits)e.NewValue), GridOffsets.Vertical);
                }
                model.m_IsPixelDefultUnit = true;
            }

            page.IsUnitChanged = true;
            page.OldUnit = page.temp;
            if (page.isexe)
            {
                page.isnotpixelwh = true;
                page.isnotpixeloffset = true;
                page.Unitconverted = true;
                if (page.OldUnit != page.MeasurementUnits)
                {
                    page.munitchanged = true;
                }
            }

            page.isexe = true;
            page.temp = page.MeasurementUnits;
            page.UpdateNodeLayout();
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.PreviewMouseWheel"/> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseWheelEventArgs"/> that contains the event data.</param>
        private void DiagramPage_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            bool wheeldown = false;

            if (this.CurrentMinY < 0)
            {
                if (!this.dc.View.IsJustWheeled)
                {
                    this.dc.View.IsJustWheeled = true;
                    this.Dragtop += this.MinTop;
                }
            }

            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (dview.IsZoomEnabled)
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

            if (Keyboard.Modifiers != ModifierKeys.Control)
            {
                this.s = Math.Min(48, Math.Abs(this.dc.View.Scrollviewer.VerticalOffset));
                double minus = Math.Abs(this.Dragtop) - (2 * this.s);
                if (e.Delta > 0)
                {
                    this.oncezero = false;
                    if (this.dc.View.Scrollviewer.VerticalOffset != 0)
                    {
                        this.scrollcount--;
                        if (this.CurrentMinY < 0 && !wheeldown)
                        {
                            this.dc.View.Y -= (this.s + this.Dragtop + minus + this.dc.View.PanConstant) / this.dc.View.CurrentZoom;
                        }
                        else
                        {
                            this.dc.View.Y += (this.s + this.dc.View.PanConstant) / this.dc.View.CurrentZoom;
                        }
                    }
                }
                else
                {
                    this.once = false;
                    if (this.dc.View.Scrollviewer.VerticalOffset < this.dc.View.Scrollviewer.ScrollableHeight)
                    {
                        this.scrollcount++;
                        this.max = Math.Min(Math.Abs(this.dc.View.Scrollviewer.ScrollableHeight - this.dc.View.Scrollviewer.VerticalOffset), 48);
                        wheeldown = true;
                        this.dc.View.Y -= (this.max + this.dc.View.PanConstant) / this.dc.View.CurrentZoom;
                    }
                }

                if (this.dc.View.Scrollviewer.VerticalOffset == this.dc.View.Scrollviewer.ScrollableHeight && e.Delta > 0)
                {
                    if (!this.once)
                    {
                        this.once = true;
                        this.dc.View.ViewGridOrigin = new Point(this.dc.View.ViewGridOrigin.X, this.dc.View.Y * this.dc.View.CurrentZoom);
                    }
                }

                if (this.dc.View.Scrollviewer.VerticalOffset == 0 && e.Delta < 0)
                {
                    if (!this.oncezero)
                    {
                        this.oncezero = true;
                        this.dc.View.ViewGridOrigin = new Point(this.dc.View.ViewGridOrigin.X, this.dc.View.Y * this.dc.View.CurrentZoom);
                    }
                }

                if (this.dc.View.Scrollviewer.VerticalOffset != this.dc.View.Scrollviewer.ScrollableHeight && this.dc.View.Scrollviewer.VerticalOffset != 0)
                {
                    this.dc.View.ViewGridOrigin = new Point(this.dc.View.ViewGridOrigin.X, this.dc.View.Y * this.dc.View.CurrentZoom);
                }
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
            ConnectorBase line = null;
            PreviewConnectorDropEventRoutedEventArgs linedropnewEventArgs = new PreviewConnectorDropEventRoutedEventArgs();
            dview.OnPreviewConnectorDrop(line, linedropnewEventArgs);
            this.IsConnectorDropped = true;
            line = new LineConnector();
            line.MeasurementUnit = this.MeasurementUnits;
            line.ConnectorType = connectortype;
            line.DropPoint = position;
            line.PxStartPointPosition = new Point(line.DropPoint.X - 25, line.DropPoint.Y - 25);
            line.PxEndPointPosition = new Point(line.DropPoint.X + 25, line.DropPoint.Y + 25);
            line.UpdateConnectorPathGeometry();
            diagctrl.Model.Connections.Add(line);
            this.SelectionList.Clear();
            this.SelectionList.Add(line);
            line.Focus();
            ConnectorDroppedRoutedEventArgs newEventArgs = new ConnectorDroppedRoutedEventArgs(line);
            dview.OnConnectorDrop(line, newEventArgs);
        }

        private void DiagramPage_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    DiagramView.MoveUp(this.dview);
                    break;
                case Key.Down:
                    DiagramView.MoveDown(this.dview);
                    break;
                case Key.Left:
                    DiagramView.MoveLeft(this.dview);
                    break;
                case Key.Right:
                    DiagramView.MoveRight(this.dview);
                    break;
                case Key.Delete:
                    this.dview.DeleteObjects(this.dview);
                    break;
                default:
                    break;
            }

            if (this.dview.SelectionList.Count > 0)
            {
                // e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        /// <summary>
        /// Is called when the diagram page gets loaded.
        /// </summary>
        /// <param name="sender">Diagram page</param>
        /// <param name="e">Event args</param>
        private void DiagramPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.diagctrl = GetDiagramControl(this);
            this.dview = this.diagctrl.View;
            this.temp = this.MeasurementUnits;
            page = this.dview.Page as DiagramPage;
        }


        /// <summary>
        /// Handles the MouseRightButtonDown event of the DiagramPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        void DiagramPage_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource == this)
            {
                this.SelectionList.Clear();
                // this.startPoint = e.GetPosition(this.dview);

            }

        }

        /// <summary>
        /// Is called when mouse left button is down on the diagram page.
        /// </summary>
        /// <param name="sender">Diagram page</param>
        /// <param name="e">Event args</param>
        private void DiagramPage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (e.OriginalSource == this && dview.IsPageEditable)
            {
                this.SelectionList.Clear();
                this.startPoint = e.GetPosition(this.dview);
                this.spoint = e.GetPosition(this);
            }
        }

        /// <summary>
        /// Is called when mouse left button is up on the diagram page.
        /// </summary>
        /// <param name="sender">Diagram page</param>
        /// <param name="e">Event args</param>
        private void DiagramPage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.ReleaseMouseCapture();
            if (this.startPoint.HasValue)
            {
                this.startPoint = null;
            }

            if (this.dview.SelectionCanvas.Children.Count > 0)
            {
                this.dview.SelectionCanvas.Children.Remove(this.dview.SelectionCanvas.Children.ElementAt(this.dview.SelectionCanvas.Children.Count() - 1));
            }

            CollectionExt.Cleared = false;
            if (this.dc.View.IsPageEditable)
            {
                if (SymbolPaletteItem.Hasvalue)
                {
                    SymbolPaletteItem.Hasvalue = false;
                    object content = o;
                    Node newItem = new Node();
                    if (content != null)
                    {
                        if (content is System.Windows.Shapes.Path && (content as System.Windows.Shapes.Path).Tag.ToString().Contains("Orthogonal"))
                        {
                            this.DropLine(ConnectorType.Orthogonal, e.GetPosition(this), this.dc);
                        }
                        else if (content is System.Windows.Shapes.Path && (content as System.Windows.Shapes.Path).Tag.ToString().Contains("Straight"))
                        {
                            this.DropLine(ConnectorType.Straight, e.GetPosition(this), this.dc);
                        }
                        else if (content is System.Windows.Shapes.Path && (content as System.Windows.Shapes.Path).Tag.ToString().Contains("Bezier"))
                        {
                            this.DropLine(ConnectorType.Bezier, e.GetPosition(this), this.dc);
                        }
                        else
                        {
                            PreviewNodeDropEventRoutedEventArgs newEventArgs1 = new PreviewNodeDropEventRoutedEventArgs();
                            dview.OnPreviewNodeDrop(newItem, newEventArgs1);

                            if (content is System.Windows.Shapes.Path)
                            {
                                var uri = new Uri("/Syncfusion.Diagram.Silverlight;component/Themes/NodeShapes.xaml", UriKind.Relative);
                                var streamResourceInfo = Application.GetResourceStream(uri);
                                string rsxaml = null;
                                using (var resourceStream = streamResourceInfo.Stream)
                                {
                                    using (var streamReader = new System.IO.StreamReader(resourceStream))
                                    {
                                        rsxaml = streamReader.ReadToEnd();
                                    }
                                }

                                this.symbols = XamlReader.Load(rsxaml) as ResourceDictionary;
                                newItem.NodeShape = new System.Windows.Shapes.Path();
                                System.Windows.Shapes.Path p = content as System.Windows.Shapes.Path;
                                newItem.NodeShape.IsHitTestVisible = false;
                                Shapes sp = Shapes.CustomPath;
                                if (!Enum.TryParse<Shapes>((p as FrameworkElement).Tag.ToString().Replace("PART_", ""), out sp))
                                {
                                    sp = Shapes.CustomPath;
                                }
                                newItem.NodeShape.Style = this.symbols[(p as FrameworkElement).Tag.ToString()] as Style;
                                if (newItem.NodeShape.Data == null && Pathstring != string.Empty)
                                {
                                    string pathXaml = "<Path xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" Data=\"" + Pathstring + "\"/>";
                                    newItem.NodeShape = (System.Windows.Shapes.Path)System.Windows.Markup.XamlReader.Load(pathXaml);
                                }
                                //newItem.Shape = sp;
                                newItem.MeasurementUnits = this.MeasurementUnits;
                                //Style CPS = new System.Windows.Style(typeof(Path));
                                //CPS.Setters.Add(new Setter(Path.FillProperty, p.Fill));
                                //CPS.Setters.Add(new Setter(Path.StretchProperty, p.Stretch));
                                //CPS.Setters.Add(new Setter(Path.StrokeProperty, p.Stroke));
                                //CPS.Setters.Add(new Setter(Path.StrokeThicknessProperty, p.StrokeThickness));
                                //newItem.CustomPathStyle = CPS;

                                ////newItem.NodeShape.Fill = p.Fill;
                                newItem.NodePathFill = p.Fill;
                                newItem.NodeShape.Stretch = p.Stretch;
                                ////newItem.NodeShape.Stroke = p.Stroke;
                                newItem.NodePathStroke = p.Stroke;
                                //newItem.NodeShape.Height = double.NaN;
                                //newItem.NodeShape.Width = double.NaN;
                                ////newItem.NodeShape.StrokeThickness = p.StrokeThickness;
                                //newItem.NodePathStrokeThickness = p.StrokeThickness;
                                //newItem.NodeShape.Stretch = Stretch.Fill;
                                newItem.Shape = sp;
                            }
                            else
                            {
                                if (content is UIElement)
                                {
                                    if (content is Panel)
                                    {
                                        Panel panel = content as Panel;
                                        for (int i = 0; i < (content as Panel).Children.Count; i++)
                                        {
                                            if ((content as Panel).Children[i] is System.Windows.Shapes.Path)
                                            {
                                                System.Windows.Shapes.Path ele = (content as Panel).Children[i] as System.Windows.Shapes.Path;
                                                System.Windows.Shapes.Path p = ele;
                                                string pathXaml = "<Path xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" Data=\"" + ele.Tag + "\"/>";
                                                ele = (System.Windows.Shapes.Path)System.Windows.Markup.XamlReader.Load(pathXaml);
                                                ele.Fill = p.Fill;
                                                ele.Stroke = p.Stroke;
                                                ele.Style = p.Style;
                                                ele.Height = p.Height;
                                                ele.Width = p.Width;
                                                ele.Stretch = p.Stretch;
                                                (content as Panel).Children[i] = ele;
                                            }

                                        }
                                    }
                                    (content as UIElement).IsHitTestVisible = true;
                                    newItem.Content = content;
                                    (newItem.Content as FrameworkElement).Name = (content as FrameworkElement).Name + "n";
                                }
                                else
                                {
                                    newItem.Content = content;
                                }
                            }
                            NodeDroppedRoutedEventArgs newEventArgs = new NodeDroppedRoutedEventArgs(newItem, newItem.Name);
                            dview.OnNodeDropped(newItem, newEventArgs);
                            this.SelectionList.Clear();
                            this.SelectionList.Add(newItem);
                            newItem.Focus();
                            Point position = e.GetPosition(this);
                            newItem.PxLogicalOffsetX = Math.Max(0, position.X);// MeasureUnitsConverter.FromPixels(Math.Max(0, position.X), this.MeasurementUnits);
                            newItem.PxLogicalOffsetY = Math.Max(0, position.Y);// MeasureUnitsConverter.FromPixels(Math.Max(0, position.Y), this.MeasurementUnits);
                            newItem.Width = 50;
                            newItem.Height = 50;
                            if (this.childcount)
                            {
                                this.no = this.Children.Count + 1;
                                this.childcount = false;
                            }

                            this.namecount = this.no++;
                            string str = "Node" + this.namecount;
                            try
                            {
                                if (string.IsNullOrEmpty(newItem.Name))
                                {
                                    if (this.diagctrl.Model.Nodes.Count == 0)
                                    {
                                        newItem.Name = str;
                                    }

                                    foreach (IShape n in this.diagctrl.Model.Nodes)
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

                            if (newItem.Content is FrameworkElement)
                            {
                                (newItem.Content as FrameworkElement).Height = double.NaN;
                                (newItem.Content as FrameworkElement).Width = double.NaN;
                                (newItem.Content as FrameworkElement).Name = newItem.Name + "content";
                            }

                            newItem.Page = this;
                            this.dc.Model.Nodes.Add(newItem);
                        }
                    }

                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Is called when mouse is moved on the diagram page.
        /// </summary>
        /// <param name="sender">Diagram page</param>
        /// <param name="e">Event args</param>
        private void DiagramPage_MouseMove(object sender, MouseEventArgs e)
        {

            if (this.dview.IsPageEditable == true)
            {

                if (this.startPoint.HasValue)
                {


                    if (this.dview.SelectionCanvas.Children.Count > 0)
                    {
                        this.dview.SelectionCanvas.Children.Remove(this.dview.SelectionCanvas.Children.ElementAt(this.dview.SelectionCanvas.Children.Count() - 1));
                    }

                    this.CaptureMouse();
                    this.SelectionList.Clear();
                    this.endPoint = e.GetPosition(this.dview);
                    Rect selectedArea = new Rect(this.startPoint.Value, this.endPoint);
                    Border selectionadorner = new Border();
                    selectionadorner.Background = new SolidColorBrush(Colors.Gray);
                    selectionadorner.BorderBrush = new SolidColorBrush(Colors.Black);
                    selectionadorner.BorderThickness = new Thickness(2);
                    Canvas.SetLeft(selectionadorner, selectedArea.X);
                    Canvas.SetTop(selectionadorner, selectedArea.Y);
                    selectionadorner.Width = selectedArea.Width;
                    selectionadorner.Height = selectedArea.Height;
                    selectionadorner.Opacity = .5;
                    this.dview.SelectionCanvas.Children.Add(selectionadorner);

                    foreach (Control item in this.Children)
                    {
                        if (item is Node)
                        {
                            Point p = new Point();
                            p = e.GetPosition(this);
                            selectedArea = new Rect(spoint, p);
                            Rect itemRect = new Rect(0, 0, (item as Node).ActualWidth, (item as Node).ActualHeight);
                            Rect itemBounds = item.TransformToVisual(this).TransformBounds(itemRect);
                            // Rect itemRect = new Rect((item as Node).PxLogicalOffsetX, (item as Node).PxLogicalOffsetY, item.ActualWidth, item.ActualHeight);
                            // if (selectedArea.Contains(new Point(itemRect.X - this.dview.Scrollviewer.HorizontalOffset, itemRect.Y - this.dview.Scrollviewer.VerticalOffset)) && selectedArea.Contains(new Point(itemRect.X + itemRect.Width - this.dview.Scrollviewer.HorizontalOffset, itemRect.Y + itemRect.Height - this.dview.Scrollviewer.VerticalOffset)))
                            if (selectedArea.Contains(new Point(itemBounds.X, itemBounds.Y)) && selectedArea.Contains(new Point(itemBounds.Right, itemBounds.Bottom)))
                            {
                                if ((item as Node).AllowSelect)
                                {
                                    this.SelectionList.Add(item);
                                }
                            }

                        }
                        else if (item is LineConnector)
                        {
                            if (selectedArea.Contains((item as LineConnector).PxStartPointPosition) && selectedArea.Contains((item as LineConnector).PxEndPointPosition) && !(item as LineConnector).IsSelected)
                            {
                                this.SelectionList.Add(item);
                            }
                        }
                    }
                }


            }
            else
            {
                //this.Cursor = Cursors.Arrow;
            }



        }

        /// <summary>
        /// Updates the node offset positions and size with respect to the current unit.
        /// </summary>
        private void UpdateNodeLayout()
        {
            DiagramPage.Munits = this.MeasurementUnits;
            foreach (UIElement node in this.Children)
            {
                if (this.isnotpixeloffset)
                {
                    if (node is Node)
                    {
                        //double poffsetX = (node as Node).PxLogicalOffsetX;// MeasureUnitsConverter.ToPixels((node as Node).LogicalOffsetX, this.OldUnit);
                        //double coffsetX = poffsetX;// MeasureUnitsConverter.Convert(poffsetX, MeasureUnits.Pixel, this.MeasurementUnits);
                        //(node as Node).PxLogicalOffsetX = coffsetX;
                        //double poffsetY = (node as Node).PxLogicalOffsetX;// MeasureUnitsConverter.ToPixels((node as Node).LogicalOffsetY, this.OldUnit);
                        //double coffsetY = poffsetY;// MeasureUnitsConverter.Convert(poffsetY, MeasureUnits.Pixel, this.MeasurementUnits);
                        //(node as Node).PxLogicalOffsetY = coffsetY;

                    }

                    if (this.notfirstexe)
                    {
                        if (!this.exeonce)
                        {
                            if (node is Node)
                            {
                                //this.dc = GetDiagramControl((FrameworkElement)(node as Node));
                                //Thickness rect = new Thickness();
                                //rect = this.dc.View.PxBounds;// MeasureUnitsConverter.ToPixels(this.dc.View.Bounds, this.OldUnit);
                                //this.dc.View.PxBounds = rect;// MeasureUnitsConverter.FromPixels(rect, this.MeasurementUnits);

                                //double vspace = this.dc.Model.PxVerticalSpacing;// MeasureUnitsConverter.ToPixels(this.dc.Model.VerticalSpacing, this.OldUnit);
                                //this.dc.Model.PxVerticalSpacing = vspace;// MeasureUnitsConverter.FromPixels(vspace, this.MeasurementUnits);
                                //double hspace = this.dc.Model.PxHorizontalSpacing;// MeasureUnitsConverter.ToPixels(this.dc.Model.HorizontalSpacing, this.OldUnit);
                                //this.dc.Model.PxHorizontalSpacing = hspace;// MeasureUnitsConverter.FromPixels(hspace, this.MeasurementUnits);
                                //double subspace = this.dc.Model.PxSpaceBetweenSubTrees;// MeasureUnitsConverter.ToPixels(this.dc.Model.SpaceBetweenSubTrees, this.OldUnit);
                                //this.dc.Model.PxSpaceBetweenSubTrees = subspace;// MeasureUnitsConverter.FromPixels(subspace, this.MeasurementUnits);
                                //this.exeonce = true;
                            }
                        }
                    }

                    this.notfirstexe = true;
                }

                if (!this.isnotpixelwh || !this.munitchanged)
                {
                    if (node is Node)
                    {
                        //double oldwidth = (node as Node).Width;// MeasureUnitsConverter.FromPixels((node as Node).Width, this.OldUnit);
                        //double a = oldwidth;// MeasureUnitsConverter.Convert(oldwidth, this.MeasurementUnits, MeasureUnits.Pixel);
                        //(node as Node).Width = a;

                        //double oldheight = (node as Node).Height;// MeasureUnitsConverter.FromPixels((node as Node).Height, this.OldUnit);
                        //double b = oldheight;// MeasureUnitsConverter.Convert(oldheight, this.MeasurementUnits, MeasureUnits.Pixel);
                        //(node as Node).Height = b;
                        //foreach (ConnectionPort port in (node as Node).Ports)
                        //{
                        //    double portleft = port.PxLeft;// MeasureUnitsConverter.FromPixels(port.Left, this.OldUnit);
                        //    double c = portleft;// MeasureUnitsConverter.Convert(portleft, this.MeasurementUnits, MeasureUnits.Pixel);
                        //    port.PxLeft = c;
                        //    double porttop = port.PxTop;// MeasureUnitsConverter.FromPixels(port.Top, this.OldUnit);
                        //    double d = porttop;// MeasureUnitsConverter.Convert(porttop, this.MeasurementUnits, MeasureUnits.Pixel);
                        //    port.PxTop = d;
                        //}
                    }
                    else
                    {
                        //this.munitchanged = false;
                    }
                }
            }

            this.exeonce = false;
        }

        #region Class variables

        #endregion

        #region Initialization

        #endregion

        #region Class Override

        #endregion

        #region  Properties

        #endregion

        #region Internal Properties

        #endregion

        #region DPs

        #endregion

        #region Events

        #endregion

        #region Implementation

        #endregion

        #region IDiagramPanel Members

        #endregion

        #region INotifyPropertyChanged Members

        #endregion

        #region Private Functions
        private void setGridOffsets(double value, GridOffsets offset)
        {
            double pixval;
            if (page != null)
            {
                pixval = MeasureUnitsConverter.ToPixels(value, page.MeasurementUnits);
                if (pixval < 1)
                {
                    value = MeasureUnitsConverter.FromPixels(1, page.MeasurementUnits);
                }
                if (offset == GridOffsets.Horizontal)
                {
                    this.horoffset = value;
                }
                else if (offset == GridOffsets.Vertical)
                {
                    this.vertoffset = value;
                }
            }
        }

        private void updateGridLines(GridOffsets offset)
        {
            if (this.Children.Count > 0 && this.Children[0] is DiagramViewGrid)
            {
                if (offset == GridOffsets.Horizontal)
                {
                    (this.Children[0] as DiagramViewGrid).RearrrangeHorGridLines();
                }
                else if (offset == GridOffsets.Vertical)
                {
                    (this.Children[0] as DiagramViewGrid).RearrrangeVerGridLines();
                }
            }
        }
        #endregion
    }
}
