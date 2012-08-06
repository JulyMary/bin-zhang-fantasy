package fantasy.servicemodel;

public final class StatusBar {

	private StatusBar()
	{
		
	}
	
	public static void safeSetStatus(IStatusBarService bar, String status, Object... args)
	{
		if(bar != null)
		{
			bar.setStatus(String.format(status, args));
		}
	}
}
