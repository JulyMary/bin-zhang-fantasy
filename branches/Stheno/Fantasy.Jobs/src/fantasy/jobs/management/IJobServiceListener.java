package fantasy.jobs.management;

import java.rmi.*;

public interface IJobServiceListener extends Remote
{

	void Added(JobMetaData job) throws RemoteException;

	void Changed(JobMetaData job) throws RemoteException;

	void Echo() throws RemoteException;
}