using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.Tracking;

namespace Fantasy.Jobs
{
    public class TrackProgressService : AbstractService,  IProgressMonitor
    {

        private ITrackProvider _track;

        public override void InitializeService()
        {
            _track = this.Site.GetService<ITrackProvider>();
            base.InitializeService();
        }

        #region IProgressMonitor Members

        public int Value
        {
            get
            {
                return _track.GetProperty("progress.value", 0); 
            }
            set
            {
                _track["progress.value"] = value; 
            }
        }

        public int Maximum
        {
            get
            {
                return _track.GetProperty("progress.maximum", 100);
            }
            set
            {
                _track["progress.maximum"] = value;
            }
        }

        public int Minimum
        {
            get
            {
                return _track.GetProperty("progress.minimum", 0);
            }
            set
            {
                _track["progress.minimum"] = value;
            }
        }

        #endregion

        #region IProgressMonitor Members


        public ProgressMonitorStyle Style
        {
            get
            {
                return _track.GetProperty("progress.style", ProgressMonitorStyle.Blocks);
            }
            set
            {
                _track["progress.style"] = value;
            }
        }

        #endregion
    }
}
