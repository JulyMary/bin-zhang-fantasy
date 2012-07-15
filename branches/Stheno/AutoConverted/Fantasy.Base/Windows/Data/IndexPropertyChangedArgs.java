package Fantasy.Windows.Data;

public class IndexPropertyChangedArgs extends EventArgs
{
	public IndexPropertyChangedArgs(Object index, Object value)
	{
		this.setIndex(index);
		this.setValue(value);
	}

	private Object privateIndex;
	public final Object getIndex()
	{
		return privateIndex;
	}
	private void setIndex(Object value)
	{
		privateIndex = value;
	}

	private Object privateValue;
	public final Object getValue()
	{
		return privateValue;
	}
	private void setValue(Object value)
	{
		privateValue = value;
	}
}