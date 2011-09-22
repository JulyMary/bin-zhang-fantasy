using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using System.Reflection;

namespace Fantasy.Studio.BusinessEngine.ApplicationEditing
{
    public class VerticalTreeDescendantLine : Shape
    {

        public VerticalTreeDescendantLine()
        {
             
        }

      


        public double DiamondRadius
        {
            get { return (double)GetValue(DiamondRadiusProperty); }
            set 
            {
                if (value != DiamondRadius)
                {
                    SetValue(DiamondRadiusProperty, value);
                    this.InvalidateVisual();
                }
            }
        }

        // Using a DependencyProperty as the backing store for DiamondRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DiamondRadiusProperty =
            DependencyProperty.Register("DiamondRadius", typeof(double), typeof(VerticalTreeDescendantLine), new UIPropertyMetadata(5.0));



        protected override Geometry DefiningGeometry
        {
            get
            {
                double middle = this.ActualWidth / 2;
                Geometry line = new LineGeometry(new Point(DiamondRadius, DiamondRadius * 2), new Point(middle / 2, this.ActualHeight));
                return new CombinedGeometry(line, DiamondGeometry()); 
            }
        }

        private PathGeometry DiamondGeometry()
        {
            double middle = this.ActualWidth / 2;
            PathFigure diamondFigrue = new PathFigure();
            diamondFigrue.StartPoint = new Point(0, DiamondRadius);
            diamondFigrue.Segments.Add(new LineSegment(new Point(DiamondRadius, 0), true));
            diamondFigrue.Segments.Add(new LineSegment(new Point(DiamondRadius * 2, DiamondRadius), true));
            diamondFigrue.Segments.Add(new LineSegment(new Point(DiamondRadius, DiamondRadius * 2), true));
            diamondFigrue.IsClosed = true;
            PathGeometry diamond = new PathGeometry(new PathFigure[] { diamondFigrue }, FillRule.EvenOdd, new TranslateTransform(middle - this.DiamondRadius, 0));
            return diamond;
        }

        private Pen InvokePen()
        {
            MethodInfo mi = typeof(Shape).GetMethod("GetPen", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            Pen rs = (Pen)mi.Invoke(this, null);
            return rs;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Pen pen = this.InvokePen();
            drawingContext.DrawLine(pen, new Point(this.ActualWidth / 2, this.DiamondRadius * 2), new Point(this.ActualWidth / 2, this.ActualHeight));
            drawingContext.DrawGeometry(this.Fill, pen, this.DiamondGeometry());

        }


       
    }
}
