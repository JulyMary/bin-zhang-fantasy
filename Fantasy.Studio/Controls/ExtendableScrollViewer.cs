using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace Fantasy.Studio.Controls
{
    public class ExtendableScrollViewer : ScrollViewer
    {

        static ExtendableScrollViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExtendableScrollViewer), new FrameworkPropertyMetadata(typeof(ExtendableScrollViewer)));


        }



        public UIElement VerticalTop
        {
            get { return (UIElement)GetValue(VerticalTopProperty); }
            set { SetValue(VerticalTopProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VerticalTop.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VerticalTopProperty =
            DependencyProperty.Register("VerticalTop", typeof(UIElement), typeof(ExtendableScrollViewer), new UIPropertyMetadata(null));





        public UIElement VerticalBottom
        {
            get { return (UIElement)GetValue(VerticalBottomProperty); }
            set { SetValue(VerticalBottomProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VerticalBottom.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VerticalBottomProperty =
            DependencyProperty.Register("VerticalBottom", typeof(UIElement), typeof(ExtendableScrollViewer), new UIPropertyMetadata(null));



        public UIElement HorizontalLeft
        {
            get { return (UIElement)GetValue(HorizontalLeftProperty); }
            set { SetValue(HorizontalLeftProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HorizontalLeft.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HorizontalLeftProperty =
            DependencyProperty.Register("HorizontalLeft", typeof(UIElement), typeof(ExtendableScrollViewer), new UIPropertyMetadata(null));



        public UIElement HorizontalRight
        {
            get { return (UIElement)GetValue(HorizontalRightProperty); }
            set { SetValue(HorizontalRightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HorizontalRight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HorizontalRightProperty =
            DependencyProperty.Register("HorizontalRight", typeof(UIElement), typeof(ExtendableScrollViewer), new UIPropertyMetadata(null));




        public UIElement Intersection
        {
            get { return (UIElement)GetValue(IntersectionProperty); }
            set { SetValue(IntersectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Intersection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IntersectionProperty =
            DependencyProperty.Register("Intersection", typeof(UIElement), typeof(ExtendableScrollViewer), new UIPropertyMetadata(null));


    }
}
