using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using Fantasy.Windows;
using Fantasy.BusinessEngine.Services;
using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Fantasy.Studio.BusinessEngine.Properties;

namespace Fantasy.Studio.BusinessEngine.MenuItemEditing
{
    class MenuEditorModel : NotifyPropertyChangedObject, IObjectWithSite, IWeakEventListener
    {

        public MenuEditorModel(IServiceProvider services)
        {
            this._entityService = services.GetRequiredService<IEntityService>();

            BusinessMenuItem[] menuitems = this._entityService.Query<BusinessMenuItem>().ToArray();

            this.Root = new BusinessMenuItem[] { menuitems.Single(m => m.Id == BusinessMenuItem.RootId) };

            foreach (BusinessMenuItem item in menuitems)
            {
                if (item.EntityState != EntityState.Clean)
                {
                    this._editingState = Studio.EditingState.Dirty;
                }

                PropertyChangedEventManager.AddListener(item, this, "EntityState");
            }

            this.Refresh();
            this.SetSelectedMenuItem(this.Root[0]);
            
        }


        public void Refresh()
        {
            this._allApplications.Clear();
            var apps = from package in this._entityService.GetRootPackage().Flatten(p=>p.ChildPackages)
                       from app in package.Applications
                       select app;
            this._allApplications.AddRange(apps);
            this._allApplications.SortBy(a => a.Name);

            foreach (BusinessMenuItem item in this.Root[0].Flatten(i => i.ChildItems).Where(i => i.ApplicationId != null))
            {
                if (!this._allApplications.Any(a => a.Id == item.ApplicationId))
                {
                    item.ApplicationId = null;
                }
            }


            var roles = from package in this._entityService.GetRootPackage().Flatten(p => p.ChildPackages)
                        from role in package.Roles
                        select role;
            this._allRoles.Clear();
            this._allRoles.AddRange(roles);

            foreach (BusinessMenuItem item in this.Root[0].Flatten(i => i.ChildItems).Where(i => i.ApplicationId != null))
            {
                foreach (Guid roleId in item.Roles.ToArray())
                {
                    if (!this._allRoles.Any(r => r.Id == roleId))
                    {
                        item.Roles.Remove(roleId);
                    }
                }
            }

            if (this.SelectedItem != null)
            {
                // Recreate MenuItemModel for sync app and roles
                this.SetSelectedMenuItem(this.SelectedItem.Entity); 

            }
        }


        private List<BusinessApplicationData> _allApplications = new List<BusinessApplicationData>();

        private List<BusinessRoleData> _allRoles = new List<BusinessRoleData>();

        public BusinessMenuItem Add(BusinessMenuItem parent)
        {
            BusinessMenuItem rs = this._entityService.AddBusinessMenuItem(parent);
            PropertyChangedEventManager.AddListener(rs, this, "EntityState");
            this.EditingState = Studio.EditingState.Dirty;

            this.SetSelectedMenuItem(rs);

            return rs;
        }


        public void Remove(BusinessMenuItem item)
        {

            BusinessMenuItem parent = item.Parent;
            foreach (BusinessMenuItem i in item.Flatten(i => i.ChildItems))
            {
                _toRemoved.Add(i);
                PropertyChangedEventManager.RemoveListener(item, this, "EntityState");
            }

            parent.ChildItems.Remove(item);
            item.Parent = null;

            this.SetSelectedMenuItem(parent);

            this.EditingState = EditingState.Dirty ;

        }




        public void Save()
        {
            this._entityService.BeginUpdate();
            try
            {
                foreach (BusinessMenuItem item in _toRemoved.Reverse())
                {
                    this._entityService.Delete(item);
                }

                this._toRemoved.Clear();
                foreach (BusinessMenuItem item in this.Root[0].Flatten(i => i.ChildItems).Where(i=>i.EntityState != EntityState.Clean))
                {
                    this._entityService.SaveOrUpdate(item);
                }
                this._entityService.EndUpdate(true);

                this.EditingState = Studio.EditingState.Clean;
            }
            catch
            {
                this._entityService.EndUpdate(false);
                 
              
            }
        }


        private ISet<BusinessMenuItem> _toRemoved = new HashSet<BusinessMenuItem>();

        private IEntityService _entityService;

        public BusinessMenuItem[] Root { get; private set; }


        private EditingState _editingState = EditingState.Clean;

        public EditingState EditingState
        {
            get { return _editingState; }
            set
            {
                if (_editingState != value)
                {
                    _editingState = value;
                    this.OnPropertyChanged("EditingState");
                }
            }
        }


        private MenuItemModel _selectedItem;

        public MenuItemModel SelectedItem
        {
            get { return _selectedItem; }
            private set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    this.OnPropertyChanged("SelectedItem");
                }
            }
        }

        public void SetSelectedMenuItem(BusinessMenuItem item)
        {
            MenuItemModel model = new MenuItemModel()
            {
                Entity = item,

            };

            model.Applications.Add(new ApplicationModel() { Name=Resources.NullValueText, PackageName=string.Empty, Id=null,  });
            model.Applications.AddRange(this._allApplications.Select(a=>new ApplicationModel(){Id = a.Id, Name = a.Name, PackageName = a.Package.FullName}) );
            model.SelectedApplication = model.Applications.SingleOrDefault(a => a.Id == item.ApplicationId);
            foreach (Guid roleId in item.Roles)
            {
                model.Roles.Add(this._allRoles.Single(r => r.Id == roleId));
            }

            this.SelectedItem = model;
           
        }


        public IServiceProvider Site { get; set; }

        #region IWeakEventListener Members

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            BusinessMenuItem item = (BusinessMenuItem)sender;
            if (item.EntityState != EntityState.Clean)
            {
                this.EditingState = Studio.EditingState.Dirty;
            }

            return true;
        }

        #endregion


        
    }

    class MenuItemModel : NotifyPropertyChangedObject
    {
        public MenuItemModel()
        {
            this.Applications = new List<ApplicationModel>();
        }

        private BusinessMenuItem _entity;

        public BusinessMenuItem Entity
        {
            get { return _entity; }
            set
            {
                if (_entity != value)
                {
                    _entity = value;
                    this.OnPropertyChanged("Entity");
                }
            }
        }

        private ApplicationModel _selectedApplication;

        public ApplicationModel SelectedApplication
        {
            get { return _selectedApplication; }
            set
            {
                if (_selectedApplication != value)
                {
                    _selectedApplication = value;
                    this.Entity.ApplicationId = value.Id;
                    this.OnPropertyChanged("SelectedApplication");
                }
            }
        }





        public List<ApplicationModel> Applications
        {
            get;
            private set;

        }


        private ObservableCollection<BusinessRoleData> _roles = new ObservableCollection<BusinessRoleData>();

        public ObservableCollection<BusinessRoleData> Roles
        {
            get { return _roles; }
           
        }
    }

    class ApplicationModel
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public string PackageName { get; set; }
    }
}
