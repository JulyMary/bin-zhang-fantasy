package fantasy.tracking;
import java.rmi.RemoteException;
import java.rmi.server.UnicastRemoteObject;
import java.util.*;

import org.apache.commons.lang3.ObjectUtils;
abstract class TrackBase extends UnicastRemoteObject
{
	/**
	 * 
	 */
	private static final long serialVersionUID = -3631356230286619678L;
	protected TrackBase() throws RemoteException {
		super();
		
	}

	protected java.util.HashMap<String, Object> Data = new java.util.HashMap<String, Object>();

	private UUID privateId = UUID.randomUUID();
	public UUID getId()
	{
		return privateId;
	}
	public void setId(UUID value)
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
			for (java.util.Map.Entry<String, Object> pair : values.entrySet())
			{
				this.Data.put(pair.getKey(), pair.getValue());
			}
		}
	}

	public String[] getPropertyNames()
	{
		synchronized (this.Data)
		{
			return this.Data.keySet().toArray(new String[0]);
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

			if (!ObjectUtils.equals(oldValue, value))
			{
				changed = true;
				this.Data.put(name, value);
			}
		}

		if (changed)
		{
			this.onChanged(new TrackChangedEventObject(this, name, oldValue, value));
		}
	}



	protected void onChanged(TrackChangedEventObject e)
	{
	}
	}
	
