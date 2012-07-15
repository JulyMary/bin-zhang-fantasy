﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Fantasy.Tracking
{
    public abstract class TrackBase
    {
        protected Dictionary<string, object> Data = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public virtual  Guid Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string Category { get; set; }

        

        protected void InitializeData(IDictionary<string, object> values)
        {
            if (values != null)
            {
                foreach (KeyValuePair<string, object> pair in values)
                {
                    this.Data.Add(pair.Key, pair.Value);
                }
            }
        }

        public virtual string[] PropertyNames 
        {
            get
            {
                lock (this.Data)
                {
                    return this.Data.Keys.ToArray();
                }
            }
        }

        public virtual object this[string name]
        {
            get
            {
                lock(this.Data)
                {
                    object rs;
                    this.Data.TryGetValue(name, out rs);
                    return rs;
                }
            }
            set
            {
                object oldValue;
                bool changed = false;
                lock (this.Data)
                {
                    this.Data.TryGetValue(name, out oldValue);

                    if (Comparer.Default.Compare(oldValue, value) != 0)
                    {
                        changed = true;
                        this.Data[name] = value;
                    }
                }

                if (changed)
                {
                    this.OnChanged(new TrackChangedEventArgs(name, oldValue, value));
                }
            }
        }

        public event EventHandler<TrackChangedEventArgs> Changed
        {
            add
            {
                this._changed += value;
            }
            remove
            {
                this._changed -= value;
            }
        }

        private event EventHandler<TrackChangedEventArgs> _changed;

        protected virtual void OnChanged(TrackChangedEventArgs e)
        {
            if (this._changed != null)
            {
                this._changed(this, e);
            }
        }
    }


}