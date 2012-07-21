package fantasy.jobs;

import fantasy.xserialization.*;
import fantasy.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[XSerializable("status", NamespaceUri=Consts.XNamespaceURI)]
public class RuntimeStatus
{
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XArray(Name="stack"), XArrayItem(Name = "stack.frame", java.lang.Class = typeof(StateBag))]
	private java.util.ArrayList<StateBag> _stack = new java.util.ArrayList<StateBag>();


	public final void PushStack()
	{
		_position++;
		if (this._stack.size() <= _position)
		{
			this._stack.add(new StateBag());
		}
	}

	public final void PopStack()
	{
		if (this._stack.size() > 0)
		{
			this._stack.removeRange(_position, this._stack.size() - _position + _position);
			_position--;
		}
	}

	public final boolean getIsRestoring()
	{
		return _position < this._stack.size() - 1;
	}

	private int _position = -1;

	public final StateBag getLocal()
	{
		return _position < this._stack.size() ? this._stack.get(this._position) : null;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XElement("heap")]
	private StateBag _global = new StateBag();
	public final StateBag getGlobal()
	{
		return _global;
	}

	public final boolean TryGetValue(String name, RefObject<Object> value)
	{
		value.argvalue = null;
		boolean rs = false;
		for(int i = this._position; i >= 0; i --)
		{
			StateBag bag = this._stack.get(i);
			if (bag.ContainsState(name))
			{
				value.argvalue = bag.getItem(name);

				rs = true;
				break;
			}
		}
		if (!rs)
		{
			if (this.getGlobal().ContainsState(name))
			{
				value.argvalue = this.getGlobal().getItem(name);

				rs = true;
			}
		}

		return rs;
	}
}