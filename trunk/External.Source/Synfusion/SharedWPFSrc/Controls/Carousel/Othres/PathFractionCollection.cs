using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;

namespace Syncfusion.Windows.Shared
{
    /// <summary>
    /// 
    /// </summary>
    public class PathFractionCollection : ObservableCollection<FractionValue>
    {
        /// <summary>
        /// Finds the nearest points.
        /// </summary>
        /// <param name="currentStopPointPathFraction">The current stop point path fraction.</param>
        /// <param name="LeftNearestStopPint">The left nearest stop pint.</param>
        /// <param name="RightNearestStopPint">The right nearest stop pint.</param>
        internal void FindNearestPoints(double currentStopPointPathFraction, out FractionValue LeftNearestStopPint, out FractionValue RightNearestStopPint)
        {
            double leftClosestDistance = -1.0;
            double rightClosestDistance = -1.0;

            FractionValue leftitem = new FractionValue();
            FractionValue rightitem = new FractionValue();

            if (this.Items.Count <= 0)
            {
                throw new NotImplementedException();
            }

            foreach (FractionValue stopPoint in this.Items)
            {
                if ((stopPoint.Fraction >= leftClosestDistance) && (stopPoint.Fraction <= currentStopPointPathFraction))
                {
                    leftClosestDistance = stopPoint.Fraction;
                    leftitem = stopPoint;
                }
                else
                {
                    rightClosestDistance = stopPoint.Fraction;
                    rightitem = stopPoint;
                    break;
                }
            }
            if (leftClosestDistance == -1.0)
            {
                LeftNearestStopPint = null;
            }
            else
            {
                LeftNearestStopPint = leftitem; //this.Items[leftClosestDistance];
                //FractionValue fv = this.Items[0];
            }
            if (rightClosestDistance == -1.0)
            {
                RightNearestStopPint = null;
            }
            else
            {
                RightNearestStopPint = rightitem;// this.Items[rightClosestDistance];
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class FractionValue : DependencyObject
    {
        /// <summary>
        /// Gets or sets the fraction.
        /// </summary>
        /// <value>The fraction.</value>
        public double Fraction
        {
            get { return (double)GetValue(FractionProperty); }
            set { SetValue(FractionProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty FractionProperty =
            DependencyProperty.Register("Fraction", typeof(double), typeof(FractionValue), new PropertyMetadata(0.0));

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(FractionValue), new PropertyMetadata(0.0));
    }
}
