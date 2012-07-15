package fantasy.xserialization;

import fantasy.ITypeConverter;

	public class XElementMap extends XMemberMap
	{

		private XElement privateElement;
		public final XElement getElement()
		{
			return privateElement;
		}
		public final void setElement(XElement value)
		{
			privateElement = value;
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