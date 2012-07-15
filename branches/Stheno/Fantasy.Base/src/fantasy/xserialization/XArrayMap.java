package fantasy.xserialization;



	public class XArrayMap extends XMemberMap
	{
		private XArray privateArray;
		public final XArray getArray()
		{
			return privateArray;
		}
		public final void setArray(XArray value)
		{
			privateArray = value;
		}

		
	}