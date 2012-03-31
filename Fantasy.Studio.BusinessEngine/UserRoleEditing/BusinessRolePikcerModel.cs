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
    class BusinessRolePikcerModel : NotifyPropertyChangedObject, IObjectWithSite
    {
        public BusinessRolePikcerModel(IServiceProvider site, bool noComputed)
        {
            this.Site = site;
            this.TreeViewModel = new ExtendableTreeViewModel(noComputed ? "fantasy/studio/businessengine/businessrolepicker/nocomputedtreeview" : "fantasy/studio/businessengine/businessrolepicker/treeview", 
                this, site);
            BusinessPackage rootPackage = this.Site.GetRequiredService<IEntityService>().GetRootPackage();
            this.TreeViewModel.Items.Add(rootPackage);
        }
       
        public ExtendableTreeViewModel TreeViewModel
        {
            get;
            private set;

        }

        private BusinessRoleData _selectedItem;

        public BusinessRoleData SelectedItem
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
            internal SelectedItemChangeingEventArgs(BusinessRoleData role)
            {
                this.Role = role;
            }


            public BusinessRoleData Role { get; private set; }


        }
        
    }
}
