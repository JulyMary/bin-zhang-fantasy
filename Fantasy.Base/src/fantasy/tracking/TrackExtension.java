package fantasy.tracking;
@SuppressWarnings("unchecked")
public final class TrackExtension
{
//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static T GetProperty<T>(this ITrackProvider provider, string name, T defaultValue)
	
	public static <T> T GetProperty(ITrackProvider provider, String name, T defaultValue)
	{
		Object obj = provider.getItem(name);
		if (obj != null)
		{
			return (T)obj;

		}

		else
		{

			return defaultValue;

		}
	}


//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static T GetProperty<T>(this ITrackListener listener, string name, T defaultValue)
	public static <T> T GetProperty(ITrackListener listener, String name, T defaultValue)
	{
		Object obj = listener.getItem(name);
		if (obj != null)
		{
			return (T)obj;

		}

		else
		{

			return defaultValue;

		}
	}
}