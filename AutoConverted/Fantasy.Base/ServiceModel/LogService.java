package Fantasy.ServiceModel;

public class LogService extends AbstractService implements ILogger
{
	private Object _syncRoot = new Object();

	private void FireEvent(MethodInvoker<ILogListener> method)
	{
		ILogListener[] listeners;
		synchronized (_syncRoot)
		{
			 listeners = this._listeners.toArray(new ILogListener[]{});
		}

		java.util.ArrayList<ILogListener> expired = new java.util.ArrayList<ILogListener>();
		for(ILogListener listener : listeners)
		{
			try
			{
				method(listener);
			}
			catch (java.lang.Exception e)
			{
				expired.add(listener);
			}
		}
		synchronized(_syncRoot)
		{
			for (ILogListener listener : expired)
			{
				_listeners.remove(listener);
			}
		}
	}


	private java.util.ArrayList<ILogListener> _listeners = new java.util.ArrayList<ILogListener>();
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IJobLogger Members

	public static final int MAX_MESSAGE_LENGTH = 1024;
	public final void LogMessage(String category, MessageImportance importance, String message, Object... messageArgs)
	{
		message = String.format(message, messageArgs);
		if (message != null && message.length() > MAX_MESSAGE_LENGTH)
		{
			message = message.substring(0, MAX_MESSAGE_LENGTH) +"...";
		}
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		this.FireEvent(delegate(ILogListener listener)
		{
			listener.OnMessage(category, importance, message);
		}
	   );
	}

	public final void LogMessage(String category, String message, Object... messageArgs)
	{
		this.LogMessage(category, MessageImportance.Low, message, messageArgs);
	}

	public final void LogWarning(String category, RuntimeException exception, MessageImportance importance, String message, Object... messageArgs)
	{
		message = String.format(message, messageArgs);
		if (message != null && message.length() > MAX_MESSAGE_LENGTH)
		{
			message = message.substring(0, MAX_MESSAGE_LENGTH) + "...";
		}
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		this.FireEvent(delegate(ILogListener listener)
		{
			listener.OnWaring(category, exception, importance, message);
		}
	   );
	}

	public final void LogWarning(String category, MessageImportance importance, String message, Object... messageArgs)
	{
		this.LogWarning(category, null, importance, message, messageArgs);
	}

	public final void LogWarning(String category, String message, Object... messageArgs)
	{
		this.LogWarning(category, null, MessageImportance.Low, message, messageArgs);
	}

	public final void LogError(String category, RuntimeException exception, String message, Object... messageArgs)
	{
		message = String.format(message, messageArgs);
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		this.FireEvent(delegate(ILogListener listener)
		{
			listener.OnError(category, exception, message);
		}
	   );
	}

	public final void LogError(String category, String message, Object... messageArgs)
	{
		this.LogError(category, null, message, messageArgs);
	}

	public final void AddListener(ILogListener listener)
	{
		synchronized (_syncRoot)
		{
			this._listeners.add(listener);
		}
	}

	public final void RemoveListener(ILogListener listener)
	{
		synchronized (_syncRoot)
		{
			this._listeners.remove(listener);
		}
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}