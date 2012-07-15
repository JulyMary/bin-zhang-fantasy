package Fantasy.Jobs.Tasks;

import Fantasy.ServiceModel.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Task("execute", Consts.XNamespaceURI, Description="Execute an external process with command line")]
public class ExecuteTask extends ObjectWithSite implements ITask
{

	public ExecuteTask()
	{
		setTimeout(TimeSpan.getMaxValue());
		this.setWorkingDirectory("");
		this.setArguments("");
		this.setWaitForExit(false);
	}
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITask Members

	public final boolean Execute()
	{
		ILogger logger = this.Site.<ILogger>GetService();
		ProcessStartInfo tempVar = new ProcessStartInfo();
		tempVar.FileName = this.getFileName();
		tempVar.Arguments = this.getArguments();
		tempVar.CreateNoWindow = true;
		tempVar.ErrorDialog = false;
		tempVar.WorkingDirectory = this.getWorkingDirectory();
		tempVar.RedirectStandardError = this.getWaitForExit();
		tempVar.RedirectStandardOutput = this.getWaitForExit();
		ProcessStartInfo si = tempVar;

		Process process = Process.Start(si);



		if (this.getWaitForExit())
		{
			WaitProcess(process);
		}

		return true;


	}


	private void WriteOutput(Object state)
	{
		try
		{
			ILogger logger = this.Site.<ILogger>GetService();
			StreamReader reader = (StreamReader)state;
			Regex regex = new Regex("\\{\\d+(:[^\\}]*)?\\}");
			String text;

			do
			{
				text = reader.ReadLine();
				if (text != null)
				{
					text = regex.Replace(text, "");
					logger.SafeLogMessage("Execute", text);
				}
			} while (text != null);
		}
		catch (java.lang.Exception e)
		{
		}

	}

	private void WaitProcess(Process process)
	{


		ThreadFactory.CreateThread(WriteOutput).WithStart(process.StandardOutput);
		ThreadFactory.CreateThread(WriteOutput).WithStart(process.StandardError);
		java.util.Date startTime = new java.util.Date();
		boolean exited = false;
		do
		{
			java.util.Date now = new java.util.Date();
			TimeSpan consumed = now - startTime;

			TimeSpan waitTime = getTimeout() - consumed;

			if (waitTime > _refreshInterval)
			{
				waitTime = _refreshInterval;
			}

			try
			{
				exited = process.HasExited;
			}
			catch (java.lang.Exception e)
			{
			}

			if (!exited)
			{

				exited = process.WaitForExit(waitTime.Milliseconds);
			}

		} while (!exited);

		try
		{
			this.setExitCode(process.ExitCode);
		}
		catch (java.lang.Exception e2)
		{
		}
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

	private final static TimeSpan _refreshInterval = TimeSpan.FromSeconds(15);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("file", Flags=TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The application to start.")]
	private String privateFileName;
	public final String getFileName()
	{
		return privateFileName;
	}
	public final void setFileName(String value)
	{
		privateFileName = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("arguments", Description="The command-line arguments to use when starting the application.")]
	private String privateArguments;
	public final String getArguments()
	{
		return privateArguments;
	}
	public final void setArguments(String value)
	{
		privateArguments = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("exitCode", Flags=TaskMemberFlags.Output, Description="The code that the associated process specified when it terminated.")]
	private int privateExitCode;
	public final int getExitCode()
	{
		return privateExitCode;
	}
	public final void setExitCode(int value)
	{
		privateExitCode = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("waitForExit", Description="true if the task to wait for application to exit; otherwise false.")]
	 boolean getWaitForExit()
	 void setWaitForExit(boolean value)

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("workingDirectory", Description="The initial directory for the process to be started.")]
	private String privateWorkingDirectory;
	public final String getWorkingDirectory()
	{
		return privateWorkingDirectory;
	}
	public final void setWorkingDirectory(String value)
	{
		privateWorkingDirectory = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("timeout", Description="The amount of time to wai for application to exit.")]
	private TimeSpan privateTimeout = new TimeSpan();
	public final TimeSpan getTimeout()
	{
		return privateTimeout;
	}
	public final void setTimeout(TimeSpan value)
	{
		privateTimeout = value;
	}



}