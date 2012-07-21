package fantasy.jobs;

import java.io.*;

import fantasy.xserialization.*;
import fantasy.*;

@XSerializable(name = "import", namespaceUri = Consts.XNamespaceURI)
public class ImportAssembly
{

	@XAttribute(name = "name")
	private String privateName;
	public final String getName()
	{
		return privateName;
	}
	public final void setName(String value)
	{
		privateName = value;
	}

	

	public final File LoadAssembly(IStringParser parser)
	{
		
		String root = JarUtils.getJar(ImportAssembly.class).getParentFile().getAbsoluteFile().getName();
		
		File rs;
		if (!StringUtils2.isNullOrEmpty(this.getName()))
		{
			
			String name = this.getName();
			if (parser != null)
			{
				name = parser.Parse(name);
			}
			rs = new File( fantasy.io.Path.combine(root, this.getName()));
		}
		else
		{
			throw new IllegalStateException("\"import\" element must has name attribute.");
		}

		return rs;

	}


}