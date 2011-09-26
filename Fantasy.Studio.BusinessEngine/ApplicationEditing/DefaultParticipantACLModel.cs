using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using Fantasy.Windows;
using Fantasy.Studio.BusinessEngine.ObjectSecurityEditing;

namespace Fantasy.Studio.BusinessEngine.ApplicationEditing
{
    public class DefaultParticipantACLModel : NotifyPropertyChangedObject
    {

        public DefaultParticipantACLModel(BusinessApplicationParticipant participant)
        {

            this.Entity = participant;

            this._items = participant.ACLs.ToFiltered(a => a.State == null).ToSorted("Role.Name");
            this.SelectedItem = this.Items.FirstOrDefault();
        }


        public BusinessApplicationParticipant Entity { get; private set; }

        private IEnumerable<BusinessApplicationACL> _items;

        public IEnumerable<BusinessApplicationACL> Items
        {
            get { return _items; }
            set
            {
                if (_items != value)
                {
                    _items = value;
                    this.OnPropertyChanged("Items");
                }
            }
        }

        private BusinessApplicationACL _selectedItem;

        public BusinessApplicationACL SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    this.OnPropertyChanged("SelectedItem");
                    if (_selectedItem != null)
                    {
                        this.SelectedSecurityModel = new ObjectSecurityEditorModel(_selectedItem.Security);
                    }
                    else
                    {
                        this.SelectedSecurityModel = null;
                    }
                }
            }
        }

        private ObjectSecurityEditorModel _selectedSecurityModel;

        public ObjectSecurityEditorModel SelectedSecurityModel
        {
            get { return _selectedSecurityModel; }
            private set
            {
                if (_selectedSecurityModel != value)
                {
                    _selectedSecurityModel = value;
                    this.OnPropertyChanged("SelectedSecurityModel");
                }
            }
        }

      



    }
}
