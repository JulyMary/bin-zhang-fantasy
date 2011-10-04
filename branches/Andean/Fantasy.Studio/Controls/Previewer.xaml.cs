using System;
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
using System.Windows.Controls.Primitives;
using System.Diagnostics;

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
                this.View.ScrollChanged += new ScrollChangedEventHandler(View_ScrollChanged);

              

                ResetScale();


                //Set viewPortThumb Layout
                SetThumbLocation();

                
            }
           
           
            

        }



        private void SetThumbLocation()
        {
            this.ViewPortThumb.Width = this.View.ViewportWidth * this._scale;
            this.ViewPortThumb.Height = this.View.ViewportHeight * this._scale;
            double vptLeft = this.View.HorizontalOffset * _scale;
            double vptTop = this.View.VerticalOffset * _scale;

            
            Canvas.SetLeft(this.ViewPortThumb, vptLeft);
            Canvas.SetTop(this.ViewPortThumb, vptTop);
        }

        private void ResetScale()
        {
            double scaleX = this.ActualWidth / this.View.ExtentWidth;
            double scaleY = this.ActualHeight / this.View.ExtentHeight;
            this._scale = Math.Min(scaleX, scaleY);

            this.PreviewBorder.Width = this.View.ExtentWidth * this._scale;
            this.PreviewBorder.Height = this.View.ExtentHeight * this._scale;
           
            


            //Set PreviewRect Layout;

            this._previewLeft = (this.ActualWidth - this.PreviewBorder.Width) / 2;
            this._previewTop = (this.ActualHeight - this.PreviewBorder.Height) / 2;
            Canvas.SetLeft(this.PreviewBorder, this._previewLeft);
            Canvas.SetTop(this.PreviewBorder, this._previewTop);


            FrameworkElement content = (FrameworkElement)this.View.Content;
            VisualBrush visualBrush = new VisualBrush(content);
            visualBrush.Stretch = Stretch.Uniform;
            visualBrush.ViewboxUnits = BrushMappingMode.RelativeToBoundingBox;
            visualBrush.Viewbox = new Rect(0, 0, 1, 1);
            //visualBrush.Transform = (Transform)content.LayoutTransform.Inverse;
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


       

        private void ViewPortThumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {

            Debug.WriteLine(e.HorizontalChange + ", " + e.VerticalChange);

            double offsetX = (e.HorizontalChange / this.PreviewBorder.ActualWidth) * this.View.ExtentWidth + this.View.HorizontalOffset; 
            offsetX = Math.Max(0, offsetX);
            offsetX = Math.Min(offsetX, this.View.ExtentWidth - this.View.ViewportWidth);

            double offsetY = (e.VerticalChange / this.PreviewBorder.ActualHeight) * this.View.ExtentHeight + this.View.VerticalOffset;
            offsetY = Math.Max(0, offsetY);
            offsetY = Math.Min(offsetY, this.View.ExtentHeight - this.View.ViewportHeight);

            this.View.ScrollToHorizontalOffset(offsetX);
            this.View.ScrollToVerticalOffset(offsetY);

            

        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.View != null)
            {
                this.BindingView(this.View);
            }
        }

        private void PreviewBorder_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point center = e.GetPosition(this.PreviewBorder);
            double x = center.X - this.ViewPortThumb.Width / 2;
            double y = center.Y - this.ViewPortThumb.Height / 2;

            if (x < 0)
            {
                x = 0;
            }
            if (y < 0)
            {
                y = 0;
            }

            double offsetX = x / this._scale;
            double offsetY = y / this._scale;

            this.View.ScrollToHorizontalOffset(offsetX);
            this.View.ScrollToVerticalOffset(offsetY);
 

        }





        
        
    }
}
