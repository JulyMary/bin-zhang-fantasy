using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Windows;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;
using NHibernate;
using Fantasy.Studio.Controls;
using System.ComponentModel;

namespace Fantasy.Studio.BusinessEngine.ClassEditing
{
    class BusinessClassPikcerModel : NotifyPropertyChangedObject, IObjectWithSite
    {
        public BusinessClassPikcerModel(IServiceProvider site)
        {
            this.Site = site;
            this.TreeViewModel = new ExtendableTreeViewModel("fantasy/studio/businessengine/businessclasspicker/treeview", this, site);
            BusinessPackage rootPackage = this.Site.GetRequiredService<IEntityService>().GetRootPackage();
            this.TreeViewModel.Items.Add(rootPackage);
        }
       


        public ExtendableTreeViewModel TreeViewModel
        {
            get;
            private set;

        }

        private BusinessClass _selectedItem;

        public BusinessClass SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != value)
                {
                    SelectedItemChangeingEventArgs e = new SelectedItemChangeingEventArgs(value);
                    this.OnSelectedItemChanging(e);
                    if (!e.Cancel)
                    {
                        _selectedItem = value;

                        this.OnPropertyChanged("SelectedItem");
                    }
                    else if(this._selectedItem != null)
                    {
                        this._selectedItem = null;
                        this.OnPropertyChanged("SelectedItem");
                    }
                    
                    
                }
            }
        }


        public event EventHandler<SelectedItemChangeingEventArgs> SelectedItemChanging;

        protected virtual void OnSelectedItemChanging(SelectedItemChangeingEventArgs e)
        {
            if (this.SelectedItemChanging != null)
            {
                this.SelectedItemChanging(this, e);
            }
        }


        public IServiceProvider Site { get; set; }


        public class SelectedItemChangeingEventArgs : CancelEventArgs
        {
            internal SelectedItemChangeingEventArgs(BusinessClass @class)
            {
                this.Class = @class;
            }

           
            public BusinessClass Class { get; private set; }


        }
        
    }
}
