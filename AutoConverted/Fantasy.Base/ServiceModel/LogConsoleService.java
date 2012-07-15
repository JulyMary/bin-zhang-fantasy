package Fantasy.ServiceModel;

import Fantasy.ServiceModel.*;

public class LogConsoleService extends AbstractService implements ILogListener
{
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

	//public void OnStatusChanged(string status)
	//{
	//    Debug.WriteLine("Status: " + status); 
	//}

	public final void OnMessage(String category, MessageImportance importance, String message)
	{
		System.out.println("Message: " + message);
	}

	public final void OnWaring(String category, RuntimeException exception, MessageImportance importance, String message)
	{
		System.out.println("Warning: " + message);
	}

	public final void OnError(String category, RuntimeException exception, String message)
	{
		System.out.println("Error: " + message);
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}