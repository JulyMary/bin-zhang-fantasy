using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Fantasy.BusinessEngine
{
    public class BusinessApplicationACL : BusinessEntity
    {

        



        public virtual BusinessEnumValue State
        {
            get
            {
                return (BusinessEnumValue)this.GetValue("State", null);
            }
            set
            {
                this.SetValue("State", value);
            }
        }

        public virtual BusinessRole Role
        {
            get
            {
                return (BusinessRole)this.GetValue("Role", null);
            }
            set
            {
                this.SetValue("Role", value);
            }
        }

        public virtual BusinessApplicationParticipant Participant
        {
            get
            {
                return (BusinessApplicationParticipant)this.GetValue("Participant", null);
            }
            set
            {
                this.SetValue("Participant", value);
            }
        }

        protected internal virtual string PersistedACL
        {
            get
            {
                return (string)this.GetValue("PersistedACL", null);
            }
            set
            {
                this.SetValue("PersistedACL", value);
            }
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            BusinessObjectSecurity seucirty = new BusinessObjectSecurity();
            if (!String.IsNullOrEmpty(this.PersistedACL))
            {
                XElement ele = XElement.Parse(this.PersistedACL);
                seucirty.Load(ele);
            }
            else
            {
                seucirty.Sync(this.Participant.Class, null);
            }

        }

        void ObjectSecurity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.PersistedACL = this._objectSecurity.ToXElement().ToString(SaveOptions.OmitDuplicateNamespaces);
        }

        private BusinessObjectSecurity _objectSecurity;
        public virtual BusinessObjectSecurity Security
        {
            get
            {
                return _objectSecurity;
            }
            set
            {
                this._objectSecurity = value;
                if (value != null)
                {
                    this._objectSecurity.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ObjectSecurity_PropertyChanged);

                }
            }
        }
        
    }
}
