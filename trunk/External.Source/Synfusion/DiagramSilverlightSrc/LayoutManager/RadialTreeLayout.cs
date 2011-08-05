// <copyright file="DirectedTreeLayout.cs" company="Syncfusion">
// Copyright Syncfusion Inc. 2001 - 2011. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
//using System.Drawing;
using System.Linq;
using System.Text;
//using System.Windows;
using System.Collections.ObjectModel;
using System.Windows;

namespace Syncfusion.Windows.Diagram
{
    /// <summary>
    /// Represents the Directed Tree layout used for the automatic arrangement of nodes. The tree is created based on the Layout root specified.
    /// </summary>
    public class RadialTreeLayout : TreeLayoutBase
    {
        #region Fields

        internal List<Node> layoutNodes = new List<Node>();

        private double leastX;

        private double leastY;

        private Node determindedLayoutRoot;

        /// <summary>
        /// Used to store the page.
        /// </summary>
        //private DiagramPage diagrampage;

        /// <summary>
        /// Used to store the default offset value
        /// </summary>
        private float moffsetValue = 50F;

        /// <summary>
        /// Used to store the anchor point
        /// </summary>
        //private PointF mAnchorPoint;

        /// <summary>
        /// Used to store the temporary anchor point
        /// </summary>
        //private PointF m_tempAnchorPoint;

        /// <summary>
        /// Used to store the depths
        /// </summary>
        private float[] m_depths = new float[10];

        /// <summary>
        /// Used to store the maximum depth
        /// </summary>
        //private int mMaxDepth = 0;

        /// <summary>
        /// Used to store the equalities.
        /// </summary>
        //private Dictionary<string, LayoutInfo> equalities;

        /// <summary>
        /// Used to store the anchor x value.
        /// </summary>
        //private float m_anchorX;

        /// <summary>
        /// Used to store the anchor y value.
        /// </summary>
        //private float m_anchorY;

        /// <summary>
        /// Used to store the units.
        /// </summary>
        private static MeasureUnits units;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the units.
        /// </summary>
        /// <value>The units.</value>
        internal static MeasureUnits Units
        {
            get { return units; }
            set { units = value; }
        }

        /// <summary>
        /// Gets or sets the root node offset.
        /// </summary>
        /// <value>The root node offset.</value>
        internal float RootNodeOffset
        {
            get { return moffsetValue; }
            set { moffsetValue = value; }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectedTreeLayout"/> class.
        /// </summary>
        /// <param name="model">The model instance.</param>
        public RadialTreeLayout(DiagramModel model)
            : base(model)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectedTreeLayout"/> class.
        /// </summary>
        /// <param name="model">The model object.</param>
        /// <param name="view">The view object.</param>
        public RadialTreeLayout(DiagramModel model, DiagramView view)
            : base(model, view)
        {
        }

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Occurs when [property changed].
        /// </summary>
        //public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raised when the appropriate property changes.
        /// </summary>
        /// <param name="name">The property name.</param>
        //protected void OnPropertyChanged(string name)
        //{
        //    PropertyChangedEventHandler handler = PropertyChanged;
        //    if (handler != null)
        //    {
        //        handler(this, new PropertyChangedEventArgs(name));
        //    }
        //}
        #endregion

        #region Methods

        private struct Level
        {
            internal ObservableCollection<Node> Nodes;
            internal double min;
            internal double max;
            internal double Height;
            internal ObservableCollection<double> Ratio;
            internal double Circumference;
            internal double radius;
        }

        private System.Windows.Point leftLast = new System.Windows.Point(0, 0);

        private bool IsValidLayout
        {
            get
            {
                if (LayoutRoot != null)
                {
                    determindedLayoutRoot = LayoutRoot as Node;
                    if (this.Model != null)
                    {
                        int maxParents = (from Node n in Model.Nodes
                                          select n.RParents.Count).Max();
                        if (maxParents < 2)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        private ObservableCollection<Level> Levels = new ObservableCollection<Level>();

        /// <summary>
        /// Refreshes the layout.
        /// </summary>
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

            PrepareActivity(this);
            UpdateInOut();
            UpdateRParentsChildrens();

            if (IsValidLayout)
            {
                AssignParent();
                layoutNodes.Clear();
                clearVisited();
                UpdateStages(determindedLayoutRoot);
                ClearUnwantedStages();
                DepthFirstAllignment(determindedLayoutRoot);

                PopulateLevels();
                TransformToCircleLayout();
                SetToBounds();
                MoveOtherNodesAway();
                (View.Page as DiagramPage).InvalidateMeasure();
                (View.Page as DiagramPage).InvalidateArrange();
            }
        }

        private void ClearUnwantedStages()
        {
            foreach (Node n in Model.Nodes)
            {
                if (!layoutNodes.Contains(n))
                {
                    n.Stage = -1;
                }
            }
        }

        private void MoveOtherNodesAway()
        {
//#if WPF
//            double x = (from Node n in layoutNodes
//                        select n.LogicalOffsetX).Min();
//            double y = (from Node n in layoutNodes
//                        select n.LogicalOffsetY).Max() + MeasureUnitsConverter.FromPixels(100, (View.Page as DiagramPage).MeasurementUnits);
//#endif
//#if SILVERLIGHT
            double x = (from Node n in layoutNodes
                        select n.PxOffsetX).Min();
            double y = (from Node n in layoutNodes
                        select n.PxOffsetY).Max() + 100;
//#endif
            foreach (Node n in Model.Nodes)
            {
                if (!layoutNodes.Contains(n))
                {
//#if WPF
//                    n.LogicalOffsetX = x;
//                    n.LogicalOffsetY = y;
//                    x += MeasureUnitsConverter.FromPixels(50 + n.Width , (View.Page as DiagramPage).MeasurementUnits);
//#endif
//#if SILVERLIGHT
                    n.PxOffsetX = x;
                    n.PxOffsetY = y;
                    x += 50 + n._Width;
//#endif
                }
            }
        }

        private void SetToBounds()
        {
#if WPF
            if (View != null && View.PxBounds.Equals(new Thickness(0, 0, 0, 0)) && View.Scrollviewer != null)
#endif
#if SILVERLIGHT
                if (View != null && View.PxBounds.Equals(new Thickness(0, 0, 0, 0)) && View.Scrollviewer != null)
#endif

            {
                this.Bounds = new Thickness(0, 0, View.Scrollviewer.ActualWidth, View.Scrollviewer.ActualHeight);
                this.Center = new Point((float)View.Scrollviewer.ActualWidth / 2, (float)View.Scrollviewer.ActualHeight / 2);
            }

            leastX = -(this.Bounds.Right / 2);
            leastY = -(this.Bounds.Bottom / 2);

            for (int i = 0; i < Levels.Count; i++)
            {
                for (int j = 0; j < Levels[i].Nodes.Count; j++)
                {
//#if WPF
//                    Levels[i].Nodes[j].LogicalOffsetX = MeasureUnitsConverter.FromPixels(Levels[i].Nodes[j].tempx - leastX, (View.Page as DiagramPage).MeasurementUnits);
//                    Levels[i].Nodes[j].LogicalOffsetY = MeasureUnitsConverter.FromPixels(Levels[i].Nodes[j].tempy - leastY, (View.Page as DiagramPage).MeasurementUnits);
//#endif
//#if SILVERLIGHT
                    Levels[i].Nodes[j].PxOffsetX = Levels[i].Nodes[j].tempx - leastX;
                    Levels[i].Nodes[j].PxOffsetY = Levels[i].Nodes[j].tempy - leastY;
//#endif
                }
            }
        }

        private void AssignParent()
        {
            foreach (Node n in Model.Nodes)
            {
                if (n.RParents.Count > 0)
                {
                    n.RParent = n.RParents[0];
                }
            }
        }

        private void TransformToCircleLayout()
        {
            determindedLayoutRoot.tempx = 0;
            determindedLayoutRoot.tempy = 0;
            determindedLayoutRoot.tempx -= determindedLayoutRoot._Width / 2;
            determindedLayoutRoot.tempy -= determindedLayoutRoot._Height / 2;
            for (int i = 1; i < Levels.Count; i++)
            {
                for (int j = 0; j < Levels[i].Nodes.Count; j++)
                {
//#if WPF
//                    Levels[i].Nodes[j].tempx = Math.Cos(Levels[i].Ratio[j] * 360 * Math.PI / 180) * (Levels[i].radius
//                                                        + MeasureUnitsConverter.ToPixels(Model.VerticalSpacing, (View.Page as DiagramPage).MeasurementUnits) * i);
//                    Levels[i].Nodes[j].tempy = Math.Sin(Levels[i].Ratio[j] * 360 * Math.PI / 180) * (Levels[i].radius
//                                                        + MeasureUnitsConverter.ToPixels(Model.VerticalSpacing, (View.Page as DiagramPage).MeasurementUnits) * i);
//#endif
//#if SILVERLIGHT
                    Levels[i].Nodes[j].tempx = Math.Cos(Levels[i].Ratio[j] * 360 * Math.PI / 180) * (Levels[i].radius
                                                        + Model.PxVerticalSpacing * i);
                    Levels[i].Nodes[j].tempy = Math.Sin(Levels[i].Ratio[j] * 360 * Math.PI / 180) * (Levels[i].radius
                                                        + Model.PxVerticalSpacing * i);
//#endif


                    Levels[i].Nodes[j].tempx -= Levels[i].Nodes[j]._Width / 2;
                    Levels[i].Nodes[j].tempy -= Levels[i].Nodes[j]._Height / 2;

                    leastX = Math.Min(leastX, Levels[i].Nodes[j].tempx);
                    leastY = Math.Min(leastY, Levels[i].Nodes[j].tempy);

                }
            }
        }

        private void DepthFirstAllignment(Node parent)
        {
            if (parent != null)
            {
                if (parent.RChildrens.Count > 0)
                {
                    leftLast.Y = leftLast.Y + 300;
                    foreach (Node n in parent.RChildrens)
                    {
                        DepthFirstAllignment(n);
                    }

                    double min = (from Node n in parent.RChildrens
                                  select n.tempx).Min();
                    double max = (from Node n in parent.RChildrens
                                  select n.tempx
                                  + n._Width
                                  ).Max();
                    parent.tempx = min + (max - min) / 2;
//#if WPF
//                    parent.segmentoffset = max + MeasureUnitsConverter.ToPixels(Model.HorizontalSpacing, (View.Page as DiagramPage).MeasurementUnits);
//                    leftLast.X = max + MeasureUnitsConverter.ToPixels(Model.HorizontalSpacing, (View.Page as DiagramPage).MeasurementUnits);
//#endif
//#if SILVERLIGHT
                    parent.segmentoffset = max + Model.PxHorizontalSpacing;
                    leftLast.X = max + Model.PxHorizontalSpacing;
//#endif
                    parent.tempx -= parent._Width / 2;
                    parent.tempy -= parent._Height / 2;

                    if (parent.tempx < min && parent.subTreeReVal)
                    {
                        parent.tempx = min;
                        leftLast.X = parent.tempx + parent._Width / 2 - (max - min) / 2;
                        parent.subTreeReVal = false;
                        DepthFirstAllignment(parent);
                        parent.subTreeReVal = true;
//#if WPF
//                        leftLast.X = parent.tempx
//                            + parent.Width
//                            + MeasureUnitsConverter.ToPixels(Model.HorizontalSpacing, (View.Page as DiagramPage).MeasurementUnits);
//#endif
//#if SILVERLIGHT
                        leftLast.X = parent.tempx
                            + parent._Width
                            + Model.PxHorizontalSpacing;
//#endif
                    }
                    max = (from Node n in parent.RChildrens
                           select n.segmentoffset
                                  ).Max();
                    leftLast.X = leftLast.X < max ? max : leftLast.X;
                    leftLast.Y = leftLast.Y - 300;
                    parent.tempy = leftLast.Y;
                }
                else
                {
//#if WPF
//                    parent.tempx = leftLast.X;
//                    parent.tempy = leftLast.Y;
//                    leftLast.X = leftLast.X
//                            + parent.Width
//                            + MeasureUnitsConverter.ToPixels(Model.HorizontalSpacing, (View.Page as DiagramPage).MeasurementUnits);
//#endif
//#if SILVERLIGHT
                    parent.tempx = leftLast.X;
                    parent.tempy = leftLast.Y;
                    leftLast.X = leftLast.X
                            + parent._Width
                            + Model.PxHorizontalSpacing;
//#endif
                }
            }
        }

        private void PopulateLevels()
        {
            double min = (from Node n in layoutNodes
                          select n.tempx).Min();
//#if WPF
//            double max = (from Node n in layoutNodes
//                          select n.tempx
//                            + n.Width
//                            + MeasureUnitsConverter.ToPixels(Model.HorizontalSpacing, (View.Page as DiagramPage).MeasurementUnits)).Max();
//#endif
//#if SILVERLIGHT
            double max = (from Node n in layoutNodes
                          select n.tempx
                            + n._Width
                            +Model.PxHorizontalSpacing).Max();
//#endif
            double full = max - min;

            List<IGrouping<Int32, Node>> StageGrouped = (from Node n in layoutNodes
                                                         orderby n.Stage
                                                         where n.Stage >= 0
                                                         group n by n.Stage into grouping
                                                         select grouping).ToList();

            Levels.Clear();

            foreach (IGrouping<Int32, Node> Stages in StageGrouped)
            {
                Level newlevel = new Level();
                List<Node> ordered = (from Node n in Stages
                                      orderby n.tempx
                                      select n as Node).ToList();
                newlevel.Nodes = new ObservableCollection<Node>();
                newlevel.Ratio = new ObservableCollection<double>();
                double x1 = (from Node n in Stages
                             select n.tempx).Min();
//#if WPF
//                double x2 = (from Node n in Stages
//                             select n.tempx
//                             + n.Width
//                             + MeasureUnitsConverter.ToPixels(Model.HorizontalSpacing, (View.Page as DiagramPage).MeasurementUnits)
//                             ).Max();
//#endif
//#if SILVERLIGHT
                double x2 = (from Node n in Stages
                             select n.tempx
                             + n._Width
                             + Model.PxHorizontalSpacing
                             ).Max();
//#endif

                newlevel.Height = (from Node n in Stages
                                   select n._Height).Max();
                newlevel.Circumference = x2 - x1;
                newlevel.min = x1;
                newlevel.max = x2;

                newlevel.radius = newlevel.Circumference / (2 * Math.PI) + newlevel.Height;
                foreach (Node Stage in Stages)
                {
                    newlevel.Nodes.Add(Stage);
                    newlevel.Ratio.Add((Math.Abs(Stage.tempx + Stage._Width / 2 - min) / full));
                }

                if (Stages.Key > 1)
                {
                    if (Levels[Stages.Key - 1].radius + Levels[Stages.Key - 1].Height >= newlevel.radius)
                    {
                        newlevel.radius = Levels[Stages.Key - 1].radius + Levels[Stages.Key - 1].Height;
                    }
                }

                Levels.Add(newlevel);
            }
        }

        private void UpdateStages(Node parent)
        {
            if (parent != null)
            {
                layoutNodes.Add(parent);
                parent.Visited = true;
                foreach (Node child in parent.RChildrens)
                {
                    if (!child.Visited && child.RParent.Equals(parent))
                    {
                        child.Stage = parent.Stage + 1;
                    }
                }
                foreach (Node child in parent.RChildrens)
                {
                    if (!child.Visited && child.RParent.Equals(parent))
                    {
                        UpdateStages(child);
                    }
                }
            }
        }

        private void UpdateRParentsChildrens()
        {
            if (Model != null)
            {
                foreach (Node n in Model.Nodes)
                {
                    n.RParents.Clear();
                    n.RChildrens.Clear();
                }
                foreach (Node n in Model.Nodes)
                {
                    if (n.In != null)
                    {
                        foreach (LineConnector ln in n.In)
                        {
                            n.RParents.Add(ln.HeadNode as Node);
                        }
                    }
                    if (n.Out != null)
                    {
                        foreach (LineConnector ln in n.Out)
                        {
                            n.RChildrens.Add(ln.TailNode as Node);
                        }
                    }
                }
            }
        }

        private void UpdateInOut()
        {
            if (Model != null)
            {
                foreach (Node n in Model.Nodes)
                {
                    n.In.Clear();
                    n.Out.Clear();
                    n.RParents.Clear();
                    n.RChildrens.Clear();

                }
                foreach (LineConnector ln in Model.Connections)
                {
                    if (ln.HeadNode != null)
                    {
                        (ln.HeadNode as Node).Out.Add(ln);
                    }
                    if (ln.TailNode != null)
                    {
                        (ln.TailNode as Node).In.Add(ln);
                    }
                }
            }
        }

        private void clearVisited()
        {
            foreach (Node n in Model.Nodes)
            {
                n.Visited = false;
            }
        }

        /// <summary>
        /// Prepares for the activity
        /// </summary>
        /// <param name="layout">ILayout instance.</param>
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
        ///        DirectedTreeLayout tree = new DirectedTreeLayout();
        ///        tree.PrepareActivity(tree);
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        internal void PrepareActivity(ILayout layout)
        {
            layout.Model = Model;

//#if WPF
//            layout.Bounds = MeasureUnitsConverter.ToPixels(View.Bounds, (View.Page as DiagramPage).MeasurementUnits);
//            layout.Center = new System.Windows.Point((float)View.Bounds.Right / 2, (float)View.Bounds.Bottom / 2);
//            if (View != null && View.Bounds.Equals(new System.Windows.Thickness(0, 0, 0, 0)) && View.Scrollviewer != null)
//#endif
//#if SILVERLIGHT
            layout.Bounds =View.PxBounds;
            layout.Center = new System.Windows.Point((float)View.PxBounds.Right / 2, (float)View.PxBounds.Bottom / 2);
            if (View != null && View.PxBounds.Equals(new System.Windows.Thickness(0, 0, 0, 0)) && View.Scrollviewer != null)
//#endif

            {
                layout.Bounds = new System.Windows.Thickness(0, 0, View.Scrollviewer.ActualWidth, View.Scrollviewer.ActualHeight);
                layout.Center = new System.Windows.Point((float)View.Scrollviewer.ActualWidth, (float)View.Scrollviewer.ActualHeight);
            }

            if (Model != null)
            {
                LayoutRoot = Model.LayoutRoot;
            }
        }

        #endregion
    }
}
