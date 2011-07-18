using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows;

namespace Fantasy.AddIns
{
    [ContentProperty("Codons")]
    public abstract class CodonBase : DependencyObject, ICodon
    {

        public CodonBase()
        {
            this.SetValue(CodonsProperty, new List<ICodon>());
        }

        #region ICodon Members

       
       
        public abstract object BuildItem(object owner, System.Collections.IEnumerable subItems, ConditionCollection condition);

    
        public ICondition Conditional
        {
            get { return (ICondition)GetValue(ConditionalProperty); }
            set { SetValue(ConditionalProperty, value); }
        }



        public virtual bool HandleCondition
        {
            get
            {
                return false;
            }
        }



        public string ID
        {
            get { return (string)GetValue(IDProperty); }
            set { SetValue(IDProperty, value); }
        }

        public string InsertAfter
        {
            get { return (string)GetValue(InsertAfterProperty); }
            set { SetValue(InsertAfterProperty, value); }
        }

        public string InsertBefore
        {
            get { return (string)GetValue(InsertBeforeProperty); }
            set { SetValue(InsertBeforeProperty, value); }
        }




        public IList<ICodon> Codons
        {
            get { return (IList<ICodon>)GetValue(CodonsProperty.DependencyProperty); }
          
        }

        // Using a DependencyProperty as the backing store for Codons.  This enables animation, styling, binding, etc...
        public static readonly DependencyPropertyKey CodonsProperty =
            DependencyProperty.RegisterReadOnly("Codons", typeof(IList<ICodon>), typeof(CodonBase), new UIPropertyMetadata(new List<ICodon>()));



        // Using a DependencyProperty as the backing store for Codons.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty CodonsProperty =
        //    DependencyProperty.Register("Codons", typeof(IList<ICodon>), typeof(CodonBase), new PropertyMetadata(new List<ICodon>()));


        // Using a DependencyProperty as the backing store for InsertBefore.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InsertBeforeProperty =
            DependencyProperty.Register("InsertBefore", typeof(string), typeof(CodonBase), new PropertyMetadata(null));



        // Using a DependencyProperty as the backing store for InsertAfter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InsertAfterProperty =
            DependencyProperty.Register("InsertAfter", typeof(string), typeof(CodonBase), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for ID.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IDProperty =
            DependencyProperty.Register("ID", typeof(string), typeof(CodonBase), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for Conditional.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConditionalProperty =
            DependencyProperty.Register("Conditional", typeof(ICondition), typeof(CodonBase), new PropertyMetadata(null));

        #endregion
    }
}
