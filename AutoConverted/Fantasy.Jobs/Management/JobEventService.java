package Fantasy.Jobs.Management;

import Fantasy.ServiceModel.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Reentrant, Namespace=Consts.JobServiceNamespaceURI)]
public class JobEventService implements IJobEventService
{
	private IJobEventHandler _handler;
	private IJobQueue _queue;
	private boolean _expired = false;
	public JobEventService()
	{


	}

	private void JobChanged(Object sender, JobQueueEventArgs e)
	{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		Task.Factory.StartNew(() =>
		{
			try
			{
				_handler.Changed(e.getJob());
			}
			catch (java.lang.Exception e)
			{
				_expired = true;
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
				_queue.Added -= new EventHandler<JobQueueEventArgs>(JobAdded);
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
				_queue.Changed -= new EventHandler<JobQueueEventArgs>(JobChanged);
			}
		}
	   );
	}

	private void JobAdded(Object sender, JobQueueEventArgs e)
	{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		Task.Factory.StartNew(() =>
		{
			try
			{
				_handler.Added(e.getJob());
			}
			catch (java.lang.Exception e)
			{
				_expired = true;
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
				_queue.Added -= new EventHandler<JobQueueEventArgs>(JobAdded);
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
				_queue.Changed -= new EventHandler<JobQueueEventArgs>(JobChanged);
			}
		}
	   );
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IJobEventService Members

	public final void Echo()
	{
		if (_expired)
		{
			throw new FaultException<CallbackExpiredException>(new CallbackExpiredException(), "Callback is expired.");
		}

	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IJobEventService Members

	public final void Connect()
	{
		_handler = OperationContext.Current.<IJobEventHandler>GetCallbackChannel();
		_queue = JobManager.getDefault().<IJobQueue>GetRequiredService();
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
		_queue.Added += new EventHandler<JobQueueEventArgs>(JobAdded);
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
		_queue.Changed += new EventHandler<JobQueueEventArgs>(JobChanged);
	}

	public final void Disconnect()
	{
		_expired = true;
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
		_queue.Added -= new EventHandler<JobQueueEventArgs>(JobAdded);
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
		_queue.Changed -= new EventHandler<JobQueueEventArgs>(JobChanged);
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}