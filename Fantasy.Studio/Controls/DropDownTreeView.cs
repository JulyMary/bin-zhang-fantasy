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

namespace Fantasy.Studio.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Fantasy.Studio.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Fantasy.Studio.Controls;assembly=Fantasy.Studio.Controls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:DropDownTreeView/>
    ///
    /// </summary>
    public class DropDownTreeView : TreeView
    {
        static DropDownTreeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DropDownTreeView), new FrameworkPropertyMetadata(typeof(DropDownTreeView)));
        }





        public DataTemplate SelectionItemTemplate
        {
            get { return (DataTemplate)GetValue(SelectionItemTemplateProperty); }
            set { SetValue(SelectionItemTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectionItemTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectionItemTemplateProperty =
            DependencyProperty.Register("SelectionItemTemplate", typeof(DataTemplate), typeof(DropDownTreeView), new UIPropertyMetadata(null
                ));




        public DataTemplateSelector SelectionItemTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(SelectionItemTemplateSelectorProperty); }
            set { SetValue(SelectionItemTemplateSelectorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectionItemTemplateSelector.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectionItemTemplateSelectorProperty =
            DependencyProperty.Register("SelectionItemTemplateSelector", typeof(DataTemplateSelector), typeof(DropDownTreeView), new UIPropertyMetadata(null));




        public string SelectionItemStringFormat
        {
            get { return (string)GetValue(SelectionItemStringFormatProperty); }
            set { SetValue(SelectionItemStringFormatProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectionItemStringFormat.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectionItemStringFormatProperty =
            DependencyProperty.Register("SelectionItemStringFormat", typeof(string), typeof(DropDownTreeView), new UIPropertyMetadata(null));





        





        

    }
}
