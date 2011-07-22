using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Studio
{

    public enum CommandParameterSource
    {
        Parameter,
        Owner
    }

    public class CommandParameter
    {
        public CommandParameterSource Source { get; set; }
    }
}
