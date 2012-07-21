package fantasy.jobs;

import java.util.*;

import org.jdom2.*;

import fantasy.xserialization.*;
import fantasy.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
@Instruction
@XSerializable(name = "items", namespaceUri = Consts.XNamespaceURI)
public class CreateItems extends AbstractInstruction implements IConditionalObject, IXSerializable
{

	private java.util.ArrayList<CreateItemsItem> _list = new java.util.ArrayList<CreateItemsItem>();

	@Override
	public void Execute() throws Exception
	{
		IStringParser parser = this.getSite().getRequiredService2(IStringParser.class);
		IItemParser itemParser = this.getSite().getRequiredService2(IItemParser.class);
		IJob job = this.getSite().getRequiredService2(IJob.class);
		IConditionService conditionSvc = this.getSite().getRequiredService2(IConditionService.class);
		if (conditionSvc.Evaluate(this))
		{
			int index = job.getRuntimeStatus().getLocal().GetValue("createitems.index", 0);
			TaskItemGroup group = null;
			while (index < _list.size())
			{
				CreateItemsItem item = _list.get(index);
				TaskItem[] parsedItems = itemParser.ParseItem(item.getName());
				if (parsedItems.length > 0)
				{
					TreeMap<String, String> meta = new TreeMap<String, String>(String.CASE_INSENSITIVE_ORDER);
					
					for (String name : item.getMetaData().keySet())
					{
						meta.put(name, parser.Parse(item.getMetaData().get(name)));
					}
					if (group == null)
					{
						group = job.AddTaskItemGroup();
					}
					for (TaskItem parsedItem : parsedItems)
					{
						TaskItem newItem = group.AddNewItem(parsedItem.getName(), item.getCategory());
						parsedItem.CopyMetaDataTo(newItem);
						for (String name : meta.keySet())
						{
							newItem.setItem(name, meta.get(name));
						}
					}

				}
				index++;
				job.getRuntimeStatus().getLocal().setItem("setproperties.index", index);

			}
		}
	}

	private String privateCondition;
	public final String getCondition()
	{
		return privateCondition;
	}
	public final void setCondition(String value)
	{
		privateCondition = value;
	}


	public final void Load(IServiceProvider context, Element element) throws Exception
	{
		this.setCondition((String)element.getAttributeValue("condition"));
		for (Element itemElement : element.getChildren())
		{
			CreateItemsItem item = new CreateItemsItem();
			item.Load(context, itemElement);
			this._list.add(item);
		}
	}

	public final void Save(IServiceProvider context, Element element) throws Exception
	{
		if (!StringUtils2.isNullOrEmpty(this.getCondition()))
		{
			element.setAttribute("condition", this.getCondition());
		}

		for (CreateItemsItem item : this._list)
		{
			Element itemElement = new Element(item.getCategory(), element.getNamespace());
			element.addContent(itemElement);
			item.Save(context, itemElement);
		}
	}


}