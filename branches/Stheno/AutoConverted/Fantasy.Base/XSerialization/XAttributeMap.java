package Fantasy.XSerialization;

import Fantasy.*;

	public class XAttributeMap extends XMemberMap
	{
		private XAttributeAttribute privateAttribute;
		public final XAttributeAttribute getAttribute()
		{
			return privateAttribute;
		}
		public final void setAttribute(XAttributeAttribute value)
		{
			privateAttribute = value;
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