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

namespace Syncfusion.Windows.Controls
{
    public class TemplateSelector : ContentControl
    {

        public TemplateSelector()
        {
            //this.Loaded+=new RoutedEventHandler(TemplateSelector_Loaded);
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);            
            if (newContent != null)
            {
                try
                {
                    ContentTemplate = SelectTemplate(newContent);
                }
                catch(NullReferenceException)
                {
                    throw new NullReferenceException("Data Template should not be null");
                }
            }
            
        }

        public virtual DataTemplate SelectTemplate(object item)
        {
            return new DataTemplate();
        }
    }
}
