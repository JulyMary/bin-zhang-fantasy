package fantasy.servicemodel;

import java.rmi.RemoteException;

import fantasy.*;

public class LogService extends AbstractService implements ILogger
{
	public LogService() throws RemoteException {
		super();
		
	}


	/**
	 * 
	 */
	private static final long serialVersionUID = -7349623958104505450L;
	private Object _syncRoot = new Object();

	private void FireEvent(Action1<ILogListener> method)
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
				method.act(listener);
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


	public static final int MAX_MESSAGE_LENGTH = 1024;
	public final void LogMessage(final String category, final MessageImportance importance, String message, Object... messageArgs)
	{
		message = String.format(message, messageArgs);
		if (message != null && message.length() > MAX_MESSAGE_LENGTH)
		{
			message = message.substring(0, MAX_MESSAGE_LENGTH) +"...";
		}
		final String message2 = message;

		this.FireEvent(new Action1<ILogListener>()
		{
			public void act(ILogListener listener) throws Exception
			{
			    listener.onMessage(category, importance, message2);
			}
		}
	   );
	}

	public final void LogMessage(String category, String message, Object... messageArgs)
	{
		this.LogMessage(category, MessageImportance.Low, message, messageArgs);
	}

	public final void LogWarning(final String category, final Throwable exception, final MessageImportance importance, String message, Object... messageArgs)
	{
		message = String.format(message, messageArgs);
		if (message != null && message.length() > MAX_MESSAGE_LENGTH)
		{
			message = message.substring(0, MAX_MESSAGE_LENGTH) + "...";
		}
		
		final String message2 = message;
		this.FireEvent(new Action1<ILogListener>()
				{
					public void act(ILogListener listener) throws Exception
					{
						listener.onWaring(category, exception, importance, message2);
					}
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

	public final void LogError(final String category, final Throwable exception, String message, Object... messageArgs)
	{
		message = String.format(message, messageArgs);
		
		final String message2 = message;
		this.FireEvent(new Action1<ILogListener>()
				{
					public void act(ILogListener listener) throws Exception
					{
						listener.onError(category, exception, message2);
					}
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

}