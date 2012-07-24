package fantasy.jobs.management;

import java.rmi.RemoteException;
import java.rmi.server.UnicastRemoteObject;

public class JobManagerAccessor extends UnicastRemoteObject
{

	private static final long serialVersionUID = 5149188311986178760L;

	public JobManagerAccessor() throws RemoteException {
		super();
	
	}

	private static IJobManager _manager;

	public final IJobManager GetJobManager()
	{
		return _manager;
		//return (_manager != null) ? _manager : JobManager.getDefault();
	}

	public final void SetJobManager(IJobManager manager)
	{
		_manager = manager;
	}
}