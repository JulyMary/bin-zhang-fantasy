package fantasy.jobs.tasks;

import java.util.*;

import fantasy.servicemodel.*;
import fantasy.*;
import fantasy.jobs.*;
import fantasy.jobs.Consts;
import fantasy.jobs.management.IJobQueue;
import fantasy.jobs.management.JobMetaData;

import org.apache.commons.lang3.StringUtils;
import org.jdom2.*;

import fantasy.collections.*;

@Task(name = "startNewJob", namespaceUri = Consts.XNamespaceURI, description = "Start new jobs")
public class StartNewJobTask extends ObjectWithSite implements ITask
{

	public final void Execute() throws Exception
	{

		if(this.Template == null) return;
		Element xt = this.Template.clone();

		Enumerable<Element> includes = new Enumerable<Element>(xt.getChildren()).where(new Predicate<Element>(){

			@Override
			public boolean evaluate(Element element) throws Exception {
				return element.getName().equals("items");
			}}).from(new Selector<Element, Iterable<Element>>(){

				@Override
				public Iterable<Element> select(Element item) {
					return item.getChildren();
				}}).where(new Predicate<Element>(){

					@Override
					public boolean evaluate(Element ele) throws Exception {
						return IncludeAttribute(ele) != null;
					}});

		// Translate include to <item name="" />
	
		IItemParser itemParser = this.getSite().getRequiredService(IItemParser.class);
		for (Element include : includes.toArrayList())
		{
			Element items = include.getParentElement();
			include.detach();
			TaskItem[] taskItems = itemParser.ParseItem(IncludeAttribute(include).getValue());
			
			String localName = include.getName();
			Namespace ns = include.getNamespace();
			for (TaskItem item : taskItems)
			{
				Element itemElement = new Element(localName, ns);
				TaskItem tempVar = new TaskItem();
				tempVar.setName(item.getName());
				TaskItem newItem = tempVar;
				item.CopyMetaDataTo(newItem);
				newItem.Save(this.getSite(), itemElement);
				items.addContent(itemElement);
			}
		}


		IStringParser strParser = this.getSite().getRequiredService(IStringParser.class);
		xt = StringParserUtils.Parse(strParser, xt, null);

		IJob job = this.getSite().getRequiredService(IJob.class);
		SetProperty(xt, "application", job.GetProperty("application"));
		SetProperty(xt, "user", job.GetProperty("user"));
		SetProperty(xt, "parent", job.getID().toString());

		String si = JDomUtils.toString(xt);

		IJobQueue queue = this.getSite().getRequiredService(IJobQueue.class);
		JobMetaData child = queue.CreateJobMetaData();
		child.LoadXml(si);

		queue.Add(child);

		ILogger logger = this.getSite().getService(ILogger.class);
		if (logger != null)
		{
			logger.LogMessage("StartNewJob", "Start a new job %1$s (%2$s).", child.getName(), child.getId());
		}

		this.Id = child.getId();
		
	}

	private void SetProperty(Element template, final String name, String value) throws Exception
	{

			Element prop = new Enumerable<Element>(template.getChildren()).where(new Predicate<Element>(){

			@Override
			public boolean evaluate(Element ele) throws Exception {
				return  ele.getName().equals("properties");
			}}).from(new Selector<Element, Iterable<Element>>(){

				@Override
				public Iterable<Element> select(Element item) {
					return item.getChildren();
				}}).single(new Predicate<Element>(){

					@Override
					public boolean evaluate(Element ele) throws Exception {
						return StringUtils.equalsIgnoreCase(ele.getName(), name);
					}});
		if (prop == null)
		{

			
			Element xprops = new Enumerable<Element>(template.getChildren()).firstOrDefault(new Predicate<Element>(){
				@Override
				public boolean evaluate(Element ele) throws Exception {
					return  ele.getName().equals("properties");
				}});
			if (xprops == null)
			{
				xprops = new Element("properties", template.getNamespace());
				template.addContent(xprops);
			}

			prop = new Element(name, template.getNamespace());
			xprops.addContent(prop);
		}
		prop.setText(value);
	}

	private Attribute IncludeAttribute(Element element) throws Exception
	{
		return new Enumerable<Attribute>(element.getAttributes()).singleOrDefault(new Predicate<Attribute>(){

			@Override
			public boolean evaluate(Attribute attr) throws Exception {
				return StringUtils.equalsIgnoreCase(attr.getName(), "include");
			}});
	}


	@TaskMember(name = "jobStart", flags= {TaskMemberFlags.Input , TaskMemberFlags.Inline, TaskMemberFlags.Required} , parseInline=false, description="A 'jobStart' element which contains job start information")
	public Element Template;

	@TaskMember(name = "id", flags = TaskMemberFlags.Output, description="The id of created job.")
	public UUID Id;
	

}