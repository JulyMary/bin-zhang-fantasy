// <copyright file="DiagramPrintDialog.xaml.cs" company="Syncfusion">
// Copyright Syncfusion Inc. 2001 - 2011. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
// </copyright>

using System;
using System.Collections.Generic;
using System.Printing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Xps;
using System.ComponentModel;

namespace Syncfusion.Windows.Diagram
{
    /// <summary>
    /// Interaction logic for DiagramPrintDialog.xaml
    /// </summary>
    /// <exclude/>
    #if SyncfusionFramework4_0
    [DesignTimeVisible(false)]
#endif
    public partial class DiagramPrintDialog : Window
    {
        #region Members
        /// <summary>
        /// Used to store the element to be printed.
        /// </summary>
        private FrameworkElement m_elementToPrint;
       
        /// <summary>
        /// Represents Print dialog
        /// </summary>
        private PrintDialog m_nativePrintDialog = new PrintDialog();
       
        /// <summary>
        /// Represents the Visual brush
        /// </summary>
        private VisualBrush m_visualBrush;
        #endregion

        #region Dependency properties

        /// <summary>
        /// Using a DependencyProperty as the backing store for PrintStrech.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty PrintStretchProperty =
                DependencyProperty.Register("PrintStretch", typeof(Stretch), typeof(DiagramPrintDialog), new FrameworkPropertyMetadata(Stretch.Uniform, new PropertyChangedCallback(OnPrintStretchChanged)));
       
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the print stretch.
        /// </summary>
        /// <value>The print stretch.</value>
        public Stretch PrintStretch
        {
            get
            {
                return (Stretch)GetValue(PrintStretchProperty);
            }

            set
            {
                SetValue(PrintStretchProperty, value);
            }
        }
        #endregion

        #region Events
       
        /// <summary>
        /// Event that is raised when PrintStretch property is changed.
        /// </summary>
        public event PropertyChangedCallback PrintStretchChanged;
       
        #endregion

        #region Constructor
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DiagramPrintDialog"/> class.
        /// </summary>
        public DiagramPrintDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Public methods
        /// <summary>
        /// Shows the print dialog.
        /// </summary>
        /// <param name="element">The element to be printed.</param>
        /// <returns>The boolean value indicating the Dialog box is shown or not.</returns>
        public bool? ShowPrintDialog(FrameworkElement element)
        {
            m_elementToPrint = element;

            VisualBrush visualBrush = new VisualBrush(element);
            visualBrush.Stretch = Stretch.Uniform;
            visualBrush.ViewboxUnits = BrushMappingMode.Absolute;
            visualBrush.Viewbox = new Rect(0, 0, m_elementToPrint.ActualWidth, m_elementToPrint.ActualHeight);

            m_visualBrush = visualBrush;
            this.PreviewRect.Fill = visualBrush;

            return this.ShowDialog();
        }

        /// <summary>
        /// Shows the print dialog.
        /// </summary>
        /// <param name="element">The element to be printed.</param>
        /// <param name="printArea">The print area.</param>
        /// <returns> /// <returns>The boolean value indicating the Dialog box is shown or not.</returns></returns>
        public bool? ShowPrintDialog(FrameworkElement element, Rect printArea)
        {
            m_elementToPrint = element;
            VisualBrush visualBrush = new VisualBrush(element);
            visualBrush.Stretch = Stretch.Uniform;
            visualBrush.ViewboxUnits = BrushMappingMode.Absolute;
            visualBrush.Viewbox = printArea;
            m_visualBrush = visualBrush;
            this.PreviewRect.Fill = visualBrush;
            return this.ShowDialog();
        }

        /// <summary>
        /// Prints the Diagram Page Directly.
        /// </summary>
        /// <param name="element">The element to be printed.</param>
        /// <param name="s">The s is a Stretch options.</param>        
        internal void Print(FrameworkElement element, Stretch s)
        {
            m_elementToPrint = element;
            VisualBrush visualBrush = new VisualBrush(element);
            visualBrush.Stretch = Stretch.Uniform;
            visualBrush.ViewboxUnits = BrushMappingMode.Absolute;
            visualBrush.Viewbox = new Rect(0, 0, m_elementToPrint.ActualWidth, m_elementToPrint.ActualHeight);
            m_visualBrush = visualBrush;
            this.PrintStretch = s;
            this.PreviewRect.Fill = visualBrush;

            StartPrint();
        }

        #endregion

        #region Implementation
        /// <summary>
        /// Starts the print.
        /// </summary>
        private void StartPrint()
        {
            PrintCapabilities printCapabilities = m_nativePrintDialog.PrintQueue.GetPrintCapabilities(m_nativePrintDialog.PrintTicket);

            Size pageSize = new Size(m_nativePrintDialog.PrintableAreaWidth, m_nativePrintDialog.PrintableAreaHeight);
            Size pageAreaSize = new Size(printCapabilities.PageImageableArea.ExtentWidth, printCapabilities.PageImageableArea.ExtentHeight);
            Rectangle rect = new Rectangle();

            rect.Fill = m_visualBrush;

            SetViewport(m_visualBrush, pageAreaSize);
            rect.Arrange(new Rect(new Point(0, 0), pageAreaSize));

            XpsDocumentWriter writer = PrintQueue.CreateXpsDocumentWriter(m_nativePrintDialog.PrintQueue);
            writer.Write(rect, m_nativePrintDialog.PrintTicket);

            SetViewport(m_visualBrush, new Size(this.PreviewRect.ActualWidth, this.PreviewRect.ActualHeight));
        }

        /// <summary>
        /// Called when [print click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnPrintClick(object sender, RoutedEventArgs args)
        {
            this.StartPrint();
            this.DialogResult = true;
        }

        /// <summary>
        /// Called when [cancel click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnCancelClick(object sender, RoutedEventArgs args)
        {
            this.DialogResult = false;
        }

        /// <summary>
        /// Called when [color click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnColorClick(object sender, RoutedEventArgs args)
        {
            m_nativePrintDialog.PrintTicket.OutputColor = OutputColor.Color;
        }

        /// <summary>
        /// Called when [black and white click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnBlackAndWhiteClick(object sender, RoutedEventArgs args)
        {
            m_nativePrintDialog.PrintTicket.OutputColor = OutputColor.Monochrome;
        }

        /// <summary>
        /// Called when [advanced click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnAdvancedClick(object sender, RoutedEventArgs args)
        {
            m_nativePrintDialog.ShowDialog();
        }
       
        /// <summary>
        /// Gets the size by specified stretch.
        /// </summary>
        /// <param name="stretch">The stretch.</param>
        /// <param name="viewport">The viewport.</param>
        /// <param name="original">The original.</param>
        /// <returns>The size to be printed.</returns>
        private static Size GetPrintSize(Stretch stretch, Size viewport, Size original)
        {
            Size result = Size.Empty;

            switch (stretch)
            {
                case Stretch.Fill:
                    result = viewport;
                    break;

                case Stretch.None:
                    result = original;
                    break;

                case Stretch.Uniform:
                    {
                        double dx = viewport.Width / original.Width;
                        double dy = viewport.Height / original.Height;

                        if (dx < dy)
                        {
                            result = new Size(viewport.Width, dx * original.Height);
                        }
                        else
                        {
                            result = new Size(dy * original.Width, viewport.Height);
                        }
                    }

                    break;

                case Stretch.UniformToFill:
                    {
                        double dx = viewport.Width / original.Width;
                        double dy = viewport.Height / original.Height;

                        if (dx > dy)
                        {
                            result = new Size(viewport.Width, dx * original.Height);
                        }
                        else
                        {
                            result = new Size(dy * original.Width, viewport.Height);
                        }
                    }

                    break;
            }

            return result;
        }
       
        /// <summary>
        /// Calls OnPrintStretchChanged method of the instance, notifies of the dependency property value changes.
        /// </summary>
        /// <param name="d">Dependency object, the change occurs on.</param>
        /// <param name="e">Property change details, such as old value and new value.</param>
        private static void OnPrintStretchChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramPrintDialog instance = (DiagramPrintDialog)d;
            instance.OnPrintStretchChanged(e);
        }
       
        /// <summary>
        /// Sets the viewport.
        /// </summary>
        /// <param name="brush">The brush.</param>
        /// <param name="size">The size of the viewport.</param>
        private void SetViewport(VisualBrush brush, Size size)
        {
            if (brush == null)
            {
                throw new ArgumentNullException("brush");
            }

            if (this.PrintStretch == Stretch.Uniform)
            {
                double coefficientHeight = size.Height / brush.Viewbox.Height;
                double coefficientWidth = size.Width / brush.Viewbox.Width;

                if (coefficientHeight < coefficientWidth)
                {
                    double width = coefficientHeight * brush.Viewbox.Width / size.Width;
                    double x = (1 - width) / 2;
                    brush.Viewport = new Rect(new Point(x, 0), new Size(width, 1));
                }
                else if (coefficientHeight > coefficientWidth)
                {
                    double height = coefficientWidth * brush.Viewbox.Height / size.Height;
                    double y = (1 - height) / 2;
                    brush.Viewport = new Rect(new Point(0, y), new Size(1, height));
                }
            }
            else if (this.PrintStretch == Stretch.None)
            {
                if (size.Width > brush.Viewbox.Width || size.Height > brush.Viewbox.Height)
                {
                    double coefficientHeight = size.Width - brush.Viewbox.Width;
                    double coefficientWidth = size.Height - brush.Viewbox.Height;
                    double width = brush.Viewbox.Width / size.Width;
                    double height = brush.Viewbox.Height / size.Height;
                    double x = (1 - width) / 2;
                    double y = (1 - height) / 2;
                    brush.Viewport = new Rect(new Point(x, y), new Size(width, height));
                }
                else
                {
                    brush.Viewport = new Rect(0, 0, 1, 1);
                }
            }
            else
            {
                brush.Viewport = new Rect(0, 0, 1, 1);
            }
        }
       
        /// <summary>
        /// Updates property value cache and raises PrintStretchChanged event.
        /// </summary>
        /// <param name="e">Property change details, such as old value and new value.</param>
        protected virtual void OnPrintStretchChanged(DependencyPropertyChangedEventArgs e)
        {
            m_visualBrush.Stretch = this.PrintStretch;
            SetViewport(m_visualBrush, new Size(this.PreviewRect.ActualWidth, this.PreviewRect.ActualHeight));

            if (PrintStretchChanged != null)
            {
                PrintStretchChanged(this, e);
            }
        }
        
        /// <summary>
        /// When overridden in a derived class, participates in rendering operations that are directed by the layout system. The rendering instructions for this element are not used directly when this method is invoked, and are instead preserved for later asynchronous use by layout and drawing.
        /// </summary>
        /// <param name="drawingContext">The drawing instructions for a specific element. This context is provided to the layout system.</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            SetViewport(m_visualBrush, new Size(this.PreviewRect.ActualWidth, this.PreviewRect.ActualHeight));
        }
     
        #endregion
    }
}