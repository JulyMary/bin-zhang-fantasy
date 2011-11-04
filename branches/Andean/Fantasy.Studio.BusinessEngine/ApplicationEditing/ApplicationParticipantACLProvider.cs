using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Controls;
using Fantasy.BusinessEngine;
using Fantasy.Adaption;
using Fantasy.Collections;

namespace Fantasy.Studio.BusinessEngine.ApplicationEditing
{
    public class ApplicationParticipantACLProvider : ObjectWithSite, IChildItemsProvider
    {
        #region IChildItemsProvider Members

        public System.Collections.IEnumerable GetChildren(object parent)
        {
            IAdapterManager am = this.Site.GetRequiredService<IAdapterManager>();

            BusinessApplicationData application = am.GetAdapter<BusinessApplicationData>(parent);

            System.Collections.IEnumerable rs = new ObservableAdapterCollection<ParticipantACL>(application.Participants, (p => new ParticipantACL((BusinessApplicationParticipant)p))).ToSorted("Name");

            return rs;
            
        }

        #endregion
    }
}
