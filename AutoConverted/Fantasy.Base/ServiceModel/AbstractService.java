package Fantasy.ServiceModel;

public abstract class AbstractService extends MarshalByRefObject implements IService, IObjectWithSite
{

	@Override
	public Object InitializeLifetimeService()
	{
		return null;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IService Members

	public void InitializeService()
	{
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
	}

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler Initialize;

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler Uninitialize;

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IObjectWithSite Members

	private IServiceProvider privateSite;
	public final IServiceProvider getSite()
	{
		return privateSite;
	}
	public final void setSite(IServiceProvider value)
	{
		privateSite = value;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}