using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.CodeEditing
{
    public abstract class EntityScript
    {
        public abstract string GetScript();
        protected virtual void OnScriptChanged(EventArgs e)
        {
            if (this.ScriptChanged != null)
            {
                this.ScriptChanged(this, e);
            }
        }

        public event EventHandler ScriptChanged;

        public abstract void SaveScript(string script);
    }
}
