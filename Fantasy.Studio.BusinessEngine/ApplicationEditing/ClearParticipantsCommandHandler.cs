using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Fantasy.Studio.BusinessEngine.ApplicationEditing
{
    public class ClearParticipantsCommandHandler : ObjectWithSite, ICommand
    {
        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            ParticipantPanelModel model = this.Site.GetRequiredService<ParticipantPanelModel>();
            return model.Items.Count > 0;
        }

        event EventHandler ICommand.CanExecuteChanged {add{} remove{}}

        public void Execute(object parameter)
        {
            ParticipantPanelModel model = this.Site.GetRequiredService<ParticipantPanelModel>();
            model.ClearRootNode();
        }

        #endregion
    }
}
