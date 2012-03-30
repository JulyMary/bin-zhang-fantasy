using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Descriptor;
using Fantasy.Studio.BusinessEngine.Properties;

namespace Fantasy.Studio.BusinessEngine.CodeEditing
{
    public class ScriptBuildActionEditor : ListBoxDropDownTypeEditor
    {
        protected override object[] Items
        {
            get { return Settings.Default.ScriptBuildActions.Cast<string>().ToArray(); }
        }
    }
}
