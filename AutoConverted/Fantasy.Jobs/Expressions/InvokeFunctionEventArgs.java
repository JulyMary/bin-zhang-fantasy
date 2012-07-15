package Fantasy.Jobs.Expressions;

public class InvokeFunctionEventArgs extends EventArgs
{

	public InvokeFunctionEventArgs(java.lang.Class t, String functionName, Object[] arguments)
	{
		this.setType(t);
		this.setFunctionName(functionName);
		this.setArguments(arguments);
		this.setHandled(false);
		this.setResult(null);
	}

	private boolean privateHandled;
	public final boolean getHandled()
	{
		return privateHandled;
	}
	public final void setHandled(boolean value)
	{
		privateHandled = value;
	}

	private java.lang.Class privateType;
	public final java.lang.Class getType()
	{
		return privateType;
	}
	private void setType(java.lang.Class value)
	{
		privateType = value;
	}

	private String privateFunctionName;
	public final String getFunctionName()
	{
		return privateFunctionName;
	}
	private void setFunctionName(String value)
	{
		privateFunctionName = value;
	}

	private Object[] privateArguments;
	public final Object[] getArguments()
	{
		return privateArguments;
	}
	private void setArguments(Object[] value)
	{
		privateArguments = value;
	}

	private Object privateResult;
	public final Object getResult()
	{
		return privateResult;
	}
	public final void setResult(Object value)
	{
		privateResult = value;
	}


}