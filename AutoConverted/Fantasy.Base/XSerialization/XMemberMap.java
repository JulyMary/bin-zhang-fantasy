package Fantasy.XSerialization;

import Fantasy.*;

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
		private MemberInfo privateMember;
		public final MemberInfo getMember()
		{
			return privateMember;
		}
		public final void setMember(MemberInfo value)
		{
			privateMember = value;
		}
	}