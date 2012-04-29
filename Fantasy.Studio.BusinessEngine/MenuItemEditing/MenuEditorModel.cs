using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using Fantasy.Windows;
using Fantasy.BusinessEngine.Services;
using System.Windows;
using System.ComponentModel;

namespace Fantasy.Studio.BusinessEngine.MenuItemEditing
{
    public class MenuEditorModel : NotifyPropertyChangedObject, IObjectWithSite, IWeakEventListener
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

            this.SelectedItem = this.Root[0];
            
        }


        public BusinessMenuItem Add(BusinessMenuItem parent)
        {
            BusinessMenuItem rs = this._entityService.AddBusinessMenuItem(parent);
            PropertyChangedEventManager.AddListener(rs, this, "EntityState");
            this.EditingState = Studio.EditingState.Dirty;

            this.SelectedItem = rs;

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

            this.SelectedItem = parent;

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


        private BusinessMenuItem _selectedItem;

        public BusinessMenuItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    this.OnPropertyChanged("SelectedItem");
                }
            }
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
}
