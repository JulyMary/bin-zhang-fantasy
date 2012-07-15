package Fantasy.ServiceModel;

import Microsoft.VisualBasic.*;

public class StatusBarServiceContainer implements IStatusBarServiceContainer
{

	private java.util.ArrayList _services = new java.util.ArrayList();
	public final void SetStatus(String status)
	{
		for (IStatusBarService service : this._services)
		{
			service.SetStatus(status);
		}
	}

	public final void Add(IStatusBarService service)
	{
		_services.add(service);
	}

	public final void Remove(IStatusBarService service)
	{
		_services.remove(service);
	}
}