using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using Fantasy.Windows;
using System.Windows;
using System.ComponentModel;

namespace Fantasy.Studio.BusinessEngine.ApplicationEditing
{
    public class ParticipantACL : NotifyPropertyChangedObject, IWeakEventListener
    {

        public override bool Equals(object obj)
        {
            ParticipantACL other = obj as ParticipantACL;
            return other != null && other.Entity == this.Entity;
        }

        public override int GetHashCode()
        {
            return unchecked(this.Entity.GetHashCode() + 1000);
        }


        public ParticipantACL(BusinessApplicationParticipant participant)
        {
            this.Entity = participant;
            this.Name = this.Entity.Class.Name;
            PropertyChangedEventManager.AddListener(this.Entity.Class, this, "Name");
        }

        public BusinessApplicationParticipant Entity { get; private set; }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    this.OnPropertyChanged("Name");
                }
            }
        }

        bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            BusinessClass @class = (BusinessClass)sender;
            this.Name = @class.Name;

            return true;
        }
    }
}
