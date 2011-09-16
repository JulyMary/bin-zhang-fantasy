using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Data;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

namespace Fantasy
{

    public static class CollectionExtensions
    {
        public static void SortBy<TSource, TKey>(this List<TSource> source, Func<TSource, TKey> keySelector)
        {
            CollectionExtensions.SortBy<TSource, TKey>(source, keySelector, Comparer.Default);
        }

        public static int BinarySearchBy<TSource, TKey>(this List<TSource> source, TKey key, Func<TSource, TKey> keySelector)
        {
            return CollectionExtensions.BinarySearchBy<TSource, TKey>(source, key, keySelector, Comparer.Default);
        }

        public static void SortBy<TSource, TKey>(this List<TSource> source, Func<TSource, TKey> keySelector, IComparer comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            Comparison<TSource> comparison = (x, y) =>
            {
                object k1 = keySelector(x);
                object k2 = keySelector(y);
                return comparer.Compare(k1, k2);
            };

            source.Sort(comparison);
        }

        public static int BinarySearchBy<TSource, TKey>(this List<TSource> source, TKey key, Func<TSource, TKey> keySelector, IComparer comparer)
        {
            Func<int, object> valueSelector = (index) =>
                {
                    return keySelector(source[index]);
                };
            return InnerBinarySearchBy(0, source.Count, key, valueSelector, comparer);
        }


        public static void SortBy<TSource, TKey>(this Collection<TSource> source, Func<TSource, TKey> keySelector)
        {
            CollectionExtensions.SortBy<TSource, TKey>(source, keySelector, Comparer.Default);
        }

        public static int BinarySearchBy<TSource, TKey>(this Collection<TSource> source, TKey key, Func<TSource, TKey> keySelector)
        {
            return CollectionExtensions.BinarySearchBy<TSource, TKey>(source, key, keySelector, Comparer.Default);
        }

        public static void SortBy<TSource, TKey>(this Collection<TSource> source, Func<TSource, TKey> keySelector, IComparer comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            Comparison<TSource> comparison = (x, y) =>
            {
                object k1 = keySelector(x);
                object k2 = keySelector(y);
                return comparer.Compare(k1, k2);
            };

            source.InnerList.Sort(comparison);
        }

        public static int BinarySearchBy<TSource, TKey>(this Collection<TSource> source, TKey key, Func<TSource, TKey> keySelector, IComparer comparer)
        {
            Func<int, object> valueSelector = (index) =>
            {
                return keySelector(source[index]);
            };
            return InnerBinarySearchBy(0, source.Count, key, valueSelector, comparer);
        }

        private static int InnerBinarySearchBy(int index, int count, object key, Func<int, object> valueSelector, IComparer comparer)
        {
            int low = index;
            int high = index + count - 1;
            while (low <= high)
            {
                int middle = low + ((high - low) >> 1);
                object value = valueSelector(middle);
                int cmp = comparer.Compare(value, key);
                if (cmp == 0)
                {
                    return middle;
                }
                if (cmp < 0)
                {
                    low = middle + 1;
                }
                else
                {
                    high = middle - 1;
                }
            }
            return ~low;
        }


        public static IEnumerable<T> Flatten<T>(this T item, Func<T, IEnumerable<T>> children, Func<T, bool> condition)
        {
            
                if (condition == null || condition(item))
                {
                    yield return item;
                    if (children != null)
                    {
                        foreach(T child in children(item))
                        {
                            foreach(T descendant in child.Flatten(children, condition))
                            {
                                yield return descendant;
                            }
                        }
                    }
                }
            
        }

        public static IEnumerable<T> Flatten<T>(this T item, Func<T, IEnumerable<T>> children)
        {

            return Flatten(item, children, null); 

        }

        public static IEnumerable<T> Flatten<T>(this T item, Func<T, T> parent, Func<T, bool> condition)
        {
            T obj = item;
            if(condition == null || condition(obj))
            {
                yield return obj;
                T p = parent(obj);
                if (p != null)
                {
                    foreach (T ancestor in Flatten(p, parent, condition))
                    {
                        yield return ancestor;
                    }
                }
            }
        }

        public static IEnumerable<T> Flatten<T>(this T item, Func<T, T> parent)
        {
            return Flatten(item, parent, null); 
        }

        public static Collection<T> ToCollection<T>(this IEnumerable<T> collection)
        {
            return new Collection<T>(collection);
        }

        private class GenericSortedView<T> : IEnumerable<T>, INotifyCollectionChanged, IWeakEventListener
        {
            private ICollectionView _view;

            public GenericSortedView(ICollectionView view)
            {
                this._view = view;
                CollectionChangedEventManager.AddListener(view, this);
            }

          
            public event NotifyCollectionChangedEventHandler CollectionChanged;

            private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
            {
                if (this.CollectionChanged != null)
                {
                    this.CollectionChanged(this, e);
                }
            }
         
          
            public IEnumerator<T> GetEnumerator()
            {
                foreach(T item in this._view)
                {
                    yield return item;
                }
            }

            

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this._view.GetEnumerator();
            }



            #region IWeakEventListener Members

            public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
            {
                this.OnCollectionChanged((NotifyCollectionChangedEventArgs)e);
                return true;
            }

            #endregion
        }

        public static IEnumerable<T> ToSorted<T>(this IEnumerable<T> collection, string propertyName, ListSortDirection direction = ListSortDirection.Ascending)
        {
            CollectionViewSource vs = new CollectionViewSource();
            vs.Source = collection;

            vs.View.SortDescriptions.Add(new System.ComponentModel.SortDescription(propertyName, direction));

            return new GenericSortedView<T>(vs.View);
        }


        public static IEnumerable ToSorted(this IEnumerable collection, string propertyName, ListSortDirection direction = ListSortDirection.Ascending)
        {
            CollectionViewSource vs = new CollectionViewSource();
            vs.Source = collection;

            vs.View.SortDescriptions.Add(new System.ComponentModel.SortDescription(propertyName, direction));


            return vs.View;
        }

        public static IEnumerable<T> OfType<T>(this IEnumerable collection)
        {
            foreach (object o in collection)
            {
                if (o is T)
                {
                    yield return (T)o;
                }
            }
        }
    }
}
