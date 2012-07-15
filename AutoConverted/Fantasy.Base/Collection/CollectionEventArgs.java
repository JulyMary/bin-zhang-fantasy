package Fantasy;

public class CollectionEventArgs<T> extends EventArgs
{
	public CollectionEventArgs(int index, T value)
	{
		this.setIndex(index);
		this.setValue(value);
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
	private T privateValue;
	public final T getValue()
	{
		return privateValue;
	}
	private void setValue(T value)
	{
		privateValue = value;
	}
}