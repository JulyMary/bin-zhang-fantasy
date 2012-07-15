package Fantasy;

import Fantasy.ServiceModel.*;

public final class ServiceProviderExtension
{

//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static T GetService<T>(this IServiceProvider services)
	public static <T> T GetService(IServiceProvider services)
	{
		return (T)services.GetService(T.class);
	}

//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static object GetRequiredService(this IServiceProvider services, Type serviceType)
	public static Object GetRequiredService(IServiceProvider services, java.lang.Class serviceType)
	{
		Object rs = services.GetService(serviceType);
		if (rs == null)
		{
			throw new MissingRequiredServiceException(serviceType);
		}
		return rs;
	}

//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static T GetRequiredService<T>(this IServiceProvider services)
	public static <T> T GetRequiredService(IServiceProvider services)
	{
		return (T)services.GetRequiredService(T.class);
	}


}