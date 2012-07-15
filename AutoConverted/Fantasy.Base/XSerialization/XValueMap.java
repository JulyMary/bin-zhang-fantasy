package Fantasy.XSerialization;

import Fantasy.*;

	public class XValueMap extends XMemberMap
	{
		private XValueAttribute privateValue;
		public final XValueAttribute getValue()
		{
			return privateValue;
		}
		public final void setValue(XValueAttribute value)
		{
			privateValue = value;
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