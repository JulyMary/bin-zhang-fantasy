using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Fantasy.Studio.Services;

namespace Fantasy.Studio.BusinessEngine.ApplicationEditing
{
    public class OpenParticipantACLEditorCommand : ObjectWithSite, ICommand
    {
        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            ParticipantNode node = (ParticipantNode)parameter;
            node.IsCheckedChanged += new EventHandler(IsCheckedChanged);
            return node.IsChecked;
        }

        void IsCheckedChanged(object sender, EventArgs e)
        {
            if (this.CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, e);
            }
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            ParticipantNode node = (ParticipantNode)parameter;
            ParticipantACL acl = new ParticipantACL(node.Participant);

            IEditingService documentService = this.Site.GetRequiredService<IEditingService>();

            IViewContent content = documentService.OpenView(acl);
            if (content != null)
            {
                content.WorkbenchWindow.Select();
            }  

        }

        #endregion
    }
}
