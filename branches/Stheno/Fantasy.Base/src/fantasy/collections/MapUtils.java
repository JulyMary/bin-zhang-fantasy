package fantasy.collections;
import java.util.*;

public final class MapUtils {

	private MapUtils()
	{
		
	}
	
	@SuppressWarnings("unchecked")
	public static <K,V,D> D  getValueOrDefault(Map<K,V> map, K key,  D defaultValue)
	{
		if(map.containsKey(key))
		{
			return (D)map.get(key);
		}
		else
		{
			return defaultValue;
		}
	}
}
