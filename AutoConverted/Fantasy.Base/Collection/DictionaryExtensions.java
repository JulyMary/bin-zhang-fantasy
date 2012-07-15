package Fantasy;

public final class DictionaryExtensions
{
//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue @default)
	public static <TKey, TValue> TValue GetValueOrDefault(java.util.Map<TKey, TValue> dict, TKey key, TValue _default)
	{
		TValue rs = null;
		if ((rs = dict.get(key)) != null)
		{
			return rs;
		}
		else
		{
			return _default;
		}
	}

//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
	public static <TKey, TValue> TValue GetValueOrDefault(java.util.Map<TKey, TValue> dict, TKey key)
	{
		TValue rs = null;
		if ((rs = dict.get(key)) != null)
		{
			return rs;
		}
		else
		{
			return null;
		}
	}

}