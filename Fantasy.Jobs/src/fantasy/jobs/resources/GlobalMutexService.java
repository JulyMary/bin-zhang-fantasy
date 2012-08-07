package fantasy.jobs.resources;

import Fantasy.Jobs.Scheduling.*;
import Fantasy.Jobs.Management.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[ServiceBehavior(InstanceContextMode=InstanceContextMode.Single, ConcurrencyMode=ConcurrencyMode.Multiple, Namespace=Consts.JobServiceNamespaceURI)]
public class GlobalMutexService extends WCFSingletonService implements IGlobalMutexService
{

	private java.util.ArrayList<Mutex> _allocated = new java.util.ArrayList<Mutex>();
	private Object _syncRoot = new Object();
	private IScheduleService _scheduleService;
	private IJobDispatcher _jobDispatcher;

	@Override
	public void InitializeService()
	{
		_scheduleService = this.Site.<IScheduleService>GetRequiredService();
		_jobDispatcher = this.Site.<IJobDispatcher>GetService();
		super.InitializeService();
	}

	private static class Mutex
	{
		private String privateName;
		public final String getName()
		{
			return privateName;
		}
		public final void setName(String value)
		{
			privateName = value;
		}

		private long privateCookie;
		public final long getCookie()
		{
			return privateCookie;
		}
		public final void setCookie(long value)
		{
			privateCookie = value;
		}
	}


//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IGlobalMutexService Members

	public final boolean IsAvaiable(String key)
	{
		synchronized (_syncRoot)
		{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			return ! this._allocated.Any(m => String.equals(key, m.getName(), StringComparison.OrdinalIgnoreCase));
		}
	}

	public final boolean Request(String key, TimeSpan timeout)
	{
		synchronized (_syncRoot)
		{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			Mutex mutex = this._allocated.Find(m => String.equals(key, m.getName(), StringComparison.OrdinalIgnoreCase));
			if (mutex == null)
			{
				Mutex tempVar = new Mutex();
				tempVar.setName(key);
				mutex = tempVar;
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
				mutex.setCookie(_scheduleService.Register(new java.util.Date() + timeout, () =>)
				{
					this.Release(mutex);
				}
			   );
				this._allocated.add(mutex);
				return true;
			}
			else
			{
				return false;
			}
		}
	}

	private void Release(Mutex mutex)
	{
		synchronized (_syncRoot)
		{
			_allocated.remove(mutex);
		}
		if (_jobDispatcher != null)
		{
			_jobDispatcher.TryDispatch();
		}

	}

	public final void Release(String key)
	{
		synchronized (_syncRoot)
		{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			Mutex mutex = this._allocated.Find(m => String.equals(key, m.getName(), StringComparison.OrdinalIgnoreCase));
			if (mutex != null)
			{
				this.Release(mutex);
				_scheduleService.Unregister(mutex.getCookie());
			}
		}
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}