using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.BusinessEngine.Services;
using Fantasy.BusinessEngine;
using Fantasy.Studio.Services;

namespace Fantasy.Studio.BusinessEngine.ApplicationEditing
{
    public class DeleteApplicationCommand : ObjectWithSite, ICommand
    {
        #region ICommand Members

        public object Execute(object args)
        {
            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            BusinessApplicationData app = (BusinessApplicationData)args;
            app.Package.Applications.Remove(app);
            app.Package = null;
            es.Delete(app);

            IEditingService editingService = this.Site.GetRequiredService<IEditingService>();
            foreach (BusinessApplicationParticipant participant in app.Participants)
            {
                editingService.CloseView(new ParticipantACL(participant), true);
            }
            editingService.CloseView(app, true);

            return null;
        }

        #endregion
    }
}
