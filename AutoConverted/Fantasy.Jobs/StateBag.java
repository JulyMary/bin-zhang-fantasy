package Fantasy.Jobs;

import Fantasy.XSerialization.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[XSerializable("stateBag", NamespaceUri=Consts.XNamespaceURI)]
public class StateBag implements IStateBag
{
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XArray(Serializer=typeof(StateBagItemsSerializer))]
	private java.util.ArrayList<StateBagItem> _items = new java.util.ArrayList<StateBagItem>();

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IStateBag Members

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
			this._items.add(~index, tempVar);
		}
	}

	public final <T> T GetValue(String name, T defaultValue)
	{
		int index = this.indexOf(name);
		if (index >= 0)
		{
			return (T)this._items[index].getValue();
		}
		else
		{
			this.setItem(name, defaultValue);
			return defaultValue;
		}
	}

	private int IndexOf(String name)
	{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		int rs = _items.BinarySearchBy(name, item => item.getName(), StringComparer.OrdinalIgnoreCase);
		return rs;
	}

	public final int getCount()
	{
		return this._items.size();
	}

	public final String[] getNames()
	{
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
		return (from state in this._items select state.getName()).toArray();
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

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IEnumerable<StateItem> Members

	public final java.util.Iterator<StateBagItem> GetEnumerator()
	{
		return this._items.iterator();
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IEnumerable Members

	private java.util.Iterator GetEnumerator()
	{
		return this.iterator();
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IStateBag Members


	public final boolean ContainsState(String name)
	{
		return this.indexOf(name) >= 0;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}