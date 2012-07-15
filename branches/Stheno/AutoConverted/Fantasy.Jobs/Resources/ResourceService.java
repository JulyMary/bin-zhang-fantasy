package Fantasy.Jobs.Resources;

import Fantasy.ServiceModel.*;

public class ResourceService extends AbstractService implements IResourceService, IResourceManagerHandler
{

	@Override
	public Object InitializeLifetimeService()
	{
		return null;
	}

	private java.util.HashMap<Guid, ResourceHandle> _resources = new java.util.HashMap<Guid, ResourceHandle>();

	public final IResourceHandle Request(ResourceParameter[] parameters)
	{

		ResourceHandle rs = InnerRequest(parameters);
		if (rs != null)
		{
			rs.setSuspendEngine(true);
			return rs;
		}
		else
		{
			IJobEngine engine = this.Site.<IJobEngine>GetRequiredService();
			IResourceRequestQueue queue = this.Site.<IResourceRequestQueue>GetRequiredService();
			queue.RegisterResourceRequest(engine.getJobId(), parameters);
			ILogger logger = this.Site.<ILogger>GetService();
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			logger.LogMessage("Resource", "Suspend job engine because it failed to request resource:" + Environment.NewLine + DotNetToJavaStringHelper.join(Environment.NewLine, parameters.Select(p=>p.toString())));
			Suspend();
			return null;
		}
	}

	private void Suspend()
	{
		IJobEngine engine = this.Site.<IJobEngine>GetRequiredService();
		engine.Suspend();
		//ManualResetEvent _waitHandler = new ManualResetEvent(false);
		//Thread thread = ThreadFactory.CreateThread(() => {
		//    ILogger logger = this.Site.GetService<ILogger>();
		//    logger.SafeLogMessage("Resource", "Suspending thread started.");

		//    engine.Suspend();

		//    _waitHandler.Set();
		//});
		//thread.Start();


		//_waitHandler.WaitOne();
		//Thread.CurrentThread.Abort();

	}

	private ResourceHandle InnerRequest(ResourceParameter[] parameters)
	{
		IJobEngine engine = this.Site.<IJobEngine>GetRequiredService();
		IResourceManager manager = this.Site.<IResourceManager>GetRequiredService();
		Guid resId = manager.Request(engine.getJobId(), parameters);
		if (!resId.equals(Guid.Empty))
		{
			ResourceHandle tempVar = new ResourceHandle();
			tempVar.setId(resId);
			tempVar.setResourceService(this);
			tempVar.setParameters(parameters);
			ResourceHandle rs = tempVar;
			synchronized (this._resources)
			{
				this._resources.put(resId, rs);
			}
			return rs;
		}
		else
		{
			return null;
		}
	}

	public final IResourceHandle TryRequest(ResourceParameter[] parameters)
	{
		return InnerRequest(parameters);
	}

	public final void Release(IResourceHandle resource)
	{
		if (resource == null)
		{
			throw new ArgumentNullException("resource");
		}
		IResourceManager manager = this.Site.<IResourceManager>GetRequiredService();
		manager.Release(((ResourceHandle)resource).getId());
	}


//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IResourceManagerHandler Members

	@Override
	public void InitializeService()
	{
		IResourceManager manager = this.Site.<IResourceManager>GetRequiredService();
		manager.RegisterHandler(this);
		super.InitializeService();
	}

	@Override
	public void UninitializeService()
	{
		super.UninitializeService();
		IResourceManager manager = this.Site.<IResourceManager>GetRequiredService();
		IJobEngine engine = this.Site.<IJobEngine>GetRequiredService();
		manager.UnregisterHandler(engine.getJobId());
	}

	private void Revoke(Guid id)
	{
		ResourceHandle resource;
		synchronized (_resources)
		{
			resource = this._resources.GetValueOrDefault(id);
		}
		if (resource != null)
		{
			try
			{
				RevokeArgs args = new RevokeArgs();
				resource.OnRevoke(args);

				if (resource.getSuspendEngine() && args.getSuspendEngine())
				{
					ILogger logger = this.Site.<ILogger>GetService();
					if (logger != null)
					{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
						logger.LogMessage("Resource", "Suspend job engine because a resource is revoked." + Environment.NewLine + DotNetToJavaStringHelper.join(Environment.NewLine, resource.getParameters().Select(p=>p.toString())));
					}

					IJobEngine engine = this.Site.<IJobEngine>GetRequiredService();
					IResourceRequestQueue queue = this.Site.<IResourceRequestQueue>GetRequiredService();
					queue.RegisterResourceRequest(engine.getJobId(), resource.getParameters());
					Suspend();
				}
			}
			finally
			{
				resource.dispose();
			}
		}
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IResourceManagerHandler Members


	private Guid Id()
	{

		IJobEngine engine = this.Site.<IJobEngine>GetRequiredService();
		return engine.getJobId();

	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}