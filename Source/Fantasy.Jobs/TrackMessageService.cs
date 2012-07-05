using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.Tracking;

namespace Fantasy.Jobs
{
    public class TrackMessageService : AbstractMessageService, IService, IObjectWithSite 
    {
        public override System.Windows.Forms.DialogResult Show(System.Windows.Forms.DialogResult defaultResult, System.Windows.Forms.IWin32Window owner, string text, string caption, System.Windows.Forms.MessageBoxButtons buttons, System.Windows.Forms.MessageBoxIcon icon, System.Windows.Forms.MessageBoxDefaultButton defaultButton, System.Windows.Forms.MessageBoxOptions options)
        {
            if (_track != null)
            {
                _track["message"] = text;

            }

            return defaultResult; 
        }

        public override void WriteLine(string text)
        {
            _track["message"] = text;
        }

        #region IService Members

        private ITrackProvider _track;

        public void InitializeService()
        {
            _track = this.Site.GetService<ITrackProvider>();
            if (Initialize != null)
            {
                Initialize(this, EventArgs.Empty);
            }
        }

        public void UninitializeService()
        {
            if (Uninitialize != null)
            {
                Uninitialize(this, EventArgs.Empty);
            }
        }

        public event EventHandler Initialize;

        public event EventHandler Uninitialize;

        #endregion

        #region IObjectWithSite Members

        public IServiceProvider Site
        {
            get;
            set;
        }

        #endregion
    }
}
