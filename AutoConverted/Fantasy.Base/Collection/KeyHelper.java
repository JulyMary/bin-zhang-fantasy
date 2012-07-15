package Fantasy.Collections;

public final class KeyHelper
{
	public static int hashCode(Object[] values)
	{
		int rs = 0;
		for (Object o : values)
		{
			int h = o != null ? o.hashCode() : 0;
			rs ^= h;
		}

		return rs;
	}

	public static boolean equals(Object[] values1, Object[] values2)
	{
		for (int i = 0; i < values1.length; i++)
		{
			if (!values1[i].equals(values2[i]))
			{
				return false;
			}
		}

		return true;
	}
}