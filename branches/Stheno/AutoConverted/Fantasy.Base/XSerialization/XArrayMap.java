package Fantasy.XSerialization;

import Fantasy.*;

	public class XArrayMap extends XMemberMap
	{
		private XArrayAttribute privateArray;
		public final XArrayAttribute getArray()
		{
			return privateArray;
		}
		public final void setArray(XArrayAttribute value)
		{
			privateArray = value;
		}

		private java.util.List<XArrayItemAttribute> _items = new java.util.ArrayList<XArrayItemAttribute>();

		public final java.util.List<XArrayItemAttribute> getItems()
		{
			return _items;
		}
	}