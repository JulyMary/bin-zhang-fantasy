package fantasy.jobs;

import java.util.*;

import org.apache.commons.lang3.StringUtils;

import fantasy.xserialization.*;

@XSerializable(name = "stateBag", namespaceUri=Consts.XNamespaceURI)
public class StateBag implements IStateBag
{

	@XArray(serializer=StateBagItemsSerializer.class, items = {})
	private ArrayList<StateBagItem> _items = new ArrayList<StateBagItem>();


	public final Object getItem(String name)
	{
		int index = this.indexOf(name);
		return index >= 0 ? _items.get(index).getValue() : null;

	}
	public final void setItem(String name, Object value)
	{
		int index = this.indexOf(name);
		if (index >= 0)
		{
			_items.get(index).setValue(value);
		}
		else
		{
			StateBagItem tempVar = new StateBagItem();
			tempVar.setName(name);
			tempVar.setValue(value);
			this._items.add(tempVar);
		}
	}

	@SuppressWarnings("unchecked")
	public final <T> T GetValue(String name, T defaultValue)
	{
		int index = this.indexOf(name);
		if (index >= 0)
		{
			return (T)this._items.get(index).getValue();
		}
		else
		{
			this.setItem(name, defaultValue);
			return defaultValue;
		}
	}

	private int indexOf(String name)
	{

		for(int i = 0; i < this._items.size(); i ++)
		{
			if(StringUtils.equalsIgnoreCase(name,  this._items.get(i).getName()))
			{
				return i;
			}
		}
		
		return -1;
		
		
	}

	public final int getCount()
	{
		return this._items.size();
	}

	public final String[] getNames()
	{
		ArrayList<String> rs = new ArrayList<String>(this._items.size());
		for(StateBagItem item : this._items)
		{
			rs.add(item.getName());
		}
		
		return rs.toArray(new String[0]);
	}

	public final void Remove(String name)
	{
		int index = this.indexOf(name);
		if (index >= 0)
		{
			this._items.remove(index);
		}
	}

	public final void Clear()
	{
		this._items.clear();
	}


	public final java.util.Iterator<StateBagItem> iterator()
	{
		return this._items.iterator();
	}



	public final boolean ContainsState(String name)
	{
		return this.indexOf(name) >= 0;
	}

}