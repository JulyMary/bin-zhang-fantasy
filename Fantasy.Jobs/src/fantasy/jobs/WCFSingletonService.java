package fantasy.jobs;

import fantasy.servicemodel.*;

public abstract class WCFSingletonService extends AbstractService
{
	private ServiceHost _serviceHost;

	@Override
	public void InitializeService()
	{

//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		Thread thread = ThreadFactory.CreateThread(() =>
		{
			_serviceHost = new ServiceHost(this);
			_serviceHost.Open();
		}
	   ).WithStart();

		thread.join();
		super.InitializeService();
	}

	@Override
	public void UninitializeService()
	{

		super.UninitializeService();
		this._serviceHost.Close();
	}
}