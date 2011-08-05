// <copyright file="Group.cs" company="Syncfusion">
// Copyright Syncfusion Inc. 2001 - 2011. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;

using System.Windows.Media;
using System.Collections;
using System.Windows.Controls.Primitives;

namespace Syncfusion.Windows.Diagram
{
    /// <summary>
    /// Represents the Group class which enables grouping of the node.
    /// </summary>
    /// Groups enable to combine two or more objects so that the same operation gets performed on all the children of the group. 
    /// Group is essentially just another node added which acts as a container for other objects.
    /// <example>
    /// <para/>The following example shows how to create a <see cref="Group"/> in C# and add nodes to it..
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
    ///        n.Level = 1;
    ///        n.OffsetX = 150;
    ///        n.OffsetY = 25;
    ///        n.Width = 150;
    ///        n.Height = 75;
    ///        Node n1 = new Node(Guid.NewGuid(), "End");
    ///        n1.Shape = Shapes.FlowChart_Start;
    ///        n1.Level = 1;
    ///        n1.OffsetX = 350;
    ///        n1.OffsetY = 325;
    ///        n1.Width = 100;
    ///        n1.Height = 75;
    ///        Model.Nodes.Add(n);
    ///        Model.Nodes.Add(n1);
    ///        Group g = new Group(Guid.NewGuid(), "group1");
    ///        g.AddChild(n);
    ///        g.AddChild(n1);
    ///        diagramModel.Nodes.Add(g);
    ///    }
    ///    }
    ///    }
    /// </code>
    /// <para/> Groups can also be added using the Group Command as follows in C#:
    /// <code language="C#">
    ///  DiagramCommandManager.Group.Execute(diagramView.Page, diagramView);
    /// </code>
    /// <para/> Groups can be removed using the Ungroup Command as follows in C#:
    /// <code language="C#">
    ///  DiagramCommandManager.Ungroup.Execute(diagramView.Page, diagramView);
    /// </code>
    /// </example>
    /// <seealso cref="Node"/>
    /// <seealso cref="LineConnector"/>
    /// <seealso cref="DiagramCommandManager"/>
#if SyncfusionFramework4_0
    [DesignTimeVisible(false)]
#endif
    public class Group : Node, INodeGroup
    {
        #region Class Fields

        /// <summary>
        /// Used to store the Group Guid.
        /// </summary>
        private Guid m_id;

        /// <summary>
        /// Used to store the group children.
        /// </summary>
        private CollectionExt m_childs = new CollectionExt();

        /// <summary>
        /// Used to store the group children reference number.
        /// </summary>
        private CollectionExt gchild = new CollectionExt();

        /// <summary>
        /// Used to store the <see cref="DiagramView"/> instance.
        /// </summary>
        //internal DiagramView dview;

        #endregion

                #region Initialization

        /// <summary>
        /// Initializes static members of the <see cref="Group"/> class.
        /// </summary>
        static Group()
        {
            
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Group"/> class.
        /// </summary>
        /// <param name="id">The Guid id.</param>
        /// <param name="name">The node name.</param>
        public Group(Guid id, string name)
        {
            this.m_id = id;
           // this.Name = name;
            this.LayoutUpdated += new EventHandler(Group_LayoutUpdated);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Group"/> class.
        /// </summary>
        /// <param name="id">The Guid id.</param>
        public Group(Guid id)
            : this(id, null)
        {
            this.m_id = id;
            this.LayoutUpdated += new EventHandler(Group_LayoutUpdated);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Group"/> class.
        /// </summary>
        public Group()
            : this(Guid.NewGuid())
        {
            this.DefaultStyleKey = typeof(Group);
            this.LayoutUpdated += new EventHandler(Group_LayoutUpdated);
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Handles the LayoutUpdated event of the Group control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Group_LayoutUpdated(object sender, EventArgs e)
        {
            if (this.GetLogicalOffsetX() != -1 && dview != null)
            {
                this.PxLogicalOffsetX = this.GetLogicalOffsetX();
                this.PxLogicalOffsetY = this.GetLogicalOffsetY();
                this.Width = this.GetRightmostposition() - this.GetLogicalOffsetX();// MeasureUnitsConverter.ToPixels(this.GetLogicalOffsetX(), (dview.Page as DiagramPage).MeasurementUnits);
                this.Height = this.GetBottommostposition() - this.GetLogicalOffsetY();// MeasureUnitsConverter.ToPixels(this.GetLogicalOffsetY(), (dview.Page as DiagramPage).MeasurementUnits);
            }
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate"/>.
        /// </summary>
        public override void OnApplyTemplate()
        {
            dview = Node.GetDiagramView(this);

            if (dview != null && !dview.InternalGroups.Contains(this))
            {
                dview.InternalGroups.Add(this);
            }
            //this.Loaded += new RoutedEventHandler(Group_Loaded);
            //this.Background = new SolidColorBrush(Colors.Black);
            base.OnApplyTemplate();
        }

       

        //internal ChildDataParser parser;


     
   
        private void HookConnection(Node parent, CollectionExt childs)
        {
            this.dview.Model.Nodes.Add(parent);

        }       

        /// <summary>
        /// Calculates the OffsetX position for the node.
        /// </summary>
        /// <returns>The OffsetX value</returns>
        private double GetLogicalOffsetX()
        {
            Point p = new Point(0, 0);
            double left = 0;
            if (this.NodeChildren.Count != 0)
            {
                if (this.NodeChildren[0] is Node || (this.NodeChildren[0] is Group))
                {
                    left = (this.NodeChildren[0] as Node).PxLogicalOffsetX;
                }
                else
                {
                    left = Math.Min((this.NodeChildren[0] as LineConnector).PxStartPointPosition.X, (this.NodeChildren[0] as LineConnector).PxEndPointPosition.X);
                    //left = Math.Min((this.NodeChildren[0] as LineConnector).minx, (this.NodeChildren[0] as LineConnector).minx);
                }

                for (int i = 0; i < this.NodeChildren.Count; i++)
                {
                    if (this.NodeChildren[i] is Node || (this.NodeChildren[i] is Group))
                    {
                        if ((this.NodeChildren[i] as Node).PxLogicalOffsetX < left)
                        {
                            left = (this.NodeChildren[i] as Node).PxLogicalOffsetX;
                        }
                    }
                    else
                    {
                        if (this.NodeChildren[i] is LineConnector)
                        {
                            if ((this.NodeChildren[i] as LineConnector).PxStartPointPosition.X < (this.NodeChildren[i] as LineConnector).PxEndPointPosition.X && (this.NodeChildren[i] as LineConnector).PxStartPointPosition != p)
                            //if ((this.NodeChildren[i] as LineConnector).minx < (this.NodeChildren[i] as LineConnector).minx && (this.NodeChildren[i] as LineConnector).StartPointPosition != p)
                            {
                                if ((this.NodeChildren[i] as LineConnector).PxStartPointPosition.X < left)
                                //if ((this.NodeChildren[i] as LineConnector).minx < left)
                                {
                                    left = (this.NodeChildren[i] as LineConnector).PxStartPointPosition.X;
                                }
                            }
                            else
                                if ((this.NodeChildren[i] as LineConnector).PxEndPointPosition.X < left && (this.NodeChildren[i] as LineConnector).PxEndPointPosition != p)
                                //if ((this.NodeChildren[i] as LineConnector).minx < left && (this.NodeChildren[i] as LineConnector).EndPointPosition != p)
                                {
                                    left = (this.NodeChildren[i] as LineConnector).PxEndPointPosition.X;
                                    //left = (this.NodeChildren[i] as LineConnector).minx;
                                }
                        }
                    }
                }

                return left;
            }

            return -1;
        }

        /// <summary>
        /// Calculates the OffsetY position for the node.
        /// </summary>
        /// <returns>The OffsetY value.</returns>
        private double GetLogicalOffsetY()
        {
            Point p = new Point(0, 0);
            double top = 0;
            if (this.NodeChildren.Count != 0)
            {
                if (this.NodeChildren[0] is Node || (this.NodeChildren[0] is Group))
                {
                    top = (this.NodeChildren[0] as Node).PxLogicalOffsetY;
                }
                else
                {
                    top = Math.Min((this.NodeChildren[0] as LineConnector).PxStartPointPosition.Y, (this.NodeChildren[0] as LineConnector).PxEndPointPosition.Y);
                    //top = Math.Min((this.NodeChildren[0] as LineConnector).miny, (this.NodeChildren[0] as LineConnector).miny);
                }

                for (int i = 0; i < this.NodeChildren.Count; i++)
                {
                    if (this.NodeChildren[i] is Node || (this.NodeChildren[i] is Group))
                    {
                        if ((this.NodeChildren[i] as Node).PxLogicalOffsetY < top)
                        {
                            top = (this.NodeChildren[i] as Node).PxLogicalOffsetY;
                        }
                    }
                    else
                    {
                        if (this.NodeChildren[i] is LineConnector)
                        {
                            if ((this.NodeChildren[i] as LineConnector).PxStartPointPosition.Y < (this.NodeChildren[i] as LineConnector).PxEndPointPosition.Y && (this.NodeChildren[i] as LineConnector).PxStartPointPosition != p)
                            //if ((this.NodeChildren[i] as LineConnector).miny < (this.NodeChildren[i] as LineConnector).miny && (this.NodeChildren[i] as LineConnector).StartPointPosition != p)
                            {
                                if ((this.NodeChildren[i] as LineConnector).PxStartPointPosition.Y < top)
                                //if ((this.NodeChildren[i] as LineConnector).miny < top)
                                {
                                    top = (this.NodeChildren[i] as LineConnector).PxStartPointPosition.Y;
                                    //top = (this.NodeChildren[i] as LineConnector).miny;
                                }
                            }
                            else
                                if ((this.NodeChildren[i] as LineConnector).PxEndPointPosition.Y < top && (this.NodeChildren[i] as LineConnector).PxEndPointPosition != p)
                                //if ((this.NodeChildren[i] as LineConnector).miny < top && (this.NodeChildren[i] as LineConnector).EndPointPosition != p)
                                {
                                    top = (this.NodeChildren[i] as LineConnector).PxEndPointPosition.Y;
                                    //top = (this.NodeChildren[i] as LineConnector).miny;
                                }
                        }
                    }
                }

                return top;
            }

            return -1;
        }

        /// <summary>
        /// Gets the rightmost position  of the group.
        /// </summary>
        /// <returns>Double value representing the rightmost position.</returns>
        internal double GetRightmostposition()
        {
            double rightmost = 0;
            if (this.NodeChildren.Count != 0)
            {
                if (this.NodeChildren[0] is Node || (this.NodeChildren[0] is Group))
                {
                    double c = (this.NodeChildren[0] as Node).PxLogicalOffsetX;// MeasureUnitsConverter.ToPixels((this.NodeChildren[0] as Node).LogicalOffsetX, (dview.Page as DiagramPage).MeasurementUnits);
                    rightmost = c + (this.NodeChildren[0] as Node).Width;
                }
                else
                {
                    rightmost = Math.Max((this.NodeChildren[0] as LineConnector).PxStartPointPosition.X, (this.NodeChildren[0] as LineConnector).PxEndPointPosition.X);
                }

                for (int i = 0; i < this.NodeChildren.Count; i++)
                {
                    if (this.NodeChildren[i] is Node || (this.NodeChildren[i] is Group))
                    {
                        double x = (this.NodeChildren[i] as Node).PxLogicalOffsetX;// MeasureUnitsConverter.ToPixels((this.NodeChildren[i] as Node).LogicalOffsetX, (dview.Page as DiagramPage).MeasurementUnits);
                        if (x + (this.NodeChildren[i] as Node).Width > rightmost)
                        {
                            rightmost = x + (this.NodeChildren[i] as Node).Width;
                        }
                    }
                    else
                    {
                        if (this.NodeChildren[i] is LineConnector)
                        {
                            if ((this.NodeChildren[i] as LineConnector).PxStartPointPosition.X > (this.NodeChildren[i] as LineConnector).PxEndPointPosition.X)
                            {
                                if ((this.NodeChildren[i] as LineConnector).PxStartPointPosition.X > rightmost)
                                {
                                    rightmost = (this.NodeChildren[i] as LineConnector).PxStartPointPosition.X;
                                }
                            }
                            else
                                if ((this.NodeChildren[i] as LineConnector).PxEndPointPosition.X > rightmost)
                                {
                                    rightmost = (this.NodeChildren[i] as LineConnector).PxEndPointPosition.X;
                                }
                        }
                        //if (this.NodeChildren[i] is LineConnector)
                        //{
                        //    if ((this.NodeChildren[i] as LineConnector).minx > (this.NodeChildren[i] as LineConnector).maxx)
                        //    {
                        //        if ((this.NodeChildren[i] as LineConnector).minx > rightmost)
                        //        {
                        //            rightmost = (this.NodeChildren[i] as LineConnector).minx;
                        //        }
                        //    }
                        //    else
                        //        if ((this.NodeChildren[i] as LineConnector).maxx > rightmost)
                        //        {
                        //            rightmost = (this.NodeChildren[i] as LineConnector).maxx;
                        //        }
                        //}
                    }
                }

                return rightmost;
            }

            return -1;
        }

        /// <summary>
        /// Gets the bottommost position  of the group.
        /// </summary>
        /// <returns>Double value representing the rightmost position.</returns>
        internal double GetBottommostposition()
        {
            double bottommost = 0;
            if (this.NodeChildren.Count != 0)
            {
                if (this.NodeChildren[0] is Node || (this.NodeChildren[0] is Group))
                {
                    double c = (this.NodeChildren[0] as Node).PxLogicalOffsetY;// MeasureUnitsConverter.ToPixels((this.NodeChildren[0] as Node).LogicalOffsetY, (dview.Page as DiagramPage).MeasurementUnits);
                    bottommost = c + (this.NodeChildren[0] as Node).Height;
                }
                else
                {
                    bottommost = Math.Max((this.NodeChildren[0] as LineConnector).PxStartPointPosition.Y, (this.NodeChildren[0] as LineConnector).PxEndPointPosition.Y);
                }

                for (int i = 0; i < this.NodeChildren.Count; i++)
                {
                    if (this.NodeChildren[i] is Node || (this.NodeChildren[i] is Group))
                    {
                        double c1 = (this.NodeChildren[i] as Node).PxLogicalOffsetY;// MeasureUnitsConverter.ToPixels((this.NodeChildren[i] as Node).LogicalOffsetY, (dview.Page as DiagramPage).MeasurementUnits);
                        if (c1 + (this.NodeChildren[i] as Node).Height > bottommost)
                        {
                            bottommost = c1 + (this.NodeChildren[i] as Node).Height;
                        }
                    }
                    else
                    {
                        if (this.NodeChildren[i] is LineConnector)
                        {
                            if ((this.NodeChildren[i] as LineConnector).PxStartPointPosition.Y > (this.NodeChildren[i] as LineConnector).PxEndPointPosition.Y)
                            {
                                if ((this.NodeChildren[i] as LineConnector).PxStartPointPosition.Y > bottommost)
                                {
                                    bottommost = (this.NodeChildren[i] as LineConnector).PxStartPointPosition.Y;
                                }
                            }
                            else
                                if ((this.NodeChildren[i] as LineConnector).PxEndPointPosition.Y > bottommost)
                                {
                                    bottommost = (this.NodeChildren[i] as LineConnector).PxEndPointPosition.Y;
                                }
                            
                            //if ((this.NodeChildren[i] as LineConnector).miny > (this.NodeChildren[i] as LineConnector).maxy)
                            //{
                            //    if ((this.NodeChildren[i] as LineConnector).miny > bottommost)
                            //    {
                            //        bottommost = (this.NodeChildren[i] as LineConnector).miny;
                            //    }
                            //}
                            //else
                            //    if ((this.NodeChildren[i] as LineConnector).maxy > bottommost)
                            //    {
                            //        bottommost = (this.NodeChildren[i] as LineConnector).maxy;
                            //    }
                        }
                    }
                }

                return bottommost;
            }

            return -1;
        }

        /// <summary>
        /// Adds a child to the group.
        /// </summary>
        /// <param name="child">The child to be added to the group.</param>
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
        /// n.Level = 1;
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// Node n1 = new Node(Guid.NewGuid(), "End");
        /// n1.Shape = Shapes.FlowChart_Start;
        /// n1.Level = 1;
        /// n1.OffsetX = 350;
        /// n1.OffsetY = 325;
        /// n1.Width = 100;
        /// n1.Height = 75;
        /// Model.Nodes.Add(n);
        /// Model.Nodes.Add(n1);
        /// Group g = new Group(Guid.NewGuid(), "group1");
        /// g.AddChild(n);
        /// g.AddChild(n1);
        /// diagramModel.Nodes.Add(g);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        public void AddChild(INodeGroup child)
        {
            if (child is Group)
            {
                foreach (INodeGroup n in (child as Group).NodeChildren)
                {
                    if (!n.Groups.Contains(this))
                    {
                        n.Groups.Add(this);
                    }

                    if (!this.NodeChildren.Contains(n))
                    {
                        this.NodeChildren.Add(n);
                    }

                    GroupCheck(n);
                }
            }

            if (!child.Groups.Contains(this))
            {
                child.Groups.Add(this);
            }

            child.IsGrouped = true;
            if (!this.NodeChildren.Contains(child))
            {
                this.NodeChildren.Add(child);
            }
        }

        /// <summary>
        /// Checks if the child is a group
        /// </summary>
        /// <param name="node">The INodeGroup object to check.</param>
        private void GroupCheck(INodeGroup node)
        {
            if (node is Group)
            {
                foreach (INodeGroup n in (node as Group).NodeChildren)
                {
                    if (!n.Groups.Contains(this))
                    {
                        n.Groups.Add(this);
                    }

                    if (!this.NodeChildren.Contains(n))
                    {
                        this.NodeChildren.Add(n);
                    }
                }
            }
        }

        /// <summary>
        /// Removes a child from the group.
        /// </summary>
        /// <param name="child">The child to be removed from the group.</param>
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
        ///        n.Level = 1;
        ///        n.OffsetX = 150;
        ///        n.OffsetY = 25;
        ///        n.Width = 150;
        ///        n.Height = 75;
        ///        Node n1 = new Node(Guid.NewGuid(), "End");
        ///        n1.Shape = Shapes.FlowChart_Start;
        ///        n1.Level = 1;
        ///        n1.OffsetX = 350;
        ///        n1.OffsetY = 325;
        ///        n1.Width = 100;
        ///        n1.Height = 75;
        ///        Model.Nodes.Add(n);
        ///        Model.Nodes.Add(n1);
        ///        Group g = new Group(Guid.NewGuid(), "group1");
        ///        g.AddChild(n);
        ///        g.AddChild(n1);
        ///        diagramModel.Nodes.Add(g);
        ///        g.RemoveChild(n);
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        public void RemoveChild(Node child)
        {
            this.NodeChildren.Remove(child);
            child.Groups.Remove(this);
            if (child.Groups.Count == 0)
            {
                child.IsGrouped = false;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the group children.
        /// </summary>
        /// <value>The group children.</value>
        public CollectionExt NodeChildren
        {
            get
            {
                return m_childs;
            }
        }

        /// <summary>
        /// Gets or sets the group children reference number. Used for serialization purposes.
        /// </summary>
        /// <value>The group children ref.</value>
        public CollectionExt GroupChildrenRef
        {
            get
            {
                return gchild;
            }

            set
            {
                gchild = value;
            }
        }

        /// <summary>
        /// Gets the unique identifier of this node.
        /// </summary>
        /// <value>The unique identifier value.</value>
        public new Guid ID
        {
            get { return m_id; }
        }

        #endregion
    }



}