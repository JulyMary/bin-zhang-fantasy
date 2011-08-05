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
    using System.Linq;
    using System.Text;
    using System.Windows;

    /// <summary>
    /// Represents the Tablelayout used for the automatic arrangement of nodes. 
    /// </summary>
    /// <remarks>
    /// TableLayout  arranges the nodes  in a tabular structure based on specified intervals between them. The number of nodes in each row and column could be specified and the layout will take place accordingly. The nodes are assigned rows and columns based on the order in which they are added to the model and based on the maximum nodes allowed in that row and column. This layout enables to layout nodes automatically without the need to specify offset positions for each node.
    /// </remarks>
    /// <example>
    /// <code language="XAML">
    ///&lt;UserControl x:Class=&quot;Sample.MainPage&quot;
    /// xmlns=&quot;http://schemas.microsoft.com/winfx/2006/xaml/presentation&quot; 
    ///     xmlns:x=&quot;http://schemas.microsoft.com/winfx/2006/xaml&quot; 
    ///     xmlns:syncfusion=&quot;clr-namespace:Syncfusion.Windows.Diagram;assembly=Syncfusion.Diagram.Silverlight&quot;
    ///    xmlns:vsm=&quot;clr-namespace:System.Windows;assembly=System.Windows&quot; 
    ///             &gt;
    ///   &lt;syncfusion:DiagramControl Grid.Column="1" Name="diagramControl" 
    ///                                IsSymbolPaletteEnabled="True" 
    ///                                Background="WhiteSmoke"&gt;
    ///             &lt;syncfusion:DiagramControl.Model&gt;
    ///                   &lt;syncfusion:DiagramModel LayoutType="TableLayout" EnableLayoutWithVariedSizes="False" TableExpandMode="Horizontal" HorizontalSpacing="50" VerticalSpacing="50" RowCount="3" ColumnCount="3"  x:Name="diagramModel" &gt;
    ///                  &lt;/syncfusion:DiagramModel&gt;
    ///             &lt;/syncfusion:DiagramControl.Model&gt;
    ///           &lt;syncfusion:DiagramControl.View&gt;
    ///              &lt;syncfusion:DiagramView   Background="LightGray"  
    ///                                        Bounds="0,0,12,12"  
    ///                                        Name="diagramView"  &gt;
    ///              &lt;/syncfusion:DiagramView&gt;
    ///          &lt;/syncfusion:DiagramControl.View&gt;
    ///     &lt;/syncfusion:DiagramControl&gt;
    /// &lt;/Window&gt;
    /// </code>
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
    ///       Model = new DiagramModel ();
    ///       View = new DiagramView ();
    ///       Control.View = View;
    ///       Control.Model = Model;
    ///       View.Bounds = new Thickness(0, 0, 1000, 1000);
    ///        Node n = new Node(Guid.NewGuid(), "Node1");
    ///        n.Shape = Shapes.FlowChart_Start;
    ///        n.Label = "Start";
    ///        n.Level = 1;
    ///        n.Width = 150;
    ///        n.Height = 75;
    ///        Node n1 = new Node(Guid.NewGuid(), "Node2");
    ///        n1.Shape = Shapes.FlowChart_Process;
    ///        n1.IsLabelEditable = true;
    ///        n1.Width = 150;
    ///        n1.Height = 75;
    ///        Node n2 = new Node(Guid.NewGuid(), "Node3");
    ///        n2.Shape = Shapes.FlowChart_Process;
    ///        n2.Width = 150;
    ///        n2.Height = 75;
    ///        Model.Nodes.Add(n);
    ///        Model.Nodes.Add(n1);
    ///        Model.Nodes.Add(n2);
    ///        Model.LayoutType = LayoutType.TableLayout;
    ///        Model.TableExpandMode = ExpandMode.Horizontal;
    ///        Model.EnableLayoutWithVariedSizes = false;
    ///        Model.HorizontalSpacing = 50;
    ///        Model.VerticalSpacing = 50;
    ///        Model.RowCount = 10;
    ///        Model.ColumnCount = 3;
    ///    }
    ///    }
    ///    }
    /// </code>
    /// </example>
    public class TableLayout : TreeLayoutBase
    {
        /// <summary>
        /// Used to store the page.
        /// </summary>
        private DiagramPage diagrampage;

        /// <summary>
        /// Uset to store the horizontal spacing value.
        /// </summary>
        private double hspace = 0;

        /// <summary>
        /// Used to store the anchor x value.
        /// </summary>
        private double manchorX;

        /// <summary>
        /// Used to store the anchor y value.
        /// </summary>
        private double manchorY;

        /// <summary>
        /// Used to store the temporary anchor point
        /// </summary>
        private Point mtempAnchorPoint;

        /// <summary>
        /// Used to store the vertical spacing value.
        /// </summary>
        private double vspace = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableLayout"/> class.
        /// </summary>
        /// <param name="model">The model object.</param>
        /// <param name="view">The view object.</param>
        public TableLayout(DiagramModel model, DiagramView view)
            : base(model, view)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableLayout"/> class.
        /// </summary>
        /// <param name="model">The model instance.</param>
        public TableLayout(DiagramModel model)
            : base(model)
        {
        }

        /// <summary>
        /// Occurs when [property changed].
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the layout anchor.
        /// </summary>
        /// <value>The layout anchor.</value>
        internal Point LayoutAnchor
        {
            get
            {
                this.mtempAnchorPoint = new Point(0, 0);
                Thickness bounds = Bounds;
                this.mtempAnchorPoint = new Point((float)bounds.Right / 2F, (float)bounds.Bottom / 2F);
                return this.mtempAnchorPoint;
            }

            set
            {
                this.mtempAnchorPoint = value;
            }
        }
        
        /// <summary>
        /// Generates the layout for nodes of same size.
        /// </summary>
        public void DoLayout()
        {
            Point a = this.LayoutAnchor;
            this.manchorX = a.X;
            this.manchorY = a.Y;

            IShape root = this.LayoutRoot as IShape;
            int rowcount = 1;
            int columncount = 1;
            CollectionExt rootnodecollection = new CollectionExt();
            rootnodecollection.Add(this.Model.Nodes[0]);
            foreach (IShape shape in this.Model.Nodes)
            {
                //if (this.diagrampage.IsUnitChanged && !this.diagrampage.Unitconverted)
                {
//#if WPF
//                    (shape as Node).PixelWidth = MeasureUnitsConverter.ToPixels((shape as Node).Width, (this.View.Page as DiagramPage).MeasurementUnits);
//                    (shape as Node).PixelHeight = MeasureUnitsConverter.ToPixels((shape as Node).Height, (this.View.Page as DiagramPage).MeasurementUnits);
//#endif
//#if SILVERLIGHT
                    (shape as Node).PixelWidth = (shape as Node)._Width;
                    (shape as Node).PixelHeight = (shape as Node)._Height;
//#endif
                }
                //else
                //{
                //    (shape as Node).PixelWidth = (shape as Node).Width;
                //    (shape as Node).PixelHeight = (shape as Node).Height;
                //}

                shape.Model = Model;
                if (this.Model.TableExpandMode == ExpandMode.Horizontal)
                {
                    if (this.Model.ColumnCount == 0)
                    {
                        this.Model.ColumnCount = 1;
                    }

                    if (columncount <= Model.ColumnCount)
                    {
                        this.SetBreadthSpace(shape);
                        if (shape.PreviousShape != null)
                        {
//#if WPF
//                            this.SetY(shape, null, MeasureUnitsConverter.ToPixels((shape.PreviousShape as Node).LogicalOffsetY, this.diagrampage.MeasurementUnits));
//#endif
//#if SILVERLIGHT
                            this.SetY(shape, null,(shape.PreviousShape as Node).PxOffsetY);
//#endif
                        }
                        else
                        {
                            this.SetY(shape, null, this.manchorY);
                        }

                        columncount++;
                    }
                    else
                    {
                        rowcount++;
//#if WPF
//                        double y = MeasureUnitsConverter.ToPixels((rootnodecollection[rootnodecollection.Count - 1] as Node).LogicalOffsetY, (View.Page as DiagramPage).MeasurementUnits) + (rootnodecollection[rootnodecollection.Count - 1] as Node).PixelHeight + this.vspace;
//#endif
//#if SILVERLIGHT
                        double y = (rootnodecollection[rootnodecollection.Count - 1] as Node).PxOffsetY + (rootnodecollection[rootnodecollection.Count - 1] as Node).PixelHeight + this.vspace;
//#endif
                        this.SetY(shape, null, y);
                        this.SetX(shape, null, this.manchorX);
                        rootnodecollection.Add(shape);
                        columncount = 2;
                    }
                }
                else
                {
                    if (this.Model.RowCount == 0)
                    {
                        this.Model.RowCount = 1;
                    }

                    if (rowcount <= this.Model.RowCount)
                    {
                        this.SetDepthSpace(shape);
                        if (shape.PreviousShape != null)
                        {
//#if WPF
//                            this.SetX(shape, null, MeasureUnitsConverter.ToPixels((shape.PreviousShape as  Node).LogicalOffsetX, this.diagrampage.MeasurementUnits));
//#endif
//#if SILVERLIGHT
                            this.SetX(shape, null, (shape.PreviousShape as Node).PxOffsetX);
//#endif
                        }
                        else
                        {
                            this.SetX(shape, null, this.manchorX);
                        }

                        rowcount++;
                    }
                    else
                    {
                        columncount++;
//#if WPF
//                        double x = MeasureUnitsConverter.ToPixels((rootnodecollection[rootnodecollection.Count - 1] as Node).LogicalOffsetX, (this.View.Page as DiagramPage).MeasurementUnits) + (rootnodecollection[rootnodecollection.Count - 1] as Node).PixelWidth + this.hspace;
//#endif
//#if SILVERLIGHT
                        double x = (rootnodecollection[rootnodecollection.Count - 1] as Node).PxOffsetX + (rootnodecollection[rootnodecollection.Count - 1] as Node).PixelWidth + this.hspace;
//#endif
                        this.SetX(shape, null, x);
                        this.SetY(shape, null, this.manchorY);
                        rootnodecollection.Add(shape);
                        rowcount = 2;
                    }
                }
            }
        }

        /// <summary>
        /// Does the layout for the nodes of difffent sizes.
        /// </summary>
        public void DoLayoutWIthDifffentSizes()
        {
            int row = 1;
            int column = 0;
            if (this.Model.TableExpandMode == ExpandMode.Horizontal)
            {
                if (this.Model.ColumnCount == 0)
                {
                    this.Model.ColumnCount = 1;
                }

                this.Model.RowCount = (int)Math.Ceiling((double)(this.Model.Nodes.Count / (double)this.Model.ColumnCount));
            }
            else
            {
                if (this.Model.RowCount == 0)
                {
                    this.Model.RowCount = 1;
                }

                this.Model.ColumnCount = (int)Math.Ceiling((double)(this.Model.Nodes.Count / (double)this.Model.RowCount));
                row = 0;
                column = 1;
            }

            IShape[,] array = new IShape[this.Model.RowCount, this.Model.ColumnCount];
            Point a = this.LayoutAnchor;
            this.manchorX = a.X;
            this.manchorY = a.Y;
            IShape root = this.LayoutRoot as IShape;
            int rowcount = 1;
            int columncount = 1;
            ObservableCollection<Size> rowsizecollection = new ObservableCollection<Size>();
            ObservableCollection<Size> columnsizecollection = new ObservableCollection<Size>();
            foreach (IShape shape in this.Model.Nodes)
            {
                //if (this.diagrampage.IsUnitChanged && !this.diagrampage.Unitconverted)
                {
//#if WPF
//                    (shape as Node).PixelWidth = MeasureUnitsConverter.ToPixels((shape as Node).Width, (this.View.Page as DiagramPage).MeasurementUnits);
//                    (shape as Node).PixelHeight = MeasureUnitsConverter.ToPixels((shape as Node).Height, (this.View.Page as DiagramPage).MeasurementUnits);
//#endif
//#if SILVERLIGHT
                    (shape as Node).PixelWidth = (shape as Node)._Width;
                    (shape as Node).PixelHeight = (shape as Node)._Height;
//#endif
                }
                //else
                //{
                //    (shape as Node).PixelWidth = (shape as Node).Width;
                //    (shape as Node).PixelHeight = (shape as Node).Height;
                //}

                shape.Model = this.Model;
                if (this.Model.TableExpandMode == ExpandMode.Horizontal)
                {
                    if (columncount <= this.Model.ColumnCount)
                    {
                        column++;
                        shape.Row = row;
                        shape.Column = column;
                        columncount++;
                    }
                    else
                    {
                        row++;
                        columncount = 2;
                        column = 1;
                        shape.Row = row;
                        shape.Column = column;
                    }
                }
                else
                {
                    if (rowcount <= this.Model.RowCount)
                    {
                        row++;
                        shape.Row = row;
                        shape.Column = column;
                        rowcount++;
                    }
                    else
                    {
                        column++;
                        rowcount = 2;
                        row = 1;
                        shape.Row = row;
                        shape.Column = column;
                    }
                }

                array[shape.Row - 1, shape.Column - 1] = shape;
            }

            this.CalculateMaximumSize(array, rowsizecollection, columnsizecollection);

            this.LayoutNodes(array, rowsizecollection, columnsizecollection);
        }      
        
        /// <summary>
        /// Prepares for the activity
        /// </summary>
        /// <param name="layout">ILayout instance.</param>
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
        ///       Model = new DiagramModel ();
        ///       View = new DiagramView ();
        ///       Control.View = View;
        ///       Control.Model = Model;
        ///       View.Bounds = new Thickness(0, 0, 1000, 1000);
        ///        Node n = new Node(Guid.NewGuid(), "Start");
        ///        n.Shape = Shapes.FlowChart_Start;
        ///        n.Label = "Start";
        ///        n.Level = 1;
        ///        n.Width = 150;
        ///        n.Height = 75;
        ///        Node n1 = new Node(Guid.NewGuid(), "Decision1");
        ///        n1.Shape = Shapes.FlowChart_Process;
        ///        n1.IsLabelEditable = true;
        ///        n1.Label = "Alarm Rings";
        ///        n1.Level = 2;
        ///        n1.Width = 150;
        ///        n1.Height = 75;
        ///        Model.Nodes.Add(n);
        ///        Model.Nodes.Add(n1);
        ///        Model.LayoutType = LayoutType.HierarchicalTreeLayout;
        ///        Model.HorizontalSpacing = 30;
        ///        Model.VerticalSpacing = 50;
        ///        Model.SpaceBetweenSubTrees = 50;
        ///        LineConnector o = new LineConnector();
        ///        o.ConnectorType = ConnectorType.Straight;
        ///        o.TailNode = n1;
        ///        o.HeadNode = n;
        ///        o.HeadDecoratorShape = DecoratorShape.None;
        ///        Model.Connections.Add(o);
        ///        TableLayout tree = new TableLayout(View,Model);
        ///        table.PrepareActivity(table);
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        public void PrepareActivity(ILayout layout)
        {
            layout.Model = this.Model;

//#if WPF
//            layout.Bounds = MeasureUnitsConverter.ToPixels(this.View.Bounds, (this.View.Page as DiagramPage).MeasurementUnits);
//            layout.Center = new Point(this.View.Bounds.Right / 2, this.View.Bounds.Bottom / 2);
//#endif
//#if SILVERLIGHT
            layout.Bounds = this.View.PxBounds;
            layout.Center = new Point(this.View.PxBounds.Right / 2, this.View.PxBounds.Bottom / 2);
//#endif

        }

        /// <summary>
        /// Refreshes the layout.
        /// </summary>
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
        ///       Model = new DiagramModel ();
        ///       View = new DiagramView ();
        ///       Control.View = View;
        ///       Control.Model = Model;
        ///       View.Bounds = new Thickness(0, 0, 1000, 1000);
        ///        Node n = new Node(Guid.NewGuid(), "Start");
        ///        n.Shape = Shapes.FlowChart_Start;
        ///        n.Label = "Start";
        ///        n.Level = 1;
        ///        n.Width = 150;
        ///        n.Height = 75;
        ///        Node n1 = new Node(Guid.NewGuid(), "Decision1");
        ///        n1.Shape = Shapes.FlowChart_Process;
        ///        n1.IsLabelEditable = true;
        ///        n1.Label = "Alarm Rings";
        ///        n1.Level = 2;
        ///        n1.Width = 150;
        ///        n1.Height = 75;
        ///        Model.Nodes.Add(n);
        ///        Model.Nodes.Add(n1);
        ///        Model.LayoutType = LayoutType.HierarchicalTreeLayout;
        ///        Model.HorizontalSpacing = 30;
        ///        Model.VerticalSpacing = 50;
        ///        Model.SpaceBetweenSubTrees = 50;
        ///        DirectedTreeLayout tree=new DirectedTreeLayout();
        ///        tree.RefreshLayout();
        ///        LineConnector o = new LineConnector();
        ///        o.ConnectorType = ConnectorType.Straight;
        ///        o.TailNode = n1;
        ///        o.HeadNode = n;
        ///        o.HeadDecoratorShape = DecoratorShape.None;
        ///        Model.Connections.Add(o);
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        public void RefreshLayout()
        {
            this.PrepareActivity(this);
            this.StartNodeArrangement();
            (this.View.Page as DiagramPage).InvalidateMeasure();
            (this.View.Page as DiagramPage).InvalidateArrange();
        }

        /// <summary>
        /// Starts arranging the nodes.
        /// </summary>
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
        ///       Model = new DiagramModel ();
        ///       View = new DiagramView ();
        ///       Control.View = View;
        ///       Control.Model = Model;
        ///       View.Bounds = new Thickness(0, 0, 1000, 1000);
        ///        Node n = new Node(Guid.NewGuid(), "Start");
        ///        n.Shape = Shapes.FlowChart_Start;
        ///        n.Label = "Start";
        ///        n.Level = 1;
        ///        n.Width = 150;
        ///        n.Height = 75;
        ///        Node n1 = new Node(Guid.NewGuid(), "Decision1");
        ///        n1.Shape = Shapes.FlowChart_Process;
        ///        n1.IsLabelEditable = true;
        ///        n1.Label = "Alarm Rings";
        ///        n1.Level = 2;
        ///        n1.Width = 150;
        ///        n1.Height = 75;
        ///        Model.Nodes.Add(n);
        ///        Model.Nodes.Add(n1);
        ///        Model.LayoutType = LayoutType.HierarchicalTreeLayout;
        ///        Model.HorizontalSpacing = 30;
        ///        Model.VerticalSpacing = 50;
        ///        Model.SpaceBetweenSubTrees = 50;
        ///        LineConnector o = new LineConnector();
        ///        o.ConnectorType = ConnectorType.Straight;
        ///        o.TailNode = n1;
        ///        o.HeadNode = n;
        ///        o.HeadDecoratorShape = DecoratorShape.None;
        ///        Model.Connections.Add(o);
        ///        TableLayout table = new TableLayout(View,Model);
        ///        table.StartNodeArrangement();
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        public void StartNodeArrangement()
        {
            if (View.IsLoaded)
            {
                this.View.IsLayout = true;
                this.diagrampage = this.View.Page as DiagramPage;

                if (this.Model.InternalNodes.Count != 0)
                {
                    this.LayoutRoot = this.Model.Nodes[0] as IShape;
                    //if (!this.Model.IsDefaultHorizontal)
                    //{
                    //#if WPF
                    //                    this.hspace = (float)MeasureUnitsConverter.ToPixels(this.Model.HorizontalSpacing, (this.View.Page as DiagramPage).MeasurementUnits);
                    //#endif
                    //#if SILVERLIGHT
                    this.hspace = (float)this.Model.PxHorizontalSpacing;
                    //#endif
                    //}
                    //else
                    //{
                    //    this.hspace = 50;
                    //}

                    //if (!this.Model.IsDefaultVertical)
                    //{
                    //#if WPF
                    //                    this.vspace = (float)MeasureUnitsConverter.ToPixels(this.Model.VerticalSpacing, (this.View.Page as DiagramPage).MeasurementUnits);
                    //#endif
                    //#if SILVERLIGHT
                    this.vspace = (float)this.Model.PxVerticalSpacing;
                    //#endif
                    //}
                    //else
                    //{
                    //    this.vspace = 50;
                    //}

                    if (this.Model.EnableLayoutWithVariedSizes)
                    {
                        this.DoLayoutWIthDifffentSizes();
                    }
                    else
                    {
                        this.DoLayout();
                    }
                }

                this.View.IsLayout = false;
            }
            else
            {
#if WPF
                Model.dc.m_delayLayout = true;
#endif
            }
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
        /// Calculates the maximum size of the node in each row and column.
        /// </summary>
        /// <param name="array">The node array.</param>
        /// <param name="rowsizecollection">The rowsizecollection.</param>
        /// <param name="columnsizecollection">The columnsizecollection.</param>
        private void CalculateMaximumSize(IShape[,] array, ObservableCollection<Size> rowsizecollection, ObservableCollection<Size> columnsizecollection)
        {
            double maxwidth = 0;
            double maxheight = 0;
            for (int i = 0; i < this.Model.RowCount; i++)
            {
                for (int j = 0; j < this.Model.ColumnCount; j++)
                {
                    if ((array[i, j] as Node) != null)
                    {
                        if ((array[i, j] as Node).PixelWidth > maxwidth)
                        {
                            maxwidth = (array[i, j] as Node).PixelWidth;
                        }

                        if ((array[i, j] as Node).PixelHeight > maxheight)
                        {
                            maxheight = (array[i, j] as Node).PixelHeight;
                        }
                    }
                }

                rowsizecollection.Add(new Size(maxwidth, maxheight));
                maxwidth = maxheight = 0;
            }

            for (int i = 0; i < this.Model.ColumnCount; i++)
            {
                for (int j = 0; j < this.Model.RowCount; j++)
                {
                    if ((array[j, i] as Node) != null)
                    {
                        if ((array[j, i] as Node).PixelWidth > maxwidth)
                        {
                            maxwidth = (array[j, i] as Node).PixelWidth;
                        }

                        if ((array[j, i] as Node).PixelHeight > maxheight)
                        {
                            maxheight = (array[j, i] as Node).PixelHeight;
                        }
                    }
                }

                columnsizecollection.Add(new Size(maxwidth, maxheight));
                maxwidth = maxheight = 0;
            }
        }

        /// <summary>
        /// Calculates the offsetX values.
        /// </summary>
        /// <param name="node">The node object.</param>
        /// <param name="maxwidth">The maximum width of the column.</param>
        /// <param name="previousNode">The previous node.</param>
        /// <param name="previouscolumnwidth">The previous column width.</param>
        /// <returns>The offsetX value.</returns>
        private double GetX(Node node, double maxwidth, Node previousNode, double previouscolumnwidth)
        {
            double center = maxwidth / 2;
            double nodecenter = node.PixelWidth / 2;
            double diff = center - nodecenter;
            double a = 0;
            if (previousNode != null)
            {
//#if WPF
//                double posx = MeasureUnitsConverter.ToPixels(previousNode.LogicalOffsetX, (View.Page as DiagramPage).MeasurementUnits) + (previouscolumnwidth / 2);
//#endif
//#if SILVERLIGHT
                double posx = previousNode.PxOffsetX + (previouscolumnwidth / 2);
//#endif
                a = posx + (previousNode.PixelWidth / 2) + this.hspace;
            }
            else
            {
                a = this.manchorX;
            }

            return a + diff;
        }

        /// <summary>
        /// Calculates the offsetY values.
        /// </summary>
        /// <param name="node">The node object.</param>
        /// <param name="maxheight">The maximum height of the row.</param>
        /// <param name="previousNode">The previous node.</param>
        /// <param name="previousrowHeight">Height of the previous row.</param>
        /// <returns>The offsety value.</returns>
        private double GetY(Node node, double maxheight, Node previousNode, double previousrowHeight)
        {
            double center = maxheight / 2;
            double nodecenter = node.PixelHeight / 2;
            double diff = center - nodecenter;
            double a = 0;
            if (previousNode != null)
            {
//#if WPF
//                double posy = MeasureUnitsConverter.ToPixels(previousNode.LogicalOffsetY, (this.View.Page as DiagramPage).MeasurementUnits) + (previousrowHeight / 2);
//#endif
//#if SILVERLIGHT
                double posy = previousNode.PxOffsetY + (previousrowHeight / 2);
//#endif
                a = posy + (previousNode.PixelHeight / 2) + this.vspace;
            }
            else
            {
                a = this.manchorY;
            }

            return a + diff;
        }
       
        /// <summary>
        /// Layouts the nodes.
        /// </summary>
        /// <param name="array">The node array.</param>
        /// <param name="rowsizecollection">The rowsizecollection.</param>
        /// <param name="columnsizecollection">The columnsizecollection.</param>
        private void LayoutNodes(IShape[,] array, ObservableCollection<Size> rowsizecollection, ObservableCollection<Size> columnsizecollection)
        {
            for (int i = 0; i < this.Model.ColumnCount; i++)
            {
                for (int j = 0; j < this.Model.RowCount; j++)
                {
                    if ((array[j, i] as Node) != null)
                    {
                        Node previousNode;
                        double previousrowHeight = 0;
                        if (j != 0)
                        {
                            previousNode = array[j - 1, i] as Node;
                            previousrowHeight = rowsizecollection[j - 1].Height;
                        }
                        else
                        {
                            previousNode = null;
                        }

                        double y = this.GetY((array[j, i] as Node), rowsizecollection[j].Height, previousNode, previousrowHeight);
                        this.SetY((array[j, i] as Node), null, y);
                    }
                }
            }

            for (int i = 0; i < this.Model.RowCount; i++)
            {
                for (int j = 0; j < this.Model.ColumnCount; j++)
                {
                    if ((array[i, j] as Node) != null)
                    {
                        Node previousNode;
                        double previouscolumnwidth = 0;
                        if (j != 0)
                        {
                            previousNode = array[i, j - 1] as Node;
                            previouscolumnwidth = columnsizecollection[j - 1].Width;
                        }
                        else
                        {
                            previousNode = null;
                        }

                        double x = this.GetX((array[i, j] as Node), columnsizecollection[j].Width, previousNode, previouscolumnwidth);
                        this.SetX((array[i, j] as Node), null, x);
                    }
                }
            }
        }

        /// <summary>
        /// Sets the breadth space.
        /// </summary>
        /// <param name="currentShape">The current shape.</param>
        private void SetBreadthSpace(IShape currentShape)
        {
            double a = 0;

            if (currentShape.PreviousShape != null)
            {
//#if WPF
//                a = MeasureUnitsConverter.ToPixels((currentShape.PreviousShape as Node).LogicalOffsetX, (this.View.Page as DiagramPage).MeasurementUnits) + (currentShape.PreviousShape as Node).PixelWidth + this.hspace;
//#endif
//#if SILVERLIGHT
                a = (currentShape.PreviousShape as Node).PxOffsetX + (currentShape.PreviousShape as Node).PixelWidth + this.hspace;
//#endif
            }
            else
            {
                a = this.manchorX;
            }

            this.SetX(currentShape, null, a);
        }

        /// <summary>
        /// Sets the depth space.
        /// </summary>
        /// <param name="currentShape">The current shape.</param>
        private void SetDepthSpace(IShape currentShape)
        {
            double a = 0;

            if (currentShape.PreviousShape != null)
            {
//#if WPF
//                a = MeasureUnitsConverter.ToPixels((currentShape.PreviousShape as Node).LogicalOffsetY, (this.View.Page as DiagramPage).MeasurementUnits) + (currentShape.PreviousShape as Node).PixelHeight + this.vspace;
//#endif
//#if SILVERLIGHT
                a = (currentShape.PreviousShape as Node).PxOffsetY + (currentShape.PreviousShape as Node).PixelHeight + this.vspace;
//#endif
            }
            else
            {
                a = this.manchorY;
            }

            this.SetY(currentShape, null, a);
        }
       
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Initialization

        #endregion

        #region INotifyPropertyChanged Members

        #endregion

        #region Methods

        #endregion
    }
}
