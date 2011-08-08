using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Windows;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;
using NHibernate;
using Fantasy.Studio.TreeViewModel;

namespace Fantasy.Studio.BusinessEngine.PropertyEditing
{
    class BusinessDataTypePikcerModel : NotifyPropertyChangedObject, IObjectWithSite
    {
        public BusinessDataTypePikcerModel(IServiceProvider site)
        {
            this.Site = site;
            this.TreeViewModel = new ExtendableTreeViewModel("fantasy/studio/businessengine/businessdatatypepicker/treeview", this, site);
            IBusinessDataTypeRepository dr = this.Site.GetRequiredService<IBusinessDataTypeRepository>();
            foreach (BusinessDataType t in dr.All)
            {
                if (t != dr.Class && t != dr.Enum)
                {
                    this.TreeViewModel.Items.Add(t);
                }
            }

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
