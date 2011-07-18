using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows;

namespace Fantasy.AddIns
{
    [ContentProperty("Codons")]
    public class Extension : DependencyObject
    {
        public Extension()
        {
            this.SetValue(CodonsProperty, new List<ICodon>());
        }

        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }

        public ICondition Conditional
        {
            get { return (ICondition)GetValue(ConditionalProperty); }
            set { SetValue(ConditionalProperty, value); }
        }


        public IList<ICodon> Codons
        {
            get { return (IList<ICodon>)GetValue(CodonsProperty.DependencyProperty); }
        }

        // Using a DependencyProperty as the backing store for Codons.  This enables animation, styling, binding, etc...
        public static readonly DependencyPropertyKey CodonsProperty =
            DependencyProperty.RegisterReadOnly("Codons", typeof(IList<ICodon>), typeof(Extension), new PropertyMetadata(new List<ICodon>()));



        // Using a DependencyProperty as the backing store for Conditional.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConditionalProperty =
            DependencyProperty.Register("Conditional", typeof(ICondition), typeof(Extension), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for Path.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PathProperty =
            DependencyProperty.Register("Path", typeof(string), typeof(Extension), new PropertyMetadata(null));


    }
}
