package fantasy.jobs.solar;

import Fantasy.Jobs.Resources.*;
import Fantasy.ServiceModel.*;

public class SatelliteResourceRequestQueue extends AbstractService implements IResourceRequestQueue
{
	private ISolarActionQueue _actionQueue;

	@Override
	public void InitializeService()
	{
		this._actionQueue = this.Site.<ISolarActionQueue>GetRequiredService();
		super.InitializeService();
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IResourceRequestQueue Members

	public final void RegisterResourceRequest(UUID jobId, ResourceParameter[] parameters)
	{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		this._actionQueue.Enqueue(solar => solar.RegisterResourceRequest(jobId, parameters));
	}

	public final void UnregisterResourceRequest(UUID jobId)
	{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		this._actionQueue.Enqueue(solar => solar.UnregisterResourceRequest(jobId));
	}

	public final ResourceParameter[] GetRequiredResources(UUID jobId)
	{
//C# TO JAVA CONVERTER NOTE: The following 'using' block is replaced by its Java equivalent:
//		using (ClientRef<ISolar> client = ClientFactory.Create<ISolar>())
		ClientRef<ISolar> client = ClientFactory.<ISolar>Create();
		try
		{
			return client.Client.GetRequiredResources(jobId);
		}
		finally
		{
		}
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}