using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.ServiceModel
{
    public interface IService
    {
        void InitializeService();
        void UninitializeService();

        event EventHandler Initialize;
        event EventHandler Uninitialize;
    }


    public abstract class ServiceBase : MarshalByRefObject, IService, IObjectWithSite
    {
        #region IService Members

        public virtual void InitializeService()
        {
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
