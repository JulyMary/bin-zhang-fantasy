package fantasy.xserialization;

import java.lang.reflect.Field;


	public abstract class XMemberMap
	{
		public XMemberMap()
		{
			setOrder(Integer.MAX_VALUE);
		}
		private int privateOrder;
		public final int getOrder()
		{
			return privateOrder;
		}
		public final void setOrder(int value)
		{
			privateOrder = value;
		}
		private Field privateMember;
		public final Field getMember()
		{
			return privateMember;
		}
		public final void setMember(Field value)
		{
			privateMember = value;
			value.setAccessible(true);
		}
	}