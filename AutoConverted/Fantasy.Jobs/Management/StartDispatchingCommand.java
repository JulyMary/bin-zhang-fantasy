package Fantasy.Jobs.Management;

public class StartDispatchingCommand implements ICommand, IObjectWithSite
{

	private IServiceProvider privateSite;
	public final IServiceProvider getSite()
	{
		return privateSite;
	}
	public final void setSite(IServiceProvider value)
	{
		privateSite = value;
	}


	public final Object Execute(Object args)
	{
		IJobDispatcher disp = (IJobDispatcher)this.Site.GetService(IJobDispatcher.class);
		if (disp != null)
		{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			Task.Factory.StartNew(() =>
			{
				disp.Start();
			}
		   );
		}


		return null;

	}


}