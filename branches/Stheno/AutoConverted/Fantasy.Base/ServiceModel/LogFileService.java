package Fantasy.ServiceModel;

public abstract class LogFileService extends AbstractService implements ILogListener
{



	private static final String _timePattern = "u";

	protected abstract StreamWriter GetWriter();

	@Override
	public void InitializeService()
	{
		ILogger logger = (ILogger)((IServiceProvider)this.Site).GetService(ILogger.class);
		logger.AddListener(this);
		super.InitializeService();
	}

	@Override
	public void UninitializeService()
	{
		super.UninitializeService();
		ILogger logger = (ILogger)((IServiceProvider)this.Site).GetService(ILogger.class);
		logger.RemoveListener(this);
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IJobLoggerListener Members


	private Object _syncRoot = new Object();
	protected void Log(XElement element)
	{
		synchronized (this._syncRoot)
		{
			StreamWriter writer = this.GetWriter();
			writer.WriteLine(element.ToString(SaveOptions.DisableFormatting | SaveOptions.OmitDuplicateNamespaces));
			writer.Flush();
		}
	}

	public final void OnMessage(String category, MessageImportance importance, String message)
	{
		XElement element = new XElement("message");
		element.SetAttributeValue("time", new java.util.Date().ToUniversalTime().ToString(_timePattern));
		element.SetAttributeValue("category", category);
		element.SetAttributeValue("importance", importance.toString());
		element.SetAttributeValue("text", message);
		this.Log(element);
	}


	protected void WriteStart()
	{
		XElement element = new XElement("start");
		element.SetAttributeValue("time", new java.util.Date().ToUniversalTime().ToString(_timePattern));
		element.SetAttributeValue("category", "Start");
		element.SetAttributeValue("importance", MessageImportance.High.toString());
		element.SetAttributeValue("text", String.format("Start. Culture : %1$s; UI Culture: %2$s.", Thread.currentThread().CurrentCulture, Thread.currentThread().CurrentUICulture));
		this.Log(element);
	}

	public final void OnWaring(String category, RuntimeException exception, MessageImportance importance, String message)
	{
		XElement element = new XElement("warning");
		element.SetAttributeValue("time", new java.util.Date().ToUniversalTime().ToString(_timePattern));
		element.SetAttributeValue("category", category);
		element.SetAttributeValue("importance", importance.toString());
		element.SetAttributeValue("text", message);
		if(exception != null)
		{
			element.Add(this.CreateExceptionElement(exception));
		}

		this.Log(element);
	}

	private XElement CreateExceptionElement(RuntimeException exception)
	{
		XElement element = new XElement("exception");
		element.SetAttributeValue("type", exception.getClass().toString());
		element.SetAttributeValue("message", exception.getMessage());
		element.SetAttributeValue("source", exception.Source);
		element.SetAttributeValue("targetSite", exception.TargetSite.toString());
		element.SetAttributeValue("stackTrace", exception.StackTrace);
		element.SetAttributeValue("helpLink", exception.HelpLink);

//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
		var query = from prop in exception.getClass().GetProperties() where prop.DeclaringType != RuntimeException.class && prop.GetIndexParameters().getLength() == 0 select prop;

		for (PropertyInfo pi : query)
		{
			Object value = pi.GetValue(exception, null);
			element.SetAttributeValue(pi.getName(), value != null ? value.toString() : "null");
		}

		if (exception.getCause() != null)
		{
			element.Add(this.CreateExceptionElement(exception.getCause()));
		}

		return element;
	}

	public final void OnError(String category, RuntimeException exception, String message)
	{
		XElement element = new XElement("error");
		element.SetAttributeValue("time", new java.util.Date().ToUniversalTime().ToString(_timePattern));
		element.SetAttributeValue("category", category);
		element.SetAttributeValue("importance", MessageImportance.High.toString());
		element.SetAttributeValue("text", message);
		if (exception != null)
		{
			element.Add(this.CreateExceptionElement(exception));
		}

		this.Log(element);
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}