package Fantasy;

//C# TO JAVA CONVERTER TODO TASK: The interface type was changed to the closest equivalent Java type, but the methods implemented will need adjustment:
public class Collection<T> implements java.util.List<T>, java.util.Collection<T>, Iterable<T>, java.util.List, java.util.Collection, Iterable
{
	public Collection()
	{
		getthis()._list = new java.util.ArrayList<T>();
	}

	public Collection(Iterable<T> collection)
	{
		getthis()._list = new java.util.ArrayList<T>(collection);
	}

	public Collection(int capacity)
	{
		getthis()._list = new java.util.ArrayList<T>(capacity);
	}

	private java.util.ArrayList<T> _list;

	public final java.util.ArrayList<T> getInnerList()
	{
		return _list;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IList<T> Members

	public final int IndexOf(T item)
	{
		return _list.indexOf(item);
	}

	public final void Insert(int index, T item)
	{
		getthis().OnInserting(index, item);
		getthis()._list.Insert(index, item);
		getthis().OnInserted(index, item);

	}

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler<CollectionEventArgs<T>> Inserting;


	protected void OnInserting(int index, T value)
	{
		if (Inserting != null)
		{
			Inserting(getthis(), new CollectionEventArgs<T>(index, value));
		}
	}

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler<CollectionEventArgs<T>> Inserted;

	protected void OnInserted(int index, T value)
	{
		if (Inserted != null)
		{
			Inserted(getthis(), new CollectionEventArgs<T>(index, value));
		}
	}




	public final void RemoveAt(int index)
	{
		T item = getthis()._list[index];
		getthis().OnRemoving(index, item);
		getthis()._list.RemoveAt(index);
		getthis().OnRemoved(index, item);
	}

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler<CollectionEventArgs<T>> Removing;

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler<CollectionEventArgs<T>> Removed;

	protected void OnRemoving(int index, T value)
	{
		if (Removing != null)
		{
			Removing(getthis(), new CollectionEventArgs<T>(index, value));
		}
	}

	protected void OnRemoved(int index, T value)
	{
		if (Removed != null)
		{
			Removed(getthis(), new CollectionEventArgs<T>(index, value));
		}
	}

	public final T getItem(int index)
	{
		return _list.get(index);
	}
	public final void setItem(int index, T value)
	{
		T old = getthis()._list[index];
		getthis().OnSetting(index, old, value);
		getthis()._list[index] = value;
		getthis().OnSet(index, old, value);
	}

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler<CollectionSetEventArgs<T>> Setting;

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler<CollectionSetEventArgs<T>> Set;

	protected void OnSetting(int index, T oldValue, T newValue)
	{
		if (Setting != null)
		{
			Setting(getthis(), new CollectionSetEventArgs<T>(index, oldValue, newValue));
		}
	}

	protected void OnSet(int index, T oldValue, T newValue)
	{
		if (Set != null)
		{
			Set(getthis(), new CollectionSetEventArgs<T>(index, oldValue, newValue));
		}
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ICollection<T> Members

	public final void Add(T item)
	{
		getthis().Insert(getthis().getCount(), item);
	}

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler Clearing;
	protected void OnClearing()
	{
		if (getthis().Clearing != null)
		{
			getthis().Clearing(getthis(), EventArgs.Empty);
		}
	}

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler Cleared;
	protected void OnCleared()
	{
		if (getthis().Cleared != null)
		{
			getthis().Cleared(getthis(), EventArgs.Empty);
		}
	}

	public final void Clear()
	{
		getthis().OnClearing();
		getthis()._list.Clear();
		getthis().OnCleared();
	}

	public final boolean Contains(T item)
	{
		return getthis()._list.Contains(item);
	}

	public final void CopyTo(T[] array, int arrayIndex)
	{
		getthis()._list.CopyTo(array, arrayIndex);
	}

	public final int getCount()
	{
		return _list.size();
	}

	public final boolean getIsReadOnly()
	{
		return ((java.util.List)_list).IsReadOnly;
	}



	public final boolean Remove(T item)
	{
		int index = getthis()._list.indexOf(item);
		if (index >= 0)
		{
			getthis().RemoveAt(index);
			return true;
		}
		else
		{
			return false;
		}

	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IEnumerable<T> Members

	public final java.util.Iterator<T> GetEnumerator()
	{
		return getthis()._list.iterator();
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IEnumerable Members

	private java.util.Iterator GetEnumerator()
	{
		return getthis().iterator();
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion


//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IList Members

	private int Add(Object value)
	{
		getthis().Add((T)value);
		return getthis().getCount() - 1;
	}

	private void Clear()
	{
		getthis().Clear();
	}

	private boolean Contains(Object value)
	{
		return getthis().Contains((T)value);
	}

	private int IndexOf(Object value)
	{
		return getthis().indexOf((T)value);
	}

	private void Insert(int index, Object value)
	{
		getthis().Insert(index, (T)value);
	}

	private boolean getIsFixedSize()
	{
		return ((java.util.List)_list).IsFixedSize;
	}

	private boolean getIsReadOnly()
	{
		return ((java.util.List)_list).IsReadOnly;
	}

	private void Remove(Object value)
	{
		getthis().Remove((T)value);
	}



	private Object getItem(int index)
	{
		return getthis()[index];
	}
	private void setItem(int index, Object value)
	{
		getthis()[index] = (T)value;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ICollection Members

	private void CopyTo(Array array, int index)
	{
		((java.util.List)_list).CopyTo(array, index);
	}



	private boolean getIsSynchronized()
	{
		return ((java.util.List)_list).IsSynchronized;
	}

	private Object getSyncRoot()
	{
		return ((java.util.List)_list).SyncRoot;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

	public final int BinarySearch(T item)
	{
		return _list.BinarySearch(item);
	}

	public final int BinarySearch(T item, java.util.Comparator<T> comparer)
	{
		return _list.BinarySearch(item, comparer);
	}

	public final int BinarySearch(int index, int count, T item, java.util.Comparator<T> comparer)
	{
		return _list.BinarySearch(index, count, item, comparer);
	}

	public final void Sort()
	{
		getthis()._list.Sort();
	}

	public final void Sort(java.util.Comparator<T> comparer)
	{
		getthis()._list.Sort(comparer);
	}

	public final void Sort(Comparison<T> comparison)
	{
		getthis()._list.Sort(comparison);
	}

	public final void Sort(int index, int count, java.util.Comparator<T> comparer)
	{
		getthis()._list.Sort(index, count, comparer);
	}


}