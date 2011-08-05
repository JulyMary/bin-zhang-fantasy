using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Syncfusion.Windows.Shared
{

    /// <summary>
    /// 
    /// </summary>
    public class PathFractions : DependencyObject
    {
        //protected override Freezable CreateInstanceCore()
        //{
        //    return new PathFractions();
        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="PathFractions"/> class.
        /// </summary>
        public PathFractions()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PathFractions"/> class.
        /// </summary>
        /// <param name="pathFraction">The path fraction.</param>
        public PathFractions(double pathFraction)
        {
            this.PathFraction = pathFraction;
        }

        /// <summary>
        /// Gets or sets the path fraction.
        /// </summary>
        /// <value>The path fraction.</value>
        public double PathFraction
        {
            get { return (double)GetValue(PathFractionProperty); }
            set { SetValue(PathFractionProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty PathFractionProperty =
            DependencyProperty.Register("PathFraction", typeof(double), typeof(PathFractions), new PropertyMetadata(0.0, null, new CoerceValueCallback(CoercePathFraction)));

        /// <summary>
        /// Coerces the path fraction.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="baseValue">The base value.</param>
        /// <returns></returns>
        private static object CoercePathFraction(DependencyObject d, object baseValue)
        {
            double value = (double)baseValue;
            if (value < 0.0)
                value = 0.0;

            if (value > 1.0)
                value = 1.0;
            return value;
        }


    }
}
