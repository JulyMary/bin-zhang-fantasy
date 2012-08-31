package fantasy.jobs.solar;

import java.net.InetAddress;
import java.rmi.RemoteException;
import java.util.*;
import java.util.concurrent.Executors;
import java.util.concurrent.ScheduledExecutorService;
import java.util.concurrent.TimeUnit;

import org.apache.commons.lang3.ObjectUtils;

import fantasy.collections.*;
import fantasy.jobs.management.*;
import fantasy.jobs.resources.*;
import fantasy.servicemodel.*;
import fantasy.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
public class SatelliteService extends AbstractService implements ISatellite
{

	

	/**
	 * 
	 */
	private static final long serialVersionUID = 2808005366760470593L;

	public SatelliteService() throws RemoteException {
		super();
		
	}


	private IJobController _controller;

	private String _name;

	private ScheduledExecutorService _executor;

	private IComputerLoadFactorEvaluator _loadFactorEvaluator;
	private IResourceManager _resourceManager;
	private ISolar _solar;
	
	private HashSet<ISatelliteListener> _listeners = new HashSet<ISatelliteListener>();

	@Override
	public void initializeService() throws Exception
	{
		this._controller = this.getSite().getRequiredService(IJobController.class);
		_name = String.format("%1$s:%2$d", InetAddress.getLocalHost().getHostName(), new Random().nextInt());

		this._loadFactorEvaluator = new Enumerable<IComputerLoadFactorEvaluator>(AddIn.CreateObjects(IComputerLoadFactorEvaluator.class, "jobService/computerLoadFactorEvaluator")).singleOrDefault();

		if (this._loadFactorEvaluator != null && this._loadFactorEvaluator instanceof IObjectWithSite)
		{
			((IObjectWithSite)_loadFactorEvaluator).setSite(this.getSite());
		}

		this._resourceManager = this.getSite().getService(IResourceManager.class);
		if (_resourceManager != null)
		{
			this._resourceManager.addListener(new IResourceManagerListener(){

				@Override
				public void available(EventObject e) throws Exception {
					SatelliteService.this.getSite().getRequiredService(ISolarActionQueue.class).enqueue(new Action1<ISolar>(){

						@Override
						public void call(ISolar arg) throws Exception {
							arg.resourceAvaiable();
							
						}});;
					
				
					
				}});
		
		}

		_executor = Executors.newScheduledThreadPool(1);
		_executor.scheduleAtFixedRate(new Runnable(){

			@Override
			public void run() {
				tryCreateHandler();
			}}, 15, 15, TimeUnit.SECONDS);
		


		super.initializeService();
	}

	

	

	private void tryCreateHandler()
	{

		ILogger logger = null;
		try {
			logger = this.getSite().getService(ILogger.class);
		} catch (Exception e) {
		
			e.printStackTrace();
		}

		if (this._solar != null)
		{
			try
			{
				this._solar.echo();
			}
			
			catch (Exception error)
			{
				
				_solar = null;
				if (!WCFExceptionHandler.canCatch(error))
				{
					Log.SafeLogError(logger, "Solar", error, "An error ocuurs when calling solar.");
					
				}
				

			}
		}

		if (_solar == null)
		{

			try
			{
				_solar = ClientFactory.create(ISolar.class);
				_solar.connect(this._name, this);

				Log.SafeLogMessage(logger, "Satellite", "Success connect to solar service.");

			}
			
			catch (Exception error)
			{

			
				Log.SafeLogWarning(logger, "Satellite", error, MessageImportance.Normal, "Failed to connect to solor service.");
				_solar = null;

				

			}
		}
	}




	@Override
	public void uninitializeService() throws Exception
	{
		this._executor.shutdown();
		this._executor.awaitTermination(Long.MAX_VALUE, TimeUnit.MILLISECONDS);
		if (this._solar != null)
		{

			try
			{
				this._solar.disconnect(this._name);
			}
			catch (java.lang.Exception e)
			{

			}

			
		}

		super.uninitializeService();
	}



	public final void echo()
	{

	}



	public final void onJobAdded(JobMetaData job) throws Exception
	{
		for(ISatelliteListener listener : this._listeners)
		{
			listener.Added(job);
		}
		
		
	}

	public final void onJobChanged(JobMetaData job) throws Exception
	{
		for(ISatelliteListener listener : this._listeners)
		{
			listener.Changed(job);
		}
		
	}

	public final boolean isResourceAvailable(ResourceParameter[] parameters) throws Exception
	{

		if (_resourceManager != null)
		{
			return _resourceManager.IsAvailable(parameters);
		}
		else
		{
			return true;
		}
	}

	@Override
	public final double getLoadFactor() throws Exception
	{
		return this._loadFactorEvaluator != null ? this._loadFactorEvaluator.Evaluate() :this._controller.GetAvailableProcessCount();
	}

	@Override
	public final void requestStartJob(JobMetaData job) throws Exception
	{
		ILogger logger = this.getSite().getService(ILogger.class);
		
		
		Log.SafeLogMessage(logger, "Satellite", "Start job %$1s (%$2s).", job.getName(), job.getId());
		this._controller.StartJob(job);
	}

	@Override
	public final void requestResume(JobMetaData job) throws Exception
	{
		ILogger logger = this.getSite().getService(ILogger.class);
		Log.SafeLogMessage(logger, "Satellite", "Resume job %$1s (%$2s).", job.getName(), job.getId());
		this._controller.Resume(job);
	}

	@Override
	public final void requestCancel(UUID id) throws Exception
	{
		ILogger logger = this.getSite().getService(ILogger.class);
		Log.SafeLogMessage(logger, "Satellite", "Cancel job %$1s.", id);
		this._controller.Cancel(id);
	}

	@Override
	public final void requestSuspend(UUID id) throws Exception
	{
		ILogger logger = this.getSite().getService(ILogger.class);
		Log.SafeLogMessage(logger, "Satellite", "Suspend job %1$s.", id);
		this._controller.Suspend(id);
	}

	public final void requestUserPause(UUID id) throws Exception
	{
		ILogger logger = this.getSite().getService(ILogger.class);
		Log.SafeLogMessage(logger, "Satellite", "Pause job %1$s.", id);
		this._controller.UserPause(id);
	}



	public final void requestSuspendAll() throws Exception
	{
		ILogger logger = this.getSite().getService(ILogger.class);
		Log.SafeLogMessage(logger, "Satellite", "Suspend all running jobs.");
		_controller.SuspendAll(true);
	}


	public final boolean isRunning(final UUID id) throws Exception
	{
		return new Enumerable<JobMetaData>(this._controller.GetRunningJobs()).any(new Predicate<JobMetaData>(){

			@Override
			public boolean evaluate(JobMetaData job) throws Exception {
				return ObjectUtils.equals(job.getId(), id);
			}});

	}





	@Override
	public void addListener(ISatelliteListener listener) {
		this._listeners.add(listener);
		
	}





	@Override
	public void removeListener(ISatelliteListener listener) {
		this._listeners.remove(listener);
		
	}





	@Override
	public String getName() {
		return _name;
	}

}