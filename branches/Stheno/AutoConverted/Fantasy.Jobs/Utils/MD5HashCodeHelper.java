package Fantasy.Jobs.Utils;

import Fantasy.IO.*;

public final class MD5HashCodeHelper
{
	public static String GetMD5HashCode(Stream stream)
	{
		String rs;
//C# TO JAVA CONVERTER NOTE: The following 'using' block is replaced by its Java equivalent:
//		using (MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider())
		MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
		try
		{

			byte[] bytes = provider.ComputeHash(stream);
			rs = BitConverter.ToString(bytes).replace("-", "");
		}
		finally
		{
			provider.dispose();
		}

		return rs;
	}

	public static String GetMD5HashCode(String filePath)
	{
		String rs;
		FileStream stream = LongPathFile.Open(filePath, FileMode.Open, FileAccess.Read);
		try
		{
			rs = GetMD5HashCode(stream);
		}
		finally
		{
			stream.Close();
		}
		return rs;
	}
}