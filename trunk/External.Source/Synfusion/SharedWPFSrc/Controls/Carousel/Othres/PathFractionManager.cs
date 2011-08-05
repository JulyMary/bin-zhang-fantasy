using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Syncfusion.Windows.Shared
{
    /// <summary>
    /// 
    /// </summary>
    internal class PathFractionManager
    {
        private double newPathFraction;
        private double currentPathFraction;

        /// <summary>
        /// Initializes a new instance of the <see cref="PathFractionManager"/> class.
        /// </summary>
        /// <param name="newVal">The new val.</param>
        /// <param name="oldVal">The old val.</param>
        public PathFractionManager(double newVal, double oldVal)
        {
            if (newVal < 0.0)
            {
                throw new Exception();
            }
            this.newPathFraction = newVal;
            this.currentPathFraction = oldVal;
        }

        /// <summary>
        /// Gets the new path fraction.
        /// </summary>
        /// <value>The new path fraction.</value>
        public double NewPathFraction
        {
            get
            {
                return this.newPathFraction;
            }
        }

        /// <summary>
        /// Gets the current path fraction.
        /// </summary>
        /// <value>The current path fraction.</value>
        public double CurrentPathFraction
        {
            get
            {
                return this.currentPathFraction;
            }
        }
    }
}
