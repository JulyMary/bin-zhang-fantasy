using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Fantasy.Studio.Properties;
using System.Collections;


namespace Fantasy.Studio.Controls
{
    class ToolBarModel : ButtonBarModel
    {
        public ToolBarModel(IEnumerable subItems, string id) : base(subItems)
        {
            
            Setting  = Fantasy.Studio.Properties.Settings.Default.ToolBarSettings.GetSetting(id);

        }

        public ToolBarSetting Setting { get; private set; }
        
        public string Text  { get; set; }
       
        
    }
}
