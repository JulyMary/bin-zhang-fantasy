package Fantasy.Jobs.Expressions;

public class ResolveTypeEventArgs extends EventArgs
{
	public ResolveTypeEventArgs(String name)
	{
		this.setTypeName(name);
	}

	private String privateTypeName;
	public final String getTypeName()
	{
		return privateTypeName;
	}
	private void setTypeName(String value)
	{
		privateTypeName = value;
	}

	private java.lang.Class privateType;
	public final java.lang.Class getType()
	{
		return privateType;
	}
	public final void setType(java.lang.Class value)
	{
		privateType = value;
	}
}