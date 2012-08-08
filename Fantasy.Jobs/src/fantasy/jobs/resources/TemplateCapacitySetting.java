package fantasy.jobs.resources;

import fantasy.jobs.*;
import fantasy.xserialization.*;

@XSerializable(name="template", namespaceUri=Consts.XNamespaceURI)
public class TemplateCapacitySetting
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

	@XAttribute(name = "capacity")
	private int privateCapacity;
	public final int getCapacity()
	{
		return privateCapacity;
	}
	public final void setCapacity(int value)
	{
		privateCapacity = value;
	}

	
}