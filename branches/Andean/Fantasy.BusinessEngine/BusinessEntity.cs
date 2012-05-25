using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using Fantasy.BusinessEngine.Events;

namespace Fantasy.BusinessEngine
{
    public abstract class BusinessEntity : IBusinessEntity,IEntity 
    {
        public BusinessEntity()
        {

        }

        private  Dictionary<string, object> _values = new Dictionary<string, object>();
        protected Dictionary<string, object> Values
        {
            get
            {
                return _values;
            }
        }

        protected virtual object GetValue(string propertyName, object defaultValue)
        {
            return this._values.GetValueOrDefault(propertyName, defaultValue);
        }

        protected virtual bool SetValue(string propertyName, object value)
        {
            bool rs = false;
            PropertyInfo prop = this.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetProperty);
            object old = prop.GetValue(this, null);
            if (old != value)
            {
                EntityPropertyChangingEventArgs e1 = new EntityPropertyChangingEventArgs(this, propertyName, value, old);
                this.OnPropertyChanging(e1);
                if (!e1.Cancel)
                {
                    if(this.EntityState == BusinessEngine.EntityState.Clean)
                    {
                        this.EntityState = BusinessEngine.EntityState.Dirty; 
                    }

                    this._values[propertyName] = value;
                    rs = true;
                    this.OnPropertyChanged(new EntityPropertyChangedEventArgs(this, propertyName, value, old));
                }
               
            }

            return rs;
        }


        private EntityState _entityState = EntityState.New;

        public virtual EntityState EntityState
        {
            get { return _entityState; }
            protected set 
            {
                if (_entityState != value)
                {
                    _entityState = value;
                    this.OnEntityStateChanged(EventArgs.Empty);
                    this.OnNotifyPropertyChangedPropertyChanged("EntityState");
                }
            }
        }


        EntityState IEntity.EntityState
        {
            get
            {
                return this.EntityState;
            }
            set
            {
               
                this.EntityState = value;
            }
        }

        protected virtual void OnEntityStateChanged(EventArgs e)
        {
            if (this.EntityStateChanged != null)
            {
                this.EntityStateChanged(this, e);
            }
        }

        public virtual event EventHandler EntityStateChanged;

        protected virtual void OnPropertyChanged(EntityPropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, e);
            }

            OnNotifyPropertyChangedPropertyChanged(e.PropertyName);
        }

        protected void OnNotifyPropertyChangedPropertyChanged(string propertyName)
        {
            if (this._propertyChanged != null)
            {
                this._propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected event PropertyChangedEventHandler _propertyChanged;

        public virtual event EventHandler<EntityPropertyChangedEventArgs> PropertyChanged;

        void IEntity.OnPropertyChanged(EntityPropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(e);
        }


        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                this._propertyChanged += value;
            }
            remove
            {
                this._propertyChanged -= value;
            }
        }


        protected virtual void OnPropertyChanging(EntityPropertyChangingEventArgs e)
        {
            if (this.PropertyChanging != null)
            {
                this.PropertyChanging(this, e);
            }
            if (this._propertyChanging != null)
            {
                this._propertyChanging(this, new PropertyChangingEventArgs(e.PropertyName));
            }
        }

        void IEntity.OnPropertyChanging(EntityPropertyChangingEventArgs e)
        {
            this.OnPropertyChanging(e);
        }

        public virtual event EventHandler<EntityPropertyChangingEventArgs> PropertyChanging;

        private event PropertyChangingEventHandler _propertyChanging;

        event PropertyChangingEventHandler INotifyPropertyChanging.PropertyChanging
        {
            add
            {
                _propertyChanging += value;
            }
            remove
            {
                _propertyChanging -= value;
            }
        }



        protected  virtual void OnLoad(EventArgs e)
        {
            if (this.Load != null)
            {
                this.Load(this, e);
            }
        }

        public virtual event EventHandler Load;

        
        public virtual Guid Id 
        {
            get
            {
                Guid rs =  (Guid)this.GetValue("Id", Guid.Empty);
                return rs;
            }
            private set
            {
                this.SetValue("Id", value);
            }
        }





        public virtual DateTime CreationTime
        {
            get
            {
                return (DateTime)this.GetValue("CreationTime", DateTime.MaxValue);
            }
            private set
            {
                this.SetValue("CreationTime", value);
            }
        }

        public virtual DateTime ModificationTime
        {
            get
            {
                return (DateTime)this.GetValue("ModificationTime", DateTime.MinValue);
            }
            set
            {
                this.SetValue("ModificationTime", value);
            }
        }

        public virtual bool IsSystem
        {
            get
            {
                return (bool)this.GetValue("IsSystem", false);
            }
            set
            {
                this.SetValue("IsSystem", value);
            }
        }

        public override bool Equals(object obj)
        {
            IBusinessEntity other = obj as IBusinessEntity;
            if(other == null)
            {
                return false;
            }
            else
            {
               return other.Id == this.Id; 
            }
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }


        public static bool operator == (BusinessEntity x, BusinessEntity y)
        {
            if (Object.Equals(x, null) && Object.Equals(y, null))
            {
                return true;
            }
            else if (Object.Equals(x, null))
            {
                return false;

            }
            else if (Object.Equals(y, null))
            {
                return false;
            }
            else
            {
                return x.Id == y.Id;
            }

        }

        public static bool operator !=(BusinessEntity x, BusinessEntity y)
        {
            return !(x == y);
        }


        #region IEntity Members

        protected virtual void OnCreate(EventArgs e)
        {
            if (this.Create != null)
            {
                this.Create(this, e);
            }
        }

        void IEntity.OnCreate(EntityCreateEventArgs e)
        {
           
            this.Id = e.Key != null ? (Guid)e.Key : Guid.NewGuid();
            this.CreationTime = DateTime.Now;
            this.OnCreate(e);
        }

        public virtual event EventHandler Create;

        void IEntity.OnLoad(EventArgs e)
        {
            this.OnLoad(e);
        }

        protected virtual void OnInserting(CancelEventArgs e)
        {
            if (this.Inserting != null)
            {
                this.Inserting(this, e);
            }

            if (!e.Cancel)
            {
                this.ModificationTime = DateTime.Now;
            }
        }

        public virtual event CancelEventHandler Inserting;

        void IEntity.OnInserting(CancelEventArgs e)
        {
            this.OnInserting(e);
           
        }




        void IEntity.OnInserted(EventArgs e)
        {
            this.OnInserted(e);
        }

        public virtual event EventHandler Inserted;

        protected virtual void OnInserted(EventArgs e)
        {
            if (this.Inserted != null)
            {
                this.Inserted(this, EventArgs.Empty);
            }
        }

        void IEntity.OnUpdating(CancelEventArgs e)
        {
            this.OnUpdating(e);
        }

        public virtual event CancelEventHandler Updating;

        protected virtual void OnUpdating(CancelEventArgs e)
        {
            if (this.Updating != null)
            {
                this.Updating(this, e);
            }

            if (!e.Cancel)
            {
                this.ModificationTime = DateTime.Now;
            }
        }

        void IEntity.OnUpdated(EventArgs e)
        {
            this.OnUpdated(e);
        }

        public virtual event EventHandler Updated;

        protected virtual void OnUpdated(EventArgs e)
        {
            if (this.Updated != null)
            {
                this.Updated(this, e);
            }
        }

        void IEntity.OnDeleting(CancelEventArgs e)
        {
            this.OnDeleting(e);
        }

        public virtual event CancelEventHandler Deleting;


        protected virtual void OnDeleting(CancelEventArgs e)
        {
            if (this.Deleting != null)
            {
                this.Deleting(this, e);
            }
        }

        void IEntity.OnDeleted(EventArgs e)
        {
            this.OnDeleted(e);
        }

        public virtual event EventHandler Deleted;

        protected virtual void OnDeleted(EventArgs e)
        {
            if (this.Deleted != null)
            {
                this.Deleted(this, e);
            }
        }


        #endregion

    }


}
