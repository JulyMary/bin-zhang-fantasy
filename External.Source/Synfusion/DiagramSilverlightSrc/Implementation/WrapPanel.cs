﻿#region Copyright
// Copyright Syncfusion Inc. 2001 - 2011. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion

namespace Syncfusion.Windows.Diagram
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Media;

    /// <summary>
    /// Represents the Resizer class which enables resizing of the node.
    /// </summary>
    public class WrapPanel : Panel
    {
        /// <summary>
        /// Defines the Orientation property.  This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(WrapPanel), new PropertyMetadata(Orientation.Horizontal));

        /// <summary>
        /// Initializes a new instance of the <see cref="WrapPanel"/> class.
        /// </summary>
        public WrapPanel()
        {
        }

        /// <summary>
        /// Gets or sets the HierarchicalDataTemplate for items.
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// Positions child elements and determines a size for the control.
        /// </summary>
        /// <param name="finalSize">The final area within the parent
        /// that this element should use to arrange itself and its children.</param>
        /// <returns>The actual size used.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            Point p = new Point(0, 0);
            int count = 1;
            double finalheight = 0;
            double temp = 0;
            if (Orientation == Orientation.Horizontal)
            {
                double maxheight = 0.0;
                foreach (UIElement element in this.Children)
                {
                    double elementwidth = element.DesiredSize.Width;
                    double elementheight = element.DesiredSize.Height;
                    element.Arrange(new Rect(p, new Point(p.X + elementwidth, p.Y + elementheight)));
                    if (elementheight > maxheight)
                    {
                        maxheight = elementheight;
                        temp = elementheight;
                    }

                    p.X = p.X + elementwidth;
                    if (count < this.Children.Count && (p.X + this.Children[count].DesiredSize.Width) > finalSize.Width)
                    {
                        p.X = 0;
                        p.Y = p.Y + maxheight;
                        maxheight = 0;
                        temp = elementheight;
                    }

                    count++;
                    finalheight = Math.Max(p.Y, finalheight);
                }
            }
            else
            {
                double maxwidth = 0.0;
                foreach (UIElement element in this.Children)
                {
                    double elementwidth = element.DesiredSize.Width;
                    double elementheight = element.DesiredSize.Height;
                    element.Arrange(new Rect(p, new Point(p.X + element.DesiredSize.Width, p.Y + element.DesiredSize.Height)));
                    if (elementwidth > maxwidth)
                    {
                        maxwidth = elementwidth;
                    }

                    p.Y = p.Y + elementheight;
                    if (count < this.Children.Count && (p.Y + this.Children[count].DesiredSize.Height) > finalSize.Height)
                    {
                        p.Y = 0;
                        p.X = p.X + maxwidth;
                        maxwidth = 0;
                    }

                    count++;
                    finalheight = Math.Max(p.Y, finalheight);
                }
            }

            return new Size(finalSize.Width, finalheight + temp);
        }

        /// <summary>
        /// Measures elements.
        /// </summary>
        /// <param name="availableSize">The available size</param>
        /// <returns>The available size.</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            Size size = new Size();
            double maxheight = 0;
            int count = 1;
            double finalheight = 0;
            double temp = 0;
            Point p = new Point(0, 0);
            foreach (UIElement element in this.Children)
            {
                element.Measure(availableSize);
                double elementwidth = element.DesiredSize.Width;
                double elementheight = element.DesiredSize.Height;
                if (elementheight > maxheight)
                {
                    maxheight = elementheight;
                    temp = elementheight;
                }

                p.X = p.X + elementwidth;
                if (count < this.Children.Count && (p.X + this.Children[count - 1].DesiredSize.Width) > availableSize.Width)
                {
                    p.X = 0;
                    p.Y = p.Y + maxheight;
                    maxheight = 0;
                    temp = elementheight;
                }

                count++;
                finalheight = Math.Max(p.Y, finalheight);
            }

            size.Width = availableSize.Width;
            size.Height = finalheight + temp;            
            return size;
        }
    }
}
