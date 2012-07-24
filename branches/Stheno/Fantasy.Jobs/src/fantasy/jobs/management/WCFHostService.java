package fantasy.jobs.management;

import Fantasy.ServiceModel.*;

public class WCFHostService extends AbstractService
{
	private ServiceHost[] _serviceHosts;


	@Override
	public void InitializeService()
	{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		Thread thread = ThreadFactory.CreateThread(() =>
		{
			java.lang.Class[] types = (java.lang.Class[])AddIn.GetTypes("jobService/wcf/serviceHost");
			this._serviceHosts = new ServiceHost[types.length];
			for (int i = 0; i < types.length; i++)
			{
				this._serviceHosts[i] = new ServiceHost(types[i]);
				this._serviceHosts[i].Open();
			}
		}
	   ).WithStart();


		super.InitializeService();
	}

	@Override
	public void UninitializeService()
	{
		//foreach (ServiceHost host in this._serviceHosts)
		//{
		//    host.Close();
		//}
		super.UninitializeService();
	}
}