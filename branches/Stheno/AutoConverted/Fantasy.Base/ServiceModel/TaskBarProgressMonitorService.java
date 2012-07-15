package Fantasy.ServiceModel;

public class TaskBarProgressMonitorService extends TaskBarProgressMonitor implements IService, IObjectWithSite
{

	private IProgressMonitorContainer _container;
	public void InitializeService()
	{
		if(this.getSite() != null)
		{
			_container = this.getSite().<IProgressMonitorContainer>GetService();
			if (_container != null)
			{
				_container.AddMoniter(this);
			}
		}



		if (this.Initialize != null)
		{
			this.Initialize(this, EventArgs.Empty);
		}
	}

	public void UninitializeService()
	{
		if (this.Uninitialize != null)
		{
			this.Uninitialize(this, EventArgs.Empty);
		}
		if (_container != null)
		{
			this._container.RemoveMoniter(this);
		}
	}

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler Initialize;

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler Uninitialize;


	private IServiceProvider privateSite;
	public final IServiceProvider getSite()
	{
		return privateSite;
	}
	public final void setSite(IServiceProvider value)
	{
		privateSite = value;
	}

}