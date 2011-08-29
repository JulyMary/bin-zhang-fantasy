using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using Fantasy.Windows;
using System.ComponentModel;
using System.Windows;
using System.Collections.Specialized;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Model
{
    internal class PropertyNode : NotifyPropertyChangedObject, IWeakEventListener
    {
        public PropertyNode(BusinessProperty property)
        {
            this.Entity = property;
            CollectionChangedEventManager.AddListener(this.Entity.Class.Properties, this);
            EvalUpDown();

        }

        public BusinessProperty Entity { get; private set; }

        private bool _canMoveUp;

        public bool CanMoveUp
        {
            get { return _canMoveUp; }
            private set
            {
                if (_canMoveUp != value)
                {
                    _canMoveUp = value;
                    this.OnPropertyChanged("CanMoveUp");
                }
            }
        }

        private bool _canMoveDown;

        public bool CanMoveDown
        {
            get { return _canMoveDown; }
            set
            {
                if (_canMoveDown != value)
                {
                    _canMoveDown = value;
                    this.OnPropertyChanged("CanMoveDown");
                }
            }
        }


        private void EvalUpDown()
        {

            if (this.Entity.IsSystem || this.Entity.Class == null)
            {
                this.CanMoveDown = false;
                this.CanMoveUp = false;
            }
            else
            {
                this.CanMoveUp = this.Entity.Class.Properties.IndexOf(this.Entity) > 0;
                this.CanMoveDown = this.Entity.Class.Properties.LastOrDefault() != this.Entity;
            }

        }

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            EvalUpDown();

            return true;
        }

    }
}
