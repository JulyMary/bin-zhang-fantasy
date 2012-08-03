package fantasy.servicemodel;

import java.io.*;
import java.lang.reflect.*;
import java.rmi.RemoteException;
import java.text.SimpleDateFormat;

import org.jdom2.*;
import org.jdom2.output.*;
import java.util.*;

import fantasy.*;

public abstract class LogFileService extends AbstractService implements ILogListener
{


	/**
	 * 
	 */
	private static final long serialVersionUID = 4525102027735701412L;
	public LogFileService() throws RemoteException {
		super();
		// TODO Auto-generated constructor stub
	}


	String _newline = System.getProperty("line.separator");
	private static SimpleDateFormat _roundTripFormat = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss.SSSSSSS"); 
	protected String getCharset()
	{
		return "UTF-8";
	}
	protected abstract OutputStreamWriter getWriter() throws Exception;

	@Override
	public void initializeService() throws Exception
	{
	
		ILogger logger = (ILogger)((IServiceProvider)this.getSite()).getService2(ILogger.class);
		logger.AddListener(this);
		super.initializeService();
	}

	@Override
	public void uninitializeService() throws Exception
	{
		super.uninitializeService();
		ILogger logger = (ILogger)((IServiceProvider)this.getSite()).getService2(ILogger.class);
		logger.RemoveListener(this);
	}


	XMLOutputter _xmlOutputter =  new XMLOutputter(Format.getCompactFormat().setLineSeparator(""));

	private Object _syncRoot = new Object();
	protected void log(Element element) throws Exception
	{
		synchronized (this._syncRoot)
		{
			OutputStreamWriter writer = this.getWriter();
			writer.write(this._xmlOutputter.outputString(element));
			writer.write(this._newline);
			writer.flush();
		}
	}

	
	private String now()
	{
		
		return _roundTripFormat.format(new Date());
		
	
	}
	
	public  void onMessage(String category, MessageImportance importance, String message) throws Exception
	{
		Element element = new Element("message");
		element.setAttribute("time", now());
		element.setAttribute("category", category);
		element.setAttribute("importance", importance.toString());
		element.setAttribute("text", message);
		this.log(element);
	}


	protected void writeStart() throws Exception
	{
		Element element = new Element("start");
		element.setAttribute("time", now());
		element.setAttribute("category", "Start");
		element.setAttribute("importance", MessageImportance.High.toString());
		element.setAttribute("text", String.format("Start. Local : %1$s", Locale.getDefault().toString()));
		this.log(element);
	}

	public void onWaring(String category, Throwable exception, MessageImportance importance, String message) throws Exception
	{
		Element element = new Element("warning");
		element.setAttribute("time", now());
		element.setAttribute("category", category);
		element.setAttribute("importance", importance.toString());
		element.setAttribute("text", message);
		if(exception != null)
		{
			element.addContent(this.CreateExceptionElement(exception));
		}

		this.log(element);
	}

	protected Element CreateExceptionElement(Throwable exception) throws Exception
	{
		Element element = new Element("exception");
		element.setAttribute("type", exception.getClass().toString());
		element.setAttribute("message", exception.getMessage());
		
		StringWriter sw = new StringWriter();
	    PrintWriter pw = new PrintWriter(sw);
	    exception.printStackTrace(pw);

		element.setAttribute("stackTrace", sw.toString());
		
        @SuppressWarnings("rawtypes")
		Class type = exception.getClass();
        while(type != Throwable.class)
        {
        	for(Method method : type.getDeclaredMethods())
        	{
        		if(Modifier.isPublic(method.getModifiers()) && method.getName().startsWith("get") && method.getParameterTypes().length == 0)
        		{
        			Object value = method.invoke(exception);
        			element.setAttribute(method.getName(), value != null ? value.toString() : "null");
        		}
        	}
        	type = type.getSuperclass();
        }

		

		if (exception.getCause() != null)
		{
			element.addContent(this.CreateExceptionElement(exception.getCause()));
		}

		return element;
	}

	public void onError(String category, Throwable exception, String message) throws Exception
	{
		Element element = new Element("error");
		element.setAttribute("time", now());
		element.setAttribute("category", category);
		element.setAttribute("importance", MessageImportance.High.toString());
		element.setAttribute("text", message);
		if (exception != null)
		{
			element.addContent(this.CreateExceptionElement(exception));
		}

		this.log(element);
	}

}