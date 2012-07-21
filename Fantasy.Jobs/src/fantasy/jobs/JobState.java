package fantasy.jobs;

public final class JobState
{
	public static final int Unterminated = 0;
	public static final int Running = 1;
	public static final int RequestStart = 2;
	public static final int Suspended = 4;
	public static final int Unstarted = 8;
	public static final int UserPaused = 0x10;

	public static final int Terminated = 1 << 31;

	public static final int Succeed = Terminated  | 0x10;
	public static final int Failed = Terminated | 0x20;
	public static final int Cancelled = Terminated | (int)0x40;
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