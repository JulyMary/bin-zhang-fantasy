using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.ServiceModel
{
    public class TaskBarProgressMonitorService : TaskBarProgressMonitor, IService, IObjectWithSite
    {

        private IProgressMonitorContainer _container;
        public virtual void InitializeService()
        {
            if(this.Site != null)
            {
                _container = this.Site.GetService<IProgressMonitorContainer>();
                if (_container != null)
                {
                    _container.AddMoniter(this);
                }
            }

            

            if (this.Initialize != null)
            {
                this.Initialize(this, EventArgs.Empty);
            }
        }

        public virtual void UninitializeService()
        {
            if (this.Uninitialize != null)
            {
                this.Uninitialize(this, EventArgs.Empty);
            }
            if (_container != null)
            {
                this._container.RemoveMoniter(this);
            }
        }

        public event EventHandler Initialize;

        public event EventHandler Uninitialize;

    
        public IServiceProvider Site { get; set; }
      
    }
}
