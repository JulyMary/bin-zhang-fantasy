package fantasy.jobs.resources;

import java.util.*;

import fantasy.*;
import fantasy.servicemodel.*;;

public class ResourceGroup extends ObjectWithSite
{



	public ResourceGroup(UUID jobId, ResourceParameter[] parameters)
	{
		this.setJobId(jobId);
		this.setParameters((ResourceParameter[])parameters.clone());
		this.setId(UUID.randomUUID());
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

	private UUID privateId;
	public final UUID getId()
	{
		return privateId;
	}
	private void setId(UUID value)
	{
		privateId = value;
	}

	private UUID privateJobId;
	public final UUID getJobId()
	{
		return privateJobId;
	}
	private void setJobId(UUID value)
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

	public final void Release() throws Exception
	{
		_released = true;
		ILogger logger = this.getSite().getService(ILogger.class);
		for(java.util.Map.Entry<IResourceProvider, Object> pair : this._resources.entrySet())
		{
			IResourceProvider provider = pair.getKey();
			Object res = pair.getValue();
			try
			{
				provider.Release(res);

			}
			catch (InterruptedException e)
			{
			}
			catch(Exception error)
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