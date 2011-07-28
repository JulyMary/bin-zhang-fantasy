using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Windows;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;
using NHibernate;

namespace Fantasy.Studio.BusinessEngine
{
    class BusinessDataTypePikcerModel : NotifyPropertyChangedObject, IObjectWithSite
    {

        public IEnumerable<IBusinessEntity> Items
        {
            get
            {
                foreach (BusinessDataType dt in this.Site.GetRequiredService<IBusinessDataTypeRepository>().All)
                {
                    if (!dt.IsBusinessClass && !dt.IsEnum)
                    {
                        yield return dt;
                    }
                }

                IEntityService es = this.Site.GetRequiredService<IEntityService>();
                ISession session = es.DefaultSession;
                BusinessPackage root = session.Get<BusinessPackage>(BusinessPackage.RootPackageId);
                yield return root; 
            }
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
