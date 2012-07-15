package Fantasy.ServiceModel;

import Microsoft.VisualBasic.*;
import Fantasy.Properties.*;

public enum ProgressMonitorStyle
{
	Blocks(0),
	Continuous(1),
	Marquee(2);

	private int intValue;
	private static java.util.HashMap<Integer, ProgressMonitorStyle> mappings;
	private synchronized static java.util.HashMap<Integer, ProgressMonitorStyle> getMappings()
	{
		if (mappings == null)
		{
			mappings = new java.util.HashMap<Integer, ProgressMonitorStyle>();
		}
		return mappings;
	}

	private ProgressMonitorStyle(int value)
	{
		intValue = value;
		ProgressMonitorStyle.getMappings().put(value, this);
	}

	public int getValue()
	{
		return intValue;
	}

	public static ProgressMonitorStyle forValue(int value)
	{
		return getMappings().get(value);
	}
}