package fantasy.jobs;

import Fantasy.Tracking.*;
import fantasy.servicemodel.*;

public class TrackProviderService extends AbstractService implements ITrackProvider, IJobEngineEventHandler, IDisposable

{

	private ITrackProvider _provider;

	private java.util.HashMap<String, Object> _initValues = new java.util.HashMap<String, Object>();



	@Override
	public void InitializeService()
	{
		IJobEngine engine = this.Site.<IJobEngine>GetService();
		engine.AddHandler(this);
		super.InitializeService();
	}

	public final Guid getId()
	{
		return _provider != null _provider.Id :this.Site.<IJobEngine>GetService().JobId;
	}

	public final String getName()
	{
		return _provider.getName();
	}

	public final String getCategory()
	{
		return _provider != null ? _provider.Category : "";
	}

	public final Object getItem(String name)
	{
		return _provider != null ? _provider[name] : _initValues.GetValueOrDefault(name, null);
	}
	public final void setItem(String name, Object value)
	{
		if (_provider != null)
		{
			_provider[name] = value;
		}
		else
		{
			_initValues.put(name, value);
		}
	}

	public final String[] getPropertyNames()
	{
		return _provider != null ? _provider.PropertyNames : _initValues.keySet().toArray();
	}

	public final TrackFactory getConnection()
	{
		return _provider != null ? _provider.Connection : null;
	}



//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IJobEngineEventHandler Members

	private void HandleStart(IJobEngine sender)
	{

	}

	private void HandleResume(IJobEngine sender)
	{

	}

	private void HandleExit(IJobEngine sender, JobExitEventArgs e)
	{



	}

	private void HandleLoad(IJobEngine sender)
	{
		IJob job = this.Site.<IJob>GetService();

		String name = job.GetProperty("name");
		if (String.IsNullOrWhiteSpace(name))
		{
			name = job.getTemplateName() + new java.util.Date().toString();
		}

		TrackFactory cnnt = new TrackFactory();
		_provider = cnnt.CreateProvider(job.getID(), name, "Jobs." + job.getTemplateName(), _initValues);
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IDisposable Members

	public final void dispose()
	{
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}