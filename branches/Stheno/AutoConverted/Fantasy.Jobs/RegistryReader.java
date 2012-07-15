package Fantasy.Jobs;

import Microsoft.Win32.*;

public class RegistryReader implements ITagValueProvider
{
	public final String GetTagValue(String key, java.util.Map<String, Object> context)
	{
		String rs = "";
		if (key == null)
		{
			throw new ArgumentNullException(key);
		}
		if (key.equals(""))
		{
			throw new IllegalArgumentException("Key can not be empty.", "key");
		}

		Regex regex = new Regex("(?<root>[^\\\\]*)\\\\(?<sub>[^@]*)(@(?<name>.*))+");

		Match m = regex.Match(key);

		RegistryKey root;
//C# TO JAVA CONVERTER NOTE: The following 'switch' operated on a string member and was converted to Java 'if-else' logic:
//		switch (m.Groups["root"].Value.ToUpper())
//ORIGINAL LINE: case "HKLM":
		if (m.Groups["root"].getValue().toUpperCase().equals("HKLM"))
		{
				root = Registry.LocalMachine;
		}
//ORIGINAL LINE: case "HKCU":
		else if (m.Groups["root"].getValue().toUpperCase().equals("HKCU"))
		{
				root = Registry.CurrentUser;
		}
//ORIGINAL LINE: case "HKCR":
		else if (m.Groups["root"].getValue().toUpperCase().equals("HKCR"))
		{
				root = Registry.ClassesRoot;
		}
//ORIGINAL LINE: case "HKCC":
		else if (m.Groups["root"].getValue().toUpperCase().equals("HKCC"))
		{
				root = Registry.CurrentConfig;
		}
//ORIGINAL LINE: case "HKPD":
		else if (m.Groups["root"].getValue().toUpperCase().equals("HKPD"))
		{
				root = Registry.PerformanceData;
		}
//ORIGINAL LINE: case "HKU":
		else if (m.Groups["root"].getValue().toUpperCase().equals("HKU"))
		{
				root = Registry.Users;
		}
		else
		{
				throw new IllegalArgumentException("Invalid key name.", "key");

		}

		root.OpenSubKey(m.Groups["sub"].getValue());
		if (root != null)
		{
			String name = m.Groups["name"].Success ? m.Groups["name"].getValue() : null;

			rs = root.GetValue(name, "").toString();
			root.Close();
		}

		return rs;

	}

	public final boolean HasTag(String tag, java.util.Map<String, Object> context)
	{
		if (tag == null)
		{
			throw new ArgumentNullException(tag);
		}

		tag = tag.toUpperCase();
		for (String root : RegRoots)
		{
			if (tag.startsWith(root))
			{
				return true;
			}
		}

		return false;

	}

	private static final String[] RegRoots = new String[] { "HKLM\\", "HKCU\\", "HKCR\\", "HKCC\\" };

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITagValueProvider Members

	public final char getPrefix()
	{
		return '$';
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITagValueProvider Members


	public final boolean IsEnabled(java.util.Map<String, Object> context)
	{
		return true;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}