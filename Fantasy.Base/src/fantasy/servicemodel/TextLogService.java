package fantasy.servicemodel;

import java.io.*;

import java.lang.reflect.*;
import java.rmi.RemoteException;

public abstract class TextLogService extends AbstractService implements ILogListener
{

	/**
	 * 
	 */
	private static final long serialVersionUID = -6974729768828289250L;
	public TextLogService() throws RemoteException {
		super();
		
	}

	protected abstract OutputStreamWriter getWriter();

	
	String _newline = System.getProperty("line.separator");
	@Override
	public void initializeService() throws Exception
	{
		ILogger logger = this.getSite().getRequiredService(ILogger.class);
		logger.AddListener(this);
		super.initializeService();
	}

	@Override
	public void uninitializeService() throws Exception
	{
		super.uninitializeService();
		ILogger logger  = this.getSite().getRequiredService(ILogger.class);
		logger.RemoveListener(this);
	}


	public void onMessage(String category, MessageImportance importance, String message) throws Exception
	{
		this.getWriter().write(String.format("[%1$s] Message : %2$s", new java.util.Date(), message));
	}

	public void onWaring(String category, Throwable exception, MessageImportance importance, String message) throws Exception
	{
		this.getWriter().write(String.format("[%1$s] Warning : %2$s", new java.util.Date(), message));
		if (exception != null)
		{
			this.writeException(exception, 0);
		}
	}

	private void writeException(Throwable exception, int indent) throws Exception
	{
		writeIndent(indent);
		this.writeLine("Type: " + exception.getClass().toString());
		writeIndent(indent);
		this.writeLine("Message: " + exception.getMessage());
		writeIndent(indent);
		
		
		StringWriter sw = new StringWriter();
	    PrintWriter pw = new PrintWriter(sw);
	    exception.printStackTrace(pw);

		
		this.writeLine("StackTrace: " + sw.toString());
		


		
		@SuppressWarnings("rawtypes")
		Class type = exception.getClass();
        while(type != Throwable.class)
        {
        	for(Method method : type.getDeclaredMethods())
        	{
        		if(Modifier.isPublic(method.getModifiers()) && method.getName().startsWith("get") && method.getParameterTypes().length == 0)
        		{
        			Object value = method.invoke(exception);
        			writeIndent(indent);
        			this.writeLine(String.format("%1$s : %2$s", method.getName(), value != null ? value : "null"));
        		}
        	}
        	type = type.getSuperclass();
        }

		if (exception.getCause() != null)
		{
			writeIndent(indent);
			this.writeLine("InnerException");
			this.writeException(exception.getCause(), indent + 1);
		}


	}
	
	private void writeLine(String str) throws Exception
	{
		this.getWriter().write(str);
		this.getWriter().write(_newline);
		this.getWriter().flush();
	}

	private void writeIndent(int indent) throws Exception
	{
		for (int i = 0; i < indent; i++)
		{
			getWriter().write("  ");
		}
	}

	public void onError(String category, Throwable exception, String message) throws Exception
	{
		this.writeLine(String.format("[%1$s] Error : %2$s", new java.util.Date(), message));
		if (exception != null)
		{
			this.writeException(exception, 0);
		}
	}

}