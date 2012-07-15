package Fantasy.Jobs.Resources;

public class ResourceHandle implements IResourceHandle
{
	private IResourceService privateResourceService;
	public final IResourceService getResourceService()
	{
		return privateResourceService;
	}
	public final void setResourceService(IResourceService value)
	{
		privateResourceService = value;
	}

	private boolean privateSuspendEngine;
	public final boolean getSuspendEngine()
	{
		return privateSuspendEngine;
	}
	public final void setSuspendEngine(boolean value)
	{
		privateSuspendEngine = value;
	}

	private ResourceParameter[] privateParameters;
	public final ResourceParameter[] getParameters()
	{
		return privateParameters;
	}
	public final void setParameters(ResourceParameter[] value)
	{
		privateParameters = value;
	}

	private Guid privateId = new Guid();
	public final Guid getId()
	{
		return privateId;
	}
	public final void setId(Guid value)
	{
		privateId = value;
	}

	private boolean _disposed = false;

	public final void dispose()
	{
		if(!_disposed)
		{
			_disposed = true;
			this.getResourceService().Release(this);
		}
	}

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler<RevokeArgs> Revoke;

	public void OnRevoke(RevokeArgs e)
	{
		if (this.Revoke != null)
		{
			this.Revoke(this, e);
		}
	}

}