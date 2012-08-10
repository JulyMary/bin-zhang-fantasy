package fantasy.jobs.scheduling;

import fantasy.*;
import fantasy.collections.*;
import fantasy.xserialization.*;
import org.jdom2.*;

@XSerializable(name = "inlineAction", namespaceUri=fantasy.jobs.Consts.ScheduleNamespaceURI)
public class InlineAction extends ScheduleAction implements IXSerializable
{

	/**
	 * 
	 */
	private static final long serialVersionUID = 8523615810849076491L;
	@Override
	public ActionType getType()
	{
		return ActionType.Inline;
	}

	private String privateXslt;
	public final String getXslt()
	{
		return privateXslt;
	}
	public final void setXslt(String value)
	{
		privateXslt = value;
	}


	public final void Load(IServiceProvider context, Element element) throws Exception
	{
		XHelper.getDefault().LoadByXAttributes(context, element, this);
		Element xsltElement = new Enumerable<Element>(element.getChildren()).firstOrDefault();
		this.setXslt(xsltElement != null ? xsltElement.toString() : null);
	}

	public final void Save(IServiceProvider context, Element element) throws Exception
	{
		XHelper.getDefault().SaveByXAttributes(context, element, this);
		if (!StringUtils2.isNullOrEmpty(getXslt()))
		{
			element.addContent(JDomUtils.parseElement(getXslt()));
		}
	}
}