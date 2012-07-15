package Fantasy;

public final class CommandArgumentsHelper
{
	private static final String REGEX_CMD = "(/|-)(?<name>\\w+)(:(?<value>.*))?";

	private static java.util.HashMap<String, java.util.ArrayList<String>> _arguments;

	public static java.util.HashMap<String, java.util.ArrayList<String>> getArguments()
	{
		return _arguments;
	}

	static
	{
		_arguments = new java.util.HashMap<String, java.util.ArrayList<String>>(StringComparer.OrdinalIgnoreCase);

		InitializeArgPairs();
	}

	public static boolean HasArgument(String name)
	{
		return _arguments.containsKey(name);
	}

	public static String GetValue(String name)
	{
		String rs = null;
		java.util.ArrayList<String> values = null;
		if ((values = _arguments.get(name)) != null)
		{
			if (values.size() > 0)
			{
				rs = values.get(0);
			}
		}
		return rs;

	}

	public static String[] GetValues(String name)
	{
		String[] rs = new String[0];
		java.util.ArrayList<String> values = null;
		if ((values = _arguments.get(name)) != null)
		{
			if (values.size() > 0)
			{
				rs = values.toArray(new String[]{});
			}
		}
		return rs;
	}



	public static boolean TryGetArgumentValue(String name, RefObject<String> value)
	{
		boolean rs = false;
		value.argvalue = null;
		java.util.ArrayList<String> values = null;
		if ((values = _arguments.get(name)) != null)
		{
			if (values.size() > 0)
			{
				rs = true;
				value.argvalue = values.get(0);
			}
		}
		return rs;
	}

	private static void InitializeArgPairs()
	{
		Regex regex = new System.Text.RegularExpressions.Regex(REGEX_CMD);

		for (String argument : Environment.GetCommandLineArgs())
		{
			Match match = regex.Match(argument);

			if (match.Success)
			{
				String name = match.Groups["name"].getValue();
				java.util.ArrayList<String> values = null;
				if (!((values = _arguments.get(name)) != null))
				{
					values = new java.util.ArrayList<String>();
					_arguments.put(name, values);
				}
				String value = match.Groups["value"].getValue().Trim('"', '\'');
				values.add(value);
			}
		}

	}

}