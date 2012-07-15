package System.Reflection;

public final class MemberInfoExtension
{
//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static T[] GetCustomAttributes<T>(this MemberInfo mi, bool inherit)
	public static <T> T[] GetCustomAttributes(MemberInfo mi, boolean inherit)
	{
		return mi.GetCustomAttributes(T.class, inherit).<T>Cast().toArray();
	}
}