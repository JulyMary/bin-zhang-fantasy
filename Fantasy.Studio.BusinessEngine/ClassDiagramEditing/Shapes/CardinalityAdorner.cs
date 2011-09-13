using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Shapes;
using Syncfusion.Windows.Diagram;
using Fantasy.Windows;
using System.Windows;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Controls;
using System.Globalization;
using System.Collections;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Shapes
{
    public class CardinalityAdorner : Adorner
    {
        public CardinalityAdorner(IDiagramPage dPanel, LineConnector connector)
            : base(dPanel as Panel)
        {
            this.IsHitTestVisible = false;
            this._lineConnector = connector;
            this._lineConnector.ConnectorPathGeometryUpdated += new EventHandler<EventArgs>(LineConnector_ConnectorPathGeometryUpdated);
            this._lineConnector.DataContextChanged += new DependencyPropertyChangedEventHandler(LineConnector_DataContextChanged);
            this._entityListener = new WeakEventListener(this.EntityCardinityChanged);

            HandleCardinalityChanged(this._lineConnector.DataContext as Model.AssociationGlyph, null);

            AdornerLayer layer = AdornerLayer.GetAdornerLayer(this.AdornedElement);
            layer.Add(this);

            this._lineConnector.Unloaded += new RoutedEventHandler(LineConnector_Unloaded);

           
        }

      

        void LineConnector_Unloaded(object sender, RoutedEventArgs e)
        {
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(this.AdornedElement);
            layer.Remove(this);
        }

        private WeakEventListener _entityListener;

        private bool EntityCardinityChanged(Type managerType, object sender, EventArgs e)
        {
            this.InvalidateVisual();
            return true;
        }

        void LineConnector_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.HandleCardinalityChanged(e.NewValue as Model.AssociationGlyph, e.OldValue as Model.AssociationGlyph);
        }

        private void HandleCardinalityChanged(Model.AssociationGlyph newValue, Model.AssociationGlyph oldValue)
        {


            if (oldValue != null)
            {
                PropertyChangedEventManager.RemoveListener(oldValue.Entity, this._entityListener, "RightCardinality");
                PropertyChangedEventManager.RemoveListener(oldValue.Entity, this._entityListener, "LeftCardinality");
            }



            if (newValue != null)
            {
                PropertyChangedEventManager.AddListener(newValue.Entity, this._entityListener, "RightCardinality");
                PropertyChangedEventManager.AddListener(newValue.Entity, this._entityListener, "LeftCardinality");
            }
          
        }

        void LineConnector_ConnectorPathGeometryUpdated(object sender, EventArgs e)
        {
            this.InvalidateVisual();
        }

        private bool IsEmptyIntermediatePoints
        {
            get
            {
                return this._lineConnector.IntermediatePoints.Count == 2 && this._lineConnector.IntermediatePoints[0].X == 0.0 && this._lineConnector.IntermediatePoints[0].Y == 0
                    && this._lineConnector.IntermediatePoints[1].X == 0 && this._lineConnector.IntermediatePoints[1].Y == 0;
            }
        }

        private double _textOffset = 5.0;


        private int Compare(double x, double y)
        {
            x = Math.Round(x, 4, MidpointRounding.ToEven);
            y = Math.Round(y, 4, MidpointRounding.ToEven);

            return Comparer.Default.Compare(x, y);
        }

        private void DrawCardinality(System.Windows.Media.DrawingContext drawingContext, bool isHead)
        {
            Model.AssociationGlyph association = this._lineConnector.DataContext as Model.AssociationGlyph;
            if (association == null || association.Entity == null)
            {
                return;
            }
            string text = isHead ? association.Entity.LeftCardinality : association.Entity.RightCardinality;
            FormattedText formatted = new FormattedText(text, CultureInfo.CurrentCulture, System.Windows.FlowDirection.LeftToRight, new Typeface("Tahoma"), 12.0, this._lineConnector.LineStyle.Stroke);
           


            Point ps, pe;
            if (isHead)
            {
                ps = this._lineConnector.HeadDecoratorPosition;
                pe = this._lineConnector.IntermediatePoints[0];
            }

            else
            {
                ps = this._lineConnector.TailDecoratorPosition;
                pe = this._lineConnector.IntermediatePoints.Last();
            }

            Point textPosition;

            if (Compare(ps.Y , pe.Y) > 0)
            {
                //north
                textPosition = new Point(this._textOffset, -this._textOffset - formatted.Height);
            }
            else if (Compare(ps.X , pe.X) < 0)
            {
                //east
                textPosition = new Point(this._textOffset, this._textOffset);
            }
            else if (Compare(ps.Y , pe.Y) < 0)
            {
                textPosition = new Point(this._textOffset, this._textOffset);
            }
            else
            {
                textPosition = new Point(-this._textOffset - formatted.Width, this._textOffset);
            }

            Point offset = new Point(ps.X + textPosition.X, ps.Y + textPosition.Y);
           

            Control control = this.AdornedElement.GetAncestor<Control>();

            
            drawingContext.DrawText(formatted, offset); 
        }


        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            this.DrawCardinality(drawingContext, true);
            this.DrawCardinality(drawingContext, false);
        }

        private LineConnector _lineConnector = null;
       
       

       


       
    }
}
