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

namespace Fantasy.Studio.BusinessEngine.UserRoleEditing
{
    class BusinessUserPikcerModel : NotifyPropertyChangedObject, IObjectWithSite
    {
        public BusinessUserPikcerModel(IServiceProvider site)
        {
            this.Site = site;
            this.TreeViewModel = new ExtendableTreeViewModel("fantasy/studio/businessengine/businessuserpicker/treeview", this, site);
            BusinessPackage rootPackage = this.Site.GetRequiredService<IEntityService>().GetRootPackage();
            this.TreeViewModel.Items.Add(rootPackage);
        }
       
        public ExtendableTreeViewModel TreeViewModel
        {
            get;
            private set;

        }

        private BusinessUser _selectedItem;

        public BusinessUser SelectedItem
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
            internal SelectedItemChangeingEventArgs(BusinessUser user)
            {
                this.User = user;
            }


            public BusinessUser User { get; private set; }


        }
        
    }
}
