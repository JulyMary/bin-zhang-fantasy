using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Configuration;

namespace Fantasy.ServiceModel
{
    public class ChannelSettings : SettingsBase
    {
        private static ChannelSettings _default = (ChannelSettings)Fantasy.Configuration.SettingStorage.Load(new ChannelSettings());

        public static ChannelSettings Default
        {
            get
            {
                return _default;
            }
        }

        
        public List<AddressSetting> Addresses
        {
            get
            {
                return (List<AddressSetting>)base.GetValue("Addresses") ?? new List<AddressSetting>();
            }
        }
    }
}
