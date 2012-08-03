package fantasy.jobs;

import java.rmi.RemoteException;
import java.util.*;

import fantasy.*;
import fantasy.tracking.*;
import fantasy.servicemodel.*;

public class TrackProviderService extends AbstractService implements ITrackProvider, IJobEngineEventHandler, IDisposable

{

	/**
	 * 
	 */
	private static final long serialVersionUID = -6317840824010522511L;



	public TrackProviderService() throws RemoteException {
		super();
		
	}


	private ITrackProvider _provider;

	private java.util.HashMap<String, Object> _initValues = new java.util.HashMap<String, Object>();



	@Override
	public void initializeService() throws Exception
	{
		IJobEngine engine = this.getSite().getRequiredService(IJobEngine.class);
		engine.AddHandler(this);
		super.initializeService();
	}
	
	@Override 
	public void uninitializeService() throws Exception
	{
		super.uninitializeService();
		this.dispose();
	}

	public final UUID getId()
	{
		try
		{
		return _provider != null ? _provider.getId() : this.getSite().getRequiredService(IJobEngine.class).getJobId();
		}
		catch(Exception e)
		{
			return null;
		}
	}

	public final String getName()
	{
		return _provider.getName();
	}

	public final String getCategory()
	{
		return _provider != null ? _provider.getCategory() : "";
	}

	public final Object getProperty(String name)
	{
		return _provider != null ? _provider.getProperty(name) :  _initValues.get(name);
	}
	
	@SuppressWarnings("unchecked")
	@Override
	public <T> T getProperty(String name, T defaultValue) {
		Object rs = this.getProperty(name);
		
		return rs != null ? (T)rs : defaultValue;
	}
	
	public final void setProperty(String name, Object value)
	{
		if (_provider != null)
		{
			_provider.setProperty(name, value);
		}
		else
		{
			_initValues.put(name, value);
		}
	}

	public final String[] getPropertyNames()
	{
		return _provider != null ? _provider.getPropertyNames() : _initValues.keySet().toArray(new String[0]);
	}

	


	public void HandleStart(IJobEngine sender)
	{

	}

	public void HandleResume(IJobEngine sender)
	{

	}

	public void HandleExit(IJobEngine sender, JobExitEventArgs e)
	{



	}

	
	private ITrackManager _manager;
	
	public void HandleLoad(IJobEngine sender) throws Exception
	{
		IJob job = this.getSite().getService(IJob.class);

		String name = job.GetProperty("name");
		if (StringUtils2.isNullOrWhiteSpace(name))
		{
			name = job.getTemplateName() + new java.util.Date().toString();
		}

		_manager = TrackManagerFactory.createTrackManager();

		_provider = _manager.getProvider(job.getID(), name, "Jobs." + job.getTemplateName(), _initValues);
	}


	public final void dispose()
	{
		if(_provider != null)
		{
		    _provider.dispose();
		}
	}

}