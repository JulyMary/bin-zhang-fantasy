package fantasy;

import java.lang.reflect.Constructor;

@SuppressWarnings({"unchecked", "rawtypes"})
public final class Activator {

	private Activator()
	{
		
	}
	
	
	public static Object createInstance(Class type) throws Exception
	{
		
		Constructor ctor = type.getDeclaredConstructor();
		ctor.setAccessible(true);
		return ctor.newInstance();
	}
}
