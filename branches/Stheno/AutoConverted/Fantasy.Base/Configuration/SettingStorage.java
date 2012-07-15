package Fantasy.Configuration;

import Fantasy.IO.*;

public class SettingStorage
{

	private static XElement Root;



	private static String _location = null;
	public static String getLocation()
	{
		if (_location == null)
		{

			_location = ConfigurationManager.AppSettings["fantasySettingsFile"];
			String cfg = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
			if (DotNetToJavaStringHelper.isNullOrEmpty(_location))
			{
				_location = Path.ChangeExtension(cfg, "xsettings");

			}
			_location = Environment.ExpandEnvironmentVariables(_location);

			String defaultDir = LongPath.GetDirectoryName(cfg);

			_location = LongPath.Combine(defaultDir, _location);


		}

		return _location;
	}



	public static SettingsBase Load(SettingsBase data)
	{
		if (Root == null)
		{
			if (File.Exists(getLocation()))
			{
				Root = LongPathXNode.LoadXElement(getLocation());
			}
			else
			{
				Root = new XElement("settingsFile", new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"), new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"), new XAttribute("version", 0));
			}
		}

		XElement settings = Root.XPathSelectElement(String.format("settings[@type=\"%1$s\"]", data.getClass().FullName));
		if (settings != null)
		{
			data.Load(settings.toString());
		}
		return data;
	}
	XmlWriterSettings tempVar = new XmlWriterSettings();
	tempVar.Encoding = Encoding.UTF8;
	tempVar.Indent = true;
	tempVar.IndentChars = "    ";
	tempVar.OmitXmlDeclaration = false;
	tempVar.NewLineChars="\r\n";
	tempVar.NewLineHandling= NewLineHandling.Replace;
	tempVar.NewLineOnAttributes= true;
	private static XmlWriterSettings _xmlWriterSettings = tempVar;
	public static void Save(SettingsBase data)
	{
		XElement newSettings = XElement.Parse(data.ToXml(), LoadOptions.None);
		XElement settings = Root.XPathSelectElement(String.format("settings[@type=\"%1$s\"]", data.getClass().FullName));
		if (settings != null)
		{
			settings.ReplaceWith(newSettings);
		}
		else
		{
			Root.Add(newSettings);
		}

		XmlWriter writer = XmlWriter.Create(getLocation(), _xmlWriterSettings);
		try
		{
			StringWriter sw = new StringWriter();

			Root.Save(sw, SaveOptions.OmitDuplicateNamespaces);

			String xml = sw.GetStringBuilder().toString();

			XElement savingData = XElement.Parse(xml, LoadOptions.None);

			savingData.Save(writer);
		}
		finally
		{
			writer.Close();
		}

	}
}