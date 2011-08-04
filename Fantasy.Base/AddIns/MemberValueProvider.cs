using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace Fantasy.AddIns
{
    public class MemberValueProvider : IValueProvider 
    {
        #region IValueProvider Members

        private object _source;

        public object Source
        {
            get { return _source; }
            set 
            {
                if (_source != value)
                {
                    _source = value;
                    EvaluateValue(_source, 0);
                    this.OnValueChanged(EventArgs.Empty);
                }
            }
        }


        private void HandleValueChanged(int index)
        {
            object value = Invoker.Invoke(this._values[index], this._paths[index]);
            EvaluateValue(value, index + 1);
            this.OnValueChanged(EventArgs.Empty);
        }

        private void EvaluateValue(object src, int index)
        {
            if (index < this._paths.Length)
            {
                object oldValue = this._values[index];
                Delegate handler = this._handlers[index];

                if (oldValue != null && handler != null)
                {
                    Type t = oldValue.GetType();
                    EventInfo ei = t.GetEvent(this._paths[index]);
                    if (ei != null)
                    {
                        ei.RemoveEventHandler(oldValue, handler);
                    }
                    else
                    {
                        ((INotifyPropertyChanged)oldValue).PropertyChanged -= (PropertyChangedEventHandler)handler;
                    }

                }

                this._handlers[index] = null;

            }

            this._values[index] = src;
            if (src != null && index < this._paths.Length)
            {
                Type t = src.GetType();
                EventInfo ei = t.GetEvent(this._paths[index]);
                if (ei != null)
                {
                    EventHandler handler = (sender, args)=>
                        {
                            this.HandleValueChanged(index);
                        };
                    ei.AddEventHandler(src, handler);
                    this._handlers[index] = handler;
                }
                else if (src is INotifyPropertyChanged)
                {
                    PropertyChangedEventHandler handler = (sender, args) =>
                        {
                            if (args.PropertyName == this._paths[index])
                            {
                                this.HandleValueChanged(index);
                            }

                        };
                    ((INotifyPropertyChanged)src).PropertyChanged += handler;
                    this._handlers[index] = handler;
                }
            }
            if (index < this._paths.Length)
            {
                object value = Invoker.Invoke(src, this._paths[index]);
                EvaluateValue(value, index + 1);
            }
        }

        private string[] _paths;
        private object[] _values;
        private Delegate[] _handlers;

        

        public object Value
        {
            get { return this._values.LastOrDefault(); }
        }

        private string _member;

        public string Member
        {
            get { return _member; }
            set {

                _member = value;
                this._paths = _member != null ? _member.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries) : new string[0];
                this._handlers = new Delegate[this._paths.Length];
                this._values = new object[this._paths.Length + 1];
            }
        }


        protected virtual void OnValueChanged(EventArgs e)
        {
            if (this.ValueChanged != null)
            {
                this.ValueChanged(this, e);
            }
        }

        public event EventHandler ValueChanged;



        #endregion
    }
}
