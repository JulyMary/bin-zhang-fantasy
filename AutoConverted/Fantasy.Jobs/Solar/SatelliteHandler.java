package Fantasy.Jobs.Solar;

import Fantasy.Jobs.Management.*;
import Fantasy.ServiceModel.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[ServiceBehavior(ConcurrencyMode=ConcurrencyMode.Reentrant, InstanceContextMode=InstanceContextMode.PerSession, Namespace=Consts.JobServiceNamespaceURI)]
public class SatelliteHandler implements ISatelliteHandler
{
	public SatelliteHandler()
	{

	}

	private ISatellite _satellite;
	private String _name;
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ISatelliteHandler Members

	public final void Connect(String name)
	{
		_name = name;
		_satellite = OperationContext.Current.<ISatellite>GetCallbackChannel();
		SatelliteManager manager = JobManager.getDefault().<SatelliteManager>GetRequiredService();
		manager.RegisterSatellite(name, _satellite);
	}



	public final void Echo()
	{
		SatelliteManager manager = JobManager.getDefault().<SatelliteManager>GetRequiredService();
		if (!manager.IsValid(_satellite))
		{
			throw new FaultException<CallbackExpiredException>(new CallbackExpiredException(), new FaultReason("Satellite handle is invalid."));
		}
		else
		{
			manager.UpdateEchoTime(_name);
		}
	}

	public final void Disconnect()
	{
		SatelliteManager manager = JobManager.getDefault().<SatelliteManager>GetRequiredService();
		manager.UnregisterSatellite(_satellite);
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion


}