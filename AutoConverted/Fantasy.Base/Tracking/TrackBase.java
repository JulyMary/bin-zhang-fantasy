package Fantasy.Tracking;

public abstract class TrackBase
{
	protected java.util.HashMap<String, Object> Data = new java.util.HashMap<String, Object>(StringComparer.OrdinalIgnoreCase);

	private Guid privateId = new Guid();
	public Guid getId()
	{
		return privateId;
	}
	public void setId(Guid value)
	{
		privateId = value;
	}

	private String privateName;
	public String getName()
	{
		return privateName;
	}
	public void setName(String value)
	{
		privateName = value;
	}

	private String privateCategory;
	public String getCategory()
	{
		return privateCategory;
	}
	public void setCategory(String value)
	{
		privateCategory = value;
	}



	protected final void InitializeData(java.util.Map<String, Object> values)
	{
		if (values != null)
		{
			for (java.util.Map.Entry<String, Object> pair : values)
			{
				this.Data.put(pair.getKey(), pair.getValue());
			}
		}
	}

	public String[] getPropertyNames()
	{
		synchronized (this.Data)
		{
			return this.Data.keySet().toArray();
		}
	}

	public Object getItem(String name)
	{
		synchronized(this.Data)
		{
			Object rs = null;
			rs = this.Data.get(name);
			return rs;
		}
	}
	public void setItem(String name, Object value)
	{
		Object oldValue = null;
		boolean changed = false;
		synchronized (this.Data)
		{
			oldValue = this.Data.get(name);

			if (Comparer.Default.Compare(oldValue, value) != 0)
			{
				changed = true;
				this.Data.put(name, value);
			}
		}

		if (changed)
		{
			this.OnChanged(new TrackChangedEventArgs(name, oldValue, value));
		}
	}

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler<TrackChangedEventArgs> Changed
//		{
//			add
//			{
//				this._changed += value;
//			}
//			remove
//			{
//				this._changed -= value;
//			}
//		}

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	private event EventHandler<TrackChangedEventArgs> _changed;

	protected void OnChanged(TrackChangedEventArgs e)
	{
		if (this._changed != null)
		{
			this._changed(this, e);
		}
	}
}