package Fantasy.Jobs;

import Fantasy.XSerialization.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[XSerializable("import", NamespaceUri = Consts.XNamespaceURI)]
public class ImportAssembly
{
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("name")]
	private String privateName;
	public final String getName()
	{
		return privateName;
	}
	public final void setName(String value)
	{
		privateName = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("path")]
	private String privatePath;
	public final String getPath()
	{
		return privatePath;
	}
	public final void setPath(String value)
	{
		privatePath = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("src")]
	private String privateSrc;
	public final String getSrc()
	{
		return privateSrc;
	}
	public final void setSrc(String value)
	{
		privateSrc = value;
	}

	public final Assembly LoadAssembly(IStringParser parser)
	{
		Assembly rs;
		if (!DotNetToJavaStringHelper.isNullOrEmpty(this.getName()))
		{
			if (parser != null)
			{
				this.setName(parser.Parse(this.getName()));
			}
			rs = Assembly.Load(this.getName());
		}
		else if (!DotNetToJavaStringHelper.isNullOrEmpty(this.getPath()))
		{
			if (parser != null)
			{
				this.setPath(parser.Parse(this.getPath()));
			}

			String fullPath = this.getPath();

			if (!System.IO.Path.IsPathRooted(fullPath))
			{
				fullPath = Fantasy.IO.LongPath.Combine(getSrc(), fullPath);
			}

			rs = Assembly.LoadFile(fullPath);
		}
		else
		{
			throw new InvalidOperationException("\"import\" element must has name or path attribute.");
		}

		return rs;

	}


}