using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace Syncfusion.Windows.Shared
{
    internal static class CarouselPanelHelperMethods
    {
        /// <summary>
        /// Gets the default path.
        /// </summary>
        /// <returns></returns>
        public static Path GetDefaultPath()
        {
            System.Windows.Shapes.Path newPath = new System.Windows.Shapes.Path();
            object geometryFigures = new PathFigureCollectionConverter().ConvertFromString("M639,-115.5 C702,-106.5 666.49972,-35 491.49972,-35 300.4994,-35 293.49973,-116 343.50004,-116");
            PathGeometry newGeometry = new PathGeometry();
            newPath.Stretch = Stretch.Fill;
            BrushConverter brushConverter = new BrushConverter();
            newPath.Stroke = (Brush)brushConverter.ConvertFromString("#FF0998f8");
            newPath.StrokeThickness = 2.0;
            newGeometry.Figures = (PathFigureCollection)geometryFigures;
            newPath.Data = newGeometry;
            return newPath;

            //return new System.Windows.Shapes.Path()
            //{
            //    Data = PathGeometry.Parse("M639,-115.5 C702,-106.5 666.49972,-35 491.49972,-35 300.4994,-35 293.49973,-116 343.50004,-116"),
            //    VerticalAlignment = System.Windows.VerticalAlignment.Top,
            //    Stroke = new SolidColorBrush(Colors.Red),
            //    StrokeThickness = 2,
            //    HorizontalAlignment = HorizontalAlignment.Stretch,
            //};
        }

        /// <summary>
        /// Gets the default opacity fractions collection.
        /// </summary>
        /// <returns></returns>
        public static PathFractionCollection GetDefaultOpacityFractionsCollection()
        {
            PathFractionCollection opacityFractions = new PathFractionCollection();
            opacityFractions.Add(new FractionValue() { Fraction = 0.0, Value = 0.5 });
            opacityFractions.Add(new FractionValue() { Fraction = 0.5, Value = 1 });
            opacityFractions.Add(new FractionValue() { Fraction = 1, Value = 0.5 });
            return opacityFractions;
        }

        /// <summary>
        /// Gets the default scale fractions collection.
        /// </summary>
        /// <returns></returns>
        public static PathFractionCollection GetDefaultScaleFractionsCollection()
        {
            PathFractionCollection scaleFractions = new PathFractionCollection();
            scaleFractions.Add(new FractionValue() { Fraction = 0.0, Value = 0.3 });
            scaleFractions.Add(new FractionValue() { Fraction = 0.5, Value = 1 });
            scaleFractions.Add(new FractionValue() { Fraction = 1, Value = 0.3 });
            return scaleFractions;
        }

        /// <summary>
        /// Gets the default skew angle X fractions collection.
        /// </summary>
        /// <returns></returns>
        public static PathFractionCollection GetDefaultSkewAngleXFractionsCollection()
        {
            return new PathFractionCollection();
        }

        /// <summary>
        /// Gets the default skew angle Y fractions collection.
        /// </summary>
        /// <returns></returns>
        public static PathFractionCollection GetDefaultSkewAngleYFractionsCollection()
        {
            return new PathFractionCollection();
        }

        /// <summary>
        /// Gets the item count after.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <param name="itemCount">The item count.</param>
        /// <returns></returns>
        internal static int GetItemCountAfter(PathFractionRangeHandler range, int itemCount)
        {
            if (range.LastVisibleItemIndex >= itemCount)
            {
                return 0;
            }
            return ((itemCount - range.LastVisibleItemIndex) - 1);
        }

        /// <summary>
        /// Gets the item count before.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <returns></returns>
        internal static int GetItemCountBefore(PathFractionRangeHandler range)
        {
            if (range.FirstVisibleItemIndex < 0)
            {
                return 0;
            }
            return range.FirstVisibleItemIndex;
        }

        /// <summary>
        /// Determines whether [is in range] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        /// <returns>
        /// 	<c>true</c> if [is in range] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInRange(int value, int min, int max)
        {
            return ((value >= min) && (value <= max));
        }

        /// <summary>
        /// Coerces the value between range.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        /// <returns></returns>
        public static int CoerceValueBetweenRange(int value, int min, int max)
        {
            int newValue = 0;
            if (value > max)
            {
                newValue = max;
            }
            else
            {
                newValue = value;
            }
            if (newValue < min)
            {
                newValue = min;
            }
            return newValue;
        }
    }
}
