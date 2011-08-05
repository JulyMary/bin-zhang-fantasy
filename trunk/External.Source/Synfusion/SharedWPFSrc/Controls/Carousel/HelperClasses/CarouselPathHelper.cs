using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Animation;

namespace Syncfusion.Windows.Shared
{
    /// <summary>
    /// Helper methods for Path property in Carousel control
    /// </summary>
    internal class CarouselPathHelper
    {

        #region Privatemembers

        /// <summary>
        /// 
        /// </summary>
        private Path _CarouselPath;
        /// <summary>
        /// 
        /// </summary>
        private PathGeometry _Geometry;
        /// <summary>
        /// 
        /// </summary>
        private PathFractions[] _PathFractions;

        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CarouselPathHelper"/> class.
        /// </summary>
        /// <param name="Path">The path.</param>
        /// <param name="ItemsPerPage">The items per page.</param>
        public CarouselPathHelper(Path Path, int ItemsPerPage)
        {
            if (Path.Data != null)
                this.Geometry = PathGeometry.CreateFromGeometry(Path.Data);
            else
                this.Geometry = PathGeometry.CreateFromGeometry(System.Windows.Media.Geometry.Empty);

            this.CarouselPath = Path;
            this._PathFractions = CarouselPathHelper.GetPathFractions(ItemsPerPage);
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the carousel path.
        /// </summary>
        /// <value>The carousel path.</value>
        public Path CarouselPath
        {
            get { return _CarouselPath; }
            set { _CarouselPath = value; }
        }

        /// <summary>
        /// Gets or sets the geometry.
        /// </summary>
        /// <value>The geometry.</value>
        public PathGeometry Geometry
        {
            get { return _Geometry; }
            set { _Geometry = value; }
        }

        /// <summary>
        /// Gets the path fractions.
        /// </summary>
        /// <value>The path fractions.</value>
        public PathFractions[] PathFractions
        {
            get { return _PathFractions; }
        }

        /// <summary>
        /// 
        /// </summary>
        PathFractions topElementPathFraction;
        /// <summary>
        /// Gets the index of the top element path fraction.
        /// </summary>
        /// <value>The index of the top element path fraction.</value>
        public int TopElementPathFractionIndex
        {
            get
            {
                return this.GetPathFractionIndex(this.topElementPathFraction.PathFraction);
            }
        }

        /// <summary>
        /// Gets the top element path fraction.
        /// </summary>
        /// <value>The top element path fraction.</value>
        public double TopElementPathFraction
        {
            get
            {
                return this.topElementPathFraction.PathFraction;
            }
        }

        /// <summary>
        /// Sets the top element path fraction.
        /// </summary>
        /// <param name="desiredPathFraction">The desired path fraction.</param>
        public void SetTopElementPathFraction(PathFractions desiredPathFraction)
        {
            this.topElementPathFraction = new PathFractions(desiredPathFraction.PathFraction);
            if (!this.IsPathFractionDifferentFromStartAndEndFractions(desiredPathFraction))
            {
                PathFractions leftPoint = this.FindLeftNearestPathFraction(desiredPathFraction.PathFraction);
                PathFractions rightPoint = this.FindRightNearestPathFraction(desiredPathFraction.PathFraction);
                if ((leftPoint == null) || (leftPoint.PathFraction == 0.0))
                {
                    this.topElementPathFraction = rightPoint;
                }
                else if ((rightPoint == null) || (rightPoint.PathFraction == 1.0))
                {
                    this.topElementPathFraction = leftPoint;
                }
                else if (Math.Abs((double)(leftPoint.PathFraction - desiredPathFraction.PathFraction)) <= Math.Abs((double)(rightPoint.PathFraction - desiredPathFraction.PathFraction)))
                {
                    this.topElementPathFraction = leftPoint;
                }
                else
                {
                    this.topElementPathFraction = rightPoint;
                }
            }
        }

        /// <summary>
        /// Finds the left nearest path fraction.
        /// </summary>
        /// <param name="pathFraction">The path fraction.</param>
        /// <returns></returns>
        public PathFractions FindLeftNearestPathFraction(double pathFraction)
        {
            int controlPointIndex = FindLeftNearestPathFraction(this.PathFractions, pathFraction);
            if (controlPointIndex != -1)
            {
                return this.PathFractions[controlPointIndex];
            }
            return null;
        }

        /// <summary>
        /// Finds the left nearest path fraction.
        /// </summary>
        /// <param name="_pathfractions">The _pathfractions.</param>
        /// <param name="pathFraction">The path fraction.</param>
        /// <returns></returns>
        public static int FindLeftNearestPathFraction(PathFractions[] _pathfractions, double pathFraction)
        {
            int leftNearestIndex = -1;
            int foundIndex = FindNearestPathFractionIndex(pathFraction, _pathfractions);
            if (foundIndex < 0)
            {
                int firstLargerIndex = Math.Abs(foundIndex) - 1;
                if ((firstLargerIndex - 1) >= 0)
                {
                    leftNearestIndex = firstLargerIndex - 1;
                }
                return leftNearestIndex;
            }
            if ((foundIndex - 1) >= 0)
            {
                leftNearestIndex = foundIndex - 1;
            }
            return leftNearestIndex;
        }

        /// <summary>
        /// Finds the index of the nearest path fraction.
        /// </summary>
        /// <param name="pathFraction">The path fraction.</param>
        /// <param name="PathFractions">The path fractions.</param>
        /// <returns></returns>
        private static int FindNearestPathFractionIndex(double pathFraction, PathFractions[] PathFractions)
        {
            PathFractions _pathfraction = new PathFractions(pathFraction);

            int i = 0;
            foreach (PathFractions item in PathFractions)
            {
                if (item.PathFraction == pathFraction)
                {
                    return i;
                }
                i++;
            }
            return -1;
        }

        /// <summary>
        /// Finds the right nearest path fraction.
        /// </summary>
        /// <param name="pathFraction">The path fraction.</param>
        /// <returns></returns>
        public PathFractions FindRightNearestPathFraction(double pathFraction)
        {
            int pathFractionIndex = FindRightNearestPathFraction(this.PathFractions, pathFraction);
            if (pathFractionIndex != -1)
            {
                return this.PathFractions[pathFractionIndex];
            }
            return null;
        }

        /// <summary>
        /// Finds the right nearest path fraction.
        /// </summary>
        /// <param name="_pathFractions">The _path fractions.</param>
        /// <param name="pathFraction">The path fraction.</param>
        /// <returns></returns>
        public static int FindRightNearestPathFraction(PathFractions[] _pathFractions, double pathFraction)
        {
            int rightNearestIndex = -1;
            int foundIndex = FindNearestPathFractionIndex(pathFraction, _pathFractions);
            if (foundIndex < 0)
            {
                int firstLargerIndex = Math.Abs(foundIndex) - 1;
                if (firstLargerIndex < _pathFractions.Length)
                {
                    rightNearestIndex = firstLargerIndex;
                }
                return rightNearestIndex;
            }
            if ((foundIndex + 1) < _pathFractions.Length)
            {
                rightNearestIndex = foundIndex + 1;
            }
            return rightNearestIndex;
        }

        /// <summary>
        /// Determines whether [is path fraction different from start and end fractions] [the specified _path fraction].
        /// </summary>
        /// <param name="_pathFraction">The _path fraction.</param>
        /// <returns>
        /// 	<c>true</c> if [is path fraction different from start and end fractions] [the specified _path fraction]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsPathFractionDifferentFromStartAndEndFractions(PathFractions _pathFraction)
        {
            return ((this.IsPathFraction(_pathFraction.PathFraction) && (_pathFraction.PathFraction != 0.0)) && (_pathFraction.PathFraction != 1.0));
        }

        /// <summary>
        /// Determines whether [is path fraction] [the specified path fraction].
        /// </summary>
        /// <param name="pathFraction">The path fraction.</param>
        /// <returns>
        /// 	<c>true</c> if [is path fraction] [the specified path fraction]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsPathFraction(double pathFraction)
        {
            return (this.GetPathFractionIndex(pathFraction) >= 0);
        }
        #endregion

        /// <summary>
        /// Splits the Path based on no of items in the page.
        /// </summary>
        /// <param name="ItemsPerPage">The items per page.</param>
        /// <returns></returns>
        public static PathFractions[] GetPathFractions(int ItemsPerPage)
        {
            int NoOfFractions = ItemsPerPage + 2;
            PathFractions[] PathFractions = new PathFractions[NoOfFractions];
            double distanceRatio = Math.Round((double)1 / (NoOfFractions - 1), 3);

            for (int i = 0; i < NoOfFractions; i++)
            {
                double Fraction = Math.Round(distanceRatio * i, 3);
                PathFractions.SetValue(new PathFractions(Fraction), i);
            }
            return PathFractions;
        }

        /// <summary>
        /// Updating Path positions based on the available size.
        /// </summary>
        /// <param name="availablesize">The availablesize.</param>
        /// <param name="padding">The padding.</param>
        public void UpdateGeometryPath(Size availablesize, Thickness padding)
        {
            if (this.Geometry != null)
            {
                this.Geometry.Transform = null;
                Rect ViewPort = new Rect(padding.Left, padding.Top, (double)availablesize.Width - (padding.Left + padding.Right), (double)availablesize.Height - (padding.Top + padding.Bottom));
                ScaleTransform scaleTransform = ScalePathToAvailableSize(ViewPort, this.Geometry.Bounds, this.CarouselPath.Stretch);
                Rect TransformedGeometryBounds = ChangeGeometryBounds(this.Geometry.Bounds, scaleTransform.Value);
                TranslateTransform translateTransform = TranslatePathToAvailableSize(ViewPort, TransformedGeometryBounds);

                TransformGroup transformGroup = new TransformGroup();
                transformGroup.Children.Add(scaleTransform);
                transformGroup.Children.Add(translateTransform);
                translateTransform.Freeze();

                this.Geometry.Transform = transformGroup;
            }
        }

        /// <summary>
        /// Scales the size of the path to AvailableSize
        /// </summary>
        /// <param name="ViewPort">The view port.</param>
        /// <param name="GeometryBounds">The geometry bounds.</param>
        /// <param name="Stretch">The stretch.</param>
        /// <returns></returns>
        private ScaleTransform ScalePathToAvailableSize(Rect ViewPort, Rect GeometryBounds, Stretch Stretch)
        {
#if new

            //----
            double PathViewPortWidth = double.IsNaN(GeometryBounds.Width) ? ViewPort.Width : GeometryBounds.Width;
            double PathViewPortHeight = double.IsNaN(GeometryBounds.Height) ? ViewPort.Height : GeometryBounds.Height;
            //----
            double ScaleX = GeometryBounds.Width == 0 ? PathViewPortWidth : ViewPort.Width / GeometryBounds.Width;
            double ScaleY = GeometryBounds.Height == 0 ? PathViewPortHeight : ViewPort.Height / GeometryBounds.Height;
            //----
            switch (Stretch)
            {
                case Stretch.None:
                    ScaleX = ScaleY = 1;
                    break;
                case Stretch.Fill:
                    break;
                case Stretch.Uniform:
                    ScaleX = ScaleY = Math.Min(ScaleX, ScaleY);
                    break;
                case Stretch.UniformToFill:
                    ScaleX = ScaleY = Math.Max(ScaleX, ScaleY);
                    break;
            }
            //----
            ScaleTransform scaleTransform = new ScaleTransform();
            scaleTransform.ScaleX = ScaleX;
            scaleTransform.ScaleY = ScaleY;
            return scaleTransform;

#endif

#if old
#endif

            //----
            double PathViewPortWidth = double.IsNaN(this.CarouselPath.Width) ? ViewPort.Width : this.CarouselPath.Width;
            double PathViewPortHeight = double.IsNaN(this.CarouselPath.Height) ? ViewPort.Height : this.CarouselPath.Height;
            //----
            double ScaleX = GeometryBounds.Width == 0 ? PathViewPortWidth : PathViewPortWidth / GeometryBounds.Width;
            double ScaleY = GeometryBounds.Height == 0 ? PathViewPortHeight : PathViewPortHeight / GeometryBounds.Height;

            //----
            switch (Stretch)
            {
                case Stretch.None:
                    ScaleX = ScaleY = 1;
                    break;
                case Stretch.Fill:
                    break;
                case Stretch.Uniform:
                    ScaleX = ScaleY = Math.Min(ScaleX, ScaleY);
                    break;
                case Stretch.UniformToFill:
                    ScaleX = ScaleY = Math.Max(ScaleX, ScaleY);
                    break;
            }
            //----
            ScaleTransform scaleTransform = new ScaleTransform();
            scaleTransform.ScaleX = ScaleX;
            scaleTransform.ScaleY = ScaleY;
            return scaleTransform;
        }

        /// <summary>
        /// Changes the geometry bounds based on the ScaleTransform
        /// </summary>
        /// <param name="CurrentGeometryBounds">The current geometry bounds.</param>
        /// <param name="Transformation">The transformation.</param>
        /// <returns></returns>
        private static Rect ChangeGeometryBounds(Rect CurrentGeometryBounds, Matrix Transformation)
        {
            Rect TransformedBounds = Rect.Transform(CurrentGeometryBounds, Transformation);
            TransformedBounds.Width = TransformedBounds.Width == 0 ? 0.5 : TransformedBounds.Width;
            TransformedBounds.Height = TransformedBounds.Height == 0 ? 0.5 : TransformedBounds.Height;
            return TransformedBounds;
        }

        /// <summary>
        /// Translates the size of the path based on AvailableSize.
        /// </summary>
        /// <param name="ViewPort">The view port.</param>
        /// <param name="GeometryBounds">The geometry bounds.</param>
        /// <returns></returns>
        private TranslateTransform TranslatePathToAvailableSize(Rect ViewPort, Rect GeometryBounds)
        {
#if Old
#endif

            double HorizontalOffset = ViewPort.Left;
            double VerticalOffset = ViewPort.Top;
            //Path Width and Height
            double PathWidth = double.IsNaN(this.CarouselPath.Width) ? GeometryBounds.Width : this.CarouselPath.Width;
            double PathHeight = double.IsNaN(this.CarouselPath.Height) ? GeometryBounds.Height : this.CarouselPath.Height;
            
            //Remaining Width and Height
            double RemainingWidth = Math.Max(0.0, ViewPort.Width - PathWidth);
            double RemainingHeight = Math.Max(0.0, ViewPort.Height - PathHeight);

            //Calculating Transformations
            HorizontalOffset += CalculateHorizontalTransformation(this.CarouselPath.HorizontalAlignment, RemainingWidth);
            VerticalOffset += CalculateVerticalTransformation(this.CarouselPath.VerticalAlignment, RemainingHeight);
            double TranX = HorizontalOffset - GeometryBounds.Left;
            double TranY = RemainingHeight - GeometryBounds.Top;
            
            return new TranslateTransform(TranX, TranY);

#if New

            double HorizontalOffset = ViewPort.Left;
            double VerticalOffset = ViewPort.Top;
            //Path Width and Height
            //double PathWidth = double.IsNaN(this.CarouselPath.Width) ? GeometryBounds.Width : this.CarouselPath.Width;
            //double PathHeight = double.IsNaN(this.CarouselPath.Height) ? GeometryBounds.Height : this.CarouselPath.Height;
            
            //Remaining Width and Height
            double RemainingWidth = Math.Max(0.0, ViewPort.Width - GeometryBounds.Width);
            double RemainingHeight = Math.Max(0.0, ViewPort.Height - GeometryBounds.Height);

            //Calculating Transformations
            HorizontalOffset += CalculateHorizontalTransformation(this.CarouselPath.HorizontalAlignment, RemainingWidth);
            VerticalOffset += CalculateVerticalTransformation(this.CarouselPath.VerticalAlignment, RemainingHeight);
            double TranX = HorizontalOffset - GeometryBounds.Left;
            double TranY = RemainingHeight - GeometryBounds.Top;
            
            return new TranslateTransform(TranX, TranY);
#endif

        }

        private static double CalculateHorizontalTransformation(HorizontalAlignment Alignment, double RemainingWidth)
        {
            switch (Alignment)
            {
                case HorizontalAlignment.Center:
                case HorizontalAlignment.Stretch:
                    return Math.Max(0.0, RemainingWidth / 2);
                case HorizontalAlignment.Right:
                    return Math.Max(0.0, RemainingWidth);
            }
            return 0.0;
        }

        private static double CalculateVerticalTransformation(VerticalAlignment Alignment, double RemainHeight)
        {
            switch (Alignment)
            {
                case VerticalAlignment.Center:
                case VerticalAlignment.Stretch:
                    return Math.Max(0.0, RemainHeight / 2);
                case VerticalAlignment.Bottom:
                    return Math.Max(0.0, RemainHeight);
            }
            return 0.0;
        }

        #region SupportingMethods

        public int GetPathFractionIndex(double pathFraction)
        {
            int i = 0;
            foreach (PathFractions item in this.PathFractions)
            {
                if (item.PathFraction == pathFraction)
                {
                    return i;
                }
                i++;
            }
            return -1;
        }

        public int GetVisiblePathFractionCount()
        {
            return (this.PathFractions.Length);
            //return (this.PathFractions.Length - 2);
        }
        #endregion

        #region HelperMethods

        public static bool IsVisible(double pathFraction)
        {
            bool visible = ((pathFraction == -1.0) || (pathFraction == 0.0)) || (pathFraction == 1.0);
            return !visible;
        }

        public int ComparePathFractions(PathFractions x, PathFractions y)
        {
            if (x == null)
            {
                throw new ArgumentNullException("x");
            }
            if (y == null)
            {
                throw new ArgumentNullException("y");
            }
            return x.PathFraction.CompareTo(y.PathFraction);
        }
        #endregion

        public PathFractions GetVisiblePathFraction(int index)
        {
            if ((index >= 0) && (index != this.GetVisiblePathFractionCount()))
            {
                return this.PathFractions[index + 1];
            }
            return null;
        }
    }
}
