package fantasy.jobs.scheduling;

import java.io.Serializable;

import fantasy.xserialization.*;

import fantasy.*;

import org.jdom2.*;


@XSerializable(name = "schedule", namespaceUri = fantasy.jobs.Consts.ScheduleNamespaceURI)
public class ScheduleItem implements IXSerializable, Serializable
{
	/**
	 * 
	 */
	private static final long serialVersionUID = -9064456907115551404L;
	public ScheduleItem()
	{

	}


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

	@XAttribute(name = "author")
	private String privateAuthor;
	public final String getAuthor()
	{
		return privateAuthor;
	}
	public final void setAuthor(String value)
	{
		privateAuthor = value;
	}

    @XAttribute(name = "description")
	private String privateDescription;
	public final String getDescription()
	{
		return privateDescription;
	}
	public final void setDescription(String value)
	{
		privateDescription = value;
	}

    @XAttribute(name = "enabled")
	private boolean privateEnabled;
	public final boolean getEnabled()
	{
		return privateEnabled;
	}
	public final void setEnabled(boolean value)
	{
		privateEnabled = value;
	}

	private Trigger privateTrigger;
	public final Trigger getTrigger()
	{
		return privateTrigger;
	}
	public final void setTrigger(Trigger value)
	{
		privateTrigger = value;
	}

	private ScheduleAction privateAction;
	public final ScheduleAction getAction()
	{
		return privateAction;
	}
	public final void setAction(ScheduleAction value)
	{
		privateAction = value;
	}
    
	@XAttribute(name = "priority")
	private int privatePriority;
	public final int getPriority()
	{
		return privatePriority;
	}
	public final void setPriority(int value)
	{
		privatePriority = value;
	}

    @XAttribute(name = "restartOnFailure")
	private Restart privateRestartOnFailure;
	public final Restart getRestartOnFailure()
	{
		return privateRestartOnFailure;
	}
	public final void setRestartOnFailure(Restart value)
	{
		privateRestartOnFailure = value;
	}



	@XAttribute(name = "runOnlyIfIdle")
	private boolean privateRunOnlyIfIdle;
	public final boolean getRunOnlyIfIdle()
	{
		return privateRunOnlyIfIdle;
	}
	public final void setRunOnlyIfIdle(boolean value)
	{
		privateRunOnlyIfIdle = value;
	}

	@XAttribute(name = "startWhenAvailable")
	private boolean privateStartWhenAvailable;
	public final boolean getStartWhenAvailable()
	{
		return privateStartWhenAvailable;
	}
	public final void setStartWhenAvailable(boolean value)
	{
		privateStartWhenAvailable = value;
	}

	@XElement(name = "custom")
	private Element privateCustomParams;
	public final Element getCustomParams()
	{
		return privateCustomParams;
	}
	public final void setCustomParams(Element value)
	{
		privateCustomParams = value;
	}


    @XAttribute(name = "multipleInstances")
	private InstancesPolicy privateMultipleInstance = InstancesPolicy.forValue(0);
	public final InstancesPolicy getMultipleInstance()
	{
		return privateMultipleInstance;
	}
	public final void setMultipleInstance(InstancesPolicy value)
	{
		privateMultipleInstance = value;
	}

    @XAttribute(name = "expired")
	private boolean privateExpired;
	public final boolean getExpired()
	{
		return privateExpired;
	}
	public final void setExpired(boolean value)
	{
		privateExpired = value;
	}

	@XAttribute(name = "deleteAfterExpired")
	private boolean privateDeleteAfterExpired;
	public final boolean getDeleteAfterExpired()
	{
		return privateDeleteAfterExpired;
	}
	public final void setDeleteAfterExpired(boolean value)
	{
		privateDeleteAfterExpired = value;
	}


	@SuppressWarnings("rawtypes")
	@Override
	public void Load(IServiceProvider context, Element element) throws Exception
	{
		XHelper.getDefault().LoadByXAttributes(context, element, this);
		
		
		Namespace ns = Namespace.getNamespace(fantasy.jobs.Consts.ScheduleNamespaceURI);

	


		Element triggerElement = element.getChild("trigger", ns);
		if (triggerElement != null)
		{
			java.lang.Class type;
			TriggerType triggerType = TriggerType.valueOf((String)triggerElement.getAttributeValue("type"));
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
			ser.setContext(context);
			this.setTrigger((Trigger)ser.deserialize(triggerElement));

		}

		Element actionElement = element.getChild("action", ns);
		if (actionElement != null)
		{
			java.lang.Class type;
			ActionType actionType = ActionType.valueOf((String)actionElement.getAttributeValue("type"));
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
			XSerializer ser = new XSerializer(type);
			ser.setContext(context);
		
			this.setAction((ScheduleAction)ser.deserialize(actionElement));
		}
	}

	@Override
	public void Save(IServiceProvider context, Element element) throws Exception
	{
		XHelper.getDefault().SaveByXAttributes(context, element, this);
		Namespace ns = Namespace.getNamespace(fantasy.jobs.Consts.ScheduleNamespaceURI);

		if (this.getTrigger() != null)
		{
			Element triggerElement = new Element("trigger", ns);
			element.addContent(triggerElement);
			XSerializer ser = new XSerializer(this.getTrigger().getClass());
			ser.setContext(context);
			
			triggerElement.setAttribute("type", this.getTrigger().getType().name());
			ser.serialize(triggerElement, this.getTrigger());
		}

		if (this.getAction() != null)
		{
			Element actionElement = new Element(ns + "action");
			element.addContent(actionElement);
			XSerializer ser = new XSerializer(this.getAction().getClass());
			ser.setContext(context);
			actionElement.setAttribute("type", this.getAction().getType().name());
			ser.serialize(actionElement, this.getAction());
		}
	}

}