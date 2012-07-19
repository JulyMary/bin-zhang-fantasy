package fantasy.jobs;

public final class JobState
{
	public static final int Unterminated = 0;
	public static final int Running = 1;
	public static final int RequestStart = 2;
	public static final int Suspended = 4;
	public static final int Unstarted = 8;
	public static final int UserPaused = 0x10;
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to 'unchecked' in this context:
//ORIGINAL LINE: public const int Terminated = unchecked((int)0x80000000);
	public static final int Terminated = (int)0x80000000;
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to 'unchecked' in this context:
//ORIGINAL LINE: public const int Succeed = unchecked((int)0x80000010);
	public static final int Succeed = (int)0x80000010;
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to 'unchecked' in this context:
//ORIGINAL LINE: public const int Failed = unchecked((int)0x80000020);
	public static final int Failed = (int)0x80000020;
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to 'unchecked' in this context:
//ORIGINAL LINE: public const int Cancelled = unchecked((int)0x80000040);
	public static final int Cancelled = (int)0x80000040;
	public static final int All = Running | RequestStart | Suspended | Unstarted | UserPaused | Suspended | Failed | Cancelled;

	public static String ToString(int state)
	{
		switch (state)
		{
			case Unterminated:
				return "Unterminated";
			case Running:
				return "Running";
			case RequestStart:
				return "RequestStart";
			case Suspended:
				return "Suspended";
			case Unstarted:
				return "Unstarted";
			case UserPaused:
				return "UserPaused";
			case Terminated:
				return "Terminated";
			case Succeed:
				return "Succeed";
			case Failed:
				return "Failed";
			case Cancelled:
				return "Cancelled";
			case All:
				return "All";
			default :
				throw new RuntimeException("Absurd");

		}
	}
}