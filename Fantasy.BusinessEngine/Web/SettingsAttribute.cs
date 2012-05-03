using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Web
{
    public class SettingsAttribute : Attribute
    {
        public SettingsAttribute(Type settingsType)
        {
            this.SettingsType = settingsType;
        }

        public Type SettingsType { get; private set; }
    }
}
