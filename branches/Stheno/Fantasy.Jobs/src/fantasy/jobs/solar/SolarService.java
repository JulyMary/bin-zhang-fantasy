package fantasy.jobs.solar;

import Fantasy.Jobs.Management.*;
import Fantasy.Jobs.Resources.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[ServiceBehavior(InstanceContextMode=InstanceContextMode.Single, ConcurrencyMode=ConcurrencyMode.Multiple, Namespace=Consts.JobServiceNamespaceURI)]
public class SolarService extends WCFSingletonService implements ISolar
{
	private IJobQueue _queue;

	@Override
	public void InitializeService()
	{
		this._queue = this.Site.<IJobQueue>GetRequiredService();

		super.InitializeService();
	}

	@Override
	public void UninitializeService()
	{

		super.UninitializeService();

	}



	public final JobMetaData[] Unterminates(int skip, int take)
	{
		return this._queue.getUnterminates().Skip(skip).Take(take).toArray();
	}

	public final JobMetaData[] Terminates(int skip, int take)
	{
		return this._queue.getTerminates().Skip(skip).Take(take).toArray();
	}

	public final JobMetaData FindJobMetaDataById(UUID id)
	{
		return this._queue.FindJobMetaDataById(id);
	}

	public final JobMetaData[] FindTerminated(RefObject<Integer> totalCount, String filter, String[] args, String order, int skip, int take)
	{
		return this._queue.FindTerminated(totalCount, filter, args, order, skip, take).toArray();
	}

	public final JobMetaData[] FindUnterminated(RefObject<Integer> totalCount, String filter, String[] args, String order, int skip, int take)
	{
		return this._queue.FindUnterminated(totalCount, filter, args, order, skip, take).toArray();
	}

	public final void ApplyChange(JobMetaData job)
	{
		this._queue.ApplyChange(job);
	}

	public final void Resume(UUID id)
	{
		this._queue.Resume(id);
	}

	public final void Cancel(UUID id)
	{
		this._queue.Cancel(id);
	}

	public final void Suspend(UUID id)
	{
		this._queue.Suspend(id);
	}

	public final void UserPause(UUID id)
	{
		this._queue.UserPause(id);
	}

	public final String[] GetAllApplications()
	{
		return this._queue.GetAllApplications();
	}

	public final String[] GetAllUsers()
	{
		return this._queue.GetAllUsers();
	}


	public final void RegisterResourceRequest(UUID jobId, Resources.ResourceParameter[] parameters)
	{
		IResourceRequestQueue resQueue = this.Site.<IResourceRequestQueue>GetService();
		if (resQueue != null)
		{
			resQueue.RegisterResourceRequest(jobId, parameters);
		}
	}




	public final void UnregisterResourceRequest(UUID jobId)
	{
		IResourceRequestQueue resQueue = this.Site.<IResourceRequestQueue>GetService();
		if (resQueue != null)
		{
			resQueue.UnregisterResourceRequest(jobId);
		}
	}

	public final ResourceParameter[] GetRequiredResources(UUID jobId)
	{
		IResourceRequestQueue resQueue = this.Site.<IResourceRequestQueue>GetService();
		if (resQueue != null)
		{
			return resQueue.GetRequiredResources(jobId);
		}
		else
		{
			return new ResourceParameter[] { };
		}
	}



	public final void ResourceAvaiable()
	{
		IJobDispatcher dispatcher = this.Site.<IJobDispatcher>GetRequiredService();
		dispatcher.TryDispatch();
	}


	public final boolean IsTermianted(UUID id)
	{
		return _queue.IsTerminated(id);
	}


}