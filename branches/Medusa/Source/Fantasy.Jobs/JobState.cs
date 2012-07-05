using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs
{
    public static class JobState
    {
        public const int Unterminated = 0;
        public const int Running = 1;
        public const int RequestStart = 2;
        public const int Suspended = 4;
        public const int Unstarted = 8;
        public const int UserPaused = 0x10;
        public const int Terminated = unchecked((int)0x80000000);
        public const int Succeed = unchecked((int)0x80000010);
        public const int Failed = unchecked((int)0x80000020);
        public const int Cancelled = unchecked((int)0x80000040);
        public const int All = Running | RequestStart | Suspended | Unstarted | UserPaused | Suspended | Failed | Cancelled;

        public static string ToString(int state)
        {
            switch (state)
            {
                case Unterminated: return "Unterminated";
                case Running: return "Running";
                case RequestStart: return "RequestStart";
                case Suspended: return "Suspended";
                case Unstarted: return "Unstarted";
                case UserPaused: return "UserPaused";
                case Terminated: return "Terminated";
                case Succeed: return "Succeed";
                case Failed: return "Failed";
                case Cancelled: return "Cancelled";
                case All: return "All";
                default :
                    throw new Exception("Absurd");
                   
            }
        }
    } 
}
