using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Specialized;

namespace Fantasy.BusinessEngine.Collections
{
    public class ObservableAdapterCollection<T> : IEnumerable<T>, INotifyCollectionChanged
    {

        private Dictionary<object, T> _adapters = null;
        private IEnumerable _adaptees;

        private Func<object, T> _createAdapter = null;

        public ObservableAdapterCollection(IEnumerable adaptees, Func<object, T> createAdapter)
        {
            this._adaptees = adaptees;
            this._createAdapter = createAdapter;
        }

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region INotifyCollectionChanged Members

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            if (this._adapters == null)
            {
                this._adapters = new Dictionary<object, T>();
                ((INotifyCollectionChanged)this._adaptees).CollectionChanged += new NotifyCollectionChangedEventHandler(AdapteesCollectionChanged);
            }


            foreach (object o in this._adaptees)
            {
                T rs;
                if (!this._adapters.TryGetValue(o, out rs))
                {
                    rs = this._createAdapter(o);
                    this._adapters.Add(o, rs);

                }
                yield return rs;
            }
        }

        private List<T> CreateAddedAdapters(ICollection collection)
        {
            List<T> rs = new List<T>(collection.Count);
            foreach (object o in collection)
            {
                T item;
                if (!this._adapters.TryGetValue(o, out item))
                {
                    item = this._createAdapter(o);
                    this._adapters.Add(o, item);

                }
                rs.Add(item);
            }
            return rs;
        }

        void AdapteesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (this.CollectionChanged != null)
            {
                NotifyCollectionChangedEventArgs args = null;
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        {
                            args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, this.CreateAddedAdapters(e.NewItems), e.NewStartingIndex);
                        }
                        break;
                    case NotifyCollectionChangedAction.Move:
                        {
                            args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, this.CreateAddedAdapters(e.NewItems), e.NewStartingIndex, e.OldStartingIndex);
                        }
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        {
                            args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, this.CreateAddedAdapters(e.NewItems));
                        }
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        {
                            args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, this.CreateAddedAdapters(e.NewItems), this.CreateAddedAdapters(e.OldItems));
                            foreach (object o in e.OldItems)
                            {
                                this._adapters.Remove(o);
                            }
                        }
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        {
                            args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                            this._adapters.Clear();
                        }
                        break;
                    default:
                       throw new Exception("Absurd!");

                }
                this.CollectionChanged(this, args);
            }
           
        }


        #endregion
    }
}
