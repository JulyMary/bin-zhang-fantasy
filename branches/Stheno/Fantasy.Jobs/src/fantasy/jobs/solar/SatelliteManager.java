package fantasy.jobs.solar;

import java.rmi.RemoteException;
import java.util.concurrent.*;

import org.apache.commons.lang3.StringUtils;

import fantasy.collections.*;
import fantasy.jobs.management.*;
import fantasy.servicemodel.*;
import fantasy.*;

public class SatelliteManager extends AbstractService
{
	/**
	 * 
	 */
	private static final long serialVersionUID = -2649936598311903847L;
	ScheduledExecutorService _executor;


	public SatelliteManager() throws RemoteException {
		super();
		
	}


	private IJobDispatcher _dispatcher;

	private Object _syncRoot = new Object();

	private java.util.ArrayList<ActionData> _actionQueue = new java.util.ArrayList<ActionData>();

	


	@Override
	public void initializeService() throws Exception
	{
		this._dispatcher = this.getSite().getRequiredService(IJobDispatcher.class);
		
		_executor =  Executors.newScheduledThreadPool(2);
		_executor.scheduleAtFixedRate(new Runnable(){

			@Override
			public void run() {
				SatelliteManager.this.refresh();
			}}, 15, 15, TimeUnit.SECONDS);
		
		_executor.scheduleAtFixedRate(new Runnable(){

			@Override
			public void run() {
				retryActions();
				
			}}, 60, 60, TimeUnit.SECONDS);



		super.initializeService();
	}


	@Override
	public void uninitializeService() throws Exception
	{
		
		_executor.shutdown();
		_executor.awaitTermination(Long.MAX_VALUE, TimeUnit.MILLISECONDS);
		
		ExecutorService exec =  Executors.newCachedThreadPool();
		for(final SatelliteSite site : this._satellites)
		{
			exec.execute(new Runnable(){

				@Override
				public void run() {
					try {
						site.getSatellite().requestSuspendAll();
					} catch (Exception e) {
					
						e.printStackTrace();
					}
					
				}});
			
		}
		
		exec.shutdown();
		exec.awaitTermination(Long.MAX_VALUE, TimeUnit.MILLISECONDS);

		super.uninitializeService();
	}


	
	
	public final <T> boolean SafeCallSatellite(final String name, Func1<ISatellite, T> function, RefObject<T> value)
	{
		boolean rs = false;
		SatelliteSite site = null;
		value.argvalue = null;
		try
		{

			site = new Enumerable<SatelliteSite>(this._satellites).firstOrDefault(new Predicate<SatelliteSite>(){

				@Override
				public boolean evaluate(SatelliteSite site) throws Exception {
					return StringUtils.equals(site.getName(), name);
				}});
			if (site != null)
			{
				value.argvalue = function.call(site.getSatellite());
			}

			rs = true;

		}
		catch (java.lang.Exception e)
		{


		}
		return rs;
	}

	public final void enqueue(String name, Object state, Action2<ISatellite, Object> action, Action1<Object> failAction)
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

	private boolean TryCallAction(final ActionData data)
	{
		boolean rs = false;
		try
		{
			SatelliteSite site = new Enumerable<SatelliteSite>(this._satellites).firstOrDefault(new Predicate<SatelliteSite>(){

				@Override
				public boolean evaluate(SatelliteSite site) throws Exception {
					return StringUtils.equals(site.getName(), data.Satellite);
				}});
			if (site != null)
			{
				data.Action.call(site.getSatellite(), data.State);
				rs = true;
			}


		}
		catch (java.lang.Exception e)
		{
		}
		return rs;

	}


	public final boolean isValid(final ISatellite satellite)
	{
		synchronized (_syncRoot)
		{
			return new Enumerable<SatelliteSite>(this._satellites).any(new Predicate<SatelliteSite>(){

				@Override
				public boolean evaluate(SatelliteSite site) throws Exception {
					return site.getSatellite() == satellite;
				}});

		}
	}


	public final void updateEchoTime(final String name) throws Exception
	{
		SatelliteSite site = new Enumerable<SatelliteSite>(this._satellites).firstOrDefault(new Predicate<SatelliteSite>(){

			@Override
			public boolean evaluate(SatelliteSite site) throws Exception {
				return StringUtils.equals(site.getName(), name);
			}});
		if (site != null)
		{
			site.setLastEchoTime(new java.util.Date());
		}
	}

	public final void registerSatellite(final String name, ISatellite satellite) throws Exception
	{

		synchronized (_syncRoot)
		{
			SatelliteSite site = new Enumerable<SatelliteSite>(this._satellites).firstOrDefault(new Predicate<SatelliteSite>(){

				@Override
				public boolean evaluate(SatelliteSite site) throws Exception {
					return StringUtils.equals(site.getName(), name);
				}});
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

		this._dispatcher.tryDispatch();

		ILogger logger = this.getSite().getService(ILogger.class);
		if (logger != null)
		{
			logger.LogMessage("SatelliteManager", "Satellite %1$s is connected", name);
		}


	}

	public final void unregisterSatellite(final String name) throws Exception
	{
		synchronized (_syncRoot)
		{
			SatelliteSite site = new Enumerable<SatelliteSite>(this._satellites).firstOrDefault(new Predicate<SatelliteSite>(){

				@Override
				public boolean evaluate(SatelliteSite site) throws Exception {
					return StringUtils.equals(site.getName(), name);
				}});
			this._dispatcher.tryDispatch();
			ILogger logger = this.getSite().getService(ILogger.class);
			if (logger != null)
			{
				logger.LogMessage("SatelliteManager", "Satellite %1$s is disconnected", site.getName());
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


	private void refresh()
	{
		TimeSpan timeout = new TimeSpan(0, 0, 15);
		ILogger logger = null;
		try {
			logger = this.getSite().getService(ILogger.class);
		} catch (Exception e) {
		
			e.printStackTrace();
		}
		
			java.util.ArrayList<SatelliteSite> list;
			synchronized (_syncRoot)
			{
				list = new java.util.ArrayList<SatelliteSite>(_satellites);
			}

			for (SatelliteSite site : list)
			{
				try
				{
					if (new java.util.Date().getTime() - site.getLastEchoTime().getTime() > timeout.getTotalMilliseconds())
					{
						site.getSatellite().echo();
						site.setLastEchoTime(new java.util.Date());
					}
				}
				catch(Exception error)
				{
					
						synchronized (_syncRoot)
						{
							_satellites.remove(site);
						}

						if (logger != null)
						{
							logger.LogWarning("SatelliteManager", "Satellite {0} is stop working, remove it from satellite manager.", site.getName());
						}
					
				}
			}


	}


	private void retryActions() 
	{
		
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
							data.FailAction.call(data.State);
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

	private static class ActionData
	{
		public String Satellite;

		public Action2<ISatellite, Object> Action;

		public Action1<Object> FailAction;

		public Object State;

		public int RetryCount;
	}
}