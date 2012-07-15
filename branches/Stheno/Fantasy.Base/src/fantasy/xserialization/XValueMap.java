package fantasy.xserialization;

import fantasy.ITypeConverter;


public class XValueMap extends XMemberMap
{
	private XValue privateValue;
	public final XValue getValue()
	{
		return privateValue;
	}
	public final void setValue(XValue value)
	{
		privateValue = value;
	}

	private ITypeConverter privateConverter;
	public final ITypeConverter getConverter()
	{
		return privateConverter;
	}
	public final void setConverter(ITypeConverter value)
	{
		privateConverter = value;
	}
}