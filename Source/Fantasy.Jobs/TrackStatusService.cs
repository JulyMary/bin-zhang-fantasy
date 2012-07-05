using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.Tracking;

namespace Fantasy.Jobs
{
    public class TrackStatusService : AbstractService, IStatusBarService
    {

        private ITrackProvider _track;

        public override void InitializeService()
        {
            _track = this.Site.GetService<ITrackProvider>();
            base.InitializeService();
        }

        #region IStatusBarService Members

        public void SetStatus(string status)
        {
            _track["status"] = status;
        }

        #endregion
    }
}
