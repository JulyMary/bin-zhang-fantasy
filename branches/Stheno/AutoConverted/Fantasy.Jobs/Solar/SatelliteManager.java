package Fantasy.Jobs.Solar;

import Fantasy.Jobs.Management.*;
import Fantasy.ServiceModel.*;

public class SatelliteManager extends AbstractService
{
	private IJobDispatcher _dispatcher;

	private Object _syncRoot = new Object();

	private java.util.ArrayList<ActionData> _actionQueue = new java.util.ArrayList<ActionData>();

	private Thread _refreshThread;
	private Thread _actionThread;


	@Override
	public void InitializeService()
	{
		this._dispatcher = this.Site.<IJobDispatcher>GetRequiredService();

		_refreshThread = ThreadFactory.CreateThread(this.Refresh).WithStart();

		_actionThread = ThreadFactory.CreateThread(this.RetryActions).WithStart();


		super.InitializeService();
	}


	@Override
	public void UninitializeService()
	{
		this._actionThread.stop();
		this._refreshThread.stop();
		_actionThread.join();
		_refreshThread.join();


//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		Parallel.ForEach(this._satellites, site =>
		{
			try
			{
				site.Satellite.RequestSuspendAll();
			}
			catch (java.lang.Exception e)
			{
			}
		}
	   );


		super.UninitializeService();
	}


	public final <T> boolean SafeCallSatellite(String name, Func<ISatellite, T> function, RefObject<T> value)
	{
		boolean rs = false;
		SatelliteSite site = null;
		value.argvalue = null;
		try
		{

//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			site = this._satellites.Find(s => name == s.getName());
			if (site != null)
			{
				value.argvalue = function(site.getSatellite());
			}

			rs = true;

		}
		catch (java.lang.Exception e)
		{


		}
		return rs;
	}

	public final void Enqueue(String name, Object state, Action<ISatellite, Object> action, Action<Object> failAction)
	{

		ActionData tempVar = new ActionData();
		tempVar.Action = action;
		tempVar.State=state;
		tempVar.Satellite = name;
		tempVar.FailAction = failAction;
		tempVar.RetryCount = 10;
		ActionData data = tempVar;

		if (!TryCallAction(data))
		{
			synchronized (this._actionQueue)
			{
				this._actionQueue.add(data);
			}
		}

	}

	private boolean TryCallAction(ActionData data)
	{
		boolean rs = false;
		try
		{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			SatelliteSite site = this._satellites.Find(s => data.Satellite.equals(s.getName()));
			if (site != null)
			{
				data.Action(site.getSatellite(), data.State);
				rs = true;
			}


		}
		catch (java.lang.Exception e)
		{
		}
		return rs;

	}


	public final boolean IsValid(ISatellite satellite)
	{
		synchronized (_syncRoot)
		{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			return this._satellites.Find(s => satellite == s.Satellite) != null;
		}
	}


	public final void UpdateEchoTime(String name)
	{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		SatelliteSite site = this._satellites.Find(s => name == s.getName());
		if (site != null)
		{
			site.setLastEchoTime(new java.util.Date());
		}
	}

	public final void RegisterSatellite(String name, ISatellite satellite)
	{

		synchronized (_syncRoot)
		{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			SatelliteSite site = this._satellites.Find(s => name == s.getName());
			if (site != null)
			{
				this._satellites.remove(site);
			}
			SatelliteSite tempVar = new SatelliteSite();
			tempVar.setName(name);
			tempVar.setSatellite(satellite);
			tempVar.setLastEchoTime(new java.util.Date());
			site = tempVar;
			this._satellites.add(site);
		}

		this._dispatcher.TryDispatch();

		ILogger logger = this.Site.<ILogger>GetService();
		if (logger != null)
		{
			logger.LogMessage("SatelliteManager", "Satellite {0} is connected", name);
		}


	}

	public final void UnregisterSatellite(ISatellite satellite)
	{
		synchronized (_syncRoot)
		{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			SatelliteSite site = this._satellites.Find(s => satellite == s.Satellite);
			if (site != null)
			{
				this._satellites.remove(site);
			}
			this._dispatcher.TryDispatch();
			ILogger logger = this.Site.<ILogger>GetService();
			if (logger != null)
			{
				logger.LogMessage("SatelliteManager", "Satellite {0} is disconnected", site.getName());
			}
		}
	}


	private java.util.ArrayList<SatelliteSite> _satellites = new java.util.ArrayList<SatelliteSite>();

	public final java.util.Collection<SatelliteSite> getSatellites()
	{
		synchronized (_syncRoot)
		{
			return new java.util.ArrayList<SatelliteSite>(_satellites);
		}
	}


	private void Refresh()
	{
		TimeSpan timeout = new TimeSpan(0, 0, 15);
		ILogger logger = this.Site.<ILogger>GetService();
		while (true)
		{
			Thread.sleep(15 * 1000);
			java.util.ArrayList<SatelliteSite> list;
			synchronized (_syncRoot)
			{
				list = new java.util.ArrayList<SatelliteSite>(_satellites);
			}

			for (SatelliteSite site : list)
			{
				try
				{
					if (new java.util.Date() - site.getLastEchoTime() > timeout)
					{
						site.getSatellite().Echo();
						site.setLastEchoTime(new java.util.Date());
					}
				}
				catch(RuntimeException error)
				{
					if (WCFExceptionHandler.CanCatch(error))
					{
						synchronized (_syncRoot)
						{
							_satellites.remove(site);
						}

						if (logger != null)
						{
							logger.LogError("Solar", error, "WCF error");

							logger.LogWarning("SatelliteManager", "Satellite {0} is stop working, remove it from satellite manager.", site.getName());
						}
					}
					else
					{
						throw error;
					}
				}
			}


		}
	}


	private void RetryActions()
	{
		while (true)
		{

			Thread.sleep(60 * 1000);
			java.util.ArrayList<ActionData> list;
			synchronized (_actionQueue)
			{
				list = new java.util.ArrayList<ActionData>(this._actionQueue);
			}

			for (ActionData data : list)
			{
				boolean success = this.TryCallAction(data);
				if (!success)
				{
					data.RetryCount--;
					if (data.RetryCount == 0 && data.FailAction != null)
					{
						try
						{
							data.FailAction(data.State);
						}
						catch (java.lang.Exception e)
						{
						}
					}
				}
				if (success || data.RetryCount == 0)
				{
					synchronized (_actionQueue)
					{
						_actionQueue.remove(data);
					}
				}


			}

		}
	}

	private static class ActionData
	{
		public String Satellite;

		public Action<ISatellite, Object> Action;

		public Action<Object> FailAction;

		public Object State;

		public int RetryCount;
	}
}