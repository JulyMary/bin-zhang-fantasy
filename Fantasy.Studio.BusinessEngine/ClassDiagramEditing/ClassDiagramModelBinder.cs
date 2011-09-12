using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sf = Syncfusion.Windows.Diagram;
using System.ComponentModel.Design;
using Syncfusion.Windows.Diagram;
using System.Windows.Media;
using System.Windows;
using System.Collections.Specialized;
using System.Windows.Documents;


namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing
{
    public class ClassDiagramModelBinder : IDisposable
    {

        private Model.ClassDiagram _model;
        private sf.DiagramModel _view;

        public ClassDiagramModelBinder(Model.ClassDiagram model, sf.DiagramModel view)
        {
            this._model = model;
            this._view = view;


            foreach (Model.ClassGlyph node in this._model.Classes)
            {
                this.AddNode(node);

            }

            foreach (Model.InheritanceGlyph inheritance in this._model.Inheritances)
            {
                this.AddInheritance(inheritance);
            }

            foreach (Model.EnumGlyph @enum in this._model.Enums)
            {
                this.AddNode(@enum);
            }

            foreach (Model.AssociationGlyph association in this._model.Associations)
            {
                this.AddAssociation(association);
            }

            this._model.Classes.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Classes_CollectionChanged);
            this._model.Inheritances.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Inheritances_CollectionChanged);
            this._model.Enums.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Enums_CollectionChanged);
            this._model.Associations.CollectionChanged += new NotifyCollectionChangedEventHandler(Associations_CollectionChanged);


        }

        void Associations_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                if (e.OldItems != null)
                {
                    foreach (Model.AssociationGlyph node in e.OldItems)
                    {
                        this.RemoveConnector(node);
                    }
                }
                if (e.NewItems != null)
                {
                    foreach (Model.AssociationGlyph node in e.NewItems)
                    {
                        this.AddAssociation(node);
                    }
                }
            }
        }

      

       

        void Enums_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                if (e.OldItems != null)
                {
                    foreach (Model.EnumGlyph node in e.OldItems)
                    {
                        this.RemoveNode(node);
                    }
                }
                if (e.NewItems != null)
                {
                    foreach (Model.EnumGlyph node in e.NewItems)
                    {
                        this.AddNode(node);
                    }
                }
            }
        }

        private void AddNode(Model.EnumGlyph node)
        {
            Shapes.EnumNode shape = new Shapes.EnumNode(node.Id);

            shape.DataContext = node;

            this._view.Nodes.Add(shape);
        }

        private void RemoveNode(Model.EnumGlyph node)
        {
            Shapes.EnumNode shape = this._view.Nodes.FilterAndCast<Shapes.EnumNode>().SingleOrDefault(s => s.DataContext == node);
            if (shape != null)
            {
                this._view.Nodes.Remove(shape);
            }
        }


        private void AddAssociation(Model.AssociationGlyph association)
        {
            Shapes.ClassNode leftShape = this._view.Nodes.FilterAndCast<Shapes.ClassNode>().Single(n => n.DataContext == association.LeftClass);
            Shapes.ClassNode rightShape = this._view.Nodes.FilterAndCast<Shapes.ClassNode>().Single(n => n.DataContext == association.RightClass);

            LineConnector connector = new LineConnector()
            {
                IsHeadMovable = false,
                IsTailMovable = false,
                HeadNode = leftShape,
                TailNode = rightShape,
                HeadDecoratorShape = DecoratorShape.Diamond,
               
                TailDecoratorShape = DecoratorShape.Diamond,
                IsLabelEditable = false,
                
            };
            connector.TailDecoratorStyle.Fill = connector.HeadDecoratorStyle.Fill = Brushes.White;
           
            connector.LineStyle.StrokeDashArray = new DoubleCollection(new double[] { 3, 1 });

            if (association.IntermediatePoints.Count > 0)
            {
                connector.IntermediatePoints.Clear();
                foreach (Model.Point mp in association.IntermediatePoints)
                {
                    connector.IntermediatePoints.Add(new Point(mp.X, mp.Y));
                }
            }

            connector.DataContext = association;



            

            association.IntermediatePoints.CollectionChanged += new NotifyCollectionChangedEventHandler(IntermediatePoints_CollectionChanged);
            connector.ConnectorPathGeometryUpdated += new EventHandler<EventArgs>(ConnectorPathGeometryUpdated);
            this._view.Connections.Add(connector);

            Shapes.CardinalityAdorner lc = new Shapes.CardinalityAdorner(connector);
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(connector);
            adornerLayer.Add(lc);

        }


        private void AddInheritance(Model.InheritanceGlyph inheritance)
        {
            Shapes.ClassNode childShape = this._view.Nodes.FilterAndCast<Shapes.ClassNode>().Single(n => n.DataContext == inheritance.DerivedClass);
            Shapes.ClassNode parentShape = this._view.Nodes.FilterAndCast<Shapes.ClassNode>().Single(n => n.DataContext == inheritance.BaseClass);
            LineConnector connector = new LineConnector()
            {
                IsHeadMovable = false,
                IsTailMovable = false,
                TailNode = childShape,
                HeadNode = parentShape,
                ConnectorType = ConnectorType.Orthogonal,
                HeadDecoratorShape = DecoratorShape.Arrow,
                TailDecoratorShape = DecoratorShape.None,
                IsLabelEditable = false
           
            };

            // connector.LineStyle.Stroke = Brushes.Black;
            connector.LineStyle.StrokeThickness = 2;
            if (inheritance.IntermediatePoints.Count > 0)
            {
                connector.IntermediatePoints.Clear();
                foreach (Model.Point mp in inheritance.IntermediatePoints)
                {
                    connector.IntermediatePoints.Add(new Point(mp.X, mp.Y));
                }
            }

            connector.DataContext = inheritance;

            inheritance.IntermediatePoints.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(IntermediatePoints_CollectionChanged);
            connector.ConnectorPathGeometryUpdated += new EventHandler<EventArgs>(ConnectorPathGeometryUpdated);
            this._view.Connections.Add(connector);

        }

        void ConnectorPathGeometryUpdated(object sender, EventArgs e)
        {
            lock (_synRoot)
            {
                if (this._bindingConnector == false)
                {
                    this._bindingConnector = true;
                    try
                    {
                        LineConnector connector = (LineConnector)sender;
                        Model.IConnectGlyph glyph = (Model.IConnectGlyph)connector.DataContext;
                        glyph.IntermediatePoints.Clear();
                        foreach (Point p in connector.IntermediatePoints)
                        {
                            glyph.IntermediatePoints.Add(new Model.Point() { X = p.X, Y = p.Y });
                        }
                    }
                    finally
                    {
                        this._bindingConnector = false;
                    }
                }
            }
        }

        void IntermediatePoints_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            lock (_synRoot)
            {
                if (this._bindingConnector == false)
                {
                    this._bindingConnector = true;
                    try
                    {

                        Model.IConnectGlyph glyph = (Model.IConnectGlyph)sender;
                        LineConnector connector = this._view.Connections.Cast<LineConnector>().Single(c => c.DataContext == sender);

                        switch (e.Action)
                        {
                            case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                                {
                                    connector.IntermediatePoints.InsertRange(e.NewStartingIndex, e.NewItems.Cast<Model.Point>().Select(p => new Point(p.X, p.Y)));
                                }
                                break;
                            case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                                // Does not support
                                break;
                            case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                                {
                                    connector.IntermediatePoints.RemoveRange(e.OldStartingIndex, e.NewItems.Count);
                                }
                                break;
                            case
                            System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                                {
                                    for (int i = 0; i < e.OldItems.Count; i++)
                                    {
                                        Model.Point p = (Model.Point)e.NewItems[i];
                                        connector.IntermediatePoints[i + e.OldStartingIndex] = new Point(p.X, p.Y);
                                    }
                                }
                                break;
                            case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                                {
                                    connector.IntermediatePoints.Clear();
                                    connector.IntermediatePoints.AddRange(e.NewItems.Cast<Model.Point>().Select(p => new Point(p.X, p.Y)));
                                }
                                break;

                        }

                        connector.UpdateConnectorPathGeometry();

                    }
                    finally
                    {
                        this._bindingConnector = false;
                    }
                }
            }
        }



        private object _synRoot = new object();

        private bool _bindingConnector = false;


        void Inheritances_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                if (e.OldItems != null)
                {
                    foreach (Model.InheritanceGlyph iht in e.OldItems)
                    {
                        this.RemoveConnector(iht);
                    }
                }

                if (e.NewItems != null)
                {
                    foreach (Model.InheritanceGlyph iht in e.NewItems)
                    {
                        this.AddInheritance(iht);
                    }
                }
            }
        }

        private void RemoveConnector(Model.IConnectGlyph glyph)
        {
            glyph.IntermediatePoints.CollectionChanged -= new NotifyCollectionChangedEventHandler(IntermediatePoints_CollectionChanged);
            LineConnector connector = this._view.Connections.Cast<LineConnector>().Single(c => c.DataContext == glyph);
            connector.ConnectorPathGeometryUpdated -= new EventHandler<EventArgs>(ConnectorPathGeometryUpdated);
            DiagramControl dc = DiagramPage.GetDiagramControl(connector);
            dc.View.Islinedeleted = true;
            connector.IsSelected = false;
            ConnectionDeleteRoutedEventArgs lnewEventArgs = new ConnectionDeleteRoutedEventArgs(connector);
            lnewEventArgs.RoutedEvent = DiagramView.ConnectorDeletingEvent;
            dc.View.RaiseEvent(lnewEventArgs);
            this._view.Connections.Remove(connector);
            dc.View.Islinedeleted = false;
        }

        void Classes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

            if (e.Action != System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                if (e.OldItems != null)
                {
                    foreach (Model.ClassGlyph node in e.OldItems)
                    {
                        this.RemoveNode(node);
                    }
                }
                if (e.NewItems != null)
                {
                    foreach (Model.ClassGlyph node in e.NewItems)
                    {
                        this.AddNode(node);
                    }
                }
            }

        }


        private void RemoveNode(Model.ClassGlyph node)
        {
            Shapes.ClassNode shape = this._view.Nodes.FilterAndCast<Shapes.ClassNode>().SingleOrDefault(s => s.DataContext == node);
            if (shape != null)
            {
                this._view.Nodes.Remove(shape);
            }
        }

        private void AddNode(Model.ClassGlyph node)
        {
            Shapes.ClassNode shape = new Shapes.ClassNode(node.Id);

            shape.DataContext = node;

            this._view.Nodes.Add(shape);
        }

        #region IDisposable Members

        public void Dispose()
        {
            this._model.Classes.CollectionChanged -= new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Classes_CollectionChanged);
            this._model.Inheritances.CollectionChanged -= new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Inheritances_CollectionChanged);
            this._model.Enums.CollectionChanged -= new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Enums_CollectionChanged);
            this._model.Associations.CollectionChanged -= new NotifyCollectionChangedEventHandler(Associations_CollectionChanged);
        }

        #endregion
    }
}
