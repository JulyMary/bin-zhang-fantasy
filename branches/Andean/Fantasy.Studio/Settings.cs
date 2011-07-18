using Fantasy.Studio.Codons;
using System.Configuration;
using System.Diagnostics;
namespace Fantasy.Studio.Properties {
    
    
    // This class allows you to handle specific events on the settings class:
    //  The SettingChanging event is raised before a setting's value is changed.
    //  The PropertyChanged event is raised after a setting's value is changed.
    //  The SettingsLoaded event is raised after the setting values are loaded.
    //  The SettingsSaving event is raised before the setting values are saved.
    sealed partial class Settings {
        

        [DebuggerNonUserCodeAttribute()]
        [DefaultSettingValue("<ToolBarSettings/>")]
        public ToolBarSettings ToolBarSettings
        {
            get
            {
                ToolBarSettings rs = ((global::Fantasy.Studio.Codons.ToolBarSettings)(this.GetValue("ToolBarSettings")));
                return rs;
            }
           
        }

        [DebuggerNonUserCodeAttribute()]
        [DefaultSettingValue("<WindowStateSetting/>")]
        public WindowStateSetting WorkbenchState
        {
            get
            {
                WindowStateSetting rs = (WindowStateSetting)(this.GetValue("WorkbenchState"));
                return rs;
            }
        }
    }
}
