package fantasy.xserialization;

import fantasy.ITypeConverter;


	public class XAttributeMap extends XMemberMap
	{
		private XAttribute privateAttribute;
		public final XAttribute getAttribute()
		{
			return privateAttribute;
		}
		public final void setAttribute(XAttribute value)
		{
			privateAttribute = value;
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