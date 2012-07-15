package Fantasy.Jobs.Tasks;

import Fantasy.Jobs.Management.*;
import Fantasy.ServiceModel.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Task("startNewJob", Consts.XNamespaceURI, Description = "Start new jobs")]
public class StartNewJobTask extends ObjectWithSite implements ITask
{
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITask Members

	public final boolean Execute()
	{

		XElement xt = new XElement(this.getTemplate());
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
		var includes = from items in xt.Elements() where items.getName().LocalName.equals("items") from item in items.Elements() where IncludeAttribute(item) != null select item;

		// Traslate include to <item name="" />
		ExecuteTaskInstruction inst = this.Site.<ExecuteTaskInstruction>GetRequiredService();
		IItemParser itemParser = this.Site.<IItemParser>GetRequiredService();
		for (XElement include : includes.toArray())
		{
			XElement items = include.Parent;
			include.Remove();
			TaskItem[] taskItems = itemParser.ParseItem((String)IncludeAttribute(include));
			String prefix = include.GetPrefixOfNamespace(include.getName().NamespaceName);
			String localName = include.getName().LocalName;
			XNamespace ns = include.getName().NamespaceName;
			for (TaskItem item : taskItems)
			{
				XElement itemElement = new XElement(ns + localName);
				TaskItem tempVar = new TaskItem();
				tempVar.setName(item.getItem("fullname"));
				TaskItem newItem = tempVar;
				item.CopyMetaDataTo(newItem);
				newItem.Save(this.Site, itemElement);
				items.Add(itemElement);
			}
		}


		IStringParser strParser = this.Site.<IStringParser>GetRequiredService();
		xt = strParser.Parse(xt);

		IJob job = this.Site.<IJob>GetRequiredService();
		SetProperty(xt, "application", job.GetProperty("application"));
		SetProperty(xt, "user", job.GetProperty("user"));
		SetProperty(xt, "parent", job.getID().toString());

		String si = xt.toString();

		IJobQueue queue = this.Site.<IJobQueue>GetRequiredService();
		JobMetaData child = queue.CreateJobMetaData();
		child.LoadXml(si);

		queue.ApplyChange(child);

		ILogger logger = this.Site.<ILogger>GetService();
		if (logger != null)
		{
			logger.LogMessage("StartNewJob", "Start a new job {0} ({1}).", child.getName(), child.getId());
		}

		this.setId(child.getId());
		return true;
	}

	private void SetProperty(XElement template, String name, String value)
	{
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
		var query = from properties in template.Elements() where properties.getName().LocalName.equals("properties") from property in properties.Elements() where property.getName().LocalName.compareToIgnoreCase(name) == 0 select property;
		XElement prop = query.SingleOrDefault();
		if (prop == null)
		{
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
			var query2 = from properties in template.Elements() where properties.getName().LocalName.equals("properties") select properties;
			XElement xprops = query2.SingleOrDefault();
			if (xprops == null)
			{
				xprops = new XElement(template.getName().Namespace + "properties");
				template.Add(xprops);
			}

			prop = new XElement(template.getName().Namespace + name);
			xprops.Add(prop);
		}
		prop.setValue(value);
	}

	private XAttribute IncludeAttribute(XElement element)
	{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		return element.Attributes().SingleOrDefault(a => a.getName().LocalName.compareToIgnoreCase("include") == 0);
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion


//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("jobStart", Flags= TaskMemberFlags.Input | TaskMemberFlags.Inline | TaskMemberFlags.Required, ParseInline=false, Description="A 'jobStart' element which contains job start information")]
	private XElement privateTemplate;
	public final XElement getTemplate()
	{
		return privateTemplate;
	}
	public final void setTemplate(XElement value)
	{
		privateTemplate = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("id", Flags=TaskMemberFlags.Output, Description="The id of created job.")]
	private Guid privateId = new Guid();
	public final Guid getId()
	{
		return privateId;
	}
	public final void setId(Guid value)
	{
		privateId = value;
	}

}