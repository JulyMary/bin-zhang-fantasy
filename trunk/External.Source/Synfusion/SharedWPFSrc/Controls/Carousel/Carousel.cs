using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;
using System.Windows.Media.Media3D;
using Syncfusion.Licensing;

namespace Syncfusion.Windows.Shared
{
    public class Carousel : ItemsControl
    {
        /// <summary>
        /// Occurs when [selection changed].
        /// </summary>
        public event PropertyChangedCallback SelectionChanged;
        /// <summary>
        /// Occurs when [selected index changed].
        /// </summary>
        public event PropertyChangedCallback SelectedIndexChanged;
        /// <summary>
        /// Occurs when [selected value changed].
        /// </summary>
        public event PropertyChangedCallback SelectedValueChanged;
        /// <summary>
        /// Gets or sets the items per page.
        /// </summary>
        /// <value>The items per page.</value>
        public int ItemsPerPage
        {
            get { return (int)GetValue(ItemsPerPageProperty); }
            set { SetValue(ItemsPerPageProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ItemsPerPageProperty =
            DependencyProperty.Register("ItemsPerPage", typeof(int), typeof(Carousel), new PropertyMetadata(-1, (s, a) => ((Carousel)s).OnItemsPerPageChanged(s)));

        /// <summary>
        /// Called when [items per page changed].
        /// </summary>
        /// <param name="obj">The obj.</param>
        protected void OnItemsPerPageChanged(DependencyObject obj)
        {
            if (this.ItemsHost != null)
            {
                CarouselPanel pane = (CarouselPanel)this.ItemsHost;
                pane.ItemsPerPage = this.ItemsPerPage;
            }
        }

        /// <summary>
        /// Gets or sets the scale fractions.
        /// </summary>
        /// <value>The scale fractions.</value>
        public PathFractionCollection ScaleFractions
        {
            get { return (PathFractionCollection)GetValue(ScaleFractionsProperty); }
            set { SetValue(ScaleFractionsProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ScaleFractionsProperty =
            DependencyProperty.Register("ScaleFractions", typeof(PathFractionCollection), typeof(Carousel), new PropertyMetadata(null, (s, a) => ((Carousel)s).OnItemsVisualChanged(s)));

        /// <summary>
        /// Gets or sets a value indicating whether [scaling enabled].
        /// </summary>
        /// <value><c>true</c> if [scaling enabled]; otherwise, <c>false</c>.</value>
        public bool ScalingEnabled
        {
            get { return (bool)GetValue(ScalingEnabledProperty); }
            set { SetValue(ScalingEnabledProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ScalingEnabledProperty =
            DependencyProperty.Register("ScalingEnabled", typeof(bool), typeof(Carousel), new PropertyMetadata(true, (s, a) => ((Carousel)s).OnItemsVisualChanged(s)));

        /// <summary>
        /// Gets or sets a value indicating whether [opacity enabled].
        /// </summary>
        /// <value><c>true</c> if [opacity enabled]; otherwise, <c>false</c>.</value>
        public bool OpacityEnabled
        {
            get { return (bool)GetValue(OpacityEnabledProperty); }
            set { SetValue(OpacityEnabledProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty OpacityEnabledProperty =
            DependencyProperty.Register("OpacityEnabled", typeof(bool), typeof(Carousel), new PropertyMetadata(true, (s, a) => ((Carousel)s).OnItemsVisualChanged(s)));

        /// <summary>
        /// Gets or sets the opacity fractions.
        /// </summary>
        /// <value>The opacity fractions.</value>
        public PathFractionCollection OpacityFractions
        {
            get { return (PathFractionCollection)GetValue(OpacityFractionsProperty); }
            set { SetValue(OpacityFractionsProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty OpacityFractionsProperty =
            DependencyProperty.Register("OpacityFractions", typeof(PathFractionCollection), typeof(Carousel), new PropertyMetadata(null, (s, a) => ((Carousel)s).OnItemsVisualChanged(s)));

        /// <summary>
        /// Gets or sets a value indicating whether [skew angle X enabled].
        /// </summary>
        /// <value><c>true</c> if [skew angle X enabled]; otherwise, <c>false</c>.</value>
        public bool SkewAngleXEnabled
        {
            get { return (bool)GetValue(SkewAngleXEnabledProperty); }
            set { SetValue(SkewAngleXEnabledProperty, value); }
        }

        public static readonly DependencyProperty SkewAngleXEnabledProperty =
            DependencyProperty.Register("SkewAngleXEnabled", typeof(bool), typeof(Carousel), new PropertyMetadata(false, (s, a) => ((Carousel)s).OnItemsVisualChanged(s)));

        /// <summary>
        /// Gets or sets the skew angle X fractions.
        /// </summary>
        /// <value>The skew angle X fractions.</value>
        public PathFractionCollection SkewAngleXFractions
        {
            get { return (PathFractionCollection)GetValue(SkewAngleXFractionsProperty); }
            set { SetValue(SkewAngleXFractionsProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SkewAngleXFractionsProperty =
            DependencyProperty.Register("SkewAngleXFractions", typeof(PathFractionCollection), typeof(Carousel), new PropertyMetadata(null, (s, a) => ((Carousel)s).OnItemsVisualChanged(s)));

        /// <summary>
        /// Gets or sets a value indicating whether [skew angle Y enabled].
        /// </summary>
        /// <value><c>true</c> if [skew angle Y enabled]; otherwise, <c>false</c>.</value>
        public bool SkewAngleYEnabled
        {
            get { return (bool)GetValue(SkewAngleYEnabledProperty); }
            set { SetValue(SkewAngleYEnabledProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SkewAngleYEnabledProperty =
            DependencyProperty.Register("SkewAngleYEnabled", typeof(bool), typeof(Carousel), new PropertyMetadata(false, (s, a) => ((Carousel)s).OnItemsVisualChanged(s)));

        /// <summary>
        /// Gets or sets the skew angle Y fractions.
        /// </summary>
        /// <value>The skew angle Y fractions.</value>
        public PathFractionCollection SkewAngleYFractions
        {
            get { return (PathFractionCollection)GetValue(SkewAngleYFractionsProperty); }
            set { SetValue(SkewAngleYFractionsProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SkewAngleYFractionsProperty =
            DependencyProperty.Register("SkewAngleYFractions", typeof(PathFractionCollection), typeof(Carousel), new UIPropertyMetadata(null, (s, a) => ((Carousel)s).OnItemsVisualChanged(s)));

        /// <summary>
        /// 
        /// </summary>
        private bool IsVisualChanged = false;
        /// <summary>
        /// Called when [items visual changed].
        /// </summary>
        /// <param name="obj">The obj.</param>
        protected void OnItemsVisualChanged(DependencyObject obj)
        {
            if (this.ItemsHost != null)
            {
                this.SetVisualProperties();
                this.ItemsHost.InvalidateMeasure();
            }
            else
            {
                IsVisualChanged = true;
            }
        }

        /// <summary>
        /// Sets the visual properties.
        /// </summary>
        private void SetVisualProperties()
        {
            CarouselPanel pane = (CarouselPanel)this.ItemsHost;
            if (pane != null)
            {
                pane.ScalingEnabled = this.ScalingEnabled;
                pane.ScaleFractions = this.ScaleFractions;

                pane.OpacityEnabled = this.OpacityEnabled;
                pane.OpacityFractions = this.OpacityFractions;

                pane.SkewAngleXEnabled = this.SkewAngleXEnabled;
                pane.SkewAngleXFractions = this.SkewAngleXFractions;

                pane.SkewAngleYEnabled = this.SkewAngleYEnabled;
                pane.SkewAngleYFractions = this.SkewAngleYFractions;
            }
        }

        /// <summary>
        /// Gets or sets the top item position.
        /// </summary>
        /// <value>The top item position.</value>
        public double TopItemPosition
        {
            get { return (double)GetValue(TopItemPositionProperty); }
            set { SetValue(TopItemPositionProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty TopItemPositionProperty =
            DependencyProperty.Register("TopItemPosition", typeof(double), typeof(Carousel), new PropertyMetadata(0.5, (s, a) => ((Carousel)s).OnTopItemPositionChanged(s)));

        /// <summary>
        /// Called when [top item position changed].
        /// </summary>
        /// <param name="obj">The obj.</param>
        protected void OnTopItemPositionChanged(DependencyObject obj)
        {
            if (this.ItemsHost != null)
            {
                CarouselPanel panel = (CarouselPanel)this.ItemsHost;
                if (panel != null)
                {
                    panel.TopItemPosition = this.TopItemPosition;
                    panel.InvalidateMeasure();
                }
            }
        }

        private Path _Path;
        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>The path.</value>
        public Path Path
        {
            get { return _Path; }
            set
            {
                _Path = value;
                OnPathChanged();
            }
        }

        /// <summary>
        /// Called when [path changed].
        /// </summary>
        protected void OnPathChanged()
        {
            if (this.ItemsHost != null)
            {
                CarouselPanel panel = (CarouselPanel)this.ItemsHost;
                if (panel != null)
                {
                    panel.Path = this.Path;
                    panel.Invalidate(false);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Carousel"/> class.
        /// </summary>
        public Carousel()
        {
            if (EnvironmentTest.IsSecurityGranted)
            {
                EnvironmentTest.StartValidateLicense(typeof(Carousel));
            }
            DefaultStyleKey = typeof(Carousel);
            LayoutUpdated += new EventHandler(Carousel_LayoutUpdated);
        }

#if WPF
        /// <summary>
        /// Initializes the <see cref="Carousel"/> class.
        /// </summary>
        static Carousel()
        {
            EnvironmentTest.ValidateLicense(typeof(Carousel));
        }
#endif
        Panel _itemsHost;
        /// <summary>
        /// Get the current ItemsHost (FlowPanel)
        /// </summary>
        /// <value>The items host.</value>
        private Panel ItemsHost
        {
            get
            {
                if (_itemsHost == null && ItemContainerGenerator != null)
                {
                    _itemsHost = VisualUtils.GetItemsPanel(this, typeof(CarouselPanel));
                }
                return _itemsHost;
            }
        }

        /// <summary>
        /// Handles the LayoutUpdated event of the Carousel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void Carousel_LayoutUpdated(object sender, EventArgs e)
        {
            if (this.ItemsHost != null)
            {
                if (this.IsVisualChanged)
                {
                    this.SetVisualProperties();
                    this.InvalidateMeasure();
                    this.IsVisualChanged = false;
                }

                CarouselPanel panel = (CarouselPanel)this.ItemsHost;
                if (panel != null)
                {
                    if (this.Path != null)
                    {
                        panel.Path = this.Path;
                    }
                    if (this.TopItemPosition != panel.TopItemPosition)
                    {
                        panel.TopItemPosition = this.TopItemPosition;
                    }
                    if (panel.ItemsPerPage <= 0)
                    {
                        if (this.ItemsPerPage == -1 || (this.Items.Count <= this.ItemsPerPage && this.Items.Count > 0))
                        {
                            panel.ItemsPerPage = this.Items.Count;
                        }
                        else
                        {
                            panel.ItemsPerPage = this.ItemsPerPage;
                        }
                    }
                }
                LayoutUpdated -= Carousel_LayoutUpdated;
            }
        }
        
        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>The selected item.</value>
        public CarouselItem SelectedItem
        {
            get { return (CarouselItem)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(CarouselItem), typeof(Carousel), new PropertyMetadata(null, OnSelectedItemChanged));

        /// <summary>
        /// Called when [selected item changed].
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnSelectedItemChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            Carousel carousel = (Carousel)obj;
            if (carousel != null)
            {
                carousel.OnSelectedItemChanged(args);
            }
        }

        /// <summary>
        /// Called when [selected item changed].
        /// </summary>
        protected void OnSelectedItemChanged(DependencyPropertyChangedEventArgs args)
        {
            if (args.OldValue != null)
            {
                CarouselItem item = args.OldValue as CarouselItem;
                item.IsSelected = false;
            }
            if (this.SelectedItem == null)
            {
                SelectedIndex = -1;
                SelectedValue = null;
            }
            else
            {
                if (this.ItemsSource != null)
                {
                    if (Items.Contains(this.SelectedItem.Content))
                    {
                        SelectedIndex = Items.IndexOf(SelectedItem.Content);
                        SelectedValue = SelectedItem.Content;
                    }
                    else
                    {
                        SelectedIndex = -1;
                        SelectedValue = null;
                    }
                }
                else if (this.Items.Count > 0)
                {
                    if (this.Items[0].GetType() == typeof(CarouselItem))
                    {
                        if (Items.Contains(this.SelectedItem))
                        {
                            SelectedIndex = Items.IndexOf(SelectedItem);
                            SelectedValue = SelectedItem;
                        }
                        else
                        {
                            SelectedIndex = -1;
                            SelectedValue = null;
                        }
                    }
                    else
                    {
                        if (Items.Contains(this.SelectedItem.Content))
                        {
                            SelectedIndex = Items.IndexOf(SelectedItem.Content);
                            SelectedValue = SelectedItem.Content;
                        }
                        else
                        {
                            SelectedIndex = -1;
                            SelectedValue = null;
                        }
                    }
                }
            }

            if (SelectionChanged != null)
            {
                SelectionChanged(this, args);
            }
  
            if (this.ItemsHost != null)
            {
                ((CarouselPanel)this.ItemsHost).BringItemIntoView((UIElement)this.SelectedItem, true);
            }
        }

        /// <summary>
        /// Gets or sets the selected value.
        /// </summary>
        /// <value>The selected value.</value>
        public object SelectedValue
        {
            get { return (object)GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SelectedValueProperty =
            DependencyProperty.Register("SelectedValue", typeof(object), typeof(Carousel), new PropertyMetadata(null, (s, a) => ((Carousel)s).OnSelectedValueChanged(a)));

        protected void OnSelectedValueChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.SelectedValueChanged != null)
                this.SelectedValueChanged(this, args);
        }

        /// <summary>
        /// Gets or sets the index of the selected.
        /// </summary>
        /// <value>The index of the selected.</value>
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(Carousel), new PropertyMetadata(-1, (s, a) => ((Carousel)s).OnSelectedIndexChanged(a)));

        /// <summary>
        /// Raises the <see cref="E:SelectedIndexChanged"/> event.
        /// </summary>
        /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnSelectedIndexChanged(DependencyPropertyChangedEventArgs args)
        {
            if (SelectedIndex == -1)
            {
                SelectedItem = null;
            }
            else
            {
                var selectedContainer = ItemContainerGenerator.ContainerFromIndex(SelectedIndex) as CarouselItem;
                if (selectedContainer != SelectedItem)
                {
                    this.SelectedItem = selectedContainer;
                }
            }
            if (this.SelectedIndexChanged != null)
            {
                this.SelectedIndexChanged(this, args);
            }
        }

        #region Overrides

        /// <summary>
        /// Determines if the specified item is (or is eligible to be) its own container.
        /// </summary>
        /// <param name="item">The item to check.</param>
        /// <returns>
        /// true if the item is (or is eligible to be) its own container; otherwise, false.
        /// </returns>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is CarouselItem;
        }

        /// <summary>
        /// Creates or identifies the element that is used to display the given item.
        /// </summary>
        /// <returns>
        /// The element that is used to display the given item.
        /// </returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new CarouselItem();
        }

        /// <summary>
        /// Prepares the specified element to display the specified item.
        /// </summary>
        /// <param name="element">Element used to display the specified item.</param>
        /// <param name="item">Specified item.</param>
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            var ele = element as CarouselItem;
            if (ele != null)
            {
                ele.Owner = this;
                ele.ItemContent = item;
                if (SelectedItem == ele)
                {
                    ele.IsSelected = true;
                }
            }
            base.PrepareContainerForItemOverride(element, item);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        #endregion
    }
}
