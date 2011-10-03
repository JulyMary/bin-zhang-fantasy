using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.Specialized;
using Fantasy.BusinessEngine.Events;
using System.Collections;

namespace Fantasy.BusinessEngine
{
   public class EntityPropertyChangedEventManager : WeakEventManager
{
    // Fields
    private WeakEventManager.ListenerList _proposedAllListenersList;
    private static readonly string AllListenersKey = "<All Listeners>";

    // Methods
    private EntityPropertyChangedEventManager()
    {

    }

    public static void AddListener(IBusinessEntity source, IWeakEventListener listener, string propertyName)
    {
        if (source == null)
        {
            throw new ArgumentNullException("source");
        }
        if (listener == null)
        {
            throw new ArgumentNullException("listener");
        }
        CurrentManager.PrivateAddListener(source, listener, propertyName);
    }

    private void OnPropertyChanged(object sender, EntityPropertyChangedEventArgs args)
    {
        WeakEventManager.ListenerList empty;
        string propertyName = args.PropertyName;
        using (base.ReadLock)
        {
            HybridDictionary dictionary = (HybridDictionary) base[sender];
            if (dictionary == null)
            {
                empty = WeakEventManager.ListenerList.Empty;
            }
            else if (!string.IsNullOrEmpty(propertyName))
            {
                WeakEventManager.ListenerList list2 = (WeakEventManager.ListenerList) dictionary[propertyName];
                WeakEventManager.ListenerList list3 = (WeakEventManager.ListenerList) dictionary[string.Empty];
                if (list3 == null)
                {
                    if (list2 != null)
                    {
                        empty = list2;
                    }
                    else
                    {
                        empty = WeakEventManager.ListenerList.Empty;
                    }
                }
                else if (list2 != null)
                {
                    empty = new WeakEventManager.ListenerList(list2.Count + list3.Count);
                    int num3 = 0;
                    int count = list2.Count;
                    while (num3 < count)
                    {
                        empty.Add(list2[num3]);
                        num3++;
                    }
                    int num2 = 0;
                    int num6 = list3.Count;
                    while (num2 < num6)
                    {
                        empty.Add(list3[num2]);
                        num2++;
                    }
                }
                else
                {
                    empty = list3;
                }
            }
            else
            {
                empty = (WeakEventManager.ListenerList) dictionary[AllListenersKey];
                if (empty == null)
                {
                    int capacity = 0;
                    foreach (DictionaryEntry entry2 in dictionary)
                    {
                        capacity += ((WeakEventManager.ListenerList) entry2.Value).Count;
                    }
                    empty = new WeakEventManager.ListenerList(capacity);
                    foreach (DictionaryEntry entry in dictionary)
                    {
                        WeakEventManager.ListenerList list4 = (WeakEventManager.ListenerList) entry.Value;
                        int num = 0;
                        int num5 = list4.Count;
                        while (num < num5)
                        {
                            empty.Add(list4[num]);
                            num++;
                        }
                    }
                    this._proposedAllListenersList = empty;
                }
            }
            empty.BeginUse();
        }
        try
        {
            base.DeliverEventToList(sender, args, empty);
        }
        finally
        {
            empty.EndUse();
        }
        if (this._proposedAllListenersList == empty)
        {
            using (base.WriteLock)
            {
                if (this._proposedAllListenersList == empty)
                {
                    HybridDictionary dictionary2 = (HybridDictionary) base[sender];
                    if (dictionary2 != null)
                    {
                        dictionary2[AllListenersKey] = empty;
                    }
                    this._proposedAllListenersList = null;
                }
            }
        }
    }

    private void PrivateAddListener(IBusinessEntity source, IWeakEventListener listener, string propertyName)
    {
        using (base.WriteLock)
        {
            HybridDictionary dictionary = (HybridDictionary) base[source];
            if (dictionary == null)
            {
                dictionary = new HybridDictionary(true);
                base[source] = dictionary;
                this.StartListening(source);
            }
            WeakEventManager.ListenerList list = (WeakEventManager.ListenerList) dictionary[propertyName];
            if (list == null)
            {
                list = new WeakEventManager.ListenerList();
                dictionary[propertyName] = list;
            }
            if (WeakEventManager.ListenerList.PrepareForWriting(ref list))
            {
                dictionary[propertyName] = list;
            }
            list.Add(listener);
            dictionary.Remove(AllListenersKey);
            this._proposedAllListenersList = null;
            base.ScheduleCleanup();
        }
    }

    private void PrivateRemoveListener(IBusinessEntity source, IWeakEventListener listener, string propertyName)
    {
        using (base.WriteLock)
        {
            HybridDictionary dictionary = (HybridDictionary) base[source];
            if (dictionary != null)
            {
                WeakEventManager.ListenerList list = (WeakEventManager.ListenerList) dictionary[propertyName];
                if (list != null)
                {
                    if (WeakEventManager.ListenerList.PrepareForWriting(ref list))
                    {
                        dictionary[propertyName] = list;
                    }
                    list.Remove(listener);
                    if (list.IsEmpty)
                    {
                        dictionary.Remove(propertyName);
                    }
                }
                if (dictionary.Count == 0)
                {
                    this.StopListening(source);
                    base.Remove(source);
                }
                dictionary.Remove(AllListenersKey);
                this._proposedAllListenersList = null;
            }
        }
    }

    protected override bool Purge(object source, object data, bool purgeAll)
    {
        bool flag = false;
        if (!purgeAll)
        {
            HybridDictionary dictionary = (HybridDictionary) data;
            ICollection keys = dictionary.Keys;
            string[] array = new string[keys.Count];
            keys.CopyTo(array, 0);
            for (int i = array.Length - 1; i >= 0; i--)
            {
                if (array[i] != AllListenersKey)
                {
                    bool isEmpty = purgeAll || (source == null);
                    if (!isEmpty)
                    {
                        WeakEventManager.ListenerList list = (WeakEventManager.ListenerList) dictionary[array[i]];
                        if (WeakEventManager.ListenerList.PrepareForWriting(ref list))
                        {
                            dictionary[array[i]] = list;
                        }
                        if (list.Purge())
                        {
                            flag = true;
                        }
                        isEmpty = list.IsEmpty;
                    }
                    if (isEmpty)
                    {
                        dictionary.Remove(array[i]);
                    }
                }
            }
            if (dictionary.Count == 0)
            {
                purgeAll = true;
                if (source != null)
                {
                    base.Remove(source);
                }
            }
            else if (flag)
            {
                dictionary.Remove(AllListenersKey);
                this._proposedAllListenersList = null;
            }
        }
        if (!purgeAll)
        {
            return flag;
        }
        if (source != null)
        {
            this.StopListening(source);
        }
        return true;
    }

    public static void RemoveListener(IBusinessEntity source, IWeakEventListener listener, string propertyName)
    {
        if (listener == null)
        {
            throw new ArgumentNullException("listener");
        }
        CurrentManager.PrivateRemoveListener(source, listener, propertyName);
    }

    protected override void StartListening(object source)
    {
        IBusinessEntity changed = (IBusinessEntity)source;
        changed.PropertyChanged += new EventHandler<EntityPropertyChangedEventArgs>(this.OnPropertyChanged);
    }

    protected override void StopListening(object source)
    {
        IBusinessEntity changed = (IBusinessEntity)source;
        changed.PropertyChanged -= new EventHandler<EntityPropertyChangedEventArgs>(this.OnPropertyChanged);
    }

    // Properties
    private static EntityPropertyChangedEventManager CurrentManager
    {
        get
        {
            Type managerType = typeof(EntityPropertyChangedEventManager);
            EntityPropertyChangedEventManager currentManager = (EntityPropertyChangedEventManager)WeakEventManager.GetCurrentManager(managerType);
            if (currentManager == null)
            {
                currentManager = new EntityPropertyChangedEventManager();
                WeakEventManager.SetCurrentManager(managerType, currentManager);
            }
            return currentManager;
        }
    }
}

 

 

}
