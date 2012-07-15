package Fantasy.Jobs;

import Fantasy.XSerialization.*;
import Fantasy.ServiceModel.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Instruction, XSerializable("items", NamespaceUri = Consts.XNamespaceURI)]
public class CreateItems extends AbstractInstruction implements IConditionalObject, IXSerializable
{

	private java.util.ArrayList<CreateItemsItem> _list = new java.util.ArrayList<CreateItemsItem>();

	@Override
	public void Execute()
	{
		IStringParser parser = this.getSite().<IStringParser>GetRequiredService();
		IItemParser itemParser = this.getSite().<IItemParser>GetRequiredService();
		ILogger logger = this.getSite().<ILogger>GetService();
		IJob job = this.getSite().<IJob>GetRequiredService();
		IConditionService conditionSvc = this.getSite().<IConditionService>GetRequiredService();
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
					NameValueCollection meta = new NameValueCollection();
					for (String name : item.getMetaData().AllKeys)
					{
						meta.Add(name, parser.Parse(item.getMetaData()[name]));
					}
					if (group == null)
					{
						group = job.AddTaskItemGroup();
					}
					for (TaskItem parsedItem : parsedItems)
					{
						TaskItem newItem = group.AddNewItem(parsedItem.getName(), item.getCategory());
						parsedItem.CopyMetaDataTo(newItem);
						for (String name : meta)
						{
							newItem.setItem(name, meta[name]);
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

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IXSerializable Members

	public final void Load(IServiceProvider context, XElement element)
	{
		this.setCondition((String)element.Attribute("condition"));
		for (XElement itemElement : element.Elements())
		{
			CreateItemsItem item = new CreateItemsItem();
			item.Load(context, itemElement);
			this._list.add(item);
		}
	}

	public final void Save(IServiceProvider context, XElement element)
	{
		if (!DotNetToJavaStringHelper.isNullOrEmpty(this.getCondition()))
		{
			element.SetAttributeValue("condition", this.getCondition());
		}

		for (CreateItemsItem item : this._list)
		{
			XElement itemElement = new XElement(element.getName().Namespace + item.getCategory());
			element.Add(itemElement);
			item.Save(context, itemElement);
		}
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}