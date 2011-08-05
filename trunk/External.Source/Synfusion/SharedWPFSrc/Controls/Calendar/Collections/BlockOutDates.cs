using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Markup;
using System.ComponentModel;

namespace Syncfusion.Windows.Shared
{
    public class BlackoutDatesRange 

    {


        
        public DateTime StartDate
        {
            get;
            set;
        }

        public DateTime EndDate
        {
            get;
            set;
        }

        

       // public event PropertyChangedEventHandler PropertyChanged;

       // public event PropertyChangedEventHandler PropertyChanged;

       
    }

       
       [ContentPropertyAttribute("BlockoutDatesRange")]
        public class BlackDatesCollection : ObservableCollection<BlackoutDatesRange>
        {
           protected override void OnCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
           {
               base.OnCollectionChanged(e);
           }
           
        }
    
}
