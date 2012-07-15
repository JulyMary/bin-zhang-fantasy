package Fantasy.Jobs.Management;

public final class IJobServiceExtension
{

	private static String NormalizeTypeName(java.lang.Class t)
	{
		String rs = String.format("%1$s, %2$s", t.FullName, t.Assembly.GetName().getName());
		return rs;
	}

//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static string GetSettings<T>(this IJobService service)
	public static <T> String GetSettings(IJobService service)
	{
		return service.GetSettings(NormalizeTypeName(T.class));
	}

//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static void SetSettings<T>(this IJobService service, string xml)
	public static <T> void SetSettings(IJobService service, String xml)
	{
		service.SetSettings(NormalizeTypeName(T.class), xml);
	}
}