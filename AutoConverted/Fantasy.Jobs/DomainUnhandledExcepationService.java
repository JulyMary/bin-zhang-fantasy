package Fantasy.Jobs;

import Fantasy.ServiceModel.*;

public class DomainUnhandledExcepationService extends AbstractService
{
	public DomainUnhandledExcepationService()
	{
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
		AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
	}




	private void CurrentDomain_UnhandledException(Object sender, UnhandledExceptionEventArgs e)
	{
		RuntimeException error = (RuntimeException)((e.ExceptionObject instanceof RuntimeException) ? e.ExceptionObject : null);
		if (! IsKnownError(error))
		{
			if (this.Site != null)
			{
				ILogger logger = this.Site.<ILogger>GetService();
				if (logger != null)
				{

					if (error != null)
					{
						logger.LogError("Domain", error, "An unhandled CLR  exception is throwed by current domain.");
					}
					else
					{
						logger.LogError("Domain", "An unhandled none CLR  exception is throwed by current domain. exception object : {0}", e.ExceptionObject.toString());
					}

					if (e.IsTerminating)
					{
						logger.LogError("Domain", "CLR is terminating.");
					}
				}
			}
		}
	}

	private boolean IsKnownError(RuntimeException error)
	{
		boolean rs = false;
		if (error != null)
		{
			if (error instanceof ThreadAbortException)
			{
				rs = true;
			}
			else if(WCFExceptionHandler.CanCatch(error))
			{
				rs = true;
			}
		}
		return rs;
	}
}