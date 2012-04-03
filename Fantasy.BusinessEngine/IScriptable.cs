using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine
{
    public interface IScriptable
    {
        string Script { get; set; }

        ScriptOptions ScriptOptions { get; set; }

        string ExternalType { get; set; }
    }
}
