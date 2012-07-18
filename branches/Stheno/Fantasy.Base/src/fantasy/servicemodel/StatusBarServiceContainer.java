package fantasy.servicemodel;



public class StatusBarServiceContainer implements IStatusBarServiceContainer
{

	private java.util.ArrayList<IStatusBarService> _services = new java.util.ArrayList<IStatusBarService>();
	
	@Override
	public final void setStatus(String status)
	{
		for (IStatusBarService service : this._services)
		{
			service.setStatus(status);
		}
	}

	public final void add(IStatusBarService service)
	{
		_services.add(service);
	}

	public final void remove(IStatusBarService service)
	{
		_services.remove(service);
	}

	
	
}