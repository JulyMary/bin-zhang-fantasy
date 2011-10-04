﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Fantasy.Studio.Controls
{
    /// <summary>
    /// Interaction logic for Previewer.xaml
    /// </summary>
    public partial class Previewer : UserControl
    {
        public Previewer()
        {
            InitializeComponent();
        }



        public ScrollViewer View
        {
            get { return (ScrollViewer)GetValue(ViewProperty); }
            set 
            {
                if (this.View != null)
                {
                    this.View.ScrollChanged -= new ScrollChangedEventHandler(View_ScrollChanged);
                }
                SetValue(ViewProperty, value);
                if(value != null)
                {
                    this.BindingView(value);
                }
            }
        }

        private void BindingView(ScrollViewer value)
        {
            if (this.ActualWidth > 0 && this.ActualHeight > 0)
            {
                ResetScale();


                //Set viewPortThumb Layout
                SetThumbLocation();

                this.View.ScrollChanged += new ScrollChangedEventHandler(View_ScrollChanged);
            }
           
           
            

        }

        private void SetThumbLocation()
        {
            this.ViewPortThumb.Width = this.PreviewRect.Width * this._scale;
            this.ViewPortThumb.Height = this.PreviewRect.Height * this._scale;
            double vptLeft = this.View.HorizontalOffset * _scale + this._previewLeft;
            double vptTop = this.View.VerticalOffset * _scale + this._previewTop;
            Canvas.SetLeft(this.ViewPortThumb, vptLeft);
            Canvas.SetTop(this.ViewPortThumb, vptTop);
        }

        private void ResetScale()
        {
            double scaleX = this.ActualWidth / this.View.ExtentWidth;
            double scaleY = this.ActualHeight / this.View.ExtentHeight;
            if (scaleX < scaleY)
            {
                this.PreviewRect.Width = this.ActualWidth;
                this.PreviewRect.Height = this.ActualWidth * this.View.ExtentHeight / this.View.ExtentWidth;
                this._scale = scaleX; 
            }
            else
            {
                this.PreviewRect.Height = this.ActualHeight;
                this.PreviewRect.Height = this.ActualHeight * this.View.ExtentWidth / this.View.ExtentHeight ;
                this._scale = scaleY; 
            }


            //Set PreviewRect Layout;
           
            this._previewLeft = this.ActualWidth - this.PreviewRect.Width / 2;
            this._previewTop = this.ActualHeight - this.PreviewRect.Height / 2;
            Canvas.SetLeft(this.PreviewRect, this._previewLeft);
            Canvas.SetTop(this.PreviewRect, this._previewTop);

            FrameworkElement content = (FrameworkElement)this.View.Content;

            VisualBrush visualBrush = new VisualBrush(this.View);
            visualBrush.Stretch = Stretch.Uniform;
            visualBrush.ViewboxUnits = BrushMappingMode.Absolute;
            visualBrush.Viewbox = new Rect(0, 0, content.ActualWidth, content.ActualHeight);
            this.PreviewRect.Fill = visualBrush;

            this.SetThumbLocation();
        }

        void View_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange != 0 || e.ExtentWidthChange != 0)
            {
                this.ResetScale();
            }
            else
            {
                this.SetThumbLocation();
            }
        }

       

        private double _scale = 0;
        private double _previewLeft = 0;
        private double _previewTop = 0;

        // Using a DependencyProperty as the backing store for View.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewProperty =
            DependencyProperty.Register("View", typeof(ScrollViewer), typeof(Previewer), new PropertyMetadata(null));


        private double _dragStartLeft;
        private double _dragStartTop;

        private void ViewPortThumb_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            _dragStartLeft = Canvas.GetLeft(this.ViewPortThumb);
            _dragStartTop = Canvas.GetTop(this.ViewPortThumb);
        }

        private void ViewPortThumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            double offSetX = (e.HorizontalChange + _dragStartLeft) / this._scale;
            offSetX = Math.Max(0, offSetX);
            offSetX = Math.Min(offSetX, this.View.ExtentWidth - this.View.ViewportWidth);

            double offsetY = (e.VerticalChange + this._dragStartTop) / this._scale;
            offsetY = Math.Max(0, offsetY);
            offsetY = Math.Min(offsetY, this.View.ExtentHeight - this.View.ViewportHeight);

            this.View.ScrollToHorizontalOffset(offSetX);
            this.View.ScrollToVerticalOffset(offsetY);
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.View != null)
            {
                this.BindingView(this.View);
            }
        }
        
    }
}
