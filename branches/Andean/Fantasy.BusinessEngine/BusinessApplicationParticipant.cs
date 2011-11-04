using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Collections;
using Fantasy.Windows;

namespace Fantasy.BusinessEngine
{
    public class BusinessApplicationParticipant : BusinessEntity
    {

        public virtual BusinessClass Class
        {
            get
            {
                return (BusinessClass)this.GetValue("Class", null);
            }
            set
            {
                this.SetValue("Class", value);
                if (value != null)
                {
                    _classStateListener = new WeakEventListener((type, sender, e)=>
                    {
                        BusinessClass cls = (BusinessClass)sender;
                        if(cls.EntityState == BusinessEngine.EntityState.Deleted && this.Application != null)
                        {
                            this.Application.Participants.Remove(this);
                            this.Application = null;

                        }

                        return true;
                    });

                    EntityStateChangedEventManager.AddListener(value, this._classStateListener);
                }
            }
        }

        private WeakEventListener _classStateListener;

        public virtual BusinessApplicationData Application
        {
            get
            {
                return (BusinessApplicationData)this.GetValue("Application", null);
            }
            set
            {
                this.SetValue("Application", value);
            }
        }

      
        public virtual bool IsEntry
        {
            get
            {
                return (bool)this.GetValue("IsEntry", false);
            }
            set
            {
                this.SetValue("IsEntry", value);
            }
        }


        private IObservableList<BusinessApplicationACL> _persistedACLs = new ObservableList<BusinessApplicationACL>();
        protected internal virtual IObservableList<BusinessApplicationACL> PersistedACLs
        {
            get { return _persistedACLs; }
            private set
            {
                if (_persistedACLs != value)
                {
                    _persistedACLs = value;
                    _acls.Source = value;
                }
            }
        }

        private ObservableListView<BusinessApplicationACL> _acls;
        public virtual IObservableList<BusinessApplicationACL> ACLs
        {
            get
            {
                if (this._acls == null)
                {
                    this._acls = new ObservableListView<BusinessApplicationACL>(this._persistedACLs);
                }
                return _acls;
            }
        }


    }
}
