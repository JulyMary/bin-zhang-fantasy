using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Specialized;
using Fantasy.Windows;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Model
{
    public class MemaberNodeCollection : IEnumerable<MemberNode>, INotifyCollectionChanged
    {

        public MemaberNodeCollection()
        {
            this._displayOrderListener = new WeakEventListener((managerType, sender, e) => 
            {
                this.OnCollectionChanged();
                return true;
            });

            this._childCollectionListener = new WeakEventListener((managerType, sender, args) =>
                {
                    NotifyCollectionChangedEventArgs e = (NotifyCollectionChangedEventArgs)args;
                    if (e.Action != NotifyCollectionChangedAction.Move)
                    {
                        if (e.OldItems != null)
                        {
                            foreach (MemberNode member in e.OldItems)
                            {
                                this.RemoveListenDisplayOrderChanged(member);
                            }
                        }
                        if (e.NewItems != null)
                        {
                            foreach (MemberNode member in e.NewItems)
                            {
                                this.AddListenDisplayOrderChanged(member);
                            }
                        }
                    }

                    this.OnCollectionChanged();
                    return true;
                });
        }


        private List<IEnumerable<MemberNode>> _childCollections = new List<IEnumerable<MemberNode>>();
        private WeakEventListener _displayOrderListener;
        private WeakEventListener _childCollectionListener;

        public void AddChildCollection(IEnumerable<IEnumerable<MemberNode>> childCollections)
        {
            foreach (IEnumerable<MemberNode> childCollection in childCollections)
            {
                this._childCollections.Add(childCollection);
                INotifyCollectionChanged nc = (INotifyCollectionChanged)childCollection;
                CollectionChangedEventManager.AddListener(nc, this._childCollectionListener);

                foreach (MemberNode member in childCollection)
                {
                    AddListenDisplayOrderChanged(member);
                }
            }
            this.OnCollectionChanged();
        }

        private void AddListenDisplayOrderChanged(MemberNode member)
        {
            PropertyChangedEventManager.AddListener(member, this._displayOrderListener, "DisplayOrder");
        }

        private void RemoveListenDisplayOrderChanged(MemberNode member)
        {
            PropertyChangedEventManager.RemoveListener(member, this._displayOrderListener, "DisplayOrder");
        }

      

        public void RemoveChildCollection(IEnumerable<IEnumerable<MemberNode>> childCollections)
        {
            foreach (IEnumerable<MemberNode> childCollection in childCollections)
            {
                this._childCollections.Remove(childCollection);
                INotifyCollectionChanged nc = (INotifyCollectionChanged)childCollection;
                CollectionChangedEventManager.RemoveListener(nc, this._childCollectionListener);
                foreach (MemberNode member in childCollection)
                {
                    RemoveListenDisplayOrderChanged(member);
                }
            }
            this.OnCollectionChanged();
        }


        #region IEnumerable<MemberNode> Members

        private List<MemberNode> _list = null;

        private List<MemberNode> List
        {
            get
            {
                if (_list == null)
                {
                    _list = new List<MemberNode>();
                    foreach (IEnumerable<MemberNode> childCollection in this._childCollections)
                    {
                        _list.AddRange(childCollection);
                    }
                    _list.SortBy(m => m.DisplayOrder);

                    for (int i = 0; i < _list.Count; i++)
                    {
                        if (i == 0)
                        {
                            _list[i].CanMoveUp = false;
                            _list[i].CanMoveDown = _list.Count > 1;
                        }
                        else if (i == _list.Count - 1)
                        {
                            _list[i].CanMoveUp = true;
                            _list[i].CanMoveDown = false;
                        }
                        else
                        {
                            _list[i].CanMoveDown = _list[i].CanMoveUp = true;
                        }
                    }
                }
                return _list;
            }
        }

        public int IndexOf(MemberNode member)
        {
            return this.List.IndexOf(member);
        }

        public MemberNode this[int index]
        {
            get
            {
                return this.List[index];
            }
        }

        public IEnumerator<MemberNode> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region INotifyCollectionChanged Members
        private int _lock = 0;
        public void Lock()
        {
            _lock++;
        }

        public void Unlock()
        {
            _lock--;
            if (_lock == 0)
            {
                this.OnCollectionChanged();
            }
        }
       
        protected internal virtual void OnCollectionChanged()
        {
            this._list = null;
            if (this.CollectionChanged != null && _lock == 0)
            {
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset) );
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion
    }
}
