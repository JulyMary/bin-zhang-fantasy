using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows;
using System.Windows.Controls;
using System.Collections;
using System.Collections.ObjectModel;
using System.Reflection;
using System.ComponentModel;
using Syncfusion.Windows.Shared;
using System.Windows.Input;

namespace Syncfusion.Windows.Tools.Controls
{
    /// <summary>
    /// 
    /// </summary>
#if SILVERLIGHT 
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Blend,
      Type = typeof(ComboBoxAdv), XamlResource = "/Syncfusion.Theming.Blend;component/ComboBoxAdv.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2007Blue,
        Type = typeof(ComboBoxAdv), XamlResource = "/Syncfusion.Theming.Office2007Blue;component/ComboBoxAdv.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2007Black,
        Type = typeof(ComboBoxAdv), XamlResource = "/Syncfusion.Theming.Office2007Black;component/ComboBoxAdv.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2007Silver,
        Type = typeof(ComboBoxAdv), XamlResource = "/Syncfusion.Theming.Office2007Silver;component/ComboBoxAdv.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Default,
        Type = typeof(ComboBoxAdv), XamlResource = "/Syncfusion.Tools.Silverlight;component/Controls/ComboBoxAdv/Themes/generic.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2003,
        Type = typeof(ComboBoxAdv), XamlResource = "/Syncfusion.Theming.Office2003;component/ComboBoxAdv.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2010Blue,
       Type = typeof(ComboBoxAdv), XamlResource = "/Syncfusion.Theming.Office2010Blue;component/ComboBoxAdv.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2010Black,
        Type = typeof(ComboBoxAdv), XamlResource = "/Syncfusion.Theming.Office2010Black;component/ComboBoxAdv.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Office2010Silver,
        Type = typeof(ComboBoxAdv), XamlResource = "/Syncfusion.Theming.Office2010Silver;component/ComboBoxAdv.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.Windows7,
        Type = typeof(ComboBoxAdv), XamlResource = "/Syncfusion.Theming.Windows7;component/ComboBoxAdv.xaml")]
    [Syncfusion.Windows.Shared.SkinType(SkinVisualStyle = Syncfusion.Windows.Shared.VisualStyle.VS2010,
      Type = typeof(ComboBoxAdv), XamlResource = "/Syncfusion.Theming.VS2010;component/ComboBoxAdv.xaml")]

#else
    [SkinType(SkinVisualStyle = Skin.Office2007Blue,
   Type = typeof(ComboBoxAdv), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/ComboBoxAdv/Themes/Office2007BlueStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2007Black,
    Type = typeof(ComboBoxAdv), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/ComboBoxAdv/Themes/Office2007BlackStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2007Silver,
    Type = typeof(ComboBoxAdv), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/ComboBoxAdv/Themes/Office2007SilverStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2010Blue,
    Type = typeof(ComboBoxAdv), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/ComboBoxAdv/Themes/Office2010BlueStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2010Black,
    Type = typeof(ComboBoxAdv), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/ComboBoxAdv/Themes/Office2010BlackStyle.xaml")]
    [SkinType(SkinVisualStyle = Skin.Office2010Silver,
    Type = typeof(ComboBoxAdv), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/ComboBoxAdv/Themes/Office2010SilverStyle.xaml")]
 
    [SkinType(SkinVisualStyle = Skin.Blend,
    Type = typeof(ComboBoxAdv), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/ComboBoxAdv/Themes/BlendStyle.xaml")]

    [SkinType(SkinVisualStyle = Skin.Default,
    Type = typeof(ComboBoxAdv), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/ComboBoxAdv/Themes/Generic.xaml")]
    [SkinType(SkinVisualStyle = Skin.VS2010,
    Type = typeof(ComboBoxAdv), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/ComboBoxAdv/Themes/VS2010Style.xaml")]
#endif
    public class ComboBoxAdv  : ComboBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComboBoxAdv"/> class.
        /// </summary>
        public ComboBoxAdv()
        {
            DefaultStyleKey = typeof(ComboBoxAdv);
            if (SelectedItems == null)
                SelectedItems = new List<object>();           
        }

        
      

        private ItemsControl selectedItems;

        private ContentPresenter selectedContent;

        internal TextBlock defaultText;

        /// <summary>
        /// Determines if the specified item is (or is eligible to be) its own ItemContainer.
        /// </summary>
        /// <param name="item">Specified item.</param>
        /// <returns>
        /// true if the item is its own ItemContainer; otherwise, false.
        /// </returns>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is ComboBoxItemAdv;
        }

        /// <summary>
        /// Creates or identifies the element used to display the specified item.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Windows.Controls.ComboBoxItem"/>.
        /// </returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ComboBoxItemAdv();
        }

      
        /// <summary>
        /// Prepares the specified element to display the specified item.
        /// </summary>
        /// <param name="element">Element used to display the specified item.</param>
        /// <param name="item">Specified item.</param>
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            ComboBoxItemAdv comboItem = element as ComboBoxItemAdv;
            comboItem.ContentTemplate = ItemTemplate;
            if (comboItem.CheckBox != null)
            {
                if (!AllowMultiSelect)
                {
                    comboItem.CheckBox.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    comboItem.CheckBox.Visibility = System.Windows.Visibility.Visible;
                }
            }
#if WPF

            comboItem.ContentTemplateSelector = ItemTemplateSelector;
#endif
            comboItem.Parent = this;
            if (item is ComboBoxItemAdv)
            {
                base.PrepareContainerForItemOverride(comboItem, item);
            }
            else
            {
#if !WPF
                if (DisplayMemberPath == null)
                {
                    comboItem.Content = item;
                }
                else
                {
                    Type type = item.GetType();
                    PropertyInfo propertyInfo = type.GetProperty(DisplayMemberPath, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
                    comboItem.Content = propertyInfo.GetValue(item, null);
                }
#else
                comboItem.Content = item;
#endif
            }
        }


#if WPF


        /// <summary>
        /// Called when the selection changes.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
        }

#endif
        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate"/>.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            selectedItems = GetTemplateChild("PART_SelectedItems") as ItemsControl;
            selectedContent = GetTemplateChild("ContentPresenter") as ContentPresenter;
            defaultText = GetTemplateChild("PART_DefaultText") as TextBlock;
            UpdateSelectionBox();         
            UpdateSelectMode();

        }
        
        internal void UpdateSelectMode()
        {
            if (selectedItems != null && selectedContent != null)
            {
                if (AllowMultiSelect)
                {
                    selectedItems.Visibility = System.Windows.Visibility.Visible;
                    selectedContent.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    selectedItems.Visibility = System.Windows.Visibility.Collapsed;
                    selectedContent.Visibility = System.Windows.Visibility.Visible;
                }
            }
            for (int i = 0; i < Items.Count; i++)
            {
                ComboBoxItemAdv item = ItemContainerGenerator.ContainerFromIndex(i) as ComboBoxItemAdv;
                if (item != null && item.CheckBox != null)
                {
                    if (AllowMultiSelect)
                    {
                        item.CheckBox.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        item.CheckBox.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        internal void UpdateSelectionBox()
        {
            IList selItems = this.SelectedItems as IList;
            if (defaultText != null)
            {
                if (AllowMultiSelect && SelectedItems != null)
                {
                    if (selItems.Count == 0)
                    {
                        defaultText.Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        defaultText.Visibility = System.Windows.Visibility.Collapsed;
                    }
                }
                else
                {
                    if (SelectedItem == null)
                    {
                        defaultText.Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        defaultText.Visibility = System.Windows.Visibility.Collapsed;
                    }
                }
            }

            if (AllowMultiSelect)
            {
                if (selectedItems != null && SelectedItems != null)
                {
                    selectedItems.Items.Clear();
                    for (int i = 0; i < selItems.Count; i++)
                    {
                        ContentControl _content = new ContentControl();
                        _content.ContentTemplate = SelectionBoxTemplate;

                        if (selItems[i] is ComboBoxItemAdv)
                        {
                            if (((ComboBoxItemAdv)selItems[i]).CheckBox != null)
                            {
                                if (!((ComboBoxItemAdv)selItems[i]).CheckBox.IsChecked.Value)
                                {
                                    ((ComboBoxItemAdv)selItems[i]).CheckBox.IsChecked = true;
                                }
                            }
                            _content.Content = ((ComboBoxItemAdv)selItems[i]).Content;
                        }
                        else
                        {
                           if (SelectedValuePath == String.Empty)
                            {
                                _content.Content = selItems[i];
                            }
                            else
                            {
                                Type type = selItems[i].GetType();
                                PropertyInfo propertyInfo = type.GetProperty(SelectedValuePath, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
                                if (null != propertyInfo)
                                {
                                    _content.Content = propertyInfo.GetValue(selItems[i], null);
                                }
                                else
                                {
                                    throw new InvalidOperationException("SelectedValuePath has invalid property name");
                                }
                            }
                        }

                        selectedItems.Items.Add(_content);

                        if (i < selItems.Count - 1)
                        {
                            selectedItems.Items.Add(SelectedValueDelimiter);
                        }
                    }
                }
            }
        }

        protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            UpdateSelectionBox();
        }

#if !WPF
        protected override void OnKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            ComboBoxItemAdv item = e.OriginalSource as ComboBoxItemAdv;
            if (item != null)
            {
                if (e.Key == Key.Up)
                {
                    int index = ItemContainerGenerator.IndexFromContainer(item);

                    if (index == 0)
                        index = 0;
                    else
                    {
                        index = index - 1;
                        VisualStateManager.GoToState(item, "Normal", true);
                    }

                    ComboBoxItemAdv nextitem = ItemContainerGenerator.ContainerFromIndex(index) as ComboBoxItemAdv;
                    SelectedIndex = index;
                    VisualStateManager.GoToState(nextitem, "MouseOver", true);
                }
                else if (e.Key == Key.Down)
                {
                    int index = ItemContainerGenerator.IndexFromContainer(item);

                    if (index == Items.Count - 1)
                        index = Items.Count - 1;
                    else
                    {
                        index = index + 1;
                        VisualStateManager.GoToState(item, "Normal", true);
                    }
                    ComboBoxItemAdv nextitem = ItemContainerGenerator.ContainerFromIndex(index) as ComboBoxItemAdv;
                    SelectedIndex = index;
                    VisualStateManager.GoToState(nextitem, "MouseOver", true);
                }
            }
        }
#endif
       private static void OnAllowMultiSelectChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            ComboBoxAdv instance = sender as ComboBoxAdv;
            if (instance != null)
            {
                instance.UpdateSelectionBox();

                instance.UpdateSelectMode();

            }
        }
        
        private static void OnSelectedValueDelimiterChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            ComboBoxAdv instance = sender as ComboBoxAdv;
            if (instance != null)
            {
                instance.UpdateSelectionBox();

                instance.UpdateSelectMode();

            }
        }
        

#if WPF

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new bool IsEditable
        {
            get { return (bool)GetValue(IsEditableProperty); }
            set { SetValue(IsEditableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsEditable.  This enables animation, styling, binding, etc...
        public new static readonly DependencyProperty IsEditableProperty =
            DependencyProperty.Register("IsEditable", typeof(bool), typeof(ComboBoxAdv), new UIPropertyMetadata(false));

#endif

        /// <summary>
        /// Gets or sets a value indicating whether [allow multi select].
        /// </summary>
        /// <value><c>true</c> if [allow multi select]; otherwise, <c>false</c>.</value>
        public bool AllowMultiSelect
        {
            get { return (bool)GetValue(AllowMultiSelectProperty); }
            set { SetValue(AllowMultiSelectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AllowMultiSelect.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AllowMultiSelectProperty =
            DependencyProperty.Register("AllowMultiSelect", typeof(bool), typeof(ComboBoxAdv), new PropertyMetadata(false, new PropertyChangedCallback(OnAllowMultiSelectChanged)));



        public IEnumerable SelectedItems
        {
            get { return (IEnumerable)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }
        
        // Using a DependencyProperty as the backing store for SelectedItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", typeof(IEnumerable), typeof(ComboBoxAdv), new PropertyMetadata(null, new PropertyChangedCallback(OnSelectedItemsChanged)));
        
        private static void OnSelectedItemsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ComboBoxAdv cboxadv = sender as ComboBoxAdv;
            cboxadv.OnSelectedItemsChanged(args);
            
        }

        internal void OnSelectedItemsChanged(DependencyPropertyChangedEventArgs args)
        {
            IList newSelItems = args.NewValue as IList;
            if (newSelItems != null)
            {
                foreach (var item in newSelItems)
                {
                    if (this.Items.Contains(item))
                    {
                        ComboBoxItemAdv cboxItemAdv = this.ItemContainerGenerator.ContainerFromItem(item) as ComboBoxItemAdv;
                        if (cboxItemAdv != null)
                            cboxItemAdv.IsSelected = true;
                        else
                        {
                            UpdateSelectionBox();
                        }
                    }
                }
            }
        }

        public string SelectedValueDelimiter
        {
            get { return (string)GetValue(SelectedValueDelimiterProperty); }
            set { SetValue(SelectedValueDelimiterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedValueDelimiter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedValueDelimiterProperty =
            DependencyProperty.Register("SelectedValueDelimiter", typeof(string), typeof(ComboBoxAdv), new PropertyMetadata(" - ", new PropertyChangedCallback(OnSelectedValueDelimiterChanged)));



        public DataTemplate SelectionBoxTemplate
        {
            get { return (DataTemplate)GetValue(SelectionBoxTemplateProperty); }
            set { SetValue(SelectionBoxTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectionBoxTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectionBoxTemplateProperty =
            DependencyProperty.Register("SelectionBoxTemplate", typeof(DataTemplate), typeof(ComboBoxAdv), new PropertyMetadata(null));



        public string DefaultText
        {
            get { return (string)GetValue(DefaultTextProperty); }
            set { SetValue(DefaultTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DefaultText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefaultTextProperty =
            DependencyProperty.Register("DefaultText", typeof(string), typeof(ComboBoxAdv), new PropertyMetadata(String.Empty));
        
    }
}
