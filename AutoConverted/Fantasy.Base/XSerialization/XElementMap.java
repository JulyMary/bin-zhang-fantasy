package Fantasy.XSerialization;

import Fantasy.*;

	public class XElementMap extends XMemberMap
	{

		private XElementAttribute privateElement;
		public final XElementAttribute getElement()
		{
			return privateElement;
		}
		public final void setElement(XElementAttribute value)
		{
			privateElement = value;
		}
		private TypeConverter privateConverter;
		public final TypeConverter getConverter()
		{
			return privateConverter;
		}
		public final void setConverter(TypeConverter value)
		{
			privateConverter = value;
		}

	}