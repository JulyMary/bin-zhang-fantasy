package fantasy.jobs.scheduling;

import fantasy.xserialization.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[DataContract, KnownType(typeof(TimeTrigger)), KnownType(typeof(DailyTrigger)), KnownType(typeof(WeeklyTrigger)), KnownType(typeof(MonthlyTrigger)), KnownType(typeof(MonthlyDOWTrigger)), KnownType(typeof(TemplateAction)), KnownType(typeof(InlineAction)), KnownType(typeof(CustomAction)), XSerializable("schedule", NamespaceUri = Consts.ScheduleNamespaceURI)]
public class ScheduleItem extends IXSerializable
{
	public ScheduleItem()
	{

	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XAttribute("name")]
	private String privateName;
	public final String getName()
	{
		return privateName;
	}
	public final void setName(String value)
	{
		privateName = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XAttribute("author")]
	private String privateAuthor;
	public final String getAuthor()
	{
		return privateAuthor;
	}
	public final void setAuthor(String value)
	{
		privateAuthor = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XAttribute("description")]
	private String privateDescription;
	public final String getDescription()
	{
		return privateDescription;
	}
	public final void setDescription(String value)
	{
		privateDescription = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XAttribute("enabled")]
	private boolean privateEnabled;
	public final boolean getEnabled()
	{
		return privateEnabled;
	}
	public final void setEnabled(boolean value)
	{
		privateEnabled = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember]
	private Trigger privateTrigger;
	public final Trigger getTrigger()
	{
		return privateTrigger;
	}
	public final void setTrigger(Trigger value)
	{
		privateTrigger = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember]
	private Action privateAction;
	public final Action getAction()
	{
		return privateAction;
	}
	public final void setAction(Action value)
	{
		privateAction = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XAttribute("priority")]
	private int privatePriority;
	public final int getPriority()
	{
		return privatePriority;
	}
	public final void setPriority(int value)
	{
		privatePriority = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XAttribute("restartOnFailure")]
	private Restart privateRestartOnFailure;
	public final Restart getRestartOnFailure()
	{
		return privateRestartOnFailure;
	}
	public final void setRestartOnFailure(Restart value)
	{
		privateRestartOnFailure = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XArray(Name = "requiredUncPath"), XArrayItem(Name = "unc", java.lang.Class = typeof(UncPath))]
	private UncPath[] privateRequiredUncPths;
	public final UncPath[] getRequiredUncPths()
	{
		return privateRequiredUncPths;
	}
	public final void setRequiredUncPths(UncPath[] value)
	{
		privateRequiredUncPths = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XAttribute("runOnlyIfIdle")]
	private boolean privateRunOnlyIfIdle;
	public final boolean getRunOnlyIfIdle()
	{
		return privateRunOnlyIfIdle;
	}
	public final void setRunOnlyIfIdle(boolean value)
	{
		privateRunOnlyIfIdle = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XAttribute("startWhenAvailable")]
	private boolean privateStartWhenAvailable;
	public final boolean getStartWhenAvailable()
	{
		return privateStartWhenAvailable;
	}
	public final void setStartWhenAvailable(boolean value)
	{
		privateStartWhenAvailable = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember]
	private String privateCustomParams;
	public final String getCustomParams()
	{
		return privateCustomParams;
	}
	public final void setCustomParams(String value)
	{
		privateCustomParams = value;
	}


//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XAttribute("multipleInstances")]
	private InstancesPolicy privateMultipleInstance = InstancesPolicy.forValue(0);
	public final InstancesPolicy getMultipleInstance()
	{
		return privateMultipleInstance;
	}
	public final void setMultipleInstance(InstancesPolicy value)
	{
		privateMultipleInstance = value;
	}


//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XAttribute("expired")]
	private boolean privateExpired;
	public final boolean getExpired()
	{
		return privateExpired;
	}
	public final void setExpired(boolean value)
	{
		privateExpired = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XAttribute("deleteAfterExpired")]
	private boolean privateDeleteAfterExpired;
	public final boolean getDeleteAfterExpired()
	{
		return privateDeleteAfterExpired;
	}
	public final void setDeleteAfterExpired(boolean value)
	{
		privateDeleteAfterExpired = value;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IXSerializable Members

	private void Load(IServiceProvider context, XElement element)
	{
		XHelper.Default.LoadByXAttributes(context, element, this);

		XmlNamespaceManager nm = new XmlNamespaceManager(new NameTable());
		nm.AddNamespace("s", Consts.ScheduleNamespaceURI);

		XElement customParams = element.XPathSelectElement("s:custom", nm);
		if (customParams != null)
		{
			this.setCustomParams(customParams.toString());
		}


		XElement triggerElement = element.XPathSelectElement("s:trigger", nm);
		if (triggerElement != null)
		{
			java.lang.Class type;
			TriggerType triggerType = TriggerType.valueOf((String)triggerElement.Attribute("type"));
			switch (triggerType)
			{
				case Time:
					type = TimeTrigger.class;
					break;
				case Daily:
					type = DailyTrigger.class;
					break;
				case Weekly:
					type = WeeklyTrigger.class;
					break;
				case Monthly:
					type = MonthlyTrigger.class;
					break;
				case MonthlyDayOfWeek:
					type = MonthlyDOWTrigger.class;
					break;
				default:
					throw new RuntimeException("Absurd!");
			}
			XSerializer ser = new XSerializer(type);
			ser.Context = context;
			this.setTrigger((Trigger)ser.Deserialize(triggerElement));

		}

		XElement actionElement = element.XPathSelectElement("s:action", nm);
		if (actionElement != null)
		{
			java.lang.Class type;
			ActionType actionType = ActionType.valueOf((String)actionElement.Attribute("type"));
			switch (actionType)
			{
				case Template:
					type = TemplateAction.class;
					break;
				case Inline:
					type = InlineAction.class;
					break;
				case Custom:
					type = CustomAction.class;
					break;
				default:
					throw new RuntimeException("absurd!");
			}
			XSerializer tempVar = new XSerializer(type);
			tempVar.Context = context;
			XSerializer ser = tempVar;
			this.setAction((Action)ser.Deserialize(actionElement));
		}
	}

	private void Save(IServiceProvider context, XElement element)
	{
		XHelper.Default.SaveByXAttributes(context, element, this);
		XNamespace ns = Consts.ScheduleNamespaceURI;

		if (this.getTrigger() != null)
		{
			XElement triggerElement = new XElement(ns + "trigger");
			element.Add(triggerElement);
			XSerializer tempVar = new XSerializer(this.getTrigger().getClass());
			tempVar.Context = context;
			XSerializer ser = tempVar;
			triggerElement.SetAttributeValue("type", this.getTrigger().getType().toString());
			ser.Serialize(triggerElement, this.getTrigger());
		}

		if (this.getAction() != null)
		{
			XElement actionElement = new XElement(ns + "action");
			element.Add(actionElement);
			XSerializer tempVar2 = new XSerializer(this.getAction().getClass());
			tempVar2.Context = context;
			XSerializer ser = tempVar2;
			actionElement.SetAttributeValue("type", this.getAction().getType().toString());
			ser.Serialize(actionElement, this.getAction());
		}

		if (this.getCustomParams() != null)
		{
			element.Add(XElement.Parse(this.getCustomParams()));
		}

	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}