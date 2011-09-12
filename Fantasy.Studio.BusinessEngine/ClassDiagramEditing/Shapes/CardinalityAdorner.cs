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

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Shapes
{
    public class CardinalityAdorner : Adorner
    {
        public CardinalityAdorner(LineConnector connector)
            : base(connector)
        {
            this.IsHitTestVisible = false;
            this.LineConnector.ConnectorPathGeometryUpdated += new EventHandler<EventArgs>(LineConnector_ConnectorPathGeometryUpdated);
            this.LineConnector.DataContextChanged += new DependencyPropertyChangedEventHandler(LineConnector_DataContextChanged);
            this._entityListener = new WeakEventListener(this.EntityCardinityChanged);
        }

        private WeakEventListener _entityListener;

        private bool EntityCardinityChanged(Type managerType, object sender, EventArgs e)
        {
            this.InvalidateVisual();
            return true;
        }

        void LineConnector_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Model.AssociationGlyph association;
            association = e.OldValue as Model.AssociationGlyph;

            if (association != null)
            {
                PropertyChangedEventManager.RemoveListener(association.Entity, this._entityListener, "RightCardinality");
                PropertyChangedEventManager.RemoveListener(association.Entity, this._entityListener, "LeftCardinality");
            }
            
            association = this.LineConnector.DataContext as Model.AssociationGlyph;
            
            if (association != null)
            {
                PropertyChangedEventManager.AddListener(association.Entity, this._entityListener, "RightCardinality");
                PropertyChangedEventManager.AddListener(association.Entity, this._entityListener, "LeftCardinality");
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
                return this.LineConnector.IntermediatePoints.Count == 2 && this.LineConnector.IntermediatePoints[0].X == 0.0 && this.LineConnector.IntermediatePoints[0].Y == 0
                    && this.LineConnector.IntermediatePoints[1].X == 0 && this.LineConnector.IntermediatePoints[1].Y == 0;
            }
        }

        private double _textOffset = 10.0;

        private void DrawCardinality(System.Windows.Media.DrawingContext drawingContext, bool isHead)
        {
            Model.AssociationGlyph association = this.LineConnector.DataContext as Model.AssociationGlyph;
            if (association == null || association.Entity == null)
            {
                return;
            }
            string text = isHead ? association.Entity.LeftCardinality : association.Entity.RightCardinality;

            Point ps, pe;
            if (isHead)
            {
                ps = this.LineConnector.StartPointPosition;
                pe = this.LineConnector.IntermediatePoints[0];
            }

            else
            {
                ps = this.LineConnector.EndPointPosition;
                pe = this.LineConnector.IntermediatePoints.Last();
            }

            Point textPosition;

            if (ps.Y > pe.Y)
            {
                //north
                textPosition = new Point(this._textOffset, -this._textOffset);
            }
            else if (ps.X < pe.X)
            {
                //east
                textPosition = new Point(this._textOffset, this._textOffset);
            }
            else if (ps.Y < pe.Y)
            {
                textPosition = new Point(this._textOffset, this._textOffset);
            }
            else
            {
                textPosition = new Point(-this._textOffset, this._textOffset);
            }

            Point offset = new Point(ps.X + textPosition.X, ps.Y + textPosition.Y);
           

            Control control = this.AdornedElement.GetAncestor<Control>();

            FormattedText formatted = new FormattedText(text, CultureInfo.CurrentCulture, System.Windows.FlowDirection.LeftToRight, new Typeface("Tahoma"), 12.0, this.LineConnector.LineStyle.Stroke);

            drawingContext.DrawText(formatted, offset); 
        }


        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            this.DrawCardinality(drawingContext, true);
            this.DrawCardinality(drawingContext, false);
        }

        private LineConnector _lineConnector = null;
        public LineConnector LineConnector
        {
            get
            {
                return this.AdornedElement as LineConnector;
            }
        }

       


       
    }
}
