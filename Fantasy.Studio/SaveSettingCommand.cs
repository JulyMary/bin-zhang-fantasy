using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Configuration;
using Fantasy.ServiceModel;
using Fantasy.AddIns;

namespace Fantasy.Studio
{
    public class SaveSettingCommand : ICommand
    {
        #region ICommand Members

        public object Execute(object caller)
        {
            SettingStorage.BeginUpdate();
            foreach (SettingsBase settings in AddInTree.Tree.GetTreeNode("fantasy/applicationsettings").BuildChildItems(null))
            {
                settings.Save();
            }
            SettingStorage.EndUpdate();

            return null;
        }

        #endregion
    }
}
