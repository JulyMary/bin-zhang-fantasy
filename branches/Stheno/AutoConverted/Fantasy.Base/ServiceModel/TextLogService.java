package Fantasy.ServiceModel;

public abstract class TextLogService extends AbstractService implements ILogListener
{

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
		///#region ILogListener Members

	public final void OnMessage(String category, MessageImportance importance, String message)
	{
		this.GetWriter().WriteLine(String.format("[%1$s] Message : %2$s", new java.util.Date(), message));
	}

	public final void OnWaring(String category, RuntimeException exception, MessageImportance importance, String message)
	{
		this.GetWriter().WriteLine(String.format("[%1$s] Warning : %2$s", new java.util.Date(), message));
		if (exception != null)
		{
			this.WriteException(exception, 0);
		}
	}

	private void WriteException(RuntimeException exception, int indent)
	{
		WriteIndent(indent);
		this.GetWriter().WriteLine("Type: " + exception.getClass().toString());
		WriteIndent(indent);
		this.GetWriter().WriteLine("Message: " + exception.getMessage());
		WriteIndent(indent);
		this.GetWriter().WriteLine("Source: " + exception.Source);
		WriteIndent(indent);
		this.GetWriter().WriteLine("TargetSite: " + exception.TargetSite);
		WriteIndent(indent);
		this.GetWriter().WriteLine("StackTrace: " + exception.StackTrace);
		WriteIndent(indent);
		this.GetWriter().WriteLine("HelpLink: " + exception.HelpLink);

//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
		var query = from prop in exception.getClass().GetProperties() where prop.DeclaringType != RuntimeException.class && prop.GetIndexParameters().getLength() == 0 select prop;

		for (PropertyInfo pi : query)
		{
			Object value = pi.GetValue(exception, null);
			WriteIndent(indent);
			this.GetWriter().WriteLine(String.format("%1$s : %2$s", pi.getName(), (value != null) ? value : "null"));
		}

		if (exception.getCause() != null)
		{
			WriteIndent(indent);
			this.GetWriter().WriteLine("InnerException");
			this.WriteException(exception.getCause(), indent + 1);
		}


	}

	private void WriteIndent(int indent)
	{
		for (int i = 0; i < indent; i++)
		{
			GetWriter().Write("  ");
		}
	}

	public final void OnError(String category, RuntimeException exception, String message)
	{
		this.GetWriter().WriteLine(String.format("[%1$s] Error : %2$s", new java.util.Date(), message));
		if (exception != null)
		{
			this.WriteException(exception, 0);
		}
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}