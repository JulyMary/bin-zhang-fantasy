using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;
using System.Reflection;

namespace Fantasy.Studio.BusinessEngine.ApplicationEditing
{
    public class VerticalTreeAncestorLine : Shape
    {
        protected override Geometry DefiningGeometry
        {
            get 
            {
                double middle = this.ActualWidth / 2;

                Geometry rs = new LineGeometry(new Point(middle, this.StrokeThickness), new Point(middle, this.ActualHeight));

                if (this.HasLeftSibling)
                {
                    rs = new CombinedGeometry(rs, new LineGeometry(new Point(0, this.StrokeThickness), new Point(middle, this.StrokeThickness)));
                }

                if (this.HasRightSibling)
                {
                    rs = new CombinedGeometry(rs, new LineGeometry(new Point(middle, this.StrokeThickness), new Point(this.ActualWidth, this.StrokeThickness)));
                }

                rs = new CombinedGeometry(rs, ArrowGeometry());

                return rs;

            }
        }




        public double ArrowRadius
        {
            get { return (double)GetValue(ArrowRadiusProperty); }
            set { SetValue(ArrowRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ArrowLength.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ArrowRadiusProperty =
            DependencyProperty.Register("ArrowRadius", typeof(double), typeof(VerticalTreeAncestorLine), new UIPropertyMetadata(5.0));


        private PathGeometry ArrowGeometry()
        {
            double middle = this.ActualWidth / 2;
            PathFigure diamondFigrue = new PathFigure();
            diamondFigrue.StartPoint = new Point(0, 0);
            diamondFigrue.Segments.Add(new LineSegment(new Point(ArrowRadius * 2, 0), true));
            diamondFigrue.Segments.Add(new LineSegment(new Point(ArrowRadius , ArrowRadius * 2), true));
            diamondFigrue.IsClosed = true;
            PathGeometry diamond = new PathGeometry(new PathFigure[] { diamondFigrue }, FillRule.EvenOdd, new TranslateTransform(middle - this.ArrowRadius, this.ActualHeight - this.ArrowRadius * 2 - this.StrokeThickness));
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
            Pen pen = InvokePen();
            double middle = this.ActualWidth / 2;
            drawingContext.DrawLine(pen, new Point(middle, this.StrokeThickness), new Point(middle, this.ActualHeight - this.ArrowRadius * 2));
            if (this.HasLeftSibling)
            {
                drawingContext.DrawLine(pen, new Point(0, this.StrokeThickness), new Point(middle, this.StrokeThickness)); 
            }
            if (this.HasRightSibling)
            {
                drawingContext.DrawLine(pen, new Point(middle, this.StrokeThickness), new Point(this.ActualWidth, this.StrokeThickness));
            }

            drawingContext.DrawGeometry(this.Fill, pen, ArrowGeometry());
        }

        public bool HasLeftSibling
        {
            get { return (bool)GetValue(HasLeftSiblingProperty); }
            set 
            {
                if (value != this.HasLeftSibling)
                {
                    SetValue(HasLeftSiblingProperty, value);
                    InvalidateVisual();
                }
            }
        }

        // Using a DependencyProperty as the backing store for HasLeftSibling.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasLeftSiblingProperty =
            DependencyProperty.Register("HasLeftSibling", typeof(bool), typeof(VerticalTreeAncestorLine), new UIPropertyMetadata(true));

        public bool HasRightSibling
        {
            get { return (bool)GetValue(HasRightSiblingProperty); }
            set
            {
                if (value != this.HasRightSibling)
                {
                    SetValue(HasRightSiblingProperty, value);
                    InvalidateVisual();
                }
            }
        }

        // Using a DependencyProperty as the backing store for HasRightSibling.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasRightSiblingProperty =
            DependencyProperty.Register("HasRightSibling", typeof(bool), typeof(VerticalTreeAncestorLine), new UIPropertyMetadata(true));




    }

}
