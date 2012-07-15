package Fantasy.Jobs.Resources;

import Fantasy.ServiceModel.*;

public class ResourceGroup extends ObjectWithSite
{



	public ResourceGroup(Guid jobId, ResourceParameter[] parameters)
	{
		this.setJobId(jobId);
		this.setParameters((ResourceParameter[])parameters.clone());
		this.setId(Guid.NewGuid());
		this.setCreationTime(new java.util.Date());
	}

	private java.util.Date privateCreationTime = new java.util.Date(0);
	public final java.util.Date getCreationTime()
	{
		return privateCreationTime;
	}
	private void setCreationTime(java.util.Date value)
	{
		privateCreationTime = value;
	}

	private ResourceParameter[] privateParameters;
	public final ResourceParameter[] getParameters()
	{
		return privateParameters;
	}
	private void setParameters(ResourceParameter[] value)
	{
		privateParameters = value;
	}

	private Guid privateId = new Guid();
	public final Guid getId()
	{
		return privateId;
	}
	private void setId(Guid value)
	{
		privateId = value;
	}

	private Guid privateJobId = new Guid();
	public final Guid getJobId()
	{
		return privateJobId;
	}
	private void setJobId(Guid value)
	{
		privateJobId = value;
	}

	private java.util.HashMap<IResourceProvider, Object> _resources = new java.util.HashMap<IResourceProvider, Object>();

	public final boolean ContainsResource(Object resource)
	{
		return _resources.containsValue(resource);
	}

	public final void AddResource(IResourceProvider provider, Object resource)
	{
		this._resources.put(provider, resource);
	}

	public final void Release()
	{
		_released = true;
		ILogger logger = this.Site.<ILogger>GetService();
		for(java.util.Map.Entry<IResourceProvider, Object> pair : this._resources.entrySet())
		{
			IResourceProvider provider = pair.getKey();
			Object res = pair.getValue();
			try
			{
				provider.Release(res);

			}
			catch (ThreadAbortException e)
			{
			}
			catch(RuntimeException error)
			{
				if (logger != null)
				{
					logger.LogWarning("Resource", error, MessageImportance.Normal, "A error occurs when release resource {0}", provider.getClass());
				}
			}
		}

		this._resources.clear();


	}

	private boolean _released = false;

	public final boolean getReleased()
	{
		return _released;
	}

}