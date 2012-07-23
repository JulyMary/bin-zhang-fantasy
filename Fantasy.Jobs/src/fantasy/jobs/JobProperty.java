package fantasy.jobs;

import java.io.*;

import org.jdom2.*;

import fantasy.*;
import fantasy.xserialization.*;

@XSerializable(name = "property", namespaceUri=Consts.XNamespaceURI)
public class JobProperty implements IXSerializable , Cloneable, IConditionalObject, Serializable
{
	/**
	 * 
	 */
	private static final long serialVersionUID = 2244505658037378778L;
	private String privateName;
	public final String getName()
	{
		return privateName;
	}
	public final void setName(String value)
	{
		privateName = value;
	}

	@XValue
	private String privateValue;
	public final String getValue()
	{
		return privateValue;
	}
	public final void setValue(String value)
	{
		privateValue = value;
	}

	public final Object clone()
	{
		JobProperty tempVar = new JobProperty();
		tempVar.setName(this.getName());
		tempVar.setValue(this.getValue());
		tempVar.setCondition(this.getCondition());
		return tempVar;
	}

	public final void Load(IServiceProvider context, Element element) throws Exception
	{
		this.setName(element.getName());
		XHelper.getDefault().LoadByXAttributes(context, element, this);
	}

	public final void Save(IServiceProvider context, Element element) throws Exception
	{
		XHelper.getDefault().SaveByXAttributes(context, element, this);
	}

	@XAttribute(name = "condition")
	private String privateCondition;
	public final String getCondition()
	{
		return privateCondition;
	}
	public final void setCondition(String value)
	{
		privateCondition = value;
	}

}