package System.Xml.Linq;

import Fantasy.IO.*;

public final class LongPathXNode
{
	public static XElement LoadXElement(String url)
	{
		XElement rs;
		FileStream fs = LongPathFile.Open(url, FileMode.Open, FileAccess.Read, FileShare.Read);
		try
		{

			rs = XElement.Load(XmlReader.Create(fs));
		}
		finally
		{
			fs.Close();
		}
		return rs;
	}

	public static XDocument LoadXDocument(String url)
	{
		XDocument rs;
		FileStream fs = LongPathFile.Open(url, FileMode.Open, FileAccess.Read, FileShare.Read);
		try
		{
			rs = XDocument.Load(XmlReader.Create(fs));
		}
		finally
		{
			fs.Close();
		}

		return rs;
	}


}