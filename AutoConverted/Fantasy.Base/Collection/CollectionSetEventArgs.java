package Fantasy;

public class CollectionSetEventArgs<T> extends EventArgs
{
	public CollectionSetEventArgs(int index, T oldValue, T newValue)
	{
		this.setIndex(index);
		this.setOldValue(oldValue);
		this.setNewValue(getNewValue());
	}
	private int privateIndex;
	public final int getIndex()
	{
		return privateIndex;
	}
	private void setIndex(int value)
	{
		privateIndex = value;
	}
	private T privateOldValue;
	public final T getOldValue()
	{
		return privateOldValue;
	}
	private void setOldValue(T value)
	{
		privateOldValue = value;
	}
	private T privateNewValue;
	public final T getNewValue()
	{
		return privateNewValue;
	}
	private void setNewValue(T value)
	{
		privateNewValue = value;
	}
}