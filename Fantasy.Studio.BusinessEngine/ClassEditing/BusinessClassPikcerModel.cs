using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Windows;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;
using NHibernate;
using Fantasy.Studio.Controls;

namespace Fantasy.Studio.BusinessEngine.ClassEditing
{
    class BusinessClassPikcerModel : NotifyPropertyChangedObject, IObjectWithSite
    {
        public BusinessClassPikcerModel(IServiceProvider site)
        {
            this.Site = site;
            this.TreeViewModel = new ExtendableTreeViewModel("fantasy/studio/businessengine/businessclasspicker/treeview", this, site);
            BusinessPackage rootPackage = this.Site.GetRequiredService<IEntityService>().DefaultSession.Get<BusinessPackage>(BusinessPackage.RootPackageId);
            this.TreeViewModel.Items.Add(rootPackage);
        }
       


        public ExtendableTreeViewModel TreeViewModel
        {
            get;
            private set;

        }

        private IBusinessEntity _selectedItem;

        public IBusinessEntity SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;

                    this.OnPropertyChanged("SelectedItem");

                    this.IsOKEnabled = _selectedItem != null && !(_selectedItem is BusinessPackage);
                }
            }
        }

        private bool _isOKEnabled = false;

        public bool IsOKEnabled
        {
            get { return _isOKEnabled; }
            private set
            {
                if (_isOKEnabled != value)
                {
                    _isOKEnabled = value;
                    this.OnPropertyChanged("IsOKEnabled");
                }
            }
        }

        public IServiceProvider Site { get; set; }
        
    }
}
